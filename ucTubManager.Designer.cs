namespace DonkeyUi
{
    partial class ucTubManager
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>

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
            btnAngleRemove = new Button();
            btnSpeedRemove = new Button();
            cmbAngleFilters = new ComboBox();
            cmbSpeedFilters = new ComboBox();
            btnDeleteAllRanges = new Button();
            btnRangeCancel = new Button();
            cmbRanges = new ComboBox();
            BtnRangeDelete = new Button();
            BtnRightSet = new Button();
            BtnLeftSet = new Button();
            IblRange = new Label();
            lblDeleteStatus = new Label();
            pnlTimeline = new Panel();
            label1 = new Label();
            nudSpeedMin = new NumericUpDown();
            pnlSpeedRange = new Panel();
            panel4 = new Panel();
            nudSpeedMax = new NumericUpDown();
            FilThrottle = new Label();
            nudAngleMin = new NumericUpDown();
            pnlAngleRange = new Panel();
            panel6 = new Panel();
            nudAngleMax = new NumericUpDown();
            FilAngle = new Label();
            btnApplyFilter = new Button();
            btnClearFilter = new Button();
            btnSetLeft = new Button();
            btnSetRight = new Button();
            btnDelete = new Button();
            btnRestore = new Button();
            btnReroadTub = new Button();
            btnSave = new Button();
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
            ((System.ComponentModel.ISupportInitialize)nudSpeedMin).BeginInit();
            pnlSpeedRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSpeedMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAngleMin).BeginInit();
            pnlAngleRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudAngleMax).BeginInit();
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
            tlpWorkspace.BackColor = Color.FromArgb(18, 25, 42);
            tlpWorkspace.ColumnCount = 2;
            tlpWorkspace.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
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
            panel2.BackColor = Color.White;
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
            panel2.Size = new Size(204, 414);
            panel2.TabIndex = 2;
            // 
            // lblThrottle
            // 
            lblThrottle.BackColor = Color.FromArgb(18, 25, 42);
            lblThrottle.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblThrottle.ForeColor = Color.FromArgb(122, 154, 187);
            lblThrottle.Location = new Point(4, 4);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(200, 18);
            lblThrottle.TabIndex = 15;
            lblThrottle.Text = "속도 THROTTLE";
            // 
            // picThrottle
            // 
            picThrottle.BackColor = Color.FromArgb(18, 25, 42);
            picThrottle.Location = new Point(4, 22);
            picThrottle.Name = "picThrottle";
            picThrottle.Size = new Size(200, 140);
            picThrottle.SizeMode = PictureBoxSizeMode.StretchImage;
            picThrottle.TabIndex = 16;
            picThrottle.TabStop = false;
            // 
            // lblAngle
            // 
            lblAngle.BackColor = Color.FromArgb(18, 25, 42);
            lblAngle.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblAngle.ForeColor = Color.FromArgb(122, 154, 187);
            lblAngle.Location = new Point(4, 168);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(200, 18);
            lblAngle.TabIndex = 14;
            lblAngle.Text = "각도 ANGLE";
            // 
            // txtRecordNumber
            // 
            txtRecordNumber.BackColor = Color.FromArgb(30, 45, 64);
            txtRecordNumber.BorderStyle = BorderStyle.FixedSingle;
            txtRecordNumber.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
            txtRecordNumber.ForeColor = Color.White;
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
            picAngle.BackColor = Color.FromArgb(18, 25, 42);
            picAngle.Location = new Point(4, 186);
            picAngle.Name = "picAngle";
            picAngle.Size = new Size(200, 140);
            picAngle.SizeMode = PictureBoxSizeMode.StretchImage;
            picAngle.TabIndex = 17;
            picAngle.TabStop = false;
            // 
            // lblRecordInfo
            // 
            lblRecordInfo.BackColor = Color.FromArgb(18, 25, 42);
            lblRecordInfo.Font = new Font("맑은 고딕", 9F);
            lblRecordInfo.ForeColor = Color.FromArgb(122, 154, 187);
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
            picTubImage.Location = new Point(213, 3);
            picTubImage.Name = "picTubImage";
            picTubImage.Size = new Size(984, 414);
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
            cmbSpeed.BackColor = Color.FromArgb(30, 45, 64);
            cmbSpeed.FlatStyle = FlatStyle.Flat;
            cmbSpeed.Font = new Font("맑은 고딕", 9F);
            cmbSpeed.ForeColor = Color.White;
            cmbSpeed.Items.AddRange(new object[] { "0.25x", "0.50x", "0.75x", "1.00x", "1.25x", "1.50x", "1.75x", "2.00x" });
            cmbSpeed.Location = new Point(1100, 141);
            cmbSpeed.Name = "cmbSpeed";
            cmbSpeed.Size = new Size(92, 23);
            cmbSpeed.TabIndex = 7;
            cmbSpeed.Text = "1.0x";
            // 
            // btnFastNext
            // 
            btnFastNext.BackColor = Color.FromArgb(30, 45, 64);
            btnFastNext.Cursor = Cursors.Hand;
            btnFastNext.FlatAppearance.BorderColor = Color.FromArgb(42, 74, 106);
            btnFastNext.FlatStyle = FlatStyle.Flat;
            btnFastNext.Font = new Font("맑은 고딕", 11F);
            btnFastNext.ForeColor = Color.FromArgb(200, 200, 200);
            btnFastNext.Location = new Point(1017, 207);
            btnFastNext.Name = "btnFastNext";
            btnFastNext.Size = new Size(180, 32);
            btnFastNext.TabIndex = 6;
            btnFastNext.Text = "⏭";
            btnFastNext.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.FromArgb(30, 45, 64);
            btnNext.Cursor = Cursors.Hand;
            btnNext.FlatAppearance.BorderColor = Color.FromArgb(42, 74, 106);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("맑은 고딕", 11F);
            btnNext.ForeColor = Color.FromArgb(200, 200, 200);
            btnNext.Location = new Point(796, 207);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(204, 32);
            btnNext.TabIndex = 5;
            btnNext.Text = "⏩";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // btnStartStop
            // 
            btnStartStop.BackColor = Color.FromArgb(24, 95, 165);
            btnStartStop.Cursor = Cursors.Hand;
            btnStartStop.FlatAppearance.BorderSize = 0;
            btnStartStop.FlatStyle = FlatStyle.Flat;
            btnStartStop.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnStartStop.ForeColor = Color.White;
            btnStartStop.Location = new Point(418, 207);
            btnStartStop.Name = "btnStartStop";
            btnStartStop.Size = new Size(356, 32);
            btnStartStop.TabIndex = 4;
            btnStartStop.Text = "▶ 재생";
            btnStartStop.UseVisualStyleBackColor = false;
            // 
            // btnFastPrev
            // 
            btnFastPrev.BackColor = Color.FromArgb(30, 45, 64);
            btnFastPrev.Cursor = Cursors.Hand;
            btnFastPrev.FlatAppearance.BorderColor = Color.FromArgb(42, 74, 106);
            btnFastPrev.FlatStyle = FlatStyle.Flat;
            btnFastPrev.Font = new Font("맑은 고딕", 11F);
            btnFastPrev.ForeColor = Color.FromArgb(200, 200, 200);
            btnFastPrev.Location = new Point(8, 207);
            btnFastPrev.Name = "btnFastPrev";
            btnFastPrev.Size = new Size(170, 32);
            btnFastPrev.TabIndex = 3;
            btnFastPrev.Text = "⏮";
            btnFastPrev.UseVisualStyleBackColor = false;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.FromArgb(30, 45, 64);
            btnPrev.Cursor = Cursors.Hand;
            btnPrev.FlatAppearance.BorderColor = Color.FromArgb(42, 74, 106);
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.Font = new Font("맑은 고딕", 11F);
            btnPrev.ForeColor = Color.FromArgb(200, 200, 200);
            btnPrev.Location = new Point(196, 207);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(200, 32);
            btnPrev.TabIndex = 2;
            btnPrev.Text = "⏪";
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
            btnLoadCarDirectory.ForeColor = Color.FromArgb(30, 30, 30);
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
            btnLoadTub.Size = new Size(120, 28);
            btnLoadTub.TabIndex = 5;
            btnLoadTub.Text = "📂 데이터 불러오기";
            btnLoadTub.UseVisualStyleBackColor = false;
            // 
            // txtCarDirectory
            // 
            txtCarDirectory.BackColor = Color.FromArgb(15, 30, 53);
            txtCarDirectory.BorderStyle = BorderStyle.FixedSingle;
            txtCarDirectory.Font = new Font("맑은 고딕", 9F);
            txtCarDirectory.ForeColor = Color.FromArgb(122, 154, 187);
            txtCarDirectory.Location = new Point(124, 10);
            txtCarDirectory.Name = "txtCarDirectory";
            txtCarDirectory.Size = new Size(440, 23);
            txtCarDirectory.TabIndex = 1;
            // 
            // txtTub
            // 
            txtTub.BackColor = Color.FromArgb(15, 30, 53);
            txtTub.BorderStyle = BorderStyle.FixedSingle;
            txtTub.Font = new Font("맑은 고딕", 9F);
            txtTub.ForeColor = Color.FromArgb(122, 154, 187);
            txtTub.Location = new Point(698, 10);
            txtTub.Name = "txtTub";
            txtTub.Size = new Size(490, 23);
            txtTub.TabIndex = 3;
            // 
            // trkRecord
            // 
            trkRecord.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            trkRecord.BackColor = Color.FromArgb(18, 25, 42);
            trkRecord.Location = new Point(8, 141);
            trkRecord.Name = "trkRecord";
            trkRecord.Size = new Size(1076, 45);
            trkRecord.TabIndex = 2;
            trkRecord.TickStyle = TickStyle.None;
            // 
            // pnlTools
            // 
            pnlTools.BackColor = Color.White;
            pnlTools.BorderStyle = BorderStyle.FixedSingle;
            pnlTools.Controls.Add(btnAngleRemove);
            pnlTools.Controls.Add(btnSpeedRemove);
            pnlTools.Controls.Add(cmbAngleFilters);
            pnlTools.Controls.Add(cmbSpeedFilters);
            pnlTools.Controls.Add(btnDeleteAllRanges);
            pnlTools.Controls.Add(btnRangeCancel);
            pnlTools.Controls.Add(cmbRanges);
            pnlTools.Controls.Add(BtnRangeDelete);
            pnlTools.Controls.Add(BtnRightSet);
            pnlTools.Controls.Add(BtnLeftSet);
            pnlTools.Controls.Add(IblRange);
            pnlTools.Controls.Add(lblDeleteStatus);
            pnlTools.Controls.Add(pnlTimeline);
            pnlTools.Controls.Add(trkRecord);
            pnlTools.Controls.Add(cmbSpeed);
            pnlTools.Controls.Add(btnFastPrev);
            pnlTools.Controls.Add(btnPrev);
            pnlTools.Controls.Add(btnStartStop);
            pnlTools.Controls.Add(btnNext);
            pnlTools.Controls.Add(btnFastNext);
            pnlTools.Controls.Add(label1);
            pnlTools.Controls.Add(nudSpeedMin);
            pnlTools.Controls.Add(pnlSpeedRange);
            pnlTools.Controls.Add(nudSpeedMax);
            pnlTools.Controls.Add(FilThrottle);
            pnlTools.Controls.Add(nudAngleMin);
            pnlTools.Controls.Add(pnlAngleRange);
            pnlTools.Controls.Add(nudAngleMax);
            pnlTools.Controls.Add(FilAngle);
            pnlTools.Controls.Add(btnApplyFilter);
            pnlTools.Controls.Add(btnClearFilter);
            pnlTools.Controls.Add(btnSetLeft);
            pnlTools.Controls.Add(btnSetRight);
            pnlTools.Controls.Add(btnDelete);
            pnlTools.Controls.Add(btnRestore);
            pnlTools.Controls.Add(btnReroadTub);
            pnlTools.Controls.Add(btnSave);
            pnlTools.Dock = DockStyle.Top;
            pnlTools.Location = new Point(0, 464);
            pnlTools.Name = "pnlTools";
            pnlTools.Size = new Size(1200, 410);
            pnlTools.TabIndex = 3;
            // 
            // btnAngleRemove
            // 
            btnAngleRemove.Location = new Point(545, 365);
            btnAngleRemove.Name = "btnAngleRemove";
            btnAngleRemove.Size = new Size(75, 23);
            btnAngleRemove.TabIndex = 32;
            btnAngleRemove.Text = "해제";
            btnAngleRemove.UseVisualStyleBackColor = true;
            btnAngleRemove.Click += btnAngleRemove_Click;
            // 
            // btnSpeedRemove
            // 
            btnSpeedRemove.Location = new Point(337, 364);
            btnSpeedRemove.Name = "btnSpeedRemove";
            btnSpeedRemove.Size = new Size(75, 23);
            btnSpeedRemove.TabIndex = 31;
            btnSpeedRemove.Text = "해제";
            btnSpeedRemove.UseVisualStyleBackColor = true;
            btnSpeedRemove.Click += btnSpeedRemove_Click;
            // 
            // cmbAngleFilters
            // 
            cmbAngleFilters.FormattingEnabled = true;
            cmbAngleFilters.Location = new Point(418, 365);
            cmbAngleFilters.Name = "cmbAngleFilters";
            cmbAngleFilters.Size = new Size(121, 23);
            cmbAngleFilters.TabIndex = 30;
            cmbAngleFilters.Text = "각도 범위";
            // 
            // cmbSpeedFilters
            // 
            cmbSpeedFilters.FormattingEnabled = true;
            cmbSpeedFilters.Location = new Point(212, 363);
            cmbSpeedFilters.Name = "cmbSpeedFilters";
            cmbSpeedFilters.Size = new Size(121, 23);
            cmbSpeedFilters.TabIndex = 29;
            cmbSpeedFilters.Text = "속도 범위";
            // 
            // btnDeleteAllRanges
            // 
            btnDeleteAllRanges.Location = new Point(402, 112);
            btnDeleteAllRanges.Name = "btnDeleteAllRanges";
            btnDeleteAllRanges.Size = new Size(98, 23);
            btnDeleteAllRanges.TabIndex = 28;
            btnDeleteAllRanges.Text = "모든 범위 삭제";
            btnDeleteAllRanges.UseVisualStyleBackColor = true;
            btnDeleteAllRanges.Click += btnDeleteAllRanges_Click;
            // 
            // btnRangeCancel
            // 
            btnRangeCancel.Location = new Point(602, 112);
            btnRangeCancel.Name = "btnRangeCancel";
            btnRangeCancel.Size = new Size(90, 23);
            btnRangeCancel.TabIndex = 27;
            btnRangeCancel.Text = "범위 취소";
            btnRangeCancel.UseVisualStyleBackColor = true;
            btnRangeCancel.Click += btnRangeCancel_Click;
            // 
            // cmbRanges
            // 
            cmbRanges.FormattingEnabled = true;
            cmbRanges.Location = new Point(200, 111);
            cmbRanges.Name = "cmbRanges";
            cmbRanges.Size = new Size(196, 23);
            cmbRanges.TabIndex = 26;
            cmbRanges.Text = "범위 목록";
            cmbRanges.SelectedIndexChanged += cmbRanges_SelectedIndexChanged;
            // 
            // BtnRangeDelete
            // 
            BtnRangeDelete.Location = new Point(506, 112);
            BtnRangeDelete.Name = "BtnRangeDelete";
            BtnRangeDelete.Size = new Size(90, 23);
            BtnRangeDelete.TabIndex = 24;
            BtnRangeDelete.Text = "범위 삭제";
            BtnRangeDelete.UseVisualStyleBackColor = true;
            BtnRangeDelete.Click += BtnRangeDelete_Click;
            // 
            // BtnRightSet
            // 
            BtnRightSet.Location = new Point(104, 111);
            BtnRightSet.Name = "BtnRightSet";
            BtnRightSet.Size = new Size(90, 24);
            BtnRightSet.TabIndex = 23;
            BtnRightSet.Text = "우측 설정 ⚙";
            BtnRightSet.UseVisualStyleBackColor = true;
            BtnRightSet.Click += BtnRightSet_Click;
            // 
            // BtnLeftSet
            // 
            BtnLeftSet.Location = new Point(9, 111);
            BtnLeftSet.Name = "BtnLeftSet";
            BtnLeftSet.Size = new Size(89, 23);
            BtnLeftSet.TabIndex = 22;
            BtnLeftSet.Text = "좌측 설정 ⚙";
            BtnLeftSet.UseVisualStyleBackColor = true;
            BtnLeftSet.Click += BtnLeftSet_Click;
            // 
            // IblRange
            // 
            IblRange.AutoSize = true;
            IblRange.ForeColor = Color.FromArgb(0, 192, 192);
            IblRange.Location = new Point(8, 93);
            IblRange.Name = "IblRange";
            IblRange.Size = new Size(59, 15);
            IblRange.TabIndex = 21;
            IblRange.Text = "범위 설정";
            // 
            // lblDeleteStatus
            // 
            lblDeleteStatus.AutoSize = true;
            lblDeleteStatus.Font = new Font("맑은 고딕", 9F);
            lblDeleteStatus.ForeColor = Color.FromArgb(255, 100, 100);
            lblDeleteStatus.Location = new Point(8, 6);
            lblDeleteStatus.Name = "lblDeleteStatus";
            lblDeleteStatus.Size = new Size(150, 15);
            lblDeleteStatus.TabIndex = 9;
            lblDeleteStatus.Text = "전체 100개  ●  20개 삭제";
            lblDeleteStatus.Visible = false;
            // 
            // pnlTimeline
            // 
            pnlTimeline.BackColor = Color.FromArgb(74, 158, 255);
            pnlTimeline.Location = new Point(8, 24);
            pnlTimeline.Name = "pnlTimeline";
            pnlTimeline.Size = new Size(1184, 22);
            pnlTimeline.TabIndex = 20;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(74, 158, 255);
            label1.Location = new Point(8, 244);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 15;
            label1.Text = "필터 기능";
            // 
            // nudSpeedMin
            // 
            nudSpeedMin.BackColor = Color.FromArgb(30, 45, 64);
            nudSpeedMin.BorderStyle = BorderStyle.FixedSingle;
            nudSpeedMin.DecimalPlaces = 3;
            nudSpeedMin.Font = new Font("맑은 고딕", 8.5F);
            nudSpeedMin.ForeColor = Color.White;
            nudSpeedMin.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            nudSpeedMin.Location = new Point(8, 284);
            nudSpeedMin.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSpeedMin.Name = "nudSpeedMin";
            nudSpeedMin.Size = new Size(90, 23);
            nudSpeedMin.TabIndex = 11;
            // 
            // pnlSpeedRange
            // 
            pnlSpeedRange.BackColor = Color.FromArgb(18, 25, 42);
            pnlSpeedRange.Controls.Add(panel4);
            pnlSpeedRange.Location = new Point(104, 284);
            pnlSpeedRange.Name = "pnlSpeedRange";
            pnlSpeedRange.Size = new Size(980, 22);
            pnlSpeedRange.TabIndex = 10;
            pnlSpeedRange.Paint += pnlSpeedRange_Paint;
            // 
            // panel4
            // 
            panel4.Location = new Point(0, 28);
            panel4.Name = "panel4";
            panel4.Size = new Size(1020, 22);
            panel4.TabIndex = 11;
            // 
            // nudSpeedMax
            // 
            nudSpeedMax.BackColor = Color.FromArgb(30, 45, 64);
            nudSpeedMax.BorderStyle = BorderStyle.FixedSingle;
            nudSpeedMax.DecimalPlaces = 3;
            nudSpeedMax.Font = new Font("맑은 고딕", 8.5F);
            nudSpeedMax.ForeColor = Color.White;
            nudSpeedMax.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            nudSpeedMax.Location = new Point(1100, 283);
            nudSpeedMax.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSpeedMax.Name = "nudSpeedMax";
            nudSpeedMax.Size = new Size(86, 23);
            nudSpeedMax.TabIndex = 12;
            nudSpeedMax.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // FilThrottle
            // 
            FilThrottle.AutoSize = true;
            FilThrottle.Font = new Font("맑은 고딕", 8.5F);
            FilThrottle.ForeColor = Color.FromArgb(122, 154, 187);
            FilThrottle.Location = new Point(8, 266);
            FilThrottle.Name = "FilThrottle";
            FilThrottle.Size = new Size(76, 15);
            FilThrottle.TabIndex = 17;
            FilThrottle.Text = "속도 Throttle";
            // 
            // nudAngleMin
            // 
            nudAngleMin.BackColor = Color.FromArgb(30, 45, 64);
            nudAngleMin.BorderStyle = BorderStyle.FixedSingle;
            nudAngleMin.Font = new Font("맑은 고딕", 8.5F);
            nudAngleMin.ForeColor = Color.White;
            nudAngleMin.Location = new Point(8, 332);
            nudAngleMin.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudAngleMin.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            nudAngleMin.Name = "nudAngleMin";
            nudAngleMin.Size = new Size(90, 23);
            nudAngleMin.TabIndex = 13;
            nudAngleMin.Value = new decimal(new int[] { 1, 0, 0, int.MinValue });
            // 
            // pnlAngleRange
            // 
            pnlAngleRange.BackColor = Color.FromArgb(18, 25, 42);
            pnlAngleRange.Controls.Add(panel6);
            pnlAngleRange.Location = new Point(104, 332);
            pnlAngleRange.Name = "pnlAngleRange";
            pnlAngleRange.Size = new Size(980, 22);
            pnlAngleRange.TabIndex = 12;
            pnlAngleRange.Paint += pnlAngleRange_Paint;
            // 
            // panel6
            // 
            panel6.Location = new Point(0, 28);
            panel6.Name = "panel6";
            panel6.Size = new Size(1020, 22);
            panel6.TabIndex = 11;
            // 
            // nudAngleMax
            // 
            nudAngleMax.BackColor = Color.FromArgb(30, 45, 64);
            nudAngleMax.BorderStyle = BorderStyle.FixedSingle;
            nudAngleMax.Font = new Font("맑은 고딕", 8.5F);
            nudAngleMax.ForeColor = Color.White;
            nudAngleMax.Location = new Point(1100, 331);
            nudAngleMax.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudAngleMax.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            nudAngleMax.Name = "nudAngleMax";
            nudAngleMax.Size = new Size(86, 23);
            nudAngleMax.TabIndex = 14;
            nudAngleMax.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // FilAngle
            // 
            FilAngle.AutoSize = true;
            FilAngle.Font = new Font("맑은 고딕", 8.5F);
            FilAngle.ForeColor = Color.FromArgb(122, 154, 187);
            FilAngle.Location = new Point(8, 314);
            FilAngle.Name = "FilAngle";
            FilAngle.Size = new Size(66, 15);
            FilAngle.TabIndex = 16;
            FilAngle.Text = "각도 Angle";
            // 
            // btnApplyFilter
            // 
            btnApplyFilter.BackColor = Color.FromArgb(24, 95, 165);
            btnApplyFilter.Cursor = Cursors.Hand;
            btnApplyFilter.FlatAppearance.BorderSize = 0;
            btnApplyFilter.FlatStyle = FlatStyle.Flat;
            btnApplyFilter.Font = new Font("맑은 고딕", 9F);
            btnApplyFilter.ForeColor = Color.White;
            btnApplyFilter.Location = new Point(8, 362);
            btnApplyFilter.Name = "btnApplyFilter";
            btnApplyFilter.Size = new Size(100, 26);
            btnApplyFilter.TabIndex = 19;
            btnApplyFilter.Text = "✔ 필터 적용";
            btnApplyFilter.UseVisualStyleBackColor = false;
            btnApplyFilter.Click += btnApplyFilter_Click;
            // 
            // btnClearFilter
            // 
            btnClearFilter.BackColor = Color.FromArgb(42, 58, 74);
            btnClearFilter.Cursor = Cursors.Hand;
            btnClearFilter.FlatAppearance.BorderColor = Color.FromArgb(60, 80, 100);
            btnClearFilter.FlatStyle = FlatStyle.Flat;
            btnClearFilter.Font = new Font("맑은 고딕", 9F);
            btnClearFilter.ForeColor = Color.FromArgb(180, 180, 180);
            btnClearFilter.Location = new Point(114, 362);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(93, 26);
            btnClearFilter.TabIndex = 18;
            btnClearFilter.Text = "✕ 모두 해제";
            btnClearFilter.UseVisualStyleBackColor = false;
            btnClearFilter.Click += btnClearFilter_Click;
            // 
            // btnSetLeft
            // 
            btnSetLeft.BackColor = Color.FromArgb(42, 58, 74);
            btnSetLeft.Cursor = Cursors.Hand;
            btnSetLeft.FlatAppearance.BorderColor = Color.FromArgb(60, 80, 100);
            btnSetLeft.FlatStyle = FlatStyle.Flat;
            btnSetLeft.Font = new Font("맑은 고딕", 9F);
            btnSetLeft.ForeColor = Color.FromArgb(180, 180, 180);
            btnSetLeft.Location = new Point(8, 52);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(100, 26);
            btnSetLeft.TabIndex = 0;
            btnSetLeft.Text = "첫장가기";
            btnSetLeft.UseVisualStyleBackColor = false;
            btnSetLeft.Click += btnSetLeft_Click;
            // 
            // btnSetRight
            // 
            btnSetRight.BackColor = Color.FromArgb(42, 58, 74);
            btnSetRight.Cursor = Cursors.Hand;
            btnSetRight.FlatAppearance.BorderColor = Color.FromArgb(60, 80, 100);
            btnSetRight.FlatStyle = FlatStyle.Flat;
            btnSetRight.Font = new Font("맑은 고딕", 9F);
            btnSetRight.ForeColor = Color.FromArgb(180, 180, 180);
            btnSetRight.Location = new Point(114, 52);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(100, 26);
            btnSetRight.TabIndex = 1;
            btnSetRight.Text = "끝장가기";
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
            btnDelete.Location = new Point(220, 52);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 26);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "⏸ 삭제";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRestore
            // 
            btnRestore.BackColor = Color.FromArgb(42, 58, 74);
            btnRestore.Cursor = Cursors.Hand;
            btnRestore.FlatAppearance.BorderColor = Color.FromArgb(60, 80, 100);
            btnRestore.FlatStyle = FlatStyle.Flat;
            btnRestore.Font = new Font("맑은 고딕", 9F);
            btnRestore.ForeColor = Color.FromArgb(180, 180, 180);
            btnRestore.Location = new Point(316, 52);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(80, 26);
            btnRestore.TabIndex = 4;
            btnRestore.Text = "↩ 복원";
            btnRestore.UseVisualStyleBackColor = false;
            btnRestore.Click += btnRestore_Click;
            // 
            // btnReroadTub
            // 
            btnReroadTub.BackColor = Color.FromArgb(42, 58, 74);
            btnReroadTub.Cursor = Cursors.Hand;
            btnReroadTub.FlatAppearance.BorderColor = Color.FromArgb(60, 80, 100);
            btnReroadTub.FlatStyle = FlatStyle.Flat;
            btnReroadTub.Font = new Font("맑은 고딕", 9F);
            btnReroadTub.ForeColor = Color.FromArgb(180, 180, 180);
            btnReroadTub.Location = new Point(402, 52);
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
            btnSave.Location = new Point(508, 52);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 26);
            btnSave.TabIndex = 8;
            btnSave.Text = "💾 저장";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // pnlGraph
            // 
            pnlGraph.BackColor = Color.White;
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
            BackColor = Color.FromArgb(18, 25, 42);
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
            ((System.ComponentModel.ISupportInitialize)nudSpeedMin).EndInit();
            pnlSpeedRange.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudSpeedMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAngleMin).EndInit();
            pnlAngleRange.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudAngleMax).EndInit();
            contextFilter.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion

        private Panel pnlWorkspace;
        private TableLayoutPanel tlpWorkspace;
        private Panel panel1;
        private TextBox txtCarDirectory;
        private TextBox txtTub;
        private Panel panel2;
        private PictureBox picTubImage;
        private Panel panel3;
        private TrackBar trkRecord;
        private Panel pnlTools;
        private Button btnSetLeft;
        private Button btnReroadTub;
        private Button btnRestore;
        private Button btnDelete;
        private Button btnSetRight;
        private Panel pnlGraph;
        private ContextMenuStrip contextFilter;
        private ToolStripMenuItem menuThrottle;
        private ToolStripMenuItem menuAngle;
        private Button btnSave;
        private Label lblDeleteStatus;
        private Label lblRecordInfo;
        private Label lblThrottle;
        private PictureBox picThrottle;
        private PictureBox picAngle;
        private Label lblAngle;
        private ComboBox cmbSpeed;
        private Button btnFastNext;
        private Button btnNext;
        private Button btnStartStop;
        private Button btnFastPrev;
        private Button btnPrev;
        private TextBox txtRecordNumber;
        private Button btnLoadTub;
        private Button btnLoadCarDirectory;
        private Panel pnlSpeedRange;
        private NumericUpDown nudSpeedMax;
        private NumericUpDown nudSpeedMin;
        private NumericUpDown nudAngleMax;
        private NumericUpDown nudAngleMin;
        private Panel panel4;
        private Panel pnlAngleRange;
        private Panel panel6;
        private Label label1;
        private Label FilThrottle;
        private Label FilAngle;
        private Button btnApplyFilter;
        private Button btnClearFilter;
        private Panel pnlTimeline;
        private Label IblRange;
        private Button BtnRightSet;
        private Button BtnLeftSet;
        private Button BtnRangeDelete;
        private ComboBox cmbRanges;
        private Button btnRangeCancel;
        private Button btnDeleteAllRanges;
        private Button btnAngleRemove;
        private Button btnSpeedRemove;
        private ComboBox cmbAngleFilters;
        private ComboBox cmbSpeedFilters;
    }
}
