namespace DonkeyUi
{
    partial class ucPilotArena
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
        // Declare UI controls (Hungarian notation)
        private System.Windows.Forms.ComboBox cmbChoosePilot;
        private System.Windows.Forms.ComboBox cmbModelType;
        // additional top comboboxes (legacy names used in designer body)
        private System.Windows.Forms.ComboBox cmbTop2;
        private System.Windows.Forms.ComboBox cmbTop1;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        // left/right tablepanels used inside
        private System.Windows.Forms.TableLayoutPanel tlpLeft;
        private System.Windows.Forms.TableLayoutPanel tlpRight;
        private System.Windows.Forms.PictureBox picLeft;
        private System.Windows.Forms.PictureBox picLeft2;
        private System.Windows.Forms.PictureBox picLeft3;
        private System.Windows.Forms.PictureBox picLeft4;
        private System.Windows.Forms.PictureBox picRight;
        private System.Windows.Forms.Panel pnlImageArea;
        private System.Windows.Forms.Panel pnlLeftContainer;
        private System.Windows.Forms.Panel pnlLeftButtons;
        private System.Windows.Forms.Button btnAddLeftPic;
        private System.Windows.Forms.Button btnRemoveLeftPic;
        private System.Windows.Forms.FlowLayoutPanel flpLeftPics;
        private System.Windows.Forms.Panel pnlRecordLeft;
        private System.Windows.Forms.Panel pnlRecordRight;
        private System.Windows.Forms.Label lblLeftAngle;
        private System.Windows.Forms.Label lblLeftThrottle;
        private System.Windows.Forms.Label lblRightAngle;
        private System.Windows.Forms.Label lblRightThrottle;
        private System.Windows.Forms.ProgressBar prgLeftAngle;
        private System.Windows.Forms.ProgressBar prgLeftThrottle;
        private System.Windows.Forms.ProgressBar prgRightAngle;
        private System.Windows.Forms.ProgressBar prgRightThrottle;
        private System.Windows.Forms.TrackBar trkTimeline;
        private System.Windows.Forms.TrackBar trkBrightness;
        private System.Windows.Forms.TrackBar trkBlur;
        private System.Windows.Forms.Label lblBrightnessValue;
        private System.Windows.Forms.Label lblBlurValue;
        private System.Windows.Forms.Panel pnlBrightness;
        private System.Windows.Forms.Panel pnlBlur;
        // legacy names for brightness/blur controls used in some sections
        private System.Windows.Forms.TrackBar trackBarBrightness;
        private System.Windows.Forms.TrackBar trackBarBlur;
        private System.Windows.Forms.Label labelBrightnessValue;
        private System.Windows.Forms.Label labelBlurValue;
        // bright/blur container (legacy)
        private System.Windows.Forms.Panel pnlBrightBlur;
        private System.Windows.Forms.TableLayoutPanel tlpBrightBlur;
        private System.Windows.Forms.Label lblRecordNumber;
        private System.Windows.Forms.Label lblScaleValue;
        private System.Windows.Forms.Button btnAddRemoveLeft;
        private System.Windows.Forms.Button btnAddRemoveRight;
        private System.Windows.Forms.Button btnRewind;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFastForward;
        private System.Windows.Forms.Button btnStop;
        // legacy bottom controls names used in designer body
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label labelRecordNumber;
        private System.Windows.Forms.Label labelScaleValue;
        private System.Windows.Forms.Button buttonRewind;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonFastForward;
        private System.Windows.Forms.Button buttonAddRemoveLeft;
        private System.Windows.Forms.Button buttonAddRemoveRight;

