namespace DonkeyUi
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlContent = new Panel();
            pnlTopMenu = new Panel();
            btnCarConnector = new Button();
            btnPilotArena = new Button();
            btnTrainer = new Button();
            btnTubManager = new Button();
            pnlTopMenu.SuspendLayout();
            SuspendLayout();
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.White;
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 0);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(947, 726);
            pnlContent.TabIndex = 0;
            // 
            // pnlTopMenu
            // 
            pnlTopMenu.BackColor = Color.White;
            pnlTopMenu.Controls.Add(btnCarConnector);
            pnlTopMenu.Controls.Add(btnPilotArena);
            pnlTopMenu.Controls.Add(btnTrainer);
            pnlTopMenu.Controls.Add(btnTubManager);
            pnlTopMenu.Dock = DockStyle.Top;
            pnlTopMenu.Location = new Point(0, 0);
            pnlTopMenu.Name = "pnlTopMenu";
            pnlTopMenu.Size = new Size(947, 71);
            pnlTopMenu.TabIndex = 1;
            // 
            // btnCarConnector
            // 
            btnCarConnector.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnCarConnector.Location = new Point(720, 16);
            btnCarConnector.Name = "btnCarConnector";
            btnCarConnector.Size = new Size(215, 37);
            btnCarConnector.TabIndex = 3;
            btnCarConnector.Text = "Car Connector";
            btnCarConnector.UseVisualStyleBackColor = true;
            btnCarConnector.Visible = false;
            btnCarConnector.Click += btnCarConnector_Click;
            // 
            // btnPilotArena
            // 
            btnPilotArena.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnPilotArena.Location = new Point(489, 16);
            btnPilotArena.Name = "btnPilotArena";
            btnPilotArena.Size = new Size(213, 37);
            btnPilotArena.TabIndex = 2;
            btnPilotArena.Text = "주행 분석";
            btnPilotArena.UseVisualStyleBackColor = true;
            btnPilotArena.Click += btnPilotArena_Click;
            // 
            // btnTrainer
            // 
            btnTrainer.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnTrainer.Location = new Point(251, 16);
            btnTrainer.Name = "btnTrainer";
            btnTrainer.Size = new Size(221, 37);
            btnTrainer.TabIndex = 1;
            btnTrainer.Text = "모델 학습";
            btnTrainer.UseVisualStyleBackColor = true;
            btnTrainer.Click += btnTrainer_Click;
            // 
            // btnTubManager
            // 
            btnTubManager.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnTubManager.Location = new Point(12, 16);
            btnTubManager.Name = "btnTubManager";
            btnTubManager.Size = new Size(221, 37);
            btnTubManager.TabIndex = 0;
            btnTubManager.Text = "데이터 관리";
            btnTubManager.UseVisualStyleBackColor = true;
            btnTubManager.Click += btnTubManager_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(947, 726);
            Controls.Add(pnlTopMenu);
            Controls.Add(pnlContent);
            Name = "Form1";
            Text = "Donkey Car";
            pnlTopMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlContent;
        private Panel pnlTopMenu;
        private Button btnCarConnector;
        private Button btnPilotArena;
        private Button btnTrainer;
        private Button btnTubManager;
    }


}
