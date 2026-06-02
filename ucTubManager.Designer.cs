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
            lblRecordInfo = new Label();
            lblThrottle = new Label();
            picThrottle = new PictureBox();
            picAngle = new PictureBox();
            lblAngle = new Label();
            picTubImage = new PictureBox();
            panel3 = new Panel();
            cmbSpeed = new ComboBox();
            btnFastNext = new Button();
            btnNext = new Button();
            btnStartStop = new Button();
            btnFastPrev = new Button();
            btnPrev = new Button();
            txtRecordNumber = new TextBox();
            panel1 = new Panel();
            btnLoadTub = new Button();
            btnLoadCarDirectory = new Button();
            txtTub = new TextBox();
            txtCarDirectory = new TextBox();
            trkRecord = new TrackBar();
            pnlTools = new Panel();
            pnlTimeline = new Panel();
            btnApplyFilter = new Button();
            btnClearFilter = new Button();
            FilThrottle = new Label();
            FilAngle = new Label();
            label1 = new Label();
            pnlAngleRange = new Panel();
            panel6 = new Panel();
            nudAngleMax = new NumericUpDown();
            nudAngleMin = new NumericUpDown();
            nudSpeedMax = new NumericUpDown();
            nudSpeedMin = new NumericUpDown();
            pnlSpeedRange = new Panel();
            panel4 = new Panel();
            lblDeleteStatus = new Label();
            btnSave = new Button();
            btnReroadTub = new Button();
            btnRestore = new Button();
            btnDelete = new Button();
            btnSetRight = new Button();
            btnSetLeft = new Button();
            pnlGraph = new Panel();
            contextFilter = new ContextMenuStrip(components);
            menuThrottle = new ToolStripMenuItem();
            menuAngle = new ToolStripMenuItem();
            tlpWorkspace.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picThrottle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAngle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picTubImage).BeginInit();
            panel3.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkRecord).BeginInit();
            pnlTools.SuspendLayout();
            pnlAngleRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudAngleMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAngleMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeedMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeedMin).BeginInit();
            pnlSpeedRange.SuspendLayout();
            contextFilter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlWorkspace
            // 
            pnlWorkspace.Location = new Point(0, 59);
            pnlWorkspace.Name = "pnlWorkspace";
            pnlWorkspace.Size = new Size(947, 232);
            pnlWorkspace.TabIndex = 0;
            // 
            // tlpWorkspace
            // 
            tlpWorkspace.ColumnCount = 3;
            tlpWorkspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34.0455055F));
            tlpWorkspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36.7726364F));
            tlpWorkspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29.18186F));
            tlpWorkspace.Controls.Add(panel2, 0, 0);
            tlpWorkspace.Controls.Add(picTubImage, 1, 0);
            tlpWorkspace.Controls.Add(panel3, 2, 0);
            tlpWorkspace.Dock = DockStyle.Top;
            tlpWorkspace.Location = new Point(0, 59);
            tlpWorkspace.Name = "tlpWorkspace";
            tlpWorkspace.RowCount = 1;
            tlpWorkspace.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpWorkspace.Size = new Size(947, 285);
            tlpWorkspace.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblRecordInfo);
            panel2.Controls.Add(lblThrottle);
            panel2.Controls.Add(picThrottle);
            panel2.Controls.Add(picAngle);
            panel2.Controls.Add(lblAngle);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(316, 279);
            panel2.TabIndex = 2;
            // 
            // lblRecordInfo
            // 
            lblRecordInfo.BackColor = Color.FromArgb(80, 80, 80);
            lblRecordInfo.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblRecordInfo.ForeColor = Color.White;
            lblRecordInfo.Location = new Point(-6, -48);
            lblRecordInfo.Name = "lblRecordInfo";
            lblRecordInfo.Size = new Size(328, 44);
            lblRecordInfo.TabIndex = 13;
            lblRecordInfo.Text = "기록 사진";
            lblRecordInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblThrottle
            // 
            lblThrottle.BackColor = Color.FromArgb(80, 80, 80);
            lblThrottle.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblThrottle.ForeColor = Color.White;
            lblThrottle.Location = new Point(-6, -1);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(106, 137);
            lblThrottle.TabIndex = 15;
            lblThrottle.Text = "속도";
            lblThrottle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picThrottle
            // 
            picThrottle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            picThrottle.Location = new Point(106, -1);
            picThrottle.Name = "picThrottle";
            picThrottle.Size = new Size(216, 137);
            picThrottle.SizeMode = PictureBoxSizeMode.StretchImage;
            picThrottle.TabIndex = 16;
            picThrottle.TabStop = false;
            // 
            // picAngle
            // 
            picAngle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            picAngle.Location = new Point(106, 142);
            picAngle.Name = "picAngle";
            picAngle.Size = new Size(216, 133);
            picAngle.SizeMode = PictureBoxSizeMode.StretchImage;
            picAngle.TabIndex = 17;
            picAngle.TabStop = false;
            // 
            // lblAngle
            // 
            lblAngle.BackColor = Color.FromArgb(80, 80, 80);
            lblAngle.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblAngle.ForeColor = Color.White;
            lblAngle.Location = new Point(-6, 142);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(106, 133);
            lblAngle.TabIndex = 14;
            lblAngle.Text = "각도";
            lblAngle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picTubImage
            // 
            picTubImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picTubImage.BackColor = Color.Black;
            picTubImage.Location = new Point(325, 3);
            picTubImage.Name = "picTubImage";
            picTubImage.Size = new Size(342, 279);
            picTubImage.SizeMode = PictureBoxSizeMode.StretchImage;
            picTubImage.TabIndex = 3;
            picTubImage.TabStop = false;
            // 
            // panel3
            // 
            panel3.Controls.Add(cmbSpeed);
            panel3.Controls.Add(btnFastNext);
            panel3.Controls.Add(btnNext);
            panel3.Controls.Add(btnStartStop);
            panel3.Controls.Add(btnFastPrev);
            panel3.Controls.Add(btnPrev);
            panel3.Controls.Add(txtRecordNumber);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(673, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(271, 279);
            panel3.TabIndex = 4;
            // 
            // cmbSpeed
            // 
            cmbSpeed.BackColor = Color.DimGray;
            cmbSpeed.Font = new Font("맑은 고딕", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
            cmbSpeed.ForeColor = Color.White;
            cmbSpeed.FormattingEnabled = true;
            cmbSpeed.Items.AddRange(new object[] { "0.25x", "0.50x", "0.75x", "1.00x", "1.25x", "1.50x", "1.75x", "2.00x" });
            cmbSpeed.Location = new Point(133, 9);
            cmbSpeed.Name = "cmbSpeed";
            cmbSpeed.Size = new Size(132, 48);
            cmbSpeed.TabIndex = 7;
            cmbSpeed.Text = "1.00x";
            // 
            // btnFastNext
            // 
            btnFastNext.BackColor = Color.FromArgb(80, 80, 80);
            btnFastNext.FlatStyle = FlatStyle.Flat;
            btnFastNext.Font = new Font("맑은 고딕", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnFastNext.ForeColor = Color.White;
            btnFastNext.Location = new Point(133, 136);
            btnFastNext.Name = "btnFastNext";
            btnFastNext.Size = new Size(132, 65);
            btnFastNext.TabIndex = 6;
            btnFastNext.Text = ">>";
            btnFastNext.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.FromArgb(80, 80, 80);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("맑은 고딕", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnNext.ForeColor = Color.White;
            btnNext.Location = new Point(133, 63);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(132, 67);
            btnNext.TabIndex = 5;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // btnStartStop
            // 
            btnStartStop.BackColor = Color.FromArgb(80, 80, 80);
            btnStartStop.FlatStyle = FlatStyle.Flat;
            btnStartStop.Font = new Font("맑은 고딕", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnStartStop.ForeColor = Color.White;
            btnStartStop.Location = new Point(4, 207);
            btnStartStop.Name = "btnStartStop";
            btnStartStop.Size = new Size(261, 68);
            btnStartStop.TabIndex = 4;
            btnStartStop.Text = "시작";
            btnStartStop.UseVisualStyleBackColor = false;
            // 
            // btnFastPrev
            // 
            btnFastPrev.BackColor = Color.FromArgb(80, 80, 80);
            btnFastPrev.FlatStyle = FlatStyle.Flat;
            btnFastPrev.Font = new Font("맑은 고딕", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnFastPrev.ForeColor = Color.White;
            btnFastPrev.Location = new Point(4, 136);
            btnFastPrev.Name = "btnFastPrev";
            btnFastPrev.Size = new Size(123, 65);
            btnFastPrev.TabIndex = 3;
            btnFastPrev.Text = "<<";
            btnFastPrev.UseVisualStyleBackColor = false;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.FromArgb(80, 80, 80);
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.Font = new Font("맑은 고딕", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnPrev.ForeColor = Color.White;
            btnPrev.Location = new Point(4, 63);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(123, 67);
            btnPrev.TabIndex = 2;
            btnPrev.Text = "<";
            btnPrev.UseVisualStyleBackColor = false;
            // 
            // txtRecordNumber
            // 
            txtRecordNumber.BackColor = Color.FromArgb(80, 80, 80);
            txtRecordNumber.BorderStyle = BorderStyle.None;
            txtRecordNumber.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            txtRecordNumber.ForeColor = Color.White;
            txtRecordNumber.Location = new Point(5, 7);
            txtRecordNumber.Multiline = true;
            txtRecordNumber.Name = "txtRecordNumber";
            txtRecordNumber.Size = new Size(121, 50);
            txtRecordNumber.TabIndex = 0;
            txtRecordNumber.Text = "기록 000000";
            txtRecordNumber.TextAlign = HorizontalAlignment.Center;
            txtRecordNumber.ReadOnly = false;
            txtRecordNumber.BorderStyle = BorderStyle.FixedSingle;
            txtRecordNumber.Leave += TxtRecordNumber_Leave;
            txtRecordNumber.KeyDown += TxtRecordNumber_KeyDown;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnLoadTub);
            panel1.Controls.Add(btnLoadCarDirectory);
            panel1.Controls.Add(txtTub);
            panel1.Controls.Add(txtCarDirectory);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(947, 59);
            panel1.TabIndex = 1;
            // 
            // btnLoadTub
            // 
            btnLoadTub.ForeColor = Color.Black;
            btnLoadTub.Location = new Point(489, 12);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Size = new Size(139, 33);
            btnLoadTub.TabIndex = 5;
            btnLoadTub.Text = "데이터 불러오기";
            btnLoadTub.UseVisualStyleBackColor = true;
            // 
            // btnLoadCarDirectory
            // 
            btnLoadCarDirectory.ForeColor = Color.Black;
            btnLoadCarDirectory.Location = new Point(13, 12);
            btnLoadCarDirectory.Name = "btnLoadCarDirectory";
            btnLoadCarDirectory.Size = new Size(139, 33);
            btnLoadCarDirectory.TabIndex = 4;
            btnLoadCarDirectory.Text = "차량 폴더 불러오기";
            btnLoadCarDirectory.UseVisualStyleBackColor = true;
            // 
            // txtTub
            // 
            txtTub.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            txtTub.Location = new Point(634, 12);
            txtTub.Name = "txtTub";
            txtTub.Size = new Size(304, 33);
            txtTub.TabIndex = 3;
            // 
            // txtCarDirectory
            // 
            txtCarDirectory.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            txtCarDirectory.Location = new Point(158, 12);
            txtCarDirectory.Name = "txtCarDirectory";
            txtCarDirectory.Size = new Size(310, 33);
            txtCarDirectory.TabIndex = 1;
            // 
            // trkRecord
            // 
            trkRecord.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            trkRecord.Location = new Point(3, 6);
            trkRecord.Name = "trkRecord";
            trkRecord.Size = new Size(943, 45);
            trkRecord.TabIndex = 2;
            // 
            // pnlTools
            // 
            pnlTools.Controls.Add(pnlTimeline);
            pnlTools.Controls.Add(btnApplyFilter);
            pnlTools.Controls.Add(btnClearFilter);
            pnlTools.Controls.Add(FilThrottle);
            pnlTools.Controls.Add(FilAngle);
            pnlTools.Controls.Add(label1);
            pnlTools.Controls.Add(pnlAngleRange);
            pnlTools.Controls.Add(nudAngleMax);
            pnlTools.Controls.Add(nudAngleMin);
            pnlTools.Controls.Add(nudSpeedMax);
            pnlTools.Controls.Add(nudSpeedMin);
            pnlTools.Controls.Add(pnlSpeedRange);
            pnlTools.Controls.Add(lblDeleteStatus);
            pnlTools.Controls.Add(btnSave);
            pnlTools.Controls.Add(trkRecord);
            pnlTools.Controls.Add(btnReroadTub);
            pnlTools.Controls.Add(btnRestore);
            pnlTools.Controls.Add(btnDelete);
            pnlTools.Controls.Add(btnSetRight);
            pnlTools.Controls.Add(btnSetLeft);
            pnlTools.Dock = DockStyle.Top;
            pnlTools.Location = new Point(0, 344);
            pnlTools.Name = "pnlTools";
            pnlTools.Size = new Size(947, 312);
            pnlTools.TabIndex = 3;
            // 
            // pnlTimeline
            // 
            pnlTimeline.Location = new Point(3, 52);
            pnlTimeline.Name = "pnlTimeline";
            pnlTimeline.Size = new Size(943, 45);
            pnlTimeline.TabIndex = 20;
            // 
            // btnApplyFilter
            // 
            btnApplyFilter.Location = new Point(644, 171);
            btnApplyFilter.Name = "btnApplyFilter";
            btnApplyFilter.Size = new Size(115, 26);
            btnApplyFilter.TabIndex = 19;
            btnApplyFilter.Text = "필터 적용";
            btnApplyFilter.UseVisualStyleBackColor = true;
            btnApplyFilter.Click += btnApplyFilter_Click;
            // 
            // btnClearFilter
            // 
            btnClearFilter.Location = new Point(765, 171);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(115, 26);
            btnClearFilter.TabIndex = 18;
            btnClearFilter.Text = "필터 해제";
            btnClearFilter.UseVisualStyleBackColor = true;
            btnClearFilter.Click += btnClearFilter_Click;
            // 
            // FilThrottle
            // 
            FilThrottle.AutoSize = true;
            FilThrottle.BackColor = Color.FromArgb(192, 255, 255);
            FilThrottle.Location = new Point(3, 182);
            FilThrottle.Name = "FilThrottle";
            FilThrottle.Size = new Size(31, 15);
            FilThrottle.TabIndex = 17;
            FilThrottle.Text = "속도";
            // 
            // FilAngle
            // 
            FilAngle.AutoSize = true;
            FilAngle.BackColor = Color.FromArgb(128, 255, 128);
            FilAngle.Location = new Point(3, 240);
            FilAngle.Name = "FilAngle";
            FilAngle.Size = new Size(31, 15);
            FilAngle.TabIndex = 16;
            FilAngle.Text = "각도";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.WhiteSmoke;
            label1.ForeColor = Color.Blue;
            label1.Location = new Point(0, 158);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 15;
            label1.Text = "필터 기능";
            // 
            // pnlAngleRange
            // 
            pnlAngleRange.Controls.Add(panel6);
            pnlAngleRange.Location = new Point(59, 255);
            pnlAngleRange.Name = "pnlAngleRange";
            pnlAngleRange.Size = new Size(821, 37);
            pnlAngleRange.TabIndex = 12;
            // 
            // panel6
            // 
            panel6.Location = new Point(0, 43);
            panel6.Name = "panel6";
            panel6.Size = new Size(835, 37);
            panel6.TabIndex = 11;
            // 
            // nudAngleMax
            // 
            nudAngleMax.Location = new Point(886, 258);
            nudAngleMax.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudAngleMax.Minimum = new decimal(new int[] { 10, 0, 0, int.MinValue });
            nudAngleMax.Name = "nudAngleMax";
            nudAngleMax.Size = new Size(59, 23);
            nudAngleMax.TabIndex = 14;
            nudAngleMax.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // nudAngleMin
            // 
            nudAngleMin.Location = new Point(3, 258);
            nudAngleMin.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudAngleMin.Minimum = new decimal(new int[] { 10, 0, 0, int.MinValue });
            nudAngleMin.Name = "nudAngleMin";
            nudAngleMin.Size = new Size(50, 23);
            nudAngleMin.TabIndex = 13;
            nudAngleMin.Value = new decimal(new int[] { 10, 0, 0, int.MinValue });
            // 
            // nudSpeedMax
            // 
            nudSpeedMax.Location = new Point(886, 200);
            nudSpeedMax.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudSpeedMax.Name = "nudSpeedMax";
            nudSpeedMax.Size = new Size(59, 23);
            nudSpeedMax.TabIndex = 12;
            nudSpeedMax.Value = new decimal(new int[] { 200, 0, 0, 0 });
            // 
            // nudSpeedMin
            // 
            nudSpeedMin.Location = new Point(3, 200);
            nudSpeedMin.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudSpeedMin.Name = "nudSpeedMin";
            nudSpeedMin.Size = new Size(50, 23);
            nudSpeedMin.TabIndex = 11;
            // 
            // pnlSpeedRange
            // 
            pnlSpeedRange.Controls.Add(panel4);
            pnlSpeedRange.Location = new Point(59, 200);
            pnlSpeedRange.Name = "pnlSpeedRange";
            pnlSpeedRange.Size = new Size(821, 37);
            pnlSpeedRange.TabIndex = 10;
            pnlSpeedRange.Paint += pnlSpeedRange_Paint;
            // 
            // panel4
            // 
            panel4.Location = new Point(0, 43);
            panel4.Name = "panel4";
            panel4.Size = new Size(835, 37);
            panel4.TabIndex = 11;
            // 
            // lblDeleteStatus
            // 
            lblDeleteStatus.AutoSize = true;
            lblDeleteStatus.ForeColor = Color.FromArgb(255, 128, 128);
            lblDeleteStatus.Location = new Point(325, 100);
            lblDeleteStatus.Name = "lblDeleteStatus";
            lblDeleteStatus.Size = new Size(105, 15);
            lblDeleteStatus.TabIndex = 9;
            lblDeleteStatus.Text = "0개 중 0개 삭제됨";
            lblDeleteStatus.Visible = false;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(779, 117);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(149, 38);
            btnSave.TabIndex = 8;
            btnSave.Text = "저장 💾";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnReroadTub
            // 
            btnReroadTub.Location = new Point(619, 118);
            btnReroadTub.Name = "btnReroadTub";
            btnReroadTub.Size = new Size(154, 37);
            btnReroadTub.TabIndex = 5;
            btnReroadTub.Text = "새로고침 🔄";
            btnReroadTub.UseVisualStyleBackColor = true;
            btnReroadTub.Click += btnReroadTub_Click;
            // 
            // btnRestore
            // 
            btnRestore.Location = new Point(461, 118);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(152, 37);
            btnRestore.TabIndex = 4;
            btnRestore.Text = "복원 ↩️";
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(306, 118);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(149, 37);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "삭제 🗑️";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSetRight
            // 
            btnSetRight.BackColor = Color.FromArgb(80, 80, 80);
            btnSetRight.FlatStyle = FlatStyle.Flat;
            btnSetRight.ForeColor = Color.White;
            btnSetRight.Location = new Point(150, 118);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(150, 37);
            btnSetRight.TabIndex = 1;
            btnSetRight.Text = "우측 설정 ⚙️";
            btnSetRight.UseVisualStyleBackColor = false;
            btnSetRight.Click += btnSetRight_Click;
            // 
            // btnSetLeft
            // 
            btnSetLeft.BackColor = Color.FromArgb(80, 80, 80);
            btnSetLeft.FlatStyle = FlatStyle.Flat;
            btnSetLeft.ForeColor = Color.White;
            btnSetLeft.Location = new Point(0, 118);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(144, 37);
            btnSetLeft.TabIndex = 0;
            btnSetLeft.Text = "좌측 설정 ⚙️";
            btnSetLeft.UseVisualStyleBackColor = false;
            btnSetLeft.Click += btnSetLeft_Click;
            // 
            // pnlGraph
            // 
            pnlGraph.BackColor = Color.Black;
            pnlGraph.Dock = DockStyle.Fill;
            pnlGraph.Location = new Point(0, 656);
            pnlGraph.Name = "pnlGraph";
            pnlGraph.Size = new Size(947, 35);
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
            Controls.Add(pnlGraph);
            Controls.Add(pnlTools);
            Controls.Add(tlpWorkspace);
            Controls.Add(panel1);
            Name = "ucTubManager";
            Size = new Size(947, 691);
            tlpWorkspace.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picThrottle).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAngle).EndInit();
            ((System.ComponentModel.ISupportInitialize)picTubImage).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkRecord).EndInit();
            pnlTools.ResumeLayout(false);
            pnlTools.PerformLayout();
            pnlAngleRange.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)nudAngleMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAngleMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeedMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSpeedMin).EndInit();
            pnlSpeedRange.ResumeLayout(false);
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
    }
}
