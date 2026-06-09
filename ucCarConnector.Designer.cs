namespace DonkeyUi
{
    partial class ucCarConnector
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
        // UI fields (Hungarian notation)
        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblConnectionValue;
        private System.Windows.Forms.TextBox txtCarDirectory;
        private System.Windows.Forms.ComboBox cmbSelectTub;
        private System.Windows.Forms.Button btnCreateNewFolder;
        private System.Windows.Forms.Button btnPullTubData;
        private System.Windows.Forms.ProgressBar prgPullStatus;
        private System.Windows.Forms.Button btnPushPilots;
        private System.Windows.Forms.ProgressBar prgPushStatus;
        private System.Windows.Forms.Button btnSyncH5;
        private System.Windows.Forms.Button btnSyncSavedModel;
        private System.Windows.Forms.Button btnSyncTFlite;
        private System.Windows.Forms.Panel pnlDriveArea;
        private System.Windows.Forms.Label lblDriveModel;
        private System.Windows.Forms.Label lblDrivePilot;
        private System.Windows.Forms.Button btnDrive;
        private System.Windows.Forms.Button btnStop;

        private void InitializeComponent()
        {
            tlpRoot = new TableLayoutPanel();
            lblConnectionStatus = new Label();
            lblConnectionValue = new Label();
            txtCarDirectory = new TextBox();
            cmbSelectTub = new ComboBox();
            btnCreateNewFolder = new Button();
            btnPullTubData = new Button();
            prgPullStatus = new ProgressBar();
            btnSyncH5 = new Button();
            btnSyncSavedModel = new Button();
            btnSyncTFlite = new Button();
            pnlDriveArea = new Panel();
            tlpDrive = new TableLayoutPanel();
            lblDriveModel = new Label();
            lblDrivePilot = new Label();
            btnDrive = new Button();
            btnStop = new Button();
            prgPushStatus = new ProgressBar();
            btnPushPilots = new Button();
            tlpRoot.SuspendLayout();
            pnlDriveArea.SuspendLayout();
            tlpDrive.SuspendLayout();
            SuspendLayout();
            // 
            // tlpRoot
            // 
            tlpRoot.BackColor = Color.FromArgb(64, 64, 64);
            tlpRoot.ColumnCount = 3;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 213F));
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.653595F));
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.346405F));
            tlpRoot.Controls.Add(lblConnectionStatus, 0, 1);
            tlpRoot.Controls.Add(lblConnectionValue, 2, 1);
            tlpRoot.Controls.Add(txtCarDirectory, 0, 2);
            tlpRoot.Controls.Add(cmbSelectTub, 2, 2);
            tlpRoot.Controls.Add(btnCreateNewFolder, 0, 3);
            tlpRoot.Controls.Add(btnPullTubData, 1, 3);
            tlpRoot.Controls.Add(prgPullStatus, 2, 3);
            tlpRoot.Controls.Add(btnSyncH5, 0, 5);
            tlpRoot.Controls.Add(btnSyncSavedModel, 1, 5);
            tlpRoot.Controls.Add(btnSyncTFlite, 2, 5);
            tlpRoot.Controls.Add(pnlDriveArea, 0, 6);
            tlpRoot.Controls.Add(prgPushStatus, 2, 4);
            tlpRoot.Controls.Add(btnPushPilots, 1, 4);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Location = new Point(0, 0);
            tlpRoot.Name = "tlpRoot";
            tlpRoot.Padding = new Padding(12);
            tlpRoot.RowCount = 7;
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 63F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.Size = new Size(905, 691);
            tlpRoot.TabIndex = 0;
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.Anchor = AnchorStyles.Left;
            lblConnectionStatus.AutoSize = true;
            lblConnectionStatus.ForeColor = Color.White;
            lblConnectionStatus.Location = new Point(15, 48);
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(104, 15);
            lblConnectionStatus.TabIndex = 0;
            lblConnectionStatus.Text = "Connection status";
            // 
            // lblConnectionValue
            // 
            lblConnectionValue.Anchor = AnchorStyles.Right;
            lblConnectionValue.AutoSize = true;
            lblConnectionValue.ForeColor = Color.LimeGreen;
            lblConnectionValue.Location = new Point(827, 48);
            lblConnectionValue.Name = "lblConnectionValue";
            lblConnectionValue.Size = new Size(63, 15);
            lblConnectionValue.TabIndex = 1;
            lblConnectionValue.Text = "connected";
            // 
            // txtCarDirectory
            // 
            tlpRoot.SetColumnSpan(txtCarDirectory, 2);
            txtCarDirectory.Dock = DockStyle.Fill;
            txtCarDirectory.Location = new Point(15, 79);
            txtCarDirectory.Name = "txtCarDirectory";
            txtCarDirectory.Size = new Size(545, 23);
            txtCarDirectory.TabIndex = 2;
            // 
            // cmbSelectTub
            // 
            cmbSelectTub.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSelectTub.Items.AddRange(new object[] { "data" });
            cmbSelectTub.Location = new Point(566, 79);
            cmbSelectTub.Name = "cmbSelectTub";
            cmbSelectTub.Size = new Size(160, 23);
            cmbSelectTub.TabIndex = 3;
            // 
            // btnCreateNewFolder
            // 
            btnCreateNewFolder.BackColor = Color.FromArgb(80, 80, 80);
            btnCreateNewFolder.Dock = DockStyle.Fill;
            btnCreateNewFolder.FlatStyle = FlatStyle.Flat;
            btnCreateNewFolder.ForeColor = Color.White;
            btnCreateNewFolder.Location = new Point(15, 127);
            btnCreateNewFolder.Name = "btnCreateNewFolder";
            btnCreateNewFolder.Size = new Size(207, 34);
            btnCreateNewFolder.TabIndex = 4;
            btnCreateNewFolder.Text = "Create new folder";
            btnCreateNewFolder.UseVisualStyleBackColor = false;
            // 
            // btnPullTubData
            // 
            btnPullTubData.BackColor = Color.FromArgb(80, 80, 80);
            btnPullTubData.Dock = DockStyle.Fill;
            btnPullTubData.FlatStyle = FlatStyle.Flat;
            btnPullTubData.ForeColor = Color.White;
            btnPullTubData.Location = new Point(228, 127);
            btnPullTubData.Name = "btnPullTubData";
            btnPullTubData.Size = new Size(332, 34);
            btnPullTubData.TabIndex = 5;
            btnPullTubData.Text = "Pull tub data";
            btnPullTubData.UseVisualStyleBackColor = false;
            // 
            // prgPullStatus
            // 
            prgPullStatus.Dock = DockStyle.Fill;
            prgPullStatus.Location = new Point(566, 127);
            prgPullStatus.Name = "prgPullStatus";
            prgPullStatus.Size = new Size(324, 34);
            prgPullStatus.TabIndex = 6;
            // 
            // btnSyncH5
            // 
            btnSyncH5.BackColor = Color.FromArgb(80, 80, 80);
            btnSyncH5.Dock = DockStyle.Fill;
            btnSyncH5.FlatStyle = FlatStyle.Flat;
            btnSyncH5.ForeColor = Color.White;
            btnSyncH5.Location = new Point(15, 215);
            btnSyncH5.Name = "btnSyncH5";
            btnSyncH5.Size = new Size(207, 57);
            btnSyncH5.TabIndex = 9;
            btnSyncH5.Text = "Sync h5";
            btnSyncH5.UseVisualStyleBackColor = false;
            // 
            // btnSyncSavedModel
            // 
            btnSyncSavedModel.BackColor = Color.FromArgb(80, 80, 80);
            btnSyncSavedModel.Dock = DockStyle.Fill;
            btnSyncSavedModel.FlatStyle = FlatStyle.Flat;
            btnSyncSavedModel.ForeColor = Color.White;
            btnSyncSavedModel.Location = new Point(228, 215);
            btnSyncSavedModel.Name = "btnSyncSavedModel";
            btnSyncSavedModel.Size = new Size(332, 57);
            btnSyncSavedModel.TabIndex = 10;
            btnSyncSavedModel.Text = "Sync savedmodel";
            btnSyncSavedModel.UseVisualStyleBackColor = false;
            // 
            // btnSyncTFlite
            // 
            btnSyncTFlite.BackColor = Color.FromArgb(80, 80, 80);
            btnSyncTFlite.Dock = DockStyle.Fill;
            btnSyncTFlite.FlatStyle = FlatStyle.Flat;
            btnSyncTFlite.ForeColor = Color.White;
            btnSyncTFlite.Location = new Point(566, 215);
            btnSyncTFlite.Name = "btnSyncTFlite";
            btnSyncTFlite.Size = new Size(324, 57);
            btnSyncTFlite.TabIndex = 11;
            btnSyncTFlite.Text = "Sync tflite";
            btnSyncTFlite.UseVisualStyleBackColor = false;
            // 
            // pnlDriveArea
            // 
            tlpRoot.SetColumnSpan(pnlDriveArea, 3);
            pnlDriveArea.Controls.Add(tlpDrive);
            pnlDriveArea.Dock = DockStyle.Fill;
            pnlDriveArea.Location = new Point(15, 278);
            pnlDriveArea.Name = "pnlDriveArea";
            pnlDriveArea.Padding = new Padding(8);
            pnlDriveArea.Size = new Size(875, 398);
            pnlDriveArea.TabIndex = 13;
            // 
            // tlpDrive
            // 
            tlpDrive.ColumnCount = 2;
            tlpDrive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.25373F));
            tlpDrive.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.74627F));
            tlpDrive.Controls.Add(lblDriveModel, 0, 0);
            tlpDrive.Controls.Add(lblDrivePilot, 1, 0);
            tlpDrive.Controls.Add(btnDrive, 0, 2);
            tlpDrive.Controls.Add(btnStop, 1, 2);
            tlpDrive.Dock = DockStyle.Fill;
            tlpDrive.Location = new Point(8, 8);
            tlpDrive.Name = "tlpDrive";
            tlpDrive.RowCount = 3;
            tlpDrive.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpDrive.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpDrive.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpDrive.Size = new Size(859, 382);
            tlpDrive.TabIndex = 0;
            // 
            // lblDriveModel
            // 
            lblDriveModel.BackColor = Color.FromArgb(80, 80, 80);
            lblDriveModel.Dock = DockStyle.Top;
            lblDriveModel.ForeColor = Color.White;
            lblDriveModel.Location = new Point(3, 0);
            lblDriveModel.Name = "lblDriveModel";
            lblDriveModel.Size = new Size(417, 23);
            lblDriveModel.TabIndex = 0;
            lblDriveModel.Text = "tflite_linear";
            lblDriveModel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDrivePilot
            // 
            lblDrivePilot.BackColor = Color.FromArgb(80, 80, 80);
            lblDrivePilot.Dock = DockStyle.Top;
            lblDrivePilot.ForeColor = Color.White;
            lblDrivePilot.Location = new Point(426, 0);
            lblDrivePilot.Name = "lblDrivePilot";
            lblDrivePilot.Size = new Size(430, 23);
            lblDrivePilot.TabIndex = 1;
            lblDrivePilot.Text = "No pilot";
            lblDrivePilot.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnDrive
            // 
            btnDrive.BackColor = Color.FromArgb(80, 80, 80);
            btnDrive.Dock = DockStyle.Left;
            btnDrive.FlatStyle = FlatStyle.Flat;
            btnDrive.ForeColor = Color.White;
            btnDrive.Location = new Point(3, 337);
            btnDrive.Name = "btnDrive";
            btnDrive.Size = new Size(75, 42);
            btnDrive.TabIndex = 2;
            btnDrive.Text = "Drive";
            btnDrive.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.FromArgb(80, 80, 80);
            btnStop.Dock = DockStyle.Right;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(781, 337);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 42);
            btnStop.TabIndex = 3;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = false;
            // 
            // prgPushStatus
            // 
            prgPushStatus.Dock = DockStyle.Fill;
            prgPushStatus.Location = new Point(566, 167);
            prgPushStatus.Name = "prgPushStatus";
            prgPushStatus.Size = new Size(324, 42);
            prgPushStatus.TabIndex = 8;
            // 
            // btnPushPilots
            // 
            btnPushPilots.BackColor = Color.FromArgb(80, 80, 80);
            btnPushPilots.Dock = DockStyle.Fill;
            btnPushPilots.FlatStyle = FlatStyle.Flat;
            btnPushPilots.ForeColor = Color.White;
            btnPushPilots.Location = new Point(228, 167);
            btnPushPilots.Name = "btnPushPilots";
            btnPushPilots.Size = new Size(332, 42);
            btnPushPilots.TabIndex = 7;
            btnPushPilots.Text = "Push pilots";
            btnPushPilots.UseVisualStyleBackColor = false;
            // 
            // ucCarConnector
            // 
            BackColor = Color.White;
            Controls.Add(tlpRoot);
            Name = "ucCarConnector";
            Size = new Size(905, 691);
            tlpRoot.ResumeLayout(false);
            tlpRoot.PerformLayout();
            pnlDriveArea.ResumeLayout(false);
            tlpDrive.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpDrive;
    }
}
