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
        // drawn throttle gauge state
        private double? _currentThrottle = null; // range expected -1.0 .. +1.0 or 0..1 depending on data
        // drawn angle gauge state
        private double? _currentAngle = null; // in degrees, 0 = center
        private List<string> _imageFiles = new List<string>();
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
        private double _playBaseIntervalMs = 100.0; // base interval for 1.0x speed
        private List<string> _catalogLines = new List<string>();

        public ucTubManager()
        {
            InitializeComponent();

            // mycar 사진 및 데이터 디렉토리 선택
            btnLoadCarDirectory.Click += BtnLoadCarDirectory_Click;
            btnLoadTub.Click += BtnLoadTub_Click;
            trkRecord.Scroll += TrkRecord_Scroll;
            trkRecord.ValueChanged += TrkRecord_ValueChanged;
            // click handlers are handled via MouseDown/MouseUp to support hold-to-repeat behavior
            // continuous advance when holding buttons with an initial delay
            _prevTimer = new System.Windows.Forms.Timer { Interval = 100 };
            _nextTimer = new System.Windows.Forms.Timer { Interval = 100 };
            _prevTimer.Tick += (s, ea) => MovePrev();
            _nextTimer.Tick += (s, ea) => MoveNext();

            // initial delay timers before starting continuous repeat
            _prevInitialTimer = new System.Windows.Forms.Timer { Interval = 300 }; // initial delay ms
            _nextInitialTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _prevInitialTimer.Tick += (s, ea) => { _prevInitialTimer.Stop(); _prevTimer.Start(); };
            _nextInitialTimer.Tick += (s, ea) => { _nextInitialTimer.Stop(); _nextTimer.Start(); };

            // MouseDown: do one step immediately, then start initial-delay timer which will start repeat timer
            btnPrev.MouseDown += (s, ea) => { if (ea.Button == MouseButtons.Left) { MovePrev(); _prevInitialTimer.Start(); } };
            btnPrev.MouseUp += (s, ea) => { if (ea.Button == MouseButtons.Left) { _prevInitialTimer.Stop(); _prevTimer.Stop(); } };
            btnPrev.MouseLeave += (s, ea) => { _prevInitialTimer.Stop(); _prevTimer.Stop(); };

            btnNext.MouseDown += (s, ea) => { if (ea.Button == MouseButtons.Left) { MoveNext(); _nextInitialTimer.Start(); } };
            btnNext.MouseUp += (s, ea) => { if (ea.Button == MouseButtons.Left) { _nextInitialTimer.Stop(); _nextTimer.Stop(); } };
            btnNext.MouseLeave += (s, ea) => { _nextInitialTimer.Stop(); _nextTimer.Stop(); };

            // fast skip buttons (step 100) with hold-to-repeat
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

            // playback timer (Start/Stop)
            _playTimer = new System.Windows.Forms.Timer { Interval = 100 }; // 100ms per frame (~10 FPS)
            _playTimer.Tick += (s, ea) =>
            {
                // on each tick advance; if at end, stop playback
                if (_imageFiles.Count == 0) return;
                if (_currentIndex >= _imageFiles.Count - 1)
                {
                    StopPlayback();
                    return;
                }




                MoveNext();
            };

            // use btnStartStop as Start/Stop toggle: initialize text
            btnStartStop.Text = "재생";
            btnStartStop.Click += BtnStartStop_Click;

            // initialize speed combo box if empty and wire events
            if (cmbSpeed.Items.Count == 0)
            {
                cmbSpeed.Items.AddRange(new object[] { "0.25", "0.50", "0.75", "1.00", "1.25", "1.50", "1.75", "2.00" });
            }
            cmbSpeed.Text = "1.00";
            cmbSpeed.SelectedIndexChanged += (s, e) => UpdatePlaybackIntervalFromCombo();
            cmbSpeed.TextChanged += (s, e) => UpdatePlaybackIntervalFromCombo();
            UpdatePlaybackIntervalFromCombo();

            // setup custom throttle paint
            picThrottle.Paint += PicThrottle_Paint;
            // setup angle paint
            picAngle.Paint += PicAngle_Paint;
        }

        private void BtnLoadCarDirectory_Click(object? sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog();
            dlg.Description = "Select car directory";
            dlg.UseDescriptionForTitle = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // txtCarDirectory is the designer name for the car directory textbox
                txtCarDirectory.Text = dlg.SelectedPath;
                LoadImagesFromDirectory(dlg.SelectedPath);
            }
        }

        // Extract the first continuous integer found in the input string (returns null if none)
        private int? ExtractFirstNumber(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            var m = Regex.Match(input, "(\\d+)");
            if (m.Success && int.TryParse(m.Groups[1].Value, out var v)) return v;
            return null;
        }

        private void UpdatePlaybackIntervalFromCombo()
        {
            // parse speed value from cmbSpeed (e.g. "1.00", "0.50", "2.00")
            if (cmbSpeed == null) return;
            var text = cmbSpeed.Text?.Trim() ?? string.Empty;
            if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var speed) || speed <= 0)
            {
                speed = 1.0;
            }

            // interval should be baseInterval / speed. Clamp minimum interval to avoid too fast.
            var interval = (int)Math.Max(10, Math.Round(_playBaseIntervalMs / speed));
            _playTimer.Interval = interval;
        }
        private void BtnStartStop_Click(object? sender, EventArgs e)
        {
            if (_isPlaying)
            {
                StopPlayback();
            }
            else
            {
                StartPlayback();
            }
        }

        private void StartPlayback()
        {
            if (_imageFiles.Count == 0) return;
            _isPlaying = true;
            btnStartStop.Text = "정지";
            _playTimer.Start();
        }

        private void StopPlayback()
        {
            _isPlaying = false;
            btnStartStop.Text = "재생";
            _playTimer.Stop();
        }

        private void BtnLoadTub_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog();
            dlg.Title = "Select tub file";
            dlg.Filter = "Tub files (*.json;*.csv;*.jpg;*.jpeg;*.png)|*.json;*.csv;*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            dlg.CheckFileExists = true;
            dlg.Multiselect = false;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // txtTub is the designer name for the tub/file textbox
                txtTub.Text = dlg.FileName;

                // if the selected file is an image, load images from its folder and show that image
                var ext = Path.GetExtension(dlg.FileName)?.ToLowerInvariant();
                var imageExts = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                if (!string.IsNullOrEmpty(ext) && imageExts.Contains(ext))
                {
                    var folder = Path.GetDirectoryName(dlg.FileName) ?? string.Empty;
                    LoadImagesFromDirectory(folder);
                    // set current index to the selected file
                    var idx = _imageFiles.FindIndex(p => string.Equals(Path.GetFullPath(p), Path.GetFullPath(dlg.FileName), StringComparison.OrdinalIgnoreCase));
                    if (idx >= 0)
                    {
                        SetCurrentIndex(idx);
                    }
                }
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
            var files = exts.SelectMany(e => Directory.GetFiles(folder, e, SearchOption.TopDirectoryOnly)).ToList();
            files.Sort(StringComparer.OrdinalIgnoreCase);
            _imageFiles = files;

            if (_imageFiles.Count > 0)
            {
                trkRecord.Minimum = 0;
                trkRecord.Maximum = Math.Max(0, _imageFiles.Count - 1);
                trkRecord.Value = 0;
                trkRecord.Enabled = true;
                SetCurrentIndex(0);
                // try load .catalog / catalog_* files from the car directory or its parent if present
                try
                {
                    LoadCatalogsFromDirectory(folder);
                    // if not found, also try parent (catalogs might live next to images folder)
                    if (_catalogLines.Count == 0)
                    {
                        var parent = Path.GetDirectoryName(folder);
                        if (!string.IsNullOrEmpty(parent) && Directory.Exists(parent))
                        {
                            LoadCatalogsFromDirectory(parent);
                        }
                    }
                }
                catch
                {
                    // ignore errors reading catalogs
                }
            }
            else
            {
                trkRecord.Enabled = false;
                picTubImage.Image?.Dispose();
                picTubImage.Image = null;
                _currentIndex = -1;
                // no images -> reset record label
                lblRecordNumber.Text = "기록 000000";
                // clear catalog lines
                _catalogLines.Clear();
            }
        }

        private void SetCurrentIndex(int idx)
        {
            if (idx < 0 || idx >= _imageFiles.Count) return;
            _currentIndex = idx;
            try
            {
                var path = _imageFiles[idx];
                // load without locking file
                using var fs = File.OpenRead(path);
                var img = Image.FromStream(fs);
                // assign a copy to picturebox
                picTubImage.Image?.Dispose();
                picTubImage.Image = new Bitmap(img);
                // update record label with 6-digit zero padded index (1-based)
                lblRecordNumber.Text = $"기록 {(_currentIndex + 1).ToString("D6")}";
                // update catalog-based labels if available
                if (_catalogLines != null && _catalogLines.Count > _currentIndex)
                {
                    var line = _catalogLines[_currentIndex];
                    var parsed = ParseCatalogLine(line);
                    // timestamp label removed

                    if (parsed.angle.HasValue)
                    {
                        // round at 4th decimal -> display 3 decimal places
                        var a = Math.Round(parsed.angle.Value, 3);
                        lblAngleValue.Text = (a >= 0 ? "+" : string.Empty) + a.ToString("0.000", CultureInfo.InvariantCulture);
                        _currentAngle = parsed.angle.Value;
                        picAngle.Invalidate();
                    }
                    else
                    {
                        lblAngleValue.Text = string.Empty;
                        _currentAngle = null;
                        picAngle.Invalidate();
                    }

                    // mode label removed

                    if (parsed.throttle.HasValue)
                    {
                        var t = Math.Round(parsed.throttle.Value, 3);
                        lblThrottleValue.Text = (t >= 0 ? "+" : string.Empty) + t.ToString("0.000", CultureInfo.InvariantCulture);
                        _currentThrottle = parsed.throttle.Value;
                        // request redraw of throttle picture
                        picThrottle.Invalidate();
                    }
                    else
                    {
                        lblThrottleValue.Text = string.Empty;
                        _currentThrottle = null;
                        picThrottle.Invalidate();
                    }
                }
                else
                {
                    // clear labels if no catalog
                    lblAngleValue.Text = string.Empty;
                    lblThrottleValue.Text = string.Empty;
                    _currentThrottle = null;
                    picThrottle.Invalidate();
                }
            }
            catch
            {
                // ignore load errors
            }
        }

        private void PicThrottle_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = picThrottle.ClientRectangle;

            // background
            using (var b = new SolidBrush(Color.FromArgb(40, 40, 40)))
            {
                g.FillRectangle(b, rect);
            }

            // semicircle center (centered horizontally, near bottom of control)
            var cx = rect.Left + rect.Width / 2f;
            var cy = rect.Top + rect.Height - 15f; // slight padding from bottom
            var radius = Math.Min(rect.Width / 2f - 12f, rect.Height - 12f);
            if (radius <= 8) radius = Math.Min(rect.Width, rect.Height) / 2f;
            var arcRect = new RectangleF(cx - radius, cy - radius, radius * 2f, radius * 2f);

            // determine throttle normalized to -1..1 (used for fill progress)
            double throttleVal = 0.0;
            var hasValue = _currentThrottle.HasValue;
            if (hasValue)
            {
                throttleVal = _currentThrottle.Value;
            }

            // normalize: if value in [-1,1], use directly; otherwise assume [0,1] and map to -1..1
            double tnorm;
            if (throttleVal >= -1.01 && throttleVal <= 1.01)
            {
                tnorm = Math.Max(-1.0, Math.Min(1.0, throttleVal));
            }
            else
            {
                // map 0..1 -> -1..1
                tnorm = (throttleVal * 2.0) - 1.0;
                tnorm = Math.Max(-1.0, Math.Min(1.0, tnorm));
            }

            // segmented colored arc (thicker)
            int segments = 12;
            float segSweep = 180f / segments;
            float gap = 2f; // degrees gap between segments
            float drawSweep = Math.Max(1f, segSweep - gap);
            float segPenWidth = Math.Max(12f, radius * 0.18f);

            for (int i = 0; i < segments; i++)
            {
                // position along 0..1 from left to right
                float pos = i / (float)(segments - 1);

                // determine color for this segment (green -> red)
                int rCol = (int)(pos * 255);
                int gCol = (int)((1 - pos) * 255);
                var segColor = Color.FromArgb(200, rCol, gCol, 0);

                // always show colored segments (semi-opaque) like a real speedometer background
                var penColor = segColor;

                using (var pen = new Pen(penColor, segPenWidth) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round })
                {
                    var start = 180f + i * segSweep + gap / 2f;
                    g.DrawArc(pen, arcRect, start, drawSweep);
                }
            }

            // small ticks over segments (optional subtle)
            using (var tickPen = new Pen(Color.FromArgb(120, 120, 120), 1.5f))
            {
                int ticks = 6;
                for (int i = 0; i <= ticks; i++)
                {
                    var t = 180.0 - (i * (180.0 / ticks)); // 180..0
                    var rad = t * Math.PI / 180.0;
                    var inner = new PointF((float)(cx + Math.Cos(rad) * (radius - segPenWidth / 2f - 2)), (float)(cy + Math.Sin(rad) * (radius - segPenWidth / 2f - 2)));
                    var outer = new PointF((float)(cx + Math.Cos(rad) * (radius + segPenWidth / 2f + 2)), (float)(cy + Math.Sin(rad) * (radius + segPenWidth / 2f + 2)));
                    g.DrawLine(tickPen, inner, outer);
                }
            }



            // map normalized throttle (-1..1) to angle along semicircle: -1->180deg (left), 0->90deg (top), +1->0deg (right)
            var angleDeg = 180.0 + ((tnorm + 1.0) / 2.0) * 180.0;
            var angleRad = angleDeg * Math.PI / 180.0;

            // needle end point
            var nx = (float)(cx + Math.Cos(angleRad) * (radius - 14));
            var ny = (float)(cy + Math.Sin(angleRad) * (radius - 14));
            // draw an outline for visibility
            using (var outline = new Pen(Color.FromArgb(220, 0, 0, 0), 7f) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round })
            {
                g.DrawLine(outline, cx, cy, nx, ny);
            }
            // draw main red needle on top
            using (var pen = new Pen(Color.Red, 3.5f) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round })
            {
                g.DrawLine(pen, cx, cy, nx, ny);
            }

            // center hub
            using (var hub = new SolidBrush(Color.White))
            {
                g.FillEllipse(hub, cx - 5f, cy - 5f, 10f, 10f);
            }

            // numeric display
            var valText = hasValue ? throttleVal.ToString("0.000", CultureInfo.InvariantCulture) : "--";
            using (var f = new Font("맑은 고딕", 10))
            using (var b = new SolidBrush(Color.White))
            {
                var textPosX = rect.Left + 8;
                var textPosY = rect.Top + rect.Height - 20;
                g.DrawString(valText, f, b, textPosX, textPosY);
            }
        }

        private void PicAngle_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = picAngle.ClientRectangle;

            // background
            using (var b = new SolidBrush(Color.FromArgb(40, 40, 40)))
            {
                g.FillRectangle(b, rect);
            }

            // semicircle center
            var cx = rect.Left + rect.Width / 2f;
            var cy = rect.Top + rect.Height / 2f + 40f;
            var radius = Math.Min(rect.Width / 2f - 20f, rect.Height - 20f);
            if (radius <= 6) radius = Math.Min(rect.Width, rect.Height) / 2f;

            var arcRect = new RectangleF(cx - radius, cy - radius, radius * 2f, radius * 2f);

            // left half (blue)
            using (var pen = new Pen(Color.FromArgb(220, 30, 144, 255), Math.Max(10f, radius * 0.14f)) { StartCap = System.Drawing.Drawing2D.LineCap.Flat, EndCap = System.Drawing.Drawing2D.LineCap.Flat })
            {
                g.DrawArc(pen, arcRect, 180f, 90f);
            }

            // right half (red)
            using (var pen = new Pen(Color.FromArgb(220, 255, 60, 60), Math.Max(10f, radius * 0.14f)) { StartCap = System.Drawing.Drawing2D.LineCap.Flat, EndCap = System.Drawing.Drawing2D.LineCap.Flat })
            {
                g.DrawArc(pen, arcRect, 270f, 90f);
            }

            // compute needle angle from _currentAngle (degrees)
            double angDeg = 0.0;
            if (_currentAngle.HasValue)
            {
                var maxAngle = 45.0;
                angDeg = Math.Max(-maxAngle, Math.Min(maxAngle, _currentAngle.Value));
            }

            var angleDeg = 270.0 - (angDeg / 45.0) * 90.0;
            var angleRad = angleDeg * Math.PI / 180.0;

            var nx = (float)(cx + Math.Cos(angleRad) * (radius - 8));
            var ny = (float)(cy + Math.Sin(angleRad) * (radius - 8));

            // outline then black needle
            using (var outline = new Pen(Color.FromArgb(200, 255, 255, 255), 6f) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round })
            {
                g.DrawLine(outline, cx, cy, nx, ny);
            }
            using (var pen = new Pen(Color.Yellow, 3.5f) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round })
            {
                g.DrawLine(pen, cx, cy, nx, ny);
            }

            using (var hub = new SolidBrush(Color.Black))
            {
                g.FillEllipse(hub, cx - 4f, cy - 4f, 8f, 8f);
            }
        }

        private void LoadCatalogsFromDirectory(string folder)
        {
            _catalogLines.Clear();
            if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder)) return;
            // collect files starting with "catalog_" (any extension) or ending with .catalog
            var allFiles = Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly);
            var catalogFiles = allFiles.Where(p =>
                {
                    var fn = Path.GetFileName(p);
                    return fn.StartsWith("catalog_", StringComparison.OrdinalIgnoreCase) || fn.EndsWith(".catalog", StringComparison.OrdinalIgnoreCase);
                })
                .OrderBy(p =>
                {
                    // numeric-aware sort: try to get number after "catalog_" prefix, otherwise first number in filename
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
                    var lines = File.ReadAllLines(cf);
                    foreach (var l in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(l)) _catalogLines.Add(l.Trim());
                    }
                }
                catch
                {
                    // ignore single catalog read errors
                }
            }

            // if number of catalog lines doesn't match images, we still keep available lines; mapping is by index
        }

        private (long? timestamp, double? angle, string mode, double? throttle) ParseCatalogLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return (null, null, null, null);
            long? timestamp = null;
            double? angle = null;
            string mode = null;
            double? throttle = null;
            // First try to parse as JSON (many catalog lines are JSON objects)
            try
            {
                var doc = System.Text.Json.JsonDocument.Parse(line);
                var root = doc.RootElement;

                // timestamp candidates
                if (root.TryGetProperty("_timestamp_ms", out var p) || root.TryGetProperty("timestamp_ms", out p) || root.TryGetProperty("timestamp", out p))
                {
                    if (p.ValueKind == System.Text.Json.JsonValueKind.Number && p.TryGetInt64(out var v)) timestamp = v;
                    else if (p.ValueKind == System.Text.Json.JsonValueKind.String && long.TryParse(p.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var vv)) timestamp = vv;
                }

                // angle
                if (root.TryGetProperty("user/angle", out var pa) || root.TryGetProperty("angle", out pa) || root.TryGetProperty("user_angle", out pa))
                {
                    if (pa.ValueKind == System.Text.Json.JsonValueKind.Number && pa.TryGetDouble(out var dv)) angle = dv;
                    else if (pa.ValueKind == System.Text.Json.JsonValueKind.String && double.TryParse(pa.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var dv2)) angle = dv2;
                }

                // throttle
                if (root.TryGetProperty("user/throttle", out var pt) || root.TryGetProperty("throttle", out pt) || root.TryGetProperty("user_throttle", out pt))
                {
                    if (pt.ValueKind == System.Text.Json.JsonValueKind.Number && pt.TryGetDouble(out var dv)) throttle = dv;
                    else if (pt.ValueKind == System.Text.Json.JsonValueKind.String && double.TryParse(pt.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var dv2)) throttle = dv2;
                }

                // mode
                if (root.TryGetProperty("user/mode", out var pm) || root.TryGetProperty("mode", out pm) || root.TryGetProperty("user_mode", out pm))
                {
                    if (pm.ValueKind == System.Text.Json.JsonValueKind.String) mode = pm.GetString();
                    else mode = pm.ToString();
                }

                return (timestamp, angle, mode, throttle);
            }
            catch
            {
                // fallback to regex-based parsing for non-JSON lines
            }

            // helper to match key: value where separator may be ':' or '=' or may appear without word boundaries
            double tmpd;
            long tmpl;

            string TryMatchNumber(string key)
            {
                var pattern = $"{Regex.Escape(key)}\\s*[:=]\\s*([-+]?\\d*\\.?\\d+(?:[eE][-+]?\\d+)?)";
                var m = Regex.Match(line, pattern);
                return m.Success ? m.Groups[1].Value : null;
            }

            string TryMatchToken(string key)
            {
                var pattern = $"{Regex.Escape(key)}\\s*[:=]\\s*([^,;\\s]+)";
                var m = Regex.Match(line, pattern);
                return m.Success ? m.Groups[1].Value : null;
            }

            // timestamp keys
            var tsStr = TryMatchNumber("_timestamp_ms") ?? TryMatchNumber("timestamp_ms") ?? TryMatchNumber("timestamp");
            if (tsStr != null && long.TryParse(tsStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out tmpl)) timestamp = tmpl;

            // angle
            var aStr = TryMatchNumber("user/angle") ?? TryMatchNumber("angle") ?? TryMatchNumber("user_angle");
            if (aStr != null && double.TryParse(aStr, NumberStyles.Float, CultureInfo.InvariantCulture, out tmpd)) angle = tmpd;

            // throttle
            var tStr = TryMatchNumber("user/throttle") ?? TryMatchNumber("throttle") ?? TryMatchNumber("user_throttle");
            if (tStr != null && double.TryParse(tStr, NumberStyles.Float, CultureInfo.InvariantCulture, out tmpd)) throttle = tmpd;

            // mode
            var mStr = TryMatchToken("user/mode") ?? TryMatchToken("mode") ?? TryMatchToken("user_mode");
            if (!string.IsNullOrEmpty(mStr)) mode = mStr.Trim('"');

            return (timestamp, angle, mode, throttle);
        }

        private void TrkRecord_Scroll(object? sender, EventArgs e)
        {
            SetCurrentIndex(trkRecord.Value);
        }

        private void TrkRecord_ValueChanged(object? sender, EventArgs e)
        {
            // also respond to ValueChanged to handle keyboard/ programmatic changes
            SetCurrentIndex(trkRecord.Value);
        }

        private void BtnPrev_Click(object? sender, EventArgs e)
        {
            MovePrev();
        }

        private void BtnNext_Click(object? sender, EventArgs e)
        {
            MoveNext();
        }

        private void MovePrev()
        {
            if (_imageFiles.Count == 0) return;
            var newIndex = _currentIndex <= 0 ? 0 : _currentIndex - 1;
            if (trkRecord.Enabled)
            {
                // ensure value change triggers handlers
                trkRecord.Value = newIndex;
            }
            else
            {
                SetCurrentIndex(newIndex);
            }
        }

        private void MoveNext()
        {
            if (_imageFiles.Count == 0) return;
            var newIndex = _currentIndex < 0 ? 0 : Math.Min(_imageFiles.Count - 1, _currentIndex + 1);
            if (trkRecord.Enabled)
            {
                trkRecord.Value = newIndex;
            }
            else
            {
                SetCurrentIndex(newIndex);
            }
        }

        private void MoveFastPrev()
        {
            if (_imageFiles.Count == 0) return;
            var newIndex = _currentIndex <= 0 ? 0 : Math.Max(0, _currentIndex - 100);
            if (trkRecord.Enabled)
            {
                trkRecord.Value = newIndex;
            }
            else
            {
                SetCurrentIndex(newIndex);
            }
        }

        private void MoveFastNext()
        {
            if (_imageFiles.Count == 0) return;
            var newIndex = _currentIndex < 0 ? 0 : Math.Min(_imageFiles.Count - 1, _currentIndex + 100);
            if (trkRecord.Enabled)
            {
                trkRecord.Value = newIndex;
            }
            else
            {
                SetCurrentIndex(newIndex);
            }
        }

        
    }

}
