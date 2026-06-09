using System;
using System.Drawing;
using System.Windows.Forms;

namespace DonkeyUi
{
    public partial class ucPilotArena
    {
        // ── 패널 루트 (Designer 선언 교체 대상) ─────────────────
        // Designer.cs 에서 아래 한 줄을 추가하세요.
        //   private System.Windows.Forms.Panel pnlUserValue;
        private Panel pnlUserValue;

        // ── 내부 컨트롤 (코드로만 관리) ─────────────────────────
        private Label _lblUserAngleVal;
        private Label _lblUserThrottleVal;
        private Panel _barAngleTrack;
        private Panel _barAngleFill;
        private Panel _barThrottleTrack;
        private Panel _barThrottleFill;

        // ── 색상 상수 ────────────────────────────────────────────
        private static readonly Color BarOrange = Color.FromArgb(255, 87, 34);
        private static readonly Color BarTrackColor = Color.FromArgb(230, 225, 215);
        private static readonly Color PanelBg = Color.FromArgb(248, 247, 242);
        private static readonly Color HeaderColor = Color.FromArgb(70, 70, 70);

        // ════════════════════════════════════════════════════════
        // 초기화 — 생성자에서 호출
        // ════════════════════════════════════════════════════════
        private void InitUserValuePanel()
        {
            // ── pnlUserValue 생성 (tableLayoutPanel1 교체) ──────
            pnlUserValue = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = PanelBg,
                Padding = new Padding(0)
            };

            // Designer.cs 의 pnlTimeline.Controls 에서
            //   tableLayoutPanel1  →  pnlUserValue 로 교체합니다.
            // 아래 코드는 런타임 교체 방식입니다.
            // (Designer 파일을 수정하면 이 블록은 생략 가능)
            if (pnlTimeline != null)
            {
                pnlTimeline.Controls.Clear();
                pnlTimeline.Controls.Add(pnlUserValue);
            }

            // ── 헤더 행 ─────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = Color.FromArgb(240, 238, 232),
                Padding = new Padding(12, 0, 0, 0)
            };
            pnlHeader.Paint += (s, e) =>
            {
                // 하단 구분선
                e.Graphics.DrawLine(
                    new Pen(Color.FromArgb(215, 210, 200), 1),
                    0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };

            // 아이콘(≡) + 헤더 라벨
            var lblHeader = new Label
            {
                Text = "≡  사용자 조종값",
                Font = new Font("맑은 고딕", 9f, FontStyle.Bold),
                ForeColor = HeaderColor,
                AutoSize = true,
                Location = new Point(12, 8)
            };
            pnlHeader.Controls.Add(lblHeader);

            // ── 콘텐츠 영역 (2열 그리드) ────────────────────────
            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = PanelBg,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // 왼쪽 셀: user/angle
            var cellAngle = BuildValueCell(
                "사용자/각도",
                out _lblUserAngleVal,
                out _barAngleTrack,
                out _barAngleFill,
                rightBorder: true);

            // 오른쪽 셀: user/throttle
            var cellThrottle = BuildValueCell(
                "사용자/속도",
                out _lblUserThrottleVal,
                out _barThrottleTrack,
                out _barThrottleFill,
                rightBorder: false);

            tlp.Controls.Add(cellAngle, 0, 0);
            tlp.Controls.Add(cellThrottle, 1, 0);

            pnlUserValue.Controls.Add(tlp);
            pnlUserValue.Controls.Add(pnlHeader);   // Top → 마지막에 추가

            // 초기값 렌더링
            UpdateUserValuePanel();
        }

        // ════════════════════════════════════════════════════════
        // 셀 빌더 — 라벨 + 숫자 + 주황 바
        // ════════════════════════════════════════════════════════
        private static Panel BuildValueCell(
            string caption,
            out Label valueLabel,
            out Panel track,
            out Panel fill,
            bool rightBorder)
        {
            var cell = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(16, 10, 16, 10)
            };

