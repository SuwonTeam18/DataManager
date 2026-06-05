

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
        // ── 캐시: 슬롯별 누적 오차 데이터 ──────────────────────
        // key = pilotName, value = 레코드별 (angleErr, throttleErr) 리스트
        private readonly Dictionary<string, List<(double angleErr, double throttleErr)>>
            _tubPlotData = new();

        // ════════════════════════════════════════════════════════
        // btnTubPlot Click 핸들러 — Designer에서 연결하거나
        // 생성자에서 btnTubPlot.Click += BtnTubPlot_Click; 추가
        // ════════════════════════════════════════════════════════
        private void BtnTubPlot_Click(object sender, EventArgs e)
        {
            // 현재 파일럿 슬롯 목록 구성
            var pilots = _pilotSlots
                .Select(s => (name: s.PilotName, modelType: s.ModelType))
                .ToList();

            if (pilots.Count == 0)
            {
                MessageBox.Show("추가된 파일럿이 없습니다.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int totalRecords = _imageFiles.Count > 0 ? _imageFiles.Count : 1000;

            using var dlg = new TubPlotDialog(pilots, totalRecords);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            // 다이얼로그 결과 수집
            string pilotName = dlg.SelectedPilot;
            string modelType = dlg.SelectedModelType;
            int rangeStart = dlg.RangeStart;
            int rangeEnd = dlg.RangeEnd;

            // 선택 범위의 이미지 파일 슬라이스
            var targetFiles = _imageFiles
                .Skip(rangeStart)
                .Take(rangeEnd - rangeStart)
                .ToList();

            if (targetFiles.Count == 0)
            {
                MessageBox.Show("선택된 범위에 이미지 파일이 없습니다.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 그래프 창 열기 (비동기 수집 + 즉시 창 표시)
            OpenTubPlotWindow(pilotName, modelType, targetFiles);
        }

        // ════════════════════════════════════════════════════════
        // TubPlot 창 오픈 및 데이터 수집 + 렌더링
        // ════════════════════════════════════════════════════════
        private async void OpenTubPlotWindow(
            string pilotName, string modelType, List<string> imageFiles)
        {
            // ── 그래프 전용 Form 생성 ───────────────────────────
            var plotForm = new Form
            {
                Text = $"Tub Plot — {pilotName}",
                Size = new Size(860, 520),
                MinimumSize = new Size(640, 400),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(28, 28, 30),
                FormBorderStyle = FormBorderStyle.Sizable
            };

            // 상태 라벨 (로딩 중 표시)
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

            // 진행률 바
            var progress = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 4,
                Minimum = 0,
                Maximum = imageFiles.Count,
                Value = 0,
                Style = ProgressBarStyle.Continuous
            };

            // PictureBox — 그래프 렌더 대상
            var pic = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(28, 28, 30),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            plotForm.Controls.Add(pic);
            plotForm.Controls.Add(progress);
            plotForm.Controls.Add(lblStatus);
            plotForm.Show(this);

            // ── 각 이미지에 대해 예측 수집 ──────────────────────
            var angleErrors = new List<double>();
            var throttleErrors = new List<double>();
            var humanAngles = new List<double>();
            var aiAngles = new List<double>();
            var humanThrottles = new List<double>();
            var aiThrottles = new List<double>();

            for (int i = 0; i < imageFiles.Count; i++)
            {
                if (plotForm.IsDisposed) return;

                string imgPath = imageFiles[i];
                string fname = Path.GetFileName(imgPath);

                // 사람 데이터 — catalog json에서 읽기 (없으면 0)
                var (hAngle, hThrottle) = ReadCatalogData(imgPath);

                // AI 예측 — 캐시 우선. RequestPrediction(PilotSlot, wslPath) 사용
                double aiAngle = 0, aiThrottle = 0;
                if (_predictionCache.TryGetValue(fname, out var cached))
                {
                    aiAngle = cached.angle;
                    aiThrottle = cached.throttle;
                }
                else
                {
                    string wslPath = ConvertToWslPath(imgPath);
                    // Find the slot with matching pilotName
                    var slot = _pilotSlots.FirstOrDefault(s => s.PilotName == pilotName);
                    (double angle, double throttle)? result = null;
                    if (slot != null)
                    {
                        result = await RequestPrediction(slot, wslPath);
                    }
                    if (result.HasValue)
                    {
                        aiAngle = result.Value.angle;
                        aiThrottle = result.Value.throttle;
                        _predictionCache[fname] = result.Value;
                    }
                }

                humanAngles.Add(hAngle);
                aiAngles.Add(aiAngle);
                humanThrottles.Add(hThrottle);
                aiThrottles.Add(aiThrottle);
                angleErrors.Add(aiAngle - hAngle);
                throttleErrors.Add(aiThrottle - hThrottle);

                // UI 업데이트 (10프레임마다)
                if (i % 10 == 0 || i == imageFiles.Count - 1)
                {
                    int captured = i;
                    if (!plotForm.IsDisposed)
                    {
                        plotForm.Invoke(() =>
                        {
                            progress.Value = Math.Min(captured + 1, imageFiles.Count);
                            lblStatus.Text = $"데이터 수집 중...  {captured + 1} / {imageFiles.Count}";
                            RenderTubPlot(pic, humanAngles, aiAngles,
                                          humanThrottles, aiThrottles,
                                          angleErrors, throttleErrors, pilotName);
                        });
                    }
                }
            }

            // ── 수집 완료 ────────────────────────────────────────
            if (!plotForm.IsDisposed)
            {
                plotForm.Invoke(() =>
                {
                    lblStatus.Text = $"완료  |  {imageFiles.Count}개 레코드  " +
                        $"|  각도 평균오차 {angleErrors.Average(Math.Abs):F4}  " +
                        $"|  속도 평균오차 {throttleErrors.Average(Math.Abs):F4}";
                    progress.Value = imageFiles.Count;
                    RenderTubPlot(pic, humanAngles, aiAngles,
                                  humanThrottles, aiThrottles,
                                  angleErrors, throttleErrors, pilotName);
                });
            }

            // 슬롯별 데이터 캐시 저장
            _tubPlotData[pilotName] = angleErrors
                .Zip(throttleErrors, (a, t) => (a, t))
                .ToList();
        }

        // ════════════════════════════════════════════════════════
        // Catalog JSON 파싱 — 같은 폴더의 record_N.json 읽기
        // ════════════════════════════════════════════════════════
        private static (double angle, double throttle) ReadCatalogData(string imagePath)
        {
            try
            {
                // DonkeyCar tub 구조: 이미지명 cam-image_array_N.jpg → record_N.json
                string dir = Path.GetDirectoryName(imagePath) ?? "";
                string name = Path.GetFileNameWithoutExtension(imagePath); // e.g. "cam-image_array_42"

                // 숫자 추출
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

        // 간단한 JSON 값 추출 (외부 라이브러리 없이)
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
            return double.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out double d) ? d : 0;
        }

        // ════════════════════════════════════════════════════════
        // 그래프 렌더링
        // ════════════════════════════════════════════════════════
        private static void RenderTubPlot(
            PictureBox pic,
            List<double> humanAngles, List<double> aiAngles,
            List<double> humanThrottles, List<double> aiThrottles,
            List<double> angleErrors, List<double> throttleErrors,
            string pilotName)
        {
            int W = Math.Max(pic.Width, 800);
            int H = Math.Max(pic.Height, 420);

            var bmp = new Bitmap(W, H);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(28, 28, 30));

            int n = humanAngles.Count;
            if (n == 0) { pic.Image?.Dispose(); pic.Image = bmp; return; }

            // ── 레이아웃 분할: 상단 각도, 하단 속도 ─────────────
            int pad = 52;
            int gapY = 24;
            int titleH = 22;
            int subH = (H - pad - titleH - gapY) / 2;

            var rectAngle = new Rectangle(pad, titleH, W - pad - 12, subH);
            var rectThrottle = new Rectangle(pad, titleH + subH + gapY, W - pad - 12, subH);

            // 제목
            using var titleFont = new Font("맑은 고딕", 10f, FontStyle.Bold);
            using var axisFont = new Font("맑은 고딕", 8f);
            using var lblFont = new Font("맑은 고딕", 9f);

            g.DrawString($"Tub Plot — {pilotName}", titleFont,
                Brushes.White, new PointF(pad, 4));

            // ── 서브그래프 공통 렌더 ─────────────────────────────
            DrawSubGraph(g, rectAngle, n, humanAngles, aiAngles,
                angleErrors, "각도 (Angle)",
                Color.FromArgb(255, 87, 34),   // human
                Color.FromArgb(0, 176, 255),   // ai
                Color.FromArgb(180, 220, 80),  // error
                axisFont, lblFont);

            DrawSubGraph(g, rectThrottle, n, humanThrottles, aiThrottles,
                throttleErrors, "속도 (Throttle)",
                Color.FromArgb(255, 87, 34),
                Color.FromArgb(0, 176, 255),
                Color.FromArgb(220, 100, 200),
                axisFont, lblFont);

            // 범례
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
            // 배경
            using var bgBrush = new SolidBrush(Color.FromArgb(36, 36, 40));
            g.FillRectangle(bgBrush, r);
            using var borderPen = new Pen(Color.FromArgb(60, 60, 65), 1);
            g.DrawRectangle(borderPen, r);

            // 서브타이틀
            g.DrawString(title, lblFont, new SolidBrush(Color.FromArgb(200, 200, 200)),
                new PointF(r.Left + 6, r.Top + 4));

            // 데이터 범위 계산
            double yMin = Math.Min(humanVals.Min(), aiVals.Min());
            double yMax = Math.Max(humanVals.Max(), aiVals.Max());
            double errMax = errorVals.Select(Math.Abs).DefaultIfEmpty(0.1).Max();
            yMin = Math.Min(yMin, -0.05);
            yMax = Math.Max(yMax, 0.05);
            double yRange = yMax - yMin;

            // 내부 그리기 영역 (패딩 적용)
            int innerX = r.Left + 8;
            int innerY = r.Top + 20;
            int innerW = r.Width - 16;
            int innerH = r.Height - 28;

            // Y=0 기준선
            float y0 = innerY + innerH - (float)((0 - yMin) / yRange * innerH);
            using var zeroPen = new Pen(Color.FromArgb(70, 70, 75), 1) { DashStyle = DashStyle.Dash };
            g.DrawLine(zeroPen, innerX, y0, innerX + innerW, y0);
            g.DrawString("0", axisFont, new SolidBrush(Color.FromArgb(90, 90, 95)),
                new PointF(r.Left, y0 - 7));

            // Y 눈금선 (±0.5, ±1.0)
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

            // 오차 영역 (filled area)
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

            // Human 라인
            DrawLine(g, humanVals, humanColor, innerX, innerY, innerW, innerH, yMin, yRange, n, xStep, 1.8f);
            // AI 라인
            DrawLine(g, aiVals, aiColor, innerX, innerY, innerW, innerH, yMin, yRange, n, xStep, 1.8f);
            // Error 라인
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