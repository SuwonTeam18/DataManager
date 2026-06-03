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
        // PilotArena 연동용 정적 멤버
        // ════════════════════════════════════════════════════════════
        public static string LastImagePath { get; private set; } = "";
        public static double? LastAngle { get; private set; } = null;
        public static double? LastThrottle { get; private set; } = null;

        // PilotArena가 구독할 이벤트 (imagePath, angle, throttle, currentIndex, totalCount)
        public static event Action<string, double?, double?, int, int> OnTubDataChanged;

        // ── 게이지 상태 ──────────────────────────────────────────────
        private double? _currentThrottle = null;
        private double? _currentAngle = null;

        private List<string> _imageFiles = new List<string>();
        private List<string> _deletedImages = new List<string>();
        private List<string> _allImageFiles = new List<string>();
        private Dictionary<string, int> _deletedIndexes = new Dictionary<string, int>();
        private int _currentIndex = -1;

        // ── 속도 범위 슬라이더 (코드1) ───────────────────────────────
        private int _minX = 20;
        private int _maxX = 815;
        private bool _dragMin = false;
        private bool _dragMax = false;
        private const double SPEED_MAX = 200.0;

        // ── 각도 범위 슬라이더 (코드1) ───────────────────────────────
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

        // ── 카탈로그 인덱스 매핑 ────────────────────────────────────
        private int? _maxCatalogIndex = null;
        private Dictionary<string, int> _catalogIndexByFileName = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        // ════════════════════════════════════════════════════════════
        // 생성자
        // ════════════════════════════════════════════════════════════
        public ucTubManager()
        {
            InitializeComponent();

            btnLoadCarDirectory.Click += BtnLoadCarDirectory_Click;
            btnLoadTub.Click += BtnLoadTub_Click;
            trkRecord.Scroll += TrkRecord_Scroll;
            trkRecord.ValueChanged += TrkRecord_ValueChanged;
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

            // ════════════════════════════════════════════════════════
            // [PilotArena 연동] PilotArena 트랙바 변경 이벤트 구독
            // PilotArena에서 트랙바를 움직이면 TubManager도 해당 인덱스로 이동
            // ════════════════════════════════════════════════════════
            ucPilotArena.OnTimelineIndexChanged += (idx) =>
            {
                try { SetExternalIndex(idx); }
                catch { }
            };
        }

        // ════════════════════════════════════════════════════════════
        // 외부 인덱스 제어 (PilotArena 연동)
        // ════════════════════════════════════════════════════════════

        /// <summary>
        /// PilotArena 트랙바가 움직였을 때 호출됨.
        /// TubManager 트랙바와 화면을 해당 인덱스로 동기화.
        /// SetCurrentIndex 내부에서 OnTubDataChanged가 발행되므로
        /// PilotArena.OnTubDataChanged → SetTimelineIndexFromExternal 로 역전파되어
        /// PilotArena 트랙바가 한 번 더 업데이트되지만, _suppressTimelineNotify로 루프 차단됨.
        /// </summary>
        public void SetExternalIndex(int idx)
        {
            if (this.InvokeRequired) { this.Invoke(new Action(() => SetExternalIndex(idx))); return; }
            if (_imageFiles == null || _imageFiles.Count == 0) return;
            if (idx < 0) idx = 0;
            if (idx >= _imageFiles.Count) idx = _imageFiles.Count - 1;
            if (_currentIndex == idx) return;
            try { if (trkRecord != null) trkRecord.Value = idx; } catch { }
            try { SetCurrentIndex(idx); } catch { }
        }

        public void ShowImageAndSelectByPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (this.InvokeRequired) { this.Invoke(new Action(() => ShowImageAndSelectByPath(path))); return; }
            try
            {
                string targetFull = string.Empty;
                try { targetFull = Path.GetFullPath(path); } catch { targetFull = path; }

                int idx = -1;
                for (int i = 0; i < _imageFiles.Count; i++)
                    try { if (string.Equals(Path.GetFullPath(_imageFiles[i]), targetFull, StringComparison.OrdinalIgnoreCase)) { idx = i; break; } } catch { }

                if (idx >= 0) { SetExternalIndex(idx); return; }

                var fname = Path.GetFileName(path);
                if (!string.IsNullOrEmpty(fname))
                    for (int i = 0; i < _imageFiles.Count; i++)
                        try { if (string.Equals(Path.GetFileName(_imageFiles[i]), fname, StringComparison.OrdinalIgnoreCase)) { idx = i; break; } } catch { }

                if (idx >= 0) { SetExternalIndex(idx); return; }

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

        public string? GetImagePathAt(int idx)
        {
            if (this.InvokeRequired) return (string?)this.Invoke(new Func<int, string?>(GetImagePathAt), idx);
            if (_imageFiles == null || _imageFiles.Count == 0) return null;
            if (idx < 0 || idx >= _imageFiles.Count) return null;
            return _imageFiles[idx];
        }

        // ════════════════════════════════════════════════════════════
        // 기록 번호 직접 입력
        // ════════════════════════════════════════════════════════════

        private void TxtRecordNumber_Leave(object? sender, EventArgs e) => TryNavigateToRecordFromText();

        private void TxtRecordNumber_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TryNavigateToRecordFromText(); }
        }

        private void TryNavigateToRecordFromText()
        {
            try
            {
                var txt = txtRecordNumber.Text ?? string.Empty;
                var m = Regex.Match(txt, @"(\d+)");
                if (!m.Success) return;
                if (!int.TryParse(m.Groups[1].Value, out var num)) return;
                // 입력값은 카탈로그 _index 기준 (0-base)로 해석
                // 카탈로그 매핑이 있으면 _index로 검색, 없으면 배열 인덱스로 처리
                if (_imageFiles == null || _imageFiles.Count == 0) { txtRecordNumber.Text = "기록 000000"; return; }
                int targetIndex = FindIndexByCatalogIndex(num);
                if (targetIndex < 0) targetIndex = Math.Clamp(num, 0, _imageFiles.Count - 1);
                if (trkRecord.Enabled) trkRecord.Value = targetIndex;
                else SetCurrentIndex(targetIndex);
            }
            catch { }
        }

        /// <summary>
        /// 카탈로그 _index 값으로 _imageFiles 배열 인덱스를 역산.
        /// 매핑이 없으면 -1 반환.
        /// </summary>
        private int FindIndexByCatalogIndex(int catalogIdx)
        {
            if (_catalogIndexByFileName.Count == 0) return -1;
            foreach (var kv in _catalogIndexByFileName)
            {
                if (kv.Value == catalogIdx)
                {
                    for (int i = 0; i < _imageFiles.Count; i++)
                        if (string.Equals(Path.GetFileName(_imageFiles[i]), kv.Key, StringComparison.OrdinalIgnoreCase))
                            return i;
                }
            }
            return -1;
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
                    if (string.IsNullOrEmpty(ext) || !imageExts.Contains(ext))
                        LoadImagesFromDirectory(dlg.SelectedPath);
                }
                catch { }
            }
        }

        private void BtnLoadTub_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog();
            dlg.Title = "주행 데이터 열기";
            dlg.Filter = "Tub files (*.json;*.csv;*.jpg;*.jpeg;*.png)|*.json;*.csv;*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            dlg.CheckFileExists = true;
            dlg.Multiselect = false;
            if (dlg.ShowDialog() != DialogResult.OK) return;

            txtTub.Text = dlg.FileName;
            CurrentTubPath = Path.GetDirectoryName(dlg.FileName) ?? "";

            try
            {
                var dir = Path.GetDirectoryName(dlg.FileName) ?? string.Empty;
                if (!string.IsNullOrEmpty(dir))
                {
                    var dinfo = new DirectoryInfo(dir);
                    if (string.Equals(dinfo.Name, "images", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(dinfo.Name, "image", StringComparison.OrdinalIgnoreCase))
                    {
                        var parent = dinfo.Parent;
                        if (parent != null)
                            txtCarDirectory.Text = parent.FullName.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
                    }
                }
            }
            catch { }

            var ext = Path.GetExtension(dlg.FileName)?.ToLowerInvariant();
            var imageExts = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            if (!string.IsNullOrEmpty(ext) && imageExts.Contains(ext))
            {
                var folder = Path.GetDirectoryName(dlg.FileName) ?? string.Empty;
                LoadImagesFromDirectory(folder);
                var idx = _imageFiles.FindIndex(p =>
                    string.Equals(Path.GetFullPath(p), Path.GetFullPath(dlg.FileName), StringComparison.OrdinalIgnoreCase));
                if (idx >= 0) SetCurrentIndex(idx);
            }
        }

        private void LoadImagesFromDirectory(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
            {
                _imageFiles.Clear(); trkRecord.Enabled = false; return;
            }

            var exts = new[] { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif" };
            var files = exts.SelectMany(e => Directory.GetFiles(folder, e, SearchOption.TopDirectoryOnly)).ToList();

            // ── [코드2] 숫자 기준 정렬 — 파일명에 숫자가 있으면 숫자로, 없으면 문자열로 ──
            // "10.jpg" > "9.jpg" 같은 lexicographic 오류 방지
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
                if (_maxCatalogIndex.HasValue && _maxCatalogIndex.Value >= 0)
                    trkRecord.Maximum = Math.Max(_maxCatalogIndex.Value, Math.Max(0, _imageFiles.Count - 1));
                else
                    trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);
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
                picTubImage.Image?.Dispose(); picTubImage.Image = null;
                _currentIndex = -1;
                txtRecordNumber.Text = "기록 000000";
                _catalogLines.Clear();
                _currentThrottle = null; _currentAngle = null;
                picThrottle.Invalidate(); picAngle.Invalidate();
                LastImagePath = ""; LastAngle = null; LastThrottle = null;
            }
        }

        private void SetCurrentIndex(int idx)
        {
            if (idx < 0 || idx >= _imageFiles.Count) return;
            _currentIndex = idx;
            try
            {
                var path = _imageFiles[idx];
                using var fs = File.OpenRead(path);
                using var img = Image.FromStream(fs);
                picTubImage.Image?.Dispose();
                picTubImage.Image = new Bitmap(img);

                // ── 기록 번호 표시: 카탈로그 _index 우선, 없으면 배열 인덱스(0-base) ──
                // 코드1은 (_currentIndex + 1)로 +1 오프셋이 있었으나,
                // 카탈로그 _index가 0-base이므로 그대로 표시하는 것이 정확함.
                // catalogIndex가 없는 경우에도 _currentIndex(0-base)를 그대로 사용.
                int? catalogIndex = null;
                try
                {
                    var fname = Path.GetFileName(path);
                    if (!string.IsNullOrEmpty(fname) && _catalogIndexByFileName.TryGetValue(fname, out var val))
                        catalogIndex = val;
                    else if (_catalogLines != null && _catalogLines.Count > _currentIndex)
                    {
                        var parsed = ParseCatalogLine(_catalogLines[_currentIndex]);
                        catalogIndex = parsed.index;
                    }
                }
                catch { }

                // catalogIndex가 있으면 그 값, 없으면 0-base 인덱스 그대로
                txtRecordNumber.Text = catalogIndex.HasValue
                    ? $"기록 {catalogIndex.Value:D6}"
                    : $"기록 {_currentIndex:D6}";

                double? angle = null;
                double? throttle = null;

                if (_catalogLines != null && _catalogLines.Count > _currentIndex)
                {
                    var parsed = ParseCatalogLine(_catalogLines[_currentIndex]);

                    if (parsed.angle.HasValue) { _currentAngle = parsed.angle.Value; angle = parsed.angle.Value; }
                    else _currentAngle = null;
                    picAngle.Invalidate();

                    if (parsed.throttle.HasValue) { _currentThrottle = parsed.throttle.Value; throttle = parsed.throttle.Value; }
                    else _currentThrottle = null;
                    picThrottle.Invalidate();
                }
                else
                {
                    _currentAngle = null; _currentThrottle = null;
                    picAngle.Invalidate(); picThrottle.Invalidate();
                }

                LastImagePath = path;
                LastAngle = angle;
                LastThrottle = throttle;
                OnTubDataChanged?.Invoke(path, angle, throttle, _currentIndex, _imageFiles.Count);
            }
            catch { }
        }

        // ════════════════════════════════════════════════════════════
        // 게이지 Paint (코드1 기준)
        // ════════════════════════════════════════════════════════════

        private void PicThrottle_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = picThrottle.ClientRectangle;

            using (var b = new SolidBrush(Color.FromArgb(40, 40, 40)))
                g.FillRectangle(b, rect);

            var cx = rect.Left + rect.Width / 2f;
            var cy = rect.Top + rect.Height - 15f;
            var radius = Math.Min(rect.Width / 2f - 12f, rect.Height - 12f);
            if (radius <= 8) radius = Math.Min(rect.Width, rect.Height) / 2f;
            var arcRect = new RectangleF(cx - radius, cy - radius, radius * 2f, radius * 2f);

            double throttleVal = 0.0;
            var hasValue = _currentThrottle.HasValue;
            if (hasValue) throttleVal = _currentThrottle.Value;

            double tnorm;
            if (throttleVal >= -1.01 && throttleVal <= 1.01) tnorm = Math.Max(-1.0, Math.Min(1.0, throttleVal));
            else { tnorm = (throttleVal * 2.0) - 1.0; tnorm = Math.Max(-1.0, Math.Min(1.0, tnorm)); }

            int segments = 12;
            float segSweep = 180f / segments;
            float gap = 2f;
            float drawSweep = Math.Max(1f, segSweep - gap);
            float segPenWidth = Math.Max(12f, radius * 0.18f);

            for (int i = 0; i < segments; i++)
            {
                float pos = i / (float)(segments - 1);
                int rCol = (int)(pos * 255); int gCol = (int)((1 - pos) * 255);
                var segColor = Color.FromArgb(200, rCol, gCol, 0);
                using (var pen = new Pen(segColor, segPenWidth) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round })
                    g.DrawArc(pen, arcRect, 180f + i * segSweep + gap / 2f, drawSweep);
            }

            using (var tickPen = new Pen(Color.FromArgb(120, 120, 120), 1.5f))
                for (int i = 0; i <= 6; i++)
                {
                    var t = 180.0 - (i * 30.0); var rad = t * Math.PI / 180.0;
                    var inner = new PointF((float)(cx + Math.Cos(rad) * (radius - segPenWidth / 2f - 2)), (float)(cy + Math.Sin(rad) * (radius - segPenWidth / 2f - 2)));
                    var outer = new PointF((float)(cx + Math.Cos(rad) * (radius + segPenWidth / 2f + 2)), (float)(cy + Math.Sin(rad) * (radius + segPenWidth / 2f + 2)));
                    g.DrawLine(tickPen, inner, outer);
                }

            var angleDeg = 180.0 + ((tnorm + 1.0) / 2.0) * 180.0;
            var angleRad = angleDeg * Math.PI / 180.0;
            var nx = (float)(cx + Math.Cos(angleRad) * (radius - 14));
            var ny = (float)(cy + Math.Sin(angleRad) * (radius - 14));

            using (var outline = new Pen(Color.FromArgb(220, 0, 0, 0), 7f) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round })
                g.DrawLine(outline, cx, cy, nx, ny);
            using (var pen = new Pen(Color.Red, 3.5f) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round })
                g.DrawLine(pen, cx, cy, nx, ny);
            using (var hub = new SolidBrush(Color.White))
                g.FillEllipse(hub, cx - 5f, cy - 5f, 10f, 10f);

            var valText = hasValue ? throttleVal.ToString("0.000", CultureInfo.InvariantCulture) : "--";
            using (var f = new Font("맑은 고딕", 10))
            using (var b = new SolidBrush(Color.White))
                g.DrawString(valText, f, b, rect.Left + 8, rect.Top + rect.Height - 20);
        }

        private void PicAngle_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = picAngle.ClientRectangle;

            using (var b = new SolidBrush(Color.FromArgb(40, 40, 40)))
                g.FillRectangle(b, rect);

            var cx = rect.Left + rect.Width / 2f;
            var cy = rect.Top + rect.Height / 2f + 40f;
            var radius = Math.Min(rect.Width / 2f - 20f, rect.Height - 20f);
            if (radius <= 6) radius = Math.Min(rect.Width, rect.Height) / 2f;
            var arcRect = new RectangleF(cx - radius, cy - radius, radius * 2f, radius * 2f);

            using (var pen = new Pen(Color.FromArgb(220, 30, 144, 255), Math.Max(10f, radius * 0.14f)) { StartCap = System.Drawing.Drawing2D.LineCap.Flat, EndCap = System.Drawing.Drawing2D.LineCap.Flat })
                g.DrawArc(pen, arcRect, 180f, 90f);
            using (var pen = new Pen(Color.FromArgb(220, 255, 60, 60), Math.Max(10f, radius * 0.14f)) { StartCap = System.Drawing.Drawing2D.LineCap.Flat, EndCap = System.Drawing.Drawing2D.LineCap.Flat })
                g.DrawArc(pen, arcRect, 270f, 90f);

            double angDeg = 0.0;
            if (_currentAngle.HasValue) angDeg = Math.Max(-45.0, Math.Min(45.0, _currentAngle.Value));

            var angleDeg = 270.0 - (angDeg / 45.0) * 90.0;
            var angleRad = angleDeg * Math.PI / 180.0;
            var nx = (float)(cx + Math.Cos(angleRad) * (radius - 8));
            var ny = (float)(cy + Math.Sin(angleRad) * (radius - 8));

            using (var outline = new Pen(Color.FromArgb(200, 255, 255, 255), 6f) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round })
                g.DrawLine(outline, cx, cy, nx, ny);
            using (var pen = new Pen(Color.Yellow, 3.5f) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round })
                g.DrawLine(pen, cx, cy, nx, ny);
            using (var hub = new SolidBrush(Color.Black))
                g.FillEllipse(hub, cx - 4f, cy - 4f, 8f, 8f);
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
                .Where(p => { var fn = Path.GetFileName(p); return fn.StartsWith("catalog_", StringComparison.OrdinalIgnoreCase) || fn.EndsWith(".catalog", StringComparison.OrdinalIgnoreCase); })
                .OrderBy(p =>
                {
                    var fn = Path.GetFileNameWithoutExtension(p);
                    var m = Regex.Match(fn, @"catalog_(\d+)", RegexOptions.IgnoreCase);
                    if (m.Success && int.TryParse(m.Groups[1].Value, out var v)) return (0, v, fn);
                    var firstNum = ExtractFirstNumber(fn);
                    if (firstNum.HasValue) return (1, firstNum.Value, fn);
                    return (2, int.MaxValue, fn);
                })
                .ToList();

            foreach (var cf in catalogFiles)
                try { foreach (var l in File.ReadAllLines(cf)) if (!string.IsNullOrWhiteSpace(l)) _catalogLines.Add(l.Trim()); } catch { }

            // 파일명 → 카탈로그 인덱스 매핑 빌드
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

                    string? fname = null;
                    try
                    {
                        var doc = System.Text.Json.JsonDocument.Parse(line);
                        var root = doc.RootElement;
                        if (root.TryGetProperty("cam/image_array", out var pc) || root.TryGetProperty("cam_image_array", out pc) || root.TryGetProperty("cam/image", out pc) || root.TryGetProperty("cam_image", out pc))
                            if (pc.ValueKind == System.Text.Json.JsonValueKind.String) fname = pc.GetString();
                    }
                    catch { }

                    if (string.IsNullOrEmpty(fname))
                    {
                        var mm = Regex.Match(line, @"""cam(?:/|_)image(?:_array)?""\s*:\s*""([^""]+)""");
                        if (mm.Success) fname = mm.Groups[1].Value;
                    }

                    if (!string.IsNullOrEmpty(fname) && parsed.index.HasValue)
                        try { _catalogIndexByFileName[Path.GetFileName(fname)] = parsed.index.Value; } catch { }
                }
                catch { }
            }

            // 카탈로그 로드 후 트랙바 최대값 재조정
            if (_maxCatalogIndex.HasValue && _maxCatalogIndex.Value >= 0 && _imageFiles.Count > 0)
                trkRecord.Maximum = Math.Max(_maxCatalogIndex.Value, Math.Max(0, _imageFiles.Count - 1));
        }

        private int? ExtractFirstNumber(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            var m = Regex.Match(input, @"(\d+)");
            if (m.Success && int.TryParse(m.Groups[1].Value, out var v)) return v;
            return null;
        }

        private (long? timestamp, double? angle, string mode, double? throttle, int? index) ParseCatalogLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return (null, null, null, null, null);
            long? timestamp = null; double? angle = null; string mode = null; double? throttle = null; int? idx = null;

            try
            {
                var doc = System.Text.Json.JsonDocument.Parse(line);
                var root = doc.RootElement;

                if (root.TryGetProperty("_timestamp_ms", out var p) || root.TryGetProperty("timestamp_ms", out p) || root.TryGetProperty("timestamp", out p))
                {
                    if (p.ValueKind == System.Text.Json.JsonValueKind.Number && p.TryGetInt64(out var v)) timestamp = v;
                    else if (p.ValueKind == System.Text.Json.JsonValueKind.String && long.TryParse(p.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var vv)) timestamp = vv;
                }
                if (root.TryGetProperty("user/angle", out var pa) || root.TryGetProperty("angle", out pa) || root.TryGetProperty("user_angle", out pa))
                {
                    if (pa.ValueKind == System.Text.Json.JsonValueKind.Number && pa.TryGetDouble(out var dv)) angle = dv;
                    else if (pa.ValueKind == System.Text.Json.JsonValueKind.String && double.TryParse(pa.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var dv2)) angle = dv2;
                }
                if (root.TryGetProperty("user/throttle", out var pt) || root.TryGetProperty("throttle", out pt) || root.TryGetProperty("user_throttle", out pt))
                {
                    if (pt.ValueKind == System.Text.Json.JsonValueKind.Number && pt.TryGetDouble(out var dv)) throttle = dv;
                    else if (pt.ValueKind == System.Text.Json.JsonValueKind.String && double.TryParse(pt.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var dv2)) throttle = dv2;
                }
                if (root.TryGetProperty("_index", out var pi) || root.TryGetProperty("index", out pi))
                {
                    if (pi.ValueKind == System.Text.Json.JsonValueKind.Number && pi.TryGetInt32(out var iv)) idx = iv;
                    else if (pi.ValueKind == System.Text.Json.JsonValueKind.String && int.TryParse(pi.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var iv2)) idx = iv2;
                }
                if (root.TryGetProperty("user/mode", out var pm) || root.TryGetProperty("mode", out pm) || root.TryGetProperty("user_mode", out pm))
                    mode = pm.ValueKind == System.Text.Json.JsonValueKind.String ? pm.GetString() : pm.ToString();

                return (timestamp, angle, mode, throttle, idx);
            }
            catch { }

            double tmpd; long tmpl;
            string TryMatchNumber(string key) { var mm = Regex.Match(line, $"{Regex.Escape(key)}\\s*[:=]\\s*([-+]?\\d*\\.?\\d+(?:[eE][-+]?\\d+)?)"); return mm.Success ? mm.Groups[1].Value : null; }
            string TryMatchToken(string key) { var mm = Regex.Match(line, $"{Regex.Escape(key)}\\s*[:=]\\s*([^,;\\s]+)"); return mm.Success ? mm.Groups[1].Value : null; }

            var tsStr = TryMatchNumber("_timestamp_ms") ?? TryMatchNumber("timestamp_ms") ?? TryMatchNumber("timestamp");
            if (tsStr != null && long.TryParse(tsStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out tmpl)) timestamp = tmpl;
            var aStr = TryMatchNumber("user/angle") ?? TryMatchNumber("angle") ?? TryMatchNumber("user_angle");
            if (aStr != null && double.TryParse(aStr, NumberStyles.Float, CultureInfo.InvariantCulture, out tmpd)) angle = tmpd;
            var tStr = TryMatchNumber("user/throttle") ?? TryMatchNumber("throttle") ?? TryMatchNumber("user_throttle");
            if (tStr != null && double.TryParse(tStr, NumberStyles.Float, CultureInfo.InvariantCulture, out tmpd)) throttle = tmpd;
            var mStr = TryMatchToken("user/mode") ?? TryMatchToken("mode") ?? TryMatchToken("user_mode");
            if (!string.IsNullOrEmpty(mStr)) mode = mStr.Trim('"');
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
            text = text.Replace("×", "x").Trim();
            if (text.EndsWith("x", StringComparison.OrdinalIgnoreCase)) text = text.Substring(0, text.Length - 1).Trim();
            if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var speed) || speed <= 0) speed = 1.0;
            _playTimer.Interval = (int)Math.Max(10, Math.Round(_playBaseIntervalMs / speed));
        }

        private void BtnStartStop_Click(object? sender, EventArgs e) { if (_isPlaying) StopPlayback(); else StartPlayback(); }

        private void StartPlayback()
        {
            if (_imageFiles.Count == 0) return;
            _isPlaying = true; btnStartStop.Text = "정지 ⏹️"; _playTimer.Start();
        }

        private void StopPlayback()
        {
            _isPlaying = false; btnStartStop.Text = "시작 ▶️"; _playTimer.Stop();
        }

        // ════════════════════════════════════════════════════════════
        // 탐색
        // ════════════════════════════════════════════════════════════

        private void TrkRecord_Scroll(object? sender, EventArgs e) => SetCurrentIndex(trkRecord.Value);
        private void TrkRecord_ValueChanged(object? sender, EventArgs e) => SetCurrentIndex(trkRecord.Value);
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
        // 삭제 / 복원 / 저장 / 필터 (코드1 기준)
        // ════════════════════════════════════════════════════════════

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentIndex < 0 || _currentIndex >= _imageFiles.Count) return;
            string deletedImage = _imageFiles[_currentIndex];
            _deletedImages.Add(deletedImage); _deletedIndexes[deletedImage] = _currentIndex;
            _imageFiles.RemoveAt(_currentIndex);
            if (_currentIndex >= _imageFiles.Count) _currentIndex = _imageFiles.Count - 1;
            trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);
            if (_imageFiles.Count > 0) { trkRecord.Value = _currentIndex; SetCurrentIndex(_currentIndex); }
            else { picTubImage.Image?.Dispose(); picTubImage.Image = null; }
            UpdateDeleteStatus();
            pnlTimeline.Invalidate();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (_deletedImages.Count == 0) return;
            string restoreImage = _deletedImages[_deletedImages.Count - 1];
            int restoreIndex = _deletedIndexes[restoreImage];
            if (restoreIndex > _imageFiles.Count) restoreIndex = _imageFiles.Count;
            _imageFiles.Insert(restoreIndex, restoreImage);
            _deletedImages.Remove(restoreImage); _deletedIndexes.Remove(restoreImage);
            trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);
            trkRecord.Value = restoreIndex; SetCurrentIndex(restoreIndex);
            UpdateDeleteStatus(); pnlTimeline.Invalidate();
        }

        private void btnReroadTub_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_loadedFolderPath)) return;
            StopPlayback(); LoadImagesFromDirectory(_loadedFolderPath);
            MessageBox.Show("처음 상태로 다시 불러왔습니다.");
        }

        private void btnSetLeft_Click(object sender, EventArgs e) => trkRecord.Value = trkRecord.Minimum;
        private void btnSetRight_Click(object sender, EventArgs e) => trkRecord.Value = trkRecord.Maximum;

        private void menuThrottle_Click(object sender, EventArgs e) => ApplyFilter(true);
        private void menuAngle_Click(object sender, EventArgs e) => ApplyFilter(false);

        private void ApplyFilter(bool isThrottle)
        {
            string minInput = Microsoft.VisualBasic.Interaction.InputBox("최소값 입력", "필터 설정", "0");
            string maxInput = Microsoft.VisualBasic.Interaction.InputBox("최대값 입력", "필터 설정", "999999");
            if (!double.TryParse(minInput, out double minValue) || !double.TryParse(maxInput, out double maxValue))
            { MessageBox.Show("숫자를 입력하세요."); return; }
            if (minValue > maxValue) { MessageBox.Show("최소값이 최대값보다 클 수 없습니다."); return; }

            _imageFiles = _imageFiles
                .Where((img, idx) =>
                {
                    if (idx >= _catalogLines.Count) return false;
                    var parsed = ParseCatalogLine(_catalogLines[idx]);
                    double? value = isThrottle ? parsed.throttle : parsed.angle;
                    return value.HasValue && value.Value >= minValue && value.Value <= maxValue;
                })
                .ToList();

            trkRecord.Minimum = 0;
            trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);
            if (_imageFiles.Count > 0) { trkRecord.Value = 0; SetCurrentIndex(0); }
            MessageBox.Show($"필터 적용 완료 ({minValue} ~ {maxValue})");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "삭제된 파일이 실제로 저장되며 복구가 어려울 수 있습니다.\n그래도 저장하시겠습니까?",
                "저장 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            var toDelete = _deletedImages.ToList();
            int deletedCount = 0;
            foreach (string imagePath in toDelete)
                try { if (File.Exists(imagePath)) { File.Delete(imagePath); deletedCount++; } }
                catch (Exception ex) { MessageBox.Show("삭제 실패:\n" + ex.Message); }

            _deletedImages.Clear(); _deletedIndexes.Clear();
            UpdateDeleteStatus();
            MessageBox.Show($"{deletedCount}개 파일 저장 완료");
            pnlTimeline.Invalidate();
        }

        // ════════════════════════════════════════════════════════════
        // 필터 UI (코드1 기준 — pnlSpeedRange / pnlAngleRange)
        // ════════════════════════════════════════════════════════════

        private void pnlSpeedRange_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            int y = pnlSpeedRange.Height / 2;
            g.DrawLine(Pens.Gray, 20, y, pnlSpeedRange.Width - 20, y);
            using (var pen = new Pen(Color.DodgerBlue, 4)) g.DrawLine(pen, _minX, y, _maxX, y);
            g.FillEllipse(Brushes.DodgerBlue, _minX - 8, y - 8, 16, 16);
            g.FillEllipse(Brushes.DodgerBlue, _maxX - 8, y - 8, 16, 16);
        }

        private void pnlSpeedRange_MouseDown(object sender, MouseEventArgs e)
        {
            if (Math.Abs(e.X - _minX) < 15) _dragMin = true;
            if (Math.Abs(e.X - _maxX) < 15) _dragMax = true;
        }
        private void pnlSpeedRange_MouseUp(object sender, MouseEventArgs e) { _dragMin = false; _dragMax = false; }
        private void pnlSpeedRange_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragMin) { _minX = Math.Max(20, Math.Min(e.X, _maxX)); nudSpeedMin.Value = Math.Min(nudSpeedMin.Maximum, XToSpeed(_minX)); pnlSpeedRange.Invalidate(); }
            if (_dragMax) { _maxX = Math.Min(815, Math.Max(e.X, _minX)); nudSpeedMax.Value = Math.Min(nudSpeedMax.Maximum, XToSpeed(_maxX)); pnlSpeedRange.Invalidate(); }
        }

        private decimal XToSpeed(int x) { x = Math.Max(20, Math.Min(x, 815)); return (decimal)((double)(x - 20) / (815 - 20)); }
        private int SpeedToX(decimal speed) { return 20 + (int)((double)speed * (815 - 20)); }
        private void nudSpeedMin_ValueChanged(object sender, EventArgs e) { _minX = SpeedToX(nudSpeedMin.Value); pnlSpeedRange.Invalidate(); }
        private void nudSpeedMax_ValueChanged(object sender, EventArgs e) { _maxX = SpeedToX(nudSpeedMax.Value); pnlSpeedRange.Invalidate(); }

        private decimal XToAngle(int x) { x = Math.Max(20, Math.Min(x, 815)); return (decimal)(((double)(x - 20) / (815 - 20)) * 20 - 10); }
        private int AngleToX(decimal angle) { return 20 + (int)(((double)angle + 10) / 20 * (815 - 20)); }
        private void nudAngleMin_ValueChanged(object sender, EventArgs e) { _angleMinX = AngleToX(nudAngleMin.Value); pnlAngleRange.Invalidate(); }
        private void nudAngleMax_ValueChanged(object sender, EventArgs e) { _angleMaxX = AngleToX(nudAngleMax.Value); pnlAngleRange.Invalidate(); }

        private void pnlAngleRange_MouseDown(object sender, MouseEventArgs e)
        {
            if (Math.Abs(e.X - _angleMinX) < 15) _dragAngleMin = true;
            if (Math.Abs(e.X - _angleMaxX) < 15) _dragAngleMax = true;
        }
        private void pnlAngleRange_MouseUp(object sender, MouseEventArgs e) { _dragAngleMin = false; _dragAngleMax = false; }
        private void pnlAngleRange_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragAngleMin) { _angleMinX = Math.Max(20, Math.Min(e.X, _angleMaxX)); nudAngleMin.Value = Math.Min(nudAngleMin.Maximum, XToAngle(_angleMinX)); pnlAngleRange.Invalidate(); }
            if (_dragAngleMax) { _angleMaxX = Math.Min(815, Math.Max(e.X, _angleMinX)); nudAngleMax.Value = Math.Max(nudAngleMax.Minimum, XToAngle(_angleMaxX)); pnlAngleRange.Invalidate(); }
        }
        private void pnlAngleRange_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            int y = pnlAngleRange.Height / 2;
            g.DrawLine(Pens.Gray, 20, y, pnlAngleRange.Width - 20, y);
            using (var pen = new Pen(Color.DodgerBlue, 4)) g.DrawLine(pen, _angleMinX, y, _angleMaxX, y);
            g.FillEllipse(Brushes.DodgerBlue, _angleMinX - 8, y - 8, 16, 16);
            g.FillEllipse(Brushes.DodgerBlue, _angleMaxX - 8, y - 8, 16, 16);
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            double speedMin = (double)nudSpeedMin.Value;
            double speedMax = (double)nudSpeedMax.Value;
            double angleMin = (double)nudAngleMin.Value / 10.0;
            double angleMax = (double)nudAngleMax.Value / 10.0;

            _filteredImages.Clear();
            _imageFiles = _allImageFiles
                .Where((img, idx) =>
                {
                    if (idx >= _catalogLines.Count) return false;
                    var parsed = ParseCatalogLine(_catalogLines[idx]);
                    return parsed.throttle.HasValue && parsed.angle.HasValue &&
                           parsed.throttle.Value >= speedMin && parsed.throttle.Value <= speedMax &&
                           parsed.angle.Value >= angleMin && parsed.angle.Value <= angleMax;
                })
                .ToList();

            foreach (var img in _allImageFiles)
                if (!_imageFiles.Contains(img)) _filteredImages.Add(img);

            pnlTimeline.Invalidate();
            trkRecord.Minimum = 0;
            trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);
            if (_imageFiles.Count > 0) { trkRecord.Value = 0; SetCurrentIndex(0); }
            MessageBox.Show($"필터 적용 완료\n남은 프레임: {_imageFiles.Count}개");
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            _imageFiles = _allImageFiles.ToList();
            trkRecord.Minimum = 0;
            trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);
            if (_imageFiles.Count > 0) { trkRecord.Value = 0; SetCurrentIndex(0); }
            MessageBox.Show("필터 해제 완료");
            _filteredImages.Clear();
            pnlTimeline.Invalidate();
        }

        // ════════════════════════════════════════════════════════════
        // 타임라인 패널 (코드1 기준)
        // ════════════════════════════════════════════════════════════

        private void pnlTimeline_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            g.FillRectangle(Brushes.DodgerBlue, 0, 0, pnlTimeline.Width, pnlTimeline.Height);

            int totalFrames = _allImageFiles.Count;
            if (totalFrames == 0) return;

            foreach (string deletedImage in _deletedImages)
            {
                int index = _allImageFiles.IndexOf(deletedImage); if (index < 0) continue;
                int startX = index * pnlTimeline.Width / totalFrames;
                int width = Math.Max(1, (index + 1) * pnlTimeline.Width / totalFrames - startX);
                g.FillRectangle(Brushes.Red, startX, 0, width, pnlTimeline.Height);
            }
            foreach (string filteredImage in _filteredImages)
            {
                int index = _allImageFiles.IndexOf(filteredImage); if (index < 0) continue;
                int startX = index * pnlTimeline.Width / totalFrames;
                int width = Math.Max(1, (index + 1) * pnlTimeline.Width / totalFrames - startX);
                g.FillRectangle(Brushes.Red, startX, 0, width, pnlTimeline.Height);
            }

            if (_imageFiles.Count == 0) return;
            int currentX = _currentIndex * pnlTimeline.Width / _imageFiles.Count;
            g.DrawLine(Pens.Yellow, currentX, 0, currentX, pnlTimeline.Height);
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
    }
}