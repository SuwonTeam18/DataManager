namespace DonkeyUi
{
    partial class ucTubManager
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pnlWorkspace = new Panel();
            tlpWorkspace = new TableLayoutPanel();
            panel2 = new Panel();
            lblThrottle = new Label();
            picThrottle = new PictureBox();
            lblAngle = new Label();
            txtRecordNumber = new TextBox();
            picAngle = new PictureBox();
            lblRecordInfo = new Label();
            picTubImage = new PictureBox();
            panel3 = new Panel();
            cmbSpeed = new ComboBox();
            btnFastNext = new Button();
            btnNext = new Button();
            btnStartStop = new Button();
            btnFastPrev = new Button();
            btnPrev = new Button();
            panel1 = new Panel();
            btnLoadCarDirectory = new Button();
            btnLoadTub = new Button();
            txtCarDirectory = new TextBox();
            txtTub = new TextBox();
            trkRecord = new TrackBar();
            pnlTools = new Panel();
            pnlFilter = new Panel();
            tplFilter = new TableLayoutPanel();
            label1 = new Label();
            nudSpeedMin = new NumericUpDown();
            pnlSpeedRange = new Panel();
            panel4 = new Panel();
            nudSpeedMax = new NumericUpDown();
            pnlAngleRange = new Panel();
            panel6 = new Panel();
            nudAngleMin = new NumericUpDown();
            nudAngleMax = new NumericUpDown();
            pnlThrottleText = new Panel();
            FilThrottle = new Label();
            pnlAngleText = new Panel();
            FilAngle = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnApplyFilter = new Button();
            btnClearFilter = new Button();
            pnlTimeline = new Panel();
            lblRange = new Label();
            btnSetLeft = new Button();
            btnSetRight = new Button();
            btnDelete = new Button();
            btnRestore = new Button();
            btnReroadTub = new Button();
            btnSave = new Button();
            IblRange = new Label();
            BtnLeftSet = new Button();
            BtnRightSet = new Button();
            BtnRangeDelete = new Button();
            cmbSpeedFilters = new ComboBox();
            cmbAngleFilters = new ComboBox();
            cmbRanges = new ComboBox();
            btnDeleteAllRanges = new Button();
            btnRangeCancel = new Button();
            lblDeleteStatus = new Label();
            pnlGraph = new Panel();
            contextFilter = new ContextMenuStrip(components);
            menuThrottle = new ToolStripMenuItem();
            menuAngle = new ToolStripMenuItem();
            tlpWorkspace.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picThrottle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAngle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picTubImage).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkRecord).BeginInit();
            pnlTools.SuspendLayout();
            pnlFilter.SuspendLayout();
            tplFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpeedMin).BeginInit();
            pnlSpeedRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpeedMax).BeginInit();
            pnlAngleRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudAngleMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAngleMax).BeginInit();
            pnlThrottleText.SuspendLayout();
            pnlAngleText.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            contextFilter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlWorkspace
            // 
            pnlWorkspace.Location = new Point(0, 0);
            pnlWorkspace.Name = "pnlWorkspace";
            pnlWorkspace.Size = new Size(200, 100);
            pnlWorkspace.TabIndex = 0;
            // 
            // tlpWorkspace
            // 
            tlpWorkspace.BackColor = Color.FromArgb(244, 243, 238);
            tlpWorkspace.ColumnCount = 2;
            tlpWorkspace.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tlpWorkspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpWorkspace.Controls.Add(panel2, 0, 0);
            tlpWorkspace.Controls.Add(picTubImage, 1, 0);
            tlpWorkspace.Dock = DockStyle.Top;
            tlpWorkspace.Location = new Point(0, 44);
            tlpWorkspace.Name = "tlpWorkspace";
            tlpWorkspace.RowCount = 1;
            tlpWorkspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 420F));
            tlpWorkspace.Size = new Size(1200, 420);
            tlpWorkspace.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(244, 243, 238);
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(lblThrottle);
            panel2.Controls.Add(picThrottle);
            panel2.Controls.Add(lblAngle);
            panel2.Controls.Add(txtRecordNumber);
            panel2.Controls.Add(picAngle);
            panel2.Controls.Add(lblRecordInfo);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(214, 414);
            panel2.TabIndex = 2;
            // 
            // lblThrottle
            // 
            lblThrottle.BackColor = Color.FromArgb(244, 243, 238);
            lblThrottle.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblThrottle.ForeColor = Color.FromArgb(80, 80, 80);
            lblThrottle.Location = new Point(4, 4);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(200, 18);
            lblThrottle.TabIndex = 15;
            lblThrottle.Text = "속도 THROTTLE";
            // 
            // picThrottle
            // 
            picThrottle.BackColor = Color.FromArgb(40, 40, 40);
            picThrottle.Location = new Point(4, 22);
            picThrottle.Name = "picThrottle";
            picThrottle.Size = new Size(200, 140);
            picThrottle.SizeMode = PictureBoxSizeMode.StretchImage;
            picThrottle.TabIndex = 16;
            picThrottle.TabStop = false;
            // 
            // lblAngle
            // 
            lblAngle.BackColor = Color.FromArgb(244, 243, 238);
            lblAngle.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblAngle.ForeColor = Color.FromArgb(80, 80, 80);
            lblAngle.Location = new Point(4, 168);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(200, 18);
            lblAngle.TabIndex = 14;
            lblAngle.Text = "각도 ANGLE";
            // 
            // txtRecordNumber
            // 
            txtRecordNumber.BackColor = Color.White;
            txtRecordNumber.BorderStyle = BorderStyle.FixedSingle;
            txtRecordNumber.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
            txtRecordNumber.ForeColor = Color.FromArgb(30, 30, 30);
            txtRecordNumber.Location = new Point(4, 353);
            txtRecordNumber.Name = "txtRecordNumber";
            txtRecordNumber.Size = new Size(195, 27);
            txtRecordNumber.TabIndex = 0;
            txtRecordNumber.TextAlign = HorizontalAlignment.Center;
            txtRecordNumber.KeyDown += TxtRecordNumber_KeyDown;
            txtRecordNumber.Leave += TxtRecordNumber_Leave;
            // 
            // picAngle
            // 
            picAngle.BackColor = Color.FromArgb(40, 40, 40);
            picAngle.Location = new Point(4, 186);
            picAngle.Name = "picAngle";
            picAngle.Size = new Size(200, 140);
            picAngle.SizeMode = PictureBoxSizeMode.StretchImage;
            picAngle.TabIndex = 17;
            picAngle.TabStop = false;
            // 
            // lblRecordInfo
            // 
            lblRecordInfo.BackColor = Color.FromArgb(244, 243, 238);
            lblRecordInfo.Font = new Font("맑은 고딕", 9F);
            lblRecordInfo.ForeColor = Color.FromArgb(120, 120, 120);
            lblRecordInfo.Location = new Point(4, 332);
            lblRecordInfo.Name = "lblRecordInfo";
            lblRecordInfo.Size = new Size(200, 18);
            lblRecordInfo.TabIndex = 13;
            lblRecordInfo.Text = "실제 프레임 번호";
            // 
            // picTubImage
            // 
            picTubImage.BackColor = Color.Black;
            picTubImage.Dock = DockStyle.Fill;
            picTubImage.Location = new Point(223, 3);
            picTubImage.Name = "picTubImage";
            picTubImage.Size = new Size(974, 414);
            picTubImage.SizeMode = PictureBoxSizeMode.Zoom;
            picTubImage.TabIndex = 3;
            picTubImage.TabStop = false;
            // 
            // panel3
            // 
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(200, 100);
            panel3.TabIndex = 0;
            // 
            // cmbSpeed
            // 
            cmbSpeed.BackColor = Color.White;
            cmbSpeed.FlatStyle = FlatStyle.Flat;
            cmbSpeed.Font = new Font("맑은 고딕", 9F);
            cmbSpeed.ForeColor = Color.FromArgb(50, 50, 50);
            cmbSpeed.Items.AddRange(new object[] { "0.25x", "0.50x", "0.75x", "1.00x", "1.25x", "1.50x", "1.75x", "2.00x" });
            cmbSpeed.Location = new Point(8, 60);
            cmbSpeed.Name = "cmbSpeed";
            cmbSpeed.Size = new Size(140, 23);
            cmbSpeed.TabIndex = 7;
            cmbSpeed.Text = "1.0x";
            // 
            // btnFastNext
            // 
            btnFastNext.BackColor = Color.FromArgb(224, 224, 224);
            btnFastNext.Cursor = Cursors.Hand;
            btnFastNext.FlatAppearance.BorderColor = Color.FromArgb(204, 204, 204);
            btnFastNext.FlatStyle = FlatStyle.Flat;
            btnFastNext.Font = new Font("맑은 고딕", 9F);
            btnFastNext.ForeColor = Color.FromArgb(68, 68, 68);
            btnFastNext.Location = new Point(956, 90);
            btnFastNext.Name = "btnFastNext";
            btnFastNext.Size = new Size(236, 32);
            btnFastNext.TabIndex = 6;
            btnFastNext.Text = ">>";
            btnFastNext.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.FromArgb(236, 236, 236);
            btnNext.Cursor = Cursors.Hand;
            btnNext.FlatAppearance.BorderColor = Color.FromArgb(221, 221, 221);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("맑은 고딕", 9F);
            btnNext.ForeColor = Color.FromArgb(85, 85, 85);
            btnNext.Location = new Point(730, 90);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(220, 32);
            btnNext.TabIndex = 5;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // btnStartStop
            // 
            btnStartStop.BackColor = Color.FromArgb(230, 242, 255);
            btnStartStop.Cursor = Cursors.Hand;
            btnStartStop.FlatAppearance.BorderSize = 0;
            btnStartStop.FlatStyle = FlatStyle.Flat;
            btnStartStop.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnStartStop.ForeColor = Color.FromArgb(24, 95, 165);
            btnStartStop.Location = new Point(460, 90);
            btnStartStop.Name = "btnStartStop";
            btnStartStop.Size = new Size(264, 32);
            btnStartStop.TabIndex = 4;
            btnStartStop.Text = "▶ 재생";
            btnStartStop.UseVisualStyleBackColor = false;
            // 
            // btnFastPrev
            // 
            btnFastPrev.BackColor = Color.FromArgb(224, 224, 224);
            btnFastPrev.Cursor = Cursors.Hand;
            btnFastPrev.FlatAppearance.BorderColor = Color.FromArgb(204, 204, 204);
            btnFastPrev.FlatStyle = FlatStyle.Flat;
            btnFastPrev.Font = new Font("맑은 고딕", 9F);
            btnFastPrev.ForeColor = Color.FromArgb(68, 68, 68);
            btnFastPrev.Location = new Point(8, 90);
            btnFastPrev.Name = "btnFastPrev";
            btnFastPrev.Size = new Size(220, 32);
            btnFastPrev.TabIndex = 3;
            btnFastPrev.Text = "<<";
            btnFastPrev.UseVisualStyleBackColor = false;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.FromArgb(236, 236, 236);
            btnPrev.Cursor = Cursors.Hand;
            btnPrev.FlatAppearance.BorderColor = Color.FromArgb(221, 221, 221);
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.Font = new Font("맑은 고딕", 9F);
            btnPrev.ForeColor = Color.FromArgb(85, 85, 85);
            btnPrev.Location = new Point(234, 90);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(220, 32);
            btnPrev.TabIndex = 2;
            btnPrev.Text = "<";
            btnPrev.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(244, 243, 238);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(btnLoadCarDirectory);
            panel1.Controls.Add(btnLoadTub);
            panel1.Controls.Add(txtCarDirectory);
            panel1.Controls.Add(txtTub);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1200, 44);
            panel1.TabIndex = 1;
            // 
            // btnLoadCarDirectory
            // 
            btnLoadCarDirectory.BackColor = Color.FromArgb(24, 95, 165);
            btnLoadCarDirectory.Cursor = Cursors.Hand;
            btnLoadCarDirectory.FlatAppearance.BorderSize = 0;
            btnLoadCarDirectory.FlatStyle = FlatStyle.Flat;
            btnLoadCarDirectory.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            btnLoadCarDirectory.ForeColor = Color.White;
            btnLoadCarDirectory.Location = new Point(8, 8);
            btnLoadCarDirectory.Name = "btnLoadCarDirectory";
            btnLoadCarDirectory.Size = new Size(110, 28);
            btnLoadCarDirectory.TabIndex = 4;
            btnLoadCarDirectory.Text = "📁 차량 폴더";
            btnLoadCarDirectory.UseVisualStyleBackColor = false;
            // 
            // btnLoadTub
            // 
            btnLoadTub.BackColor = Color.FromArgb(24, 95, 165);
            btnLoadTub.Cursor = Cursors.Hand;
            btnLoadTub.FlatAppearance.BorderSize = 0;
            btnLoadTub.FlatStyle = FlatStyle.Flat;
            btnLoadTub.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            btnLoadTub.ForeColor = Color.White;
            btnLoadTub.Location = new Point(572, 8);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Size = new Size(130, 28);
            btnLoadTub.TabIndex = 5;
            btnLoadTub.Text = "📂 데이터 불러오기";
            btnLoadTub.UseVisualStyleBackColor = false;
            // 
            // txtCarDirectory
            // 
            txtCarDirectory.BackColor = Color.White;
            txtCarDirectory.BorderStyle = BorderStyle.FixedSingle;
            txtCarDirectory.Font = new Font("맑은 고딕", 9F);
            txtCarDirectory.ForeColor = Color.FromArgb(50, 50, 50);
            txtCarDirectory.Location = new Point(124, 10);
            txtCarDirectory.Name = "txtCarDirectory";
            txtCarDirectory.Size = new Size(440, 23);
            txtCarDirectory.TabIndex = 1;
            // 
            // txtTub
            // 
            txtTub.BackColor = Color.White;
            txtTub.BorderStyle = BorderStyle.FixedSingle;
            txtTub.Font = new Font("맑은 고딕", 9F);
            txtTub.ForeColor = Color.FromArgb(50, 50, 50);
            txtTub.Location = new Point(708, 10);
            txtTub.Name = "txtTub";
            txtTub.Size = new Size(480, 23);
            txtTub.TabIndex = 3;
            // 
            // trkRecord
            // 
            trkRecord.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            trkRecord.BackColor = Color.FromArgb(244, 243, 238);
            trkRecord.Location = new Point(8, 28);
            trkRecord.Name = "trkRecord";
            trkRecord.Size = new Size(1184, 45);
            trkRecord.TabIndex = 2;
            trkRecord.TickStyle = TickStyle.None;
            // 
            // pnlTools
            // 
            pnlTools.BackColor = Color.FromArgb(244, 243, 238);
            pnlTools.BorderStyle = BorderStyle.FixedSingle;
            pnlTools.Controls.Add(cmbSpeed);
            pnlTools.Controls.Add(pnlFilter);
            pnlTools.Controls.Add(pnlTimeline);
            pnlTools.Controls.Add(trkRecord);
            pnlTools.Controls.Add(btnFastPrev);
            pnlTools.Controls.Add(btnPrev);
            pnlTools.Controls.Add(btnNext);
            pnlTools.Controls.Add(btnFastNext);
            pnlTools.Controls.Add(btnStartStop);
            pnlTools.Controls.Add(lblRange);
            pnlTools.Controls.Add(btnSetLeft);
            pnlTools.Controls.Add(btnSetRight);
            pnlTools.Controls.Add(btnDelete);
            pnlTools.Controls.Add(btnRestore);
            pnlTools.Controls.Add(btnReroadTub);
            pnlTools.Controls.Add(btnSave);
            pnlTools.Controls.Add(IblRange);
            pnlTools.Controls.Add(BtnLeftSet);
            pnlTools.Controls.Add(BtnRightSet);
            pnlTools.Controls.Add(BtnRangeDelete);
            pnlTools.Controls.Add(cmbSpeedFilters);
            pnlTools.Controls.Add(cmbAngleFilters);
            pnlTools.Controls.Add(cmbRanges);
            pnlTools.Controls.Add(btnDeleteAllRanges);
            pnlTools.Controls.Add(btnRangeCancel);
            pnlTools.Dock = DockStyle.Top;
            pnlTools.Location = new Point(0, 464);
            pnlTools.Name = "pnlTools";
            pnlTools.Size = new Size(1200, 410);
            pnlTools.TabIndex = 3;
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = Color.FromArgb(244, 243, 238);
            pnlFilter.BorderStyle = BorderStyle.FixedSingle;
            pnlFilter.Controls.Add(tplFilter);
            pnlFilter.Dock = DockStyle.Bottom;
            pnlFilter.Location = new Point(0, 248);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Size = new Size(1198, 160);
            pnlFilter.TabIndex = 21;
            // 
            // tplFilter
            // 
            tplFilter.ColumnCount = 3;
            tplFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tplFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tplFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tplFilter.Controls.Add(label1, 0, 0);
            tplFilter.Controls.Add(nudSpeedMin, 0, 2);
            tplFilter.Controls.Add(pnlSpeedRange, 1, 2);
            tplFilter.Controls.Add(nudSpeedMax, 2, 2);
            tplFilter.Controls.Add(pnlAngleRange, 1, 4);
            tplFilter.Controls.Add(nudAngleMin, 0, 4);
            tplFilter.Controls.Add(nudAngleMax, 2, 4);
            tplFilter.Controls.Add(pnlThrottleText, 0, 1);
            tplFilter.Controls.Add(pnlAngleText, 0, 3);
            tplFilter.Controls.Add(flowLayoutPanel1, 1, 0);
            tplFilter.Dock = DockStyle.Fill;
            tplFilter.Location = new Point(0, 0);
            tplFilter.Name = "tplFilter";
            tplFilter.RowCount = 5;
            tplFilter.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tplFilter.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tplFilter.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tplFilter.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tplFilter.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tplFilter.Size = new Size(1196, 158);
            tplFilter.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("맑은 고딕", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label1.ForeColor = Color.FromArgb(24, 95, 165);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(113, 39);
            label1.TabIndex = 15;
            label1.Text = "필터 기능";
            // 
            // nudSpeedMin
            // 
            nudSpeedMin.BackColor = Color.White;
            nudSpeedMin.BorderStyle = BorderStyle.FixedSingle;
            nudSpeedMin.DecimalPlaces = 3;
            nudSpeedMin.Dock = DockStyle.Fill;
            nudSpeedMin.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            nudSpeedMin.ForeColor = Color.FromArgb(30, 30, 30);
            nudSpeedMin.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            nudSpeedMin.Location = new Point(3, 61);
            nudSpeedMin.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSpeedMin.Name = "nudSpeedMin";
            nudSpeedMin.Size = new Size(113, 29);
            nudSpeedMin.TabIndex = 11;
            // 
            // pnlSpeedRange
            // 
            pnlSpeedRange.BackColor = Color.FromArgb(244, 243, 238);
            pnlSpeedRange.Controls.Add(panel4);
            pnlSpeedRange.Dock = DockStyle.Fill;
            pnlSpeedRange.Location = new Point(122, 61);
            pnlSpeedRange.Name = "pnlSpeedRange";
            pnlSpeedRange.Size = new Size(950, 33);
            pnlSpeedRange.TabIndex = 10;
            pnlSpeedRange.Paint += pnlSpeedRange_Paint;
            // 
            // panel4
            // 
            panel4.Location = new Point(3, 33);
            panel4.Name = "panel4";
            panel4.Size = new Size(1020, 22);
            panel4.TabIndex = 11;
            // 
            // nudSpeedMax
            // 
            nudSpeedMax.BackColor = Color.White;
            nudSpeedMax.BorderStyle = BorderStyle.FixedSingle;
            nudSpeedMax.DecimalPlaces = 3;
            nudSpeedMax.Dock = DockStyle.Fill;
            nudSpeedMax.Font = new Font("맑은 고딕", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            nudSpeedMax.ForeColor = Color.FromArgb(30, 30, 30);
            nudSpeedMax.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            nudSpeedMax.Location = new Point(1078, 61);
            nudSpeedMax.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSpeedMax.Name = "nudSpeedMax";
            nudSpeedMax.Size = new Size(115, 27);
            nudSpeedMax.TabIndex = 12;
            nudSpeedMax.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // pnlAngleRange
            // 
            pnlAngleRange.BackColor = Color.FromArgb(244, 243, 238);
            pnlAngleRange.Controls.Add(panel6);
            pnlAngleRange.Dock = DockStyle.Fill;
            pnlAngleRange.Location = new Point(122, 119);
            pnlAngleRange.Name = "pnlAngleRange";
            pnlAngleRange.Size = new Size(950, 36);
            pnlAngleRange.TabIndex = 12;
            pnlAngleRange.Paint += pnlAngleRange_Paint;
            // 
            // panel6
            // 
            panel6.Location = new Point(0, 28);
            panel6.Name = "panel6";
            panel6.Size = new Size(947, 13);
            panel6.TabIndex = 11;
            // 
            // nudAngleMin
            // 
            nudAngleMin.BackColor = Color.White;
            nudAngleMin.BorderStyle = BorderStyle.FixedSingle;
            nudAngleMin.Dock = DockStyle.Fill;
            nudAngleMin.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            nudAngleMin.ForeColor = Color.FromArgb(30, 30, 30);
            nudAngleMin.Location = new Point(3, 119);
            nudAngleMin.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudAngleMin.Minimum = new decimal(new int[] { 10, 0, 0, int.MinValue });
            nudAngleMin.Name = "nudAngleMin";
            nudAngleMin.Size = new Size(113, 29);
            nudAngleMin.TabIndex = 13;
            nudAngleMin.Value = new decimal(new int[] { 10, 0, 0, int.MinValue });
            // 
            // nudAngleMax
            // 
            nudAngleMax.BackColor = Color.White;
            nudAngleMax.BorderStyle = BorderStyle.FixedSingle;
            nudAngleMax.Dock = DockStyle.Fill;
            nudAngleMax.Font = new Font("맑은 고딕", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            nudAngleMax.ForeColor = Color.FromArgb(30, 30, 30);
            nudAngleMax.Location = new Point(1078, 119);
            nudAngleMax.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudAngleMax.Minimum = new decimal(new int[] { 10, 0, 0, int.MinValue });
            nudAngleMax.Name = "nudAngleMax";
            nudAngleMax.Size = new Size(115, 27);
            nudAngleMax.TabIndex = 14;
            nudAngleMax.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // pnlThrottleText
            // 
            pnlThrottleText.BorderStyle = BorderStyle.FixedSingle;
            pnlThrottleText.Controls.Add(FilThrottle);
            pnlThrottleText.Dock = DockStyle.Fill;
            pnlThrottleText.Location = new Point(3, 42);
            pnlThrottleText.Name = "pnlThrottleText";
            pnlThrottleText.Size = new Size(113, 13);
            pnlThrottleText.TabIndex = 16;
            // 
            // FilThrottle
            // 
            FilThrottle.AutoSize = true;
            FilThrottle.Dock = DockStyle.Fill;
            FilThrottle.Font = new Font("맑은 고딕", 8.5F);
            FilThrottle.ForeColor = Color.FromArgb(80, 80, 80);
            FilThrottle.Location = new Point(0, 0);
            FilThrottle.Name = "FilThrottle";
            FilThrottle.Size = new Size(76, 15);
            FilThrottle.TabIndex = 17;
            FilThrottle.Text = "속도 Throttle";
            // 
            // pnlAngleText
            // 
            pnlAngleText.BorderStyle = BorderStyle.FixedSingle;
            pnlAngleText.Controls.Add(FilAngle);
            pnlAngleText.Dock = DockStyle.Fill;
            pnlAngleText.Location = new Point(3, 100);
            pnlAngleText.Name = "pnlAngleText";
            pnlAngleText.Size = new Size(113, 13);
            pnlAngleText.TabIndex = 17;
            // 
            // FilAngle
            // 
            FilAngle.AutoSize = true;
            FilAngle.Dock = DockStyle.Fill;
            FilAngle.Font = new Font("맑은 고딕", 8.5F);
            FilAngle.ForeColor = Color.FromArgb(80, 80, 80);
            FilAngle.Location = new Point(0, 0);
            FilAngle.Name = "FilAngle";
            FilAngle.Size = new Size(66, 15);
            FilAngle.TabIndex = 16;
            FilAngle.Text = "각도 Angle";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(btnApplyFilter);
            flowLayoutPanel1.Controls.Add(btnClearFilter);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(122, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(950, 33);
            flowLayoutPanel1.TabIndex = 18;
            // 
            // btnApplyFilter
            // 
            btnApplyFilter.BackColor = Color.FromArgb(24, 95, 165);
            btnApplyFilter.Cursor = Cursors.Hand;
            btnApplyFilter.FlatAppearance.BorderSize = 0;
            btnApplyFilter.FlatStyle = FlatStyle.Flat;
            btnApplyFilter.Font = new Font("맑은 고딕", 9F);
            btnApplyFilter.ForeColor = Color.White;
            btnApplyFilter.Location = new Point(3, 3);
            btnApplyFilter.Name = "btnApplyFilter";
            btnApplyFilter.Size = new Size(184, 30);
            btnApplyFilter.TabIndex = 19;
            btnApplyFilter.Text = "✔ 필터 적용";
            btnApplyFilter.UseVisualStyleBackColor = false;
            btnApplyFilter.Click += btnApplyFilter_Click;
            // 
            // btnClearFilter
            // 
            btnClearFilter.BackColor = Color.FromArgb(210, 210, 210);
            btnClearFilter.Cursor = Cursors.Hand;
            btnClearFilter.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            btnClearFilter.FlatAppearance.BorderSize = 0;
            btnClearFilter.FlatStyle = FlatStyle.Flat;
            btnClearFilter.Font = new Font("맑은 고딕", 9F);
            btnClearFilter.ForeColor = Color.FromArgb(50, 50, 50);
            btnClearFilter.Location = new Point(193, 3);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(184, 30);
            btnClearFilter.TabIndex = 18;
            btnClearFilter.Text = "✕ 해제";
            btnClearFilter.UseVisualStyleBackColor = false;
            btnClearFilter.Click += btnClearFilter_Click;
            // 
            // pnlTimeline
            // 
            pnlTimeline.BackColor = Color.FromArgb(24, 95, 165);
            pnlTimeline.Location = new Point(8, 8);
            pnlTimeline.Name = "pnlTimeline";
            pnlTimeline.Size = new Size(1184, 18);
            pnlTimeline.TabIndex = 20;
            // 
            // lblRange
            // 
            lblRange.AutoSize = true;
            lblRange.Font = new Font("맑은 고딕", 9F);
            lblRange.ForeColor = Color.FromArgb(80, 80, 80);
            lblRange.Location = new Point(8, 130);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(55, 15);
            lblRange.TabIndex = 22;
            lblRange.Text = "범위 : [-]";
            // 
            // btnSetLeft
            // 
            btnSetLeft.BackColor = Color.FromArgb(210, 210, 210);
            btnSetLeft.Cursor = Cursors.Hand;
            btnSetLeft.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            btnSetLeft.FlatAppearance.BorderSize = 0;
            btnSetLeft.FlatStyle = FlatStyle.Flat;
            btnSetLeft.Font = new Font("맑은 고딕", 9F);
            btnSetLeft.ForeColor = Color.FromArgb(50, 50, 50);
            btnSetLeft.Location = new Point(8, 148);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(100, 26);
            btnSetLeft.TabIndex = 0;
            btnSetLeft.Text = "좌측 설정 ⚙";
            btnSetLeft.UseVisualStyleBackColor = false;
            btnSetLeft.Click += btnSetLeft_Click;
            // 
            // btnSetRight
            // 
            btnSetRight.BackColor = Color.FromArgb(210, 210, 210);
            btnSetRight.Cursor = Cursors.Hand;
            btnSetRight.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            btnSetRight.FlatAppearance.BorderSize = 0;
            btnSetRight.FlatStyle = FlatStyle.Flat;
            btnSetRight.Font = new Font("맑은 고딕", 9F);
            btnSetRight.ForeColor = Color.FromArgb(50, 50, 50);
            btnSetRight.Location = new Point(114, 148);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(100, 26);
            btnSetRight.TabIndex = 1;
            btnSetRight.Text = "우측 설정 ⚙";
            btnSetRight.UseVisualStyleBackColor = false;
            btnSetRight.Click += btnSetRight_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.White;
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("맑은 고딕", 9F);
            btnDelete.ForeColor = Color.FromArgb(50, 50, 50);
            btnDelete.Location = new Point(220, 148);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 26);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "⏸ 삭제";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRestore
            // 
            btnRestore.BackColor = Color.FromArgb(210, 210, 210);
            btnRestore.Cursor = Cursors.Hand;
            btnRestore.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            btnRestore.FlatAppearance.BorderSize = 0;
            btnRestore.FlatStyle = FlatStyle.Flat;
            btnRestore.Font = new Font("맑은 고딕", 9F);
            btnRestore.ForeColor = Color.FromArgb(50, 50, 50);
            btnRestore.Location = new Point(316, 148);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(80, 26);
            btnRestore.TabIndex = 4;
            btnRestore.Text = "↩ 복원";
            btnRestore.UseVisualStyleBackColor = false;
            btnRestore.Click += btnRestore_Click;
            // 
            // btnReroadTub
            // 
            btnReroadTub.BackColor = Color.FromArgb(210, 210, 210);
            btnReroadTub.Cursor = Cursors.Hand;
            btnReroadTub.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            btnReroadTub.FlatAppearance.BorderSize = 0;
            btnReroadTub.FlatStyle = FlatStyle.Flat;
            btnReroadTub.Font = new Font("맑은 고딕", 9F);
            btnReroadTub.ForeColor = Color.FromArgb(50, 50, 50);
            btnReroadTub.Location = new Point(402, 148);
            btnReroadTub.Name = "btnReroadTub";
            btnReroadTub.Size = new Size(100, 26);
            btnReroadTub.TabIndex = 5;
            btnReroadTub.Text = "🔄 새로고침";
            btnReroadTub.UseVisualStyleBackColor = false;
            btnReroadTub.Click += btnReroadTub_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(24, 95, 165);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(508, 148);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 26);
            btnSave.TabIndex = 8;
            btnSave.Text = "💾 저장";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // IblRange
            // 
            IblRange.AutoSize = true;
            IblRange.ForeColor = Color.FromArgb(24, 95, 165);
            IblRange.Location = new Point(8, 130);
            IblRange.Name = "IblRange";
            IblRange.Size = new Size(59, 15);
            IblRange.TabIndex = 21;
            IblRange.Text = "범위 설정";
            IblRange.Visible = false;
            // 
            // BtnLeftSet
            // 
            BtnLeftSet.Location = new Point(7, 130);
            BtnLeftSet.Name = "BtnLeftSet";
            BtnLeftSet.Size = new Size(89, 23);
            BtnLeftSet.TabIndex = 22;
            BtnLeftSet.Text = "좌측 설정 ⚙";
            BtnLeftSet.UseVisualStyleBackColor = true;
            BtnLeftSet.Visible = false;
            BtnLeftSet.Click += BtnLeftSet_Click;
            // 
            // BtnRightSet
            // 
            BtnRightSet.Location = new Point(102, 130);
            BtnRightSet.Name = "BtnRightSet";
            BtnRightSet.Size = new Size(90, 24);
            BtnRightSet.TabIndex = 23;
            BtnRightSet.Text = "우측 설정 ⚙";
            BtnRightSet.UseVisualStyleBackColor = true;
            BtnRightSet.Visible = false;
            BtnRightSet.Click += BtnRightSet_Click;
            // 
            // BtnRangeDelete
            // 
            BtnRangeDelete.BackColor = Color.FromArgb(210, 210, 210);
            BtnRangeDelete.Cursor = Cursors.Hand;
            BtnRangeDelete.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            BtnRangeDelete.FlatAppearance.BorderSize = 0;
            BtnRangeDelete.FlatStyle = FlatStyle.Flat;
            BtnRangeDelete.Font = new Font("맑은 고딕", 9F);
            BtnRangeDelete.ForeColor = Color.FromArgb(50, 50, 50);
            BtnRangeDelete.Location = new Point(604, 148);
            BtnRangeDelete.Name = "BtnRangeDelete";
            BtnRangeDelete.Size = new Size(90, 26);
            BtnRangeDelete.TabIndex = 24;
            BtnRangeDelete.Text = "범위 삭제";
            BtnRangeDelete.UseVisualStyleBackColor = false;
            BtnRangeDelete.Click += BtnRangeDelete_Click;
            // 
            // cmbSpeedFilters
            // 
            cmbSpeedFilters.Location = new Point(0, 0);
            cmbSpeedFilters.Name = "cmbSpeedFilters";
            cmbSpeedFilters.Size = new Size(121, 23);
            cmbSpeedFilters.TabIndex = 25;
            cmbSpeedFilters.Visible = false;
            // 
            // cmbAngleFilters
            // 
            cmbAngleFilters.Location = new Point(0, 0);
            cmbAngleFilters.Name = "cmbAngleFilters";
            cmbAngleFilters.Size = new Size(330, 23);
            cmbAngleFilters.TabIndex = 26;
            cmbAngleFilters.Visible = false;
            // 
            // cmbRanges
            // 
            cmbRanges.Location = new Point(720, 150);
            cmbRanges.Name = "cmbRanges";
            cmbRanges.Size = new Size(121, 23);
            cmbRanges.TabIndex = 34;
            cmbRanges.Text = "범위 목록";
            cmbRanges.SelectedIndexChanged += cmbRanges_SelectedIndexChanged;
            // 
            // btnDeleteAllRanges
            // 
            btnDeleteAllRanges.Location = new Point(847, 150);
            btnDeleteAllRanges.Name = "btnDeleteAllRanges";
            btnDeleteAllRanges.Size = new Size(98, 23);
            btnDeleteAllRanges.TabIndex = 28;
            btnDeleteAllRanges.Text = "모든 범위 삭제";
            btnDeleteAllRanges.UseVisualStyleBackColor = true;
            btnDeleteAllRanges.Click += btnDeleteAllRanges_Click;
            // 
            // btnRangeCancel
            // 
            btnRangeCancel.Location = new Point(951, 150);
            btnRangeCancel.Name = "btnRangeCancel";
            btnRangeCancel.Size = new Size(90, 23);
            btnRangeCancel.TabIndex = 33;
            btnRangeCancel.Text = "범위 취소";
            btnRangeCancel.UseVisualStyleBackColor = true;
            btnRangeCancel.Click += btnRangeCancel_Click;
            // 
            // lblDeleteStatus
            // 
            lblDeleteStatus.AutoSize = true;
            lblDeleteStatus.Font = new Font("맑은 고딕", 9F);
            lblDeleteStatus.ForeColor = Color.FromArgb(200, 60, 60);
            lblDeleteStatus.Location = new Point(8, 6);
            lblDeleteStatus.Name = "lblDeleteStatus";
            lblDeleteStatus.Size = new Size(150, 15);
            lblDeleteStatus.TabIndex = 9;
            lblDeleteStatus.Text = "전체 100개  ●  20개 삭제";
            lblDeleteStatus.Visible = false;
            // 
            // pnlGraph
            // 
            pnlGraph.BackColor = Color.FromArgb(244, 243, 238);
            pnlGraph.BorderStyle = BorderStyle.FixedSingle;
            pnlGraph.Dock = DockStyle.Fill;
            pnlGraph.Location = new Point(0, 874);
            pnlGraph.Name = "pnlGraph";
            pnlGraph.Size = new Size(1200, 126);
            pnlGraph.TabIndex = 4;
            // 
            // contextFilter
            // 
            contextFilter.Items.AddRange(new ToolStripItem[] { menuThrottle, menuAngle });
            contextFilter.Name = "contextFilter";
            contextFilter.Size = new Size(99, 48);
            // 
            // menuThrottle
            // 
            menuThrottle.Name = "menuThrottle";
            menuThrottle.Size = new Size(98, 22);
            menuThrottle.Text = "속도";
            menuThrottle.Click += menuThrottle_Click;
            // 
            // menuAngle
            // 
            menuAngle.Name = "menuAngle";
            menuAngle.Size = new Size(98, 22);
            menuAngle.Text = "각도";
            menuAngle.Click += menuAngle_Click;
            // 
            // ucTubManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMinSize = new Size(1200, 1000);
            BackColor = Color.FromArgb(244, 243, 238);
            Controls.Add(pnlGraph);
            Controls.Add(pnlTools);
            Controls.Add(tlpWorkspace);
            Controls.Add(panel1);
            Name = "ucTubManager";
            Size = new Size(1200, 1000);
            tlpWorkspace.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picThrottle).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAngle).EndInit();
            ((System.ComponentModel.ISupportInitialize)picTubImage).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkRecord).EndInit();
            pnlTools.ResumeLayout(false);
            pnlTools.PerformLayout();
            pnlFilter.ResumeLayout(false);
            tplFilter.ResumeLayout(false);
            tplFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpeedMin).EndInit();
            pnlSpeedRange.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudSpeedMax).EndInit();
            pnlAngleRange.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudAngleMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAngleMax).EndInit();
            pnlThrottleText.ResumeLayout(false);
            pnlThrottleText.PerformLayout();
            pnlAngleText.ResumeLayout(false);
            pnlAngleText.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            contextFilter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlWorkspace;
        private TableLayoutPanel tlpWorkspace;
        private Panel panel1;
        private Button btnLoadCarDirectory;
        private Button btnLoadTub;
        private TextBox txtCarDirectory;
        private TextBox txtTub;
        private Panel panel2;
        private Label lblThrottle;
        private PictureBox picThrottle;
        private Label lblAngle;
        private TextBox txtRecordNumber;
        private PictureBox picAngle;
        private Label lblRecordInfo;
        private PictureBox picTubImage;
        private Panel panel3;
        private ComboBox cmbSpeed;
        private Button btnFastNext;
        private Button btnNext;
        private Button btnStartStop;
        private Button btnFastPrev;
        private Button btnPrev;
        private TrackBar trkRecord;
        private Panel pnlTools;
        private Button btnClearFilter;
        private Panel pnlFilter;
        private TableLayoutPanel tplFilter;
        private Label label1;
        private NumericUpDown nudSpeedMin;
        private Panel pnlSpeedRange;
        private Panel panel4;
        private NumericUpDown nudSpeedMax;
        private Panel pnlAngleRange;
        private Panel panel6;
        private NumericUpDown nudAngleMin;
        private NumericUpDown nudAngleMax;
        private Panel pnlThrottleText;
        private Label FilThrottle;
        private Panel pnlAngleText;
        private Label FilAngle;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnApplyFilter;
        private Label lblDeleteStatus;
        private Panel pnlTimeline;
        private Label lblRange;
        private Button btnSetLeft;
        private Button btnSetRight;
        private Button btnDelete;
        private Button btnRestore;
        private Button btnReroadTub;
        private Button btnSave;
        private Panel pnlGraph;
        private ContextMenuStrip contextFilter;
        private ToolStripMenuItem menuThrottle;
        private ToolStripMenuItem menuAngle;
        // 문서10 원본 호환 컨트롤
        private Label IblRange;
        private Button BtnLeftSet;
        private Button BtnRightSet;
        private Button BtnRangeDelete;
        private ComboBox cmbSpeedFilters;
        private ComboBox cmbAngleFilters;
        private ComboBox cmbRanges;
        private Button btnDeleteAllRanges;
        private Button btnRangeCancel;
    }
}