            // 오른쪽 구분선
            if (rightBorder)
            {
                cell.Paint += (s, e) =>
                {
                    var p = (Panel)s;
                    e.Graphics.DrawLine(
                        new Pen(Color.FromArgb(215, 210, 200), 1),
                        p.Width - 1, 8, p.Width - 1, p.Height - 8);
                };
            }

            // 캡션 라벨 (user/angle)
            var lblCaption = new Label
            {
                Text = caption,
                Font = new Font("맑은 고딕", 8.5f),
                ForeColor = Color.FromArgb(130, 130, 130),
                AutoSize = true,
                Location = new Point(16, 10)
            };

            // 값 라벨 (+0.033)
            valueLabel = new Label
            {
                Text = "+0.000",
                Font = new Font("맑은 고딕", 17f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                AutoSize = true,
                Location = new Point(14, 28)
            };

            // 바 트랙
            track = new Panel
            {
                BackColor = BarTrackColor,
                Height = 5,
                BorderStyle = BorderStyle.None
            };
            // Resize 핸들러로 트랙 너비를 셀에 맞춤
            Panel barTrack = new Panel
            {
                BackColor = BarTrackColor,
                Height = 5,
                BorderStyle = BorderStyle.None
            };

            track = barTrack;

            cell.Resize += (s, e) =>
            {
                var c = (Panel)s;
                barTrack.Width = c.Width - 32;
                barTrack.Location = new Point(16, c.Height - 20);
            };

            // 바 채움 (주황)
            fill = new Panel
            {
                BackColor = BarOrange,
                Height = 5,
                Location = new Point(0, 0),
                Width = 0
            };

            track.Controls.Add(fill);

            cell.Controls.Add(lblCaption);
            cell.Controls.Add(valueLabel);
            cell.Controls.Add(track);

            return cell;
        }

        // ════════════════════════════════════════════════════════
        // 실시간 업데이트 — OnTubDataChanged 에서 호출
        // ════════════════════════════════════════════════════════
        public void UpdateUserValuePanel()
        {
            if (_lblUserAngleVal == null) return;

            // ── catalog 데이터 우선 사용 (재생 중 튀는 현상 방지) ──
            double angle, throttle;

            if (_graphHumanAngles != null &&
                _graphHumanThrottles != null &&
                _currentIndex >= 0 &&
                _currentIndex < _graphHumanAngles.Count)
            {
                angle = _graphHumanAngles[_currentIndex];
                throttle = _graphHumanThrottles[_currentIndex];
            }
            else
            {
                angle = _humanAngle ?? 0.0;
                throttle = _humanThrottle ?? 0.0;
            }

            // ── 각도 ────────────────────────────────────────────
            _lblUserAngleVal.Text = angle.ToString("+0.000;-0.000;0.000");
            SetBar(_barAngleFill, _barAngleTrack, angle);

            // ── 속도 ────────────────────────────────────────────
            _lblUserThrottleVal.Text = throttle.ToString("+0.000;-0.000;0.000");
            SetBar(_barThrottleFill, _barThrottleTrack, throttle);
        }

        // ── 바 채움 계산 ─────────────────────────────────────────
        //  값 범위 -1 ~ +1
        //  양수: 왼쪽 끝에서 오른쪽으로 (값 * 100%)
        //  음수: 중앙에서 왼쪽 방향으로
        //  → 단순하게 |값| * trackWidth 로 채움 (DonkeyCar 관례)
        private static void SetBar(Panel fill, Panel track, double value)
        {
            if (track.Width <= 0) return;

            double ratio = Math.Abs(Math.Clamp(value, -1.0, 1.0));
            int fillW = (int)(ratio * track.Width);
            int fillX = value >= 0
                ? 0                           // 양수: 왼쪽 기준
                : track.Width - fillW;        // 음수: 오른쪽 기준

            fill.Width = fillW;
            fill.Location = new Point(fillX, 0);
            fill.Height = track.Height;
        }
    }
}