        private void InitializeComponent()
        {
            cmbChoosePilot = new ComboBox();
            cmbModelType = new ComboBox();
            pnlTop = new Panel();
            btnRemoveLeftPic = new Button();
            btnAddLeftPic = new Button();
            cmbTop2 = new ComboBox();
            cmbTop1 = new ComboBox();
            tlpMain = new TableLayoutPanel();
            pnlImageArea = new Panel();
            pnlLeftContainer = new Panel();
            flpLeftPics = new FlowLayoutPanel();
            picLeft = new PictureBox();
            picLeft2 = new PictureBox();
            picLeft3 = new PictureBox();
            picLeft4 = new PictureBox();
            pnlLeftButtons = new Panel();
            picRight = new PictureBox();
            pnlRecordLeft = new Panel();
            tlpLeft = new TableLayoutPanel();
            prgLeftAngle = new ProgressBar();
            prgLeftThrottle = new ProgressBar();
            lblLeftThrottle = new Label();
            lblLeftAngle = new Label();
            pnlRecordRight = new Panel();
            tlpRight = new TableLayoutPanel();
            lblRightAngle = new Label();
            prgRightAngle = new ProgressBar();
            lblRightThrottle = new Label();
            prgRightThrottle = new ProgressBar();
            trkTimeline = new TrackBar();
            pnlBrightBlur = new Panel();
            tlpBrightBlur = new TableLayoutPanel();
            pnlBrightness = new Panel();
            trkBrightness = new TrackBar();
            lblBrightnessValue = new Label();
            pnlBlur = new Panel();
            trkBlur = new TrackBar();
            lblBlurValue = new Label();
            lblRecordNumber = new Label();
            btnRewind = new Button();
            btnPrev = new Button();
            btnStop = new Button();
            btnNext = new Button();
            btnFastForward = new Button();
            lblScaleValue = new Label();
            btnAddRemoveLeft = new Button();
            btnAddRemoveRight = new Button();
            pnlTop.SuspendLayout();
            tlpMain.SuspendLayout();
            pnlLeftContainer.SuspendLayout();
            flpLeftPics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLeft).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLeft2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLeft3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLeft4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picRight).BeginInit();
            pnlRecordLeft.SuspendLayout();
            tlpLeft.SuspendLayout();
            pnlRecordRight.SuspendLayout();
            tlpRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkTimeline).BeginInit();
            pnlBrightBlur.SuspendLayout();
            tlpBrightBlur.SuspendLayout();
            pnlBrightness.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkBrightness).BeginInit();
            pnlBlur.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkBlur).BeginInit();
            SuspendLayout();
            // 
            // cmbChoosePilot
            // 
            cmbChoosePilot.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbChoosePilot.Location = new Point(9, 36);
            cmbChoosePilot.Name = "cmbChoosePilot";
            cmbChoosePilot.Size = new Size(180, 23);
            cmbChoosePilot.TabIndex = 0;
            // 
            // cmbModelType
            // 
            cmbModelType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModelType.Items.AddRange(new object[] { "KerasLinear", "KerasCategorical", "Other" });
            cmbModelType.Location = new Point(218, 36);
            cmbModelType.Name = "cmbModelType";
            cmbModelType.Size = new Size(180, 23);
            cmbModelType.TabIndex = 1;
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(40, 40, 40);
            pnlTop.Controls.Add(btnRemoveLeftPic);
            pnlTop.Controls.Add(btnAddLeftPic);
            pnlTop.Controls.Add(cmbTop2);
            pnlTop.Controls.Add(cmbTop1);
            pnlTop.Controls.Add(cmbChoosePilot);
            pnlTop.Controls.Add(cmbModelType);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(6);
            pnlTop.Size = new Size(1100, 59);
            pnlTop.TabIndex = 2;
            // 
            // btnRemoveLeftPic
            // 
            btnRemoveLeftPic.BackColor = Color.FromArgb(64, 64, 64);
            btnRemoveLeftPic.ForeColor = Color.White;
            btnRemoveLeftPic.Location = new Point(126, 5);
            btnRemoveLeftPic.Name = "btnRemoveLeftPic";
            btnRemoveLeftPic.Size = new Size(100, 28);
            btnRemoveLeftPic.TabIndex = 1;
            btnRemoveLeftPic.Text = "- 파일럿 제거";
            btnRemoveLeftPic.UseVisualStyleBackColor = false;
            // 
            // btnAddLeftPic
            // 
            btnAddLeftPic.BackColor = Color.FromArgb(64, 64, 64);
            btnAddLeftPic.ForeColor = Color.White;
            btnAddLeftPic.Location = new Point(9, 5);
            btnAddLeftPic.Name = "btnAddLeftPic";
            btnAddLeftPic.Size = new Size(100, 28);
            btnAddLeftPic.TabIndex = 0;
            btnAddLeftPic.Text = "+ 파일럿 추가";
            btnAddLeftPic.UseVisualStyleBackColor = false;
            // 
            // cmbTop2
            // 
            cmbTop2.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTop2.Items.AddRange(new object[] { "KerasLinear", "KerasCategorical", "Other" });
            cmbTop2.Location = new Point(732, 33);
            cmbTop2.Name = "cmbTop2";
            cmbTop2.Size = new Size(180, 23);
            cmbTop2.TabIndex = 3;
            // 
            // cmbTop1
            // 
            cmbTop1.BackColor = SystemColors.Window;
            cmbTop1.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTop1.Items.AddRange(new object[] { "Choose pilot" });
            cmbTop1.Location = new Point(535, 33);
            cmbTop1.Name = "cmbTop1";
            cmbTop1.Size = new Size(180, 23);
            cmbTop1.TabIndex = 2;
            // 
            // tlpMain
            // 
            tlpMain.BackColor = Color.FromArgb(40, 40, 40);
            tlpMain.ColumnCount = 2;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.6695F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.3305F));
            tlpMain.Controls.Add(pnlImageArea, 0, 1);
            tlpMain.Controls.Add(pnlLeftContainer, 0, 1);
            tlpMain.Controls.Add(picRight, 1, 1);
            tlpMain.Controls.Add(pnlRecordLeft, 0, 2);
            tlpMain.Controls.Add(pnlRecordRight, 1, 2);
            tlpMain.Controls.Add(trkTimeline, 0, 3);
            tlpMain.Controls.Add(pnlBrightBlur, 0, 4);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 59);
            tlpMain.Name = "tlpMain";
            tlpMain.Padding = new Padding(6);
            tlpMain.RowCount = 5;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 6F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 8F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            tlpMain.Size = new Size(1100, 641);
            tlpMain.TabIndex = 1;
            // 
            // pnlImageArea
            // 
            pnlImageArea.BackColor = Color.Black;
            tlpMain.SetColumnSpan(pnlImageArea, 2);
            pnlImageArea.Dock = DockStyle.Fill;
            pnlImageArea.Location = new Point(9, 409);
            pnlImageArea.Name = "pnlImageArea";
            pnlImageArea.Size = new Size(1082, 78);
            pnlImageArea.TabIndex = 5;
            // 
            // pnlLeftContainer
            // 
            pnlLeftContainer.Controls.Add(flpLeftPics);
            pnlLeftContainer.Controls.Add(pnlLeftButtons);
            pnlLeftContainer.Dock = DockStyle.Fill;
            pnlLeftContainer.Location = new Point(9, 15);
            pnlLeftContainer.Name = "pnlLeftContainer";
            pnlLeftContainer.Size = new Size(534, 388);
            pnlLeftContainer.TabIndex = 0;
            pnlLeftContainer.Visible = false;
            // 
            // flpLeftPics
            // 
            flpLeftPics.Controls.Add(picLeft);
            flpLeftPics.Controls.Add(picLeft2);
            flpLeftPics.Controls.Add(picLeft3);
            flpLeftPics.Controls.Add(picLeft4);
            flpLeftPics.Dock = DockStyle.Fill;
            flpLeftPics.FlowDirection = FlowDirection.TopDown;
            flpLeftPics.Location = new Point(0, 49);
            flpLeftPics.Name = "flpLeftPics";
            flpLeftPics.Size = new Size(534, 339);
            flpLeftPics.TabIndex = 1;
            flpLeftPics.WrapContents = false;
            // 
            // picLeft
            // 
            picLeft.BackColor = Color.Black;
            picLeft.BorderStyle = BorderStyle.FixedSingle;
            picLeft.Location = new Point(3, 3);
            picLeft.Name = "picLeft";
            picLeft.Size = new Size(500, 200);
            picLeft.SizeMode = PictureBoxSizeMode.StretchImage;
            picLeft.TabIndex = 10;
            picLeft.TabStop = false;
            // 
            // picLeft2
            // 
            picLeft2.BackColor = Color.Black;
            picLeft2.BorderStyle = BorderStyle.FixedSingle;
            picLeft2.Location = new Point(3, 209);
            picLeft2.Name = "picLeft2";
            picLeft2.Size = new Size(500, 200);
            picLeft2.SizeMode = PictureBoxSizeMode.StretchImage;
            picLeft2.TabIndex = 11;
            picLeft2.TabStop = false;
            picLeft2.Visible = false;
            // 
            // picLeft3
            // 
            picLeft3.BackColor = Color.Black;
            picLeft3.BorderStyle = BorderStyle.FixedSingle;
            picLeft3.Location = new Point(3, 415);
            picLeft3.Name = "picLeft3";
            picLeft3.Size = new Size(500, 200);
            picLeft3.SizeMode = PictureBoxSizeMode.StretchImage;
            picLeft3.TabIndex = 12;
            picLeft3.TabStop = false;
            picLeft3.Visible = false;
            // 
            // picLeft4
            // 
            picLeft4.BackColor = Color.Black;
            picLeft4.BorderStyle = BorderStyle.FixedSingle;
            picLeft4.Location = new Point(3, 621);
            picLeft4.Name = "picLeft4";
            picLeft4.Size = new Size(500, 200);
            picLeft4.SizeMode = PictureBoxSizeMode.StretchImage;
            picLeft4.TabIndex = 13;
            picLeft4.TabStop = false;
            picLeft4.Visible = false;
            // 
            // pnlLeftButtons
            // 
            pnlLeftButtons.Dock = DockStyle.Top;
            pnlLeftButtons.Location = new Point(0, 0);
            pnlLeftButtons.Name = "pnlLeftButtons";
            pnlLeftButtons.Padding = new Padding(6);
            pnlLeftButtons.Size = new Size(534, 49);
            pnlLeftButtons.TabIndex = 2;
            // 
            // picRight
            // 
            picRight.BackColor = Color.Black;
            picRight.BorderStyle = BorderStyle.FixedSingle;
            picRight.Dock = DockStyle.Fill;
            picRight.Location = new Point(9, 493);
            picRight.Name = "picRight";
            picRight.Size = new Size(534, 33);
            picRight.SizeMode = PictureBoxSizeMode.StretchImage;
            picRight.TabIndex = 1;
            picRight.TabStop = false;
            picRight.Visible = false;
            // 
            // pnlRecordLeft
            // 
            pnlRecordLeft.BackColor = Color.FromArgb(64, 64, 64);
            pnlRecordLeft.BorderStyle = BorderStyle.FixedSingle;
            pnlRecordLeft.Controls.Add(tlpLeft);
            pnlRecordLeft.Dock = DockStyle.Fill;
            pnlRecordLeft.Location = new Point(549, 493);
            pnlRecordLeft.Name = "pnlRecordLeft";
            pnlRecordLeft.Padding = new Padding(6);
            pnlRecordLeft.Size = new Size(542, 33);
            pnlRecordLeft.TabIndex = 2;
            // 
            // tlpLeft
            // 
            tlpLeft.ColumnCount = 2;
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 89F));
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLeft.Controls.Add(prgLeftAngle, 1, 0);
            tlpLeft.Controls.Add(prgLeftThrottle, 1, 1);
            tlpLeft.Controls.Add(lblLeftThrottle, 0, 1);
            tlpLeft.Controls.Add(lblLeftAngle, 0, 0);
            tlpLeft.Dock = DockStyle.Fill;
            tlpLeft.Location = new Point(6, 6);
            tlpLeft.Name = "tlpLeft";
            tlpLeft.RowCount = 2;
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpLeft.Size = new Size(528, 19);
            tlpLeft.TabIndex = 0;
            // 
            // prgLeftAngle
            // 
            prgLeftAngle.Location = new Point(92, 3);
            prgLeftAngle.Name = "prgLeftAngle";
            prgLeftAngle.Size = new Size(411, 3);
            prgLeftAngle.Style = ProgressBarStyle.Continuous;
            prgLeftAngle.TabIndex = 1;
            // 
            // prgLeftThrottle
            // 
            prgLeftThrottle.Location = new Point(92, 12);
            prgLeftThrottle.Name = "prgLeftThrottle";
            prgLeftThrottle.Size = new Size(411, 3);
            prgLeftThrottle.Style = ProgressBarStyle.Continuous;
            prgLeftThrottle.TabIndex = 3;
            // 
            // lblLeftThrottle
            // 
            lblLeftThrottle.AutoSize = true;
            lblLeftThrottle.ForeColor = Color.White;
            lblLeftThrottle.Location = new Point(3, 9);
            lblLeftThrottle.Name = "lblLeftThrottle";
            lblLeftThrottle.Size = new Size(83, 10);
            lblLeftThrottle.TabIndex = 2;
            lblLeftThrottle.Text = "자율주행 속도";
            // 
            // lblLeftAngle
            // 
            lblLeftAngle.AutoSize = true;
            lblLeftAngle.ForeColor = Color.White;
            lblLeftAngle.Location = new Point(3, 0);
            lblLeftAngle.Name = "lblLeftAngle";
            lblLeftAngle.Size = new Size(83, 9);
            lblLeftAngle.TabIndex = 0;
            lblLeftAngle.Text = "자율주행 각도";
            // 
            // pnlRecordRight
            // 
            pnlRecordRight.BackColor = Color.FromArgb(64, 64, 64);
            pnlRecordRight.BorderStyle = BorderStyle.FixedSingle;
            pnlRecordRight.Controls.Add(tlpRight);
            pnlRecordRight.Dock = DockStyle.Fill;
            pnlRecordRight.Location = new Point(9, 532);
            pnlRecordRight.Name = "pnlRecordRight";
            pnlRecordRight.Padding = new Padding(6);
            pnlRecordRight.Size = new Size(534, 39);
            pnlRecordRight.TabIndex = 3;
            // 
            // tlpRight
            // 
            tlpRight.ColumnCount = 2;
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpRight.Controls.Add(lblRightAngle, 0, 0);
            tlpRight.Controls.Add(prgRightAngle, 1, 0);
            tlpRight.Controls.Add(lblRightThrottle, 0, 1);
            tlpRight.Controls.Add(prgRightThrottle, 1, 1);
            tlpRight.Dock = DockStyle.Fill;
            tlpRight.Location = new Point(6, 6);
            tlpRight.Name = "tlpRight";
            tlpRight.RowCount = 2;
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpRight.Size = new Size(520, 25);
            tlpRight.TabIndex = 0;
            // 
            // lblRightAngle
            // 
            lblRightAngle.AutoSize = true;
            lblRightAngle.ForeColor = Color.White;
            lblRightAngle.Location = new Point(3, 0);
            lblRightAngle.Name = "lblRightAngle";
            lblRightAngle.Size = new Size(71, 12);
            lblRightAngle.TabIndex = 0;
            lblRightAngle.Text = "사용자 각도";
            // 
            // prgRightAngle
            // 
            prgRightAngle.Location = new Point(93, 3);
            prgRightAngle.Name = "prgRightAngle";
            prgRightAngle.Size = new Size(410, 6);
            prgRightAngle.Style = ProgressBarStyle.Continuous;
            prgRightAngle.TabIndex = 1;
            // 
            // lblRightThrottle
            // 
            lblRightThrottle.AutoSize = true;
            lblRightThrottle.ForeColor = Color.White;
            lblRightThrottle.Location = new Point(3, 12);
            lblRightThrottle.Name = "lblRightThrottle";
            lblRightThrottle.Size = new Size(71, 13);
            lblRightThrottle.TabIndex = 2;
            lblRightThrottle.Text = "사용자 속도";
            // 
            // prgRightThrottle
            // 
            prgRightThrottle.Location = new Point(93, 15);
            prgRightThrottle.Name = "prgRightThrottle";
            prgRightThrottle.Size = new Size(410, 6);
            prgRightThrottle.Style = ProgressBarStyle.Continuous;
            prgRightThrottle.TabIndex = 3;
            // 
            // trkTimeline
            // 
            tlpMain.SetColumnSpan(trkTimeline, 2);
            trkTimeline.Dock = DockStyle.Fill;
            trkTimeline.Location = new Point(9, 577);
            trkTimeline.Name = "trkTimeline";
            trkTimeline.Size = new Size(1082, 14);
            trkTimeline.TabIndex = 4;
            // 
            // pnlBrightBlur
            // 
            tlpMain.SetColumnSpan(pnlBrightBlur, 2);
            pnlBrightBlur.Controls.Add(tlpBrightBlur);
            pnlBrightBlur.Dock = DockStyle.Fill;
            pnlBrightBlur.Location = new Point(9, 597);
            pnlBrightBlur.Name = "pnlBrightBlur";
            pnlBrightBlur.Padding = new Padding(6);
            pnlBrightBlur.Size = new Size(1082, 35);
            pnlBrightBlur.TabIndex = 5;
            // 
            // tlpBrightBlur
            // 
            tlpBrightBlur.BackColor = Color.FromArgb(64, 64, 64);
            tlpBrightBlur.ColumnCount = 2;
            tlpBrightBlur.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBrightBlur.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBrightBlur.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpBrightBlur.Controls.Add(pnlBrightness, 0, 0);
            tlpBrightBlur.Controls.Add(pnlBlur, 1, 0);
            tlpBrightBlur.Dock = DockStyle.Fill;
            tlpBrightBlur.Location = new Point(6, 6);
            tlpBrightBlur.Name = "tlpBrightBlur";
            tlpBrightBlur.RowCount = 1;
            tlpBrightBlur.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            tlpBrightBlur.Size = new Size(1070, 23);
            tlpBrightBlur.TabIndex = 0;
            // 
            // pnlBrightness
            // 
            pnlBrightness.BorderStyle = BorderStyle.FixedSingle;
            pnlBrightness.Controls.Add(trkBrightness);
            pnlBrightness.Controls.Add(lblBrightnessValue);
            pnlBrightness.Dock = DockStyle.Fill;
            pnlBrightness.Location = new Point(3, 3);
            pnlBrightness.Name = "pnlBrightness";
            pnlBrightness.Size = new Size(529, 74);
            pnlBrightness.TabIndex = 0;
            // 
            // trkBrightness
            // 
            trkBrightness.Dock = DockStyle.Bottom;
            trkBrightness.Location = new Point(0, 27);
            trkBrightness.Maximum = 100;
            trkBrightness.Minimum = -100;
            trkBrightness.Name = "trkBrightness";
            trkBrightness.Size = new Size(527, 45);
            trkBrightness.TabIndex = 0;
            // 
            // lblBrightnessValue
            // 
            lblBrightnessValue.AutoSize = true;
            lblBrightnessValue.Dock = DockStyle.Top;
            lblBrightnessValue.ForeColor = Color.White;
            lblBrightnessValue.Location = new Point(0, 0);
            lblBrightnessValue.Name = "lblBrightnessValue";
            lblBrightnessValue.Size = new Size(31, 15);
            lblBrightnessValue.TabIndex = 1;
            lblBrightnessValue.Text = "밝기";
            // 
            // pnlBlur
            // 
            pnlBlur.Controls.Add(trkBlur);
            pnlBlur.Controls.Add(lblBlurValue);
            pnlBlur.Dock = DockStyle.Fill;
            pnlBlur.Location = new Point(538, 3);
            pnlBlur.Name = "pnlBlur";
            pnlBlur.Size = new Size(529, 74);
            pnlBlur.TabIndex = 2;
            // 
            // trkBlur
            // 
            trkBlur.Dock = DockStyle.Bottom;
            trkBlur.Location = new Point(0, 29);
            trkBlur.Maximum = 100;
            trkBlur.Name = "trkBlur";
            trkBlur.Size = new Size(529, 45);
            trkBlur.TabIndex = 0;
            // 
            // lblBlurValue
            // 
            lblBlurValue.AutoSize = true;
            lblBlurValue.Dock = DockStyle.Top;
            lblBlurValue.ForeColor = Color.White;
            lblBlurValue.Location = new Point(0, 0);
            lblBlurValue.Name = "lblBlurValue";
            lblBlurValue.Size = new Size(59, 15);
            lblBlurValue.TabIndex = 1;
            lblBlurValue.Text = "흐림 효과";
            // 
            // lblRecordNumber
            // 
            lblRecordNumber.Location = new Point(0, 0);
            lblRecordNumber.Name = "lblRecordNumber";
            lblRecordNumber.Size = new Size(100, 23);
            lblRecordNumber.TabIndex = 0;
            // 
            // btnRewind
            // 
            btnRewind.Location = new Point(0, 0);
            btnRewind.Name = "btnRewind";
            btnRewind.Size = new Size(75, 23);
            btnRewind.TabIndex = 0;
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(0, 0);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(75, 23);
            btnPrev.TabIndex = 0;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(0, 0);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 0;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(0, 0);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(75, 23);
            btnNext.TabIndex = 0;
            // 
            // btnFastForward
            // 
            btnFastForward.Location = new Point(0, 0);
            btnFastForward.Name = "btnFastForward";
            btnFastForward.Size = new Size(75, 23);
            btnFastForward.TabIndex = 0;
            // 
            // lblScaleValue
            // 
            lblScaleValue.Location = new Point(0, 0);
            lblScaleValue.Name = "lblScaleValue";
            lblScaleValue.Size = new Size(100, 23);
            lblScaleValue.TabIndex = 0;
            // 
            // btnAddRemoveLeft
            // 
            btnAddRemoveLeft.Location = new Point(0, 0);
            btnAddRemoveLeft.Name = "btnAddRemoveLeft";
            btnAddRemoveLeft.Size = new Size(75, 23);
            btnAddRemoveLeft.TabIndex = 0;
            // 
            // btnAddRemoveRight
            // 
            btnAddRemoveRight.Location = new Point(0, 0);
            btnAddRemoveRight.Name = "btnAddRemoveRight";
            btnAddRemoveRight.Size = new Size(75, 23);
            btnAddRemoveRight.TabIndex = 0;
            // 
            // ucPilotArena
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            Controls.Add(tlpMain);
            Controls.Add(pnlTop);
            Name = "ucPilotArena";
            Size = new Size(1100, 700);
            pnlTop.ResumeLayout(false);
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            pnlLeftContainer.ResumeLayout(false);
            flpLeftPics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLeft).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLeft2).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLeft3).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLeft4).EndInit();
            ((System.ComponentModel.ISupportInitialize)picRight).EndInit();
            pnlRecordLeft.ResumeLayout(false);
            tlpLeft.ResumeLayout(false);
            tlpLeft.PerformLayout();
            pnlRecordRight.ResumeLayout(false);
            tlpRight.ResumeLayout(false);
            tlpRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkTimeline).EndInit();
            pnlBrightBlur.ResumeLayout(false);
            tlpBrightBlur.ResumeLayout(false);
            pnlBrightness.ResumeLayout(false);
            pnlBrightness.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkBrightness).EndInit();
            pnlBlur.ResumeLayout(false);
            pnlBlur.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkBlur).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
