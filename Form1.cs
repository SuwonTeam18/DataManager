namespace DonkeyUi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadControl(UserControl uc)
        {
            // 기존 화면 제거
            pnlContent.Controls.Clear();

            // 패널 크기에 맞게 채우기
            uc.Dock = DockStyle.Fill;

            // 패널에 UserControl 추가
            pnlContent.Controls.Add(uc);
            pnlContent.BringToFront();

        }

        // Tub Manager 버튼 클릭
        private void btnTubManager_Click(object sender, EventArgs e)
        {
            LoadControl(new ucTubManager());
        }

        // Trainer 버튼 클릭
        private void btnTrainer_Click(object sender, EventArgs e)
        {
            LoadControl(new ucTrainer());
        }

        // Pilot Arena 버튼 클릭
        private void btnPilotArena_Click(object sender, EventArgs e)
        {
            LoadControl(new ucPilotArena());
        }

        // Car Connector 버튼 클릭
        private void btnCarConnector_Click(object sender, EventArgs e)
        {
            LoadControl(new ucCarConnector());
        }

    }
}
