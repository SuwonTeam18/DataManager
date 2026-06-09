using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DonkeyUi
{
    public class TubPlotDialog : Form
    {
        // ── 외부에서 읽는 결과값 ─────────────────────────────────
        public string SelectedModelFileName { get; private set; } = "";
        public string SelectedModelType { get; private set; } = "linear";
        public bool ApplyBrightness { get; private set; } = false;
        public bool ApplyBlur { get; private set; } = false;

        // ── 내부 컨트롤 ─────────────────────────────────────────
        private ComboBox _cmbModel;
        private Label _lblModelName;
        private Label _lblModelMeta;
        private Label _lblModelType;
        private CheckBox _chkBrightness;
        private CheckBox _chkBlur;
        private Label _lblTotalRecords;
        private Label _lblCurrentPos;
        private Button _btnCancel;
        private Button _btnCreate;

        // ── 생성자 파라미터 ──────────────────────────────────────
        private readonly List<string> _modelFiles;   // 파일명 목록 (예: "best.h5")
        private readonly string _mycarWinPath; // 모델 폴더 루트
        private readonly int _totalRecords;
        private readonly int _currentIndex;

        private static readonly Color Bg = Color.FromArgb(244, 243, 238);
        private static readonly Color CardBg = Color.White;
        private static readonly Color Border = Color.FromArgb(210, 210, 210);
        private static readonly Color Accent = Color.FromArgb(24, 95, 165);
        private static readonly Color TextMain = Color.FromArgb(30, 30, 30);
        private static readonly Color TextMute = Color.FromArgb(120, 120, 120);

        public TubPlotDialog(
            List<string> modelFiles,
            string mycarWinPath,
            int totalRecords,
            int currentIndex)
        {
            _modelFiles = modelFiles;
            _mycarWinPath = mycarWinPath;
            _totalRecords = totalRecords;
            _currentIndex = currentIndex;

            BuildUI();
        }

        private void BuildUI()
        {
            Text = "그래프 설정";
            Size = new Size(480, 560);
            MinimumSize = new Size(480, 550);
            MaximumSize = new Size(480, 550 );
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Bg;
            Font = new Font("맑은 고딕", 9f);

            int y = 0;

            // ── 헤더 ─────────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 58,
                BackColor = CardBg,
                Padding = new Padding(20, 0, 0, 0)
            };
            var lblTitle = new Label
            {
                Text = "그래프 설정",
                Font = new Font("맑은 고딕", 12f, FontStyle.Bold),
                ForeColor = TextMain,
                AutoSize = true,
                Location = new Point(20, 12)
            };
            var lblSub = new Label
            {
                Text = "모델을 선택하고 그래프를 생성합니다",
                Font = new Font("맑은 고딕", 8.5f),
                ForeColor = TextMute,
                AutoSize = true,
                Location = new Point(20, 35)
            };
            var headerLine = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = Border
            };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(headerLine);
            Controls.Add(pnlHeader);

            // ── 바디 스크롤 패널 ─────────────────────────────────
            var body = new Panel
            {
                Location = new Point(0, 58),
                Size = new Size(460, 376),
                BackColor = Bg,
                Padding = new Padding(20, 16, 20, 0)
            };
            Controls.Add(body);
            y = 16;

            // ── 섹션: 모델 선택 ──────────────────────────────────
            body.Controls.Add(MakeSectionLabel("모델 선택", y));
            y += 22;

            _cmbModel = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(20, y),
                Width = 418,
                Height = 28,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("맑은 고딕", 9f),
                BackColor = CardBg,
                ForeColor = TextMain
            };
            // 변경
            foreach (var f in _modelFiles)
            {
                string type = InferModelType(f);
                _cmbModel.Items.Add($"{f}  —  {type}");
            }
            if (_cmbModel.Items.Count > 0)
            {
                _cmbModel.SelectedIndex = 0;
            }
            else
            {
                _cmbModel.Items.Add("모델 없음 — 모델 학습 탭에서 훈련해주세요");
                _cmbModel.SelectedIndex = 0;
                _cmbModel.Enabled = false;
                _cmbModel.ForeColor = Color.FromArgb(120, 120, 120);
            }
            _cmbModel.SelectedIndexChanged += CmbModel_Changed;
            body.Controls.Add(_cmbModel);
            y += 34;

            // 모델 카드
            var modelCard = new Panel
            {
                Location = new Point(20, y),
                Size = new Size(418, 52),
                BackColor = CardBg,
                BorderStyle = BorderStyle.FixedSingle
            };

            var dot = new Panel
            {
                Location = new Point(12, 22),
                Size = new Size(8, 8),
                BackColor = Accent
            };
            dot.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, 8, 8, 8, 8));

            _lblModelName = new Label
            {
                Location = new Point(28, 8),
                Size = new Size(280, 18),
                Font = new Font("맑은 고딕", 9f, FontStyle.Bold),
                ForeColor = TextMain,
                AutoEllipsis = true
            };
            _lblModelMeta = new Label
            {
                Location = new Point(28, 28),
                Size = new Size(280, 16),
                Font = new Font("맑은 고딕", 8f),
                ForeColor = TextMute
            };
            _lblModelType = new Label
            {
                Location = new Point(318, 15),
                Size = new Size(80, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("맑은 고딕", 8f, FontStyle.Bold),
                ForeColor = Accent,
                BackColor = Color.FromArgb(230, 242, 255),
                BorderStyle = BorderStyle.None
            };
            modelCard.Controls.Add(dot);
            modelCard.Controls.Add(_lblModelName);
            modelCard.Controls.Add(_lblModelMeta);
            modelCard.Controls.Add(_lblModelType);
            body.Controls.Add(modelCard);
            y += 62;

            UpdateModelCard();

            // ── 구분선 ────────────────────────────────────────────
            body.Controls.Add(MakeDivider(y)); y += 18;

            // ── 섹션: 예측 옵션 ──────────────────────────────────
            body.Controls.Add(MakeSectionLabel("예측 옵션", y));
            y += 22;

            _chkBrightness = MakeToggleRow(
                body, y,
                "밝기 필터 반영",
                "현재 밝기 슬라이더 값을 이미지에 적용 후 예측",
                false);
            y += 52;

            _chkBlur = MakeToggleRow(
                body, y,
                "흐림 필터 반영",
                "현재 흐림 슬라이더 값을 이미지에 적용 후 예측",
                false);
            y += 52;

            // ── 구분선 ────────────────────────────────────────────
            body.Controls.Add(MakeDivider(y)); y += 18;

            // ── 섹션: 데이터셋 요약 ──────────────────────────────
            body.Controls.Add(MakeSectionLabel("데이터셋 요약", y));
            y += 22;

            var statPanel = new TableLayoutPanel
            {
                Location = new Point(20, y),
                Size = new Size(418, 80),
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Bg
            };
            statPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            statPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            _lblTotalRecords = MakeStatBox($"{_totalRecords:N0}", "전체 레코드");
            _lblCurrentPos = MakeStatBox($"{_currentIndex:N0}", "현재 위치");

            statPanel.Controls.Add(WrapStatBox(_lblTotalRecords, "전체 레코드"), 0, 0);
            statPanel.Controls.Add(WrapStatBox(_lblCurrentPos, "현재 위치", accent: true), 1, 0);
            body.Controls.Add(statPanel);

            // ── 푸터 ─────────────────────────────────────────────
            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 56,
                BackColor = CardBg
            };
            var footerLine = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Border
            };
            _btnCancel = new Button
            {
                Text = "취소",
                Size = new Size(80, 30),
                Location = new Point(230, 12),
                FlatStyle = FlatStyle.Flat,
                BackColor = CardBg,
                ForeColor = TextMain,
                Font = new Font("맑은 고딕", 9f),
                Cursor = Cursors.Hand
            };
            _btnCancel.FlatAppearance.BorderColor = Border;
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            _btnCreate = new Button
            {
                Text = "그래프 생성",
                Size = new Size(118, 30),
                Location = new Point(316, 12),
                FlatStyle = FlatStyle.Flat,
                BackColor = Accent,
                ForeColor = Color.White,
                Font = new Font("맑은 고딕", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnCreate.FlatAppearance.BorderSize = 0;
            _btnCreate.Click += BtnCreate_Click;

            pnlFooter.Controls.Add(footerLine);
            pnlFooter.Controls.Add(_btnCancel);
            pnlFooter.Controls.Add(_btnCreate);
            Controls.Add(pnlFooter);
        }

        // ════════════════════════════════════════════════════════
        // 이벤트
        // ════════════════════════════════════════════════════════
        private void CmbModel_Changed(object sender, EventArgs e) => UpdateModelCard();

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            // 변경
            if (_modelFiles.Count == 0)
            {
                MessageBox.Show(
                    "모델 파일이 없습니다.\n모델 학습 탭에서 먼저 모델을 훈련해주세요.",
                    "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_cmbModel.SelectedIndex < 0)
            {
                MessageBox.Show("모델을 선택해주세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SelectedModelFileName = _modelFiles[_cmbModel.SelectedIndex];
            SelectedModelType = InferModelType(SelectedModelFileName);
            ApplyBrightness = _chkBrightness.Checked;
            ApplyBlur = _chkBlur.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }

        // ════════════════════════════════════════════════════════
        // 모델 카드 업데이트
        // ════════════════════════════════════════════════════════
        private void UpdateModelCard()
        {
            if (_cmbModel == null || _cmbModel.SelectedIndex < 0
                || _cmbModel.SelectedIndex >= _modelFiles.Count) return;

            string fname = _modelFiles[_cmbModel.SelectedIndex];
            string type = InferModelType(fname);

            _lblModelName.Text = fname;
            _lblModelType.Text = type;

            // 파일 정보 읽기
            try
            {
                string fullPath = Path.Combine(_mycarWinPath, "models", fname);
                if (File.Exists(fullPath))
                {
                    var fi = new FileInfo(fullPath);
                    double mb = fi.Length / 1024.0 / 1024.0;
                    string date = fi.LastWriteTime.ToString("yyyy-MM-dd");
                    _lblModelMeta.Text = $"마지막 수정: {date}  ·  {mb:F1} MB";
                }
                else
                {
                    _lblModelMeta.Text = "파일 정보를 읽을 수 없습니다";
                }
            }
            catch
            {
                _lblModelMeta.Text = "";
            }
        }

        // ════════════════════════════════════════════════════════
        // 헬퍼
        // ════════════════════════════════════════════════════════
        // 변경
        private static string InferModelType(string fileName)
        {
            if (fileName.EndsWith(".tflite", StringComparison.OrdinalIgnoreCase)) return "tflite";
            if (fileName.EndsWith(".keras", StringComparison.OrdinalIgnoreCase)) return "keras";
            if (fileName.EndsWith(".savedmodel", StringComparison.OrdinalIgnoreCase)) return "savedmodel";
            if (fileName.EndsWith(".pkl", StringComparison.OrdinalIgnoreCase)) return "pkl";
            if (fileName.Contains("categorical", StringComparison.OrdinalIgnoreCase)) return "categorical";
            if (fileName.Contains("behavior", StringComparison.OrdinalIgnoreCase)) return "behavior";
            return "linear";
        }

        private Label MakeSectionLabel(string text, int y) => new Label
        {
            Text = text.ToUpper(),
            Font = new Font("맑은 고딕", 8f, FontStyle.Bold),
            ForeColor = TextMute,
            AutoSize = true,
            Location = new Point(20, y)
        };

        private Panel MakeDivider(int y) => new Panel
        {
            Location = new Point(20, y + 4),
            Size = new Size(418, 1),
            BackColor = Border
        };

        // 변경
        private CheckBox MakeToggleRow(Panel parent, int y, string title, string desc, bool defaultOn)
        {
            var row = new Panel
            {
                Location = new Point(20, y),
                Size = new Size(418, 44),
                BackColor = CardBg,
                BorderStyle = BorderStyle.FixedSingle
            };
            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("맑은 고딕", 9f),
                ForeColor = TextMain,
                AutoSize = true,
                Location = new Point(12, 6)
            };
            var lblDesc = new Label
            {
                Text = desc,
                Font = new Font("맑은 고딕", 8f),
                ForeColor = TextMute,
                AutoSize = true,
                Location = new Point(12, 24)
            };

            // 토글 스위치 패널
            bool isOn = defaultOn;
            var toggle = new Panel
            {
                Location = new Point(374, 12),
                Size = new Size(34, 18),
                Cursor = Cursors.Hand,
                BackColor = isOn ? Accent : Color.FromArgb(200, 200, 200)
            };
            toggle.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, 34, 18, 18, 18));

            var thumb = new Panel
            {
                Size = new Size(14, 14),
                Location = new Point(isOn ? 17 : 2, 2),
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            thumb.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, 14, 14, 14, 14));

            // 체크박스는 숨겨서 상태만 보관
            var chk = new CheckBox
            {
                Checked = defaultOn,
                Visible = false
            };

            EventHandler toggleClick = (s, e) =>
            {
                chk.Checked = !chk.Checked;
                toggle.BackColor = chk.Checked ? Accent : Color.FromArgb(200, 200, 200);
                thumb.Location = new Point(chk.Checked ? 17 : 2, 2);
            };

            toggle.Click += toggleClick;
            thumb.Click += toggleClick;

            toggle.Controls.Add(thumb);
            row.Controls.Add(lblTitle);
            row.Controls.Add(lblDesc);
            row.Controls.Add(toggle);
            row.Controls.Add(chk);
            parent.Controls.Add(row);
            return chk;
        }

        private static Label MakeStatBox(string value, string label) => new Label
        {
            Text = value,
            Font = new Font("맑은 고딕", 16f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 30, 30),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };

        // 변경
        private Panel WrapStatBox(Label valLabel, string labelText, bool accent = false)
        {
            var wrap = new Panel
            {
                BackColor = CardBg,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, accent ? 0 : 4, 0),
                Height = 58
            };
            var lbl = new Label
            {
                Text = labelText,
                Font = new Font("맑은 고딕", 8f),
                ForeColor = TextMute,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 6),
                Size = new Size(wrap.Width, 18)
            };
            if (accent) valLabel.ForeColor = Accent;
            valLabel.AutoSize = false;
            valLabel.TextAlign = ContentAlignment.MiddleCenter;
            valLabel.Location = new Point(0, 24);
            valLabel.Size = new Size(200, 28);
            valLabel.Dock = DockStyle.None;
            wrap.Controls.Add(lbl);
            wrap.Controls.Add(valLabel);

            wrap.Resize += (s, e) =>
            {
                lbl.Width = wrap.Width;
                valLabel.Width = wrap.Width;
            };
            return wrap;
        }

        [System.Runtime.InteropServices.DllImport("Gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);
    }
}