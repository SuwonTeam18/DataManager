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
        private Dictionary<string, int> _deletedIndexes = new Dictionary<string, int>();
        private int _currentIndex = -1;
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
                // find digits in the text
                var m = System.Text.RegularExpressions.Regex.Match(txt, "(\\d+)");
                if (!m.Success) return;
                if (!int.TryParse(m.Groups[1].Value, out var num)) return;
                if (num <= 0) num = 1;
                var targetIndex = num - 1;
                if (_imageFiles == null || _imageFiles.Count == 0)
                {
                    txtRecordNumber.Text = "기록 000000";
                    return;
                }
                if (targetIndex < 0) targetIndex = 0;
                if (targetIndex >= _imageFiles.Count) targetIndex = _imageFiles.Count - 1;
                if (trkRecord.Enabled)
                {
                    trkRecord.Value = targetIndex;
                    // Update textbox to show normalized value immediately; SetCurrentIndex will also refresh it.
                    txtRecordNumber.Text = $"기록 {(targetIndex + 1):D6}";
                }
                else
                {
                    SetCurrentIndex(targetIndex);
                }
            }
            catch { }
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
                // If the selected file resides in an "images" (or "image") folder, set txtCarDirectory to its parent folder
                var dir = Path.GetDirectoryName(dlg.FileName) ?? string.Empty;
                if (!string.IsNullOrEmpty(dir))
                {
                    var dinfo = new DirectoryInfo(dir);
                    if (string.Equals(dinfo.Name, "images", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(dinfo.Name, "image", StringComparison.OrdinalIgnoreCase))
                    {
                        var parent = dinfo.Parent;
                        if (parent != null)
                        {
                            // include trailing directory separator to match example
                            txtCarDirectory.Text = parent.FullName.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                                + Path.DirectorySeparatorChar;
                        }
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
                    string.Equals(Path.GetFullPath(p), Path.GetFullPath(dlg.FileName),
                    StringComparison.OrdinalIgnoreCase));
                if (idx >= 0) SetCurrentIndex(idx);
            }
        }

        private void LoadImagesFromDirectory(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
            {
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
            _loadedFolderPath = folder;

            _deletedImages.Clear();
            _deletedIndexes.Clear();
            UpdateDeleteStatus();

            if (_imageFiles.Count > 0)
            {
                trkRecord.Minimum = 0;
                // If catalog indicates a larger final index, use that as maximum so timeline matches catalog
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
            if (idx < 0 || idx >= _imageFiles.Count) return;
            _currentIndex = idx;
            try
            {
                var path = _imageFiles[idx];
                using var fs = File.OpenRead(path);
                using var img = Image.FromStream(fs);
                picTubImage.Image?.Dispose();
                picTubImage.Image = new Bitmap(img);

                // 기본: 표시할 기록 번호는 catalog에 있는 _index 값을 사용
                // Prefer mapping by filename: if catalog contains a mapping from filename->_index, use it.
                int? catalogIndex = null;
                try
                {
                    var fname = Path.GetFileName(path);
                    if (!string.IsNullOrEmpty(fname) && _catalogIndexByFileName.TryGetValue(fname, out var val))
                    {
                        catalogIndex = val;
                    }
                    else if (_catalogLines != null && _catalogLines.Count > _currentIndex)
                    {
                        var parsed = ParseCatalogLine(_catalogLines[_currentIndex]);
                        catalogIndex = parsed.index;
                    }
                }
                catch { }

                if (catalogIndex.HasValue)
                    txtRecordNumber.Text = $"기록 {catalogIndex.Value:D6}";
                else
                    txtRecordNumber.Text = $"기록 {(_currentIndex):D6}";

                double? angle = null;
                double? throttle = null;

                if (_catalogLines != null && _catalogLines.Count > _currentIndex)
                {
                    var parsed = ParseCatalogLine(_catalogLines[_currentIndex]);

                    // ── 각도 ────────────────────────────────────────
                    if (parsed.angle.HasValue)
                    {
                        _currentAngle = parsed.angle.Value;
                        angle = parsed.angle.Value;
                    }
                    else
                    {
                        _currentAngle = null;
                    }
                    picAngle.Invalidate();

                    // ── 속도 ────────────────────────────────────────
                    if (parsed.throttle.HasValue)
                    {
                        _currentThrottle = parsed.throttle.Value;
                        throttle = parsed.throttle.Value;
                    }
                    else
                    {
                        _currentThrottle = null;
                    }
                    picThrottle.Invalidate();
                }
                else
                {
                    _currentAngle = null;
                    _currentThrottle = null;
                    picAngle.Invalidate();
                    picThrottle.Invalidate();
                }

                // ── PilotArena 연동: 정적 상태 저장 + 이벤트 발생 ──
                LastImagePath = path;
                LastAngle = angle;
                LastThrottle = throttle;
                OnTubDataChanged?.Invoke(path, angle, throttle, _currentIndex, _imageFiles.Count);
            }
            catch { }
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

            var cx = rect.Left + rect.Width / 2f;
            var cy = rect.Top + rect.Height - 15f;
            var radius = Math.Min(rect.Width / 2f - 12f, rect.Height - 12f);
            if (radius <= 8) radius = Math.Min(rect.Width, rect.Height) / 2f;
            var arcRect = new RectangleF(cx - radius, cy - radius, radius * 2f, radius * 2f);

            double throttleVal = 0.0;
            var hasValue = _currentThrottle.HasValue;
            if (hasValue) throttleVal = _currentThrottle.Value;

            double tnorm;
            if (throttleVal >= -1.01 && throttleVal <= 1.01)
                tnorm = Math.Max(-1.0, Math.Min(1.0, throttleVal));
            else
            {
                tnorm = (throttleVal * 2.0) - 1.0;
                tnorm = Math.Max(-1.0, Math.Min(1.0, tnorm));
            }

            int segments = 12;
            float segSweep = 180f / segments;
            float gap = 2f;
            float drawSweep = Math.Max(1f, segSweep - gap);
            float segPenWidth = Math.Max(12f, radius * 0.18f);

            for (int i = 0; i < segments; i++)
            {
                float pos = i / (float)(segments - 1);
                int rCol = (int)(pos * 255);
                int gCol = (int)((1 - pos) * 255);
                var segColor = Color.FromArgb(200, rCol, gCol, 0);
                using (var pen = new Pen(segColor, segPenWidth)
                {
                    StartCap = System.Drawing.Drawing2D.LineCap.Round,
                    EndCap = System.Drawing.Drawing2D.LineCap.Round
                })
                    g.DrawArc(pen, arcRect, 180f + i * segSweep + gap / 2f, drawSweep);
            }

            using (var tickPen = new Pen(Color.FromArgb(120, 120, 120), 1.5f))
            {
                for (int i = 0; i <= 6; i++)
                {
                    var t = 180.0 - (i * 30.0);
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

            var angleDeg = 180.0 + ((tnorm + 1.0) / 2.0) * 180.0;
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

            using (var pen = new Pen(Color.FromArgb(220, 30, 144, 255), Math.Max(10f, radius * 0.14f))
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Flat,
                EndCap = System.Drawing.Drawing2D.LineCap.Flat
            })
                g.DrawArc(pen, arcRect, 180f, 90f);

            using (var pen = new Pen(Color.FromArgb(220, 255, 60, 60), Math.Max(10f, radius * 0.14f))
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Flat,
                EndCap = System.Drawing.Drawing2D.LineCap.Flat
            })
                g.DrawArc(pen, arcRect, 270f, 90f);

            double angDeg = 0.0;
            if (_currentAngle.HasValue)
                angDeg = Math.Max(-45.0, Math.Min(45.0, _currentAngle.Value));

            var angleDeg = 270.0 - (angDeg / 45.0) * 90.0;
            var angleRad = angleDeg * Math.PI / 180.0;
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
            btnStartStop.Text = "정지 ⏹️";
            _playTimer.Start();
        }

        private void StopPlayback()
        {
            _isPlaying = false;
            btnStartStop.Text = "시작 ▶️";
            _playTimer.Stop();
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
        // 삭제 / 복원 / 저장 / 필터
        // ════════════════════════════════════════════════════════════

        private void btnSetFillter_Click(object sender, EventArgs e)
        {
            contextFilter.Show(btnSetFillter, 0, btnSetFillter.Height);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentIndex < 0 || _currentIndex >= _imageFiles.Count) return;

            string deletedImage = _imageFiles[_currentIndex];
            _deletedImages.Add(deletedImage);
            _deletedIndexes[deletedImage] = _currentIndex;
            _imageFiles.RemoveAt(_currentIndex);

            if (_currentIndex >= _imageFiles.Count)
                _currentIndex = _imageFiles.Count - 1;

            trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);

            if (_imageFiles.Count > 0)
            {
                trkRecord.Value = _currentIndex;
                SetCurrentIndex(_currentIndex);
            }
            else
            {
                picTubImage.Image?.Dispose();
                picTubImage.Image = null;
            }
            UpdateDeleteStatus();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (_deletedImages.Count == 0) return;

            string restoreImage = _deletedImages[_deletedImages.Count - 1];
            int restoreIndex = _deletedIndexes[restoreImage];
            if (restoreIndex > _imageFiles.Count) restoreIndex = _imageFiles.Count;

            _imageFiles.Insert(restoreIndex, restoreImage);
            _deletedImages.Remove(restoreImage);
            _deletedIndexes.Remove(restoreImage);

            trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);
            trkRecord.Value = restoreIndex;
            SetCurrentIndex(restoreIndex);
            UpdateDeleteStatus();
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
            string input = Microsoft.VisualBasic.Interaction.InputBox("최소값 입력", "필터 설정", "0");
            if (!double.TryParse(input, out double minValue))
            {
                MessageBox.Show("숫자를 입력하세요.");
                return;
            }

            _imageFiles = _imageFiles
                .Where((img, idx) =>
                {
                    if (idx >= _catalogLines.Count) return false;
                    var parsed = ParseCatalogLine(_catalogLines[idx]);
                    return isThrottle
                        ? parsed.throttle.HasValue && parsed.throttle.Value > minValue
                        : parsed.angle.HasValue && Math.Abs(parsed.angle.Value) > minValue;
                })
                .ToList();

            trkRecord.Minimum = 0;
            trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);
            if (_imageFiles.Count > 0) { trkRecord.Value = 0; SetCurrentIndex(0); }
            MessageBox.Show("필터 적용 완료");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
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