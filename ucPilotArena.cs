using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DonkeyUi
{
    public partial class ucPilotArena : UserControl
    {
        // ════════════════════════════════════════════════════════════
        // 파일럿 세트 관리
        // ════════════════════════════════════════════════════════════
        private readonly List<ComboBox> _pilotCombos = new();
        private readonly List<ComboBox> _modelCombos = new();
        // per-slot data model so each slot (image + two combos + labels) appears/disappears as a unit
        private class PilotSlot { public string PilotName; public string ModelType; }
        private readonly List<PilotSlot> _pilotSlots = new();
        private readonly List<PictureBox> _displayPictureBoxes = new List<PictureBox>();
        private readonly List<Label> _aiAngleLabels = new();
        private readonly List<Label> _angleErrorLabels = new();
        private readonly List<Label> _aiThrottleLabels = new();
        private readonly List<Label> _throttleErrorLabels = new();
        private readonly List<Label> _avgErrorLabels = new();

        // ════════════════════════════════════════════════════════════
        // 이미지/타임라인 관리
        // ════════════════════════════════════════════════════════════
        private List<string> _imageFiles = new List<string>();
        private int _currentIndex = 0;

        // ════════════════════════════════════════════════════════════
        // AI 예측 관련
        // ════════════════════════════════════════════════════════════
        private string _modelPath = "/home/xytron/mycar/models/test1.h5";
        private string _modelType = "linear";
        private const string PythonPath = "/home/xytron/miniconda3/envs/e2e_env/bin/python3";
        private const string WslScriptPath = "/tmp/predict_pilot.py";
        private const string WinScriptFile = @"\\wsl.localhost\Ubuntu-22.04\tmp\predict_pilot.py";

        // ✅ 상시 실행 Python 프로세스
        private Process _pythonProc = null;
        private bool _pythonReady = false;
        private readonly SemaphoreSlim _pythonSemaphore = new SemaphoreSlim(1, 1);

        private double? _humanAngle = null;
        private double? _humanThrottle = null;
        private double? _aiAngle = null;
        private double? _aiThrottle = null;
        private string _lastImagePath = null;

        private Dictionary<string, (double angle, double throttle)> _predictionCache
            = new Dictionary<string, (double, double)>();

        private double _humanMaxThrottle = 0.0;
        private double _humanMinThrottle = 999.0;
        private int[] _throttleHistogram = new int[101];
        private int _totalThrottleCount = 0;

        // ✅ 슬라이더 디바운스 타이머
        private System.Windows.Forms.Timer _sliderDebounce = new System.Windows.Forms.Timer();

        private static readonly Color HumanColor = Color.FromArgb(255, 255, 87, 34);
        private static readonly Color AiColor = Color.FromArgb(255, 0, 176, 255);

        // ✅ 서버 모드 추가된 ScriptContent
        private const string ScriptContent =
"import sys, os, logging\n" +
"os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'\n" +
"logging.disable(logging.CRITICAL)\n" +
"_real_stdout_fd = os.dup(1)\n" +
"_real_stdout = os.fdopen(_real_stdout_fd, 'w', buffering=1)\n" +
"_devnull = os.open(os.devnull, os.O_WRONLY)\n" +
"os.dup2(_devnull, 1)\n" +
"import donkeycar as dk\n" +
"import logging as _logging\n" +
"for name in list(_logging.root.manager.loggerDict.keys()):\n" +
"    _logging.getLogger(name).setLevel(_logging.CRITICAL)\n" +
"_logging.root.setLevel(_logging.CRITICAL)\n" +
"os.dup2(_real_stdout_fd, 1)\n" +
"os.close(_devnull)\n" +
"def output(text):\n" +
"    _real_stdout.write(text + '\\n')\n" +
"    _real_stdout.flush()\n" +
"def load_model(model_path, model_type):\n" +
"    os.dup2(os.open(os.devnull, os.O_WRONLY), 1)\n" +
"    try:\n" +
"        cfg = dk.load_config(myconfig='myconfig.py')\n" +
"        kl = dk.utils.get_model_by_type(model_type, cfg)\n" +
"        kl.load(model_path)\n" +
"    finally:\n" +
"        os.dup2(_real_stdout_fd, 1)\n" +
"    return kl, cfg\n" +
"def predict_single(kl, cfg, image_path):\n" +
"    import numpy as np\n" +
"    from PIL import Image\n" +
"    img = Image.open(image_path).convert('RGB')\n" +
"    img = img.resize((cfg.IMAGE_W, cfg.IMAGE_H))\n" +
"    arr = np.array(img, dtype=np.float32) / 255.0\n" +
"    result = kl.run(arr)\n" +
"    if isinstance(result, (list, tuple)):\n" +
"        angle = float(result[0])\n" +
"        throttle = float(result[1]) if len(result) > 1 else 0.0\n" +
"    else:\n" +
"        angle = float(result)\n" +
"        throttle = 0.0\n" +
"    return max(-1.0, min(1.0, angle)), max(-1.0, min(1.0, throttle))\n" +
"if __name__ == '__main__':\n" +
"    if len(sys.argv) < 3:\n" +
"        sys.exit(0)\n" +
"    model_path = sys.argv[1]\n" +
"    model_type = sys.argv[2] if len(sys.argv) > 2 else 'linear'\n" +
"    is_server = '--server' in sys.argv\n" +
"    try:\n" +
"        kl, cfg = load_model(model_path, model_type)\n" +
// ✅ 서버 모드: 모델 한 번 로딩 후 stdin으로 경로 받아서 즉시 예측
"        if is_server:\n" +
"            output('READY')\n" +
"            for line in sys.stdin:\n" +
"                line = line.strip()\n" +
"                if not line: continue\n" +
"                try:\n" +
"                    angle, throttle = predict_single(kl, cfg, line)\n" +
"                    output(f'RESULT:{angle:.6f}:{throttle:.6f}')\n" +
"                except:\n" +
"                    output('RESULT:0.0:0.0')\n" +
"    except:\n" +
"        output('ERROR')\n";

        // ════════════════════════════════════════════════════════════
        // 생성자
        // ════════════════════════════════════════════════════════════
        public ucPilotArena()
        {
            InitializeComponent();

            try { File.WriteAllText(WinScriptFile, ScriptContent, Encoding.UTF8); } catch { }

            // ✅ 슬라이더 디바운스 설정 (200ms)
            _sliderDebounce.Interval = 200;
            _sliderDebounce.Tick += (s, e) => {
                _sliderDebounce.Stop();
                if (_imageFiles.Count > 0) ShowFrame(_currentIndex);
                else RefreshAllSlots();
            };

            btnAddLeftPic.Click += BtnAddLeftPic_Click;
            btnRemoveLeftPic.Click += BtnRemoveLeftPic_Click;

            // number of columns changed should re-layout
            if (cmbNumColumns != null) cmbNumColumns.SelectedIndexChanged += (s, e) => UpdateDisplay();

            trkTimeline.Scroll += trkTimeline_Scroll;
            trkBrightness.Scroll += trkBrightness_Scroll;
            trkBlur.Scroll += trkBlur_Scroll;

            pnlImageArea.Resize += (s, e) => UpdateDisplay();
            ucTubManager.OnTubDataChanged += OnTubDataChanged;

            // start with one pilot slot
            _pilotCombos.Clear();
            _modelCombos.Clear();
            _pilotSlots.Clear();
            AddPilotSet();

            if (!string.IsNullOrEmpty(ucTubManager.CurrentTubPath))
                LoadImages(ucTubManager.CurrentTubPath);
        }

        // ════════════════════════════════════════════════════════════
        // ✅ Python 서버 프로세스 관리
        // ════════════════════════════════════════════════════════════

        private async Task EnsurePythonServer()
        {
            if (_pythonReady && _pythonProc != null && !_pythonProc.HasExited) return;

            _pythonReady = false;
            _pythonProc?.Kill();
            _pythonProc?.Dispose();

            string bashCmd = $"cd /home/xytron/mycar && {PythonPath} {WslScriptPath} {_modelPath} {_modelType} --server";
            var psi = new ProcessStartInfo
            {
                FileName = "wsl",
                Arguments = $"-e bash -c \"{bashCmd}\"",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _pythonProc = Process.Start(psi);

            // READY 신호 대기
            string line = await _pythonProc.StandardOutput.ReadLineAsync();
            if (line == "READY") _pythonReady = true;
        }

        private async Task<(double angle, double throttle)?> RequestPrediction(string wslImagePath)
        {
            await _pythonSemaphore.WaitAsync();
            try
            {
                await EnsurePythonServer();
                if (!_pythonReady) return null;

                await _pythonProc.StandardInput.WriteLineAsync(wslImagePath);
                string result = await _pythonProc.StandardOutput.ReadLineAsync();

                if (result == null || !result.StartsWith("RESULT:")) return null;

                var parts = result.Substring(7).Split(':');
                if (parts.Length < 2) return null;

                if (!double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double ang)) return null;
                double thr = 0.0;
                if (parts.Length >= 2)
                    double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out thr);

                return (ang, thr);
            }
            catch { return null; }
            finally { _pythonSemaphore.Release(); }
        }

        // ════════════════════════════════════════════════════════════
        // 파일럿 추가 / 제거
        // ════════════════════════════════════════════════════════════

        private void BtnAddLeftPic_Click(object? sender, EventArgs e) => AddPilotSet();

        private void BtnRemoveLeftPic_Click(object? sender, EventArgs e)
        {
            if (_pilotSlots.Count <= 1) return;
            _pilotSlots.RemoveAt(_pilotSlots.Count - 1);
            UpdateDisplay();
        }

        private void AddPilotSet()
        {
            if (_pilotSlots.Count >= 4) return;

            string defaultPilot = "파일럿 " + (_pilotSlots.Count + 1);
            string defaultModel = !string.IsNullOrEmpty(_modelType) ? _modelType : "linear";

            _pilotSlots.Add(new PilotSlot { PilotName = defaultPilot, ModelType = defaultModel });
            UpdateDisplay();
        }

        // ════════════════════════════════════════════════════════════
        // UpdateDisplay
        // ════════════════════════════════════════════════════════════

        private void UpdateDisplay()
        {
            if (pnlImageArea == null) return;
            pnlImageArea.Controls.Clear();
            _displayPictureBoxes.Clear();
            _aiAngleLabels.Clear();
            _angleErrorLabels.Clear();
            _aiThrottleLabels.Clear();
            _throttleErrorLabels.Clear();
            _avgErrorLabels.Clear();

            int count = _pilotSlots.Count;
            if (count == 0) return;

            int columns = 1;
            if (cmbNumColumns != null && int.TryParse(cmbNumColumns.SelectedItem?.ToString(), out int parsed))
                columns = Math.Clamp(parsed, 1, 4);

            int rows = (int)Math.Ceiling(count / (double)columns);

            var table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;
            table.ColumnCount = columns;
            table.RowCount = rows * 2; // each row has image row and info row
            table.RowStyles.Clear();
            table.ColumnStyles.Clear();

            // column styles
            for (int c = 0; c < columns; c++) table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));

            // row styles: image row proportional, info row fixed height so controls (combos + labels) are visible
            for (int r = 0; r < rows; r++)
            {
                // image row: take remaining proportional space
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));
                // info row: fixed height to fit two combos and labels
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            }

            for (int i = 0; i < count; i++)
            {
                int col = i % columns;
                int visualRow = (i / columns) * 2;

                var pb = new PictureBox();
                pb.Dock = DockStyle.Fill;
                pb.MaximumSize = new Size(0, 1000);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.BackColor = Color.Black;
                _displayPictureBoxes.Add(pb);

                // create combo panel and info labels panel as a single set per slot
                var comboPanel = new FlowLayoutPanel();
                comboPanel.Dock = DockStyle.Fill;
                comboPanel.FlowDirection = FlowDirection.LeftToRight;
                comboPanel.AutoSize = true;

                // create pilot selection combo for this slot
                var pilotCombo = new ComboBox();
                pilotCombo.DropDownStyle = ComboBoxStyle.DropDownList;
                pilotCombo.Width = 180;
                // use the slot's current pilot name as the only/default item
                pilotCombo.Items.Add(_pilotSlots[i].PilotName ?? ("파일럿 " + (i + 1)));
                pilotCombo.SelectedIndex = 0;

                // create model selection combo for this slot
                var modelCombo = new ComboBox();
                modelCombo.DropDownStyle = ComboBoxStyle.DropDownList;
                modelCombo.Width = 120;
                modelCombo.Items.AddRange(new string[] { "linear", "categorical", "behavior" });
                int sel = Array.FindIndex(modelCombo.Items.Cast<string>().ToArray(), s => string.Equals(s, _pilotSlots[i].ModelType, StringComparison.OrdinalIgnoreCase));
                modelCombo.SelectedIndex = sel >= 0 ? sel : 0;

                comboPanel.Controls.Add(pilotCombo);
                comboPanel.Controls.Add(modelCombo);

                var infoPanel = new FlowLayoutPanel();
                infoPanel.Dock = DockStyle.Fill;
                infoPanel.FlowDirection = FlowDirection.TopDown;
                infoPanel.Padding = new Padding(4);
                infoPanel.AutoSize = false;

                var dataFlow = new FlowLayoutPanel();
                dataFlow.FlowDirection = FlowDirection.LeftToRight;
                dataFlow.AutoSize = true;

                var aiAngleLbl = new Label(); aiAngleLbl.ForeColor = Color.White; aiAngleLbl.AutoSize = true; aiAngleLbl.Text = "AI 각도 : N/A";
                var angleErrLbl = new Label(); angleErrLbl.ForeColor = Color.LightGreen; angleErrLbl.AutoSize = true; angleErrLbl.Text = "오차 : N/A";
                var aiThrLbl = new Label(); aiThrLbl.ForeColor = Color.White; aiThrLbl.AutoSize = true; aiThrLbl.Text = "AI 속도 : N/A";
                var thrErrLbl = new Label(); thrErrLbl.ForeColor = Color.LightGreen; thrErrLbl.AutoSize = true; thrErrLbl.Text = "오차 : N/A";
                var avgLbl = new Label(); avgLbl.ForeColor = Color.White; avgLbl.AutoSize = true; avgLbl.Text = "평균 오차율 : N/A";

                dataFlow.Controls.Add(aiAngleLbl);
                dataFlow.Controls.Add(angleErrLbl);
                dataFlow.Controls.Add(new Label() { AutoSize = true, Text = "   " });
                dataFlow.Controls.Add(aiThrLbl);
                dataFlow.Controls.Add(thrErrLbl);

                infoPanel.Controls.Add(comboPanel);
                infoPanel.Controls.Add(dataFlow);
                infoPanel.Controls.Add(avgLbl);

                // keep references for updates
                _aiAngleLabels.Add(aiAngleLbl);
                _angleErrorLabels.Add(angleErrLbl);
                _aiThrottleLabels.Add(aiThrLbl);
                _throttleErrorLabels.Add(thrErrLbl);
                _avgErrorLabels.Add(avgLbl);

                table.Controls.Add(pb, col, visualRow);
                table.Controls.Add(infoPanel, col, visualRow + 1);
            }

            pnlImageArea.Controls.Add(table);
            btnAddLeftPic.Enabled = _pilotSlots.Count < 4;
            btnRemoveLeftPic.Enabled = _pilotSlots.Count > 1;
            RefreshAllSlots();
        }

        // ════════════════════════════════════════════════════════════
        // RefreshAllSlots
        // ════════════════════════════════════════════════════════════

        private void RefreshAllSlots()
        {
            if (_displayPictureBoxes.Count == 0) return;
            if (string.IsNullOrEmpty(_lastImagePath) || !File.Exists(_lastImagePath)) return;

            for (int i = 0; i < _displayPictureBoxes.Count; i++)
            {
                try
                {
                    Bitmap bmp = BuildOverlayBitmap(_lastImagePath);
                    _displayPictureBoxes[i].Image?.Dispose();
                    _displayPictureBoxes[i].Image = bmp;

                    // update data labels per slot using global human/ai values
                    string aiAngleText = _aiAngle.HasValue ? _aiAngle.Value.ToString("+0.000;-0.000;0.000") : "N/A";
                    string aiThrText = _aiThrottle.HasValue ? _aiThrottle.Value.ToString("+0.000;-0.000;0.000") : "N/A";
                    double? angleErr = null, thrErr = null;
                    if (_aiAngle.HasValue && _humanAngle.HasValue) angleErr = _aiAngle.Value - _humanAngle.Value;
                    if (_aiThrottle.HasValue && _humanThrottle.HasValue) thrErr = _aiThrottle.Value - _humanThrottle.Value;

                    if (i < _aiAngleLabels.Count) _aiAngleLabels[i].Text = "AI 각도 : " + aiAngleText;
                    if (i < _angleErrorLabels.Count) _angleErrorLabels[i].Text = "오차 : " + (angleErr.HasValue ? angleErr.Value.ToString("+0.000;-0.000;0.000") : "N/A");
                    if (i < _aiThrottleLabels.Count) _aiThrottleLabels[i].Text = "AI 속도 : " + aiThrText;
                    if (i < _throttleErrorLabels.Count) _throttleErrorLabels[i].Text = "오차 : " + (thrErr.HasValue ? thrErr.Value.ToString("+0.000;-0.000;0.000") : "N/A");
                    if (i < _avgErrorLabels.Count)
                    {
                        if (angleErr.HasValue && thrErr.HasValue)
                        {
                            double avg = (Math.Abs(angleErr.Value) + Math.Abs(thrErr.Value)) / 2.0 * 100.0;
                            _avgErrorLabels[i].Text = "평균 오차율 : " + avg.ToString("0.0") + "%";
                        }
                        else _avgErrorLabels[i].Text = "평균 오차율 : N/A";
                    }
                }
                catch { }
            }
        }

        // ════════════════════════════════════════════════════════════
        // BuildOverlayBitmap
        // ════════════════════════════════════════════════════════════

        private Bitmap BuildOverlayBitmap(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath)) return null;
            try
            {
                Bitmap bmp;
                using (var fs = File.OpenRead(imagePath))
                using (var rawImg = Image.FromStream(fs))
                using (var bright = MakeBrightness(rawImg, trkBrightness.Value))
                {
                    var processed = MakeBlur(bright, trkBlur.Value);
                    bmp = new Bitmap(processed);
                    processed.Dispose();
                }

                using var g = Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                int w = bmp.Width, h = bmp.Height;
                int centerX = w / 2, startY = h;
                int humanStartX = centerX - 2, aiStartX = centerX + 2;

                if (_humanAngle.HasValue)
                {
                    int lineLen = CalcLineLen(_humanThrottle, h);
                    double rad = _humanAngle.Value * 45.0 * Math.PI / 180.0;
                    DrawStem(g, HumanColor, humanStartX, startY, rad, lineLen);
                }
                if (_aiAngle.HasValue)
                {
                    int lineLen = CalcLineLen(_aiThrottle, h);
                    double rad = _aiAngle.Value * 45.0 * Math.PI / 180.0;
                    DrawStem(g, AiColor, aiStartX, startY, rad, lineLen);
                }
                return bmp;
            }
            catch { return null; }
        }

        // ════════════════════════════════════════════════════════════
        // 이미지 로드 / 타임라인 / 밝기·블러
        // ════════════════════════════════════════════════════════════

        private void LoadImages(string folder)
        {
            if (!Directory.Exists(folder)) return;
            _imageFiles = Directory.GetFiles(folder)
                .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                         || f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                         || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                .OrderBy(f => f).ToList();

            if (_imageFiles.Count == 0) return;
            trkTimeline.Minimum = 0;
            trkTimeline.Maximum = _imageFiles.Count - 1;
            trkTimeline.Value = 0;
            _currentIndex = 0;
            ShowFrame(0);
        }

        private void ShowFrame(int index)
        {
            if (index < 0 || index >= _imageFiles.Count) return;
            try
            {
                using var fs = File.OpenRead(_imageFiles[index]);
                using var rawImg = Image.FromStream(fs);
                using var bright = MakeBrightness(rawImg, trkBrightness.Value);
                var processed = MakeBlur(bright, trkBlur.Value);

                if (_displayPictureBoxes.Count > 0)
                {
                    _displayPictureBoxes[0].Image?.Dispose();
                    _displayPictureBoxes[0].Image = new Bitmap(processed);
                }
                processed.Dispose();
            }
            catch { }
        }

        private void trkTimeline_Scroll(object sender, EventArgs e)
        {
            _currentIndex = trkTimeline.Value;
            ShowFrame(_currentIndex);
        }

        // ✅ 슬라이더는 디바운스 — 멈추고 200ms 후에만 처리
        private void trkBrightness_Scroll(object sender, EventArgs e)
        {
            lblBrightnessValue.Text = "밝기 " + trkBrightness.Value;
            _sliderDebounce.Stop();
            _sliderDebounce.Start();
        }

        private void trkBlur_Scroll(object sender, EventArgs e)
        {
            lblBlurValue.Text = "흐림 " + trkBlur.Value;
            _sliderDebounce.Stop();
            _sliderDebounce.Start();
        }

        public void LoadUserTub(string folder) => LoadImages(folder);

        // ════════════════════════════════════════════════════════════
        // ✅ 빠른 이미지 처리 (ColorMatrix + Scale Blur)
        // ════════════════════════════════════════════════════════════

        private Bitmap MakeBrightness(Image image, int brightness)
        {
            // ✅ ColorMatrix 사용 — GetPixel 없음, 매우 빠름
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            float b = brightness / 100f;
            float[][] matrix = {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {b, b, b, 0, 1}
            };
            var cm = new ColorMatrix(matrix);
            var ia = new ImageAttributes();
            ia.SetColorMatrix(cm);
            using var g = Graphics.FromImage(bmp);
            g.DrawImage(image,
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                0, 0, image.Width, image.Height,
                GraphicsUnit.Pixel, ia);
            return bmp;
        }

        private Bitmap MakeBlur(Image image, int blurAmount)
        {
            // ✅ 스케일 다운/업 방식 — GetPixel 없음, 매우 빠름
            int radius = blurAmount / 20;
            if (radius < 1) return new Bitmap(image);

            int sw = Math.Max(1, image.Width / (radius + 1));
            int sh = Math.Max(1, image.Height / (radius + 1));

            using var small = new Bitmap(sw, sh);
            using var g1 = Graphics.FromImage(small);
            g1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g1.DrawImage(image, 0, 0, sw, sh);

            var result = new Bitmap(image.Width, image.Height);
            using var g2 = Graphics.FromImage(result);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g2.DrawImage(small, 0, 0, image.Width, image.Height);
            return result;
        }

        private Bitmap MakeGray(Image image)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            float[][] matrix = {
                new float[] {.299f, .299f, .299f, 0, 0},
                new float[] {.587f, .587f, .587f, 0, 0},
                new float[] {.114f, .114f, .114f, 0, 0},
                new float[] {0,     0,     0,     1, 0},
                new float[] {0,     0,     0,     0, 1}
            };
            var cm = new ColorMatrix(matrix);
            var ia = new ImageAttributes();
            ia.SetColorMatrix(cm);
            using var g = Graphics.FromImage(bmp);
            g.DrawImage(image,
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                0, 0, image.Width, image.Height,
                GraphicsUnit.Pixel, ia);
            return bmp;
        }

        // ════════════════════════════════════════════════════════════
        // ✅ AI 예측 — 서버 프로세스에 경로 전달 후 즉시 결과 수신
        // ════════════════════════════════════════════════════════════

        private void UpdateThrottleStatistics(double? throttle)
        {
            if (!throttle.HasValue) return;
            double thr = Math.Abs(throttle.Value);
            if (thr < 0.01) return;

            int binIndex = Math.Min(100, (int)(thr * 100.0));
            _throttleHistogram[binIndex]++;
            _totalThrottleCount++;

            int dropCount = (int)(_totalThrottleCount * 0.01);
            if (dropCount == 0)
            {
                _humanMaxThrottle = Math.Max(_humanMaxThrottle, thr);
                _humanMinThrottle = Math.Min(_humanMinThrottle, thr);
            }
            else
            {
                int cc = 0;
                for (int i = 0; i <= 100; i++) { cc += _throttleHistogram[i]; if (cc > dropCount) { _humanMinThrottle = i / 100.0; break; } }
                cc = 0;
                for (int i = 100; i >= 0; i--) { cc += _throttleHistogram[i]; if (cc > dropCount) { _humanMaxThrottle = i / 100.0; break; } }
            }
        }

        private async void OnTubDataChanged(string imagePath, double? angle, double? throttle,
                                            int currentIndex, int totalCount)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => OnTubDataChanged(imagePath, angle, throttle, currentIndex, totalCount));
                return;
            }

            _humanAngle = angle;
            _humanThrottle = throttle;
            _lastImagePath = imagePath;
            UpdateThrottleStatistics(throttle);

            if (!string.IsNullOrEmpty(imagePath))
            {
                string fname = Path.GetFileName(imagePath);

                // 캐시에 있으면 즉시 표시
                if (_predictionCache.TryGetValue(fname, out var cached))
                {
                    _aiAngle = cached.angle;
                    _aiThrottle = cached.throttle;
                }
                else
                {
                    _aiAngle = null;
                    _aiThrottle = null;
                    RefreshAllSlots();

                    // ✅ 서버에 경로 보내고 즉시 결과 받기
                    string wslPath = ConvertToWslPath(imagePath);
                    var result = await RequestPrediction(wslPath);

                    if (result.HasValue && !this.IsDisposed)
                    {
                        _predictionCache[fname] = result.Value;

                        // 아직 같은 이미지 보고 있을 때만 업데이트
                        if (_lastImagePath == imagePath)
                        {
                            _aiAngle = result.Value.angle;
                            _aiThrottle = result.Value.throttle;
                        }
                    }
                }
            }

            RefreshAllSlots();
        }

        private static string ConvertToWslPath(string windowsPath)
        {
            if (string.IsNullOrEmpty(windowsPath)) return windowsPath;
            if (windowsPath.StartsWith("\\\\wsl.localhost\\"))
            {
                int nextSlash = windowsPath.IndexOf('\\', 16);
                if (nextSlash != -1) return windowsPath.Substring(nextSlash).Replace("\\", "/");
            }
            if (windowsPath.Length >= 2 && windowsPath[1] == ':')
            {
                char drive = char.ToLower(windowsPath[0]);
                return $"/mnt/{drive}" + windowsPath.Substring(2).Replace("\\", "/");
            }
            return windowsPath.Replace("\\", "/");
        }

        private int CalcLineLen(double? throttle, int h)
        {
            int zeroLen = (int)(h * 0.1);
            int minLen = (int)(h * 0.2);
            int maxLen = (int)(h * 0.6);
            int absoluteMaxLen = (int)(h * 0.95);

            if (!throttle.HasValue) return zeroLen;
            double thr = Math.Abs(throttle.Value);
            if (thr < 0.01) return zeroLen;
            if (_humanMaxThrottle == 0.0 || _humanMinThrottle == 999.0) return minLen;
            if (_humanMaxThrottle - _humanMinThrottle < 0.01)
            {
                int safeLen = (int)(minLen + (thr / _humanMaxThrottle) * (maxLen - minLen));
                return Math.Max(zeroLen, Math.Min(absoluteMaxLen, safeLen));
            }

            int length;
            if (thr >= _humanMinThrottle)
            {
                double ratio = (thr - _humanMinThrottle) / (_humanMaxThrottle - _humanMinThrottle);
                length = (int)(minLen + ratio * (maxLen - minLen));
            }
            else
            {
                double ratio = thr / _humanMinThrottle;
                length = (int)(zeroLen + ratio * (minLen - zeroLen));
            }
            return Math.Max(zeroLen, Math.Min(absoluteMaxLen, length));
        }

        private void DrawStem(Graphics g, Color color, int startX, int startY, double rad, int lineLen)
        {
            int endX = startX + (int)(lineLen * Math.Sin(rad));
            int endY = startY - (int)(lineLen * Math.Cos(rad));
            using var pen = new Pen(color, 4f);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Flat;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            g.DrawLine(pen, startX, startY, endX, endY);
        }

        private void DrawOverlay(string imagePath) => RefreshAllSlots();

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!this.Visible) return;

            _humanAngle = ucTubManager.LastAngle;
            _humanThrottle = ucTubManager.LastThrottle;
            _lastImagePath = ucTubManager.LastImagePath;
            UpdateThrottleStatistics(ucTubManager.LastThrottle);

            if (!string.IsNullOrEmpty(_lastImagePath))
            {
                string fname = Path.GetFileName(_lastImagePath);
                if (_predictionCache.TryGetValue(fname, out var cached))
                {
                    _aiAngle = cached.angle;
                    _aiThrottle = cached.throttle;
                }
                RefreshAllSlots();
            }
            else RefreshAllSlots();
        }

        public void SetModel(string modelPath, string modelType = "linear")
        {
            _modelPath = modelPath;
            _modelType = modelType;
            _predictionCache.Clear();

            // ✅ 모델 바뀌면 Python 서버 재시작
            _pythonReady = false;
            _pythonProc?.Kill();
            _pythonProc?.Dispose();
            _pythonProc = null;

            Array.Clear(_throttleHistogram, 0, _throttleHistogram.Length);
            _totalThrottleCount = 0;
            _humanMaxThrottle = 0.0;
            _humanMinThrottle = 999.0;
        }
    }
}