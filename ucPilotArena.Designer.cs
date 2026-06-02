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
        // Color palette (match ucTrainer theme)
        private static readonly Color ClrBg = Color.FromArgb(245, 245, 245);
        private static readonly Color ClrCard = Color.White;
        private static readonly Color ClrBorder = Color.FromArgb(210, 210, 210);
        private static readonly Color ClrText = Color.FromArgb(30, 30, 30);
        private static readonly Color ClrMuted = Color.FromArgb(120, 120, 120);
        private static readonly Color ClrAccent = Color.FromArgb(24, 95, 165);
        // per-slot pilot/model comboboxes created dynamically; remove global top ones
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
        private System.Windows.Forms.FlowLayoutPanel flpPilotCards;
        private System.Windows.Forms.Panel pnlAugmentations;
        // record panels removed; per-slot info panels are added dynamically
        private System.Windows.Forms.Label lblLeftAngle;
        private System.Windows.Forms.Label lblLeftThrottle;
        private System.Windows.Forms.Label lblRightAngle;
        private System.Windows.Forms.Label lblRightThrottle;
        // replaced progress bars with numeric label displays
        private System.Windows.Forms.FlowLayoutPanel flpLeftAngleData;
        private System.Windows.Forms.FlowLayoutPanel flpLeftThrottleData;
        private System.Windows.Forms.Label lblLeftAvgError;
        private System.Windows.Forms.FlowLayoutPanel flpRightAngleData;
        private System.Windows.Forms.FlowLayoutPanel flpRightThrottleData;
        private System.Windows.Forms.Label lblRightAvgError;
        // ranking dropdowns
        private System.Windows.Forms.ComboBox cmbRankOverall;
        private System.Windows.Forms.ComboBox cmbRankAngle;
        private System.Windows.Forms.ComboBox cmbRankThrottle;
        // top controls: number of columns and tub plot
        private System.Windows.Forms.Label lblNumColumns;
        private System.Windows.Forms.ComboBox cmbNumColumns;
        private System.Windows.Forms.Button btnTubPlot;
        // playback and graph area
        private System.Windows.Forms.TableLayoutPanel tlpPlayback;
        private System.Windows.Forms.Panel pnlGraphArea;
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
            pnlTop = new Panel();
            btnRemoveLeftPic = new Button();
            btnAddLeftPic = new Button();
            cmbTop2 = new ComboBox();
            cmbTop1 = new ComboBox();
            lblNumColumns = new Label();
            cmbNumColumns = new ComboBox();
            btnTubPlot = new Button();
            cmbRankOverall = new ComboBox();
            cmbRankAngle = new ComboBox();
            cmbRankThrottle = new ComboBox();
            tlpMain = new TableLayoutPanel();
            pnlImageArea = new Panel();
            flpPilotCards = new FlowLayoutPanel();
            pnlLeftContainer = new Panel();
            flpLeftPics = new FlowLayoutPanel();
            picLeft = new PictureBox();
            picLeft2 = new PictureBox();
            picLeft3 = new PictureBox();
            picLeft4 = new PictureBox();
            pnlLeftButtons = new Panel();
            picRight = new PictureBox();
            trkTimeline = new TrackBar();
            pnlBrightBlur = new Panel();
            tlpBrightBlur = new TableLayoutPanel();
            pnlBrightness = new Panel();
            trkBrightness = new TrackBar();
            lblBrightnessValue = new Label();
            pnlBlur = new Panel();
            trkBlur = new TrackBar();
            lblBlurValue = new Label();
            tlpPlayback = new TableLayoutPanel();
            btnRewind = new Button();
            btnPrev = new Button();
            btnStop = new Button();
            btnNext = new Button();
            btnFastForward = new Button();
            pnlGraphArea = new Panel();
            pnlAugmentations = new Panel();
            tlpLeft = new TableLayoutPanel();
            flpLeftAngleData = new FlowLayoutPanel();
            lblLeftAI_Angle = new Label();
            lblLeftAngleError = new Label();
            flpLeftThrottleData = new FlowLayoutPanel();
            lblLeftAI_Throttle = new Label();
            lblLeftThrottleError = new Label();
            lblLeftThrottle = new Label();
            lblLeftAngle = new Label();
            lblLeftAvgError = new Label();
            tlpRight = new TableLayoutPanel();
            lblRightAngle = new Label();
            flpRightAngleData = new FlowLayoutPanel();
            lblRightAI_Angle = new Label();
            lblRightAngleError = new Label();
            lblRightThrottle = new Label();
            flpRightThrottleData = new FlowLayoutPanel();
            lblRightAI_Throttle = new Label();
            lblRightThrottleError = new Label();
            lblRightAvgError = new Label();
            lblRecordNumber = new Label();
            lblScaleValue = new Label();
            btnAddRemoveLeft = new Button();
            btnAddRemoveRight = new Button();
            pnlTop.SuspendLayout();
            tlpMain.SuspendLayout();
            pnlImageArea.SuspendLayout();
            pnlLeftContainer.SuspendLayout();
            flpLeftPics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLeft).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLeft2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLeft3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLeft4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picRight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trkTimeline).BeginInit();
            pnlBrightBlur.SuspendLayout();
            tlpBrightBlur.SuspendLayout();
            pnlBrightness.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkBrightness).BeginInit();
            pnlBlur.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkBlur).BeginInit();
            tlpPlayback.SuspendLayout();
            tlpLeft.SuspendLayout();
            flpLeftAngleData.SuspendLayout();
            flpLeftThrottleData.SuspendLayout();
            tlpRight.SuspendLayout();
            flpRightAngleData.SuspendLayout();
            flpRightThrottleData.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(244, 243, 238);
            pnlTop.Controls.Add(btnRemoveLeftPic);
            pnlTop.Controls.Add(btnAddLeftPic);
            pnlTop.Controls.Add(cmbTop2);
            pnlTop.Controls.Add(cmbTop1);
            pnlTop.Controls.Add(lblNumColumns);
            pnlTop.Controls.Add(cmbNumColumns);
            pnlTop.Controls.Add(btnTubPlot);
            pnlTop.Controls.Add(cmbRankOverall);
            pnlTop.Controls.Add(cmbRankAngle);
            pnlTop.Controls.Add(cmbRankThrottle);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(6);
            pnlTop.Size = new Size(1100, 59);
            pnlTop.TabIndex = 2;
            // 
            // btnRemoveLeftPic
            // 
            btnRemoveLeftPic.BackColor = Color.FromArgb(244, 243, 238);
            btnRemoveLeftPic.ForeColor = Color.Black;
            btnRemoveLeftPic.Location = new Point(126, 5);
            btnRemoveLeftPic.Name = "btnRemoveLeftPic";
            btnRemoveLeftPic.Size = new Size(100, 28);
            btnRemoveLeftPic.TabIndex = 1;
            btnRemoveLeftPic.Text = "- 파일럿 제거";
            btnRemoveLeftPic.UseVisualStyleBackColor = false;
            // 
            // btnAddLeftPic
            // 
            btnAddLeftPic.BackColor = Color.FromArgb(244, 243, 238);
            btnAddLeftPic.ForeColor = Color.Black;
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
            cmbTop2.Location = new Point(772, 34);
            cmbTop2.Name = "cmbTop2";
            cmbTop2.Size = new Size(180, 23);
            cmbTop2.TabIndex = 3;
            cmbTop2.Visible = false;
            // 
            // cmbTop1
            // 
            cmbTop1.BackColor = SystemColors.Window;
            cmbTop1.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTop1.Items.AddRange(new object[] { "Choose pilot" });
            cmbTop1.Location = new Point(772, 5);
            cmbTop1.Name = "cmbTop1";
            cmbTop1.Size = new Size(180, 23);
            cmbTop1.TabIndex = 2;
            cmbTop1.Visible = false;
            // 
            // lblNumColumns
            // 
            lblNumColumns.AutoSize = true;
            lblNumColumns.BackColor = Color.White;
            lblNumColumns.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblNumColumns.ForeColor = Color.Black;
            lblNumColumns.Location = new Point(248, 15);
            lblNumColumns.Name = "lblNumColumns";
            lblNumColumns.Size = new Size(26, 21);
            lblNumColumns.TabIndex = 4;
            lblNumColumns.Text = "열";
            // 
            // cmbNumColumns
            // 
            cmbNumColumns.BackColor = Color.FromArgb(244, 243, 238);
            cmbNumColumns.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNumColumns.Items.AddRange(new object[] { "1", "2", "3", "4" });
            cmbNumColumns.Location = new Point(280, 15);
            cmbNumColumns.Name = "cmbNumColumns";
            cmbNumColumns.Size = new Size(80, 23);
            cmbNumColumns.TabIndex = 5;
            // 
            // btnTubPlot
            // 
            btnTubPlot.BackColor = Color.FromArgb(244, 243, 238);
            btnTubPlot.FlatStyle = FlatStyle.System;
            btnTubPlot.ForeColor = Color.Black;
            btnTubPlot.Location = new Point(380, 15);
            btnTubPlot.Name = "btnTubPlot";
            btnTubPlot.Size = new Size(80, 26);
            btnTubPlot.TabIndex = 6;
            btnTubPlot.Text = "Tub plot";
            btnTubPlot.UseVisualStyleBackColor = false;
            // 
            // cmbRankOverall
            // 
            cmbRankOverall.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRankOverall.Items.AddRange(new object[] { "종합", "각도", "속도" });
            cmbRankOverall.Location = new Point(466, 18);
            cmbRankOverall.Name = "cmbRankOverall";
            cmbRankOverall.Size = new Size(80, 23);
            cmbRankOverall.TabIndex = 7;
            // 
            // cmbRankAngle
            // 
            cmbRankAngle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRankAngle.Items.AddRange(new object[] { "각도" });
            cmbRankAngle.Location = new Point(562, 18);
            cmbRankAngle.Name = "cmbRankAngle";
            cmbRankAngle.Size = new Size(80, 23);
            cmbRankAngle.TabIndex = 8;
            // 
            // cmbRankThrottle
            // 
            cmbRankThrottle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRankThrottle.Items.AddRange(new object[] { "속도" });
            cmbRankThrottle.Location = new Point(657, 18);
            cmbRankThrottle.Name = "cmbRankThrottle";
            cmbRankThrottle.Size = new Size(80, 23);
            cmbRankThrottle.TabIndex = 9;
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 2;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.6695F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.3305F));
            tlpMain.Controls.Add(pnlImageArea, 0, 1);
            tlpMain.Controls.Add(pnlLeftContainer, 0, 1);
            tlpMain.Controls.Add(picRight, 1, 1);
            tlpMain.Controls.Add(trkTimeline, 0, 3);
            tlpMain.Controls.Add(pnlBrightBlur, 0, 4);
            tlpMain.Controls.Add(pnlGraphArea, 0, 5);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 59);
            tlpMain.Name = "tlpMain";
            tlpMain.Padding = new Padding(6);
            tlpMain.RowCount = 6;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 6F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpMain.Size = new Size(1100, 941);
            tlpMain.TabIndex = 1;
            // 
            // pnlImageArea
            // 
            pnlImageArea.BackColor = Color.Black;
            tlpMain.SetColumnSpan(pnlImageArea, 2);
            pnlImageArea.Controls.Add(flpPilotCards);
            pnlImageArea.Dock = DockStyle.Fill;
            pnlImageArea.Location = new Point(9, 388);
            pnlImageArea.MinimumSize = new Size(600, 420);
            pnlImageArea.Name = "pnlImageArea";
            pnlImageArea.Size = new Size(1082, 420);
            pnlImageArea.TabIndex = 5;
            // 
            // flpPilotCards
            // 
            flpPilotCards.AutoScroll = true;
            flpPilotCards.Dock = DockStyle.Top;
            flpPilotCards.Location = new Point(0, 0);
            flpPilotCards.Name = "flpPilotCards";
            flpPilotCards.Size = new Size(1082, 220);
            flpPilotCards.TabIndex = 0;
            flpPilotCards.WrapContents = false;
            // 
            // pnlLeftContainer
            // 
            pnlLeftContainer.Controls.Add(flpLeftPics);
            pnlLeftContainer.Controls.Add(pnlLeftButtons);
            pnlLeftContainer.Dock = DockStyle.Fill;
            pnlLeftContainer.Location = new Point(9, 15);
            pnlLeftContainer.Name = "pnlLeftContainer";
            pnlLeftContainer.Size = new Size(534, 367);
            pnlLeftContainer.TabIndex = 0;
            pnlLeftContainer.Visible = false;
            // 
            // flpLeftPics
            // 
            flpLeftPics.AutoScroll = true;
            flpLeftPics.Controls.Add(picLeft);
            flpLeftPics.Controls.Add(picLeft2);
            flpLeftPics.Controls.Add(picLeft3);
            flpLeftPics.Controls.Add(picLeft4);
            flpLeftPics.Dock = DockStyle.Fill;
            flpLeftPics.FlowDirection = FlowDirection.TopDown;
            flpLeftPics.Location = new Point(0, 49);
            flpLeftPics.Name = "flpLeftPics";
            flpLeftPics.Size = new Size(534, 318);
            flpLeftPics.TabIndex = 1;
            flpLeftPics.WrapContents = false;
            // 
            // picLeft
            // 
            picLeft.BackColor = Color.Black;
            picLeft.BorderStyle = BorderStyle.FixedSingle;
            picLeft.Location = new Point(3, 3);
            picLeft.Name = "picLeft";
            picLeft.Size = new Size(800, 600);
            picLeft.SizeMode = PictureBoxSizeMode.StretchImage;
            picLeft.TabIndex = 10;
            picLeft.TabStop = false;
            // 
            // picLeft2
            // 
            picLeft2.BackColor = Color.Black;
            picLeft2.BorderStyle = BorderStyle.FixedSingle;
            picLeft2.Location = new Point(3, 609);
            picLeft2.Name = "picLeft2";
            picLeft2.Size = new Size(1000, 900);
            picLeft2.SizeMode = PictureBoxSizeMode.StretchImage;
            picLeft2.TabIndex = 11;
            picLeft2.TabStop = false;
            picLeft2.Visible = false;
            // 
            // picLeft3
            // 
            picLeft3.BackColor = Color.Black;
            picLeft3.BorderStyle = BorderStyle.FixedSingle;
            picLeft3.Location = new Point(3, 1515);
            picLeft3.Name = "picLeft3";
            picLeft3.Size = new Size(1000, 900);
            picLeft3.SizeMode = PictureBoxSizeMode.StretchImage;
            picLeft3.TabIndex = 12;
            picLeft3.TabStop = false;
            picLeft3.Visible = false;
            // 
            // picLeft4
            // 
            picLeft4.BackColor = Color.Black;
            picLeft4.BorderStyle = BorderStyle.FixedSingle;
            picLeft4.Location = new Point(3, 2421);
            picLeft4.Name = "picLeft4";
            picLeft4.Size = new Size(1000, 900);
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
            picRight.Location = new Point(9, 481);
            picRight.Name = "picRight";
            picRight.Size = new Size(534, 30);
            picRight.SizeMode = PictureBoxSizeMode.StretchImage;
            picRight.TabIndex = 1;
            picRight.TabStop = false;
            picRight.Visible = false;
            // 
            // trkTimeline
            // 
            tlpMain.SetColumnSpan(trkTimeline, 2);
            trkTimeline.Dock = DockStyle.Fill;
            trkTimeline.Location = new Point(9, 517);
            trkTimeline.Maximum = 1000;
            trkTimeline.Name = "trkTimeline";
            trkTimeline.Size = new Size(1082, 214);
            trkTimeline.TabIndex = 4;
            trkTimeline.Value = 200;
            // 
            // pnlBrightBlur
            // 
            tlpMain.SetColumnSpan(pnlBrightBlur, 2);
            pnlBrightBlur.Controls.Add(tlpBrightBlur);
            pnlBrightBlur.Controls.Add(tlpPlayback);
            pnlBrightBlur.Dock = DockStyle.Fill;
            pnlBrightBlur.Location = new Point(9, 737);
            pnlBrightBlur.Name = "pnlBrightBlur";
            pnlBrightBlur.Padding = new Padding(6);
            pnlBrightBlur.Size = new Size(1082, 174);
            pnlBrightBlur.TabIndex = 5;
            // 
            // tlpBrightBlur
            // 
            tlpBrightBlur.BackColor = Color.FromArgb(240, 240, 240);
            tlpBrightBlur.ColumnCount = 2;
            tlpBrightBlur.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBrightBlur.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBrightBlur.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpBrightBlur.Controls.Add(pnlBrightness, 0, 0);
            tlpBrightBlur.Controls.Add(pnlBlur, 1, 0);
            tlpBrightBlur.Dock = DockStyle.Top;
            tlpBrightBlur.Location = new Point(6, 6);
            tlpBrightBlur.Name = "tlpBrightBlur";
            tlpBrightBlur.RowCount = 1;
            tlpBrightBlur.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            tlpBrightBlur.Size = new Size(1070, 96);
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
            pnlBrightness.Size = new Size(529, 90);
            pnlBrightness.TabIndex = 0;
            // 
            // trkBrightness
            // 
            trkBrightness.Dock = DockStyle.Bottom;
            trkBrightness.Location = new Point(0, 43);
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
            pnlBlur.Size = new Size(529, 90);
            pnlBlur.TabIndex = 2;
            // 
            // trkBlur
            // 
            trkBlur.Dock = DockStyle.Bottom;
            trkBlur.Location = new Point(0, 45);
            trkBlur.Maximum = 100;
            trkBlur.Name = "trkBlur";
            trkBlur.Size = new Size(529, 45);
            trkBlur.TabIndex = 0;
            // 
            // lblBlurValue
            // 
            lblBlurValue.AutoSize = true;
            lblBlurValue.Dock = DockStyle.Top;
            lblBlurValue.Location = new Point(0, 0);
            lblBlurValue.Name = "lblBlurValue";
            lblBlurValue.Size = new Size(59, 15);
            lblBlurValue.TabIndex = 1;
            lblBlurValue.Text = "흐림 효과";
            // 
            // tlpPlayback
            // 
            tlpPlayback.ColumnCount = 5;
            tlpPlayback.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpPlayback.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpPlayback.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpPlayback.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpPlayback.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpPlayback.Controls.Add(btnRewind, 0, 0);
            tlpPlayback.Controls.Add(btnPrev, 1, 0);
            tlpPlayback.Controls.Add(btnStop, 2, 0);
            tlpPlayback.Controls.Add(btnNext, 3, 0);
            tlpPlayback.Controls.Add(btnFastForward, 4, 0);
            tlpPlayback.Dock = DockStyle.Bottom;
            tlpPlayback.Location = new Point(6, 126);
            tlpPlayback.Name = "tlpPlayback";
            tlpPlayback.Padding = new Padding(6);
            tlpPlayback.RowCount = 1;
            tlpPlayback.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpPlayback.Size = new Size(1070, 42);
            tlpPlayback.TabIndex = 1;
            // 
            // btnRewind
            // 
            btnRewind.BackColor = Color.FromArgb(64, 64, 64);
            btnRewind.Dock = DockStyle.Fill;
            btnRewind.ForeColor = Color.White;
            btnRewind.Location = new Point(9, 9);
            btnRewind.Name = "btnRewind";
            btnRewind.Size = new Size(205, 30);
            btnRewind.TabIndex = 0;
            btnRewind.Text = "<<";
            btnRewind.UseVisualStyleBackColor = false;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.FromArgb(64, 64, 64);
            btnPrev.Dock = DockStyle.Fill;
            btnPrev.ForeColor = Color.White;
            btnPrev.Location = new Point(220, 9);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(205, 30);
            btnPrev.TabIndex = 0;
            btnPrev.Text = "<";
            btnPrev.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.FromArgb(230, 242, 255);
            btnStop.Dock = DockStyle.Fill;
            btnStop.ForeColor = Color.Black;
            btnStop.Location = new Point(431, 9);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(205, 30);
            btnStop.TabIndex = 0;
            btnStop.Text = "재생/정지";
            btnStop.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.FromArgb(64, 64, 64);
            btnNext.Dock = DockStyle.Fill;
            btnNext.ForeColor = Color.White;
            btnNext.Location = new Point(642, 9);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(205, 30);
            btnNext.TabIndex = 0;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // btnFastForward
            // 
            btnFastForward.BackColor = Color.FromArgb(64, 64, 64);
            btnFastForward.Dock = DockStyle.Fill;
            btnFastForward.ForeColor = Color.White;
            btnFastForward.Location = new Point(853, 9);
            btnFastForward.Name = "btnFastForward";
            btnFastForward.Size = new Size(208, 30);
            btnFastForward.TabIndex = 0;
            btnFastForward.Text = ">>";
            btnFastForward.UseVisualStyleBackColor = false;
            // 
            // pnlGraphArea
            // 
            pnlGraphArea.BackColor = Color.FromArgb(50, 50, 50);
            pnlGraphArea.BorderStyle = BorderStyle.FixedSingle;
            tlpMain.SetColumnSpan(pnlGraphArea, 2);
            pnlGraphArea.Dock = DockStyle.Fill;
            pnlGraphArea.Location = new Point(9, 917);
            pnlGraphArea.Name = "pnlGraphArea";
            pnlGraphArea.Size = new Size(1082, 15);
            pnlGraphArea.TabIndex = 0;
            // 
            // pnlAugmentations
            // 
            pnlAugmentations.Location = new Point(0, 0);
            pnlAugmentations.Name = "pnlAugmentations";
            pnlAugmentations.Size = new Size(200, 100);
            pnlAugmentations.TabIndex = 0;
            // 
            // tlpLeft
            // 
            tlpLeft.ColumnCount = 2;
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 89F));
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLeft.Controls.Add(flpLeftAngleData, 1, 0);
            tlpLeft.Controls.Add(flpLeftThrottleData, 1, 1);
            tlpLeft.Controls.Add(lblLeftThrottle, 0, 1);
            tlpLeft.Controls.Add(lblLeftAngle, 0, 0);
            tlpLeft.Dock = DockStyle.Fill;
            tlpLeft.Location = new Point(6, 6);
            tlpLeft.Name = "tlpLeft";
            tlpLeft.RowCount = 2;
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpLeft.Size = new Size(528, 40);
            tlpLeft.TabIndex = 0;
            // 
            // flpLeftAngleData
            // 
            flpLeftAngleData.Controls.Add(lblLeftAI_Angle);
            flpLeftAngleData.Controls.Add(lblLeftAngleError);
            flpLeftAngleData.Dock = DockStyle.Fill;
            flpLeftAngleData.Location = new Point(92, 3);
            flpLeftAngleData.Name = "flpLeftAngleData";
            flpLeftAngleData.Size = new Size(433, 14);
            flpLeftAngleData.TabIndex = 0;
            flpLeftAngleData.WrapContents = false;
            // 
            // lblLeftAI_Angle
            // 
            lblLeftAI_Angle.AutoSize = true;
            lblLeftAI_Angle.Location = new Point(3, 0);
            lblLeftAI_Angle.Name = "lblLeftAI_Angle";
            lblLeftAI_Angle.Size = new Size(86, 15);
            lblLeftAI_Angle.TabIndex = 0;
            lblLeftAI_Angle.Text = "ai 각도 : 0.000";
            // 
            // lblLeftAngleError
            // 
            lblLeftAngleError.AutoSize = true;
            lblLeftAngleError.ForeColor = Color.LightGreen;
            lblLeftAngleError.Location = new Point(95, 0);
            lblLeftAngleError.Name = "lblLeftAngleError";
            lblLeftAngleError.Size = new Size(73, 15);
            lblLeftAngleError.TabIndex = 1;
            lblLeftAngleError.Text = "오차 : 0.000";
            // 
            // flpLeftThrottleData
            // 
            flpLeftThrottleData.Controls.Add(lblLeftAI_Throttle);
            flpLeftThrottleData.Controls.Add(lblLeftThrottleError);
            flpLeftThrottleData.Dock = DockStyle.Fill;
            flpLeftThrottleData.Location = new Point(92, 23);
            flpLeftThrottleData.Name = "flpLeftThrottleData";
            flpLeftThrottleData.Size = new Size(433, 14);
            flpLeftThrottleData.TabIndex = 1;
            flpLeftThrottleData.WrapContents = false;
            // 
            // lblLeftAI_Throttle
            // 
            lblLeftAI_Throttle.AutoSize = true;
            lblLeftAI_Throttle.Location = new Point(3, 0);
            lblLeftAI_Throttle.Name = "lblLeftAI_Throttle";
            lblLeftAI_Throttle.Size = new Size(86, 15);
            lblLeftAI_Throttle.TabIndex = 0;
            lblLeftAI_Throttle.Text = "ai 속도 : 0.000";
            // 
            // lblLeftThrottleError
            // 
            lblLeftThrottleError.AutoSize = true;
            lblLeftThrottleError.ForeColor = Color.LightGreen;
            lblLeftThrottleError.Location = new Point(95, 0);
            lblLeftThrottleError.Name = "lblLeftThrottleError";
            lblLeftThrottleError.Size = new Size(73, 15);
            lblLeftThrottleError.TabIndex = 1;
            lblLeftThrottleError.Text = "오차 : 0.000";
            // 
            // lblLeftThrottle
            // 
            lblLeftThrottle.AutoSize = true;
            lblLeftThrottle.Location = new Point(3, 20);
            lblLeftThrottle.Name = "lblLeftThrottle";
            lblLeftThrottle.Size = new Size(83, 15);
            lblLeftThrottle.TabIndex = 2;
            lblLeftThrottle.Text = "자율주행 속도";
            // 
            // lblLeftAngle
            // 
            lblLeftAngle.AutoSize = true;
            lblLeftAngle.Location = new Point(3, 0);
            lblLeftAngle.Name = "lblLeftAngle";
            lblLeftAngle.Size = new Size(83, 15);
            lblLeftAngle.TabIndex = 0;
            lblLeftAngle.Text = "자율주행 각도";
            // 
            // lblLeftAvgError
            // 
            lblLeftAvgError.AutoSize = true;
            lblLeftAvgError.Dock = DockStyle.Bottom;
            lblLeftAvgError.Location = new Point(6, 10);
            lblLeftAvgError.Name = "lblLeftAvgError";
            lblLeftAvgError.Size = new Size(109, 15);
            lblLeftAvgError.TabIndex = 1;
            lblLeftAvgError.Text = "평균 오차율 : 0.0%";
            // 
            // tlpRight
            // 
            tlpRight.ColumnCount = 2;
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpRight.Controls.Add(lblRightAngle, 0, 0);
            tlpRight.Controls.Add(flpRightAngleData, 1, 0);
            tlpRight.Controls.Add(lblRightThrottle, 0, 1);
            tlpRight.Controls.Add(flpRightThrottleData, 1, 1);
            tlpRight.Dock = DockStyle.Fill;
            tlpRight.Location = new Point(6, 6);
            tlpRight.Name = "tlpRight";
            tlpRight.RowCount = 2;
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpRight.Size = new Size(520, 10);
            tlpRight.TabIndex = 0;
            // 
            // lblRightAngle
            // 
            lblRightAngle.AutoSize = true;
            lblRightAngle.Location = new Point(3, 0);
            lblRightAngle.Name = "lblRightAngle";
            lblRightAngle.Size = new Size(71, 5);
            lblRightAngle.TabIndex = 0;
            lblRightAngle.Text = "사용자 각도";
            // 
            // flpRightAngleData
            // 
            flpRightAngleData.Controls.Add(lblRightAI_Angle);
            flpRightAngleData.Controls.Add(lblRightAngleError);
            flpRightAngleData.Dock = DockStyle.Fill;
            flpRightAngleData.Location = new Point(93, 3);
            flpRightAngleData.Name = "flpRightAngleData";
            flpRightAngleData.Size = new Size(424, 1);
            flpRightAngleData.TabIndex = 1;
            flpRightAngleData.WrapContents = false;
            // 
            // lblRightAI_Angle
            // 
            lblRightAI_Angle.AutoSize = true;
            lblRightAI_Angle.Location = new Point(3, 0);
            lblRightAI_Angle.Name = "lblRightAI_Angle";
            lblRightAI_Angle.Size = new Size(86, 15);
            lblRightAI_Angle.TabIndex = 0;
            lblRightAI_Angle.Text = "ai 각도 : 0.000";
            // 
            // lblRightAngleError
            // 
            lblRightAngleError.AutoSize = true;
            lblRightAngleError.ForeColor = Color.LightGreen;
            lblRightAngleError.Location = new Point(95, 0);
            lblRightAngleError.Name = "lblRightAngleError";
            lblRightAngleError.Size = new Size(73, 15);
            lblRightAngleError.TabIndex = 1;
            lblRightAngleError.Text = "오차 : 0.000";
            // 
            // lblRightThrottle
            // 
            lblRightThrottle.Location = new Point(3, 5);
            lblRightThrottle.Name = "lblRightThrottle";
            lblRightThrottle.Size = new Size(84, 5);
            lblRightThrottle.TabIndex = 2;
            // 
            // flpRightThrottleData
            // 
            flpRightThrottleData.Controls.Add(lblRightAI_Throttle);
            flpRightThrottleData.Controls.Add(lblRightThrottleError);
            flpRightThrottleData.Dock = DockStyle.Fill;
            flpRightThrottleData.Location = new Point(93, 8);
            flpRightThrottleData.Name = "flpRightThrottleData";
            flpRightThrottleData.Size = new Size(424, 1);
            flpRightThrottleData.TabIndex = 3;
            flpRightThrottleData.WrapContents = false;
            // 
            // lblRightAI_Throttle
            // 
            lblRightAI_Throttle.AutoSize = true;
            lblRightAI_Throttle.Location = new Point(3, 0);
            lblRightAI_Throttle.Name = "lblRightAI_Throttle";
            lblRightAI_Throttle.Size = new Size(86, 15);
            lblRightAI_Throttle.TabIndex = 0;
            lblRightAI_Throttle.Text = "ai 속도 : 0.000";
            // 
            // lblRightThrottleError
            // 
            lblRightThrottleError.AutoSize = true;
            lblRightThrottleError.ForeColor = Color.LightGreen;
            lblRightThrottleError.Location = new Point(95, 0);
            lblRightThrottleError.Name = "lblRightThrottleError";
            lblRightThrottleError.Size = new Size(73, 15);
            lblRightThrottleError.TabIndex = 1;
            lblRightThrottleError.Text = "오차 : 0.000";
            // 
            // lblRightAvgError
            // 
            lblRightAvgError.AutoSize = true;
            lblRightAvgError.Dock = DockStyle.Bottom;
            lblRightAvgError.Location = new Point(6, 16);
            lblRightAvgError.Name = "lblRightAvgError";
            lblRightAvgError.Size = new Size(109, 15);
            lblRightAvgError.TabIndex = 1;
            lblRightAvgError.Text = "평균 오차율 : 0.0%";
            // 
            // lblRecordNumber
            // 
            lblRecordNumber.Location = new Point(0, 0);
            lblRecordNumber.Name = "lblRecordNumber";
            lblRecordNumber.Size = new Size(100, 23);
            lblRecordNumber.TabIndex = 0;
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
            AutoScroll = true;
            AutoScrollMinSize = new Size(1100, 1000);
            BackColor = SystemColors.Control;
            Controls.Add(tlpMain);
            Controls.Add(pnlTop);
            Name = "ucPilotArena";
            Size = new Size(643, 654);
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            pnlImageArea.ResumeLayout(false);
            pnlLeftContainer.ResumeLayout(false);
            flpLeftPics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLeft).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLeft2).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLeft3).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLeft4).EndInit();
            ((System.ComponentModel.ISupportInitialize)picRight).EndInit();
            ((System.ComponentModel.ISupportInitialize)trkTimeline).EndInit();
            pnlBrightBlur.ResumeLayout(false);
            tlpBrightBlur.ResumeLayout(false);
            pnlBrightness.ResumeLayout(false);
            pnlBrightness.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkBrightness).EndInit();
            pnlBlur.ResumeLayout(false);
            pnlBlur.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkBlur).EndInit();
            tlpPlayback.ResumeLayout(false);
            tlpLeft.ResumeLayout(false);
            tlpLeft.PerformLayout();
            flpLeftAngleData.ResumeLayout(false);
            flpLeftAngleData.PerformLayout();
            flpLeftThrottleData.ResumeLayout(false);
            flpLeftThrottleData.PerformLayout();
            tlpRight.ResumeLayout(false);
            tlpRight.PerformLayout();
            flpRightAngleData.ResumeLayout(false);
            flpRightAngleData.PerformLayout();
            flpRightThrottleData.ResumeLayout(false);
            flpRightThrottleData.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblLeftAI_Angle;
        private Label lblLeftAngleError;
        private Label lblLeftAI_Throttle;
        private Label lblLeftThrottleError;
        private Label lblRightAI_Angle;
        private Label lblRightAngleError;
        private Label lblRightAI_Throttle;
        private Label lblRightThrottleError;
    }
}
