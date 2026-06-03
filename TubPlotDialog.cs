using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Globalization;

namespace DonkeyUi
{
    // ════════════════════════════════════════════════════════════
    // TubPlotDialog — Tub Plot 설정 팝업
    // ════════════════════════════════════════════════════════════
    public class TubPlotDialog : Form
    {
        // ── 결과 프로퍼티 (OK 클릭 후 읽음) ────────────────────
        public string SelectedPilot { get; private set; }
        public string SelectedModelType { get; private set; }
        public int RangeStart { get; private set; }
        public int RangeEnd { get; private set; }

        // ── UI 컨트롤 ───────────────────────────────────────────
        private ComboBox _cmbPilot;
        private TrackBar _trkStart;
        private TrackBar _trkEnd;
        private Label _lblRangeDisplay;
        private Label _lblStartVal;
        private Label _lblEndVal;
        private Label _lblCount;
        private Label _lblStatStart;
        private Label _lblStatEnd;
        private Button _btnPlot;
        private Button _btnCancel;

        // ── 색상 ────────────────────────────────────────────────
        private static readonly Color ClrBg = Color.FromArgb(250, 249, 245);
        private static readonly Color ClrCard = Color.White;
        private static readonly Color ClrBorder = Color.FromArgb(215, 215, 215);
        private static readonly Color ClrText = Color.FromArgb(28, 28, 28);
        private static readonly Color ClrMuted = Color.FromArgb(110, 110, 110);
        private static readonly Color ClrAccent = Color.FromArgb(24, 95, 165);
        private static readonly Color ClrAccentBg = Color.FromArgb(232, 244, 255);

        // ── 파일럿 목록 (이름 → 모델타입) ──────────────────────
        private readonly List<(string name, string modelType)> _pilots;
        private readonly int _totalRecords;

        public TubPlotDialog(List<(string name, string modelType)> pilots, int totalRecords)
        {
            _pilots = pilots ?? new List<(string, string)>();
            _totalRecords = Math.Max(1, totalRecords);

            BuildUI();
            PopulatePilots();
            UpdateStats();
        }

