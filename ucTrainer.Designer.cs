namespace DonkeyUi
{
    partial class ucTrainer
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
        // UI controls (Hungarian notation)
        private System.Windows.Forms.Label lblTrainPilot;
        private System.Windows.Forms.Label lblModelType;
        private System.Windows.Forms.ComboBox cmbModelType;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.DataGridView dgvTrains;
        private System.Windows.Forms.Panel pnlTopArea;

        private void InitializeComponent()
        {
            pnlTopArea = new Panel();
            lblTrainPilot = new Label();
            lblModelType = new Label();
            cmbModelType = new ComboBox();
            lblComment = new Label();
            txtComment = new TextBox();
            btnTrain = new Button();
            dgvTrains = new DataGridView();
            colName = new DataGridViewTextBoxColumn();
            colType = new DataGridViewTextBoxColumn();
            colTime = new DataGridViewTextBoxColumn();
            colComment = new DataGridViewTextBoxColumn();
            pnlTopArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTrains).BeginInit();
            SuspendLayout();
            // 
            // pnlTopArea
            // 
            pnlTopArea.Controls.Add(lblTrainPilot);
            pnlTopArea.Controls.Add(lblModelType);
            pnlTopArea.Controls.Add(cmbModelType);
            pnlTopArea.Controls.Add(lblComment);
            pnlTopArea.Controls.Add(txtComment);
            pnlTopArea.Controls.Add(btnTrain);
            pnlTopArea.Dock = DockStyle.Top;
            pnlTopArea.Location = new Point(0, 0);
            pnlTopArea.Name = "pnlTopArea";
            pnlTopArea.Padding = new Padding(8);
            pnlTopArea.Size = new Size(947, 80);
            pnlTopArea.TabIndex = 1;
            // 
            // lblTrainPilot
            // 
            lblTrainPilot.AutoSize = true;
            lblTrainPilot.ForeColor = Color.White;
            lblTrainPilot.Location = new Point(12, 12);
            lblTrainPilot.Name = "lblTrainPilot";
            lblTrainPilot.Size = new Size(71, 15);
            lblTrainPilot.TabIndex = 0;
            lblTrainPilot.Text = "주행 데이터";
            // 
            // lblModelType
            // 
            lblModelType.AutoSize = true;
            lblModelType.ForeColor = Color.White;
            lblModelType.Location = new Point(12, 36);
            lblModelType.Name = "lblModelType";
            lblModelType.Size = new Size(59, 15);
            lblModelType.TabIndex = 1;
            lblModelType.Text = "모델 종류";
            // 
            // cmbModelType
            // 
            cmbModelType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModelType.Items.AddRange(new object[] { "KerasLinear", "KerasCategorical", "Other" });
            cmbModelType.Location = new Point(96, 32);
            cmbModelType.Name = "cmbModelType";
            cmbModelType.Size = new Size(160, 23);
            cmbModelType.TabIndex = 2;
            // 
            // lblComment
            // 
            lblComment.AutoSize = true;
            lblComment.ForeColor = Color.White;
            lblComment.Location = new Point(280, 36);
            lblComment.Name = "lblComment";
            lblComment.Size = new Size(31, 15);
            lblComment.TabIndex = 3;
            lblComment.Text = "설명";
            // 
            // txtComment
            // 
            txtComment.Location = new Point(340, 32);
            txtComment.Name = "txtComment";
            txtComment.Size = new Size(360, 23);
            txtComment.TabIndex = 4;
            // 
            // btnTrain
            // 
            btnTrain.BackColor = Color.FromArgb(64, 64, 64);
            btnTrain.ForeColor = Color.White;
            btnTrain.Location = new Point(712, 32);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(120, 28);
            btnTrain.TabIndex = 5;
            btnTrain.Text = "학습 시작";
            btnTrain.UseVisualStyleBackColor = false;
            // 
            // dgvTrains
            // 
            dgvTrains.AllowUserToAddRows = false;
            dgvTrains.AllowUserToDeleteRows = false;
            dgvTrains.Columns.AddRange(new DataGridViewColumn[] { colName, colType, colTime, colComment });
            dgvTrains.Dock = DockStyle.Fill;
            dgvTrains.Location = new Point(0, 80);
            dgvTrains.Name = "dgvTrains";
            dgvTrains.ReadOnly = true;
            dgvTrains.RowHeadersVisible = false;
            dgvTrains.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTrains.Size = new Size(947, 611);
            dgvTrains.TabIndex = 0;
            // 
            // colName
            // 
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colName.HeaderText = "데이터명";
            colName.Name = "colName";
            colName.ReadOnly = true;
            // 
            // colType
            // 
            colType.HeaderText = "모델";
            colType.Name = "colType";
            colType.ReadOnly = true;
            colType.Width = 140;
            // 
            // colTime
            // 
            colTime.HeaderText = "학습시간";
            colTime.Name = "colTime";
            colTime.ReadOnly = true;
            colTime.Width = 180;
            // 
            // colComment
            // 
            colComment.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colComment.HeaderText = "설명";
            colComment.Name = "colComment";
            colComment.ReadOnly = true;
            // 
            // ucTrainer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            Controls.Add(dgvTrains);
            Controls.Add(pnlTopArea);
            Name = "ucTrainer";
            Size = new Size(947, 691);
            pnlTopArea.ResumeLayout(false);
            pnlTopArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTrains).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colType;
        private DataGridViewTextBoxColumn colTime;
        private DataGridViewTextBoxColumn colComment;
    }
}
