using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DonkeyUi
{
    public partial class ucPilotArena
    {
        private readonly Dictionary<string, List<(double angleErr, double throttleErr)>>
            _tubPlotData = new();

        // ════════════════════════════════════════════════════════
        // Tub Plot 버튼 클릭
        // ════════════════════════════════════════════════════════
        private void BtnTubPlot_Click(object sender, EventArgs e)
        {
            // models 폴더의 모델 파일 목록
            var modelFiles = GetModelFiles();
            

            // 버튼을 누른 시점의 타임라인 인덱스 캡처
            int currentIdx = _currentIndex;
            int totalCount = _imageFiles.Count > 0 ? _imageFiles.Count : 0;

            using var dlg = new TubPlotDialog(
                modelFiles,
                _mycarWinPath,
                totalCount,
                currentIdx);

            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            // 다이얼로그 결과 수집
            string modelFileName = dlg.SelectedModelFileName;
            string modelType = dlg.SelectedModelType;
            bool applyBrightness = dlg.ApplyBrightness;
            bool applyBlur = dlg.ApplyBlur;

            if (_imageFiles.Count == 0)
            {
                MessageBox.Show("이미지 파일이 없습니다.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 밝기/흐림 값 결정
            int brightness = applyBrightness ? trkBrightness.Value : 0;
            int blur = applyBlur ? trkBlur.Value : 0;

            // 선택한 모델을 슬롯 0에 적용
            if (_pilotSlots.Count > 0)
            {
                _pilotSlots[0].ModelFileName = modelFileName;
                _pilotSlots[0].ModelType = modelType;
                ResetSlotServer(_pilotSlots[0]);
            }

            // ★ 이전 모델의 AI 예측 배열 초기화
            Array.Clear(_graphAiAngles, 0, _graphAiAngles.Length);
            Array.Clear(_graphAiThrottles, 0, _graphAiThrottles.Length);
            _graphDrawPanel?.Invalidate();

            // ★ 모델명 라벨 갱신
            if (_lblGraphModelName != null)
                _lblGraphModelName.Text = $"모델: {modelFileName}";

            // 그래프용 필터값 저장
            _graphBrightness = brightness;
            _graphBlur = blur;
            if (_lblGraphFilterStatus != null)
                _lblGraphFilterStatus.Text = $"필터값 - 밝기: {_graphBrightness}, 흐림: {_graphBlur}";
            if (_lblGraphModelName != null)
                _lblGraphModelName.Text = $"모델: {modelFileName}";

            // 기존 AI 예측 데이터 초기화
            Array.Clear(_graphAiAngles, 0, _graphAiAngles.Length);
            Array.Clear(_graphAiThrottles, 0, _graphAiThrottles.Length);
            _graphDrawPanel?.Invalidate();

            // 백그라운드로 전체 그래프 채우기 시작
            _graphTaskCts?.Cancel();
            _graphTaskCts = new CancellationTokenSource();
            _ = FillGraphFromSlotCacheAsync(_graphImagePaths, _graphTaskCts.Token, _graphBrightness, _graphBlur);
        }

        // ════════════════════════════════════════════════════════
        // TubPlot 창 오픈 및 비동기 수집
        // ════════════════════════════════════════════════════════
        private async void OpenTubPlotWindow(
            string modelFileName,
            string modelType,
            List<string> imageFiles,
            int brightness,
            int blur)
        {
            // 전용 PilotSlot 생성 (TubPlot 전용, 기존 슬롯에 영향 없음)
            var slot = new PilotSlot
            {
                ModelFileName = modelFileName,
                ModelType = modelType
            };

            var plotForm = new Form
            {
                Text = $"Tub Plot — {modelFileName}",
                Size = new Size(860, 520),
                MinimumSize = new Size(640, 400),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(28, 28, 30),
                FormBorderStyle = FormBorderStyle.Sizable
            };

            var lblStatus = new Label
            {
                Text = "데이터 수집 중...",
                ForeColor = Color.FromArgb(160, 160, 160),
                Font = new Font("맑은 고딕", 10f),
                Dock = DockStyle.Top,
                Height = 28,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            };

            var progress = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 4,
                Minimum = 0,
                Maximum = imageFiles.Count,
                Value = 0,
                Style = ProgressBarStyle.Continuous
            };

            var pic = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(28, 28, 30),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            plotForm.Controls.Add(pic);
            plotForm.Controls.Add(progress);
            plotForm.Controls.Add(lblStatus);
            plotForm.FormClosed += (s, e) => ResetSlotServer(slot);
            plotForm.Show(this);

            // ── 데이터 수집 ──────────────────────────────────────
            var humanAngles = new List<double>();
            var aiAngles = new List<double>();
            var humanThrottles = new List<double>();
            var aiThrottles = new List<double>();
            var angleErrors = new List<double>();
            var throttleErrors = new List<double>();

            for (int i = 0; i < imageFiles.Count; i++)
            {
                if (plotForm.IsDisposed) { ResetSlotServer(slot); return; }

                string imgPath = imageFiles[i];
                string fname = Path.GetFileName(imgPath);

                // 사람 데이터 (catalog)
                double hAngle = 0, hThrottle = 0;
                if (_graphHumanAngles.Count > i)
                {
                    hAngle = _graphHumanAngles[i];
                    hThrottle = _graphHumanThrottles[i];
                }
                else
                {
                    (hAngle, hThrottle) = ReadCatalogData(imgPath);
                }

                // AI 예측
                double aiAngle = 0, aiThrottle = 0;
                string cacheKey = $"{fname}_{brightness}_{blur}";

                if (slot.Cache.TryGetValue(cacheKey, out var cached))
                {
                    aiAngle = cached.angle;
                    aiThrottle = cached.throttle;
                }
                else
                {
                    (double angle, double throttle)? result = null;
                    if (brightness == 0 && blur == 0)
                    {
                        result = await RequestPrediction(slot, ConvertToWslPath(imgPath));
                    }
                    else
                    {
                        using var bmp = GetFilteredBitmap(imgPath, brightness, blur);
                        if (bmp != null) result = await RequestPrediction(slot, bmp);
                    }

                    if (result.HasValue)
                    {
                        slot.Cache[cacheKey] = result.Value;
                        aiAngle = result.Value.angle;
                        aiThrottle = result.Value.throttle;
                    }
                }

                humanAngles.Add(hAngle);
                aiAngles.Add(aiAngle);
                humanThrottles.Add(hThrottle);
                aiThrottles.Add(aiThrottle);
                angleErrors.Add(aiAngle - hAngle);
                throttleErrors.Add(aiThrottle - hThrottle);

                if ((i % 10 == 0 || i == imageFiles.Count - 1) && !plotForm.IsDisposed)
                {
                    int captured = i;
                    plotForm.Invoke(() =>
                    {
                        progress.Value = Math.Min(captured + 1, imageFiles.Count);
                        lblStatus.Text = $"데이터 수집 중...  {captured + 1} / {imageFiles.Count}";
                        RenderTubPlot(pic, humanAngles, aiAngles,
                                      humanThrottles, aiThrottles,
                                      angleErrors, throttleErrors, modelFileName);
                    });
                }
            }

            // ── 완료 ────────────────────────────────────────────
            if (!plotForm.IsDisposed)
            {
                plotForm.Invoke(() =>
                {
                    double avgAngleErr = angleErrors.Count > 0 ? angleErrors.Average(Math.Abs) : 0;
                    double avgThrottleErr = throttleErrors.Count > 0 ? throttleErrors.Average(Math.Abs) : 0;
                    lblStatus.Text =
                        $"완료  |  {imageFiles.Count}개 레코드  " +
                        $"|  각도 평균오차 {avgAngleErr:F4}  " +
                        $"|  속도 평균오차 {avgThrottleErr:F4}";
                    progress.Value = imageFiles.Count;
                    RenderTubPlot(pic, humanAngles, aiAngles,
                                  humanThrottles, aiThrottles,
                                  angleErrors, throttleErrors, modelFileName);
                });
            }

            _tubPlotData[modelFileName] = angleErrors
                .Zip(throttleErrors, (a, t) => (a, t))
                .ToList();
        }

        // ════════════════════════════════════════════════════════
        // Catalog 데이터 읽기 (graphHumanAngles 없을 때 폴백)
        // ════════════════════════════════════════════════════════
        private static (double angle, double throttle) ReadCatalogData(string imagePath)
        {
            try
            {
                string dir = Path.GetDirectoryName(imagePath) ?? "";
                string name = Path.GetFileNameWithoutExtension(imagePath);
                string numStr = new string(name.Where(char.IsDigit).ToArray());
                if (!int.TryParse(numStr, out int num)) return (0, 0);

                string jsonPath = Path.Combine(dir, $"record_{num}.json");
                if (!File.Exists(jsonPath)) return (0, 0);

                string json = File.ReadAllText(jsonPath);
                double angle = ExtractJsonDouble(json, "user/angle");
                double throttle = ExtractJsonDouble(json, "user/throttle");
                return (angle, throttle);
            }
            catch { return (0, 0); }
        }

        private static double ExtractJsonDouble(string json, string key)
        {
            string search = $"\"{key}\"";
            int idx = json.IndexOf(search, StringComparison.Ordinal);
            if (idx < 0) return 0;
            int colon = json.IndexOf(':', idx + search.Length);
            if (colon < 0) return 0;
            int start = colon + 1;
            while (start < json.Length && (json[start] == ' ' || json[start] == '\t')) start++;
            int end = start;
            while (end < json.Length && json[end] != ',' && json[end] != '}') end++;
            string val = json.Substring(start, end - start).Trim();
            return double.TryParse(val, NumberStyles.Float,
                CultureInfo.InvariantCulture, out double d) ? d : 0;
        }

        // ════════════════════════════════════════════════════════
        // 그래프 렌더링 (기존 코드 유지)
        // ════════════════════════════════════════════════════════
        private static void RenderTubPlot(
            PictureBox pic,
            List<double> humanAngles, List<double> aiAngles,
            List<double> humanThrottles, List<double> aiThrottles,
            List<double> angleErrors, List<double> throttleErrors,
            string modelFileName)
        {
            int W = Math.Max(pic.Width, 800);
            int H = Math.Max(pic.Height, 420);

            var bmp = new Bitmap(W, H);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(28, 28, 30));

            int n = humanAngles.Count;
            if (n == 0) { pic.Image?.Dispose(); pic.Image = bmp; return; }

            int pad = 52;
            int gapY = 24;
            int titleH = 22;
            int subH = (H - pad - titleH - gapY) / 2;

            var rectAngle = new Rectangle(pad, titleH, W - pad - 12, subH);
            var rectThrottle = new Rectangle(pad, titleH + subH + gapY, W - pad - 12, subH);

            using var titleFont = new Font("맑은 고딕", 10f, FontStyle.Bold);
            using var axisFont = new Font("맑은 고딕", 8f);
            using var lblFont = new Font("맑은 고딕", 9f);

            g.DrawString($"Tub Plot — {modelFileName}", titleFont, Brushes.White, new PointF(pad, 4));

            DrawSubGraph(g, rectAngle, n, humanAngles, aiAngles,
                angleErrors, "각도 (Angle)",
                Color.FromArgb(255, 87, 34),
                Color.FromArgb(0, 176, 255),
                Color.FromArgb(180, 220, 80),
                axisFont, lblFont);

            DrawSubGraph(g, rectThrottle, n, humanThrottles, aiThrottles,
                throttleErrors, "속도 (Throttle)",
                Color.FromArgb(255, 87, 34),
                Color.FromArgb(0, 176, 255),
                Color.FromArgb(220, 100, 200),
                axisFont, lblFont);

            DrawLegend(g, W - 200, titleH + 4, lblFont);

            pic.Image?.Dispose();
            pic.Image = bmp;
        }

        private static void DrawSubGraph(
            Graphics g, Rectangle r, int n,
            List<double> humanVals, List<double> aiVals, List<double> errorVals,
            string title,
            Color humanColor, Color aiColor, Color errorColor,
            Font axisFont, Font lblFont)
        {
            using var bgBrush = new SolidBrush(Color.FromArgb(36, 36, 40));
            using var borderPen = new Pen(Color.FromArgb(60, 60, 65), 1);
            g.FillRectangle(bgBrush, r);
            g.DrawRectangle(borderPen, r);

            g.DrawString(title, lblFont, new SolidBrush(Color.FromArgb(200, 200, 200)),
                new PointF(r.Left + 6, r.Top + 4));

            double yMin = Math.Min(humanVals.Min(), aiVals.Min());
            double yMax = Math.Max(humanVals.Max(), aiVals.Max());
            yMin = Math.Min(yMin, -0.05);
            yMax = Math.Max(yMax, 0.05);
            double yRange = yMax - yMin;

            int innerX = r.Left + 8;
            int innerY = r.Top + 20;
            int innerW = r.Width - 16;
            int innerH = r.Height - 28;

            float y0 = innerY + innerH - (float)((0 - yMin) / yRange * innerH);
            using var zeroPen = new Pen(Color.FromArgb(70, 70, 75), 1) { DashStyle = DashStyle.Dash };
            g.DrawLine(zeroPen, innerX, y0, innerX + innerW, y0);
            g.DrawString("0", axisFont, new SolidBrush(Color.FromArgb(90, 90, 95)),
                new PointF(r.Left, y0 - 7));

            foreach (double tick in new[] { -1.0, -0.5, 0.5, 1.0 })
            {
                float yt = innerY + innerH - (float)((tick - yMin) / yRange * innerH);
                if (yt < innerY || yt > innerY + innerH) continue;
                using var tickPen = new Pen(Color.FromArgb(50, 50, 55), 1) { DashStyle = DashStyle.Dot };
                g.DrawLine(tickPen, innerX, yt, innerX + innerW, yt);
                g.DrawString(tick.ToString("+0.0;-0.0;0.0"), axisFont,
                    new SolidBrush(Color.FromArgb(70, 70, 75)), new PointF(r.Left - 2, yt - 6));
            }

            if (n < 2) return;

            float xStep = (float)innerW / (n - 1);

            var errPoints = new PointF[n * 2];
            for (int i = 0; i < n; i++)
            {
                float fx = innerX + i * xStep;
                float fy = y0 - (float)(errorVals[i] / yRange * innerH);
                fy = Math.Clamp(fy, innerY, innerY + innerH);
                errPoints[i] = new PointF(fx, fy);
                errPoints[2 * n - 1 - i] = new PointF(fx, y0);
            }
            using var errBrush = new SolidBrush(Color.FromArgb(40, errorColor));
            g.FillPolygon(errBrush, errPoints);

            DrawLine(g, humanVals, humanColor, innerX, innerY, innerW, innerH, yMin, yRange, n, xStep, 1.8f);
            DrawLine(g, aiVals, aiColor, innerX, innerY, innerW, innerH, yMin, yRange, n, xStep, 1.8f);
            DrawLine(g, errorVals, errorColor, innerX, innerY, innerW, innerH, yMin, yRange, n, xStep, 1.2f,
                DashStyle.Dash);
        }

        private static void DrawLine(
            Graphics g, List<double> vals, Color color,
            int innerX, int innerY, int innerW, int innerH,
            double yMin, double yRange, int n, float xStep,
            float penWidth, DashStyle dash = DashStyle.Solid)
        {
            var pts = new List<PointF>(n);
            for (int i = 0; i < n; i++)
            {
                float fx = innerX + i * xStep;
                float fy = innerY + innerH - (float)((vals[i] - yMin) / yRange * innerH);
                fy = Math.Clamp(fy, innerY, innerY + innerH);
                pts.Add(new PointF(fx, fy));
            }
            using var pen = new Pen(color, penWidth) { DashStyle = dash };
            g.DrawLines(pen, pts.ToArray());
        }

        private static void DrawLegend(Graphics g, int x, int y, Font font)
        {
            var items = new[]
            {
                (Color.FromArgb(255, 87,  34),  "Human"),
                (Color.FromArgb(0,  176, 255),  "AI"),
                (Color.FromArgb(180, 220,  80), "Angle error"),
                (Color.FromArgb(220, 100, 200), "Throttle error"),
            };
            int cx = x, cy = y;
            foreach (var (color, label) in items)
            {
                using var pen = new Pen(color, 2);
                using var brush = new SolidBrush(Color.FromArgb(200, 200, 200));
                g.DrawLine(pen, cx, cy + 7, cx + 18, cy + 7);
                g.DrawString(label, font, brush, cx + 22, cy);
                cy += 18;
            }
        }
    }
}