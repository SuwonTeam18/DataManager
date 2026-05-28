namespace DonkeyUi
{
    partial class ucTrainer
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        // ── 색상 상수 ─────────────────────────────────────────────────
        private static readonly Color ClrBg = Color.FromArgb(245, 245, 245);
        private static readonly Color ClrCard = Color.White;
        private static readonly Color ClrBorder = Color.FromArgb(210, 210, 210);
        private static readonly Color ClrText = Color.FromArgb(30, 30, 30);
        private static readonly Color ClrMuted = Color.FromArgb(120, 120, 120);
        private static readonly Color ClrAccent = Color.FromArgb(24, 95, 165);
        private static readonly Color ClrGreen = Color.FromArgb(34, 120, 34);
        private static readonly Color ClrRed = Color.FromArgb(163, 45, 45);

        // ── 헬퍼 ─────────────────────────────────────────────────────

        private Panel MakeSecPanel(int y, int h)
        {
            var p = new Panel();
            p.BackColor = ClrCard;
            p.BorderStyle = BorderStyle.FixedSingle;
            p.Location = new Point(8, y);
            p.Size = new Size(900, h);
            return p;
        }

        private Label MakeSecLabel(string text, int x, int y)
        {
            var l = new Label();
            l.AutoSize = true;
            l.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            l.ForeColor = ClrAccent;
            l.Location = new Point(x, y);
            l.Text = text;
            return l;
        }

        private Label MakeLabel(string text, int x, int y,
                                 Color color, float size = 9F)
        {
            var l = new Label();
            l.AutoSize = true;
            l.Font = new Font("맑은 고딕", size);
            l.ForeColor = color;
            l.Location = new Point(x, y);
            l.Text = text;
            return l;
        }

        private TextBox MakeTextBox(int x, int y, int w,
                                     string text = "", string ph = "")
        {
            var t = new TextBox();
            t.BackColor = ClrCard;
            t.BorderStyle = BorderStyle.FixedSingle;
            t.Font = new Font("맑은 고딕", 9F);
            t.ForeColor = ClrText;
            t.Location = new Point(x, y);
            t.Size = new Size(w, 24);
            t.Text = text;
            if (!string.IsNullOrEmpty(ph)) t.PlaceholderText = ph;
            return t;
        }

        private Button MakeButton(string text, int x, int y, int w, int h,
                                   Color bg, Color fg, bool border = false)
        {
            var b = new Button();
            b.BackColor = bg;
            b.Cursor = Cursors.Hand;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = border ? 1 : 0;
            if (border) b.FlatAppearance.BorderColor = ClrBorder;
            b.Font = new Font("맑은 고딕", 9F);
            b.ForeColor = fg;
            b.Location = new Point(x, y);
            b.Size = new Size(w, h);
            b.Text = text;
            b.UseVisualStyleBackColor = false;
            return b;
        }

        private void StyleDgv(DataGridView d)
        {
            d.AllowUserToAddRows = false;
            d.AllowUserToDeleteRows = false;
            d.BackgroundColor = ClrCard;
            d.BorderStyle = BorderStyle.FixedSingle;
            d.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            d.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            d.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            d.ColumnHeadersDefaultCellStyle.ForeColor = ClrMuted;
            d.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
            d.ColumnHeadersHeight = 26;
            d.DefaultCellStyle.BackColor = ClrCard;
            d.DefaultCellStyle.ForeColor = ClrText;
            d.DefaultCellStyle.Font = new Font("맑은 고딕", 9F);
            d.DefaultCellStyle.SelectionBackColor = ClrAccent;
            d.DefaultCellStyle.SelectionForeColor = Color.White;
            d.EnableHeadersVisualStyles = false;
            d.GridColor = Color.FromArgb(230, 230, 230);
            d.RowHeadersVisible = false;
            d.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private CheckBox MakeColChk(string text, int x)
        {
            var c = new CheckBox();
            c.Appearance = Appearance.Button;
            c.AutoSize = true;
            c.Checked = true;
            c.Cursor = Cursors.Hand;
            c.FlatStyle = FlatStyle.Flat;
            c.FlatAppearance.BorderSize = 1;
            c.FlatAppearance.BorderColor = ClrAccent;
            c.FlatAppearance.CheckedBackColor = Color.FromArgb(232, 240, 253);
            c.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 240, 253);
            c.Font = new Font("맑은 고딕", 8.5F);
            c.ForeColor = ClrAccent;
            c.Location = new Point(x, 0);
            c.Padding = new Padding(4, 1, 4, 1);
            c.Text = text;
            return c;
        }

        private void ToggleColumn(DataGridViewColumn col, bool visible)
        {
            col.Visible = visible;
        }

        // ── InitializeComponent ───────────────────────────────────────
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            pnlSec1 = new Panel();
            lblModelTypeDesc = new Label();
            lblSec1 = new Label();
            lblModelType = new Label();
            cmbModelType = new ComboBox();
            lblModelName = new Label();
            txtModelName = new TextBox();
            lblComment = new Label();
            txtComment = new TextBox();
            pnlSec2 = new Panel();
            btnSaveDefault = new Button();
            lblSec2 = new Label();
            lblEpochKo = new Label();
            lblEpochEn = new Label();
            nudEpoch = new NumericUpDown();
            trkEpoch = new TrackBar();
            lblBatchKo = new Label();
            lblBatchEn = new Label();
            nudBatch = new NumericUpDown();
            trkBatch = new TrackBar();
            lblAdvanced = new Label();
            dgvConfig = new DataGridView();
            colConfigCheck = new DataGridViewCheckBoxColumn();
            colConfigKey = new DataGridViewTextBoxColumn();
            colConfigValue = new DataGridViewTextBoxColumn();
            btnAddConfig = new Button();
            btnDeleteConfig = new Button();
            pnlSec3 = new Panel();
            btnPresetCopy = new Button();
            lblSec3 = new Label();
            lblPresetListTitle = new Label();
            lstPresets = new ListBox();
            txtPresetName = new TextBox();
            btnPresetAdd = new Button();
            btnPresetSave = new Button();
            btnPresetDelete = new Button();
            pnlSec4 = new Panel();
            lblSec4 = new Label();
            pnlTransfer = new Panel();
            lblTransferTitle = new Label();
            btnChooseTransfer = new Button();
            txtTransferPath = new TextBox();
            btnClearTransfer = new Button();
            btnTrain = new Button();
            btnCancelTrain = new Button();
            lblTrainStatus = new Label();
            rtbLog = new RichTextBox();
            pnlSec5 = new Panel();
            lblSec5 = new Label();
            pnlCommentEdit = new Panel();
            lblCommentEditTitle = new Label();
            txtCommentEdit = new TextBox();
            btnCommentSave = new Button();
            pnlColToggle = new Panel();
            lblColToggleTitle = new Label();
            chkColName = new CheckBox();
            chkColPilot = new CheckBox();
            chkColType = new CheckBox();
            chkColTransfer = new CheckBox();
            chkColComment = new CheckBox();
            chkColTubs = new CheckBox();
            chkColTime = new CheckBox();
            pnlHistControls = new Panel();
            chkEnableDelete = new CheckBox();
            btnDeletePilot = new Button();
            btnShowGraph = new Button();
            btnShowConfig = new Button();
            dgvTrains = new DataGridView();
            선택 = new DataGridViewCheckBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colPilot = new DataGridViewTextBoxColumn();
            colType = new DataGridViewTextBoxColumn();
            colTubs = new DataGridViewTextBoxColumn();
            colTime = new DataGridViewTextBoxColumn();
            colTransfer = new DataGridViewTextBoxColumn();
            colComment = new DataGridViewTextBoxColumn();
            lblHistHint = new Label();
            pnlScroll = new Panel();
            pnlSec1.SuspendLayout();
            pnlSec2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudEpoch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trkEpoch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudBatch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trkBatch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvConfig).BeginInit();
            pnlSec3.SuspendLayout();
            pnlSec4.SuspendLayout();
            pnlTransfer.SuspendLayout();
            pnlSec5.SuspendLayout();
            pnlCommentEdit.SuspendLayout();
            pnlColToggle.SuspendLayout();
            pnlHistControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTrains).BeginInit();
            pnlScroll.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSec1
            // 
            pnlSec1.BackColor = Color.White;
            pnlSec1.BorderStyle = BorderStyle.FixedSingle;
            pnlSec1.Controls.Add(lblModelTypeDesc);
            pnlSec1.Controls.Add(lblSec1);
            pnlSec1.Controls.Add(lblModelType);
            pnlSec1.Controls.Add(cmbModelType);
            pnlSec1.Controls.Add(lblModelName);
            pnlSec1.Controls.Add(txtModelName);
            pnlSec1.Controls.Add(lblComment);
            pnlSec1.Controls.Add(txtComment);
            pnlSec1.Location = new Point(8, 8);
            pnlSec1.Margin = new Padding(8);
            pnlSec1.Name = "pnlSec1";
            pnlSec1.Padding = new Padding(16);
            pnlSec1.Size = new Size(931, 101);
            pnlSec1.TabIndex = 4;
            // 
            // lblModelTypeDesc
            // 
            lblModelTypeDesc.AutoSize = true;
            lblModelTypeDesc.Font = new Font("맑은 고딕", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblModelTypeDesc.Location = new Point(55, 70);
            lblModelTypeDesc.Margin = new Padding(0, 4, 0, 0);
            lblModelTypeDesc.Name = "lblModelTypeDesc";
            lblModelTypeDesc.Size = new Size(0, 13);
            lblModelTypeDesc.TabIndex = 7;
            // 
            // lblSec1
            // 
            lblSec1.AutoSize = true;
            lblSec1.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblSec1.Location = new Point(16, 16);
            lblSec1.Margin = new Padding(0, 0, 16, 16);
            lblSec1.Name = "lblSec1";
            lblSec1.Size = new Size(73, 15);
            lblSec1.TabIndex = 0;
            lblSec1.Text = "1. 모델 설정";
            // 
            // lblModelType
            // 
            lblModelType.AutoSize = true;
            lblModelType.Font = new Font("맑은 고딕", 8.5F);
            lblModelType.Location = new Point(16, 47);
            lblModelType.Margin = new Padding(0, 0, 8, 0);
            lblModelType.Name = "lblModelType";
            lblModelType.Size = new Size(31, 15);
            lblModelType.TabIndex = 1;
            lblModelType.Text = "유형";
            lblModelType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbModelType
            // 
            cmbModelType.BackColor = Color.FromArgb(244, 243, 238);
            cmbModelType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModelType.FlatStyle = FlatStyle.Flat;
            cmbModelType.Font = new Font("맑은 고딕", 9F);
            cmbModelType.Items.AddRange(new object[] { "KerasLinear", "KerasInferred", "KerasCategorical", "KerasRNN", "Other" });
            cmbModelType.Location = new Point(55, 43);
            cmbModelType.Margin = new Padding(0, 0, 80, 0);
            cmbModelType.Name = "cmbModelType";
            cmbModelType.Size = new Size(120, 23);
            cmbModelType.TabIndex = 2;
            // 
            // lblModelName
            // 
            lblModelName.AutoSize = true;
            lblModelName.Font = new Font("맑은 고딕", 8.5F);
            lblModelName.Location = new Point(255, 47);
            lblModelName.Margin = new Padding(0, 0, 8, 0);
            lblModelName.Name = "lblModelName";
            lblModelName.Size = new Size(31, 15);
            lblModelName.TabIndex = 3;
            lblModelName.Text = "이름";
            lblModelName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtModelName
            // 
            txtModelName.BackColor = Color.White;
            txtModelName.BorderStyle = BorderStyle.FixedSingle;
            txtModelName.Font = new Font("맑은 고딕", 9F);
            txtModelName.Location = new Point(294, 43);
            txtModelName.Margin = new Padding(0, 0, 80, 0);
            txtModelName.Name = "txtModelName";
            txtModelName.Size = new Size(120, 23);
            txtModelName.TabIndex = 4;
            txtModelName.Text = "mypilot";
            // 
            // lblComment
            // 
            lblComment.AutoSize = true;
            lblComment.Font = new Font("맑은 고딕", 8.5F);
            lblComment.Location = new Point(494, 47);
            lblComment.Margin = new Padding(0, 0, 8, 0);
            lblComment.Name = "lblComment";
            lblComment.Size = new Size(31, 15);
            lblComment.TabIndex = 5;
            lblComment.Text = "메모";
            lblComment.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtComment
            // 
            txtComment.BackColor = Color.White;
            txtComment.BorderStyle = BorderStyle.FixedSingle;
            txtComment.Font = new Font("맑은 고딕", 9F);
            txtComment.Location = new Point(533, 44);
            txtComment.Margin = new Padding(0);
            txtComment.Name = "txtComment";
            txtComment.PlaceholderText = "이번 학습 메모...";
            txtComment.Size = new Size(380, 23);
            txtComment.TabIndex = 6;
            // 
            // pnlSec2
            // 
            pnlSec2.BackColor = Color.White;
            pnlSec2.BorderStyle = BorderStyle.FixedSingle;
            pnlSec2.Controls.Add(btnSaveDefault);
            pnlSec2.Controls.Add(lblSec2);
            pnlSec2.Controls.Add(lblEpochKo);
            pnlSec2.Controls.Add(lblEpochEn);
            pnlSec2.Controls.Add(nudEpoch);
            pnlSec2.Controls.Add(trkEpoch);
            pnlSec2.Controls.Add(lblBatchKo);
            pnlSec2.Controls.Add(lblBatchEn);
            pnlSec2.Controls.Add(nudBatch);
            pnlSec2.Controls.Add(trkBatch);
            pnlSec2.Controls.Add(lblAdvanced);
            pnlSec2.Controls.Add(dgvConfig);
            pnlSec2.Controls.Add(btnAddConfig);
            pnlSec2.Controls.Add(btnDeleteConfig);
            pnlSec2.Location = new Point(8, 117);
            pnlSec2.Margin = new Padding(8, 0, 8, 8);
            pnlSec2.Name = "pnlSec2";
            pnlSec2.Padding = new Padding(16);
            pnlSec2.Size = new Size(931, 342);
            pnlSec2.TabIndex = 3;
            // 
            // btnSaveDefault
            // 
            btnSaveDefault.BackColor = Color.FromArgb(244, 243, 238);
            btnSaveDefault.Cursor = Cursors.Hand;
            btnSaveDefault.FlatStyle = FlatStyle.Flat;
            btnSaveDefault.Font = new Font("맑은 고딕", 9F);
            btnSaveDefault.Location = new Point(529, 284);
            btnSaveDefault.Margin = new Padding(8, 8, 0, 8);
            btnSaveDefault.Name = "btnSaveDefault";
            btnSaveDefault.Size = new Size(120, 32);
            btnSaveDefault.TabIndex = 13;
            btnSaveDefault.Text = "💾 기본 설정 저장";
            btnSaveDefault.UseVisualStyleBackColor = false;
            // 
            // lblSec2
            // 
            lblSec2.AutoSize = true;
            lblSec2.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblSec2.Location = new Point(12, 16);
            lblSec2.Margin = new Padding(0, 0, 16, 16);
            lblSec2.Name = "lblSec2";
            lblSec2.Size = new Size(97, 15);
            lblSec2.TabIndex = 0;
            lblSec2.Text = "2. 학습 파라미터";
            // 
            // lblEpochKo
            // 
            lblEpochKo.AutoSize = true;
            lblEpochKo.Font = new Font("맑은 고딕", 8.5F);
            lblEpochKo.Location = new Point(16, 70);
            lblEpochKo.Margin = new Padding(0);
            lblEpochKo.Name = "lblEpochKo";
            lblEpochKo.Size = new Size(115, 15);
            lblEpochKo.TabIndex = 1;
            lblEpochKo.Text = "전체 학습 에포크 수";
            // 
            // lblEpochEn
            // 
            lblEpochEn.AutoSize = true;
            lblEpochEn.Font = new Font("맑은 고딕", 7.5F);
            lblEpochEn.Location = new Point(16, 89);
            lblEpochEn.Margin = new Padding(0, 4, 0, 16);
            lblEpochEn.Name = "lblEpochEn";
            lblEpochEn.Size = new Size(76, 12);
            lblEpochEn.TabIndex = 2;
            lblEpochEn.Text = "(MAX_EPOCHS):";
            // 
            // nudEpoch
            // 
            nudEpoch.BorderStyle = BorderStyle.FixedSingle;
            nudEpoch.Font = new Font("맑은 고딕", 9F);
            nudEpoch.Location = new Point(177, 70);
            nudEpoch.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudEpoch.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudEpoch.Name = "nudEpoch";
            nudEpoch.Size = new Size(79, 23);
            nudEpoch.TabIndex = 3;
            nudEpoch.TextAlign = HorizontalAlignment.Center;
            nudEpoch.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // trkEpoch
            // 
            trkEpoch.Location = new Point(16, 117);
            trkEpoch.Margin = new Padding(0, 0, 32, 16);
            trkEpoch.Maximum = 200;
            trkEpoch.Minimum = 1;
            trkEpoch.Name = "trkEpoch";
            trkEpoch.Size = new Size(240, 45);
            trkEpoch.TabIndex = 4;
            trkEpoch.TickStyle = TickStyle.None;
            trkEpoch.Value = 60;
            // 
            // lblBatchKo
            // 
            lblBatchKo.AutoSize = true;
            lblBatchKo.Font = new Font("맑은 고딕", 8.5F);
            lblBatchKo.Location = new Point(16, 178);
            lblBatchKo.Margin = new Padding(0);
            lblBatchKo.Name = "lblBatchKo";
            lblBatchKo.Size = new Size(71, 15);
            lblBatchKo.TabIndex = 5;
            lblBatchKo.Text = "배치 사이즈";
            // 
            // lblBatchEn
            // 
            lblBatchEn.AutoSize = true;
            lblBatchEn.Font = new Font("맑은 고딕", 7.5F);
            lblBatchEn.Location = new Point(16, 197);
            lblBatchEn.Margin = new Padding(0, 4, 0, 16);
            lblBatchEn.Name = "lblBatchEn";
            lblBatchEn.Size = new Size(67, 12);
            lblBatchEn.TabIndex = 6;
            lblBatchEn.Text = "(BATCH_SIZE):";
            // 
            // nudBatch
            // 
            nudBatch.BorderStyle = BorderStyle.FixedSingle;
            nudBatch.Font = new Font("맑은 고딕", 9F);
            nudBatch.Location = new Point(176, 178);
            nudBatch.Margin = new Padding(0);
            nudBatch.Maximum = new decimal(new int[] { 512, 0, 0, 0 });
            nudBatch.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudBatch.Name = "nudBatch";
            nudBatch.Size = new Size(80, 23);
            nudBatch.TabIndex = 7;
            nudBatch.TextAlign = HorizontalAlignment.Center;
            nudBatch.Value = new decimal(new int[] { 64, 0, 0, 0 });
            // 
            // trkBatch
            // 
            trkBatch.Location = new Point(16, 228);
            trkBatch.Maximum = 512;
            trkBatch.Minimum = 1;
            trkBatch.Name = "trkBatch";
            trkBatch.Size = new Size(240, 45);
            trkBatch.TabIndex = 8;
            trkBatch.TickStyle = TickStyle.None;
            trkBatch.Value = 64;
            // 
            // lblAdvanced
            // 
            lblAdvanced.AutoSize = true;
            lblAdvanced.Font = new Font("맑은 고딕", 8.5F);
            lblAdvanced.Location = new Point(288, 47);
            lblAdvanced.Margin = new Padding(8, 0, 8, 8);
            lblAdvanced.Name = "lblAdvanced";
            lblAdvanced.Size = new Size(59, 15);
            lblAdvanced.TabIndex = 9;
            lblAdvanced.Text = "고급 설정";
            // 
            // dgvConfig
            // 
            dgvConfig.AllowUserToResizeColumns = false;
            dgvConfig.AllowUserToResizeRows = false;
            dgvConfig.BackgroundColor = Color.FromArgb(244, 243, 238);
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("맑은 고딕", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(244, 243, 238);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvConfig.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvConfig.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvConfig.Columns.AddRange(new DataGridViewColumn[] { colConfigCheck, colConfigKey, colConfigValue });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("맑은 고딕", 9F);
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(230, 242, 255);
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvConfig.DefaultCellStyle = dataGridViewCellStyle5;
            dgvConfig.GridColor = SystemColors.WindowText;
            dgvConfig.Location = new Point(288, 70);
            dgvConfig.Margin = new Padding(0, 0, 8, 0);
            dgvConfig.Name = "dgvConfig";
            dgvConfig.RowHeadersVisible = false;
            dgvConfig.Size = new Size(617, 206);
            dgvConfig.TabIndex = 10;
            // 
            // colConfigCheck
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = false;
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            colConfigCheck.DefaultCellStyle = dataGridViewCellStyle2;
            colConfigCheck.HeaderText = "선택";
            colConfigCheck.Name = "colConfigCheck";
            colConfigCheck.Resizable = DataGridViewTriState.False;
            colConfigCheck.SortMode = DataGridViewColumnSortMode.Automatic;
            colConfigCheck.Width = 40;
            // 
            // colConfigKey
            // 
            dataGridViewCellStyle3.Font = new Font("Consolas", 9F);
            colConfigKey.DefaultCellStyle = dataGridViewCellStyle3;
            colConfigKey.HeaderText = "KEY";
            colConfigKey.Name = "colConfigKey";
            colConfigKey.SortMode = DataGridViewColumnSortMode.NotSortable;
            colConfigKey.Width = 220;
            // 
            // colConfigValue
            // 
            colConfigValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Font = new Font("Consolas", 9F);
            colConfigValue.DefaultCellStyle = dataGridViewCellStyle4;
            colConfigValue.HeaderText = "VALUE";
            colConfigValue.Name = "colConfigValue";
            colConfigValue.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // btnAddConfig
            // 
            btnAddConfig.BackColor = Color.FromArgb(244, 243, 238);
            btnAddConfig.Cursor = Cursors.Hand;
            btnAddConfig.FlatStyle = FlatStyle.Flat;
            btnAddConfig.Font = new Font("맑은 고딕", 9F);
            btnAddConfig.Location = new Point(657, 284);
            btnAddConfig.Margin = new Padding(8);
            btnAddConfig.Name = "btnAddConfig";
            btnAddConfig.Size = new Size(120, 32);
            btnAddConfig.TabIndex = 11;
            btnAddConfig.Text = "추가";
            btnAddConfig.UseVisualStyleBackColor = false;
            // 
            // btnDeleteConfig
            // 
            btnDeleteConfig.BackColor = Color.FromArgb(255, 240, 240);
            btnDeleteConfig.Cursor = Cursors.Hand;
            btnDeleteConfig.FlatAppearance.BorderColor = Color.FromArgb(240, 180, 180);
            btnDeleteConfig.FlatStyle = FlatStyle.Flat;
            btnDeleteConfig.Font = new Font("맑은 고딕", 9F);
            btnDeleteConfig.ForeColor = Color.Black;
            btnDeleteConfig.Location = new Point(785, 284);
            btnDeleteConfig.Margin = new Padding(0);
            btnDeleteConfig.Name = "btnDeleteConfig";
            btnDeleteConfig.Size = new Size(120, 32);
            btnDeleteConfig.TabIndex = 12;
            btnDeleteConfig.Text = "🗑 삭제";
            btnDeleteConfig.UseVisualStyleBackColor = false;
            // 
            // pnlSec3
            // 
            pnlSec3.BackColor = Color.White;
            pnlSec3.BorderStyle = BorderStyle.FixedSingle;
            pnlSec3.Controls.Add(btnPresetCopy);
            pnlSec3.Controls.Add(lblSec3);
            pnlSec3.Controls.Add(lblPresetListTitle);
            pnlSec3.Controls.Add(lstPresets);
            pnlSec3.Controls.Add(txtPresetName);
            pnlSec3.Controls.Add(btnPresetAdd);
            pnlSec3.Controls.Add(btnPresetSave);
            pnlSec3.Controls.Add(btnPresetDelete);
            pnlSec3.Location = new Point(8, 467);
            pnlSec3.Margin = new Padding(8, 0, 8, 8);
            pnlSec3.Name = "pnlSec3";
            pnlSec3.Padding = new Padding(16);
            pnlSec3.Size = new Size(931, 278);
            pnlSec3.TabIndex = 2;
            // 
            // btnPresetCopy
            // 
            btnPresetCopy.BackColor = Color.FromArgb(244, 243, 238);
            btnPresetCopy.Cursor = Cursors.Hand;
            btnPresetCopy.FlatStyle = FlatStyle.Flat;
            btnPresetCopy.Font = new Font("맑은 고딕", 9F);
            btnPresetCopy.Location = new Point(793, 140);
            btnPresetCopy.Margin = new Padding(0, 0, 0, 8);
            btnPresetCopy.Name = "btnPresetCopy";
            btnPresetCopy.Size = new Size(120, 32);
            btnPresetCopy.TabIndex = 8;
            btnPresetCopy.Text = "복사";
            btnPresetCopy.UseVisualStyleBackColor = false;
            // 
            // lblSec3
            // 
            lblSec3.AutoSize = true;
            lblSec3.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblSec3.Location = new Point(16, 16);
            lblSec3.Margin = new Padding(0, 0, 16, 16);
            lblSec3.Name = "lblSec3";
            lblSec3.Size = new Size(85, 15);
            lblSec3.TabIndex = 0;
            lblSec3.Text = "3. 프리셋 관리";
            // 
            // lblPresetListTitle
            // 
            lblPresetListTitle.AutoSize = true;
            lblPresetListTitle.Font = new Font("맑은 고딕", 8.5F);
            lblPresetListTitle.Location = new Point(16, 47);
            lblPresetListTitle.Margin = new Padding(8, 0, 8, 8);
            lblPresetListTitle.Name = "lblPresetListTitle";
            lblPresetListTitle.Size = new Size(83, 15);
            lblPresetListTitle.TabIndex = 1;
            lblPresetListTitle.Text = "저장된 프리셋";
            // 
            // lstPresets
            // 
            lstPresets.BorderStyle = BorderStyle.FixedSingle;
            lstPresets.DrawMode = DrawMode.OwnerDrawFixed;
            lstPresets.Font = new Font("맑은 고딕", 12F);
            lstPresets.ItemHeight = 30;
            lstPresets.Location = new Point(16, 70);
            lstPresets.Margin = new Padding(0, 0, 16, 8);
            lstPresets.Name = "lstPresets";
            lstPresets.Size = new Size(761, 182);
            lstPresets.TabIndex = 2;
            lstPresets.DrawItem += lstPresets_DrawItem;
            // 
            // txtPresetName
            // 
            txtPresetName.BorderStyle = BorderStyle.FixedSingle;
            txtPresetName.Font = new Font("맑은 고딕", 9F);
            txtPresetName.Location = new Point(793, 69);
            txtPresetName.Margin = new Padding(0, 0, 0, 8);
            txtPresetName.Name = "txtPresetName";
            txtPresetName.PlaceholderText = "이름 입력...";
            txtPresetName.Size = new Size(120, 23);
            txtPresetName.TabIndex = 4;
            // 
            // btnPresetAdd
            // 
            btnPresetAdd.BackColor = Color.FromArgb(244, 243, 238);
            btnPresetAdd.Cursor = Cursors.Hand;
            btnPresetAdd.FlatStyle = FlatStyle.Flat;
            btnPresetAdd.Font = new Font("맑은 고딕", 9F);
            btnPresetAdd.Location = new Point(793, 100);
            btnPresetAdd.Margin = new Padding(0, 0, 0, 8);
            btnPresetAdd.Name = "btnPresetAdd";
            btnPresetAdd.Size = new Size(120, 32);
            btnPresetAdd.TabIndex = 5;
            btnPresetAdd.Text = "추가";
            btnPresetAdd.UseVisualStyleBackColor = false;
            // 
            // btnPresetSave
            // 
            btnPresetSave.BackColor = Color.FromArgb(230, 242, 255);
            btnPresetSave.Cursor = Cursors.Hand;
            btnPresetSave.FlatAppearance.BorderColor = Color.FromArgb(140, 180, 225);
            btnPresetSave.FlatStyle = FlatStyle.Flat;
            btnPresetSave.Font = new Font("맑은 고딕", 9F);
            btnPresetSave.ForeColor = SystemColors.ControlText;
            btnPresetSave.Location = new Point(793, 180);
            btnPresetSave.Margin = new Padding(0, 0, 0, 8);
            btnPresetSave.Name = "btnPresetSave";
            btnPresetSave.Size = new Size(120, 32);
            btnPresetSave.TabIndex = 6;
            btnPresetSave.Text = "💾 저장";
            btnPresetSave.UseVisualStyleBackColor = false;
            // 
            // btnPresetDelete
            // 
            btnPresetDelete.BackColor = Color.FromArgb(255, 240, 240);
            btnPresetDelete.Cursor = Cursors.Hand;
            btnPresetDelete.FlatAppearance.BorderColor = Color.FromArgb(240, 180, 180);
            btnPresetDelete.FlatStyle = FlatStyle.Flat;
            btnPresetDelete.Font = new Font("맑은 고딕", 9F);
            btnPresetDelete.Location = new Point(793, 220);
            btnPresetDelete.Margin = new Padding(0, 0, 0, 8);
            btnPresetDelete.Name = "btnPresetDelete";
            btnPresetDelete.Size = new Size(120, 32);
            btnPresetDelete.TabIndex = 7;
            btnPresetDelete.Text = "🗑 삭제";
            btnPresetDelete.UseVisualStyleBackColor = false;
            // 
            // pnlSec4
            // 
            pnlSec4.BackColor = Color.White;
            pnlSec4.BorderStyle = BorderStyle.FixedSingle;
            pnlSec4.Controls.Add(lblSec4);
            pnlSec4.Controls.Add(pnlTransfer);
            pnlSec4.Controls.Add(btnTrain);
            pnlSec4.Controls.Add(btnCancelTrain);
            pnlSec4.Controls.Add(lblTrainStatus);
            pnlSec4.Controls.Add(rtbLog);
            pnlSec4.Location = new Point(8, 753);
            pnlSec4.Margin = new Padding(8, 0, 8, 8);
            pnlSec4.Name = "pnlSec4";
            pnlSec4.Padding = new Padding(16);
            pnlSec4.Size = new Size(931, 427);
            pnlSec4.TabIndex = 1;
            // 
            // lblSec4
            // 
            lblSec4.AutoSize = true;
            lblSec4.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblSec4.Location = new Point(16, 16);
            lblSec4.Margin = new Padding(0, 0, 16, 16);
            lblSec4.Name = "lblSec4";
            lblSec4.Size = new Size(73, 15);
            lblSec4.TabIndex = 0;
            lblSec4.Text = "4. 학습 실행";
            // 
            // pnlTransfer
            // 
            pnlTransfer.BackColor = Color.White;
            pnlTransfer.Controls.Add(lblTransferTitle);
            pnlTransfer.Controls.Add(btnChooseTransfer);
            pnlTransfer.Controls.Add(txtTransferPath);
            pnlTransfer.Controls.Add(btnClearTransfer);
            pnlTransfer.Location = new Point(16, 50);
            pnlTransfer.Margin = new Padding(0, 0, 0, 16);
            pnlTransfer.Name = "pnlTransfer";
            pnlTransfer.Padding = new Padding(8);
            pnlTransfer.Size = new Size(897, 63);
            pnlTransfer.TabIndex = 1;
            // 
            // lblTransferTitle
            // 
            lblTransferTitle.AutoSize = true;
            lblTransferTitle.Font = new Font("맑은 고딕", 8F);
            lblTransferTitle.Location = new Point(0, 8);
            lblTransferTitle.Margin = new Padding(0);
            lblTransferTitle.Name = "lblTransferTitle";
            lblTransferTitle.Size = new Size(194, 13);
            lblTransferTitle.TabIndex = 0;
            lblTransferTitle.Text = "전이 학습 모델 선택 (Transfer model)";
            // 
            // btnChooseTransfer
            // 
            btnChooseTransfer.BackColor = Color.FromArgb(244, 243, 238);
            btnChooseTransfer.Cursor = Cursors.Hand;
            btnChooseTransfer.FlatStyle = FlatStyle.Flat;
            btnChooseTransfer.Font = new Font("맑은 고딕", 9F);
            btnChooseTransfer.Location = new Point(0, 29);
            btnChooseTransfer.Margin = new Padding(0, 8, 8, 0);
            btnChooseTransfer.Name = "btnChooseTransfer";
            btnChooseTransfer.Size = new Size(119, 24);
            btnChooseTransfer.TabIndex = 1;
            btnChooseTransfer.Text = "📁 모델 선택";
            btnChooseTransfer.UseVisualStyleBackColor = false;
            // 
            // txtTransferPath
            // 
            txtTransferPath.BackColor = Color.White;
            txtTransferPath.BorderStyle = BorderStyle.FixedSingle;
            txtTransferPath.Font = new Font("Consolas", 8.5F);
            txtTransferPath.Location = new Point(127, 32);
            txtTransferPath.Margin = new Padding(0, 0, 8, 0);
            txtTransferPath.Name = "txtTransferPath";
            txtTransferPath.ReadOnly = true;
            txtTransferPath.Size = new Size(722, 21);
            txtTransferPath.TabIndex = 2;
            txtTransferPath.Text = "선택 안 됨 — 처음부터 학습";
            // 
            // btnClearTransfer
            // 
            btnClearTransfer.BackColor = Color.FromArgb(255, 240, 240);
            btnClearTransfer.Cursor = Cursors.Hand;
            btnClearTransfer.FlatAppearance.BorderColor = Color.FromArgb(240, 180, 180);
            btnClearTransfer.FlatStyle = FlatStyle.Flat;
            btnClearTransfer.Font = new Font("맑은 고딕", 9F);
            btnClearTransfer.Location = new Point(860, 29);
            btnClearTransfer.Name = "btnClearTransfer";
            btnClearTransfer.Size = new Size(24, 24);
            btnClearTransfer.TabIndex = 3;
            btnClearTransfer.Text = "✕";
            btnClearTransfer.UseVisualStyleBackColor = false;
            // 
            // btnTrain
            // 
            btnTrain.BackColor = Color.FromArgb(230, 242, 255);
            btnTrain.Cursor = Cursors.Hand;
            btnTrain.FlatAppearance.BorderColor = Color.FromArgb(140, 180, 225);
            btnTrain.FlatStyle = FlatStyle.Flat;
            btnTrain.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnTrain.ForeColor = SystemColors.ControlText;
            btnTrain.Location = new Point(16, 129);
            btnTrain.Margin = new Padding(0, 0, 8, 0);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(120, 32);
            btnTrain.TabIndex = 2;
            btnTrain.Text = "▶  학습 시작";
            btnTrain.UseVisualStyleBackColor = false;
            // 
            // btnCancelTrain
            // 
            btnCancelTrain.BackColor = Color.FromArgb(255, 240, 240);
            btnCancelTrain.Cursor = Cursors.Hand;
            btnCancelTrain.FlatAppearance.BorderColor = Color.FromArgb(240, 180, 180);
            btnCancelTrain.FlatStyle = FlatStyle.Flat;
            btnCancelTrain.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnCancelTrain.ForeColor = SystemColors.ControlText;
            btnCancelTrain.Location = new Point(144, 129);
            btnCancelTrain.Margin = new Padding(0, 0, 8, 0);
            btnCancelTrain.Name = "btnCancelTrain";
            btnCancelTrain.Size = new Size(120, 32);
            btnCancelTrain.TabIndex = 3;
            btnCancelTrain.Text = "■  학습 중단";
            btnCancelTrain.UseVisualStyleBackColor = false;
            // 
            // lblTrainStatus
            // 
            lblTrainStatus.AutoSize = true;
            lblTrainStatus.Font = new Font("맑은 고딕", 9F);
            lblTrainStatus.Location = new Point(275, 138);
            lblTrainStatus.Name = "lblTrainStatus";
            lblTrainStatus.Size = new Size(47, 15);
            lblTrainStatus.TabIndex = 4;
            lblTrainStatus.Text = "대기 중";
            // 
            // rtbLog
            // 
            rtbLog.BackColor = Color.FromArgb(18, 18, 18);
            rtbLog.BorderStyle = BorderStyle.None;
            rtbLog.Font = new Font("Consolas", 9.5F);
            rtbLog.ForeColor = Color.FromArgb(180, 180, 180);
            rtbLog.Location = new Point(16, 169);
            rtbLog.Margin = new Padding(0, 8, 0, 0);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.Size = new Size(897, 240);
            rtbLog.TabIndex = 5;
            rtbLog.Text = "";
            rtbLog.WordWrap = false;
            // 
            // pnlSec5
            // 
            pnlSec5.BackColor = Color.White;
            pnlSec5.BorderStyle = BorderStyle.FixedSingle;
            pnlSec5.Controls.Add(lblSec5);
            pnlSec5.Controls.Add(pnlCommentEdit);
            pnlSec5.Controls.Add(pnlColToggle);
            pnlSec5.Controls.Add(pnlHistControls);
            pnlSec5.Controls.Add(dgvTrains);
            pnlSec5.Controls.Add(lblHistHint);
            pnlSec5.Location = new Point(8, 1188);
            pnlSec5.Margin = new Padding(8, 0, 8, 8);
            pnlSec5.Name = "pnlSec5";
            pnlSec5.Padding = new Padding(16);
            pnlSec5.Size = new Size(931, 612);
            pnlSec5.TabIndex = 0;
            // 
            // lblSec5
            // 
            lblSec5.AutoSize = true;
            lblSec5.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblSec5.Location = new Point(16, 16);
            lblSec5.Margin = new Padding(0, 0, 16, 16);
            lblSec5.Name = "lblSec5";
            lblSec5.Size = new Size(113, 15);
            lblSec5.TabIndex = 0;
            lblSec5.Text = "5. 생성된 모델 목록";
            // 
            // pnlCommentEdit
            // 
            pnlCommentEdit.BackColor = Color.White;
            pnlCommentEdit.Controls.Add(lblCommentEditTitle);
            pnlCommentEdit.Controls.Add(txtCommentEdit);
            pnlCommentEdit.Controls.Add(btnCommentSave);
            pnlCommentEdit.Location = new Point(16, 455);
            pnlCommentEdit.Margin = new Padding(0, 8, 0, 0);
            pnlCommentEdit.Name = "pnlCommentEdit";
            pnlCommentEdit.Padding = new Padding(8);
            pnlCommentEdit.Size = new Size(897, 62);
            pnlCommentEdit.TabIndex = 1;
            // 
            // lblCommentEditTitle
            // 
            lblCommentEditTitle.AutoSize = true;
            lblCommentEditTitle.Font = new Font("맑은 고딕", 8F);
            lblCommentEditTitle.Location = new Point(8, 8);
            lblCommentEditTitle.Margin = new Padding(0, 0, 0, 8);
            lblCommentEditTitle.Name = "lblCommentEditTitle";
            lblCommentEditTitle.Size = new Size(59, 13);
            lblCommentEditTitle.TabIndex = 0;
            lblCommentEditTitle.Text = "메모 수정 ";
            // 
            // txtCommentEdit
            // 
            txtCommentEdit.BorderStyle = BorderStyle.FixedSingle;
            txtCommentEdit.Enabled = false;
            txtCommentEdit.Font = new Font("맑은 고딕", 9F);
            txtCommentEdit.Location = new Point(7, 29);
            txtCommentEdit.Margin = new Padding(0, 0, 8, 0);
            txtCommentEdit.Name = "txtCommentEdit";
            txtCommentEdit.PlaceholderText = "코멘트 입력...";
            txtCommentEdit.Size = new Size(776, 23);
            txtCommentEdit.TabIndex = 1;
            // 
            // btnCommentSave
            // 
            btnCommentSave.BackColor = Color.FromArgb(230, 242, 255);
            btnCommentSave.Cursor = Cursors.Hand;
            btnCommentSave.Enabled = false;
            btnCommentSave.FlatAppearance.BorderColor = Color.FromArgb(140, 180, 225);
            btnCommentSave.FlatStyle = FlatStyle.Flat;
            btnCommentSave.Font = new Font("맑은 고딕", 9F);
            btnCommentSave.ForeColor = SystemColors.ControlText;
            btnCommentSave.Location = new Point(791, 28);
            btnCommentSave.Margin = new Padding(0);
            btnCommentSave.Name = "btnCommentSave";
            btnCommentSave.Size = new Size(96, 24);
            btnCommentSave.TabIndex = 2;
            btnCommentSave.Text = "✔  저장";
            btnCommentSave.UseVisualStyleBackColor = false;
            // 
            // pnlColToggle
            // 
            pnlColToggle.BackColor = Color.White;
            pnlColToggle.Controls.Add(lblColToggleTitle);
            pnlColToggle.Controls.Add(chkColName);
            pnlColToggle.Controls.Add(chkColPilot);
            pnlColToggle.Controls.Add(chkColType);
            pnlColToggle.Controls.Add(chkColTransfer);
            pnlColToggle.Controls.Add(chkColComment);
            pnlColToggle.Controls.Add(chkColTubs);
            pnlColToggle.Controls.Add(chkColTime);
            pnlColToggle.Location = new Point(16, 47);
            pnlColToggle.Margin = new Padding(0);
            pnlColToggle.Name = "pnlColToggle";
            pnlColToggle.Padding = new Padding(8, 8, 8, 0);
            pnlColToggle.Size = new Size(897, 69);
            pnlColToggle.TabIndex = 2;
            // 
            // lblColToggleTitle
            // 
            lblColToggleTitle.AutoSize = true;
            lblColToggleTitle.Font = new Font("맑은 고딕", 8F);
            lblColToggleTitle.Location = new Point(0, 8);
            lblColToggleTitle.Margin = new Padding(0, 0, 0, 8);
            lblColToggleTitle.Name = "lblColToggleTitle";
            lblColToggleTitle.Size = new Size(55, 13);
            lblColToggleTitle.TabIndex = 0;
            lblColToggleTitle.Text = "표시할 열";
            // 
            // chkColName
            // 
            chkColName.Checked = true;
            chkColName.CheckState = CheckState.Checked;
            chkColName.Location = new Point(8, 37);
            chkColName.Margin = new Padding(0, 0, 8, 0);
            chkColName.Name = "chkColName";
            chkColName.Size = new Size(104, 24);
            chkColName.TabIndex = 1;
            chkColName.Text = "이름";
            // 
            // chkColPilot
            // 
            chkColPilot.BackColor = Color.White;
            chkColPilot.Checked = true;
            chkColPilot.CheckState = CheckState.Checked;
            chkColPilot.Location = new Point(120, 37);
            chkColPilot.Margin = new Padding(0, 0, 8, 0);
            chkColPilot.Name = "chkColPilot";
            chkColPilot.Size = new Size(104, 24);
            chkColPilot.TabIndex = 2;
            chkColPilot.Text = "파일럿";
            chkColPilot.UseVisualStyleBackColor = false;
            // 
            // chkColType
            // 
            chkColType.Checked = true;
            chkColType.CheckState = CheckState.Checked;
            chkColType.Location = new Point(232, 37);
            chkColType.Margin = new Padding(0, 0, 8, 0);
            chkColType.Name = "chkColType";
            chkColType.Size = new Size(104, 24);
            chkColType.TabIndex = 3;
            chkColType.Text = "유형";
            // 
            // chkColTransfer
            // 
            chkColTransfer.Checked = true;
            chkColTransfer.CheckState = CheckState.Checked;
            chkColTransfer.Location = new Point(568, 37);
            chkColTransfer.Margin = new Padding(0, 0, 8, 0);
            chkColTransfer.Name = "chkColTransfer";
            chkColTransfer.Size = new Size(104, 24);
            chkColTransfer.TabIndex = 6;
            chkColTransfer.Text = "전이 모델";
            // 
            // chkColComment
            // 
            chkColComment.Checked = true;
            chkColComment.CheckState = CheckState.Checked;
            chkColComment.Location = new Point(680, 37);
            chkColComment.Margin = new Padding(0, 0, 8, 0);
            chkColComment.Name = "chkColComment";
            chkColComment.Size = new Size(104, 24);
            chkColComment.TabIndex = 7;
            chkColComment.Text = "메모";
            // 
            // chkColTubs
            // 
            chkColTubs.Checked = true;
            chkColTubs.CheckState = CheckState.Checked;
            chkColTubs.Location = new Point(344, 37);
            chkColTubs.Margin = new Padding(0, 0, 8, 0);
            chkColTubs.Name = "chkColTubs";
            chkColTubs.Size = new Size(104, 24);
            chkColTubs.TabIndex = 4;
            chkColTubs.Text = "Tubs 경로";
            // 
            // chkColTime
            // 
            chkColTime.Checked = true;
            chkColTime.CheckState = CheckState.Checked;
            chkColTime.Location = new Point(456, 37);
            chkColTime.Margin = new Padding(0, 0, 8, 0);
            chkColTime.Name = "chkColTime";
            chkColTime.Size = new Size(104, 24);
            chkColTime.TabIndex = 5;
            chkColTime.Text = "학습 시각";
            // 
            // pnlHistControls
            // 
            pnlHistControls.BackColor = Color.White;
            pnlHistControls.Controls.Add(chkEnableDelete);
            pnlHistControls.Controls.Add(btnDeletePilot);
            pnlHistControls.Controls.Add(btnShowGraph);
            pnlHistControls.Controls.Add(btnShowConfig);
            pnlHistControls.Location = new Point(16, 525);
            pnlHistControls.Margin = new Padding(0, 8, 0, 0);
            pnlHistControls.Name = "pnlHistControls";
            pnlHistControls.Padding = new Padding(8);
            pnlHistControls.Size = new Size(897, 48);
            pnlHistControls.TabIndex = 3;
            // 
            // chkEnableDelete
            // 
            chkEnableDelete.AutoSize = true;
            chkEnableDelete.Font = new Font("맑은 고딕", 9F);
            chkEnableDelete.Location = new Point(8, 16);
            chkEnableDelete.Margin = new Padding(0);
            chkEnableDelete.Name = "chkEnableDelete";
            chkEnableDelete.Size = new Size(90, 19);
            chkEnableDelete.TabIndex = 0;
            chkEnableDelete.Text = "삭제 활성화";
            // 
            // btnDeletePilot
            // 
            btnDeletePilot.BackColor = Color.FromArgb(255, 240, 240);
            btnDeletePilot.Cursor = Cursors.Hand;
            btnDeletePilot.Enabled = false;
            btnDeletePilot.FlatAppearance.BorderColor = Color.FromArgb(240, 180, 180);
            btnDeletePilot.FlatStyle = FlatStyle.Flat;
            btnDeletePilot.Font = new Font("맑은 고딕", 9F);
            btnDeletePilot.Location = new Point(106, 8);
            btnDeletePilot.Margin = new Padding(8, 0, 0, 0);
            btnDeletePilot.Name = "btnDeletePilot";
            btnDeletePilot.Size = new Size(120, 32);
            btnDeletePilot.TabIndex = 1;
            btnDeletePilot.Text = "🗑 모델 삭제";
            btnDeletePilot.UseVisualStyleBackColor = false;
            // 
            // btnShowGraph
            // 
            btnShowGraph.BackColor = Color.FromArgb(240, 240, 240);
            btnShowGraph.Cursor = Cursors.Hand;
            btnShowGraph.Enabled = false;
            btnShowGraph.FlatStyle = FlatStyle.Flat;
            btnShowGraph.Font = new Font("맑은 고딕", 9F);
            btnShowGraph.Location = new Point(641, 8);
            btnShowGraph.Margin = new Padding(0, 0, 8, 0);
            btnShowGraph.Name = "btnShowGraph";
            btnShowGraph.Size = new Size(120, 32);
            btnShowGraph.TabIndex = 2;
            btnShowGraph.Text = "📈 그래프 보기";
            btnShowGraph.UseVisualStyleBackColor = false;
            // 
            // btnShowConfig
            // 
            btnShowConfig.BackColor = Color.FromArgb(240, 240, 240);
            btnShowConfig.Cursor = Cursors.Hand;
            btnShowConfig.Enabled = false;
            btnShowConfig.FlatStyle = FlatStyle.Flat;
            btnShowConfig.Font = new Font("맑은 고딕", 9F);
            btnShowConfig.Location = new Point(769, 8);
            btnShowConfig.Margin = new Padding(0);
            btnShowConfig.Name = "btnShowConfig";
            btnShowConfig.Size = new Size(120, 32);
            btnShowConfig.TabIndex = 3;
            btnShowConfig.Text = "📋 설정 보기";
            btnShowConfig.UseVisualStyleBackColor = false;
            // 
            // dgvTrains
            // 
            dgvTrains.AllowUserToAddRows = false;
            dgvTrains.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTrains.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTrains.Columns.AddRange(new DataGridViewColumn[] { 선택, colName, colPilot, colType, colTubs, colTime, colTransfer, colComment });
            dgvTrains.Location = new Point(16, 124);
            dgvTrains.Margin = new Padding(0, 8, 0, 0);
            dgvTrains.Name = "dgvTrains";
            dgvTrains.RowHeadersVisible = false;
            dgvTrains.Size = new Size(897, 323);
            dgvTrains.TabIndex = 4;
            // 
            // 선택
            // 
            선택.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            선택.FillWeight = 40F;
            선택.HeaderText = "선택";
            선택.Name = "선택";
            선택.Resizable = DataGridViewTriState.False;
            선택.SortMode = DataGridViewColumnSortMode.Automatic;
            선택.Width = 39;
            // 
            // colName
            // 
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colName.HeaderText = "이름";
            colName.Name = "colName";
            colName.ReadOnly = true;
            // 
            // colPilot
            // 
            colPilot.HeaderText = "파일럿";
            colPilot.Name = "colPilot";
            colPilot.ReadOnly = true;
            // 
            // colType
            // 
            colType.FillWeight = 60F;
            colType.HeaderText = "유형";
            colType.Name = "colType";
            colType.ReadOnly = true;
            // 
            // colTubs
            // 
            colTubs.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTubs.FillWeight = 200F;
            colTubs.HeaderText = "Tubs 경로";
            colTubs.Name = "colTubs";
            colTubs.ReadOnly = true;
            // 
            // colTime
            // 
            colTime.FillWeight = 132F;
            colTime.HeaderText = "학습 시각";
            colTime.Name = "colTime";
            colTime.ReadOnly = true;
            colTime.Resizable = DataGridViewTriState.True;
            // 
            // colTransfer
            // 
            colTransfer.HeaderText = "전이 모델";
            colTransfer.Name = "colTransfer";
            colTransfer.ReadOnly = true;
            // 
            // colComment
            // 
            colComment.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colComment.FillWeight = 200F;
            colComment.HeaderText = "코멘트";
            colComment.Name = "colComment";
            // 
            // lblHistHint
            // 
            lblHistHint.AutoSize = true;
            lblHistHint.Font = new Font("맑은 고딕", 8F);
            lblHistHint.Location = new Point(16, 581);
            lblHistHint.Margin = new Padding(0, 8, 0, 0);
            lblHistHint.Name = "lblHistHint";
            lblHistHint.Size = new Size(253, 13);
            lblHistHint.TabIndex = 5;
            lblHistHint.Text = "행 클릭 → 코멘트 수정 / 그래프·설정 버튼 활성화";
            // 
            // pnlScroll
            // 
            pnlScroll.AutoScroll = true;
            pnlScroll.BackColor = Color.FromArgb(244, 243, 238);
            pnlScroll.Controls.Add(pnlSec5);
            pnlScroll.Controls.Add(pnlSec4);
            pnlScroll.Controls.Add(pnlSec3);
            pnlScroll.Controls.Add(pnlSec2);
            pnlScroll.Controls.Add(pnlSec1);
            pnlScroll.ForeColor = Color.Black;
            pnlScroll.Location = new Point(0, 0);
            pnlScroll.Name = "pnlScroll";
            pnlScroll.Size = new Size(947, 1808);
            pnlScroll.TabIndex = 0;
            pnlScroll.Paint += pnlScroll_Paint;
            // 
            // ucTrainer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            Controls.Add(pnlScroll);
            Name = "ucTrainer";
            Size = new Size(947, 1808);
            pnlSec1.ResumeLayout(false);
            pnlSec1.PerformLayout();
            pnlSec2.ResumeLayout(false);
            pnlSec2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudEpoch).EndInit();
            ((System.ComponentModel.ISupportInitialize)trkEpoch).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudBatch).EndInit();
            ((System.ComponentModel.ISupportInitialize)trkBatch).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvConfig).EndInit();
            pnlSec3.ResumeLayout(false);
            pnlSec3.PerformLayout();
            pnlSec4.ResumeLayout(false);
            pnlSec4.PerformLayout();
            pnlTransfer.ResumeLayout(false);
            pnlTransfer.PerformLayout();
            pnlSec5.ResumeLayout(false);
            pnlSec5.PerformLayout();
            pnlCommentEdit.ResumeLayout(false);
            pnlCommentEdit.PerformLayout();
            pnlColToggle.ResumeLayout(false);
            pnlColToggle.PerformLayout();
            pnlHistControls.ResumeLayout(false);
            pnlHistControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTrains).EndInit();
            pnlScroll.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel pnlSec1;
        private Label lblSec1;
        private Label lblModelType;
        private ComboBox cmbModelType;
        private Label lblModelName;
        private TextBox txtModelName;
        private Label lblComment;
        private TextBox txtComment;
        private Panel pnlSec2;
        private Label lblSec2;
        private Label lblEpochKo;
        private Label lblEpochEn;
        private NumericUpDown nudEpoch;
        private TrackBar trkEpoch;
        private Label lblBatchKo;
        private Label lblBatchEn;
        private NumericUpDown nudBatch;
        private TrackBar trkBatch;
        private Label lblAdvanced;
        private DataGridView dgvConfig;
        private Button btnAddConfig;
        private Panel pnlSec3;
        private Label lblSec3;
        private Label lblPresetListTitle;
        private ListBox lstPresets;
        private TextBox txtPresetName;
        private Button btnPresetAdd;
        private Button btnPresetSave;
        private Button btnPresetDelete;
        private Panel pnlSec4;
        private Label lblSec4;
        private Panel pnlTransfer;
        private Label lblTransferTitle;
        private Button btnChooseTransfer;
        private TextBox txtTransferPath;
        private Button btnClearTransfer;
        private Button btnTrain;
        private Button btnCancelTrain;
        private Label lblTrainStatus;
        private RichTextBox rtbLog;
        private Panel pnlSec5;
        private Label lblSec5;
        private Panel pnlCommentEdit;
        private Label lblCommentEditTitle;
        private TextBox txtCommentEdit;
        private Button btnCommentSave;
        private Panel pnlColToggle;
        private Label lblColToggleTitle;
        private CheckBox chkColName;
        private CheckBox chkColPilot;
        private CheckBox chkColType;
        private CheckBox chkColTubs;
        private CheckBox chkColTime;
        private CheckBox chkColTransfer;
        private CheckBox chkColComment;
        private Panel pnlHistControls;
        private CheckBox chkEnableDelete;
        private Button btnDeletePilot;
        private Button btnShowGraph;
        private Button btnShowConfig;
        private DataGridView dgvTrains;
        private Label lblHistHint;
        private Panel pnlScroll;
        private Button btnDeleteConfig;
        private DataGridViewCheckBoxColumn colConfigCheck;
        private DataGridViewTextBoxColumn colConfigKey;
        private DataGridViewTextBoxColumn colConfigValue;
        private Button btnPresetCopy;
        private Button btnSaveDefault;
        private DataGridViewCheckBoxColumn 선택;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colPilot;
        private DataGridViewTextBoxColumn colType;
        private DataGridViewTextBoxColumn colTubs;
        private DataGridViewTextBoxColumn colTime;
        private DataGridViewTextBoxColumn colTransfer;
        private DataGridViewTextBoxColumn colComment;
        private Label lblModelTypeDesc;
    }
}