        // ════════════════════════════════════════════════════════
        // UI 빌드
        // ════════════════════════════════════════════════════════
        private void BuildUI()
        {
            // ── Form 기본 설정 ──────────────────────────────────
            Text = "Tub Plot 설정";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = ClrBg;
            Width = 500;
            Height = 460;
            Padding = new Padding(0);
            Font = new Font("맑은 고딕", 9.5f);

            // ── 타이틀 바 ───────────────────────────────────────
            var pnlTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = ClrCard,
                Padding = new Padding(16, 0, 12, 0)
            };
            pnlTitle.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(ClrBorder, 1),
                    0, pnlTitle.Height - 1, pnlTitle.Width, pnlTitle.Height - 1);
            };

            var lblTitle = new Label
            {
                Text = "Tub Plot 설정",
                Font = new Font("맑은 고딕", 11f, FontStyle.Bold),
                ForeColor = ClrText,
                AutoSize = true,
                Location = new Point(16, 15)
            };
            pnlTitle.Controls.Add(lblTitle);

            // ── 바디 패널 ───────────────────────────────────────
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ClrBg,
                Padding = new Padding(20, 16, 20, 0)
            };

            // ── Select Pilot ────────────────────────────────────
            var lblPilot = MakeLabel("Select Pilot", ClrMuted, 11f);
            lblPilot.Location = new Point(20, 16);

            _cmbPilot = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("맑은 고딕", 9.5f),
                BackColor = ClrCard,
                Width = 440,
                Location = new Point(20, 34)
            };
            _cmbPilot.SelectedIndexChanged += (s, e) => ReadSelectedPilot();

            // ── 범위 슬라이더 섹션 ──────────────────────────────
            var pnlRange = new Panel
            {
                BackColor = ClrCard,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(20, 76),
                Width = 440,
                Height = 148
            };

            // 헤더 행
            var lblRangeTitle = MakeLabel("레코드 범위", ClrMuted, 11f);
            lblRangeTitle.Location = new Point(12, 10);
            _lblRangeDisplay = MakeLabel("0 — 1000", ClrText, 11f, FontStyle.Bold);
            _lblRangeDisplay.Location = new Point(300, 10);
            _lblRangeDisplay.AutoSize = false;
            _lblRangeDisplay.Width = 128;
            _lblRangeDisplay.TextAlign = ContentAlignment.MiddleRight;

            // 시작 슬라이더
            var lblStartHdr = MakeLabel("시작", ClrMuted, 10f);
            lblStartHdr.Location = new Point(12, 36);
            _lblStartVal = MakeLabel("0", ClrText, 10f, FontStyle.Bold);
            _lblStartVal.Location = new Point(400, 36);
            _lblStartVal.AutoSize = false;
            _lblStartVal.Width = 28;
            _lblStartVal.TextAlign = ContentAlignment.MiddleRight;

            _trkStart = MakeTrackBar(0, _totalRecords, 0);
            _trkStart.Location = new Point(12, 50);
            _trkStart.Width = 416; 

            // 끝 슬라이더
            var lblEndHdr = MakeLabel("끝", ClrMuted, 10f);
            lblEndHdr.Location = new Point(12, 92);
            _lblEndVal = MakeLabel(_totalRecords.ToString(), ClrText, 10f, FontStyle.Bold);
            _lblEndVal.Location = new Point(400, 90);
            _lblEndVal.AutoSize = false;
            _lblEndVal.Width = 28;
            _lblEndVal.TextAlign = ContentAlignment.MiddleRight;

            _trkEnd = MakeTrackBar(0, _totalRecords, _totalRecords);
            _trkEnd.Location = new Point(12, 104);
            _trkEnd.Width = 416;

            _trkStart.Scroll += (s, e) => OnSliderScroll();
            _trkEnd.Scroll += (s, e) => OnSliderScroll();

            pnlRange.Controls.AddRange(new Control[]
            {
                lblRangeTitle, _lblRangeDisplay,
                lblStartHdr, _lblStartVal, _trkStart,
                lblEndHdr,   _lblEndVal,   _trkEnd
            });

            // ── 요약 카드 ───────────────────────────────────────
            var pnlStats = new Panel
            {
                BackColor = Color.FromArgb(240, 245, 252),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(20, 240),
                Width = 440,
                Height = 70
            };

            (_lblCount, var lblCountHdr) = MakeStatCell("선택 레코드 수", "1,000");
            (_lblStatStart, var lblStatStartHdr) = MakeStatCell("시작 레코드", "0");
            (_lblStatEnd, var lblStatEndHdr) = MakeStatCell("끝 레코드", "1,000");

            // 3등분 배치
            LayoutStatCell(pnlStats, lblCountHdr, _lblCount, 0, 440);
            LayoutStatCell(pnlStats, lblStatStartHdr, _lblStatStart, 148, 440);
            LayoutStatCell(pnlStats, lblStatEndHdr, _lblStatEnd, 296, 440);

            // 구분선
            pnlStats.Paint += (s, e) =>
            {
                using var pen = new Pen(ClrBorder, 1);
                e.Graphics.DrawLine(pen, 147, 12, 147, 58);
                e.Graphics.DrawLine(pen, 295, 12, 295, 58);
            };

            // ── 하단 버튼 바 ────────────────────────────────────
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 56,
                BackColor = ClrCard,
                Padding = new Padding(16, 10, 16, 10)
            };
            pnlBottom.Paint += (s, e) =>
            {
                e.Graphics.DrawLine(new Pen(ClrBorder, 1), 0, 0, pnlBottom.Width, 0);
            };

            _btnCancel = new Button
            {
                Text = "취소",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("맑은 고딕", 9.5f),
                ForeColor = ClrMuted,
                Size = new Size(80, 34),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
            };
            _btnCancel.FlatAppearance.BorderColor = ClrBorder;
            _btnCancel.Location = new Point(pnlBottom.Width - 220, 10);
            _btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            _btnPlot = new Button
            {
                Text = "Tub Plot 생성",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("맑은 고딕", 9.5f, FontStyle.Bold),
                ForeColor = ClrAccent,
                BackColor = ClrAccentBg,
                Size = new Size(128, 34),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
            };
            _btnPlot.FlatAppearance.BorderColor = Color.FromArgb(160, 200, 240);
            _btnPlot.Location = new Point(pnlBottom.Width - 132, 10);
            _btnPlot.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            _btnPlot.Click += BtnPlot_Click;

            // 호버 효과
            _btnPlot.MouseEnter += (s, e) => _btnPlot.BackColor = Color.FromArgb(210, 234, 255);
            _btnPlot.MouseLeave += (s, e) => _btnPlot.BackColor = ClrAccentBg;
            _btnCancel.MouseEnter += (s, e) => _btnCancel.BackColor = Color.FromArgb(238, 238, 238);
            _btnCancel.MouseLeave += (s, e) => _btnCancel.BackColor = ClrCard;

            pnlBottom.Controls.AddRange(new Control[] { _btnCancel, _btnPlot });

            // ── 바디에 컨트롤 추가 ──────────────────────────────
            pnlBody.Controls.AddRange(new Control[]
            {
                lblPilot, _cmbPilot, pnlRange, pnlStats
            });

            Controls.AddRange(new Control[] { pnlBottom, pnlBody, pnlTitle });
        }

        // ════════════════════════════════════════════════════════
        // 파일럿 콤보 채우기
        // ════════════════════════════════════════════════════════
        private void PopulatePilots()
        {
            _cmbPilot.Items.Clear();
            foreach (var (name, modelType) in _pilots)
                _cmbPilot.Items.Add($"{name}  [{modelType}]");

            if (_cmbPilot.Items.Count > 0) _cmbPilot.SelectedIndex = 0;
        }

        private void ReadSelectedPilot()
        {
            int idx = _cmbPilot.SelectedIndex;
            if (idx >= 0 && idx < _pilots.Count)
            {
                SelectedPilot = _pilots[idx].name;
                SelectedModelType = _pilots[idx].modelType;
            }
        }

        // ════════════════════════════════════════════════════════
        // 슬라이더 이벤트
        // ════════════════════════════════════════════════════════
        private void OnSliderScroll()
        {
            int s = _trkStart.Value;
            int e = _trkEnd.Value;

            // 시작 > 끝 이면 교환
            if (s > e)
            {
                (_trkStart.Value, _trkEnd.Value) = (e, s);
                s = _trkStart.Value;
                e = _trkEnd.Value;
            }

            UpdateStats();
        }

        private void UpdateStats()
        {
            int s = _trkStart.Value;
            int e = _trkEnd.Value;

            _lblStartVal.Text = s.ToString();
            _lblEndVal.Text = e.ToString();
            _lblRangeDisplay.Text = $"{s} — {e}";

            _lblCount.Text = (e - s).ToString("N0");
            _lblStatStart.Text = s.ToString("N0");
            _lblStatEnd.Text = e.ToString("N0");
        }

        // ════════════════════════════════════════════════════════
        // Tub Plot 생성 클릭
        // ════════════════════════════════════════════════════════
        private void BtnPlot_Click(object sender, EventArgs e)
        {
            ReadSelectedPilot();
            RangeStart = _trkStart.Value;
            RangeEnd = _trkEnd.Value;

            if (string.IsNullOrEmpty(SelectedPilot))
            {
                MessageBox.Show("파일럿을 선택해 주세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (RangeStart >= RangeEnd)
            {
                MessageBox.Show("레코드 범위가 올바르지 않습니다.\n시작 < 끝 이 되도록 설정해 주세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        // ════════════════════════════════════════════════════════
        // 헬퍼 — 컨트롤 팩토리
        // ════════════════════════════════════════════════════════
        private static Label MakeLabel(string text, Color color, float size, FontStyle style = FontStyle.Regular)
        {
            return new Label
            {
                Text = text,
                ForeColor = color,
                Font = new Font("맑은 고딕", size, style),
                AutoSize = true
            };
        }

        private static TrackBar MakeTrackBar(int min, int max, int val)
        {
            return new TrackBar
            {
                Minimum = min,
                Maximum = max,
                Value = val,
                TickStyle = TickStyle.None,
                Height = 32
            };
        }

        private static (Label value, Label header) MakeStatCell(string headerText, string valueText)
        {
            var hdr = new Label
            {
                Text = headerText,
                ForeColor = Color.FromArgb(110, 110, 110),
                Font = new Font("맑은 고딕", 9f),
                AutoSize = true
            };
            var val = new Label
            {
                Text = valueText,
                ForeColor = Color.FromArgb(28, 28, 28),
                Font = new Font("맑은 고딕", 15f, FontStyle.Bold),
                AutoSize = true
            };
            return (val, hdr);
        }

        private static void LayoutStatCell(Panel parent, Label hdr, Label val, int x, int totalW)
        {
            int cellW = totalW / 3;
            hdr.Location = new Point(x + (cellW - hdr.PreferredWidth) / 2, 14);
            val.Location = new Point(x + (cellW - val.PreferredWidth) / 2, 34);
            parent.Controls.Add(hdr);
            parent.Controls.Add(val);
        }
    }
}
