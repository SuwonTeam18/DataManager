using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DonkeyUi
{
    public partial class ucPilotArena : UserControl
    {
        private readonly List<Image> _images = new List<Image>();

        // 콤보박스 세트 관리
        private readonly List<ComboBox> _pilotCombos = new();
        private readonly List<ComboBox> _modelCombos = new();

        public ucPilotArena()
        {
            InitializeComponent();

            if (tlpMain != null && tlpMain.RowStyles.Count > 1)
            {
                tlpMain.RowStyles[1].SizeType = SizeType.Percent;
                tlpMain.RowStyles[1].Height = 60F;
            }

            if (btnAddLeftPic != null)
                btnAddLeftPic.Click += BtnAddLeftPic_Click;

            if (btnRemoveLeftPic != null)
                btnRemoveLeftPic.Click += BtnRemoveLeftPic_Click;

            if (pnlImageArea != null)
                pnlImageArea.Resize += (s, e) => UpdateDisplay();

            // 초기 1세트
            _images.Clear();
            _pilotCombos.Clear();
            _modelCombos.Clear();

            AddPilotSet();
        }

        private void BtnAddLeftPic_Click(object? sender, EventArgs e)
        {
            AddPilotSet();
        }

        private void BtnRemoveLeftPic_Click(object? sender, EventArgs e)
        {
            if (_images.Count <= 1)
                return;

            // 이미지 제거
            var lastImage = _images[^1];
            _images.RemoveAt(_images.Count - 1);
            lastImage.Dispose();

            // 콤보 제거
            var lastPilot = _pilotCombos[^1];
            var lastModel = _modelCombos[^1];

            _pilotCombos.RemoveAt(_pilotCombos.Count - 1);
            _modelCombos.RemoveAt(_modelCombos.Count - 1);

            lastPilot.Dispose();
            lastModel.Dispose();

            UpdateDisplay();
        }

        private void AddPilotSet()
        {
            if (_images.Count >= 4)
                return;

            // 이미지 추가
            AddPlaceholderImage();

            // 파일럿 콤보
            ComboBox cmbPilot = new ComboBox();
            cmbPilot.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPilot.Items.AddRange(new string[]
            {
                "Pilot1",
                "Pilot2",
                "Pilot3"
            });
            cmbPilot.SelectedIndex = 0;

            // 모델 콤보
            ComboBox cmbModel = new ComboBox();
            cmbModel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModel.Items.AddRange(new string[]
            {
                "Linear",
                "Categorical",
                "Behavior"
            });
            cmbModel.SelectedIndex = 0;

            _pilotCombos.Add(cmbPilot);
            _modelCombos.Add(cmbModel);

            UpdateDisplay();
        }

        private void AddPlaceholderImage()
        {
            var bmp = new Bitmap(800, 480);

            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.DarkGray);

                using var f = new Font("맑은 고딕", 24);
                using var b = new SolidBrush(Color.White);

                g.DrawString(
                    $"이미지 {_images.Count + 1}",
                    f,
                    b,
                    20,
                    20
                );
            }

            _images.Add(bmp);
        }

        private void UpdateDisplay()
        {
            if (pnlImageArea == null)
                return;

            pnlImageArea.Controls.Clear();

            int count = _images.Count;

            if (count == 0)
                return;

            // 세트 표시용 테이블
            TableLayoutPanel table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;
            table.ColumnCount = count;
            table.RowCount = 2;

            table.RowStyles.Add(
                new RowStyle(SizeType.Percent, 80f));

            table.RowStyles.Add(
                new RowStyle(SizeType.Percent, 20f));

            for (int i = 0; i < count; i++)
            {
                table.ColumnStyles.Add(
                    new ColumnStyle(
                        SizeType.Percent,
                        100f / count));

                // 이미지
                PictureBox pb = new PictureBox();
                pb.Dock = DockStyle.Fill;
                pb.Image = _images[i];
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.BackColor = Color.Black;

                // 콤보 두 개 담을 패널
                FlowLayoutPanel comboPanel =
                    new FlowLayoutPanel();

                comboPanel.Dock = DockStyle.Fill;
                comboPanel.FlowDirection =
                    FlowDirection.LeftToRight;

                comboPanel.Controls.Add(_pilotCombos[i]);
                comboPanel.Controls.Add(_modelCombos[i]);

                table.Controls.Add(pb, i, 0);
                table.Controls.Add(comboPanel, i, 1);
            }

            pnlImageArea.Controls.Add(table);

            // 버튼 상태
            if (btnAddLeftPic != null)
                btnAddLeftPic.Enabled =
                    _images.Count < 4;

            if (btnRemoveLeftPic != null)
                btnRemoveLeftPic.Enabled =
                    _images.Count > 1;
        }
    }
}