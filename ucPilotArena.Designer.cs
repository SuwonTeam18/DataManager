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
        private System.Windows.Forms.PictureBox picRight;
        private System.Windows.Forms.Panel pnlRecordLeft;
        private System.Windows.Forms.Panel pnlRecordRight;
        // right value labels (designer-created names)
        private System.Windows.Forms.Label lblRightAngleValue;
        private System.Windows.Forms.Label lblRightThrottleValue;
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
            cmbTop2 = new ComboBox();
            cmbTop1 = new ComboBox();
            tlpMain = new TableLayoutPanel();
            picLeft = new PictureBox();
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
            lblRightAngleValue = new Label();
            lblRightThrottleValue = new Label();
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
            ((System.ComponentModel.ISupportInitialize)picLeft).BeginInit();
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
            cmbChoosePilot.Items.AddRange(new object[] { "Choose pilot" });
            cmbChoosePilot.Location = new Point(6, 6);
            cmbChoosePilot.Name = "cmbChoosePilot";
            cmbChoosePilot.Size = new Size(180, 23);
            cmbChoosePilot.TabIndex = 0;
            // 
            // cmbModelType
            // 
            cmbModelType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModelType.Items.AddRange(new object[] { "Model type" });
            cmbModelType.Location = new Point(200, 6);
            cmbModelType.Name = "cmbModelType";
            cmbModelType.Size = new Size(180, 23);
            cmbModelType.TabIndex = 1;
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(64, 64, 64);
            pnlTop.Controls.Add(cmbTop2);
            pnlTop.Controls.Add(cmbTop1);
            pnlTop.Controls.Add(cmbChoosePilot);
            pnlTop.Controls.Add(cmbModelType);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(6);
            pnlTop.Size = new Size(1071, 36);
            pnlTop.TabIndex = 2;
            // 
            // cmbTop2
            // 
            cmbTop2.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTop2.Items.AddRange(new object[] { "Model type" });
            cmbTop2.Location = new Point(734, 7);
            cmbTop2.Name = "cmbTop2";
            cmbTop2.Size = new Size(180, 23);
            cmbTop2.TabIndex = 3;
            // 
            // cmbTop1
            // 
            cmbTop1.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTop1.Items.AddRange(new object[] { "Choose pilot" });
            cmbTop1.Location = new Point(538, 7);
            cmbTop1.Name = "cmbTop1";
            cmbTop1.Size = new Size(180, 23);
            cmbTop1.TabIndex = 2;
            // 
            // tlpMain
            // 
            tlpMain.BackColor = Color.FromArgb(64, 64, 64);
            tlpMain.ColumnCount = 2;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.6695F));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.3305F));
            tlpMain.Controls.Add(picLeft, 0, 1);
            tlpMain.Controls.Add(picRight, 1, 1);
            tlpMain.Controls.Add(pnlRecordLeft, 0, 2);
            tlpMain.Controls.Add(pnlRecordRight, 1, 2);
            tlpMain.Controls.Add(trkTimeline, 0, 3);
            tlpMain.Controls.Add(pnlBrightBlur, 0, 4);
            tlpMain.Dock = DockStyle.Top;
            tlpMain.Location = new Point(0, 36);
            tlpMain.Name = "tlpMain";
            tlpMain.Padding = new Padding(6);
            tlpMain.RowCount = 5;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 6F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 262F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 99F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpMain.Size = new Size(1071, 518);
            tlpMain.TabIndex = 1;
            // 
            // picLeft
            // 
            picLeft.BackColor = Color.Black;
            picLeft.BorderStyle = BorderStyle.FixedSingle;
            picLeft.Dock = DockStyle.Fill;
            picLeft.Location = new Point(9, 15);
            picLeft.Name = "picLeft";
            picLeft.Size = new Size(520, 256);
            picLeft.SizeMode = PictureBoxSizeMode.StretchImage;
            picLeft.TabIndex = 0;
            picLeft.TabStop = false;
            // 
            // picRight
            // 
            picRight.BackColor = Color.Black;
            picRight.BorderStyle = BorderStyle.FixedSingle;
            picRight.Dock = DockStyle.Fill;
            picRight.Location = new Point(535, 15);
            picRight.Name = "picRight";
            picRight.Size = new Size(527, 256);
            picRight.SizeMode = PictureBoxSizeMode.StretchImage;
            picRight.TabIndex = 1;
            picRight.TabStop = false;
            // 
            // pnlRecordLeft
            // 
            pnlRecordLeft.Controls.Add(tlpLeft);
            pnlRecordLeft.Dock = DockStyle.Fill;
            pnlRecordLeft.Location = new Point(9, 277);
            pnlRecordLeft.Name = "pnlRecordLeft";
            pnlRecordLeft.Padding = new Padding(6);
            pnlRecordLeft.Size = new Size(520, 93);
            pnlRecordLeft.TabIndex = 2;
            // 
            // tlpLeft
            // 
            tlpLeft.ColumnCount = 3;
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 89F));
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
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
            tlpLeft.Size = new Size(508, 81);
            tlpLeft.TabIndex = 0;
            // 
            // prgLeftAngle
            // 
            prgLeftAngle.Location = new Point(92, 3);
            prgLeftAngle.Name = "prgLeftAngle";
            prgLeftAngle.Size = new Size(100, 23);
            prgLeftAngle.Style = ProgressBarStyle.Continuous;
            prgLeftAngle.TabIndex = 1;
            // 
            // prgLeftThrottle
            // 
            prgLeftThrottle.Location = new Point(92, 43);
            prgLeftThrottle.Name = "prgLeftThrottle";
            prgLeftThrottle.Size = new Size(100, 23);
            prgLeftThrottle.Style = ProgressBarStyle.Continuous;
            prgLeftThrottle.TabIndex = 3;
            // 
            // lblLeftThrottle
            // 
            lblLeftThrottle.AutoSize = true;
            lblLeftThrottle.ForeColor = Color.White;
            lblLeftThrottle.Location = new Point(3, 40);
            lblLeftThrottle.Name = "lblLeftThrottle";
            lblLeftThrottle.Size = new Size(75, 15);
            lblLeftThrottle.TabIndex = 2;
            lblLeftThrottle.Text = "pilot/throttle";
            // 
            // lblLeftAngle
            // 
            lblLeftAngle.AutoSize = true;
            lblLeftAngle.ForeColor = Color.White;
            lblLeftAngle.Location = new Point(3, 0);
            lblLeftAngle.Name = "lblLeftAngle";
            lblLeftAngle.Size = new Size(65, 15);
            lblLeftAngle.TabIndex = 0;
            lblLeftAngle.Text = "pilot/angle";
            // 
            // pnlRecordRight
            // 
            pnlRecordRight.Controls.Add(tlpRight);
            pnlRecordRight.Dock = DockStyle.Fill;
            pnlRecordRight.Location = new Point(535, 277);
            pnlRecordRight.Name = "pnlRecordRight";
            pnlRecordRight.Padding = new Padding(6);
            pnlRecordRight.Size = new Size(527, 93);
            pnlRecordRight.TabIndex = 3;
            // 
            // tlpRight
            // 
            tlpRight.ColumnCount = 3;
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tlpRight.Controls.Add(lblRightAngle, 0, 0);
            tlpRight.Controls.Add(prgRightAngle, 1, 0);
            tlpRight.Controls.Add(lblRightThrottle, 0, 1);
            tlpRight.Controls.Add(prgRightThrottle, 1, 1);
            tlpRight.Controls.Add(lblRightAngleValue, 2, 0);
            tlpRight.Controls.Add(lblRightThrottleValue, 2, 1);
            tlpRight.Dock = DockStyle.Fill;
            tlpRight.Location = new Point(6, 6);
            tlpRight.Name = "tlpRight";
            tlpRight.RowCount = 2;
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpRight.Size = new Size(515, 81);
            tlpRight.TabIndex = 0;
            // 
            // lblRightAngle
            // 
            lblRightAngle.AutoSize = true;
            lblRightAngle.ForeColor = Color.White;
            lblRightAngle.Location = new Point(3, 0);
            lblRightAngle.Name = "lblRightAngle";
            lblRightAngle.Size = new Size(63, 15);
            lblRightAngle.TabIndex = 0;
            lblRightAngle.Text = "user/angle";
            // 
            // prgRightAngle
            // 
            prgRightAngle.Location = new Point(93, 3);
            prgRightAngle.Name = "prgRightAngle";
            prgRightAngle.Size = new Size(100, 23);
            prgRightAngle.Style = ProgressBarStyle.Continuous;
            prgRightAngle.TabIndex = 1;
            // 
            // lblRightThrottle
            // 
            lblRightThrottle.AutoSize = true;
            lblRightThrottle.ForeColor = Color.White;
            lblRightThrottle.Location = new Point(3, 40);
            lblRightThrottle.Name = "lblRightThrottle";
            lblRightThrottle.Size = new Size(73, 15);
            lblRightThrottle.TabIndex = 2;
            lblRightThrottle.Text = "user/throttle";
            // 
            // prgRightThrottle
            // 
            prgRightThrottle.Location = new Point(93, 43);
            prgRightThrottle.Name = "prgRightThrottle";
            prgRightThrottle.Size = new Size(100, 23);
            prgRightThrottle.Style = ProgressBarStyle.Continuous;
            prgRightThrottle.TabIndex = 3;
            // 
            // lblRightAngleValue
            // 
            lblRightAngleValue.Dock = DockStyle.Fill;
            lblRightAngleValue.Location = new Point(438, 0);
            lblRightAngleValue.Name = "lblRightAngleValue";
            lblRightAngleValue.Size = new Size(74, 40);
            lblRightAngleValue.TabIndex = 4;
            lblRightAngleValue.Text = "+00.000";
            lblRightAngleValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblRightThrottleValue
            // 
            lblRightThrottleValue.Dock = DockStyle.Fill;
            lblRightThrottleValue.Location = new Point(438, 40);
            lblRightThrottleValue.Name = "lblRightThrottleValue";
            lblRightThrottleValue.Size = new Size(74, 41);
            lblRightThrottleValue.TabIndex = 5;
            lblRightThrottleValue.Text = "+00.598";
            lblRightThrottleValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // trkTimeline
            // 
            tlpMain.SetColumnSpan(trkTimeline, 2);
            trkTimeline.Dock = DockStyle.Fill;
            trkTimeline.Location = new Point(9, 376);
            trkTimeline.Maximum = 1000;
            trkTimeline.Name = "trkTimeline";
            trkTimeline.Size = new Size(1053, 40);
            trkTimeline.TabIndex = 4;
            trkTimeline.Value = 200;
            // 
            // pnlBrightBlur
            // 
            tlpMain.SetColumnSpan(pnlBrightBlur, 2);
            pnlBrightBlur.Controls.Add(tlpBrightBlur);
            pnlBrightBlur.Dock = DockStyle.Fill;
            pnlBrightBlur.Location = new Point(9, 422);
            pnlBrightBlur.Name = "pnlBrightBlur";
            pnlBrightBlur.Padding = new Padding(6);
            pnlBrightBlur.Size = new Size(1053, 87);
            pnlBrightBlur.TabIndex = 5;
            // 
            // tlpBrightBlur
            // 
            tlpBrightBlur.ColumnCount = 4;
            tlpBrightBlur.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBrightBlur.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tlpBrightBlur.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBrightBlur.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tlpBrightBlur.Controls.Add(pnlBrightness, 0, 0);
            tlpBrightBlur.Controls.Add(pnlBlur, 2, 0);
            tlpBrightBlur.Dock = DockStyle.Fill;
            tlpBrightBlur.Location = new Point(6, 6);
            tlpBrightBlur.Name = "tlpBrightBlur";
            tlpBrightBlur.RowCount = 1;
            tlpBrightBlur.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpBrightBlur.Size = new Size(1041, 75);
            tlpBrightBlur.TabIndex = 0;
            // 
            // pnlBrightness
            // 
            pnlBrightness.Controls.Add(trkBrightness);
            pnlBrightness.Controls.Add(lblBrightnessValue);
            pnlBrightness.Dock = DockStyle.Fill;
            pnlBrightness.Location = new Point(3, 3);
            pnlBrightness.Name = "pnlBrightness";
            pnlBrightness.Size = new Size(394, 69);
            pnlBrightness.TabIndex = 0;
            // 
            // trkBrightness
            // 
            trkBrightness.Dock = DockStyle.Bottom;
            trkBrightness.Location = new Point(0, 24);
            trkBrightness.Maximum = 200;
            trkBrightness.Name = "trkBrightness";
            trkBrightness.Size = new Size(394, 45);
            trkBrightness.TabIndex = 0;
            trkBrightness.Value = 100;
            // 
            // lblBrightnessValue
            // 
            lblBrightnessValue.AutoSize = true;
            lblBrightnessValue.Dock = DockStyle.Top;
            lblBrightnessValue.ForeColor = Color.White;
            lblBrightnessValue.Location = new Point(0, 0);
            lblBrightnessValue.Name = "lblBrightnessValue";
            lblBrightnessValue.Size = new Size(90, 15);
            lblBrightnessValue.TabIndex = 1;
            lblBrightnessValue.Text = "Brightness 1.00";
            // 
            // pnlBlur
            // 
            pnlBlur.Controls.Add(trkBlur);
            pnlBlur.Controls.Add(lblBlurValue);
            pnlBlur.Dock = DockStyle.Fill;
            pnlBlur.Location = new Point(523, 3);
            pnlBlur.Name = "pnlBlur";
            pnlBlur.Size = new Size(394, 69);
            pnlBlur.TabIndex = 2;
            // 
            // trkBlur
            // 
            trkBlur.Dock = DockStyle.Bottom;
            trkBlur.Location = new Point(0, 24);
            trkBlur.Maximum = 100;
            trkBlur.Name = "trkBlur";
            trkBlur.Size = new Size(394, 45);
            trkBlur.TabIndex = 0;
            // 
            // lblBlurValue
            // 
            lblBlurValue.AutoSize = true;
            lblBlurValue.Dock = DockStyle.Top;
            lblBlurValue.ForeColor = Color.White;
            lblBlurValue.Location = new Point(0, 0);
            lblBlurValue.Name = "lblBlurValue";
            lblBlurValue.Size = new Size(56, 15);
            lblBlurValue.TabIndex = 1;
            lblBlurValue.Text = "Blur 0.00";
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
            BackColor = Color.Black;
            Controls.Add(tlpMain);
            Controls.Add(pnlTop);
            Name = "ucPilotArena";
            Size = new Size(1071, 700);
            pnlTop.ResumeLayout(false);
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLeft).EndInit();
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
