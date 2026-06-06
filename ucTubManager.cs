using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DonkeyUi
{
    public partial class ucTubManager : UserControl
    {
        public static string CurrentTubPath = "";

        // ════════════════════════════════════════════════════════════
        // PilotArena 연동용 정적 멤버 (추가)
        // ════════════════════════════════════════════════════════════
        public static string LastImagePath { get; private set; } = "";
        public static double? LastAngle { get; private set; } = null;
        public static double? LastThrottle { get; private set; } = null;

        // PilotArena가 구독할 이벤트
        // (imagePath, angle, throttle, currentIndex, totalCount)
        public static event Action<string, double?, double?, int, int> OnTubDataChanged;

        // ── 게이지 상태 ──────────────────────────────────────────────
        private double? _currentThrottle = null;
        private double? _currentAngle = null;

        private List<string> _imageFiles = new List<string>();
        private List<string> _deletedImages = new List<string>();
        private List<string> _allImageFiles = new List<string>();
        private Dictionary<string, int> _deletedIndexes = new Dictionary<string, int>();
        private int _currentIndex = -1;
        private int _leftFrame = -1;
        private int _rightFrame = -1;
        private int _minX = 20;
        private int _maxX = 960;
        private bool _dragMin = false;
        private bool _dragMax = false;
        private const double SPEED_MAX = 200.0;
        private int _angleMinX = 20;
        private int _angleMaxX = 815;
        private List<string> _filteredImages = new();
        private bool _dragAngleMin = false;
        private bool _dragAngleMax = false;
        private int _selectStart = -1;
        private int _selectEnd = -1;
        private const int ANGLE_MAX = 10;
        private System.Windows.Forms.Timer _prevTimer;
        private System.Windows.Forms.Timer _nextTimer;
        private System.Windows.Forms.Timer _prevInitialTimer;
        private System.Windows.Forms.Timer _nextInitialTimer;
        private System.Windows.Forms.Timer _fastPrevTimer;
        private System.Windows.Forms.Timer _fastNextTimer;
        private System.Windows.Forms.Timer _fastPrevInitialTimer;
        private System.Windows.Forms.Timer _fastNextInitialTimer;
        private System.Windows.Forms.Timer _playTimer;
        private bool _isPlaying = false;
        private double _playBaseIntervalMs = 100.0;
        private List<string> _catalogLines = new List<string>();
        private string _loadedFolderPath = "";

        private List<(int Start, int End)> _ranges = new();
        private List<(int Start, int End)> _deletedRanges = new();
        private List<int> _deletedSingleFrames = new();
        private List<(double Min, double Max)> _speedFilters = new();
        private List<(double Min, double Max)> _angleFilters = new();



        // If catalogs contain explicit _index fields, store the maximum index found
        private int? _maxCatalogIndex = null;
        // Map filename (e.g. "4131_cam_image_array_.jpg") -> catalog _index
        private Dictionary<string, int> _catalogIndexByFileName = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);


        public ucTubManager()
        {
            InitializeComponent();

            btnLoadCarDirectory.Click += BtnLoadCarDirectory_Click;
            btnLoadTub.Click += BtnLoadTub_Click;
            trkRecord.Scroll += TrkRecord_Scroll;
            trkRecord.ValueChanged += TrkRecord_ValueChanged;
            // Allow user input to jump to a specific record
            txtRecordNumber.KeyDown += TxtRecordNumber_KeyDown;
            txtRecordNumber.Leave += TxtRecordNumber_Leave;

            pnlSpeedRange.Paint += pnlSpeedRange_Paint;

            pnlSpeedRange.MouseDown += pnlSpeedRange_MouseDown;
            pnlSpeedRange.MouseMove += pnlSpeedRange_MouseMove;
            pnlSpeedRange.MouseUp += pnlSpeedRange_MouseUp;

            nudSpeedMin.ValueChanged += nudSpeedMin_ValueChanged;
            nudSpeedMax.ValueChanged += nudSpeedMax_ValueChanged;

            pnlAngleRange.Paint += pnlAngleRange_Paint;

            pnlAngleRange.MouseDown += pnlAngleRange_MouseDown;
            pnlAngleRange.MouseMove += pnlAngleRange_MouseMove;
            pnlAngleRange.MouseUp += pnlAngleRange_MouseUp;

            nudAngleMin.ValueChanged += nudAngleMin_ValueChanged;
            nudAngleMax.ValueChanged += nudAngleMax_ValueChanged;

            nudSpeedMin.DecimalPlaces = 3;
            nudSpeedMax.DecimalPlaces = 3;

            nudSpeedMin.Increment = 0.01M;
            nudSpeedMax.Increment = 0.01M;

            nudSpeedMin.Maximum = 1;
            nudSpeedMax.Maximum = 1;


            nudAngleMin.Minimum = -1;
            nudAngleMin.Maximum = 1;
            nudAngleMin.DecimalPlaces = 3;
            nudAngleMin.Increment = 0.001M;
            nudAngleMin.Value = -1;

            nudAngleMax.Minimum = -1;
            nudAngleMax.Maximum = 1;
            nudAngleMax.DecimalPlaces = 3;
            nudAngleMax.Increment = 0.001M;
            nudAngleMax.Value = 1;

            _angleMinX = AngleToX(nudAngleMin.Value);
            _angleMaxX = AngleToX(nudAngleMax.Value);

            // Recalculate handle positions when the panels resize so the full panel width is used
            pnlSpeedRange.SizeChanged += (s, e) =>
            {
                _minX = SpeedToX(nudSpeedMin.Value);
                _maxX = SpeedToX(nudSpeedMax.Value);
                pnlSpeedRange.Invalidate();
            };

            pnlAngleRange.SizeChanged += (s, e) =>
            {
                _angleMinX = AngleToX(nudAngleMin.Value);
                _angleMaxX = AngleToX(nudAngleMax.Value);
                pnlAngleRange.Invalidate();
            };


            pnlTimeline.Paint += pnlTimeline_Paint;
            _prevTimer = new System.Windows.Forms.Timer { Interval = 100 };
            _nextTimer = new System.Windows.Forms.Timer { Interval = 100 };
            _prevTimer.Tick += (s, ea) => MovePrev();
            _nextTimer.Tick += (s, ea) => MoveNext();

            _prevInitialTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _nextInitialTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _prevInitialTimer.Tick += (s, ea) => { _prevInitialTimer.Stop(); _prevTimer.Start(); };
            _nextInitialTimer.Tick += (s, ea) => { _nextInitialTimer.Stop(); _nextTimer.Start(); };

            btnPrev.MouseDown += (s, ea) => { if (ea.Button == MouseButtons.Left) { MovePrev(); _prevInitialTimer.Start(); } };
            btnPrev.MouseUp += (s, ea) => { if (ea.Button == MouseButtons.Left) { _prevInitialTimer.Stop(); _prevTimer.Stop(); } };
            btnPrev.MouseLeave += (s, ea) => { _prevInitialTimer.Stop(); _prevTimer.Stop(); };

            btnNext.MouseDown += (s, ea) => { if (ea.Button == MouseButtons.Left) { MoveNext(); _nextInitialTimer.Start(); } };
            btnNext.MouseUp += (s, ea) => { if (ea.Button == MouseButtons.Left) { _nextInitialTimer.Stop(); _nextTimer.Stop(); } };
            btnNext.MouseLeave += (s, ea) => { _nextInitialTimer.Stop(); _nextTimer.Stop(); };

            _fastPrevTimer = new System.Windows.Forms.Timer { Interval = 100 };
            _fastNextTimer = new System.Windows.Forms.Timer { Interval = 100 };
            _fastPrevTimer.Tick += (s, ea) => MoveFastPrev();
            _fastNextTimer.Tick += (s, ea) => MoveFastNext();

            _fastPrevInitialTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _fastNextInitialTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _fastPrevInitialTimer.Tick += (s, ea) => { _fastPrevInitialTimer.Stop(); _fastPrevTimer.Start(); };
            _fastNextInitialTimer.Tick += (s, ea) => { _fastNextInitialTimer.Stop(); _fastNextTimer.Start(); };

            btnFastPrev.MouseDown += (s, ea) => { if (ea.Button == MouseButtons.Left) { MoveFastPrev(); _fastPrevInitialTimer.Start(); } };
            btnFastPrev.MouseUp += (s, ea) => { if (ea.Button == MouseButtons.Left) { _fastPrevInitialTimer.Stop(); _fastPrevTimer.Stop(); } };
            btnFastPrev.MouseLeave += (s, ea) => { _fastPrevInitialTimer.Stop(); _fastPrevTimer.Stop(); };

            btnFastNext.MouseDown += (s, ea) => { if (ea.Button == MouseButtons.Left) { MoveFastNext(); _fastNextInitialTimer.Start(); } };
            btnFastNext.MouseUp += (s, ea) => { if (ea.Button == MouseButtons.Left) { _fastNextInitialTimer.Stop(); _fastNextTimer.Stop(); } };
            btnFastNext.MouseLeave += (s, ea) => { _fastNextInitialTimer.Stop(); _fastNextTimer.Stop(); };

            _playTimer = new System.Windows.Forms.Timer { Interval = 100 };
            _playTimer.Tick += (s, ea) =>
            {
                if (_imageFiles.Count == 0) return;
                if (_currentIndex >= _imageFiles.Count - 1) { StopPlayback(); return; }
                MoveNext();
            };

            btnStartStop.Text = "시작 ▶️";
            btnStartStop.Click += BtnStartStop_Click;

            if (cmbSpeed.Items.Count == 0)
                cmbSpeed.Items.AddRange(new object[] { "0.25", "0.50", "0.75", "1.00", "1.25", "1.50", "1.75", "2.00" });
            cmbSpeed.Text = "1.00";
            cmbSpeed.SelectedIndexChanged += (s, e) => UpdatePlaybackIntervalFromCombo();
            cmbSpeed.TextChanged += (s, e) => UpdatePlaybackIntervalFromCombo();
            UpdatePlaybackIntervalFromCombo();

            picThrottle.Paint += PicThrottle_Paint;
            picAngle.Paint += PicAngle_Paint;
            // Subscribe to external timeline changes
            ucPilotArena.OnTimelineIndexChanged += (idx) =>
            {
                try
                {
                    SetExternalIndex(idx);
                }
                catch { }
            };
        }

        // Show image by absolute path and update internal selection if possible
        public void ShowImageAndSelectByPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ShowImageAndSelectByPath(path)));
                return;
            }
            try
            {
                string targetFull = string.Empty;
                try { targetFull = Path.GetFullPath(path); } catch { targetFull = path; }

                int idx = -1;
                for (int i = 0; i < _imageFiles.Count; i++)
                {
                    try
                    {
                        var f = Path.GetFullPath(_imageFiles[i]);
                        if (string.Equals(f, targetFull, StringComparison.OrdinalIgnoreCase)) { idx = i; break; }
                    }
                    catch { }
                }

                if (idx >= 0)
                {
                    // Move to that index
                    SetExternalIndex(idx);
                    return;
                }

                // Try match by filename only
                var fname = Path.GetFileName(path);
                if (!string.IsNullOrEmpty(fname))
                {
                    for (int i = 0; i < _imageFiles.Count; i++)
                    {
                        try { if (string.Equals(Path.GetFileName(_imageFiles[i]), fname, StringComparison.OrdinalIgnoreCase)) { idx = i; break; } }
                        catch { }
                    }
                }

                if (idx >= 0)
                {
                    SetExternalIndex(idx);
                    return;
                }

                // If not found in list, just display the image in picTubImage without changing index
                try
                {
                    using var fs = File.OpenRead(path);
                    using var img = Image.FromStream(fs);
                    picTubImage.Image?.Dispose();
                    picTubImage.Image = new Bitmap(img);
                    txtTub.Text = path;
                    txtRecordNumber.Text = "기록 000000";
                }
                catch { }
            }
            catch { }
        }

        // Allow external controls to request changing the current index (e.g., ucPilotArena)
        public void SetExternalIndex(int idx)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SetExternalIndex(idx)));
                return;
            }
            if (_imageFiles == null || _imageFiles.Count == 0) return;
            if (idx < 0) idx = 0;
            if (idx >= _imageFiles.Count) idx = _imageFiles.Count - 1;
            // If already at this index, do nothing to avoid visual bounce
            if (_currentIndex == idx) return;
            // Update trackbar if present
            try
            {
                if (trkRecord != null)
                {
                    // set value (this will normally trigger ValueChanged -> SetCurrentIndex)
                    trkRecord.Value = idx;
                }
            }
            catch { }

            // Ensure UI is updated immediately (in case trackbar is disabled or events suppressed)
            try { SetCurrentIndex(idx); } catch { }
        }

        // Return the image path at specified index or null
        public string? GetImagePathAt(int idx)
        {
            if (this.InvokeRequired)
            {
                return (string?)this.Invoke(new Func<int, string?>(GetImagePathAt), idx);
            }
            if (_imageFiles == null || _imageFiles.Count == 0) return null;
            if (idx < 0 || idx >= _imageFiles.Count) return null;
            return _imageFiles[idx];
        }

        private bool IsDeletedFrame(int originalIndex)
        {
            string image = _allImageFiles[originalIndex];
            return _deletedImages.Contains(image);
        }
        private void TxtRecordNumber_Leave(object? sender, EventArgs e)
        {
            TryNavigateToRecordFromText();
        }

        private void TxtRecordNumber_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                TryNavigateToRecordFromText();
            }
        }

        private void TryNavigateToRecordFromText()
        {
            try
            {
                var txt = txtRecordNumber.Text ?? string.Empty;

                var m = Regex.Match(txt, "(\\d+)");
                if (!m.Success) return;

                if (!int.TryParse(m.Groups[1].Value, out var num))
                    return;

                if (num <= 0)
                    num = 1;

                int originalIndex = num - 1;

                if (originalIndex < 0 ||
                    originalIndex >= _allImageFiles.Count)
                    return;

                trkRecord.Value = originalIndex;
            }
            catch
            {
            }
        }






        // ════════════════════════════════════════════════════════════
        // 로드
        // ════════════════════════════════════════════════════════════

        private void BtnLoadCarDirectory_Click(object? sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();
            dlg.Description = "차량 폴더 열기";
            dlg.UseDescriptionForTitle = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtCarDirectory.Text = dlg.SelectedPath;
                try
                {
                    var ext = Path.GetExtension(txtTub.Text ?? string.Empty)?.ToLowerInvariant();
                    var imageExts = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                    // If txtTub currently points to a single image, do not load the directory (only show path).
                    if (string.IsNullOrEmpty(ext) || !imageExts.Contains(ext))
                    {
                        LoadImagesFromDirectory(dlg.SelectedPath);
                    }
                }
                catch { }
            }
        }

        private void BtnLoadTub_Click(object? sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();

            dlg.Description = "이미지 폴더 선택";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            txtTub.Text = dlg.SelectedPath;
            CurrentTubPath = dlg.SelectedPath;

            LoadImagesFromDirectory(dlg.SelectedPath);
        }

        private void LoadImagesFromDirectory(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
            {
                // ★ 트랙바 버그 수정: 폴더 전환 시 이전 카탈로그 상태 먼저 초기화
                // _maxCatalogIndex가 이전 폴더 값으로 남아있으면 트랙바 Maximum이 잘못 설정됨
                _maxCatalogIndex = null;
                _catalogIndexByFileName.Clear();
                _catalogLines.Clear();
                _imageFiles.Clear();
                trkRecord.Enabled = false;
                return;
            }

            var exts = new[] { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif" };
            var files = exts
                .SelectMany(e => Directory.GetFiles(folder, e, SearchOption.TopDirectoryOnly))
                .ToList();
            // Prefer numeric ordering when filenames contain indices (e.g. 0, 1, 1000)
            // to avoid lexicographic order where "0" may be followed by "1000".
            files = files
                .OrderBy(p =>
                {
                    var fn = Path.GetFileName(p);
                    var n = ExtractFirstNumber(fn);
                    return n.HasValue ? n.Value : int.MaxValue;
                })
                .ThenBy(p => p, StringComparer.OrdinalIgnoreCase)
                .ToList();
            _imageFiles = files;
            _allImageFiles = files.ToList();
            _loadedFolderPath = folder;

            _deletedImages.Clear();
            _deletedIndexes.Clear();
            UpdateDeleteStatus();

            if (_imageFiles.Count > 0)
            {
                trkRecord.Minimum = 0;

                trkRecord.Maximum = Math.Max(0, _allImageFiles.Count - 1);

                trkRecord.Value = 0;
                trkRecord.Enabled = true;
                SetCurrentIndex(0);
                try
                {
                    LoadCatalogsFromDirectory(folder);
                    if (_catalogLines.Count == 0)
                    {
                        var parent = Path.GetDirectoryName(folder);
                        if (!string.IsNullOrEmpty(parent) && Directory.Exists(parent))
                            LoadCatalogsFromDirectory(parent);
                    }
                }
                catch { }
            }
            else
            {
                trkRecord.Enabled = false;
                picTubImage.Image?.Dispose();
                picTubImage.Image = null;
                _currentIndex = -1;
                txtRecordNumber.Text = "기록 000000";
                _catalogLines.Clear();
                _currentThrottle = null;
                _currentAngle = null;
                picThrottle.Invalidate();
                picAngle.Invalidate();

                // 정적 상태 초기화
                LastImagePath = "";
                LastAngle = null;
                LastThrottle = null;
            }
        }

        private void SetCurrentIndex(int idx)
        {
            if (idx < 0 || idx >= _allImageFiles.Count)
                return;

            _currentIndex = idx;

            try
            {
                var path = _allImageFiles[idx];

                using var fs = File.OpenRead(path);
                using var img = Image.FromStream(fs);

                picTubImage.Image?.Dispose();
                picTubImage.Image = new Bitmap(img);


                txtRecordNumber.Text =
                    $"기록 {(idx + 1):D6}";


                double? angle = null;
                double? throttle = null;

                if (_catalogLines != null && _catalogLines.Count > idx)
                {
                    var parsed = ParseCatalogLine(_catalogLines[idx]);

                    if (parsed.angle.HasValue)
                    {
                        _currentAngle = parsed.angle.Value;
                        angle = parsed.angle.Value;
                    }
                    else
                    {
                        _currentAngle = null;
                    }

                    if (parsed.throttle.HasValue)
                    {
                        _currentThrottle = parsed.throttle.Value;
                        throttle = parsed.throttle.Value;
                    }
                    else
                    {
                        _currentThrottle = null;
                    }
                }
                else
                {
                    _currentAngle = null;
                    _currentThrottle = null;
                }

                picAngle.Invalidate();
                picThrottle.Invalidate();

                LastImagePath = path;
                LastAngle = angle;
                LastThrottle = throttle;

                OnTubDataChanged?.Invoke(
                    path,
                    angle,
                    throttle,
                    _currentIndex,
                    _allImageFiles.Count);

                pnlTimeline.Invalidate();
            }
            catch
            {
            }
        }

        // ════════════════════════════════════════════════════════════
        // 게이지 Paint
        // ════════════════════════════════════════════════════════════

        private void PicThrottle_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = picThrottle.ClientRectangle;

            using (var b = new SolidBrush(Color.FromArgb(40, 40, 40)))
                g.FillRectangle(b, rect);

            // ── 중심점: 기존과 동일 ──────────────────────────────────
            var cx = rect.Left + rect.Width / 2f;
            var cy = rect.Bottom - 15f;
            var radius = Math.Min(rect.Width / 2f - 12f, rect.Height - 12f);
            if (radius <= 8) radius = Math.Min(rect.Width, rect.Height) / 2f;
            var arcRect = new RectangleF(cx - radius, cy - radius, radius * 2f, radius * 2f);

            double throttleVal = 0.0;
            var hasValue = _currentThrottle.HasValue;
            if (hasValue) throttleVal = _currentThrottle.Value;

            double tnorm;
            if (throttleVal >= -1.01 && throttleVal <= 1.01)
                tnorm = Math.Max(0.0, Math.Min(1.0, throttleVal)); // 0~1 범위 (음수 없음)
            else
            {
                tnorm = throttleVal;
                tnorm = Math.Max(0.0, Math.Min(1.0, tnorm));
            }

            // ── 호: 9시(180°) → 0시(0°/360°) 즉 180° 범위 ──────────
            // 9시 = 180°, 시계방향으로 증가 → 0시(360°=0°)가 최대
            // DrawArc: 시작각 = 180°, 범위 = -180° (반시계 그리기 트릭 없이,
            // 9시→12시→3시 = 시계방향이므로 startAngle=180, sweepAngle=-180)
            int segments = 12;
            float totalSweep = 180f;   // 180°→0° (시계방향)
            float segSweep = totalSweep / segments;
            float gap = 2f;
            float segPenWidth = Math.Max(12f, radius * 0.18f);

            for (int i = 0; i < segments; i++)
            {
                float pos = i / (float)(segments - 1);
                int rCol = (int)(pos * 255);
                int gCol = (int)((1 - pos) * 255);
                var segColor = Color.FromArgb(200, rCol, gCol, 0);
                float startA = 180f + i * segSweep + (i == 0 ? gap / 2f : gap / 2f);
                float sweepA = segSweep - gap;
                using (var pen = new Pen(segColor, segPenWidth)
                {
                    StartCap = System.Drawing.Drawing2D.LineCap.Round,
                    EndCap = System.Drawing.Drawing2D.LineCap.Round
                })
                    g.DrawArc(pen, arcRect, startA, sweepA);
            }

            // ── 눈금선 ───────────────────────────────────────────────
            using (var tickPen = new Pen(Color.FromArgb(120, 120, 120), 1.5f))
            {
                // 9시(180°)→0시(0°) 사이 7개 눈금
                for (int i = 0; i <= 6; i++)
                {
                    var t = 180.0 - (i * 30.0);   // 180→0 (30° 간격)
                    var rad = t * Math.PI / 180.0;
                    var inner = new PointF(
                        (float)(cx + Math.Cos(rad) * (radius - segPenWidth / 2f - 2)),
                        (float)(cy + Math.Sin(rad) * (radius - segPenWidth / 2f - 2)));
                    var outer = new PointF(
                        (float)(cx + Math.Cos(rad) * (radius + segPenWidth / 2f + 2)),
                        (float)(cy + Math.Sin(rad) * (radius + segPenWidth / 2f + 2)));
                    g.DrawLine(tickPen, inner, outer);
                }
            }

            // ── 바늘: 9시(180°)에서 시작, tnorm=1이면 0°(3시) ──────
            // 하지만 요청: 최대는 12시(90°↑ = -90°=270°). 
            // 9시(180°)→12시(90° 위 = 270° CCW 기준이지만 GDI에선 -90°)
            // GDI+ 각도: 0=3시, 90=6시, 180=9시, 270=12시
            // 범위: 180°→270° 즉 sweepAngle = -90° (반시계) × tnorm
            // 하지만 시계방향으로 올라간다고 했으므로:
            // 9시(180°) → 시계방향 → 12시(270° GDI, 실제 위쪽)
            // GDI+ 시계방향: 180°에서 증가하면 6시쪽으로 감 → 원하지 않음
            // 따라서 9시에서 반시계 방향(각도 감소)으로 12시까지 = 180°→90°
            // tnorm=0: 180°(9시), tnorm=1: 90°(12시)
            var angleDeg = 180.0 + tnorm * 180.0;
            var angleRad = angleDeg * Math.PI / 180.0;
            var nx = (float)(cx + Math.Cos(angleRad) * (radius - 14));
            var ny = (float)(cy + Math.Sin(angleRad) * (radius - 14));

            using (var outline = new Pen(Color.FromArgb(220, 0, 0, 0), 7f)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            })
                g.DrawLine(outline, cx, cy, nx, ny);
            using (var pen = new Pen(Color.Red, 3.5f)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            })
                g.DrawLine(pen, cx, cy, nx, ny);

            using (var hub = new SolidBrush(Color.White))
                g.FillEllipse(hub, cx - 5f, cy - 5f, 10f, 10f);

            // ── 가운데 숫자 ──────────────────────────────────────────
            var valText = hasValue ? throttleVal.ToString("+0.000;-0.000;0.000", System.Globalization.CultureInfo.InvariantCulture) : "--";
            using (var f = new Font("맑은 고딕", 11f, FontStyle.Bold))
            using (var b = new SolidBrush(Color.White))
            {
                var sz = g.MeasureString(valText, f);
                float tx = 10f;
                float ty = 8f;
                g.DrawString(valText, f, b, tx, ty);
            }
        }

        private void PicAngle_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = picAngle.ClientRectangle;

            using (var b = new SolidBrush(Color.FromArgb(40, 40, 40)))
                g.FillRectangle(b, rect);

            // ── 중심점: 아래쪽에 배치하여 반원이 위로 펼쳐지도록 ────
            var cx = rect.Left + rect.Width / 2f;
            var cy = rect.Top + rect.Height - 20f;          // 하단 기준
            var radius = Math.Min(rect.Width / 2f - 20f, rect.Height - 20f);
            if (radius <= 6) radius = Math.Min(rect.Width, rect.Height) / 2f;
            var arcRect = new RectangleF(cx - radius, cy - radius, radius * 2f, radius * 2f);

            float arcPenW = Math.Max(10f, radius * 0.14f);

            // ── 반원 호: 180°(9시)→0°(3시), 즉 위쪽 반원 ───────────
            // 왼쪽(180°) = 좌회전 최대, 0°(3시) = 우회전 최대
            // 파란색: 좌반원(180°→270°, 즉 180°부터 -90° sweep = 왼쪽→12시)
            // 빨간색: 우반원(270°→360°, 즉 270°부터 -90° sweep = 12시→오른쪽)
            // GDI+: 0°=3시, 180°=9시, 270°=12시(위)
            // 위 반원 전체 = 시작 180°, sweep -180°

            // 좌측(파랑): 180° → 270° (GDI시계방향 = 9시→12시)
            using (var pen = new Pen(Color.FromArgb(220, 30, 144, 255), arcPenW)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Flat,
                EndCap = System.Drawing.Drawing2D.LineCap.Flat
            })
                g.DrawArc(pen, arcRect, 180f, 90f);   // 9시→12시

            // 우측(빨강): 270° → 360° (GDI시계방향 = 12시→3시)
            using (var pen = new Pen(Color.FromArgb(220, 255, 60, 60), arcPenW)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Flat,
                EndCap = System.Drawing.Drawing2D.LineCap.Flat
            })
                g.DrawArc(pen, arcRect, 270f, 90f);   // 12시→3시

            // ── 바늘: 12시(270° GDI = -90° 수학) 기준, 좌우로 이동 ─
            // angle -1 → 9시(180°), angle 0 → 12시(270°), angle +1 → 3시(0°/360°)
            // GDI 각도 = 270° + angle * 90°  (음수=좌, 양수=우)
            double angDeg = 0.0;
            bool hasAngle = _currentAngle.HasValue;
            if (hasAngle)
                angDeg = Math.Max(-1.0, Math.Min(1.0, _currentAngle.Value));

            double gdiAngle = 270.0 + angDeg * 120.0;   // 270°±90°
            var angleRad = gdiAngle * Math.PI / 180.0;
            var nx = (float)(cx + Math.Cos(angleRad) * (radius - 8));
            var ny = (float)(cy + Math.Sin(angleRad) * (radius - 8));

            using (var outline = new Pen(Color.FromArgb(200, 255, 255, 255), 6f)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            })
                g.DrawLine(outline, cx, cy, nx, ny);
            using (var pen = new Pen(Color.Yellow, 3.5f)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            })
                g.DrawLine(pen, cx, cy, nx, ny);

            using (var hub = new SolidBrush(Color.Black))
                g.FillEllipse(hub, cx - 4f, cy - 4f, 8f, 8f);

            // ── 가운데 숫자 ──────────────────────────────────────────
            var valText = hasAngle
                ? _currentAngle!.Value.ToString("+0.000;-0.000;0.000", System.Globalization.CultureInfo.InvariantCulture)
                : "--";
            using (var f = new Font("맑은 고딕", 11f, FontStyle.Bold))
            using (var b2 = new SolidBrush(Color.White))
            {
                var sz = g.MeasureString(valText, f);
                float tx = 10f;
                float ty = 8f;   // 반원 안쪽
                g.DrawString(valText, f, b2, tx, ty);
            }
        }

        // ════════════════════════════════════════════════════════════
        // 카탈로그 로드 / 파싱
        // ════════════════════════════════════════════════════════════

        private void LoadCatalogsFromDirectory(string folder)
        {
            _catalogLines.Clear();
            if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder)) return;

            var allFiles = Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly);
            var catalogFiles = allFiles
                .Where(p =>
                {
                    var fn = Path.GetFileName(p);
                    return fn.StartsWith("catalog_", StringComparison.OrdinalIgnoreCase)
                        || fn.EndsWith(".catalog", StringComparison.OrdinalIgnoreCase);
                })
                .OrderBy(p =>
                {
                    var fn = Path.GetFileNameWithoutExtension(p);
                    var m = Regex.Match(fn, "catalog_(\\d+)", RegexOptions.IgnoreCase);
                    if (m.Success && int.TryParse(m.Groups[1].Value, out var v)) return (0, v, fn);
                    var firstNum = ExtractFirstNumber(fn);
                    if (firstNum.HasValue) return (1, firstNum.Value, fn);
                    return (2, int.MaxValue, fn);
                })
                .ToList();

            foreach (var cf in catalogFiles)
            {
                try
                {
                    foreach (var l in File.ReadAllLines(cf))
                        if (!string.IsNullOrWhiteSpace(l)) _catalogLines.Add(l.Trim());
                }
                catch { }
            }

            // Build filename->index map and compute maximum _index
            _catalogIndexByFileName.Clear();
            _maxCatalogIndex = null;
            for (int i = 0; i < _catalogLines.Count; i++)
            {
                var line = _catalogLines[i];
                try
                {
                    var parsed = ParseCatalogLine(line);
                    if (parsed.index.HasValue)
                    {
                        if (!_maxCatalogIndex.HasValue || parsed.index.Value > _maxCatalogIndex.Value)
                            _maxCatalogIndex = parsed.index.Value;
                    }
                
                    // try extract cam image filename
                    string? fname = null;
                    try
                    {
                        var doc = System.Text.Json.JsonDocument.Parse(line);
                        var root = doc.RootElement;
                        if (root.TryGetProperty("cam/image_array", out var pc) || root.TryGetProperty("cam_image_array", out pc) || root.TryGetProperty("cam/image", out pc) || root.TryGetProperty("cam_image", out pc))
                        {
                            if (pc.ValueKind == System.Text.Json.JsonValueKind.String) fname = pc.GetString();
                        }
                    }
                    catch { }

                    if (string.IsNullOrEmpty(fname))
                    {
                        var m = Regex.Match(line, "\"cam(?:/|_)image(?:_array)?\"\\s*:\\s*\"([^\"]+)\"");
                        if (m.Success) fname = m.Groups[1].Value;
                    }

                    if (!string.IsNullOrEmpty(fname))
                    {
                        try
                        {
                            var shortName = Path.GetFileName(fname);
                            if (parsed.index.HasValue)
                                _catalogIndexByFileName[shortName] = parsed.index.Value;
                        }
                        catch { }
                    }
                }
                catch { }
            }
        }

        private int? ExtractFirstNumber(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            var m = Regex.Match(input, "(\\d+)");
            if (m.Success && int.TryParse(m.Groups[1].Value, out var v)) return v;
            return null;
        }

        private (long? timestamp, double? angle, string mode, double? throttle, int? index) ParseCatalogLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return (null, null, null, null, null);
            long? timestamp = null;
            double? angle = null;
            string mode = null;
            double? throttle = null;
            int? idx = null;

            try
            {
                var doc = System.Text.Json.JsonDocument.Parse(line);
                var root = doc.RootElement;

                if (root.TryGetProperty("_timestamp_ms", out var p)
                    || root.TryGetProperty("timestamp_ms", out p)
                    || root.TryGetProperty("timestamp", out p))
                {
                    if (p.ValueKind == System.Text.Json.JsonValueKind.Number && p.TryGetInt64(out var v)) timestamp = v;
                    else if (p.ValueKind == System.Text.Json.JsonValueKind.String && long.TryParse(p.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var vv)) timestamp = vv;
                }
                if (root.TryGetProperty("user/angle", out var pa)
                    || root.TryGetProperty("angle", out pa)
                    || root.TryGetProperty("user_angle", out pa))
                {
                    if (pa.ValueKind == System.Text.Json.JsonValueKind.Number && pa.TryGetDouble(out var dv)) angle = dv;
                    else if (pa.ValueKind == System.Text.Json.JsonValueKind.String && double.TryParse(pa.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var dv2)) angle = dv2;
                }
                if (root.TryGetProperty("user/throttle", out var pt)
                    || root.TryGetProperty("throttle", out pt)
                    || root.TryGetProperty("user_throttle", out pt))
                {
                    if (pt.ValueKind == System.Text.Json.JsonValueKind.Number && pt.TryGetDouble(out var dv)) throttle = dv;
                    else if (pt.ValueKind == System.Text.Json.JsonValueKind.String && double.TryParse(pt.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var dv2)) throttle = dv2;
                }
                // try index fields
                if (root.TryGetProperty("_index", out var pi) || root.TryGetProperty("index", out pi))
                {
                    if (pi.ValueKind == System.Text.Json.JsonValueKind.Number && pi.TryGetInt32(out var iv)) idx = iv;
                    else if (pi.ValueKind == System.Text.Json.JsonValueKind.String && int.TryParse(pi.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var iv2)) idx = iv2;
                }
                if (root.TryGetProperty("user/mode", out var pm)
                    || root.TryGetProperty("mode", out pm)
                    || root.TryGetProperty("user_mode", out pm))
                    mode = pm.ValueKind == System.Text.Json.JsonValueKind.String ? pm.GetString() : pm.ToString();

                return (timestamp, angle, mode, throttle, idx);
            }
            catch { }

            double tmpd; long tmpl;
            string TryMatchNumber(string key)
            {
                var m = Regex.Match(line, $"{Regex.Escape(key)}\\s*[:=]\\s*([-+]?\\d*\\.?\\d+(?:[eE][-+]?\\d+)?)");
                return m.Success ? m.Groups[1].Value : null;
            }
            string TryMatchToken(string key)
            {
                var m = Regex.Match(line, $"{Regex.Escape(key)}\\s*[:=]\\s*([^,;\\s]+)");
                return m.Success ? m.Groups[1].Value : null;
            }

            var tsStr = TryMatchNumber("_timestamp_ms") ?? TryMatchNumber("timestamp_ms") ?? TryMatchNumber("timestamp");
            if (tsStr != null && long.TryParse(tsStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out tmpl)) timestamp = tmpl;
            var aStr = TryMatchNumber("user/angle") ?? TryMatchNumber("angle") ?? TryMatchNumber("user_angle");
            if (aStr != null && double.TryParse(aStr, NumberStyles.Float, CultureInfo.InvariantCulture, out tmpd)) angle = tmpd;
            var tStr = TryMatchNumber("user/throttle") ?? TryMatchNumber("throttle") ?? TryMatchNumber("user_throttle");
            if (tStr != null && double.TryParse(tStr, NumberStyles.Float, CultureInfo.InvariantCulture, out tmpd)) throttle = tmpd;
            var mStr = TryMatchToken("user/mode") ?? TryMatchToken("mode") ?? TryMatchToken("user_mode");
            if (!string.IsNullOrEmpty(mStr)) mode = mStr.Trim('"');

            // try to extract index via token
            var iStr = TryMatchNumber("_index") ?? TryMatchNumber("index");
            if (iStr != null && int.TryParse(iStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ip)) idx = ip;

            return (timestamp, angle, mode, throttle, idx);
        }

        // ════════════════════════════════════════════════════════════
        // 재생
        // ════════════════════════════════════════════════════════════

        private void UpdatePlaybackIntervalFromCombo()
        {
            if (cmbSpeed == null) return;
            var text = cmbSpeed.Text?.Trim() ?? "";
            // allow items like "1.00x" or "1.00×"
            text = text.Replace("×", "x").Trim();
            if (text.EndsWith("x", StringComparison.OrdinalIgnoreCase))
                text = text.Substring(0, text.Length - 1).Trim();

            if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var speed) || speed <= 0)
                speed = 1.0;
            _playTimer.Interval = (int)Math.Max(10, Math.Round(_playBaseIntervalMs / speed));
        }

        private void BtnStartStop_Click(object? sender, EventArgs e)
        {
            if (_isPlaying) StopPlayback(); else StartPlayback();
        }

        private void StartPlayback()
        {
            if (_imageFiles.Count == 0) return;
            _isPlaying = true;
            btnStartStop.Text = "⏹ 정지";
            btnStartStop.BackColor = Color.FromArgb(180, 40, 40);
            btnStartStop.FlatAppearance.BorderSize = 0;
            _playTimer.Start();
        }

        private void StopPlayback()
        {
            _isPlaying = false;
            btnStartStop.Text = "▶ 재생";
            btnStartStop.BackColor = Color.FromArgb(24, 95, 165);
            btnStartStop.FlatAppearance.BorderSize = 0;
            _playTimer.Stop();
        }

        // ════════════════════════════════════════════════════════════
        // 탐색
        // ════════════════════════════════════════════════════════════

        private void TrkRecord_Scroll(object? sender, EventArgs e)
        {

        }
        private void TrkRecord_ValueChanged(object? sender, EventArgs e)
        {
            int idx = trkRecord.Value;

            if (idx < 0 || idx >= _allImageFiles.Count)
                return;

            if (IsDeletedFrame(idx))
            {
                int fixedIndex;

                if (idx > _currentIndex)
                {
                    fixedIndex = idx;

                    while (fixedIndex < _allImageFiles.Count &&
                           IsDeletedFrame(fixedIndex))
                    {
                        fixedIndex++;
                    }
                }
                else
                {
                    fixedIndex = idx;

                    while (fixedIndex >= 0 &&
                           IsDeletedFrame(fixedIndex))
                    {
                        fixedIndex--;
                    }
                }

                if (fixedIndex < 0 || fixedIndex >= _allImageFiles.Count)
                    return;

                trkRecord.Value = fixedIndex;
                return;
            }

            SetCurrentIndex(idx);
        }
        private void BtnPrev_Click(object? sender, EventArgs e) => MovePrev();
        private void BtnNext_Click(object? sender, EventArgs e) => MoveNext();

        private void MovePrev()
        {
            if (_imageFiles.Count == 0) return;
            var newIndex = Math.Max(0, _currentIndex - 1);
            if (trkRecord.Enabled) trkRecord.Value = newIndex; else SetCurrentIndex(newIndex);
        }

        private void MoveNext()
        {
            if (_imageFiles.Count == 0) return;
            var newIndex = _currentIndex < 0 ? 0 : Math.Min(_imageFiles.Count - 1, _currentIndex + 1);
            if (trkRecord.Enabled) trkRecord.Value = newIndex; else SetCurrentIndex(newIndex);
        }

        private void MoveFastPrev()
        {
            if (_imageFiles.Count == 0) return;
            var newIndex = Math.Max(0, _currentIndex - 100);
            if (trkRecord.Enabled) trkRecord.Value = newIndex; else SetCurrentIndex(newIndex);
        }

        private void MoveFastNext()
        {
            if (_imageFiles.Count == 0) return;
            var newIndex = _currentIndex < 0 ? 0 : Math.Min(_imageFiles.Count - 1, _currentIndex + 100);
            if (trkRecord.Enabled) trkRecord.Value = newIndex; else SetCurrentIndex(newIndex);
        }

        // ════════════════════════════════════════════════════════════
        // 삭제 / 복원 / 저장 / 필터
        // ════════════════════════════════════════════════════════════


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentIndex < 0 || _currentIndex >= _allImageFiles.Count)
                return;

            string deletedImage = _allImageFiles[_currentIndex];

            if (!_deletedImages.Contains(deletedImage))
            {
                _deletedImages.Add(deletedImage);
                _deletedIndexes[deletedImage] = _currentIndex;

                _deletedSingleFrames.Add(_currentIndex);
            }

            UpdateDeleteStatus();
            pnlTimeline.Invalidate();

            int nextIndex = _currentIndex + 1;

            while (nextIndex < _allImageFiles.Count &&
                   IsDeletedFrame(nextIndex))
            {
                nextIndex++;
            }

            if (nextIndex < _allImageFiles.Count)
            {
                trkRecord.Value = nextIndex;
            }
        }


        private void btnRestore_Click(object sender, EventArgs e)
        {
            // 1순위 : 프레임 삭제 복원
            if (_deletedSingleFrames.Count > 0)
            {
                int frameIndex =
                    _deletedSingleFrames.Last();

                _deletedSingleFrames.RemoveAt(
                    _deletedSingleFrames.Count - 1);

                string image =
                    _allImageFiles[frameIndex];

                _deletedImages.Remove(image);

                if (_deletedIndexes.ContainsKey(image))
                    _deletedIndexes.Remove(image);

                UpdateDeleteStatus();
                pnlTimeline.Invalidate();

                return;
            }

            // 2순위 : 범위 삭제 복원
            if (_deletedRanges.Count > 0)
            {
                var range =
                    _deletedRanges.Last();

                _deletedRanges.RemoveAt(
                    _deletedRanges.Count - 1);

                for (int i = range.Start - 1;
                     i <= range.End - 1;
                     i++)
                {
                    string image =
                        _allImageFiles[i];

                    _deletedImages.Remove(image);

                    if (_deletedIndexes.ContainsKey(image))
                        _deletedIndexes.Remove(image);
                }

                UpdateDeleteStatus();
                pnlTimeline.Invalidate();

                return;
            }

            MessageBox.Show("복원할 항목이 없습니다.");
        }



        private void btnReroadTub_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_loadedFolderPath)) return;
            StopPlayback();
            LoadImagesFromDirectory(_loadedFolderPath);
            MessageBox.Show("처음 상태로 다시 불러왔습니다.");
        }

        private void btnSetLeft_Click(object sender, EventArgs e) => trkRecord.Value = trkRecord.Minimum;
        private void btnSetRight_Click(object sender, EventArgs e) => trkRecord.Value = trkRecord.Maximum;

        private void menuThrottle_Click(object sender, EventArgs e) => ApplyFilter(true);
        private void menuAngle_Click(object sender, EventArgs e) => ApplyFilter(false);

        private void ApplyFilter(bool isThrottle)
        {
            string minInput =
               Microsoft.VisualBasic.Interaction.InputBox(
                   "최소값 입력", "필터 설정", "0");

            string maxInput =
                Microsoft.VisualBasic.Interaction.InputBox(
                    "최대값 입력", "필터 설정", "999999");

            if (!double.TryParse(minInput, out double minValue) ||
                !double.TryParse(maxInput, out double maxValue))
            {
                MessageBox.Show("숫자를 입력하세요.");
                return;
            }

            if (minValue > maxValue)
            {
                MessageBox.Show("최소값이 최대값보다 클 수 없습니다.");
                return;
            }

            _imageFiles = _imageFiles
                .Where((img, idx) =>
                {
                    if (idx >= _catalogLines.Count)
                        return false;

                    var parsed = ParseCatalogLine(_catalogLines[idx]);

                    double? value = isThrottle
                        ? parsed.throttle
                        : parsed.angle;

                    return value.HasValue &&
                           value.Value >= minValue &&
                           value.Value <= maxValue;
                })
                .ToList();

            trkRecord.Minimum = 0;
            trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);

            if (_imageFiles.Count > 0)
            {
                trkRecord.Value = 0;
                SetCurrentIndex(0);
            }

            MessageBox.Show($"필터 적용 완료 ({minValue} ~ {maxValue})");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
         "삭제된 파일이 실제로 저장되며 복구가 어려울 수 있습니다.\n그래도 저장하시겠습니까?",
         "저장 확인",
         MessageBoxButtons.YesNo,
         MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            var toDelete = _deletedImages.ToList();

            int deletedCount = 0;
            foreach (string imagePath in toDelete)
            {
                try
                {
                    if (File.Exists(imagePath)) { File.Delete(imagePath); deletedCount++; }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("삭제 실패:\n" + ex.Message);
                }
            }
            _deletedImages.Clear();
            _deletedIndexes.Clear();
            UpdateDeleteStatus();
            MessageBox.Show($"{deletedCount}개 파일 저장 완료");
            pnlTimeline.Invalidate();
        }

        // ════════════════════════════════════════════════════════════
        // 상태 / 프로퍼티
        // ════════════════════════════════════════════════════════════

        private void UpdateDeleteStatus()
        {
            int total = _imageFiles.Count + _deletedImages.Count;
            int deleted = _deletedImages.Count;
            lblDeleteStatus.Text = $"{total}개 중 {deleted}개 삭제됨";
            lblDeleteStatus.Visible = deleted > 0;
        }

        private void pnlSpeedRange_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            int y = pnlSpeedRange.Height / 2;

            // 전체 회색 선
            g.DrawLine(
                Pens.Gray,
                20,
                y,
                pnlSpeedRange.Width - 20,
                y);

            // 선택된 파란 선
            using (var pen = new Pen(Color.DodgerBlue, 4))
            {
                g.DrawLine(
                    pen,
                    _minX,
                    y,
                    _maxX,
                    y);
            }

            // 최소값 손잡이
            g.FillEllipse(
                Brushes.DodgerBlue,
                _minX - 8,
                y - 8,
                16,
                16);

            // 최대값 손잡이
            g.FillEllipse(
                Brushes.DodgerBlue,
                _maxX - 8,
                y - 8,
                16,
                16);
        }

        public string SelectedDataPath
        {
            get
            {
                string filePath = txtTub.Text?.Trim() ?? "";
                if (string.IsNullOrEmpty(filePath)) return "";
                string imageFolder = Path.GetDirectoryName(filePath) ?? "";
                string folderName = Path.GetFileName(imageFolder);
                if (string.Equals(folderName, "images", StringComparison.OrdinalIgnoreCase))
                    return Path.GetDirectoryName(imageFolder) ?? imageFolder;
                return imageFolder;
            }
        }
        private void pnlSpeedRange_MouseDown(
    object sender,
    MouseEventArgs e)
        {
            if (Math.Abs(e.X - _minX) < 15)
            {
                _dragMin = true;
            }

            if (Math.Abs(e.X - _maxX) < 15)
            {
                _dragMax = true;
            }
        }
        private void pnlSpeedRange_MouseUp(
    object sender,
    MouseEventArgs e)
        {
            _dragMin = false;
            _dragMax = false;
        }
        private void pnlSpeedRange_MouseMove(
    object sender,
    MouseEventArgs e)
        {
            if (_dragMin)
            {
                _minX = e.X;
                int left = 20;
                int right = Math.Max(left + 1, pnlSpeedRange.Width - 20);
                _minX = Math.Max(
                    left,
                    Math.Min(_minX, _maxX));

                nudSpeedMin.Value =
                    Math.Min(
                        nudSpeedMin.Maximum,
                        XToSpeed(_minX));

                pnlSpeedRange.Invalidate();
            }

            if (_dragMax)
            {
                _maxX = e.X;
                int left = 20;
                int right = Math.Max(left + 1, pnlSpeedRange.Width - 20);
                _maxX = Math.Min(
                    right,
                    Math.Max(_maxX, _minX));

                nudSpeedMax.Value =
                    Math.Max(
                        nudSpeedMax.Minimum,
                        XToSpeed(_maxX));

                pnlSpeedRange.Invalidate();
            }
        }
        private decimal XToSpeed(int x)
        {

            int maxX = pnlSpeedRange.Width - 20;

            x = Math.Max(20, Math.Min(x, maxX));

            return (decimal)(
                (double)(x - 20)
                / (maxX - 20));
        }
        private int SpeedToX(decimal speed)
        {
            int maxX = pnlSpeedRange.Width - 20;

            return 20 + (int)(
                (double)speed
                * (maxX - 20));

        }
        private void nudSpeedMin_ValueChanged(
           object sender,
           EventArgs e)
        {
            _minX = SpeedToX(nudSpeedMin.Value);

            pnlSpeedRange.Invalidate();
        }
        private void nudSpeedMax_ValueChanged(
           object sender,
           EventArgs e)
        {
            _maxX = SpeedToX(nudSpeedMax.Value);

            pnlSpeedRange.Invalidate();
        }
        private decimal XToAngle(int x)
        {
            int maxX = pnlAngleRange.Width - 20;

            x = Math.Max(20, Math.Min(x, maxX));

            return (decimal)(
                ((double)(x - 20) / (maxX - 20))
                * 2 - 1);
        }
        private int AngleToX(decimal angle)
        {
            int maxX = pnlAngleRange.Width - 20;

            return 20 + (int)(
                ((double)angle + 1)
                / 2
                * (maxX - 20));

        }
        private void nudAngleMin_ValueChanged(
    object sender,
    EventArgs e)
        {
            _angleMinX = AngleToX(nudAngleMin.Value);

            pnlAngleRange.Invalidate();
        }

        private void nudAngleMax_ValueChanged(
            object sender,
            EventArgs e)
        {
            _angleMaxX = AngleToX(nudAngleMax.Value);

            pnlAngleRange.Invalidate();
        }
        private void pnlAngleRange_MouseDown(
    object sender,
    MouseEventArgs e)
        {
            if (Math.Abs(e.X - _angleMinX) < 15)
                _dragAngleMin = true;

            if (Math.Abs(e.X - _angleMaxX) < 15)
                _dragAngleMax = true;
        }

        private void pnlAngleRange_MouseUp(
            object sender,
            MouseEventArgs e)
        {
            _dragAngleMin = false;
            _dragAngleMax = false;
        }

        private void pnlAngleRange_MouseMove(
            object sender,
            MouseEventArgs e)
        {
            if (_dragAngleMin)
            {
                _angleMinX = e.X;
                int left = 20;
                int right = Math.Max(left + 1, pnlAngleRange.Width - 20);


                _angleMinX = Math.Max(
         left,
         Math.Min(_angleMinX, _angleMaxX));

                nudAngleMin.Value =
                    Math.Max(
                        nudAngleMin.Minimum,
                        Math.Min(
                            nudAngleMin.Maximum,
                            XToAngle(_angleMinX)));

                pnlAngleRange.Invalidate();
            }
            if (_dragAngleMax)

            {
                _angleMaxX = e.X;
                int left = 20;
                int right = Math.Max(left + 1, pnlAngleRange.Width - 20);

                int maxX = pnlAngleRange.Width - 20;
                _angleMaxX = Math.Min(
                    right,
                    Math.Max(_angleMaxX, _angleMinX));

                nudAngleMax.Value =
                    Math.Max(
                        nudAngleMax.Minimum,
                        Math.Min(
                            nudAngleMax.Maximum,
                            XToAngle(_angleMaxX)
                        ));

                pnlAngleRange.Invalidate();
            }
        }

        private void pnlAngleRange_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            int y = pnlAngleRange.Height / 2;

            // 전체 회색 선
            g.DrawLine(
                Pens.Gray,
                20,
                y,
                pnlAngleRange.Width - 20,
                y);

            // 선택된 파란 선
            using (var pen = new Pen(Color.DodgerBlue, 4))
            {
                g.DrawLine(
                    pen,
                    _angleMinX,
                    y,
                    _angleMaxX,
                    y);
            }

            // 최소값 손잡이
            g.FillEllipse(
                Brushes.DodgerBlue,
                _angleMinX - 8,
                y - 8,
                16,
                16);

            // 최대값 손잡이
            g.FillEllipse(
                Brushes.DodgerBlue,
                _angleMaxX - 8,
                y - 8,
                16,
                16);
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            double speedMin = (double)nudSpeedMin.Value;
            double speedMax = (double)nudSpeedMax.Value;

            double angleMin = (double)nudAngleMin.Value;
            double angleMax = (double)nudAngleMax.Value;
            bool speedExists =
    _speedFilters.Any(x =>
        x.Min == speedMin &&
        x.Max == speedMax);

            if (!speedExists)
            {
                _speedFilters.Add((speedMin, speedMax));

                cmbSpeedFilters.Items.Add(
                    $"속도 {speedMin:F3} ~ {speedMax:F3}");
            }

            bool angleExists =
                _angleFilters.Any(x =>
                    x.Min == angleMin &&
                    x.Max == angleMax);

            if (!angleExists)
            {
                _angleFilters.Add((angleMin, angleMax));

                cmbAngleFilters.Items.Add(
                    $"각도 {angleMin:F3} ~ {angleMax:F3}");
            }
            _filteredImages.Clear();
            _imageFiles = _allImageFiles
      .Where((img, idx) =>
      {
          if (idx >= _catalogLines.Count)
              return false;

          var parsed = ParseCatalogLine(_catalogLines[idx]);

          bool speedMatch = false;
          bool angleMatch = false;

          foreach (var filter in _speedFilters)
          {
              if (parsed.throttle.HasValue &&
                  parsed.throttle.Value >= filter.Min &&
                  parsed.throttle.Value <= filter.Max)
              {
                  speedMatch = true;
                  break;
              }
          }

          foreach (var filter in _angleFilters)
          {
              if (parsed.angle.HasValue &&
                  parsed.angle.Value >= filter.Min &&
                  parsed.angle.Value <= filter.Max)
              {
                  angleMatch = true;
                  break;
              }
          }

          return speedMatch && angleMatch;
      })
      .ToList();
            foreach (var img in _allImageFiles)
            {
                if (!_imageFiles.Contains(img))
                {
                    _filteredImages.Add(img);
                }
            }
            pnlTimeline.Invalidate();
            trkRecord.Minimum = 0;
            trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);

            if (_imageFiles.Count > 0)
            {
                trkRecord.Value = 0;
                SetCurrentIndex(0);
            }

            MessageBox.Show(
                $"필터 적용 완료\n남은 프레임: {_imageFiles.Count}개");
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            _speedFilters.Clear();
            _angleFilters.Clear();

            cmbSpeedFilters.Items.Clear();
            cmbAngleFilters.Items.Clear();

            cmbSpeedFilters.Text = "속도 필터 목록";
            cmbAngleFilters.Text = "각도 필터 목록";
            _imageFiles = _allImageFiles.ToList();

            trkRecord.Minimum = 0;
            trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);

            if (_imageFiles.Count > 0)
            {
                trkRecord.Value = 0;
                SetCurrentIndex(0);
            }

            MessageBox.Show("필터 해제 완료");
            _filteredImages.Clear();
            pnlTimeline.Invalidate();
        }
        private void pnlTimeline_Paint(
           object sender,
           PaintEventArgs e)
        {

            var g = e.Graphics;

            g.Clear(Color.White);

            g.FillRectangle(
                Brushes.DodgerBlue,
                0,
                0,
                pnlTimeline.Width,
                pnlTimeline.Height);

            int totalFrames = _allImageFiles.Count;

            if (totalFrames == 0)
                return;

            foreach (string deletedImage in _deletedImages)
            {
                int index =
                    _allImageFiles.IndexOf(deletedImage);

                if (index < 0)
                    continue;

                int startX =
                    index * pnlTimeline.Width / totalFrames;

                int endX =
                    (index + 1) * pnlTimeline.Width / totalFrames;

                int width =
                    Math.Max(1, endX - startX);

                g.FillRectangle(
                    Brushes.Red,
                    startX,
                    0,
                    width,
                    pnlTimeline.Height);
            }
            foreach (string filteredImage in _filteredImages)
            {
                int index =
                    _allImageFiles.IndexOf(filteredImage);

                if (index < 0)
                    continue;

                int startX =
                    index * pnlTimeline.Width / totalFrames;

                int endX =
                    (index + 1) * pnlTimeline.Width / totalFrames;

                int width =
                    Math.Max(1, endX - startX);

                g.FillRectangle(
                    Brushes.Red,
                    startX,
                    0,
                    width,
                    pnlTimeline.Height);
            }
            foreach (var range in _ranges)
            {
                for (int i = range.Start - 1; i <= range.End - 1; i++)
                {
                    if (IsDeletedFrame(i))
                        continue;

                    int startX =
                        i * pnlTimeline.Width / totalFrames;

                    int endX =
                        (i + 1) * pnlTimeline.Width / totalFrames;

                    int width =
                        Math.Max(1, endX - startX);

                    g.FillRectangle(
                        Brushes.Yellow,
                        startX,
                        0,
                        width,
                        pnlTimeline.Height);
                }
            }
            if (_currentIndex < 0)
                return;

            int currentX =
                _currentIndex * pnlTimeline.Width
                / _allImageFiles.Count;

            using (var pen = new Pen(Color.LimeGreen, 3))
            {
                g.DrawLine(
                    pen,
                    currentX,
                    0,
                    currentX,
                    pnlTimeline.Height);
            }
        }

        private void BtnLeftSet_Click(object sender, EventArgs e)
        {
            _leftFrame = trkRecord.Value + 1;

        }

        private void BtnRightSet_Click(object sender, EventArgs e)
        {
            _rightFrame = trkRecord.Value + 1;


            if (_leftFrame != -1)
            {
                int start = Math.Min(_leftFrame, _rightFrame);
                int end = Math.Max(_leftFrame, _rightFrame);

                _ranges.Add((start, end));

                int rangeNumber = _ranges.Count;

                cmbRanges.Items.Add(
                    $"범위{rangeNumber} : {start} ~ {end}");

                cmbRanges.Text = "범위 목록";
                pnlTimeline.Invalidate();
            }
        }

        private void BtnRangeDelete_Click(object sender, EventArgs e)
        {
            if (cmbRanges.SelectedIndex < 0)
            {
                MessageBox.Show("범위를 선택하세요.");
                return;
            }

            var range = _ranges[cmbRanges.SelectedIndex];

            _deletedRanges.Add(range);
            int start = range.Start - 1;
            int end = range.End - 1;
           
            for (int i = start; i <= end; i++)
            {
                string deletedImage = _allImageFiles[i];

                if (!_deletedImages.Contains(deletedImage))
                {
                    _deletedImages.Add(deletedImage);
                    _deletedIndexes[deletedImage] = i;

                }

            }

            trkRecord.Maximum = Math.Max(0, _allImageFiles.Count - 1);



            UpdateDeleteStatus();
            pnlTimeline.Invalidate();
            int idx = cmbRanges.SelectedIndex;

            _ranges.RemoveAt(idx);
            cmbRanges.Items.RemoveAt(idx);

            cmbRanges.Text = "범위 목록";
        }

        private void cmbRanges_SelectedIndexChanged(object sender, EventArgs e)

        {
            if (cmbRanges.SelectedIndex < 0)
                return;

            var range =
                _ranges[cmbRanges.SelectedIndex];

            trkRecord.Value =
            range.Start - 1;
        }

        private void btnRangeCancel_Click(object sender, EventArgs e)
        {
            if (cmbRanges.SelectedIndex < 0)
                return;

            int idx = cmbRanges.SelectedIndex;

            _ranges.RemoveAt(idx);

            cmbRanges.Items.RemoveAt(idx);

            pnlTimeline.Invalidate();
            if (cmbRanges.Items.Count == 0)
            {
                cmbRanges.SelectedIndex = -1;
            }

            cmbRanges.Text = "범위 목록";
        }

        private void btnDeleteAllRanges_Click(object sender, EventArgs e)
        {
            if (_ranges.Count == 0)
            {
                MessageBox.Show("삭제할 범위가 없습니다.");
                return;
            }

            var allIndexes = new HashSet<int>();

            foreach (var range in _ranges)
            {
                int start = range.Start - 1;
                int end = range.End - 1;

                for (int i = start; i <= end; i++)
                {
                    allIndexes.Add(i);

                }
                _deletedRanges.Add(range);
            }

            foreach (int i in allIndexes.OrderByDescending(x => x))
            {
                if (i < 0 || i >= _imageFiles.Count)
                    continue;

                string deletedImage = _allImageFiles[i];

                if (!_deletedImages.Contains(deletedImage))
                {
                    _deletedImages.Add(deletedImage);
                    _deletedIndexes[deletedImage] = i;
                }
            }

            _ranges.Clear();
            cmbRanges.Items.Clear();
            cmbRanges.Text = "범위 목록";

            trkRecord.Maximum =
                Math.Max(0, _allImageFiles.Count - 1);

            if (_imageFiles.Count > 0)
            {
                _currentIndex =
                    Math.Min(
                        _currentIndex,
                        _imageFiles.Count - 1);

                trkRecord.Value = _currentIndex;

                SetCurrentIndex(_currentIndex);
            }

            UpdateDeleteStatus();
            pnlTimeline.Invalidate();

            MessageBox.Show("모든 범위 삭제 완료");
        }

        private void btnSpeedRemove_Click(object sender, EventArgs e)
        {
            if (cmbSpeedFilters.SelectedIndex < 0)
                return;

            int idx = cmbSpeedFilters.SelectedIndex;

            _speedFilters.RemoveAt(idx);
            cmbSpeedFilters.Items.RemoveAt(idx);
            cmbSpeedFilters.Text = "속도 필터 목록";
        }
        

        private void btnAngleRemove_Click(object sender, EventArgs e)
        {
            if (cmbAngleFilters.SelectedIndex < 0)
                return;

            int idx = cmbAngleFilters.SelectedIndex;

            _angleFilters.RemoveAt(idx);
            cmbAngleFilters.Items.RemoveAt(idx);
            cmbAngleFilters.Text = "각도 필터 목록";
        }

        
    }
}








