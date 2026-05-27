using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace DonkeyUi
{
    public partial class ucPilotArena : UserControl
    {
        private readonly List<Image> _images = new List<Image>();
        // runtime pictureboxes used to render images in pnlImageArea
        private readonly List<PictureBox> _runtimePics = new List<PictureBox>();
        public ucPilotArena()
        {
            InitializeComponent();
            // wire add/remove buttons for left pictures (guard in case designer didn't create controls)
            if (btnAddLeftPic != null) btnAddLeftPic.Click += BtnAddLeftPic_Click;
            if (btnRemoveLeftPic != null) btnRemoveLeftPic.Click += BtnRemoveLeftPic_Click;
            if (pnlImageArea != null) pnlImageArea.Resize += (s, e) => UpdateDisplay();
            // ensure initial state
            // start with a single placeholder image displayed across the full area
            _images.Clear();
            AddPlaceholderImage();
        }

        private void BtnAddLeftPic_Click(object? sender, EventArgs e)
        {
            if (_images.Count >= 4) return;
            AddPlaceholderImage();
        }

        private void BtnRemoveLeftPic_Click(object? sender, EventArgs e)
        {
            if (_images.Count <= 1) return;
            // remove last
            var last = _images[_images.Count - 1];
            _images.RemoveAt(_images.Count - 1);
            last.Dispose();
            UpdateDisplay();
        }

        private void AddPlaceholderImage()
        {
            // create a simple placeholder bitmap
            var bmp = new Bitmap(800, 480);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.DarkGray);
                using var f = new Font("맑은 고딕", 24);
                using var b = new SolidBrush(Color.White);
                g.DrawString($"이미지 {_images.Count + 1}", f, b, 16, 16);
            }
            _images.Add(bmp);
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (pnlImageArea == null) return;
            pnlImageArea.Controls.Clear();

            int n = _images.Count;
            if (n <= 0) return;

            // helper to create picturebox for given image index
            PictureBox MakePb(Image img)
            {
                var pb = new PictureBox { Image = img, SizeMode = PictureBoxSizeMode.StretchImage, Dock = DockStyle.Fill, BackColor = Color.Black };
                return pb;
            }

            if (n == 1)
            {
                pnlImageArea.Controls.Add(MakePb(_images[0]));
            }
            else if (n == 2)
            {
                var left = new Panel { Dock = DockStyle.Left, Width = pnlImageArea.ClientSize.Width / 2, Padding = new Padding(2) };
                left.Controls.Add(MakePb(_images[0]));
                var right = new Panel { Dock = DockStyle.Fill, Padding = new Padding(2) };
                right.Controls.Add(MakePb(_images[1]));
                pnlImageArea.Controls.Add(right);
                pnlImageArea.Controls.Add(left);
            }
            else if (n == 3)
            {
                // Use TableLayoutPanel to force equal sizes for the top two images
                var outer = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
                outer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                outer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                outer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

                var top = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 1, ColumnCount = 2 };
                top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                top.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                top.Controls.Add(MakePb(_images[0]), 0, 0);
                top.Controls.Add(MakePb(_images[1]), 1, 0);

                var bottom = new Panel { Dock = DockStyle.Fill, Padding = new Padding(2) };
                bottom.Controls.Add(MakePb(_images[2]));

                outer.Controls.Add(top, 0, 0);
                outer.Controls.Add(bottom, 0, 1);

                pnlImageArea.Controls.Add(outer);
            }
            else // 4
            {
                var top = new Panel { Dock = DockStyle.Top, Height = pnlImageArea.ClientSize.Height / 2, Padding = new Padding(0) };
                var leftTop = new Panel { Dock = DockStyle.Left, Width = pnlImageArea.ClientSize.Width / 2, Padding = new Padding(2) };
                leftTop.Controls.Add(MakePb(_images[0]));
                var rightTop = new Panel { Dock = DockStyle.Fill, Padding = new Padding(2) };
                rightTop.Controls.Add(MakePb(_images[1]));
                top.Controls.Add(rightTop);
                top.Controls.Add(leftTop);

                var bottom = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0) };
                var leftBottom = new Panel { Dock = DockStyle.Left, Width = pnlImageArea.ClientSize.Width / 2, Padding = new Padding(2) };
                leftBottom.Controls.Add(MakePb(_images[2]));
                var rightBottom = new Panel { Dock = DockStyle.Fill, Padding = new Padding(2) };
                rightBottom.Controls.Add(MakePb(_images[3]));
                bottom.Controls.Add(rightBottom);
                bottom.Controls.Add(leftBottom);

                pnlImageArea.Controls.Add(bottom);
                pnlImageArea.Controls.Add(top);
            }

            // ensure legacy containers remain hidden
            if (pnlLeftContainer != null) pnlLeftContainer.Visible = false;
            if (picRight != null) picRight.Visible = false;
            // update add/remove button states
            if (btnAddLeftPic != null) btnAddLeftPic.Enabled = _images.Count < 4;
            if (btnRemoveLeftPic != null) btnRemoveLeftPic.Enabled = _images.Count > 1;
            pnlImageArea.Invalidate();
        }
    }
}
