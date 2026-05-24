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
            pnlWorkspace = new Panel();
            panel1 = new Panel();
            txtTub = new TextBox();
            btnLoadTub = new Button();
            txtCarDirectory = new TextBox();
            btnLoadCarDirectory = new Button();
            trkRecord = new TrackBar();
            pnlTools = new Panel();
            textBox3 = new TextBox();
            btnSetFillter = new Button();
            btnReroadTub = new Button();
            btnRestore = new Button();
            btnDelete = new Button();
            btnSetRight = new Button();
            btnSetLeft = new Button();
            pnlGraph = new Panel();
            picTubImage = new PictureBox();
            lblRecordNumber = new Label();
            btnPrev = new Button();
            btnFastPrev = new Button();
            btnStartStop = new Button();
            btnNext = new Button();
            btnFastNext = new Button();
            cmbSpeed = new ComboBox();
            lblRecordInfo = new Label();
            lblAngle = new Label();
            lblThrottle = new Label();
            picThrottle = new PictureBox();
            picAngle = new PictureBox();
            panel3 = new Panel();
            panel2 = new Panel();
            pnlCenter = new Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkRecord).BeginInit();
            pnlTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picTubImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picThrottle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picAngle).BeginInit();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            pnlCenter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlWorkspace
            // 
            pnlWorkspace.Location = new Point(0, 59);
            pnlWorkspace.Name = "pnlWorkspace";
            pnlWorkspace.Size = new Size(947, 232);
            pnlWorkspace.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(40, 40, 40);
            panel1.Controls.Add(txtTub);
            panel1.Controls.Add(btnLoadTub);
            panel1.Controls.Add(txtCarDirectory);
            panel1.Controls.Add(btnLoadCarDirectory);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(947, 53);
            panel1.TabIndex = 1;
            // 
            // txtTub
            // 
            txtTub.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTub.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            txtTub.Location = new Point(634, 12);
            txtTub.Name = "txtTub";
            txtTub.Size = new Size(304, 33);
            txtTub.TabIndex = 3;
            // 
            // btnLoadTub
            // 
            btnLoadTub.ForeColor = Color.Black;
            btnLoadTub.Location = new Point(489, 12);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Size = new Size(139, 33);
            btnLoadTub.TabIndex = 2;
            btnLoadTub.Text = "데이터 불러오기";
            btnLoadTub.UseVisualStyleBackColor = true;
            // 
            // txtCarDirectory
            // 
            txtCarDirectory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtCarDirectory.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            txtCarDirectory.Location = new Point(158, 12);
            txtCarDirectory.Name = "txtCarDirectory";
            txtCarDirectory.Size = new Size(310, 33);
            txtCarDirectory.TabIndex = 1;
            // 
            // btnLoadCarDirectory
            // 
            btnLoadCarDirectory.ForeColor = Color.Black;
            btnLoadCarDirectory.Location = new Point(13, 12);
            btnLoadCarDirectory.Name = "btnLoadCarDirectory";
            btnLoadCarDirectory.Size = new Size(139, 33);
            btnLoadCarDirectory.TabIndex = 0;
            btnLoadCarDirectory.Text = "차량 폴더 불러오기";
            btnLoadCarDirectory.UseVisualStyleBackColor = true;
            // 
            // trkRecord
            // 
            trkRecord.BackColor = Color.DarkGray;
            trkRecord.Dock = DockStyle.Top;
            trkRecord.Location = new Point(0, 0);
            trkRecord.Name = "trkRecord";
            trkRecord.Size = new Size(947, 45);
            trkRecord.TabIndex = 2;
            // 
            // pnlTools
            // 
            pnlTools.BackColor = Color.FromArgb(40, 40, 40);
            pnlTools.Controls.Add(textBox3);
            pnlTools.Controls.Add(btnSetFillter);
            pnlTools.Controls.Add(trkRecord);
            pnlTools.Controls.Add(btnReroadTub);
            pnlTools.Controls.Add(btnRestore);
            pnlTools.Controls.Add(btnDelete);
            pnlTools.Controls.Add(btnSetRight);
            pnlTools.Controls.Add(btnSetLeft);
            pnlTools.Dock = DockStyle.Bottom;
            pnlTools.Location = new Point(0, 386);
            pnlTools.Name = "pnlTools";
            pnlTools.Size = new Size(947, 113);
            pnlTools.TabIndex = 3;
            // 
            // textBox3
            // 
            textBox3.BackColor = Color.WhiteSmoke;
            textBox3.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            textBox3.ForeColor = Color.White;
            textBox3.Location = new Point(158, 87);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(786, 29);
            textBox3.TabIndex = 7;
            // 
            // btnSetFillter
            // 
            btnSetFillter.Location = new Point(308, 51);
            btnSetFillter.Name = "btnSetFillter";
            btnSetFillter.Size = new Size(151, 30);
            btnSetFillter.TabIndex = 6;
            btnSetFillter.Text = "필터 적용";
            btnSetFillter.UseVisualStyleBackColor = true;
            // 
            // btnReroadTub
            // 
            btnReroadTub.Location = new Point(778, 51);
            btnReroadTub.Name = "btnReroadTub";
            btnReroadTub.Size = new Size(166, 30);
            btnReroadTub.TabIndex = 5;
            btnReroadTub.Text = "새로고침 🔄";
            btnReroadTub.UseVisualStyleBackColor = true;
            // 
            // btnRestore
            // 
            btnRestore.Location = new Point(620, 51);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(152, 30);
            btnRestore.TabIndex = 4;
            btnRestore.Text = "복원 ↩️";
            btnRestore.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(465, 51);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(149, 30);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "삭제 🗑️";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnSetRight
            // 
            btnSetRight.BackColor = Color.FromArgb(80, 80, 80);
            btnSetRight.FlatStyle = FlatStyle.Flat;
            btnSetRight.ForeColor = Color.White;
            btnSetRight.Location = new Point(158, 51);
            btnSetRight.Name = "btnSetRight";
            btnSetRight.Size = new Size(144, 30);
            btnSetRight.TabIndex = 1;
            btnSetRight.Text = "우측 설정 ⚙️";
            btnSetRight.UseVisualStyleBackColor = false;
            // 
            // btnSetLeft
            // 
            btnSetLeft.BackColor = Color.FromArgb(80, 80, 80);
            btnSetLeft.FlatStyle = FlatStyle.Flat;
            btnSetLeft.ForeColor = Color.White;
            btnSetLeft.Location = new Point(0, 51);
            btnSetLeft.Name = "btnSetLeft";
            btnSetLeft.Size = new Size(152, 30);
            btnSetLeft.TabIndex = 0;
            btnSetLeft.Text = "좌측 설정 ⚙️";
            btnSetLeft.UseVisualStyleBackColor = false;
            // 
            // pnlGraph
            // 
            pnlGraph.BackColor = Color.Black;
            pnlGraph.Dock = DockStyle.Bottom;
            pnlGraph.Location = new Point(0, 499);
            pnlGraph.Name = "pnlGraph";
            pnlGraph.Size = new Size(947, 192);
            pnlGraph.TabIndex = 4;
            // 
            // picTubImage
            // 
            picTubImage.BackColor = Color.Black;
            picTubImage.Dock = DockStyle.Fill;
            picTubImage.Location = new Point(334, 0);
            picTubImage.Name = "picTubImage";
            picTubImage.Size = new Size(613, 333);
            picTubImage.SizeMode = PictureBoxSizeMode.StretchImage;
            picTubImage.TabIndex = 3;
            picTubImage.TabStop = false;
            // 
            // lblRecordNumber
            // 
            lblRecordNumber.BackColor = Color.FromArgb(80, 80, 80);
            lblRecordNumber.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblRecordNumber.ForeColor = Color.White;
            lblRecordNumber.Location = new Point(3, 0);
            lblRecordNumber.Name = "lblRecordNumber";
            lblRecordNumber.Size = new Size(140, 49);
            lblRecordNumber.TabIndex = 0;
            lblRecordNumber.Text = "기록 000000";
            lblRecordNumber.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.FromArgb(80, 80, 80);
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.Font = new Font("맑은 고딕", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnPrev.ForeColor = Color.White;
            btnPrev.Location = new Point(3, 52);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(140, 81);
            btnPrev.TabIndex = 2;
            btnPrev.Text = "<";
            btnPrev.UseVisualStyleBackColor = false;
            // 
            // btnFastPrev
            // 
            btnFastPrev.BackColor = Color.FromArgb(80, 80, 80);
            btnFastPrev.FlatStyle = FlatStyle.Flat;
            btnFastPrev.Font = new Font("맑은 고딕", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnFastPrev.ForeColor = Color.White;
            btnFastPrev.Location = new Point(3, 139);
            btnFastPrev.Name = "btnFastPrev";
            btnFastPrev.Size = new Size(140, 87);
            btnFastPrev.TabIndex = 3;
            btnFastPrev.Text = "<<";
            btnFastPrev.UseVisualStyleBackColor = false;
            // 
            // btnStartStop
            // 
            btnStartStop.BackColor = Color.FromArgb(80, 80, 80);
            btnStartStop.FlatStyle = FlatStyle.Flat;
            btnStartStop.Font = new Font("맑은 고딕", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnStartStop.ForeColor = Color.White;
            btnStartStop.Location = new Point(3, 232);
            btnStartStop.Name = "btnStartStop";
            btnStartStop.Size = new Size(280, 75);
            btnStartStop.TabIndex = 4;
            btnStartStop.Text = "재생 ▶️ / 정지 ⏹️";
            btnStartStop.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.FromArgb(80, 80, 80);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("맑은 고딕", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnNext.ForeColor = Color.White;
            btnNext.Location = new Point(149, 52);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(134, 81);
            btnNext.TabIndex = 5;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // btnFastNext
            // 
            btnFastNext.BackColor = Color.FromArgb(80, 80, 80);
            btnFastNext.FlatStyle = FlatStyle.Flat;
            btnFastNext.Font = new Font("맑은 고딕", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnFastNext.ForeColor = Color.White;
            btnFastNext.Location = new Point(149, 139);
            btnFastNext.Name = "btnFastNext";
            btnFastNext.Size = new Size(134, 87);
            btnFastNext.TabIndex = 6;
            btnFastNext.Text = ">>";
            btnFastNext.UseVisualStyleBackColor = false;
            // 
            // cmbSpeed
            // 
            cmbSpeed.BackColor = Color.DimGray;
            cmbSpeed.Font = new Font("맑은 고딕", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
            cmbSpeed.ForeColor = Color.White;
            cmbSpeed.FormattingEnabled = true;
            cmbSpeed.Items.AddRange(new object[] { "0.25", "0.50", "0.75", "1.00", "1.25", "1.50", "1.75", "2.00" });
            cmbSpeed.Location = new Point(149, 1);
            cmbSpeed.Name = "cmbSpeed";
            cmbSpeed.Size = new Size(137, 48);
            cmbSpeed.TabIndex = 7;
            cmbSpeed.Text = "1.00";
            // 
            // lblRecordInfo
            // 
            lblRecordInfo.BackColor = Color.FromArgb(80, 80, 80);
            lblRecordInfo.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblRecordInfo.ForeColor = Color.White;
            lblRecordInfo.Location = new Point(3, 0);
            lblRecordInfo.Name = "lblRecordInfo";
            lblRecordInfo.Size = new Size(328, 39);
            lblRecordInfo.TabIndex = 0;
            lblRecordInfo.Text = "기록 사진";
            lblRecordInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAngle
            // 
            lblAngle.BackColor = Color.FromArgb(80, 80, 80);
            lblAngle.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblAngle.ForeColor = Color.White;
            lblAngle.Location = new Point(3, 191);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(106, 130);
            lblAngle.TabIndex = 2;
            lblAngle.Text = "각도";
            lblAngle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblThrottle
            // 
            lblThrottle.BackColor = Color.FromArgb(80, 80, 80);
            lblThrottle.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblThrottle.ForeColor = Color.White;
            lblThrottle.Location = new Point(3, 48);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(106, 137);
            lblThrottle.TabIndex = 4;
            lblThrottle.Text = "속도";
            lblThrottle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picThrottle
            // 
            picThrottle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            picThrottle.Location = new Point(115, 48);
            picThrottle.Name = "picThrottle";
            picThrottle.Size = new Size(216, 137);
            picThrottle.SizeMode = PictureBoxSizeMode.StretchImage;
            picThrottle.TabIndex = 11;
            picThrottle.TabStop = false;
            // 
            // picAngle
            // 
            picAngle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            picAngle.Location = new Point(115, 191);
            picAngle.Name = "picAngle";
            picAngle.Size = new Size(216, 133);
            picAngle.SizeMode = PictureBoxSizeMode.StretchImage;
            picAngle.TabIndex = 12;
            picAngle.TabStop = false;
            picAngle.Paint += PicAngle_Paint;
            // 
            // panel3
            // 
            panel3.Controls.Add(lblRecordInfo);
            panel3.Controls.Add(lblThrottle);
            panel3.Controls.Add(picThrottle);
            panel3.Controls.Add(picAngle);
            panel3.Controls.Add(lblAngle);
            panel3.Dock = DockStyle.Left;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(334, 333);
            panel3.TabIndex = 4;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnStartStop);
            panel2.Controls.Add(lblRecordNumber);
            panel2.Controls.Add(btnFastNext);
            panel2.Controls.Add(btnFastPrev);
            panel2.Controls.Add(btnNext);
            panel2.Controls.Add(cmbSpeed);
            panel2.Controls.Add(btnPrev);
            panel2.Dock = DockStyle.Right;
            panel2.Location = new Point(655, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(292, 333);
            panel2.TabIndex = 2;
            // 
            // pnlCenter
            // 
            pnlCenter.Controls.Add(panel2);
            pnlCenter.Controls.Add(picTubImage);
            pnlCenter.Controls.Add(panel3);
            pnlCenter.Dock = DockStyle.Fill;
            pnlCenter.Location = new Point(0, 53);
            pnlCenter.Name = "pnlCenter";
            pnlCenter.Size = new Size(947, 333);
            pnlCenter.TabIndex = 5;
            // 
            // ucTubManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlCenter);
            Controls.Add(pnlTools);
            Controls.Add(pnlGraph);
            Controls.Add(panel1);
            Name = "ucTubManager";
            Size = new Size(947, 691);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkRecord).EndInit();
            pnlTools.ResumeLayout(false);
            pnlTools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picTubImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)picThrottle).EndInit();
            ((System.ComponentModel.ISupportInitialize)picAngle).EndInit();
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            pnlCenter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlWorkspace;
        private Panel panel1;
        private TextBox txtCarDirectory;
        private TextBox txtTub;
        private Button btnLoadTub;
        private Button btnLoadCarDirectory;
        private TrackBar trkRecord;
        private Panel pnlTools;
        private Button btnSetLeft;
        private Button btnSetFillter;
        private Button btnReroadTub;
        private Button btnRestore;
        private Button btnDelete;
        private Button btnSetRight;
        private Panel pnlGraph;
        private TextBox textBox3;
        private PictureBox picTubImage;
        private ComboBox cmbSpeed;
        private Button btnFastNext;
        private Button btnNext;
        private Button btnStartStop;
        private Button btnFastPrev;
        private Button btnPrev;
        private Label lblRecordNumber;
        private Label lblRecordInfo;
        private Label lblAngle;
        private Label lblThrottle;
        private PictureBox picThrottle;
        private PictureBox picAngle;
        private Panel panel3;
        private Panel panel2;
        private Panel pnlCenter;
    }
}
