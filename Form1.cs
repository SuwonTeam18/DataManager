namespace DonkeyUi
{
    public partial class Form1 : Form
    {
        // ═══════════════════════════════════════════════════════════════
        // [수정] 탭별 인스턴스를 미리 만들어 재사용
        // 기존: 버튼 클릭마다 new로 생성 → 탭 전환 시 데이터 초기화됨
        // 변경: readonly 필드로 한 번만 생성하여 데이터 유지
        // ═══════════════════════════════════════════════════════════════
        private readonly ucTubManager _tubManager = new ucTubManager();
        private readonly ucTrainer _trainer = new ucTrainer();
        private readonly ucPilotArena _pilotArena = new ucPilotArena();
        private readonly ucCarConnector _carConnector = new ucCarConnector();

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadControl(UserControl uc)
        {
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
            pnlContent.BringToFront();
        }

        // Tub Manager 버튼 클릭
        private void btnTubManager_Click(object sender, EventArgs e)
        {
            LoadControl(_tubManager);
        }

        // ═══════════════════════════════════════════════════════════════
        // [수정] Trainer 탭 전환 시 Tub Manager의 선택 경로를 Trainer에 전달
        // 기존: LoadControl(new ucTrainer()) 단순 전환
        // 변경: TubManager에서 선택한 데이터 경로를 Trainer로 넘겨
        //       학습 시 올바른 데이터 폴더가 자동으로 설정되도록 함
        // ═══════════════════════════════════════════════════════════════
        private void btnTrainer_Click(object sender, EventArgs e)
        {
            string tubPath = _tubManager.SelectedDataPath;
            if (!string.IsNullOrEmpty(tubPath))
                _trainer.SetTubPath(tubPath);

            // mycar 경로 추출 — Windows 경로 기준
            string mycarPath = tubPath;
            int dataIdx = mycarPath.Replace("\\", "/").IndexOf("/data/");
            if (dataIdx > 0)
                mycarPath = mycarPath.Substring(0, dataIdx);

            _trainer.InitModelWatcher(mycarPath);
            LoadControl(_trainer);
        }

        // Pilot Arena 버튼 클릭
        private void btnPilotArena_Click(object sender, EventArgs e)
        {
            LoadControl(_pilotArena);
        }

        // Car Connector 버튼 클릭
        private void btnCarConnector_Click(object sender, EventArgs e)
        {
            LoadControl(_carConnector);
        }
    }
}