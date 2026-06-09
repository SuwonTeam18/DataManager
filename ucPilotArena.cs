using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
        // [파일1 추가] 타임라인 인덱스 변경 시 외부(TubManager)에 알리는 이벤트
        // ════════════════════════════════════════════════════════════
        

        // ════════════════════════════════════════════════════════════
        // 재생 상태 공유 이벤트
        // ════════════════════════════════════════════════════════════
        public static event Action<object, bool>? OnPlaybackStateChanged; // sender, isPlaying
        public static event Action<int>? OnTimelineIndexChanged;

        public static void RaisePlaybackStarted(object sender, bool isPlaying)
        {
            OnPlaybackStateChanged?.Invoke(sender, isPlaying);
        }
        // 배속 동기화 이벤트
        public static event Action<string>? OnSpeedChanged;

        public static void RaiseSpeedChanged(string speed)
        {
            OnSpeedChanged?.Invoke(speed);
        }

        // ════════════════════════════════════════════════════════════
        // 파일럿 세트 관리
        // ════════════════════════════════════════════════════════════
        private class PilotSlot
        {
            public string PilotName = "";
            public string ModelFileName = "test1.h5";
            public string ModelType = "linear";
            public Process PythonProc = null;
            public bool PythonReady = false;
            public SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
            public ConcurrentDictionary<string, (double angle, double throttle)> Cache = new();
            public string GetWslModelPath(string wslMycarPath) => $"{wslMycarPath}/models/{ModelFileName}";
            public double? LastAiAngle = null;
            public double? LastAiThrottle = null;
        }

        private readonly List<PilotSlot> _pilotSlots = new();
        private readonly List<PictureBox> _displayPictureBoxes = new();
        private readonly List<Label> _aiAngleLabels = new();
        private readonly List<Label> _angleErrorLabels = new();
        private readonly List<Label> _aiThrottleLabels = new();
        private readonly List<Label> _throttleErrorLabels = new();
        private readonly List<Label> _avgErrorLabels = new();

        // ════════════════════════════════════════════════════════════
        // 이미지/타임라인 관리
        // ════════════════════════════════════════════════════════════
        private List<string> _imageFiles = new();
        private int _currentIndex = 0;

        // ════════════════════════════════════════════════════════════
        // AI 예측 관련
        // ════════════════════════════════════════════════════════════
        private const string PythonPath = "/home/xytron/miniconda3/envs/e2e_env/bin/python3";
        private const string WslScriptPath = "/tmp/predict_pilot.py";
        private const string WinScriptFile = @"\\wsl.localhost\Ubuntu-22.04\tmp\predict_pilot.py";

        private double? _humanAngle = null;
        private double? _humanThrottle = null;
        private string _lastImagePath = null;

        private string _mycarWinPath = @"\\wsl.localhost\Ubuntu-22.04\home\xytron\mycar";
        private string _mycarWslPath = "/home/xytron/mycar";
        private string _currentTubFolderPath = "";

        private double _humanMaxThrottle = 0.0;
        private double _humanMinThrottle = 999.0;
        private int[] _throttleHistogram = new int[101];
        private int _totalThrottleCount = 0;

        // [파일1 추가] 타임라인 디바운스
        private System.Windows.Forms.Timer _timelineDebounce = new System.Windows.Forms.Timer();
        private int _pendingTimelineIndex = -1;
        private bool _suppressTimelineNotify = false;

        private System.Windows.Forms.Timer _sliderDebounce = new System.Windows.Forms.Timer();
         private CancellationTokenSource _bgTaskCts = new CancellationTokenSource();
        private bool _hasShownPythonError = false;
        private bool _suppressOnTubDataChangedTimelineSet = false;

        private static readonly Color HumanColor = Color.FromArgb(255, 255, 87, 34);
        private static readonly Color AiColor = Color.FromArgb(255, 0, 176, 255);

        // 현재 실제로 재생 중인 타이머 (자신 또는 상대방)
        private static System.Windows.Forms.Timer? _activePlayTimer = null;

        // ════════════════════════════════════════════════════════════
        // [파일1 추가] 그래프 관련 필드
        // ════════════════════════════════════════════════════════════
        private Panel _graphDrawPanel = null;
        private Button _btnGraphError = null;
        private Button _btnGraphAngle = null;
        private Button _btnGraphThrottle = null;
        private Button _btnGenerateFilteredGraph = null;
        private Button _btnResetZoom = null;
        private Label _lblGraphFilterStatus = null;
        private int _graphBrightness = 0;
        private int _graphBlur = 0;

        private Panel _pnlErrorOptions = null;
        private RadioButton _rdoErrTotal = null;
        private RadioButton _rdoErrAngle = null;
        private RadioButton _rdoErrThrottle = null;
        private Label _lblLegendHuman = null;
        private Label _lblLegendAi = null;

        private enum GraphMode { Angle, Throttle, ErrorTotal, ErrorAngle, ErrorThrottle }
        private GraphMode _graphMode = GraphMode.ErrorTotal;

        private List<string> _graphImagePaths = new();
        private List<double> _graphHumanAngles = new();
        private List<double> _graphHumanThrottles = new();
        private double?[] _graphAiAngles = Array.Empty<double?>();
        private double?[] _graphAiThrottles = Array.Empty<double?>();

        // 캐시: 파일명 -> (angle, throttle)
        private readonly Dictionary<string, (double angle, double throttle)> _predictionCache = new();

        private double _graphZoom = 1.0;
        private double _graphZoomY = 1.0;
        private int _graphScrollX = 0;
        private double _graphOffsetY = 0.0;
        private bool _graphDragging = false;
        private int _graphDragStartX = 0;
        private int _graphDragStartY = 0;
        private double _graphDragOffY = 0.0;
        private bool _dragAxisDecided = false;
        private bool _dragIsHorizontal = false;

        // ════════════════════════════════════════════════════════════
        // Python 스크립트 내용
        // ════════════════════════════════════════════════════════════
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
            InitUserValuePanel();
            SetupRankControls();

            try
            {
                string tempScriptPath = Path.Combine(Path.GetTempPath(), "predict_pilot.py");
                File.WriteAllText(tempScriptPath, ScriptContent, new UTF8Encoding(false));
            }
            catch { }

            // 슬라이더 디바운스 (200ms)
            // 변경 — sliderDebounce는 더 이상 오차율 갱신에 사용하지 않으므로 Tick 비움
            _sliderDebounce.Interval = 200;
            _sliderDebounce.Tick += (s, e) => { _sliderDebounce.Stop(); };

            // [파일1 추가] 타임라인 디바운스 (200ms)
            _timelineDebounce.Interval = 200;
            _timelineDebounce.Tick += (s, e) =>
            {
                _timelineDebounce.Stop();
                if (_pendingTimelineIndex < 0) return;

                int idx = _pendingTimelineIndex;
                _pendingTimelineIndex = -1;

                _currentIndex = idx;
                _lastImagePath = (_imageFiles.Count > 0 && idx < _imageFiles.Count)
                    ? _imageFiles[idx] : _lastImagePath;

                try { OnTimelineIndexChanged?.Invoke(_currentIndex); } catch { }

                if (!string.IsNullOrEmpty(_lastImagePath))
                {
                    foreach (int i in Enumerable.Range(0, _pilotSlots.Count))
                        _ = RequestAndUpdateSlot(i, _lastImagePath, trkBrightness.Value, trkBlur.Value);
                }
                RefreshAllSlots();
                _graphDrawPanel?.Invalidate();
            };

            btnAddLeftPic.MouseEnter += (s, e) => btnAddLeftPic.BackColor = Color.FromArgb(14, 75, 140);
            btnAddLeftPic.MouseLeave += (s, e) => btnAddLeftPic.BackColor = Color.FromArgb(24, 95, 165);
            btnRemoveLeftPic.MouseEnter += (s, e) => btnRemoveLeftPic.BackColor = Color.FromArgb(190, 190, 190);
            btnRemoveLeftPic.MouseLeave += (s, e) => btnRemoveLeftPic.BackColor = Color.FromArgb(210, 210, 210);

            // ════════════════════════════════════════════════════════════
            // 재생/정지 + 배속 + 프레임 이동 버튼 활성화
            // ════════════════════════════════════════════════════════════
            bool isPlaying = false;
            var playTimer = new System.Windows.Forms.Timer();

            if (cmbSpeed.Items.Count > 0 && cmbSpeed.SelectedIndex < 0)
                cmbSpeed.SelectedIndex = 3; // "1.00x"

            int GetPlayInterval()
            {
                string txt = (cmbSpeed.SelectedItem?.ToString() ?? "1.00x")
                    .Replace("x", "").Trim();
                if (!double.TryParse(txt, System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out double spd) || spd <= 0)
                    spd = 1.0;
                return Math.Max(16, (int)(100.0 / spd));
            }

            playTimer.Interval = GetPlayInterval();
            cmbSpeed.SelectedIndexChanged += (s, e) =>
            {
                playTimer.Interval = GetPlayInterval();
                RaiseSpeedChanged(cmbSpeed.SelectedItem?.ToString() ?? "1.00x");
            };

            // 상대방 배속 변경 수신
            OnSpeedChanged += (speed) =>
            {
                if (this.IsDisposed || !this.IsHandleCreated) return;
                this.BeginInvoke(() =>
                {
                    if (cmbSpeed.SelectedItem?.ToString() == speed) return; // 무한루프 방지
                    for (int i = 0; i < cmbSpeed.Items.Count; i++)
                    {
                        if (cmbSpeed.Items[i]?.ToString() == speed)
                        {
                            cmbSpeed.SelectedIndex = i;
                            break;
                        }
                    }
                });
            };

            // ── PilotArena 독립 네비게이션 ──
            // TubManager 의존 없이 _imageFiles 기준으로 직접 이동
            void NavigateTo(int idx)
            {
                if (_imageFiles.Count == 0) return;
                idx = Math.Max(0, Math.Min(idx, _imageFiles.Count - 1));

                _currentIndex = idx;
                _lastImagePath = _imageFiles[idx];

                // trkTimeline 동기화
                _suppressTimelineNotify = true;
                if (idx >= trkTimeline.Minimum && idx <= trkTimeline.Maximum)
                    trkTimeline.Value = idx;
                _suppressTimelineNotify = false;

                // catalog에서 읽은 human 값으로 UI 갱신
                if (_graphHumanAngles.Count > idx)
                {
                    _humanAngle = _graphHumanAngles[idx];
                    _humanThrottle = _graphHumanThrottles[idx];
                    UpdateUserValuePanel();
                }

                RefreshAllSlots();
                UpdateRecordIndexLabel();
                _graphDrawPanel?.Invalidate();

                // TubManager에도 알림 (동기화)
                try { OnTimelineIndexChanged?.Invoke(_currentIndex); } catch { }
            }

            // ── 재생 타이머 Tick ──
            // ── 재생 타이머 Tick ──
            // ── 다른 탭에서 재생 시작 시 내 재생 강제 정지 ──
            OnPlaybackStateChanged += (sender, playing) =>
            {
                if (sender == playTimer) return;
                if (this.IsDisposed || !this.IsHandleCreated) return;
                this.BeginInvoke(() =>
                {
                    if (playing)
                    {
                        // 상대방이 재생 시작
                        // 내 타이머가 돌고 있으면 멈춤
                        if (isPlaying) { playTimer.Stop(); isPlaying = false; }
                        // 상대방 타이머 참조 저장
                        _activePlayTimer = sender as System.Windows.Forms.Timer;
                        btnStop.Text = "정지";
                        btnStop.BackColor = Color.FromArgb(255, 240, 240);
                        btnStop.ForeColor = Color.FromArgb(180, 40, 40);
                    }
                    else
                    {
                        // 상대방이 정지
                        _activePlayTimer = null;
                        btnStop.Text = "재생";
                        btnStop.BackColor = Color.FromArgb(230, 242, 255);
                        btnStop.ForeColor = Color.FromArgb(24, 95, 165);
                    }
                });
            };

            // ── 재생 타이머 Tick ──
            playTimer.Tick += (s, e) =>
            {
                if (_imageFiles.Count == 0) { playTimer.Stop(); return; }
                int next = _currentIndex + 1;
                if (next >= _imageFiles.Count)
                {
                    isPlaying = false;
                    playTimer.Stop();
                    btnStop.Text = "재생";
                    btnStop.BackColor = Color.FromArgb(230, 242, 255);
                    btnStop.ForeColor = Color.FromArgb(24, 95, 165);
                    return;
                }
                NavigateTo(next);
            };

            // ── 재생/정지 ──
            btnStop.Click += (s, e) =>
            {
                // 상대방이 재생 중인 경우 (버튼이 "정지"지만 내 isPlaying은 false)
                if (!isPlaying && _activePlayTimer != null)
                {
                    // 상대방 타이머 직접 정지
                    _activePlayTimer.Stop();
                    _activePlayTimer = null;
                    btnStop.Text = "재생";
                    btnStop.BackColor = Color.FromArgb(230, 242, 255);
                    btnStop.ForeColor = Color.FromArgb(24, 95, 165);
                    RaisePlaybackStarted(playTimer, false); // 상대방 버튼도 "재생"으로
                    return;
                }

                if (_imageFiles.Count == 0) return;
                isPlaying = !isPlaying;
                if (isPlaying)
                {
                    _activePlayTimer = playTimer;
                    btnStop.Text = "정지";
                    btnStop.BackColor = Color.FromArgb(255, 240, 240);
                    btnStop.ForeColor = Color.FromArgb(180, 40, 40);
                    playTimer.Interval = GetPlayInterval();
                    playTimer.Start();
                    RaisePlaybackStarted(playTimer, true);
                }
                else
                {
                    _activePlayTimer = null;
                    btnStop.Text = "재생";
                    btnStop.BackColor = Color.FromArgb(230, 242, 255);
                    btnStop.ForeColor = Color.FromArgb(24, 95, 165);
                    playTimer.Stop();
                    RaisePlaybackStarted(playTimer, false);
                }
            };

            // ── << 100장 이전 ──
            btnRewind.Click += (s, e) =>
            {
                if (_imageFiles.Count == 0) return;
                NavigateTo(_currentIndex - 100);
            };

            // ── >> 100장 이후 ──
            btnFastForward.Click += (s, e) =>
            {
                if (_imageFiles.Count == 0) return;
                NavigateTo(_currentIndex + 100);
            };
        
            // ── < 이전 프레임 ──
            btnPrev.Click += (s, e) =>
            {
                if (_imageFiles.Count == 0 || _currentIndex <= 0) return;
                NavigateTo(_currentIndex - 1);
            };

            // ── > 다음 프레임 ──
            btnNext.Click += (s, e) =>
            {
                if (_imageFiles.Count == 0 || _currentIndex >= _imageFiles.Count - 1) return;
                NavigateTo(_currentIndex + 1);
            };

            // ── 호버 효과 ──
            foreach (Button btn in new[] { btnRewind, btnFastForward })
            {
                btn.MouseEnter += (s, e) => {
                    ((Button)s).BackColor = Color.FromArgb(200, 200, 200);
                    ((Button)s).FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
                };
                btn.MouseLeave += (s, e) => {
                    ((Button)s).BackColor = Color.FromArgb(224, 224, 224);
                    ((Button)s).FlatAppearance.BorderColor = Color.FromArgb(204, 204, 204);
                };
            }
            foreach (Button btn in new[] { btnPrev, btnNext })
            {
                btn.MouseEnter += (s, e) => {
                    ((Button)s).BackColor = Color.FromArgb(216, 216, 216);
                    ((Button)s).FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                };
                btn.MouseLeave += (s, e) => {
                    ((Button)s).BackColor = Color.FromArgb(236, 236, 236);
                    ((Button)s).FlatAppearance.BorderColor = Color.FromArgb(221, 221, 221);
                };
            }
            btnAddLeftPic.Click += BtnAddLeftPic_Click;
            btnRemoveLeftPic.Click += BtnRemoveLeftPic_Click;
            if (cmbNumColumns != null) cmbNumColumns.SelectedIndexChanged += (s, e) => UpdateDisplay();

            trkTimeline.Scroll += trkTimeline_Scroll;
            lblRecordIndexDisplay.KeyDown += (s, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                TryNavigateFromRecordText();
            };
            lblRecordIndexDisplay.Leave += (s, e) => TryNavigateFromRecordText();
            // 타임라인 디바운스 설정 (200ms)

            trkBrightness.Scroll += trkBrightness_Scroll;
            trkBlur.Scroll += trkBlur_Scroll;
            pnlImageArea.Resize += (s, e) => UpdateDisplay();
            ucTubManager.OnTubDataChanged += OnTubDataChanged;

            string lastImg = FixPath(ucTubManager.LastImagePath);
            if (!string.IsNullOrEmpty(lastImg)) UpdateMycarPath(lastImg);

            _pilotSlots.Clear();
            AddPilotSet();
            InitGraphArea();

            // 필터 적용 / 삭제 버튼 패널을 pnlBrightBlur에 동적 추가
            var pnlFilterButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 36,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.FromArgb(244, 243, 238),
                Padding = new Padding(6, 4, 0, 0)
            };

            var btnApplyFilter = new Button
            {
                Text = "필터 적용",
                Size = new Size(120, 30),
                AutoSize = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(24, 95, 165),
                ForeColor = Color.White,
                Font = new Font("맑은 고딕", 8.5f),
                Margin = new Padding(0, 0, 6, 0),
                Cursor = Cursors.Hand
            };
            btnApplyFilter.FlatAppearance.BorderSize = 0;
            btnApplyFilter.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(_lastImagePath))
                {
                    foreach (int i in Enumerable.Range(0, _pilotSlots.Count))
                        _ = RequestAndUpdateSlot(i, _lastImagePath, trkBrightness.Value, trkBlur.Value);
                }
                _graphBrightness = trkBrightness.Value;
                _graphBlur = trkBlur.Value;
                if (_lblGraphFilterStatus != null)
                    _lblGraphFilterStatus.Text = $"필터값 - 밝기: {_graphBrightness}, 흐림: {_graphBlur}";
                _graphDrawPanel?.Invalidate();
            };

            var btnResetFilter = new Button
            {
                Text = "필터 삭제",
                Size = new Size(120, 30),
                AutoSize = false,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(210, 210, 210),
                ForeColor = Color.FromArgb(50, 50, 50),
                Font = new Font("맑은 고딕", 8.5f),
                Margin = new Padding(0, 0, 6, 0),
                Cursor = Cursors.Hand
            };
            btnResetFilter.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            btnResetFilter.Click += (s, e) =>
            {
                trkBrightness.Value = 0;
                trkBlur.Value = 0;
                lblBrightnessValue.Text = "밝기 0";
                lblBlurValue.Text = "흐림 0";

                if (!string.IsNullOrEmpty(_lastImagePath))
                {
                    foreach (int i in Enumerable.Range(0, _pilotSlots.Count))
                        _ = RequestAndUpdateSlot(i, _lastImagePath, 0, 0);
                }
                _graphBrightness = 0;
                _graphBlur = 0;
                if (_lblGraphFilterStatus != null)
                    _lblGraphFilterStatus.Text = "필터값 - 밝기: 0, 흐림: 0";
                RefreshAllSlots();
                _graphDrawPanel?.Invalidate();
            };

            pnlFilterButtons.Controls.Add(btnApplyFilter);
            pnlFilterButtons.Controls.Add(btnResetFilter);
            pnlBrightBlur.Controls.Add(pnlFilterButtons);

            if (!string.IsNullOrEmpty(ucTubManager.CurrentTubPath))
                LoadImages(ucTubManager.CurrentTubPath);
        }

        // ════════════════════════════════════════════════════════════
        // [파일1 추가] 외부에서 타임라인 인덱스 조회/설정
        // ════════════════════════════════════════════════════════════


        public void SetTimelineIndexFromExternal(int index)
        {
            if (this.InvokeRequired) { this.BeginInvoke(() => SetTimelineIndexFromExternal(index)); return; }
            if (index < trkTimeline.Minimum || index > trkTimeline.Maximum) return;
            _suppressTimelineNotify = true;
            trkTimeline.Value = index;
            _currentIndex = index;
            _suppressTimelineNotify = false;
            RefreshAllSlots();
            UpdateRecordIndexLabel();
            _graphDrawPanel?.Invalidate();
        }

        // ════════════════════════════════════════════════════════════
        // 유틸리티
        // ════════════════════════════════════════════════════════════
        private int GetFileNumber(string path)
        {
            var match = System.Text.RegularExpressions.Regex.Match(Path.GetFileNameWithoutExtension(path), @"\d+");
            return match.Success ? int.Parse(match.Value) : 0;
        }

        private static string FixPath(string path) => path?.Replace('#', '\\');

        private static string ConvertToWslPath(string windowsPath)
        {
            if (string.IsNullOrEmpty(windowsPath)) return windowsPath;
            if (windowsPath.StartsWith("\\\\wsl.localhost\\"))
            {
                int nextSlash = windowsPath.IndexOf('\\', 16);
                if (nextSlash != -1) return windowsPath.Substring(nextSlash).Replace("\\", "/");
            }
            if (windowsPath.Length >= 2 && windowsPath[1] == ':')
                return $"/mnt/{char.ToLower(windowsPath[0])}" + windowsPath.Substring(2).Replace("\\", "/");
            return windowsPath.Replace("\\", "/");
        }

        private void UpdateMycarPath(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;
            int dataIdx = imagePath.ToLower().IndexOf("\\data\\");
            if (dataIdx > 0) { _mycarWinPath = imagePath.Substring(0, dataIdx); _mycarWslPath = ConvertToWslPath(_mycarWinPath); }
        }

        // 변경
        private List<string> GetModelFiles()
        {
            if (string.IsNullOrEmpty(_mycarWinPath)) return new List<string>();
            string modelsPath = Path.Combine(_mycarWinPath, "models");
            if (!Directory.Exists(modelsPath)) return new List<string>();
            return new[] { "*.h5", "*.tflite", "*.keras", "*.savedmodel", "*.pkl" }
              .SelectMany(ext => Directory.GetFiles(modelsPath, ext))
              .Select(f => Path.GetFileName(f))
              .OrderBy(f => f)
              .ToList();
        }

        // ════════════════════════════════════════════════════════════
        // Python 서버 프로세스 관리 (슬롯별 독립)
        // ════════════════════════════════════════════════════════════
        private async Task EnsurePythonServer(PilotSlot slot)
        {
            if (slot.PythonReady && slot.PythonProc != null && !slot.PythonProc.HasExited) return;
            slot.PythonReady = false;
            try { slot.PythonProc?.Kill(); } catch { }
            try { slot.PythonProc?.Dispose(); } catch { }
            slot.PythonProc = null;
            if (string.IsNullOrEmpty(slot.ModelFileName)) return;

            string localScriptFile = Path.Combine(Path.GetTempPath(), "predict_pilot.py");
            string wslScriptPath = ConvertToWslPath(localScriptFile);
            string bashCmd = $"cd {_mycarWslPath} && {PythonPath} {wslScriptPath} {slot.GetWslModelPath(_mycarWslPath)} {slot.ModelType} --server";
            var psi = new ProcessStartInfo { FileName = "wsl", Arguments = $"-e bash -c \"{bashCmd}\"", RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true };

            Process proc;
            try { proc = Process.Start(psi); } catch { return; }
            if (proc == null) return;
            slot.PythonProc = proc;

            string pythonErrorLog = "";
            _ = Task.Run(async () =>
            {
                try { while (!proc.HasExited && !proc.StandardError.EndOfStream) { string err = await proc.StandardError.ReadLineAsync(); if (!string.IsNullOrEmpty(err)) { pythonErrorLog += err + "\n"; Debug.WriteLine($"[stderr] {err}"); } } } catch { }
            });

            try
            {
                while (!proc.HasExited && !proc.StandardOutput.EndOfStream)
                {
                    string line = await proc.StandardOutput.ReadLineAsync();
                    if (line == null) break;
                    if (line.Contains("READY")) { slot.PythonReady = true; _hasShownPythonError = false; break; }
                }
            }
            catch { }

            if (!slot.PythonReady)
            {
                _bgTaskCts?.Cancel();
                if (!_hasShownPythonError && !this.IsDisposed && this.IsHandleCreated)
                {
                    _hasShownPythonError = true;
                    this.BeginInvoke(() => { MessageBox.Show($"AI 서버(Python) 실행에 실패했습니다!\n\n모델 파일({slot.ModelFileName})이 없거나 경로에 문제가 있습니다.\n\n[파이썬 로그]\n{pythonErrorLog}", "AI 로딩 에러", MessageBoxButtons.OK, MessageBoxIcon.Error); });
                }
            }
        }

        private async Task<(double angle, double throttle)?> RequestPrediction(PilotSlot slot, string wslImagePath)
        {
            await slot.Semaphore.WaitAsync();
            try
            {
                await EnsurePythonServer(slot);
                if (!slot.PythonReady) return null;
                await slot.PythonProc.StandardInput.WriteLineAsync(wslImagePath);
                string result = null;
                while (!slot.PythonProc.HasExited && !slot.PythonProc.StandardOutput.EndOfStream)
                { string line = await slot.PythonProc.StandardOutput.ReadLineAsync(); if (line == null) break; if (line.StartsWith("RESULT:")) { result = line; break; } }
                if (result == null) return null;
                var parts = result.Substring(7).Split(':');
                if (parts.Length < 2) return null;
                if (!double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double ang)) return null;
                double thr = 0.0; double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out thr);
                return (ang, thr);
            }
            catch { return null; }
            finally { slot.Semaphore.Release(); }
        }

        private async Task<(double angle, double throttle)?> RequestPrediction(PilotSlot slot, Bitmap filteredImage)
        {
            await slot.Semaphore.WaitAsync();
            string tempWinPath = null;
            try
            {
                await EnsurePythonServer(slot);
                if (!slot.PythonReady) return null;
                tempWinPath = Path.Combine(Path.GetTempPath(), $"donkey_tmp_{Guid.NewGuid():N}.jpg");
                filteredImage.Save(tempWinPath, ImageFormat.Jpeg);
                string wslImagePath = ConvertToWslPath(tempWinPath);
                await slot.PythonProc.StandardInput.WriteLineAsync(wslImagePath);
                string result = null;
                while (!slot.PythonProc.HasExited && !slot.PythonProc.StandardOutput.EndOfStream)
                { string line = await slot.PythonProc.StandardOutput.ReadLineAsync(); if (line == null) break; if (line.StartsWith("RESULT:")) { result = line; break; } }
                if (result == null) return null;
                var parts = result.Substring(7).Split(':');
                if (parts.Length < 2) return null;
                if (!double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double ang)) return null;
                double thr = 0.0; double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out thr);
                return (ang, thr);
            }
            catch { return null; }
            finally
            {
                slot.Semaphore.Release();
                if (tempWinPath != null && File.Exists(tempWinPath)) { try { File.Delete(tempWinPath); } catch { } }
            }
        }

        private Bitmap GetFilteredBitmap(string imagePath, int brightness, int blur)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath)) return null;
            using var fs = File.OpenRead(imagePath);
            using var rawImg = Image.FromStream(fs);
            using var bright = MakeBrightness(rawImg, brightness);
            return MakeBlur(bright, blur);
        }

        private void ResetSlotServer(PilotSlot slot)
        {
            slot.PythonReady = false;
            try { slot.PythonProc?.Kill(); } catch { }
            try { slot.PythonProc?.Dispose(); } catch { }
            slot.PythonProc = null;
            slot.Cache.Clear();
            slot.LastAiAngle = null;
            slot.LastAiThrottle = null;
            _hasShownPythonError = false;
        }

        // ════════════════════════════════════════════════════════════
        // 파일럿 추가 / 제거
        // ════════════════════════════════════════════════════════════
        private void BtnAddLeftPic_Click(object? sender, EventArgs e) => AddPilotSet();

        private void BtnRemoveLeftPic_Click(object? sender, EventArgs e)
        {
            if (_pilotSlots.Count <= 1) return;
            ResetSlotServer(_pilotSlots[^1]);
            _pilotSlots.RemoveAt(_pilotSlots.Count - 1);
            UpdateDisplay();
        }

        private void AddPilotSet()
        {
            if (_pilotSlots.Count >= 4) return;
            _pilotSlots.Add(new PilotSlot());
            UpdateDisplay();
        }

        // ════════════════════════════════════════════════════════════
        // UpdateDisplay
        // ════════════════════════════════════════════════════════════
        private void UpdateDisplay()
        {
            if (pnlImageArea == null) return;
            pnlImageArea.Controls.Clear();
            _displayPictureBoxes.Clear(); _aiAngleLabels.Clear(); _angleErrorLabels.Clear();
            _aiThrottleLabels.Clear(); _throttleErrorLabels.Clear(); _avgErrorLabels.Clear();

            int count = _pilotSlots.Count; if (count == 0) return;
            int columns = 1;
            if (cmbNumColumns != null && int.TryParse(cmbNumColumns.SelectedItem?.ToString(), out int parsed)) columns = Math.Clamp(parsed, 1, 4);
            int rows = (int)Math.Ceiling(count / (double)columns);

            var table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = columns, RowCount = rows * 2 };
            table.RowStyles.Clear(); table.ColumnStyles.Clear();
            for (int c = 0; c < columns; c++) table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
            for (int r = 0; r < rows; r++) { table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows)); table.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F)); }

            var modelFiles = GetModelFiles();

            for (int i = 0; i < count; i++)
            {
                int col = i % columns, visualRow = (i / columns) * 2, slotIdx = i;
                var pb = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.StretchImage, BackColor = Color.Black };
                _displayPictureBoxes.Add(pb);

                var modelCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 110 };
                modelCombo.Items.AddRange(new string[] { "linear", "categorical", "behavior" });
                int typeIdx = Array.FindIndex(new[] { "linear", "categorical", "behavior" }, s => s == _pilotSlots[i].ModelType);
                modelCombo.SelectedIndex = typeIdx >= 0 ? typeIdx : 0;
                modelCombo.SelectedIndexChanged += (s, e) =>
                {
                    string sel = modelCombo.SelectedItem?.ToString() ?? "linear";
                    if (_pilotSlots[slotIdx].ModelType == sel) return;
                    _pilotSlots[slotIdx].ModelType = sel; ResetSlotServer(_pilotSlots[slotIdx]);
                    Array.Clear(_graphAiAngles, 0, _graphAiAngles.Length);
                    Array.Clear(_graphAiThrottles, 0, _graphAiThrottles.Length);
                    _graphDrawPanel?.Invalidate();
                    if (!string.IsNullOrEmpty(_lastImagePath)) _ = RequestAndUpdateSlot(slotIdx, _lastImagePath, trkBrightness.Value, trkBlur.Value);
                };

                var pilotCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
                foreach (var f in modelFiles) pilotCombo.Items.Add(f);
                if (!string.IsNullOrEmpty(_pilotSlots[i].ModelFileName))
                {
                    int selIdx = pilotCombo.Items.Cast<string>().ToList().IndexOf(_pilotSlots[i].ModelFileName);
                    pilotCombo.SelectedIndex = selIdx >= 0 ? selIdx : -1;
                }
                else { pilotCombo.SelectedIndex = -1; pilotCombo.Text = "모델 선택 안함"; }

                pilotCombo.SelectedIndexChanged += (s, e) =>
                {
                    string sel = pilotCombo.SelectedItem?.ToString() ?? ""; if (string.IsNullOrEmpty(sel)) return;
                    if (_pilotSlots[slotIdx].ModelFileName == sel) return;
                    _pilotSlots[slotIdx].ModelFileName = sel;
                    if (sel.EndsWith(".tflite")) _pilotSlots[slotIdx].ModelType = "tflite";
                    else if (sel.EndsWith(".keras")) _pilotSlots[slotIdx].ModelType = "keras";
                    else _pilotSlots[slotIdx].ModelType = modelCombo.SelectedItem?.ToString() ?? "linear";
                    ResetSlotServer(_pilotSlots[slotIdx]);
                    Array.Clear(_graphAiAngles, 0, _graphAiAngles.Length);
                    Array.Clear(_graphAiThrottles, 0, _graphAiThrottles.Length);
                    _graphDrawPanel?.Invalidate();
                    if (!string.IsNullOrEmpty(_lastImagePath)) _ = RequestAndUpdateSlot(slotIdx, _lastImagePath, trkBrightness.Value, trkBlur.Value);
                };

                var aiAngleLbl = new Label { ForeColor = Color.White, AutoSize = true, Text = "AI 각도 : N/A" };
                var angleErrLbl = new Label { ForeColor = Color.LightGreen, AutoSize = true, Text = "오차 : N/A" };
                var aiThrLbl = new Label { ForeColor = Color.White, AutoSize = true, Text = "AI 속도 : N/A" };
                var thrErrLbl = new Label { ForeColor = Color.LightGreen, AutoSize = true, Text = "오차 : N/A" };
                var avgLbl = new Label { ForeColor = Color.White, AutoSize = true, Text = "평균 오차율 : N/A" };
                _aiAngleLabels.Add(aiAngleLbl); _angleErrorLabels.Add(angleErrLbl);
                _aiThrottleLabels.Add(aiThrLbl); _throttleErrorLabels.Add(thrErrLbl); _avgErrorLabels.Add(avgLbl);

                var comboPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
                comboPanel.Controls.Add(pilotCombo); comboPanel.Controls.Add(modelCombo);
                var dataFlow = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
                dataFlow.Controls.Add(aiAngleLbl); dataFlow.Controls.Add(angleErrLbl);
                dataFlow.Controls.Add(new Label { AutoSize = true, Text = "   " });
                dataFlow.Controls.Add(aiThrLbl); dataFlow.Controls.Add(thrErrLbl);
                var infoPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(4) };
                infoPanel.Controls.Add(comboPanel); infoPanel.Controls.Add(dataFlow); infoPanel.Controls.Add(avgLbl);

                table.Controls.Add(pb, col, visualRow);
                table.Controls.Add(infoPanel, col, visualRow + 1);
            }

            pnlImageArea.Controls.Add(table);
            btnAddLeftPic.Enabled = _pilotSlots.Count < 4; btnRemoveLeftPic.Enabled = _pilotSlots.Count > 1;
            RefreshAllSlots();
        }

        // ════════════════════════════════════════════════════════════
        // AI 예측 요청 및 슬롯 업데이트
        // ════════════════════════════════════════════════════════════
        private async Task RequestAndUpdateSlot(int slotIdx, string imagePath, int brightness, int blur)
        {
            if (slotIdx >= _pilotSlots.Count) return;
            var slot = _pilotSlots[slotIdx];
            if (string.IsNullOrEmpty(slot.ModelFileName)) return;

            string fname = Path.GetFileName(imagePath);
            string cacheKey = $"{fname}_{brightness}_{blur}";

            if (!slot.Cache.ContainsKey(cacheKey))
            {
                (double angle, double throttle)? result = null;

                await Task.Run(async () =>
                {
                    if (brightness == 0 && blur == 0)
                    {
                        string wslPath = ConvertToWslPath(imagePath);
                        result = await RequestPrediction(slot, wslPath);
                    }
                    else
                    {
                        using var bmp = GetFilteredBitmap(imagePath, brightness, blur);
                        if (bmp != null) result = await RequestPrediction(slot, bmp);
                    }
                });

                if (result.HasValue)
                {
                    slot.Cache[cacheKey] = result.Value;
                    slot.LastAiAngle = result.Value.angle;
                    slot.LastAiThrottle = result.Value.throttle;
                }
            }

            if (!this.IsDisposed && this.IsHandleCreated)
                this.BeginInvoke(() =>
                {
                    if (slotIdx == 0 &&
                        slot.Cache.ContainsKey(cacheKey) &&
                        brightness == _graphBrightness && blur == _graphBlur)
                    {
                        int graphIdx = -1;
                        for (int gi = 0; gi < _graphImagePaths.Count; gi++)
                        {
                            if (string.Equals(Path.GetFileName(_graphImagePaths[gi]), fname, StringComparison.OrdinalIgnoreCase))
                            { graphIdx = gi; break; }
                        }
                        if (graphIdx >= 0 && graphIdx < _graphAiAngles.Length)
                        {
                            _graphAiAngles[graphIdx] = slot.Cache[cacheKey].angle;
                            _graphAiThrottles[graphIdx] = slot.Cache[cacheKey].throttle;
                            _graphDrawPanel?.Invalidate();
                        }
                    }
                    if (imagePath == _lastImagePath)
                        RefreshSlot(slotIdx);
                });
        }

        private void RefreshAllSlots() { for (int i = 0; i < _displayPictureBoxes.Count; i++) RefreshSlot(i); }

        private void UpdateRecordIndexLabel()
        {
            if (lblRecordIndexDisplay == null) return;
            int total = _imageFiles.Count;
            lblRecordIndexDisplay.Text = total == 0
                ? "기록 000000"
                : $"기록 {(_currentIndex + 1):D6}";
        }

        private void TryNavigateFromRecordText()
        {
            if (lblRecordIndexDisplay == null) return;
            var txt = lblRecordIndexDisplay.Text ?? string.Empty;

            // "기록 000042" 형태에서 숫자만 추출
            var m = System.Text.RegularExpressions.Regex.Match(txt, @"\d+");
            if (!m.Success) { UpdateRecordIndexLabel(); return; }

            if (!int.TryParse(m.Value, out int num)) { UpdateRecordIndexLabel(); return; }

            // 1-based → 0-based 변환
            int idx = num - 1;
            if (idx < 0) idx = 0;
            if (idx >= _imageFiles.Count) idx = _imageFiles.Count - 1;

            if (_imageFiles.Count == 0) { UpdateRecordIndexLabel(); return; }

            _currentIndex = idx;
            _lastImagePath = _imageFiles[idx];

            _suppressTimelineNotify = true;
            if (idx >= trkTimeline.Minimum && idx <= trkTimeline.Maximum)
                trkTimeline.Value = idx;
            _suppressTimelineNotify = false;

            if (_graphHumanAngles.Count > idx)
            {
                _humanAngle = _graphHumanAngles[idx];
                _humanThrottle = _graphHumanThrottles[idx];
                UpdateUserValuePanel();
            }

            RefreshAllSlots();
            UpdateRecordIndexLabel();
            _graphDrawPanel?.Invalidate();

            try { OnTimelineIndexChanged?.Invoke(_currentIndex); } catch { }
        }

        private (double? angle, double? throttle) GetCurrentHumanValues()
        {
            if (_graphHumanAngles != null && _graphHumanThrottles != null &&
                _currentIndex >= 0 && _currentIndex < _graphHumanAngles.Count)
            {
                return (_graphHumanAngles[_currentIndex], _graphHumanThrottles[_currentIndex]);
            }
            return (_humanAngle, _humanThrottle);
        }

        private void RefreshSlot(int i)
        {
            if (i >= _pilotSlots.Count || i >= _displayPictureBoxes.Count) return;
            if (string.IsNullOrEmpty(_lastImagePath) || !File.Exists(_lastImagePath)) return;

            var slot = _pilotSlots[i];
            string fname = Path.GetFileName(_lastImagePath);
            string cacheKey = $"{fname}_{trkBrightness.Value}_{trkBlur.Value}";

            double? finalAiAngle = null;
            double? finalAiThrottle = null;

            if (slot.Cache.TryGetValue(cacheKey, out var cached))
            {
                finalAiAngle = cached.angle;
                finalAiThrottle = cached.throttle;
                slot.LastAiAngle = cached.angle;
                slot.LastAiThrottle = cached.throttle;
            }
            else
            {
                finalAiAngle = slot.LastAiAngle;
                finalAiThrottle = slot.LastAiThrottle;
            }

            try
            {
                var (currentHumanAngle, currentHumanThrottle) = GetCurrentHumanValues();

                var bmp = BuildOverlayBitmap(_lastImagePath, finalAiAngle, finalAiThrottle);
                var oldImg = _displayPictureBoxes[i].Image;
                _displayPictureBoxes[i].Image = bmp;
                oldImg?.Dispose();

                double? angleErr = (finalAiAngle.HasValue && currentHumanAngle.HasValue) ? (finalAiAngle.Value - currentHumanAngle.Value) * 100.0 : null;
                double? thrErr = (finalAiThrottle.HasValue && currentHumanThrottle.HasValue) ? (finalAiThrottle.Value - currentHumanThrottle.Value) * 100.0 : null;

                string aiAngleText = finalAiAngle.HasValue ? finalAiAngle.Value.ToString("+0.000;-0.000;0.000") : "N/A";
                string aiThrText = finalAiThrottle.HasValue ? finalAiThrottle.Value.ToString("+0.000;-0.000;0.000") : "N/A";

                if (i < _aiAngleLabels.Count) _aiAngleLabels[i].Text = "AI 각도 : " + aiAngleText;
                if (i < _angleErrorLabels.Count) _angleErrorLabels[i].Text = "오차 : " + (angleErr.HasValue ? angleErr.Value.ToString("+0.0;-0.0;0.0") + "%" : "N/A");
                if (i < _aiThrottleLabels.Count) _aiThrottleLabels[i].Text = "AI 속도 : " + aiThrText;
                if (i < _throttleErrorLabels.Count) _throttleErrorLabels[i].Text = "오차 : " + (thrErr.HasValue ? thrErr.Value.ToString("+0.0;-0.0;0.0") + "%" : "N/A");
                if (i < _avgErrorLabels.Count)
                {
                    if (angleErr.HasValue && thrErr.HasValue)
                        _avgErrorLabels[i].Text = "평균 오차율 : " + ((Math.Abs(angleErr.Value) + Math.Abs(thrErr.Value)) / 2.0).ToString("0.0") + "%";
                    else _avgErrorLabels[i].Text = "평균 오차율 : N/A";
                }
            }
            catch { }
        }

        // ════════════════════════════════════════════════════════════
        // 오버레이 비트맵 생성
        // ════════════════════════════════════════════════════════════
        private Bitmap BuildOverlayBitmap(string imagePath, double? aiAngle, double? aiThrottle)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath)) return null;
            try
            {
                Bitmap bmp;
                using (var fs = File.OpenRead(imagePath)) using (var rawImg = Image.FromStream(fs))
                using (var bright = MakeBrightness(rawImg, trkBrightness.Value))
                { var p = MakeBlur(bright, trkBlur.Value); bmp = new Bitmap(p); p.Dispose(); }
                using var g = Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                int w = bmp.Width, h = bmp.Height, cx = w / 2, sy = h;
                var (currentHumanAngle, currentHumanThrottle) = GetCurrentHumanValues();

                if (currentHumanAngle.HasValue) { int ll = CalcLineLen(currentHumanThrottle, h); double rad = currentHumanAngle.Value * 45.0 * Math.PI / 180.0; DrawStem(g, HumanColor, cx - 2, sy, rad, ll); }
                if (aiAngle.HasValue) { int ll = CalcLineLen(aiThrottle, h); double rad = aiAngle.Value * 45.0 * Math.PI / 180.0; DrawStem(g, AiColor, cx + 2, sy, rad, ll); }
                return bmp;
            }
            catch { return null; }
        }

        public string? GetImagePathAt(int idx)
        {
            if (this.InvokeRequired)
                return (string?)this.Invoke(new Func<int, string?>(GetImagePathAt), idx);
            if (_imageFiles == null || _imageFiles.Count == 0) return null;
            if (idx < 0 || idx >= _imageFiles.Count) return null;
            return _imageFiles[idx];
        }

        // ════════════════════════════════════════════════════════════
        // 이미지 로드 / 타임라인
        // ════════════════════════════════════════════════════════════
        private void LoadImages(string folder)
        {
            if (!Directory.Exists(folder)) return;
            _imageFiles = Directory.GetFiles(folder)
              .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
              .OrderBy(f => GetFileNumber(f)).ToList();
            if (_imageFiles.Count == 0) return;

            foreach (var slot in _pilotSlots) ResetSlotServer(slot);

            trkTimeline.Minimum = 0; trkTimeline.Maximum = _imageFiles.Count - 1; trkTimeline.Value = 0; _currentIndex = 0;
            ShowFrame(0);
            UpdateRecordIndexLabel();

            string tubFolder = FixPath(ucTubManager.CurrentTubPath) ?? Directory.GetParent(folder)?.FullName ?? folder;
            if (tubFolder.EndsWith("images", StringComparison.OrdinalIgnoreCase)) tubFolder = Directory.GetParent(tubFolder)?.FullName ?? tubFolder;
            _currentTubFolderPath = tubFolder;

            _graphBrightness = trkBrightness.Value;
            _graphBlur = trkBlur.Value;
            if (_lblGraphFilterStatus != null)
                _lblGraphFilterStatus.Text = $"필터값 - 밝기: {_graphBrightness}, 흐림: {_graphBlur}";

            _ = LoadGraphDataAsync(tubFolder, _imageFiles);
        }

        private void ShowFrame(int index)
        {
            if (index < 0 || index >= _imageFiles.Count) return;
            try
            {
                using var fs = File.OpenRead(_imageFiles[index]); using var rawImg = Image.FromStream(fs);
                using var bright = MakeBrightness(rawImg, trkBrightness.Value); var p = MakeBlur(bright, trkBlur.Value);
                if (_displayPictureBoxes.Count > 0) { _displayPictureBoxes[0].Image?.Dispose(); _displayPictureBoxes[0].Image = new Bitmap(p); }
                p.Dispose();
            }
            catch { }
        }

        // Return image path at specified index
     

        private void trkTimeline_Scroll(object sender, EventArgs e)
        

            
{
    if (_suppressTimelineNotify)
    {
        _suppressTimelineNotify = false;
        return;
    }

    _currentIndex = trkTimeline.Value;

    if (_imageFiles.Count > 0 &&
        _currentIndex >= 0 &&
        _currentIndex < _imageFiles.Count)
    {
        _lastImagePath = _imageFiles[_currentIndex];
    }

            RefreshAllSlots();
            UpdateRecordIndexLabel();
            _graphDrawPanel?.Invalidate();

            _pendingTimelineIndex = trkTimeline.Value;

            _sliderDebounce.Stop();
    _sliderDebounce.Start();

    _timelineDebounce.Stop();
    _timelineDebounce.Start();
}

        private void ShowFrameByPath(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;
            try
            {
                using var fs = File.OpenRead(path);
                using var rawImg = Image.FromStream(fs);
                using var bright = MakeBrightness(rawImg, trkBrightness.Value);
                var processed = MakeBlur(bright, trkBlur.Value);

                if (_displayPictureBoxes.Count > 0)
                {
                    _displayPictureBoxes[0].Image?.Dispose();
                    _displayPictureBoxes[0].Image = new Bitmap(processed);
                }
                processed.Dispose();
                _lastImagePath = path;
            }
            catch { }
        }

        private IEnumerable<Control> FindControlsRecursive(Control.ControlCollection cols)
        {
            foreach (Control c in cols)
            {
                yield return c;
                foreach (var child in FindControlsRecursive(c.Controls))
                    yield return child;
            }

        }

        private void trkBrightness_Scroll(object sender, EventArgs e)
        {
            lblBrightnessValue.Text = "밝기 " + trkBrightness.Value;
            RefreshAllSlots();  // 이미지 미리보기만 즉시 반영
        }

        private void trkBlur_Scroll(object sender, EventArgs e)
        {
            lblBlurValue.Text = "흐림 " + trkBlur.Value;
            RefreshAllSlots();  // 이미지 미리보기만 즉시 반영
        }

        public void LoadUserTub(string folder) => LoadImages(folder);

        // ════════════════════════════════════════════════════════════
        // 이미지 필터 처리
        // ════════════════════════════════════════════════════════════
        private Bitmap MakeBrightness(Image image, int brightness)
        {
            var bmp = new Bitmap(image.Width, image.Height); float b = brightness / 100f;
            float[][] m = { new float[] { 1, 0, 0, 0, 0 }, new float[] { 0, 1, 0, 0, 0 }, new float[] { 0, 0, 1, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new float[] { b, b, b, 0, 1 } };
            var ia = new ImageAttributes(); ia.SetColorMatrix(new ColorMatrix(m));
            using var g = Graphics.FromImage(bmp); g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia); return bmp;
        }

        private Bitmap MakeBlur(Image image, int blurAmount)
        {
            int radius = blurAmount / 20; if (radius < 1) return new Bitmap(image);
            int sw = Math.Max(1, image.Width / (radius + 1)), sh = Math.Max(1, image.Height / (radius + 1));
            using var small = new Bitmap(sw, sh); using var g1 = Graphics.FromImage(small);
            g1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear; g1.DrawImage(image, 0, 0, sw, sh);
            var result = new Bitmap(image.Width, image.Height); using var g2 = Graphics.FromImage(result);
            g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear; g2.DrawImage(small, 0, 0, image.Width, image.Height); return result;
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
            g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);
            return bmp;
        }

        // ════════════════════════════════════════════════════════════
        // 스로틀 통계 / TubManager 이벤트
        // ════════════════════════════════════════════════════════════
        private void UpdateThrottleStatistics(double? throttle)
        {
            if (!throttle.HasValue) return; double thr = Math.Abs(throttle.Value); if (thr < 0.01) return;
            int binIndex = Math.Min(100, (int)(thr * 100.0)); _throttleHistogram[binIndex]++; _totalThrottleCount++;
            int dropCount = (int)(_totalThrottleCount * 0.01);
            if (dropCount == 0) { _humanMaxThrottle = Math.Max(_humanMaxThrottle, thr); _humanMinThrottle = Math.Min(_humanMinThrottle, thr); }
            else
            {
                int cc = 0; for (int i = 0; i <= 100; i++) { cc += _throttleHistogram[i]; if (cc > dropCount) { _humanMinThrottle = i / 100.0; break; } }
                cc = 0; for (int i = 100; i >= 0; i--) { cc += _throttleHistogram[i]; if (cc > dropCount) { _humanMaxThrottle = i / 100.0; break; } }
            }
        }

        private async void OnTubDataChanged(string imagePath, double? angle, double? throttle, int currentIndex, int totalCount)
        {
            if (this.InvokeRequired) { this.BeginInvoke(() => OnTubDataChanged(imagePath, angle, throttle, currentIndex, totalCount)); return; }
            imagePath = FixPath(imagePath);
            _humanAngle = angle; _humanThrottle = throttle; _lastImagePath = imagePath;
            UpdateUserValuePanel();
            UpdateThrottleStatistics(throttle);

            // ★ 추가: _imageFiles가 비어있으면 TubManager 경로로 채우기
            if (_imageFiles.Count == 0 && !string.IsNullOrEmpty(imagePath))
            {
                string imgFolder = Path.GetDirectoryName(imagePath);
                if (Directory.Exists(imgFolder))
                {
                    _imageFiles = Directory.GetFiles(imgFolder)
                        .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                                 || f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                                 || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        .OrderBy(f => GetFileNumber(f))
                        .ToList();

                    if (_imageFiles.Count > 0)
                    {
                        trkTimeline.Minimum = 0;
                        trkTimeline.Maximum = _imageFiles.Count - 1;
                    }
                }
            }

            // ★ 추가: _imageFiles가 채워졌는데 tubPath가 바뀐 경우 갱신
            if (!string.IsNullOrEmpty(imagePath))
            {
                string imgFolder = Path.GetDirectoryName(imagePath);
                if (_imageFiles.Count > 0)
                {
                    string currentFolder = Path.GetDirectoryName(_imageFiles[0]);
                    if (!string.Equals(imgFolder, currentFolder, StringComparison.OrdinalIgnoreCase))
                    {
                        _imageFiles = Directory.Exists(imgFolder)
                            ? Directory.GetFiles(imgFolder)
                                .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                                         || f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                                         || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                                .OrderBy(f => GetFileNumber(f))
                                .ToList()
                            : new List<string>();

                        if (_imageFiles.Count > 0)
                        {
                            trkTimeline.Minimum = 0;
                            trkTimeline.Maximum = _imageFiles.Count - 1;
                        }
                    }
                }
            }

            try
            {
                trkTimeline.Minimum = 0;
                trkTimeline.Maximum = Math.Max(0, totalCount - 1);
                if (currentIndex >= 0 && currentIndex <= trkTimeline.Maximum)
                {
                    _suppressTimelineNotify = true;
                    trkTimeline.Value = currentIndex;
                    _suppressTimelineNotify = false;
                }
            }
            catch { }
            _currentIndex = currentIndex;
            UpdateRecordIndexLabel();



            string tubPath = FixPath(ucTubManager.CurrentTubPath);
            if (!string.IsNullOrEmpty(tubPath))
            {
                string imagesFolder = tubPath.EndsWith("images", StringComparison.OrdinalIgnoreCase)
                    ? tubPath : Path.Combine(tubPath, "images");
                var validImages = Directory.Exists(imagesFolder)
                    ? Directory.GetFiles(imagesFolder)
                        .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                                 || f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                                 || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        .OrderBy(f => GetFileNumber(f)).ToList()
                    : _imageFiles;

                string catalogFolder = tubPath.EndsWith("images", StringComparison.OrdinalIgnoreCase)
                    ? Directory.GetParent(tubPath)?.FullName ?? tubPath : tubPath;

                if (_currentTubFolderPath != tubPath)
                {
                    // tub 경로가 바뀐 경우 → 전체 재로드
                    _currentTubFolderPath = tubPath;
                    _ = LoadGraphDataAsync(catalogFolder, validImages);
                }
                else if (validImages.Count != _graphImagePaths.Count)
                {
                    // 같은 tub인데 이미지 수가 달라진 경우 (레코드 삭제 감지)
                    _ = LoadGraphDataAsync(catalogFolder, validImages);

                    // AI 예측 배열 초기화
                    _graphAiAngles = new double?[validImages.Count];
                    _graphAiThrottles = new double?[validImages.Count];

                    // 현재 선택된 모델이 있으면 다시 채우기
                    if (_pilotSlots.Count > 0 && !string.IsNullOrEmpty(_pilotSlots[0].ModelFileName))
                    {
                        _bgTaskCts?.Cancel();
                        _bgTaskCts = new CancellationTokenSource();
                        _ = FillGraphFromSlotCacheAsync(validImages, _bgTaskCts.Token, _graphBrightness, _graphBlur);
                    }
                }
            }

            RefreshAllSlots();
            _graphDrawPanel?.Invalidate();

            if (!string.IsNullOrEmpty(imagePath))
            {
                var tasks = Enumerable.Range(0, _pilotSlots.Count).Select(i => RequestAndUpdateSlot(i, imagePath, trkBrightness.Value, trkBlur.Value));
                await Task.WhenAll(tasks);
            }
        }

        // ════════════════════════════════════════════════════════════
        // 오버레이 드로잉 헬퍼
        // ════════════════════════════════════════════════════════════
        private int CalcLineLen(double? throttle, int h)
        {
            int zeroLen = (int)(h * 0.1), minLen = (int)(h * 0.2), maxLen = (int)(h * 0.6), absMax = (int)(h * 0.95);
            if (!throttle.HasValue) return zeroLen; double thr = Math.Abs(throttle.Value); if (thr < 0.01) return zeroLen;
            if (_humanMaxThrottle == 0.0 || _humanMinThrottle == 999.0) return minLen;
            if (_humanMaxThrottle - _humanMinThrottle < 0.01) return Math.Max(zeroLen, Math.Min(absMax, (int)(minLen + (thr / _humanMaxThrottle) * (maxLen - minLen))));
            int length;
            if (thr >= _humanMinThrottle) { double ratio = (thr - _humanMinThrottle) / (_humanMaxThrottle - _humanMinThrottle); length = (int)(minLen + ratio * (maxLen - minLen)); }
            else { double ratio = thr / _humanMinThrottle; length = (int)(zeroLen + ratio * (minLen - zeroLen)); }
            return Math.Max(zeroLen, Math.Min(absMax, length));
        }

        private void DrawStem(Graphics g, Color color, int startX, int startY, double rad, int lineLen)
        {
            int endX = startX + (int)(lineLen * Math.Sin(rad)), endY = startY - (int)(lineLen * Math.Cos(rad));
            using var pen = new Pen(color, 4f) { StartCap = System.Drawing.Drawing2D.LineCap.Flat, EndCap = System.Drawing.Drawing2D.LineCap.Round };
            g.DrawLine(pen, startX, startY, endX, endY);
        }

        private void DrawOverlay(string imagePath) => RefreshAllSlots();

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e); if (!this.Visible) return;
            _humanAngle = ucTubManager.LastAngle; _humanThrottle = ucTubManager.LastThrottle;
            _lastImagePath = FixPath(ucTubManager.LastImagePath);
            UpdateThrottleStatistics(ucTubManager.LastThrottle); UpdateDisplay();

            if (!string.IsNullOrEmpty(_lastImagePath))
            { var tasks = Enumerable.Range(0, _pilotSlots.Count).Select(i => RequestAndUpdateSlot(i, _lastImagePath, trkBrightness.Value, trkBlur.Value)); _ = Task.WhenAll(tasks); }
        }

        public void SetModel(string modelPath, string modelType = "linear") { }

        // ════════════════════════════════════════════════════════════
        // [파일1 추가] 그래프 영역 초기화
        // ════════════════════════════════════════════════════════════
        private void InitGraphArea()
        {
            pnlGraphArea.Controls.Clear();
            pnlGraphArea.BackColor = Color.FromArgb(245, 244, 240);

            var btnRow = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 34, FlowDirection = FlowDirection.LeftToRight, BackColor = Color.FromArgb(245, 244, 240), Padding = new Padding(8, 4, 0, 0) };
            var lblTitle = new Label { Text = "그래프 뷰", AutoSize = true, Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold), ForeColor = Color.FromArgb(80, 80, 80), Margin = new Padding(0, 4, 12, 0) };

            _btnGraphError = MakeGraphBtn("오차율");
            _btnGraphAngle = MakeGraphBtn("각도");
            _btnGraphThrottle = MakeGraphBtn("속도");

            _pnlErrorOptions = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, Visible = false, Margin = new Padding(10, 4, 0, 0) };
            _rdoErrTotal = new RadioButton { Text = "종합", AutoSize = true, Checked = true, Font = new Font("맑은 고딕", 8f), Margin = new Padding(0, 0, 5, 0) };
            _rdoErrAngle = new RadioButton { Text = "각도", AutoSize = true, Font = new Font("맑은 고딕", 8f), Margin = new Padding(0, 0, 5, 0) };
            _rdoErrThrottle = new RadioButton { Text = "속도", AutoSize = true, Font = new Font("맑은 고딕", 8f), Margin = new Padding(0, 0, 5, 0) };
            _pnlErrorOptions.Controls.Add(_rdoErrTotal);
            _pnlErrorOptions.Controls.Add(_rdoErrAngle);
            _pnlErrorOptions.Controls.Add(_rdoErrThrottle);

            _lblLegendHuman = new Label { Text = "● 사람", AutoSize = true, ForeColor = Color.FromArgb(255, 87, 34), Margin = new Padding(20, 4, 6, 0) };
            _lblLegendAi = new Label { Text = "● AI", AutoSize = true, ForeColor = Color.FromArgb(0, 176, 255), Margin = new Padding(0, 4, 0, 0) };

            _btnGenerateFilteredGraph = new Button { Text = "그래프 생성", AutoSize = true, Height = 26, FlatStyle = FlatStyle.Flat, BackColor = Color.White, Font = new Font("맑은 고딕", 8.5f), Margin = new Padding(10, 0, 4, 0) };
            _btnGenerateFilteredGraph.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);

            _btnResetZoom = new Button { Text = "확대 초기화", AutoSize = true, Height = 26, FlatStyle = FlatStyle.Flat, BackColor = Color.White, Font = new Font("맑은 고딕", 8.5f), Margin = new Padding(4, 0, 4, 0) };
            _btnResetZoom.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);

            _lblGraphFilterStatus = new Label { Text = "필터값 - 밝기: 0, 흐림: 0", AutoSize = true, ForeColor = Color.FromArgb(100, 100, 100), Font = new Font("맑은 고딕", 8.5f), Margin = new Padding(0, 5, 0, 0) };

            btnRow.Controls.Add(lblTitle);
            btnRow.Controls.Add(_btnGraphError);
            btnRow.Controls.Add(_btnGraphAngle);
            btnRow.Controls.Add(_btnGraphThrottle);
            btnRow.Controls.Add(_pnlErrorOptions);
            btnRow.Controls.Add(_lblLegendHuman);
            btnRow.Controls.Add(_lblLegendAi);
            btnRow.Controls.Add(_btnGenerateFilteredGraph);
            btnRow.Controls.Add(_btnResetZoom);
            btnRow.Controls.Add(_lblGraphFilterStatus);

            Action updateLegend = () =>
            {
                bool isErr = _graphMode == GraphMode.ErrorTotal || _graphMode == GraphMode.ErrorAngle || _graphMode == GraphMode.ErrorThrottle;
                _lblLegendHuman.Text = isErr ? "● 기준선(0)" : "● 사람";
                _lblLegendAi.Text = isErr ? "● 오차값" : "● AI";
                _lblLegendHuman.ForeColor = isErr ? Color.Gray : Color.FromArgb(255, 87, 34);
                _lblLegendAi.ForeColor = isErr ? Color.FromArgb(255, 0, 128) : Color.FromArgb(0, 176, 255);
            };

            _btnGraphError.Click += (s, e) => { _pnlErrorOptions.Visible = true; SelectGraphBtn(_btnGraphError); if (_rdoErrTotal.Checked) _graphMode = GraphMode.ErrorTotal; else if (_rdoErrAngle.Checked) _graphMode = GraphMode.ErrorAngle; else _graphMode = GraphMode.ErrorThrottle; updateLegend(); _graphDrawPanel?.Invalidate(); };
            _btnGraphAngle.Click += (s, e) => { _pnlErrorOptions.Visible = false; SelectGraphBtn(_btnGraphAngle); _graphMode = GraphMode.Angle; updateLegend(); _graphDrawPanel?.Invalidate(); };
            _btnGraphThrottle.Click += (s, e) => { _pnlErrorOptions.Visible = false; SelectGraphBtn(_btnGraphThrottle); _graphMode = GraphMode.Throttle; updateLegend(); _graphDrawPanel?.Invalidate(); };
            _rdoErrTotal.CheckedChanged += (s, e) => { if (_rdoErrTotal.Checked && _pnlErrorOptions.Visible) { _graphMode = GraphMode.ErrorTotal; updateLegend(); _graphDrawPanel?.Invalidate(); } };
            _rdoErrAngle.CheckedChanged += (s, e) => { if (_rdoErrAngle.Checked && _pnlErrorOptions.Visible) { _graphMode = GraphMode.ErrorAngle; updateLegend(); _graphDrawPanel?.Invalidate(); } };
            _rdoErrThrottle.CheckedChanged += (s, e) => { if (_rdoErrThrottle.Checked && _pnlErrorOptions.Visible) { _graphMode = GraphMode.ErrorThrottle; updateLegend(); _graphDrawPanel?.Invalidate(); } };

            _btnGenerateFilteredGraph.Click += (s, e) =>
            {
                _graphBrightness = trkBrightness.Value;
                _graphBlur = trkBlur.Value;
                _lblGraphFilterStatus.Text = $"필터값 - 밝기: {_graphBrightness}, 흐림: {_graphBlur}";
                if (_graphImagePaths.Count == 0) return;

                Array.Clear(_graphAiAngles, 0, _graphAiAngles.Length);
                Array.Clear(_graphAiThrottles, 0, _graphAiThrottles.Length);

                if (_pilotSlots.Count > 0 && _currentIndex >= 0 && _currentIndex < _graphAiAngles.Length)
                {
                    var slot0 = _pilotSlots[0];
                    string fname0 = _currentIndex < _graphImagePaths.Count ? Path.GetFileName(_graphImagePaths[_currentIndex]) : "";
                    string ck0 = $"{fname0}_{_graphBrightness}_{_graphBlur}";
                    if (slot0.Cache.TryGetValue(ck0, out var cur))
                    {
                        _graphAiAngles[_currentIndex] = cur.angle;
                        _graphAiThrottles[_currentIndex] = cur.throttle;
                    }
                }
                _graphDrawPanel?.Invalidate();
                RefreshAllSlots();

                _bgTaskCts?.Cancel();
                _bgTaskCts = new CancellationTokenSource();
                _ = FillGraphFromSlotCacheAsync(_graphImagePaths, _bgTaskCts.Token, _graphBrightness, _graphBlur);
            };

            _btnResetZoom.Click += (s, e) => { _graphZoom = 1.0; _graphZoomY = 1.0; _graphScrollX = 0; _graphOffsetY = 0.0; _graphDrawPanel?.Invalidate(); };

            _graphDrawPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 244, 240) };
            typeof(Panel).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(_graphDrawPanel, true);
            _graphDrawPanel.Paint += GraphDrawPanel_Paint;
            _graphDrawPanel.Resize += (s, e) => _graphDrawPanel.Invalidate();

            _graphDrawPanel.MouseWheel += (s, e) =>
            {
                if (e is HandledMouseEventArgs hme) hme.Handled = true;
                if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt) return;
                bool isCtrl = (Control.ModifierKeys & Keys.Control) == Keys.Control;
                bool isShift = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
                if (isCtrl)
                {
                    _graphZoomY = e.Delta > 0 ? Math.Min(_graphZoomY * 1.2, 50.0) : Math.Max(_graphZoomY / 1.2, 1.0);
                    var (rawMin, rawMax) = GetCurrentGraphRange();
                    double yMargin = (rawMax - rawMin) * 0.1; if (yMargin < 0.05) yMargin = 0.05;
                    double maxOffsetY = ((rawMax - rawMin) / 2.0 + yMargin) * (1.0 - 1.0 / _graphZoomY); if (maxOffsetY < 0) maxOffsetY = 0;
                    _graphOffsetY = Math.Clamp(_graphOffsetY, -maxOffsetY, maxOffsetY);
                    _graphDrawPanel.Invalidate();
                }
                else if (isShift)
                {
                    double oldZoom = _graphZoom;
                    _graphZoom = e.Delta > 0 ? Math.Min(_graphZoom * 1.2, 200.0) : Math.Max(_graphZoom / 1.2, 1.0);
                    int gw2 = _graphDrawPanel.Width - 62;
                    double ratio = (double)(e.X - 52 + _graphScrollX) / Math.Max(gw2 * oldZoom, 1);
                    _graphScrollX = Math.Max(0, Math.Min((int)(ratio * gw2 * _graphZoom - (e.X - 52)), GetGraphMaxScrollX()));
                    _graphDrawPanel.Invalidate();
                }
            };
            _graphDrawPanel.MouseEnter += (s, e) => _graphDrawPanel.Focus();
            _graphDrawPanel.MouseDown += (s, e) => { if (e.Button != MouseButtons.Left) return; _graphDragging = true; _graphDragStartX = e.X; _graphDragStartY = e.Y; _graphDragOffY = _graphOffsetY; _dragAxisDecided = false; _dragIsHorizontal = false; };
            _graphDrawPanel.MouseMove += (s, e) =>
            {
                if (!_graphDragging) return;
                int dx = e.X - _graphDragStartX, dy = e.Y - _graphDragStartY;
                if (!_dragAxisDecided && (Math.Abs(dx) > 5 || Math.Abs(dy) > 5)) { _dragIsHorizontal = Math.Abs(dx) >= Math.Abs(dy); _dragAxisDecided = true; _graphDragStartX = e.X; _graphDragStartY = e.Y; _graphDragOffY = _graphOffsetY; }
                if (!_dragAxisDecided) return;
                if (_dragIsHorizontal) { int delta = e.X - _graphDragStartX; _graphDragStartX = e.X; _graphScrollX = Math.Max(0, Math.Min(_graphScrollX - delta, GetGraphMaxScrollX())); }
                else
                {
                    int gh2 = _graphDrawPanel.Height - 38;
                    var (rawMin, rawMax) = GetCurrentGraphRange();
                    double yMargin = (rawMax - rawMin) * 0.1; if (yMargin < 0.05) yMargin = 0.05;
                    double yRange = ((rawMax - rawMin) + yMargin * 2) / _graphZoomY; if (yRange < 0.0001) yRange = 1.0;
                    _graphOffsetY = _graphDragOffY + (e.Y - _graphDragStartY) * (yRange / Math.Max(gh2, 1));
                    double maxOffsetY = ((rawMax - rawMin) / 2.0 + yMargin) * (1.0 - 1.0 / _graphZoomY); if (maxOffsetY < 0) maxOffsetY = 0;
                    _graphOffsetY = Math.Clamp(_graphOffsetY, -maxOffsetY, maxOffsetY);
                }
                _graphDrawPanel.Invalidate();
            };
            _graphDrawPanel.MouseUp += (s, e) => { _graphDragging = false; _dragAxisDecided = false; };
            _graphDrawPanel.MouseLeave += (s, e) => { _graphDragging = false; _dragAxisDecided = false; };

            pnlGraphArea.Controls.Add(_graphDrawPanel);
            pnlGraphArea.Controls.Add(btnRow);
            _btnGraphError.PerformClick();
        }

        private Button MakeGraphBtn(string text)
        {
            var btn = new Button { Text = text, Width = 58, Height = 26, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(60, 60, 60), Font = new Font("맑은 고딕", 8.5f), Margin = new Padding(0, 0, 4, 0) };
            btn.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200); return btn;
        }

        private void SelectGraphBtn(Button selected)
        {
            foreach (var b in new[] { _btnGraphError, _btnGraphAngle, _btnGraphThrottle }) if (b != null) b.BackColor = Color.White;
            if (selected != null) selected.BackColor = Color.FromArgb(210, 228, 255);
        }

        private int GetGraphMaxScrollX()
        {
            int n = _graphHumanAngles.Count; if (n < 2) return 0;
            int gw = _graphDrawPanel.Width - 62;
            return Math.Max(0, (int)(gw * _graphZoom) - gw);
        }

        private (double min, double max) GetCurrentGraphRange()
        {
            int n = _graphHumanAngles.Count; if (n == 0) return (-1, 1);
            double rawMin = 0, rawMax = 0; bool first = true;
            Action<double> update = (v) => { if (first) { rawMin = rawMax = v; first = false; } else { rawMin = Math.Min(rawMin, v); rawMax = Math.Max(rawMax, v); } };

            for (int i = 0; i < n; i++)
            {
                switch (_graphMode)
                {
                    case GraphMode.Angle:
                        update(_graphHumanAngles[i]);
                        if (i < _graphAiAngles.Length && _graphAiAngles[i].HasValue) update(_graphAiAngles[i].Value);
                        break;
                    case GraphMode.Throttle:
                        update(_graphHumanThrottles[i]);
                        if (i < _graphAiThrottles.Length && _graphAiThrottles[i].HasValue) update(_graphAiThrottles[i].Value);
                        break;
                    case GraphMode.ErrorTotal:
                        update(0.0);
                        if (i < _graphAiAngles.Length && _graphAiAngles[i].HasValue &&
                            i < _graphAiThrottles.Length && _graphAiThrottles[i].HasValue)
                            update(((Math.Abs(_graphAiAngles[i].Value - _graphHumanAngles[i])
                                   + Math.Abs(_graphAiThrottles[i].Value - _graphHumanThrottles[i])) / 2.0) * 100.0);
                        else update(200.0);
                        break;
                    case GraphMode.ErrorAngle:
                        update(0.0);
                        if (i < _graphAiAngles.Length && _graphAiAngles[i].HasValue)
                            update((_graphAiAngles[i].Value - _graphHumanAngles[i]) * 100.0);
                        else update(200.0);
                        break;
                    case GraphMode.ErrorThrottle:
                        update(0.0);
                        if (i < _graphAiThrottles.Length && _graphAiThrottles[i].HasValue)
                            update((_graphAiThrottles[i].Value - _graphHumanThrottles[i]) * 100.0);
                        else update(200.0);
                        break;
                }
            }
            if (first) return (-1, 1);
            return (rawMin, rawMax);
        }

        private void GraphDrawPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(245, 244, 240));

            int n = _graphHumanAngles.Count;
            if (n < 2) { g.DrawString("데이터 로딩 중...", new Font("맑은 고딕", 9f), new SolidBrush(Color.Gray), 10, 10); return; }

            double[] human = new double[n]; double?[] ai = new double?[n];
            switch (_graphMode)
            {
                case GraphMode.Angle: human = _graphHumanAngles.ToArray(); ai = _graphAiAngles; break;
                case GraphMode.Throttle: human = _graphHumanThrottles.ToArray(); ai = _graphAiThrottles; break;
                case GraphMode.ErrorTotal: for (int i = 0; i < n; i++) { human[i] = 0.0; ai[i] = (i < _graphAiAngles.Length && _graphAiAngles[i].HasValue && i < _graphAiThrottles.Length && _graphAiThrottles[i].HasValue) ? (double?)(((Math.Abs(_graphAiAngles[i].Value - _graphHumanAngles[i]) + Math.Abs(_graphAiThrottles[i].Value - _graphHumanThrottles[i])) / 2.0) * 100.0) : null; } break;
                case GraphMode.ErrorAngle: for (int i = 0; i < n; i++) { human[i] = 0.0; ai[i] = (i < _graphAiAngles.Length && _graphAiAngles[i].HasValue) ? (double?)((_graphAiAngles[i].Value - _graphHumanAngles[i]) * 100.0) : null; } break;
                case GraphMode.ErrorThrottle: for (int i = 0; i < n; i++) { human[i] = 0.0; ai[i] = (i < _graphAiThrottles.Length && _graphAiThrottles[i].HasValue) ? (double?)((_graphAiThrottles[i].Value - _graphHumanThrottles[i]) * 100.0) : null; } break;
            }

            int panelW = _graphDrawPanel.Width, panelH = _graphDrawPanel.Height;
            const int padL = 52, padR = 10, padT = 10, padB = 28;
            int gw = panelW - padL - padR, gh = panelH - padT - padB;
            int totalW = (int)(gw * _graphZoom);

            var (rawMin, rawMax) = GetCurrentGraphRange();
            double yMargin = (rawMax - rawMin) * 0.1; if (yMargin < 0.05) yMargin = 0.05;
            double yCenter = (rawMax + rawMin) / 2.0 + _graphOffsetY;
            double yHalf = ((rawMax - rawMin) / 2.0 + yMargin) / _graphZoomY;
            double allMin = yCenter - yHalf, allMax = yCenter + yHalf;
            double range = allMax - allMin; if (range < 0.0001) range = 1.0;

            float toX(int i) => padL + (float)i / Math.Max(n - 1, 1) * totalW - _graphScrollX;
            float toY(double v) => padT + (float)((allMax - v) / range * gh);

            var humanColor = _lblLegendHuman.ForeColor;
            var aiColor = _lblLegendAi.ForeColor;
            var labelFont = new Font("맑은 고딕", 7.5f);
            var labelBrush = new SolidBrush(Color.FromArgb(130, 0, 0, 0));
            using var gridPen = new Pen(Color.FromArgb(35, 0, 0, 0), 0.5f);
            using var gridPen2 = new Pen(Color.FromArgb(15, 0, 0, 0), 0.5f);

            double[] yIntervalCandidates = { 0.1, 0.2, 0.5, 1.0, 2.0, 5.0, 10.0, 20.0, 50.0, 100.0 };
            double yInterval = 5.0;
            foreach (var iv in yIntervalCandidates) { if ((float)(iv / range * gh) >= 20f) { yInterval = iv; break; } }
            bool isErrorMode = _graphMode == GraphMode.ErrorTotal || _graphMode == GraphMode.ErrorAngle || _graphMode == GraphMode.ErrorThrottle;

            double yStart = Math.Ceiling(allMin / yInterval) * yInterval;
            for (double val = yStart; val <= allMax + yInterval * 0.5; val += yInterval)
            {
                float fy = toY(val); if (fy < padT || fy > padT + gh) continue;
                g.SetClip(new Rectangle(padL, padT, gw, gh)); g.DrawLine(Math.Abs(val) < yInterval * 0.01 ? gridPen : gridPen2, padL, fy, padL + gw, fy);
                g.SetClip(new Rectangle(0, padT, padL, gh));
                g.DrawString(isErrorMode ? val.ToString("0.#") + "%" : val.ToString("0.###"), labelFont, labelBrush, 2, fy - 7);
            }
            g.ResetClip();

            g.SetClip(new Rectangle(padL, 0, gw + padR, panelH));
            float pxPerPoint = (float)totalW / Math.Max(n - 1, 1);
            int[] xCandidates = { 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 2000, 5000 };
            int xInterval = xCandidates.Last();
            foreach (var iv in xCandidates) { if (iv * pxPerPoint >= 30f) { xInterval = iv; break; } }
            if (xInterval < 1) xInterval = 1;
            if (xInterval < n)
            {
                for (int idx = 0; idx < n; idx += xInterval)
                {
                    float fx = toX(idx); if (fx < padL || fx > padL + gw) continue;
                    g.SetClip(new Rectangle(padL, padT, gw, gh)); g.DrawLine(gridPen2, fx, padT, fx, padT + gh);
                    g.SetClip(new Rectangle(padL, padT + gh, gw, padB));
                    g.DrawString(idx.ToString(), labelFont, labelBrush, fx - 10, padT + gh + 4);
                }
            }
            g.ResetClip();

            g.SetClip(new Rectangle(padL, padT, gw, gh));
            int startIdx = Math.Max(0, (int)((double)_graphScrollX / Math.Max(totalW, 1) * n) - 1);
            int endIdx = Math.Min(n - 1, (int)((double)(_graphScrollX + gw) / Math.Max(totalW, 1) * n) + 1);

            using var hp = new Pen(humanColor, 2f);
            var humanPts = new List<PointF>();
            for (int i = startIdx; i <= endIdx; i++) humanPts.Add(new PointF(toX(i), toY(human[i])));
            if (humanPts.Count >= 2) g.DrawLines(hp, humanPts.ToArray());

            using var ap = new Pen(aiColor, 2f);
            var aiSeg = new List<PointF>();
            for (int i = startIdx; i <= endIdx + 1; i++)
            {
                int ci = Math.Min(i, n - 1); bool has = ci < ai.Length && ai[ci].HasValue;
                if (has) aiSeg.Add(new PointF(toX(ci), toY(ai[ci]!.Value)));
                else { if (aiSeg.Count >= 2) g.DrawLines(ap, aiSeg.ToArray()); aiSeg.Clear(); }
            }
            if (aiSeg.Count >= 2) g.DrawLines(ap, aiSeg.ToArray());

            int phIdx = _currentIndex;
            if (phIdx >= 0 && phIdx < n)
            {
                float px = toX(phIdx);
                if (px >= padL && px <= padL + gw)
                {
                    g.ResetClip();
                    using var indPen = new Pen(Color.FromArgb(50, 50, 50), 2.5f);
                    g.DrawLine(indPen, px, padT, px, padT + gh);
                    float py = toY(human[phIdx]);
                    g.FillEllipse(new SolidBrush(humanColor), px - 5, py - 5, 10, 10);
                    g.DrawEllipse(new Pen(Color.White, 1.5f), px - 5, py - 5, 10, 10);
                    if (phIdx < ai.Length && ai[phIdx].HasValue)
                    {
                        float py2 = toY(ai[phIdx]!.Value);
                        g.FillEllipse(new SolidBrush(aiColor), px - 5, py2 - 5, 10, 10);
                        g.DrawEllipse(new Pen(Color.White, 1.5f), px - 5, py2 - 5, 10, 10);
                    }
                }
            }

            g.ResetClip();
            using var axisPen = new Pen(Color.FromArgb(80, 0, 0, 0), 1f);
            g.DrawLine(axisPen, padL, padT, padL, padT + gh);
            g.DrawLine(axisPen, padL, padT + gh, padL + gw, padT + gh);

            string zoomInfo = "";
            if (_graphZoom > 1.01) zoomInfo += $"X x{_graphZoom:0.0}  ";
            if (_graphZoomY > 1.01) zoomInfo += $"Y x{_graphZoomY:0.0}";
            if (zoomInfo.Length > 0)
                g.DrawString(zoomInfo.Trim(), new Font("맑은 고딕", 8f), new SolidBrush(Color.FromArgb(120, 0, 0, 0)), panelW - 80, padT);
        }

        // ════════════════════════════════════════════════════════════
        // [파일1 추가] 그래프 데이터 로드 (catalog 파일 파싱)
        // ════════════════════════════════════════════════════════════
        private async Task LoadGraphDataAsync(string tubFolder, List<string> validImagePaths)
        {
            if (string.IsNullOrEmpty(tubFolder) || validImagePaths.Count == 0) return;
            _bgTaskCts.Cancel();
            _bgTaskCts = new CancellationTokenSource();
            var token = _bgTaskCts.Token;

            var angleDict = new ConcurrentDictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            var throttleDict = new ConcurrentDictionary<string, double>(StringComparer.OrdinalIgnoreCase);

            await Task.Run(async () =>
            {
                try
                {
                    var p1 = Directory.GetFiles(tubFolder, "*.catalog", SearchOption.TopDirectoryOnly);
                    var p2 = Directory.GetFiles(tubFolder, "catalog_*", SearchOption.TopDirectoryOnly);
                    var catalogFiles = p1.Union(p2).Distinct()
                        .Where(f => !Directory.Exists(f))
                        .OrderBy(p =>
                        {
                            var fn = Path.GetFileNameWithoutExtension(p);
                            var m = System.Text.RegularExpressions.Regex.Match(fn, @"\d+");
                            return m.Success ? int.Parse(m.Value) : int.MaxValue;
                        }).ToList();

                    foreach (var cf in catalogFiles)
                    {
                        if (token.IsCancellationRequested) break;
                        var text = await File.ReadAllTextAsync(cf);
                        var matches = System.Text.RegularExpressions.Regex.Matches(text, @"\{[^{}]*\}");
                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            var line = match.Value.Trim();
                            if (string.IsNullOrWhiteSpace(line)) continue;
                            try
                            {
                                using var doc = System.Text.Json.JsonDocument.Parse(line);
                                var root = doc.RootElement;

                                System.Text.Json.JsonElement imgEl;
                                if (!root.TryGetProperty("cam/image_array", out imgEl) &&
                                    !root.TryGetProperty("cam_image_array", out imgEl)) continue;
                                if (!root.TryGetProperty("user/angle", out var angEl)) continue;
                                if (!root.TryGetProperty("user/throttle", out var thrEl)) continue;

                                string imgName = Path.GetFileName(imgEl.GetString() ?? "");
                                if (string.IsNullOrEmpty(imgName)) continue;

                                if (angEl.TryGetDouble(out double angV)) angleDict[imgName] = angV;
                                if (thrEl.TryGetDouble(out double thrV)) throttleDict[imgName] = thrV;
                            }
                            catch { }
                        }
                    }
                }
                catch { }
            }, token);

            if (token.IsCancellationRequested) return;

            var imagePaths = new List<string>(); var humanAngles = new List<double>(); var humanThrottles = new List<double>();
            foreach (var path in validImagePaths)
            {
                string fname = Path.GetFileName(path);
                imagePaths.Add(path);
                humanAngles.Add(angleDict.TryGetValue(fname, out double ang) ? ang : 0.0);
                humanThrottles.Add(throttleDict.TryGetValue(fname, out double thr) ? thr : 0.0);
            }
            _graphImagePaths = imagePaths;
            _graphHumanAngles = humanAngles;
            _graphHumanThrottles = humanThrottles;
            _graphAiAngles = new double?[imagePaths.Count];
            _graphAiThrottles = new double?[imagePaths.Count];

            if (!this.IsDisposed && this.IsHandleCreated)
                this.BeginInvoke(() => _graphDrawPanel?.Invalidate());
        }

        // ════════════════════════════════════════════════════════════
        // [파일1 추가] 슬롯 캐시로 그래프 전체 채우기
        // ════════════════════════════════════════════════════════════
        private async Task FillGraphFromSlotCacheAsync(List<string> imagePaths, CancellationToken token, int brightness, int blur)
        {
            if (_pilotSlots.Count == 0) return;
            var slot = _pilotSlots[0];
            int curIdx = _currentIndex;

            if (curIdx >= 0 && curIdx < imagePaths.Count && !token.IsCancellationRequested)
            {
                string fname0 = Path.GetFileName(imagePaths[curIdx]);
                string ck0 = $"{fname0}_{brightness}_{blur}";
                if (!slot.Cache.ContainsKey(ck0) && !string.IsNullOrEmpty(slot.ModelFileName))
                {
                    (double angle, double throttle)? r0 = null;
                    if (brightness == 0 && blur == 0) r0 = await RequestPrediction(slot, ConvertToWslPath(imagePaths[curIdx]));
                    else { using var b0 = GetFilteredBitmap(imagePaths[curIdx], brightness, blur); if (b0 != null) r0 = await RequestPrediction(slot, b0); }
                    if (r0.HasValue) { slot.Cache[ck0] = r0.Value; slot.LastAiAngle = r0.Value.angle; slot.LastAiThrottle = r0.Value.throttle; }
                }
                if (slot.Cache.TryGetValue(ck0, out var c0))
                {
                    _graphAiAngles[curIdx] = c0.angle;
                    _graphAiThrottles[curIdx] = c0.throttle;
                }
                if (!this.IsDisposed && this.IsHandleCreated)
                    this.BeginInvoke(() => { RefreshAllSlots(); _graphDrawPanel?.Invalidate(); });
            }

            await Task.Run(async () =>
            {
                for (int i = 0; i < imagePaths.Count; i++)
                {
                    if (token.IsCancellationRequested) break;
                    if (i == curIdx) continue;

                    string fname = Path.GetFileName(imagePaths[i]);
                    string cacheKey = $"{fname}_{brightness}_{blur}";

                    if (!slot.Cache.ContainsKey(cacheKey) && !string.IsNullOrEmpty(slot.ModelFileName))
                    {
                        (double angle, double throttle)? result = null;
                        if (brightness == 0 && blur == 0)
                            result = await RequestPrediction(slot, ConvertToWslPath(imagePaths[i]));
                        else
                        {
                            using var bmp = GetFilteredBitmap(imagePaths[i], brightness, blur);
                            if (bmp != null) result = await RequestPrediction(slot, bmp);
                        }

                        if (result.HasValue)
                        {
                            slot.Cache[cacheKey] = result.Value;
                            slot.LastAiAngle = result.Value.angle;
                            slot.LastAiThrottle = result.Value.throttle;
                        }
                    }

                    if (slot.Cache.TryGetValue(cacheKey, out var cached))
                    {
                        _graphAiAngles[i] = cached.angle;
                        _graphAiThrottles[i] = cached.throttle;
                    }

                    if (i > 0 && i % 50 == 0 && !token.IsCancellationRequested)
                    {
                        if (!this.IsDisposed && this.IsHandleCreated)
                            this.BeginInvoke(() => _graphDrawPanel?.Invalidate());
                        await Task.Delay(15, token);
                    }
                }
            }, token);

            if (!token.IsCancellationRequested && !this.IsDisposed && this.IsHandleCreated)
            {
                this.BeginInvoke(() =>
                {
                    RefreshAllSlots();
                    int n = _graphHumanAngles.Count;
                    if (n >= 2 && _graphDrawPanel != null)
                    {
                        int gw = _graphDrawPanel.Width - 62;
                        int totalW = (int)(gw * _graphZoom);
                        float px = (float)_currentIndex / Math.Max(n - 1, 1) * totalW;
                        if (px - _graphScrollX < 0 || px - _graphScrollX > gw)
                            _graphScrollX = Math.Max(0, Math.Min((int)(px - gw / 2), GetGraphMaxScrollX()));
                    }
                    _graphDrawPanel?.Invalidate();
                    // ★ 그래프 완료 후 순위 갱신
                    UpdateRankings();
                });
            }
        }

        // ════════════════════════════════════════════════════════════
        // 순위 컨트롤 초기화
        // ════════════════════════════════════════════════════════════
        private void SetupRankControls()
        {
            flpRankControls.BackColor = Color.FromArgb(244, 243, 238);
            flpRankControls.Padding = new Padding(6, 0, 0, 0);
            flpRankControls.Height = 50;
            flpRankControls.WrapContents = false;
            flpRankControls.FlowDirection = FlowDirection.LeftToRight;
            flpRankControls.Controls.Clear();

            // ── 헤더 라벨 ──
            Label lblHeader = new Label
            {
                Text = "오차율 순위",
                Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 95, 168),
                AutoSize = true,
                Margin = new Padding(4, 12, 4, 0)
            };

            // ── 분류 선택 콤보박스 1개 ──
            var cmbRankType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 80,
                Font = new Font("맑은 고딕", 8.5f),
                Margin = new Padding(0, 10, 8, 0)
            };
            cmbRankType.Items.AddRange(new object[] { "종합", "각도", "속도" });
            cmbRankType.SelectedIndex = 0;

            // ── 순위 패널 (1~3위를 가로로 표시) ──
            var pnlRanks = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0, 6, 0, 0)
            };

            // 순위 라벨 3개 생성
            var rankLabels = new Label[3];
            Color[] medalColors = 
            {
                Color.FromArgb(212, 175, 55),  // 1위 금
                Color.FromArgb(160, 160, 160), // 2위 은
                Color.FromArgb(176, 101, 48)   // 3위 동
            };

            string[] medals = { "🥇", "🥈", "🥉" };

            for (int i = 0; i < 3; i++)
            {
                var lbl = new Label
                {
                    Text = $"{i + 1}위  -",
                    Font = new Font("맑은 고딕", 8.5f, FontStyle.Bold),
                    ForeColor = medalColors[i],
                    AutoSize = false,
                    Width = 260,
                    Height = 36,
                    TextAlign = ContentAlignment.MiddleLeft,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 4, 0),
                    Padding = new Padding(6, 0, 0, 0)
                };
                rankLabels[i] = lbl;
                pnlRanks.Controls.Add(lbl);
            }

            // ── 콤보 변경 시 순위 갱신 ──
            Action refreshRanks = () =>
            {
                ComboBox srcCombo = cmbRankType.SelectedIndex switch
                {
                    1 => cmbRankAngle,
                    2 => cmbRankThrottle,
                    _ => cmbRankOverall
                };

                string[] medals = { "🥇", "🥈", "🥉" };
                for (int i = 0; i < 3; i++)
                {
                    if (srcCombo.Items.Count > i)
                    {
                        string item = srcCombo.Items[i]?.ToString() ?? "-";
                        rankLabels[i].Text = $"{medals[i]} {item}";
                        rankLabels[i].ForeColor = medalColors[i];
                    }
                    else
                    {
                        rankLabels[i].Text = $"{medals[i]} {i + 1}위  -";
                        rankLabels[i].ForeColor = Color.FromArgb(180, 180, 180);
                    }
                }
            };

            cmbRankType.SelectedIndexChanged += (s, e) => refreshRanks();

            // 기존 숨겨진 콤보박스들(cmbRankOverall 등)의 아이템이 바뀔 때도 갱신
            cmbRankOverall.SelectedIndexChanged += (s, e) => refreshRanks();
            cmbRankAngle.SelectedIndexChanged += (s, e) => refreshRanks();
            cmbRankThrottle.SelectedIndexChanged += (s, e) => refreshRanks();

            flpRankControls.Controls.Add(lblHeader);
            flpRankControls.Controls.Add(MakeSeparator());
            flpRankControls.Controls.Add(cmbRankType);
            flpRankControls.Controls.Add(pnlRanks);

            refreshRanks();
        }

        // ════════════════════════════════════════════════════════════
        // 오차율 순위 계산 및 갱신
        // ════════════════════════════════════════════════════════════
        private void UpdateRankings()
        {
            if (_graphHumanAngles.Count == 0) return;

            // 슬롯별 오차 계산
            var rankData = new List<(string modelName, double overall, double angle, double throttle)>();

            for (int s = 0; s < _pilotSlots.Count; s++)
            {
                var slot = _pilotSlots[s];
                if (string.IsNullOrEmpty(slot.ModelFileName)) continue;

                var angleErrs = new List<double>();
                var throttleErrs = new List<double>();

                for (int i = 0; i < _graphHumanAngles.Count; i++)
                {
                    bool hasAngle = i < _graphAiAngles.Length && _graphAiAngles[i].HasValue;
                    bool hasThrottle = i < _graphAiThrottles.Length && _graphAiThrottles[i].HasValue;

                    if (hasAngle)
                        angleErrs.Add(Math.Abs(_graphAiAngles[i]!.Value - _graphHumanAngles[i]) * 100.0);
                    if (hasThrottle)
                        throttleErrs.Add(Math.Abs(_graphAiThrottles[i]!.Value - _graphHumanThrottles[i]) * 100.0);
                }

                if (angleErrs.Count == 0 && throttleErrs.Count == 0) continue;

                double avgAngle = angleErrs.Count > 0 ? angleErrs.Average() : 0.0;
                double avgThrottle = throttleErrs.Count > 0 ? throttleErrs.Average() : 0.0;
                double avgOverall = (avgAngle + avgThrottle) / 2.0;

                rankData.Add((slot.ModelFileName, avgOverall, avgAngle, avgThrottle));
            }

            if (rankData.Count == 0) return;

            // 정렬 (오차 낮은 순)
            var sortedOverall = rankData.OrderBy(r => r.overall).ToList();
            var sortedAngle = rankData.OrderBy(r => r.angle).ToList();
            var sortedThrottle = rankData.OrderBy(r => r.throttle).ToList();

            // 콤보박스 갱신
            void FillCombo(ComboBox cmb, List<(string modelName, double overall, double angle, double throttle)> sorted, Func<(string modelName, double overall, double angle, double throttle), double> getVal)
            {
                cmb.Items.Clear();
                foreach (var item in sorted)
                    cmb.Items.Add($"{item.modelName}  ({getVal(item):F2}%)");
                if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
            }

            FillCombo(cmbRankOverall, sortedOverall, r => r.overall);
            FillCombo(cmbRankAngle, sortedAngle, r => r.angle);
            FillCombo(cmbRankThrottle, sortedThrottle, r => r.throttle);
        }

        private Panel MakeComboGroup(string labelText, Color labelColor, ComboBox comboBox)
        {
            Panel pnl = new Panel { Width = 155, Height = 36, Margin = new Padding(0, 0, 4, 0), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            Label lbl = new Label { Text = labelText, Font = new Font("맑은 고딕", 8f, FontStyle.Bold), ForeColor = labelColor, Location = new Point(6, 10), AutoSize = true };
            comboBox.Location = new Point(40, 7); comboBox.Width = 108; comboBox.Font = new Font("맑은 고딕", 8.5f); comboBox.FlatStyle = FlatStyle.Flat; comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            pnl.Controls.Add(lbl); pnl.Controls.Add(comboBox); return pnl;
        }

        private Panel MakeSeparator() => new Panel { Width = 1, Height = 22, Margin = new Padding(2, 7, 2, 0), BackColor = Color.FromArgb(180, 180, 180) };

        private void btnTubPlot_Click(object sender, EventArgs e)
        {
            BtnTubPlot_Click(sender, e);
        }
    }
}