using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace DonkeyUi
{
    public partial class ucTrainer : UserControl
    {
        // ── 프리셋 데이터 구조 ────────────────────────────────────────
        private class ConfigEntry
        {
            public string Key { get; set; } = "";
            public string Value { get; set; } = "";
        }

        private class Preset
        {
            public int Epoch { get; set; } = 60;
            public int Batch { get; set; } = 64;
            public string LR { get; set; } = "0.001";
            public string Split { get; set; } = "0.8";
            public List<ConfigEntry> ConfigRows { get; set; } = new();
        }

        private readonly Dictionary<string, Preset> _presets = new();
        private string _activePreset = "";
        private Process _trainProcess = null;
        private bool _training = false;
        private string _tubPath = "./data";
        private FileSystemWatcher _modelWatcher;

        // ════════════════════════════════════════════════════════════
        // 실시간 학습 상태 추적
        // ════════════════════════════════════════════════════════════
        private int _currentEpoch = 0;
        private int _totalEpoch = 0;
        private double _lastLoss = double.NaN;
        private double _lastValLoss = double.NaN;


        private List<double> _lossHistory = new();
        private List<double> _valLossHistory = new();
        private List<double> _n0LossHistory = new();
        private List<double> _valN0LossHistory = new();
        private List<double> _n1LossHistory = new();
        private List<double> _valN1LossHistory = new();
        private bool _earlyStopDetected = false;
        private double _bestLoss = double.NaN;
        private double _bestValLoss = double.NaN;

        private Form _liveGraphForm = null;
        private Chart _liveChart = null;

        private string _currentModelName = "";
        private string _currentModelType = "";
        private string _currentTubArg = "";
        private string _currentComment = "";
        private string _currentMycarPath = "";
        private string _currentStartTime = "";

        // ════════════════════════════════════════════════════════════
        // ★ manifest.json deleted_indexes 방식 필터링
        // ════════════════════════════════════════════════════════════
        private string _manifestPath = "";
        private List<int> _originalDeletedIndexes = new List<int>();

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public ucTubManager TubManagerRef { get; set; } = null;

        private string PresetFilePath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "presets.json");

        public ucTrainer()
        {
            InitializeComponent();
            WireEvents();
            LoadPresetsFromDisk();
            SetTrainingState(false);
        }

        // ════════════════════════════════════════════════════════════
        // 이벤트 연결
        // ════════════════════════════════════════════════════════════
        private void WireEvents()
        {
            btnTrain.Click += BtnTrain_Click;
            btnCancelTrain.Click += BtnCancelTrain_Click;

            btnChooseTransfer.Click += BtnChooseTransfer_Click;
            btnClearTransfer.Click += (s, e) =>
            {
                txtTransferPath.Text = "선택 안 됨 — 처음부터 학습";
                txtTransferPath.ForeColor = Color.FromArgb(150, 150, 150);
            };

            btnAddConfig.Click += BtnAddConfig_Click;
            btnDeleteConfig.Click += BtnDeleteConfig_Click;
            dgvConfig.RowHeadersVisible = false;
            dgvConfig.AllowUserToAddRows = false;

            cmbModelType.Items.Clear();
            cmbModelType.Items.AddRange(new object[]
            {
                "linear", "categorical", "inferred", "rnn", "3d",
                "memory", "behavior", "localizer",
                "sq", "sq_imu", "sq_mem", "sq_mem_lap"
            });

            trkEpoch.ValueChanged += (s, e) =>
            {
                if (nudEpoch.Value != trkEpoch.Value)
                    nudEpoch.Value = trkEpoch.Value;
            };
            nudEpoch.ValueChanged += (s, e) =>
            {
                int v = (int)nudEpoch.Value;
                if (trkEpoch.Value != v)
                    trkEpoch.Value = Math.Max(trkEpoch.Minimum, Math.Min(trkEpoch.Maximum, v));
            };
            trkBatch.ValueChanged += (s, e) =>
            {
                if (nudBatch.Value != trkBatch.Value)
                    nudBatch.Value = trkBatch.Value;
            };
            nudBatch.ValueChanged += (s, e) =>
            {
                int v = (int)nudBatch.Value;
                if (trkBatch.Value != v)
                    trkBatch.Value = Math.Max(trkBatch.Minimum, Math.Min(trkBatch.Maximum, v));
            };

            btnPresetAdd.Click += BtnPresetAdd_Click;
            btnPresetSave.Click += BtnPresetSave_Click;
            btnPresetDelete.Click += BtnPresetDelete_Click;
            btnSaveDefault.Click += BtnSaveDefault_Click;
            btnPresetCopy.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(_activePreset)) return;
                string baseName = _activePreset;
                string copyName = baseName + " (1)";
                int count = 1;
                while (_presets.ContainsKey(copyName))
                {
                    count++;
                    copyName = baseName + $" ({count})";
                }
                var original = _presets[_activePreset];
                _presets[copyName] = new Preset
                {
                    Epoch = original.Epoch,
                    Batch = original.Batch,
                    LR = original.LR,
                    Split = original.Split,
                    ConfigRows = new List<ConfigEntry>(original.ConfigRows)
                };
                lstPresets.Items.Add(copyName);
                lstPresets.SelectedItem = copyName;
                SavePresetsToDisk();
            };
            cmbModelType.SelectedIndexChanged += (s, e) =>
            {
                var descriptions = new Dictionary<string, string>
                {
                    { "linear",      "기본형 모델 (일반 주행 데이터용)" },
                    { "categorical", "분류형 모델 (일반 주행 데이터용)" },
                    { "inferred",    "추론형 모델 (일반 주행 데이터용)" },
                    { "rnn",         "시계열 모델 (이전 주행 흐름을 기억해 학습)" },
                    { "3d",          "3D 시계열 모델 (연속된 사진들을 묶어서 학습)" },
                    { "memory",      "메모리 모델 (이전 조작 이력이 기록된 데이터 필요)" },
                    { "behavior",    "행동 규칙 모델 (주행 모드/상태 값 데이터 필요)" },
                    { "localizer",   "위치 추정 모델 (로봇의 현재 좌표 데이터 필요)" },
                    { "sq",          "경량화 모델 (연산 속도가 빠른 일반 주행용)" },
                    { "sq_imu",      "IMU 센서 필요 (가속도/자이로 센서 데이터 필수)" },
                    { "sq_mem",      "경량화+메모리 (이전 조작 이력 데이터 필요)" },
                    { "sq_mem_lap",  "경량화+메모리+랩타임 (트랙 바퀴 수 데이터 필요)" }
                };
                string selected = (cmbModelType.SelectedItem?.ToString() ?? "").Split(' ')[0];
                lblModelTypeDesc.Text = descriptions.ContainsKey(selected) ? descriptions[selected] : "";
                lblModelTypeDesc.ForeColor = new[] { "memory", "behavior", "localizer", "sq_imu", "sq_mem", "sq_mem_lap" }.Contains(selected)
                    ? Color.FromArgb(220, 120, 50)
                    : Color.FromArgb(100, 160, 100);
            };
            lstPresets.SelectedIndexChanged += LstPresets_SelectedIndexChanged;

            btnDeletePilot.Click += BtnDeletePilot_Click;
            btnShowGraph.Click += BtnShowGraph_Click;
            btnShowConfig.Click += BtnShowConfig_Click;

            chkColName.CheckedChanged += (s, e) => ToggleColumn(colName, chkColName.Checked);
            chkColPilot.CheckedChanged += (s, e) => ToggleColumn(colPilot, chkColPilot.Checked);
            chkColType.CheckedChanged += (s, e) => ToggleColumn(colType, chkColType.Checked);
            chkColTubs.CheckedChanged += (s, e) => ToggleColumn(colTubs, chkColTubs.Checked);
            chkColTime.CheckedChanged += (s, e) => ToggleColumn(colTime, chkColTime.Checked);
            chkColTransfer.CheckedChanged += (s, e) => ToggleColumn(colTransfer, chkColTransfer.Checked);
            chkColComment.CheckedChanged += (s, e) => ToggleColumn(colComment, chkColComment.Checked);

            chkEnableDelete.CheckedChanged += (s, e) =>
            {
                btnDeletePilot.Enabled = chkEnableDelete.Checked;
                btnDeletePilot.BackColor = chkEnableDelete.Checked
                    ? Color.FromArgb(163, 45, 45)
                    : Color.FromArgb(255, 240, 240);
                btnDeletePilot.ForeColor = chkEnableDelete.Checked
                    ? Color.White
                    : Color.FromArgb(163, 45, 45);
            };

            dgvTrains.AllowUserToAddRows = false;

            dgvTrains.CellValueChanged += (s, e) =>
            {
                if (e.ColumnIndex != 0 || e.RowIndex < 0) return;
                bool isChecked = Convert.ToBoolean(dgvTrains.Rows[e.RowIndex].Cells[0].Value);
                if (isChecked)
                {
                    foreach (DataGridViewRow row in dgvTrains.Rows)
                    {
                        if (row.Index != e.RowIndex && Convert.ToBoolean(row.Cells[0].Value))
                            row.Cells[0].Value = false;
                    }
                    var rowActive = dgvTrains.Rows[e.RowIndex];
                    lblCommentEditTitle.Text = $"메모 수정 — {rowActive.Cells[1].Value}";
                    txtCommentEdit.Text = rowActive.Cells[7].Value?.ToString() ?? "";
                    txtCommentEdit.Enabled = btnCommentSave.Enabled = btnShowGraph.Enabled = btnShowConfig.Enabled = true;
                }
                else if (GetCheckedRow() == null)
                {
                    lblCommentEditTitle.Text = "메모 수정 — 모델을 선택하세요";
                    txtCommentEdit.Text = "";
                    txtCommentEdit.Enabled = btnCommentSave.Enabled = btnShowGraph.Enabled = btnShowConfig.Enabled = false;
                }
            };

            dgvTrains.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvTrains.CurrentCell?.ColumnIndex == 0)
                    dgvTrains.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            btnCommentSave.Click += (s, e) =>
            {
                var row = GetCheckedRow();
                if (row == null) return;
                row.Cells[7].Value = txtCommentEdit.Text;
                string name = row.Cells[1].Value?.ToString();
                string baseName = Path.GetFileNameWithoutExtension(name);
                string mycarPath = "~/mycar";
                string lowerTub = _tubPath.ToLower();
                int dataIdx = lowerTub.IndexOf("/data");
                if (dataIdx > 0)
                    mycarPath = _tubPath.Substring(0, dataIdx);
                string tempScript = Path.Combine(Path.GetTempPath(), "save_comment.py");
                File.WriteAllText(tempScript,
                    $"import json\n" +
                    $"path = '{mycarPath}/models/database.json'\n" +
                    $"data = json.load(open(path, encoding='utf-8'))\n" +
                    $"m = [x for x in data if x.get('Name') == '{baseName}']\n" +
                    $"if m: m[-1]['Comment'] = '{txtCommentEdit.Text}'\n" +
                    $"json.dump(data, open(path, 'w', encoding='utf-8'), indent=4, ensure_ascii=False)\n",
                    new System.Text.UTF8Encoding(false)
                );
                string wslScript = ConvertToWslPath(tempScript);
                RunWsl($"python3 {wslScript}");
            };
        }

        // ════════════════════════════════════════════════════════════
        // Tub Manager 경로 수신
        // ════════════════════════════════════════════════════════════
        public void SetTubPath(string windowsPath)
        {
            if (string.IsNullOrEmpty(windowsPath)) return;
            _tubPath = ConvertToWslPath(windowsPath);
        }

        // ════════════════════════════════════════════════════════════
        // 1. Transfer model 선택
        // ════════════════════════════════════════════════════════════
        private void BtnChooseTransfer_Click(object sender, EventArgs e)
        {
            using var form = new Form();
            form.Text = "전이 학습 모델 선택";
            form.Size = new Size(480, 320);
            form.StartPosition = FormStartPosition.CenterParent;
            form.BackColor = Color.FromArgb(40, 40, 40);

            var lbl = new Label
            {
                Text = "히스토리에서 선택하거나 아래에 경로를 직접 입력하세요.",
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("맑은 고딕", 8.5F),
                Location = new Point(10, 10),
                AutoSize = true
            };
            var lst = new ListBox
            {
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.FromArgb(220, 220, 220),
                Font = new Font("Consolas", 9F),
                Location = new Point(10, 32),
                Size = new Size(440, 160),
                BorderStyle = BorderStyle.FixedSingle
            };
            foreach (DataGridViewRow row in dgvTrains.Rows)
            {
                var name = row.Cells[1].Value?.ToString();
                if (!string.IsNullOrEmpty(name)) lst.Items.Add(name);
            }
            var lblPath = new Label
            {
                Text = "직접 경로 입력",
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("맑은 고딕", 8F),
                Location = new Point(10, 200),
                AutoSize = true
            };
            var txtPath = new TextBox
            {
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.FromArgb(220, 220, 220),
                Font = new Font("Consolas", 9F),
                Location = new Point(10, 216),
                Size = new Size(340, 24),
                BorderStyle = BorderStyle.FixedSingle
            };
            var btnOk = new Button
            {
                Text = "적용",
                BackColor = Color.FromArgb(24, 95, 165),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(360, 216),
                Size = new Size(90, 24),
                DialogResult = DialogResult.OK
            };
            btnOk.FlatAppearance.BorderSize = 0;
            lst.DoubleClick += (s2, e2) => { form.DialogResult = DialogResult.OK; };
            form.Controls.AddRange(new Control[] { lbl, lst, lblPath, txtPath, btnOk });

            if (form.ShowDialog() == DialogResult.OK)
            {
                string selected = lst.SelectedItem?.ToString() ?? txtPath.Text.Trim();
                if (!string.IsNullOrEmpty(selected))
                {
                    txtTransferPath.Text = selected;
                    txtTransferPath.ForeColor = Color.FromArgb(220, 220, 220);
                }
            }
        }

        // ════════════════════════════════════════════════════════════
        // 2. 고급 설정
        // ════════════════════════════════════════════════════════════
        private void BtnAddConfig_Click(object sender, EventArgs e)
        {
            dgvConfig.Rows.Add(false, "KEY", "VALUE");
        }

        private void BtnDeleteConfig_Click(object sender, EventArgs e)
        {
            var toDelete = dgvConfig.Rows
                .Cast<DataGridViewRow>()
                .Where(r => Convert.ToBoolean(r.Cells[0].Value))
                .ToList();
            foreach (var row in toDelete)
                dgvConfig.Rows.Remove(row);
        }

        // ════════════════════════════════════════════════════════════
        // 3. 프리셋 관리
        // ════════════════════════════════════════════════════════════
        private void BtnPresetAdd_Click(object sender, EventArgs e)
        {
            string name = txtPresetName.Text.Trim();
            if (string.IsNullOrEmpty(name))
                name = "새 프리셋 " + (_presets.Count + 1);
            if (_presets.ContainsKey(name))
            {
                MessageBox.Show("같은 이름의 프리셋이 이미 있습니다.");
                return;
            }
            _presets[name] = new Preset();
            lstPresets.Items.Add(name);
            lstPresets.SelectedItem = name;
            SavePresetsToDisk();
        }

        private void BtnPresetSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_activePreset))
            {
                MessageBox.Show("저장할 프리셋을 먼저 선택하세요.");
                return;
            }
            string newName = txtPresetName.Text.Trim();
            if (!string.IsNullOrEmpty(newName) && newName != _activePreset)
            {
                _presets[newName] = _presets[_activePreset];
                _presets.Remove(_activePreset);
                int idx = lstPresets.Items.IndexOf(_activePreset);
                lstPresets.Items[idx] = newName;
                _activePreset = newName;
            }
            var configRows = new List<ConfigEntry>();
            foreach (DataGridViewRow row in dgvConfig.Rows)
            {
                string k = row.Cells[1].Value?.ToString() ?? "";
                string v = row.Cells[2].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(k))
                    configRows.Add(new ConfigEntry { Key = k, Value = v });
            }
            _presets[_activePreset] = new Preset
            {
                Epoch = (int)nudEpoch.Value,
                Batch = (int)nudBatch.Value,
                LR = "0.001",
                Split = "0.8",
                ConfigRows = configRows
            };
            SavePresetsToDisk();
        }

        private void BtnPresetDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_activePreset)) return;
            if (MessageBox.Show($"'{_activePreset}' 프리셋을 삭제할까요?",
                    "확인", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            _presets.Remove(_activePreset);
            lstPresets.Items.Remove(_activePreset);
            _activePreset = "";
            SavePresetsToDisk();
        }

        private void BtnSaveDefault_Click(object sender, EventArgs e)
        {
            string tubArg = string.IsNullOrEmpty(_tubPath) ? "./data" : _tubPath;
            string mycarPath = "~/mycar";
            int dataIdx = tubArg.IndexOf("/data/");
            if (dataIdx > 0)
                mycarPath = tubArg.Substring(0, dataIdx);
            string myconfigPath = mycarPath + "/myconfig.py";
            string defaultConfigPath = mycarPath + "/.myconfig_default.py";
            string cmd = $"cp {myconfigPath} {defaultConfigPath}";
            RunWsl(cmd, onSuccess: () => MessageBox.Show("기본 설정이 저장되었습니다!"));
        }

        private void LstPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = lstPresets.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(name) || !_presets.ContainsKey(name)) return;
            _activePreset = name;
            txtPresetName.Text = name;
            var p = _presets[name];
            nudEpoch.Value = p.Epoch;
            trkEpoch.Value = p.Epoch;
            nudBatch.Value = p.Batch;
            trkBatch.Value = p.Batch;
            dgvConfig.Rows.Clear();
            foreach (var entry in p.ConfigRows)
                dgvConfig.Rows.Add(false, entry.Key, entry.Value);
        }

        private void SavePresetsToDisk()
        {
            try
            {
                string json = JsonSerializer.Serialize(_presets,
                    new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(PresetFilePath, json);
            }
            catch { }
        }

        private void LoadPresetsFromDisk()
        {
            if (!File.Exists(PresetFilePath)) return;
            try
            {
                string json = File.ReadAllText(PresetFilePath);
                var loaded = JsonSerializer.Deserialize<Dictionary<string, Preset>>(json);
                if (loaded == null) return;
                foreach (var kv in loaded)
                {
                    _presets[kv.Key] = kv.Value;
                    lstPresets.Items.Add(kv.Key);
                }
            }
            catch { }
        }

        // ════════════════════════════════════════════════════════════
        // 4. 학습 실행
        // ════════════════════════════════════════════════════════════
        private void BtnTrain_Click(object sender, EventArgs e)
        {
            if (_training) return;

            if (cmbModelType.SelectedIndex == -1)
            {
                MessageBox.Show("모델 유형을 선택하세요.");
                return;
            }

            string modelName = txtModelName.Text.Trim();
            if (string.IsNullOrEmpty(modelName)) modelName = "mypilot";

            string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string comment = txtComment.Text.Trim();

            int epoch = (int)nudEpoch.Value;
            int batch = (int)nudBatch.Value;
            string tubArg = string.IsNullOrEmpty(_tubPath) ? "./data" : _tubPath;

            string mycarPath = "~/mycar";
            string lowerTub = tubArg.ToLower();
            int dataIdx = lowerTub.IndexOf("/data");
            if (dataIdx > 0)
                mycarPath = tubArg.Substring(0, dataIdx);
            else if (tubArg == "./data" || tubArg == "data" || string.IsNullOrEmpty(_tubPath))
                mycarPath = ".";
            else
                mycarPath = tubArg;

            string myconfigPath = mycarPath + "/myconfig.py";
            string updateScriptPath = mycarPath + "/update_config.py";
            string modelType = (cmbModelType.SelectedItem?.ToString() ?? "linear").Split(' ')[0];

            // ★ 학습 메타 정보 저장
            _currentModelName = modelName;
            _currentModelType = modelType;
            _currentTubArg = tubArg;
            _currentComment = comment;
            _currentMycarPath = mycarPath;
            _currentStartTime = startTime;

            string transferArg = "";
            string tp = txtTransferPath.Text.Trim();
            if (!string.IsNullOrEmpty(tp) && !tp.Contains("선택 안 됨") && !tp.Contains("처음부터 학습"))
            {
                string tpName = Path.GetFileNameWithoutExtension(tp);
                transferArg = $" --transfer ./models/{tpName}.h5";
            }

            string tempScript = Path.Combine(Path.GetTempPath(), "update_config.py");
            File.WriteAllText(tempScript,
                "import re, sys\n" +
                "path = sys.argv[1]\n" +
                "managed_keys = [arg.split('=', 1)[0] for arg in sys.argv[2:]]\n" +
                "with open(path, 'r') as f:\n    lines = f.readlines()\n" +
                "lines = [l for l in lines if not any(re.match(r'^' + k + r'\\s*=', l.strip()) for k in managed_keys)]\n" +
                "content = ''.join(lines)\n" +
                "for arg in sys.argv[2:]:\n    k, v = arg.split('=', 1)\n    content = content + '\\n' + k + ' = ' + v\n" +
                "with open(path, 'w') as f:\n    f.write(content)\n",
                new System.Text.UTF8Encoding(false)
            );
            string wslTempScript = ConvertToWslPath(tempScript);

            var kvArgs = new System.Text.StringBuilder();
            kvArgs.Append($"MAX_EPOCHS={epoch} BATCH_SIZE={batch}");
            foreach (DataGridViewRow row in dgvConfig.Rows)
            {
                string key = row.Cells[1].Value?.ToString()?.Trim();
                string val = row.Cells[2].Value?.ToString()?.Trim();
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(val)) continue;
                kvArgs.Append($" {key}={val}");
            }

            string cmd =
                $"cd {mycarPath} && " +
                $"sed -i 's/train(cfg, tubs, model, model_type, comment)/train(cfg, tubs, model, model_type, transfer=None, comment=comment)/' {mycarPath}/train.py && " +
                $"[ ! -f {mycarPath}/.myconfig_default.py ] && cp {myconfigPath} {mycarPath}/.myconfig_default.py; " +
                $"cp {mycarPath}/.myconfig_default.py {myconfigPath}; " +
                $"cp {wslTempScript} {updateScriptPath} && " +
                $"python3 {updateScriptPath} {myconfigPath} {kvArgs} && " +
                $"~/miniconda3/envs/e2e_env/bin/python train.py " +
                $"--tubs {tubArg} --model ./models/{modelName}.h5 --type {modelType} --comment=\"{comment}\"" +
                $"{transferArg}";

            // ★ manifest.json deleted_indexes에 필터 반영
            ApplyFilterToManifest(tubArg);

            // ★ 학습 상태 초기화
            _currentEpoch = 0;
            _totalEpoch = epoch;
            _lastLoss = double.NaN;
            _lastValLoss = double.NaN;

            _lossHistory.Clear();
            _valLossHistory.Clear();
            _n0LossHistory.Clear();
            _valN0LossHistory.Clear();
            _n1LossHistory.Clear();
            _valN1LossHistory.Clear();
            _earlyStopDetected = false;
            _bestLoss = double.NaN;
            _bestValLoss = double.NaN;

            UpdateTrainStatusLabels();
            OpenLiveGraphForm(modelName, epoch);
            SetTrainingState(true);
            rtbLog.Clear();
            AppendLog($"[{startTime}] 학습 시작: {modelName}");
            AppendLog($"명령: {cmd}");
            AppendLog("─────────────────────────────────────────");

            var psi = new ProcessStartInfo
            {
                FileName = "wsl",
                Arguments = $"bash -c \"{cmd}\"",
                WorkingDirectory = "C:\\",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _trainProcess = new Process { StartInfo = psi, EnableRaisingEvents = true };
            _trainProcess.OutputDataReceived += (s, ev) => AppendLog(ev.Data);
            _trainProcess.ErrorDataReceived += (s, ev) => AppendLog(ev.Data);

            _trainProcess.Exited += (s, ev) =>
            {
                int exitCode = _trainProcess.ExitCode;
                _trainProcess?.Dispose();
                _trainProcess = null;

                this.BeginInvoke((Action)(() =>
                {
                    // ★ 학습 완료/실패 시 manifest.json 복구
                    RestoreManifest();
                    SetTrainingState(false);

                    if (exitCode == 0)
                    {
                        AppendLog("─────────────────────────────────────────");
                        AppendLog("✔ 학습 완료!");
                        SaveTrainRecordToDb(true);
                        if (_earlyStopDetected && lblEpochStatus != null)
                        {
                            lblEpochStatus.Text = $"에포크: {_currentEpoch} / {_totalEpoch}  ⚠ 조기 종료";
                            lblEpochStatus.ForeColor = Color.FromArgb(220, 120, 50);
                        }
                    }
                    else
                    {
                        AppendLog("─────────────────────────────────────────");
                        AppendLog($"✘ 학습 실패 또는 중단 (exit code: {exitCode})");
                        SaveTrainRecordToDb(false);
                    }
                }));
            };

            try
            {
                _trainProcess.Start();
                _trainProcess.BeginOutputReadLine();
                _trainProcess.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                MessageBox.Show("연동 오류: " + ex.Message);
                SetTrainingState(false);
                _trainProcess = null;
            }
        }

        private void BtnCancelTrain_Click(object sender, EventArgs e)
        {
            if (!_training || _trainProcess == null) return;
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "wsl",
                    Arguments = "bash -c \"pkill -f train.py\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
                _trainProcess.Kill(entireProcessTree: true);
            }
            catch { }

            AppendLog("⚠ 학습이 중단되었습니다.");
            // ★ 중단 시 manifest.json 복구
            RestoreManifest();
            SetTrainingState(false);
        }

        private void SetTrainingState(bool isTraining)
        {
            _training = isTraining;

            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() => SetTrainingState(isTraining)));
                if (isTraining)
                {
                    btnShowGraph.Enabled = false;
                    btnShowConfig.Enabled = false;
                }
                else
                {
                    bool hasChecked = GetCheckedRow() != null;
                    btnShowGraph.Enabled = hasChecked;
                    btnShowConfig.Enabled = hasChecked;
                }
                var idleColor = Color.FromArgb(150, 150, 150);
                if (!isTraining)
                {
                    if (lblEpochStatus != null) lblEpochStatus.ForeColor = idleColor;
                    if (lblLossStatus != null) lblLossStatus.ForeColor = idleColor;
                    if (lblValLossStatus != null) lblValLossStatus.ForeColor = idleColor;
                }
                else
                {
                    if (lblEpochStatus != null) lblEpochStatus.ForeColor = SystemColors.ControlText;
                    if (lblLossStatus != null) lblLossStatus.ForeColor = SystemColors.ControlText;
                    if (lblValLossStatus != null) lblValLossStatus.ForeColor = SystemColors.ControlText;
                }
                return;
            }

            btnTrain.Enabled = !isTraining;
            btnCancelTrain.Enabled = isTraining;
            btnTrain.Text = isTraining ? "학습 중..." : "▶ 학습 시작";
            btnTrain.BackColor = isTraining
                ? Color.FromArgb(59, 109, 17)
                : Color.FromArgb(24, 95, 165);
            lblTrainStatus.Text = isTraining ? "학습 진행 중..." : "대기 중";
            lblTrainStatus.ForeColor = isTraining
                ? Color.FromArgb(90, 200, 170)
                : Color.FromArgb(150, 150, 150);
        }

        // ════════════════════════════════════════════════════════════
        // 5. 모델 히스토리
        // ════════════════════════════════════════════════════════════
        private void BtnShowGraph_Click(object sender, EventArgs e)
        {
            var row = GetCheckedRow();
            if (row == null) return;
            string name = row.Cells[1].Value?.ToString();
            string baseName = Path.GetFileNameWithoutExtension(name);

            string mycarPath = "~/mycar";
            string lowerTub = _tubPath.ToLower();
            int dataIdx = lowerTub.IndexOf("/data");
            if (dataIdx > 0)
                mycarPath = _tubPath.Substring(0, dataIdx);

            string tempScript = Path.Combine(Path.GetTempPath(), "show_graph.py");
            File.WriteAllText(tempScript,
                $"import json\n" +
                $"data=json.load(open('{mycarPath}/models/database.json', encoding='utf-8'))\n" +
                $"m=[x for x in data if x.get('Name')=='{baseName}']\n" +
                $"if m:\n" +
                $"    print(json.dumps(m[-1].get('History',{{}})))\n" +
                $"else:\n" +
                $"    print('{{}}')\n",
                new System.Text.UTF8Encoding(false)
            );

            string wslScript = ConvertToWslPath(tempScript);
            RunWslWithOutput($"python3 {wslScript}", output =>
            {
                try
                {
                    var history = JsonSerializer.Deserialize<Dictionary<string, List<double>>>(output);
                    if (history == null || history.Count == 0)
                    {
                        MessageBox.Show("그래프 데이터가 없습니다.");
                        return;
                    }
                    ShowHistoryChart(baseName, history);
                }
                catch
                {
                    MessageBox.Show("그래프 데이터를 불러오지 못했습니다.");
                }
            });
        }

        private void ShowHistoryChart(string modelName, Dictionary<string, List<double>> history)
        {
            var form = new Form
            {
                Text = $"학습 그래프 — {modelName}",
                Size = new Size(800, 750),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(20, 20, 20)
            };

            var lblDesc = new Label
            {
                Text = "그래프 값이 낮을수록 정확한 모델 | 두 선의 차이가 크면 과적합 (학습 데이터에만 최적화된 상태)\nloss: 학습 데이터 기준 오차 | val_loss: 미학습 데이터 기준 오차 (실제 성능에 가까움)",
                Dock = DockStyle.Top,
                Height = 55,
                ForeColor = Color.FromArgb(200, 200, 100),
                BackColor = Color.FromArgb(40, 40, 20),
                Font = new Font("맑은 고딕", 8.5F),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 8, 0)
            };

            var groups = new[]
            {
                new[] { "loss", "val_loss" },
                new[] { "n_outputs0_loss", "val_n_outputs0_loss" },
                new[] { "n_outputs1_loss", "val_n_outputs1_loss" }
            };

            int validGroups = groups.Count(g => g.Any(key => history.ContainsKey(key)));
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = validGroups,
                ColumnCount = 1,
                BackColor = Color.FromArgb(20, 20, 20)
            };
            for (int i = 0; i < validGroups; i++)
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / validGroups));

            var titles = new[]
            {
                "전체 오차 (loss)",
                "핸들 방향 오차 (steering / n_outputs0)",
                "속도 조절 오차 (throttle / n_outputs1)"
            };
            var colors = new[]
            {
                new[] { Color.Cyan, Color.OrangeRed },
                new[] { Color.Yellow, Color.DodgerBlue },
                new[] { Color.LimeGreen, Color.Orange }
            };
            var legendTexts = new Dictionary<string, string>
            {
                { "loss",                "학습 데이터 오차 (loss)" },
                { "val_loss",            "미학습 데이터 오차 (val_loss)" },
                { "n_outputs0_loss",     "핸들 학습 오차 (n_outputs0_loss)" },
                { "val_n_outputs0_loss", "핸들 미학습 오차 (val_n_outputs0_loss)" },
                { "n_outputs1_loss",     "속도 학습 오차 (n_outputs1_loss)" },
                { "val_n_outputs1_loss", "속도 미학습 오차 (val_n_outputs1_loss)" }
            };

            for (int g = 0; g < groups.Length; g++)
            {
                if (!groups[g].Any(key => history.ContainsKey(key))) continue;
                var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.FromArgb(30, 30, 30) };
                var chartArea = new ChartArea
                {
                    BackColor = Color.FromArgb(20, 20, 20),
                    AxisX = { LabelStyle = { ForeColor = Color.Gray }, LineColor = Color.Gray, MajorGrid = { LineColor = Color.FromArgb(50, 50, 50) } },
                    AxisY = { LabelStyle = { ForeColor = Color.Gray }, LineColor = Color.Gray, MajorGrid = { LineColor = Color.FromArgb(50, 50, 50) } }
                };
                chart.ChartAreas.Add(chartArea);
                chart.Titles.Add(new Title { Text = titles[g], ForeColor = Color.LightGray, Font = new Font("맑은 고딕", 9F, FontStyle.Bold) });
                chart.Legends.Add(new Legend { BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.LightGray });
                for (int s = 0; s < groups[g].Length; s++)
                {
                    string key = groups[g][s];
                    if (!history.ContainsKey(key)) continue;
                    var series = new Series
                    {
                        Name = key,
                        Color = colors[g][s],
                        ChartType = SeriesChartType.Line,
                        BorderWidth = 2,
                        LegendText = legendTexts.ContainsKey(key) ? legendTexts[key] : key
                    };
                    for (int i = 0; i < history[key].Count; i++)
                        series.Points.AddXY(i + 1, history[key][i]);
                    chart.Series.Add(series);
                }
                panel.Controls.Add(chart, 0, g);
            }

            var btnClose = new Button
            {
                Text = "닫기",
                Dock = DockStyle.Bottom,
                Height = 36,
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.FromArgb(200, 200, 200),
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => form.Close();
            form.Controls.Add(panel);
            form.Controls.Add(lblDesc);
            form.Controls.Add(btnClose);
            form.Show();
        }

        private void BtnShowConfig_Click(object sender, EventArgs e)
        {
            var row = GetCheckedRow();
            if (row == null) return;
            string name = row.Cells[1].Value?.ToString();
            string baseName = Path.GetFileNameWithoutExtension(name);

            string mycarPath = "~/mycar";
            string lowerTub = _tubPath.ToLower();
            int dataIdx = lowerTub.IndexOf("/data");
            if (dataIdx > 0)
                mycarPath = _tubPath.Substring(0, dataIdx);

            string tempScript = Path.Combine(Path.GetTempPath(), "show_config.py");
            File.WriteAllText(tempScript,
                $"import json\n" +
                $"data=json.load(open('{mycarPath}/models/database.json'))\n" +
                $"m=[x for x in data if x.get('Name')=='{baseName}']\n" +
                $"if m:\n" +
                $"    cfg=m[-1].get('Config',{{}})\n" +
                $"    for k,v in cfg.items():\n" +
                $"        print(f'{{k}}: {{v}}')\n" +
                $"else:\n" +
                $"    print('설정 없음')\n",
                new System.Text.UTF8Encoding(false)
            );

            string wslScript = ConvertToWslPath(tempScript);
            RunWslWithOutput($"python3 {wslScript}", output =>
            {
                var form = new Form
                {
                    Text = $"학습 설정 — {baseName}",
                    Size = new Size(700, 600),
                    StartPosition = FormStartPosition.CenterParent,
                    BackColor = Color.FromArgb(30, 30, 30)
                };
                var dgv = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    BackgroundColor = Color.FromArgb(30, 30, 30),
                    BorderStyle = BorderStyle.None,
                    ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                    ColumnHeadersDefaultCellStyle = { BackColor = Color.FromArgb(50, 50, 50), ForeColor = Color.FromArgb(180, 180, 180), Font = new Font("맑은 고딕", 9F, FontStyle.Bold) },
                    DefaultCellStyle = { BackColor = Color.FromArgb(40, 40, 40), ForeColor = Color.FromArgb(220, 220, 220), Font = new Font("Consolas", 9F), SelectionBackColor = Color.FromArgb(24, 95, 165), SelectionForeColor = Color.White },
                    EnableHeadersVisualStyles = false,
                    GridColor = Color.FromArgb(60, 60, 60),
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.CellSelect,
                    AllowUserToResizeRows = false,
                    RowTemplate = { Height = 24 },
                    CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                    ColumnHeadersHeight = 28
                };
                dgv.Columns.Add("colKey", "KEY");
                dgv.Columns.Add("colValue", "VALUE");
                dgv.Columns["colKey"].Width = 280;
                dgv.Columns["colValue"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.AllowUserToResizeRows = false;
                dgv.RowTemplate.Height = 24;
                foreach (var line in output.Split('\n'))
                {
                    int idx = line.IndexOf(':');
                    if (idx < 0) continue;
                    string k = line.Substring(0, idx).Trim();
                    string v = line.Substring(idx + 1).Trim();
                    if (!string.IsNullOrEmpty(k)) dgv.Rows.Add(k, v);
                }
                var btnClose = new Button
                {
                    Text = "닫기",
                    Dock = DockStyle.Bottom,
                    Height = 36,
                    BackColor = Color.FromArgb(50, 50, 50),
                    ForeColor = Color.FromArgb(200, 200, 200),
                    FlatStyle = FlatStyle.Flat
                };
                btnClose.FlatAppearance.BorderSize = 0;
                btnClose.Click += (s2, e2) => form.Close();
                form.Controls.Add(dgv);
                form.Controls.Add(btnClose);
                form.Show();
            });
        }

        private void BtnDeletePilot_Click(object sender, EventArgs e)
        {
            if (!chkEnableDelete.Checked) return;
            var row = GetCheckedRow();
            if (row == null) return;
            string name = row.Cells[1].Value?.ToString();
            string baseName = Path.GetFileNameWithoutExtension(name);
            if (MessageBox.Show($"'{name}' 파일을 삭제할까요?\n이 작업은 되돌릴 수 없습니다.",
                    "모델 삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            string mycarPath = "~/mycar";
            string lowerTub = _tubPath.ToLower();
            int dataIdx = lowerTub.IndexOf("/data");
            if (dataIdx > 0)
                mycarPath = _tubPath.Substring(0, dataIdx);

            RunWsl($"rm -rf {mycarPath}/models/{baseName}*", onSuccess: () =>
            {
                dgvTrains.Rows.Remove(row);
                chkEnableDelete.Checked = false;
            });
        }

        // ════════════════════════════════════════════════════════════
        // ★ manifest.json deleted_indexes 방식 필터링
        // ════════════════════════════════════════════════════════════

        private void ApplyFilterToManifest(string wslTubPath)
        {
            try
            {
                string winTubPath = ConvertWslToWindowsPath(wslTubPath);
                if (string.IsNullOrEmpty(winTubPath) || !Directory.Exists(winTubPath)) return;

                string manifestPath = Path.Combine(winTubPath, "manifest.json");
                if (!File.Exists(manifestPath)) return;

                // 삭제/필터된 이미지 파일명 목록
                var deletedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (TubManagerRef != null)
                {
                    foreach (var p in TubManagerRef.GetDeletedImages())
                        deletedFiles.Add(Path.GetFileName(p));
                    foreach (var p in TubManagerRef.GetFilteredImages())
                        deletedFiles.Add(Path.GetFileName(p));
                }

                if (deletedFiles.Count == 0)
                {
                    AppendLog("✔ 필터 없음 — 전체 데이터로 학습");
                    return;
                }

                // 카탈로그에서 파일명 → _index 매핑
                var fileToIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                var catalogFiles = Directory.GetFiles(winTubPath, "*.catalog").OrderBy(f => f).ToList();

                foreach (var cf in catalogFiles)
                {
                    foreach (var line in File.ReadAllLines(cf))
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        try
                        {
                            var doc = System.Text.Json.JsonDocument.Parse(line);
                            var root = doc.RootElement;
                            if (!root.TryGetProperty("_index", out var idxProp)) continue;
                            int idx = idxProp.GetInt32();
                            string fname = null;
                            if (root.TryGetProperty("cam/image_array", out var imgProp))
                                fname = Path.GetFileName(imgProp.GetString() ?? "");
                            if (!string.IsNullOrEmpty(fname))
                                fileToIndex[fname] = idx;
                        }
                        catch { }
                    }
                }

                // 삭제할 인덱스 목록
                var newDeletedIndexes = deletedFiles
                    .Where(f => fileToIndex.ContainsKey(f))
                    .Select(f => fileToIndex[f])
                    .ToList();

                if (newDeletedIndexes.Count == 0)
                {
                    AppendLog("⚠ 매핑된 인덱스 없음 — 전체 데이터로 학습");
                    return;
                }

                // manifest.json 읽기 및 업데이트
                var manifestLines = File.ReadAllLines(manifestPath).ToList();
                for (int i = 0; i < manifestLines.Count; i++)
                {
                    if (!manifestLines[i].Contains("\"paths\"")) continue;
                    try
                    {
                        var doc = System.Text.Json.JsonDocument.Parse(manifestLines[i]);
                        var paths = doc.RootElement.GetProperty("paths")
                            .EnumerateArray().Select(p => p.GetString()).ToList();
                        var currentIndex = doc.RootElement.GetProperty("current_index").GetInt32();
                        var maxLen = doc.RootElement.GetProperty("max_len").GetInt32();
                        var existingDeleted = doc.RootElement.GetProperty("deleted_indexes")
                            .EnumerateArray().Select(x => x.GetInt32()).ToList();

                        // 기존 deleted_indexes 백업
                        _originalDeletedIndexes = existingDeleted.ToList();

                        // 기존 + 새로 추가 (중복 제거, 정렬)
                        var merged = existingDeleted.Union(newDeletedIndexes).OrderBy(x => x).ToList();

                        manifestLines[i] = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            paths,
                            current_index = currentIndex,
                            max_len = maxLen,
                            deleted_indexes = merged
                        });
                    }
                    catch { }
                    break;
                }

                File.WriteAllLines(manifestPath, manifestLines);
                _manifestPath = manifestPath;
                AppendLog($"✔ 필터 적용 완료 — {newDeletedIndexes.Count}개 인덱스 학습 제외");
            }
            catch (Exception ex)
            {
                AppendLog($"⚠ 필터 적용 실패: {ex.Message}");
            }
        }

        private void RestoreManifest()
        {
            try
            {
                if (string.IsNullOrEmpty(_manifestPath) || !File.Exists(_manifestPath)) return;

                var manifestLines = File.ReadAllLines(_manifestPath).ToList();
                for (int i = 0; i < manifestLines.Count; i++)
                {
                    if (!manifestLines[i].Contains("\"paths\"")) continue;
                    try
                    {
                        var doc = System.Text.Json.JsonDocument.Parse(manifestLines[i]);
                        var paths = doc.RootElement.GetProperty("paths")
                            .EnumerateArray().Select(p => p.GetString()).ToList();
                        var currentIndex = doc.RootElement.GetProperty("current_index").GetInt32();
                        var maxLen = doc.RootElement.GetProperty("max_len").GetInt32();

                        // 원래 deleted_indexes로 복구
                        manifestLines[i] = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            paths,
                            current_index = currentIndex,
                            max_len = maxLen,
                            deleted_indexes = _originalDeletedIndexes
                        });
                    }
                    catch { }
                    break;
                }

                File.WriteAllLines(_manifestPath, manifestLines);
                _manifestPath = "";
                _originalDeletedIndexes = new List<int>();
                AppendLog("✔ manifest.json 원본 복구 완료");
            }
            catch (Exception ex)
            {
                AppendLog($"⚠ manifest.json 복구 실패: {ex.Message}");
            }
        }

        // ════════════════════════════════════════════════════════════
        // 유틸리티
        // ════════════════════════════════════════════════════════════
        private void AppendLog(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (this.InvokeRequired) { this.BeginInvoke(new Action<string>(AppendLog), text); return; }

            string clean = Regex.Replace(text, @"\x1b\[[0-9;]*[mGKHF]", "");
            clean = Regex.Replace(clean, @"[\x08\r]", "");
            rtbLog.AppendText(clean + Environment.NewLine);
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.ScrollToCaret();

            // ★ 에포크 시작 파싱
            var epochMatch = Regex.Match(clean, @"Epoch\s+(\d+)/(\d+)");
            if (epochMatch.Success)
            {
                _currentEpoch = int.Parse(epochMatch.Groups[1].Value);
                _totalEpoch = int.Parse(epochMatch.Groups[2].Value);
                UpdateTrainStatusLabels();
                return;
            }

            // ★ Early Stopping 감지
            if (Regex.IsMatch(clean, @"Epoch\s+\d+.*val_loss did not improve"))
                _earlyStopDetected = true;
            if (Regex.IsMatch(clean, @"Epoch\s+\d+.*val_loss improved"))
                _earlyStopDetected = false;

            // ★ 에포크 완료 파싱
            var epochEndMatch = Regex.Match(clean, @"^\s*\d+/\d+\s*\[=+\].*?\bloss:\s*([\d.eE+\-]+)");
            if (epochEndMatch.Success && clean.Contains("val_loss:"))
            {
                double TryParse(string pattern)
                {
                    var m = Regex.Match(clean, pattern);
                    if (m.Success && double.TryParse(m.Groups[1].Value,
                        System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out var v)) return v;
                    return double.NaN;
                }

                double newLoss = TryParse(@"(?<!_)\bloss:\s*([\d.eE+\-]+)");
                double newValLoss = TryParse(@"val_loss:\s*([\d.eE+\-]+)");
                double newN0 = TryParse(@"n_outputs0_loss:\s*([\d.eE+\-]+)");
                double newValN0 = TryParse(@"val_n_outputs0_loss:\s*([\d.eE+\-]+)");
                double newN1 = TryParse(@"n_outputs1_loss:\s*([\d.eE+\-]+)");
                double newValN1 = TryParse(@"val_n_outputs1_loss:\s*([\d.eE+\-]+)");

                if (!double.IsNaN(newLoss) && !double.IsNaN(newValLoss))
                {
                    _lastLoss = newLoss;
                    _lastValLoss = newValLoss;
                    if (double.IsNaN(_bestLoss) || newLoss < _bestLoss) _bestLoss = newLoss;
                    if (double.IsNaN(_bestValLoss) || newValLoss < _bestValLoss) _bestValLoss = newValLoss;

                    _lossHistory.Add(newLoss);
                    _valLossHistory.Add(newValLoss);
                    if (!double.IsNaN(newN0)) _n0LossHistory.Add(newN0);
                    if (!double.IsNaN(newValN0)) _valN0LossHistory.Add(newValN0);
                    if (!double.IsNaN(newN1)) _n1LossHistory.Add(newN1);
                    if (!double.IsNaN(newValN1)) _valN1LossHistory.Add(newValN1);

                    UpdateTrainStatusLabels();
                    UpdateLiveGraph();
                    return;
                }
            }

            // ★ 배치 진행 중 표시
            var batchMatch = Regex.Match(clean, @"^\s*(\d+)/(\d+)\s*\[.*\].*loss:\s*([\d.]+)");
            if (batchMatch.Success && !epochEndMatch.Success)
            {
                int curBatch = int.Parse(batchMatch.Groups[1].Value);
                int totalBatch = int.Parse(batchMatch.Groups[2].Value);
                if (lblEpochStatus != null)
                    lblEpochStatus.Text = $"에포크: {_currentEpoch} / {_totalEpoch}  ({curBatch}/{totalBatch} 배치)";
            }
        }

        private string ConvertToWslPath(string path)
        {
            path = path.Trim();
            if (path.StartsWith("\\\\wsl.localhost\\"))
            {
                int nextSlash = path.IndexOf('\\', 16);
                if (nextSlash != -1)
                    return path.Substring(nextSlash).Replace("\\", "/");
            }
            if (path.Length >= 2 && path[1] == ':')
                return "/mnt/" + path[0].ToString().ToLower() + path[2..].Replace("\\", "/");
            return path;
        }

        private string ConvertToWindowsPath(string winPath) => winPath;

        private string ConvertWslToWindowsPath(string wslPath)
        {
            if (string.IsNullOrEmpty(wslPath)) return "";
            if (wslPath.StartsWith("/mnt/"))
            {
                char drive = wslPath[5];
                return char.ToUpper(drive) + ":" + wslPath.Substring(6).Replace("/", "\\");
            }
            return @"\\wsl.localhost\Ubuntu-22.04" + wslPath.Replace("/", "\\");
        }

        private void SaveTrainRecordToDb(bool completed)
        {
            if (string.IsNullOrEmpty(_currentModelName) || string.IsNullOrEmpty(_currentMycarPath)) return;

            string lossJson = "[" + string.Join(",", _lossHistory.Select(v => v.ToString("F6", System.Globalization.CultureInfo.InvariantCulture))) + "]";
            string valLossJson = "[" + string.Join(",", _valLossHistory.Select(v => v.ToString("F6", System.Globalization.CultureInfo.InvariantCulture))) + "]";
            string n0Json = "[" + string.Join(",", _n0LossHistory.Select(v => v.ToString("F6", System.Globalization.CultureInfo.InvariantCulture))) + "]";
            string valN0Json = "[" + string.Join(",", _valN0LossHistory.Select(v => v.ToString("F6", System.Globalization.CultureInfo.InvariantCulture))) + "]";
            string n1Json = "[" + string.Join(",", _n1LossHistory.Select(v => v.ToString("F6", System.Globalization.CultureInfo.InvariantCulture))) + "]";
            string valN1Json = "[" + string.Join(",", _valN1LossHistory.Select(v => v.ToString("F6", System.Globalization.CultureInfo.InvariantCulture))) + "]";

            string tempScript = Path.Combine(Path.GetTempPath(), "save_train_record.py");
            File.WriteAllText(tempScript,
                $"import json, os\n" +
                $"db_path = '{_currentMycarPath}/models/database.json'\n" +
                $"data = []\n" +
                $"if os.path.exists(db_path):\n" +
                $"    try: data = json.load(open(db_path, encoding='utf-8'))\n" +
                $"    except: data = []\n" +
                $"existing = [x for x in data if x.get('Name') == '{_currentModelName}']\n" +
                $"if existing:\n" +
                $"    record = existing[-1]\n" +
                $"else:\n" +
                $"    record = {{}}\n" +
                $"    data.append(record)\n" +
                $"record['Name'] = '{_currentModelName}'\n" +
                $"record['Type'] = '{_currentModelType}'\n" +
                $"record['Tubs'] = '{_currentTubArg}'\n" +
                $"record['Comment'] = '{_currentComment}'\n" +
                $"record['StartTime'] = '{_currentStartTime}'\n" +
                $"record['Completed'] = {(completed ? "True" : "False")}\n" +
                $"record['History'] = {{}}\n" +
                $"record['History']['loss'] = {lossJson}\n" +
                $"record['History']['val_loss'] = {valLossJson}\n" +
                $"record['History']['n_outputs0_loss'] = {n0Json}\n" +
                $"record['History']['val_n_outputs0_loss'] = {valN0Json}\n" +
                $"record['History']['n_outputs1_loss'] = {n1Json}\n" +
                $"record['History']['val_n_outputs1_loss'] = {valN1Json}\n" +
                $"json.dump(data, open(db_path, 'w', encoding='utf-8'), indent=4, ensure_ascii=False)\n",
                new System.Text.UTF8Encoding(false)
            );

            string wslScript = ConvertToWslPath(tempScript);
            RunWsl($"python3 {wslScript}", onSuccess: () =>
            {
                string mycarWinPath = ConvertWslToWindowsPath(_currentMycarPath);
                if (!string.IsNullOrEmpty(mycarWinPath))
                    RefreshModelList(mycarWinPath);
            });
        }

        private void OpenLiveGraphForm(string modelName, int totalEpoch)
        {
            if (_liveGraphForm != null && !_liveGraphForm.IsDisposed)
            {
                _liveGraphForm.Close();
                _liveGraphForm = null;
            }

            _liveGraphForm = new Form
            {
                Text = $"실시간 학습 그래프 — {modelName}",
                Size = new Size(800, 750),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(20, 20, 20)
            };

            var lblDesc = new Label
            {
                Text = "그래프 값이 낮을수록 정확한 모델 | 두 선의 차이가 크면 과적합\n훈련 손실(loss): 학습 데이터 오차 | 검증 손실(val_loss): 미학습 데이터 오차",
                Dock = DockStyle.Top,
                Height = 40,
                ForeColor = Color.FromArgb(200, 200, 100),
                BackColor = Color.FromArgb(40, 40, 20),
                Font = new Font("맑은 고딕", 8.5F),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 8, 0)
            };

            var livePanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                BackColor = Color.FromArgb(20, 20, 20)
            };
            for (int i = 0; i < 3; i++)
                livePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));

            var chartTitles = new[] { "전체 오차 (loss)", "핸들 방향 오차 (n_outputs0)", "속도 조절 오차 (n_outputs1)" };
            var chartColors = new[]
            {
                new[] { Color.Cyan, Color.OrangeRed },
                new[] { Color.Yellow, Color.DodgerBlue },
                new[] { Color.LimeGreen, Color.Orange }
            };
            var seriesNames = new[]
            {
                new[] { "loss", "val_loss" },
                new[] { "n_outputs0_loss", "val_n_outputs0_loss" },
                new[] { "n_outputs1_loss", "val_n_outputs1_loss" }
            };
            var legendLabels = new[]
            {
                new[] { "훈련 손실 (loss)", "검증 손실 (val_loss)" },
                new[] { "핸들 훈련 오차", "핸들 검증 오차" },
                new[] { "속도 훈련 오차", "속도 검증 오차" }
            };

            var liveCharts = new Chart[3];
            for (int g = 0; g < 3; g++)
            {
                var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.FromArgb(30, 30, 30) };
                var ca = new ChartArea
                {
                    BackColor = Color.FromArgb(20, 20, 20),
                    AxisX = { LabelStyle = { ForeColor = Color.Gray }, LineColor = Color.Gray, MajorGrid = { LineColor = Color.FromArgb(50, 50, 50) } },
                    AxisY = { LabelStyle = { ForeColor = Color.Gray }, LineColor = Color.Gray, MajorGrid = { LineColor = Color.FromArgb(50, 50, 50) } }
                };
                chart.ChartAreas.Add(ca);
                chart.Titles.Add(new Title { Text = chartTitles[g], ForeColor = Color.LightGray, Font = new Font("맑은 고딕", 9F, FontStyle.Bold) });
                chart.Legends.Add(new Legend { BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.LightGray });
                for (int s = 0; s < 2; s++)
                {
                    chart.Series.Add(new Series
                    {
                        Name = seriesNames[g][s],
                        Color = chartColors[g][s],
                        ChartType = SeriesChartType.Line,
                        BorderWidth = 2,
                        LegendText = legendLabels[g][s]
                    });
                }
                liveCharts[g] = chart;
                livePanel.Controls.Add(chart, 0, g);
            }

            _liveChart = liveCharts[0];
            _liveGraphForm.Tag = liveCharts;

            var btnClose = new Button
            {
                Text = "닫기",
                Dock = DockStyle.Bottom,
                Height = 36,
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.FromArgb(200, 200, 200),
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, ev) => _liveGraphForm.Close();
            _liveGraphForm.Controls.Add(livePanel);
            _liveGraphForm.Controls.Add(lblDesc);
            _liveGraphForm.Controls.Add(btnClose);
            _liveGraphForm.Show();
        }

        private void UpdateLiveGraph()
        {
            if (_liveChart == null || _liveGraphForm == null || _liveGraphForm.IsDisposed) return;
            try
            {
                var charts = _liveGraphForm.Tag as Chart[];
                if (charts == null) return;

                charts[0].Series["loss"].Points.Clear();
                for (int i = 0; i < _lossHistory.Count; i++)
                    charts[0].Series["loss"].Points.AddXY(i + 1, _lossHistory[i]);
                charts[0].Series["val_loss"].Points.Clear();
                for (int i = 0; i < _valLossHistory.Count; i++)
                    charts[0].Series["val_loss"].Points.AddXY(i + 1, _valLossHistory[i]);
                if (charts[0].Titles.Count > 0)
                    charts[0].Titles[0].Text = $"전체 오차 (loss) — {_currentEpoch} / {_totalEpoch} 에포크";

                if (_n0LossHistory.Count > 0)
                {
                    charts[1].Series["n_outputs0_loss"].Points.Clear();
                    for (int i = 0; i < _n0LossHistory.Count; i++)
                        charts[1].Series["n_outputs0_loss"].Points.AddXY(i + 1, _n0LossHistory[i]);
                    charts[1].Series["val_n_outputs0_loss"].Points.Clear();
                    for (int i = 0; i < _valN0LossHistory.Count; i++)
                        charts[1].Series["val_n_outputs0_loss"].Points.AddXY(i + 1, _valN0LossHistory[i]);
                }
                if (charts[1].Titles.Count > 0)
                    charts[1].Titles[0].Text = $"핸들 방향 오차 (n_outputs0) — {_currentEpoch} / {_totalEpoch} 에포크";

                if (_n1LossHistory.Count > 0)
                {
                    charts[2].Series["n_outputs1_loss"].Points.Clear();
                    for (int i = 0; i < _n1LossHistory.Count; i++)
                        charts[2].Series["n_outputs1_loss"].Points.AddXY(i + 1, _n1LossHistory[i]);
                    charts[2].Series["val_n_outputs1_loss"].Points.Clear();
                    for (int i = 0; i < _valN1LossHistory.Count; i++)
                        charts[2].Series["val_n_outputs1_loss"].Points.AddXY(i + 1, _valN1LossHistory[i]);
                }
                if (charts[2].Titles.Count > 0)
                    charts[2].Titles[0].Text = $"속도 조절 오차 (n_outputs1) — {_currentEpoch} / {_totalEpoch} 에포크";
            }
            catch { }
        }

        private void UpdateTrainStatusLabels()
        {
            if (lblEpochStatus == null || lblLossStatus == null || lblValLossStatus == null) return;
            lblEpochStatus.Text = $"에포크: {_currentEpoch} / {_totalEpoch}";

            // 훈련 손실
            if (double.IsNaN(_lastLoss))
            {
                lblLossStatus.Text = "훈련 손실: -";
                lblLossStatus.ForeColor = SystemColors.ControlText;
            }
            else
            {
                string delta = "";
                Color color = SystemColors.ControlText;
                if (!double.IsNaN(_bestLoss))
                {
                    double diff = _lastLoss - _bestLoss;
                    if (diff <= 0)
                    {
                        delta = " (최저)";
                        color = Color.Green;
                    }
                    else
                    {
                        delta = $" (+{diff:0.0000})";
                        color = Color.Red;
                    }
                }
                lblLossStatus.Text = $"훈련 손실: {_lastLoss:F4}{delta}";
                lblLossStatus.ForeColor = color;
            }

            // 검증 손실
            if (double.IsNaN(_lastValLoss))
            {
                lblValLossStatus.Text = "검증 손실: -";
                lblValLossStatus.ForeColor = SystemColors.ControlText;
            }
            else
            {
                string delta = "";
                Color color = SystemColors.ControlText;
                if (!double.IsNaN(_bestValLoss))
                {
                    double diff = _lastValLoss - _bestValLoss;
                    if (diff <= 0)
                    {
                        delta = " (최저)";
                        color = Color.Green;
                    }
                    else
                    {
                        delta = $" (+{diff:0.0000})";
                        color = Color.Red;
                    }
                }
                lblValLossStatus.Text = $"검증 손실: {_lastValLoss:F4}{delta}";
                lblValLossStatus.ForeColor = color;
            }
        }

        private async void RunWsl(string bashCmd, Action onSuccess = null)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c wsl bash -c \"{bashCmd.Replace("\"", "\\\"")}\"",
                WorkingDirectory = "C:\\",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            try
            {
                var p = Process.Start(psi);
                await Task.Run(() => p.WaitForExit());
                onSuccess?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show("WSL 오류: " + ex.Message);
            }
        }

        private async void RunWslWithOutput(string bashCmd, Action<string> onOutput)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "wsl",
                Arguments = $"bash -c \"{bashCmd}\"",
                WorkingDirectory = "C:\\",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            try
            {
                var p = Process.Start(psi);
                string output = await Task.Run(() =>
                {
                    string o = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    return o;
                });
                onOutput?.Invoke(string.IsNullOrWhiteSpace(output)
                    ? "(출력 없음 — 모델 경로를 확인하세요)" : output);
            }
            catch (Exception ex)
            {
                MessageBox.Show("WSL 오류: " + ex.Message);
            }
        }

        public void InitModelWatcher(string mycarPath)
        {
            string winModelsPath = mycarPath + "\\models";
            if (!Directory.Exists(winModelsPath)) return;

            _modelWatcher?.Dispose();
            try
            {
                _modelWatcher = new FileSystemWatcher(winModelsPath)
                {
                    Filter = "*.*",
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                    IncludeSubdirectories = false,
                    EnableRaisingEvents = true
                };
                _modelWatcher.Created += (s, e) => RefreshModelList(mycarPath);
                _modelWatcher.Deleted += (s, e) => RefreshModelList(mycarPath);
            }
            catch { }

            var timer = new System.Windows.Forms.Timer { Interval = 5000 };
            timer.Tick += (s, e) => RefreshModelList(mycarPath);
            timer.Start();

            RefreshModelList(mycarPath);
        }

        private void RefreshModelList(string mycarPath)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)(() => RefreshModelList(mycarPath)));
                return;
            }

            string winPath = Path.Combine(mycarPath, "models");
            if (!Directory.Exists(winPath)) return;

            var dbDict = new Dictionary<string, (string pilot, string type, string tubs, string comment)>();
            string dbPath = Path.Combine(winPath, "database.json");
            if (File.Exists(dbPath))
            {
                try
                {
                    string json = File.ReadAllText(dbPath);
                    var db = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(json);
                    if (db != null)
                    {
                        foreach (var item in db)
                        {
                            string name = item.ContainsKey("Name") ? item["Name"].GetString() ?? "" : "";
                            string pilot = item.ContainsKey("Pilot") ? item["Pilot"].GetString() ?? "" : "";
                            string rawType = item.ContainsKey("Type") ? item["Type"].GetString() ?? "" : "";
                            var typeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                            {
                                {"KerasLinear","linear"}, {"KerasCategorical","categorical"},
                                {"KerasInferred","inferred"}, {"KerasRNN","rnn"},
                                {"KerasMemory","memory"}, {"KerasBehavioral","behavior"},
                                {"KerasLocalizer","localizer"}, {"SquashedModelWithMemory","sq_mem"}
                            };
                            string baseType = rawType.Contains("-") ? rawType.Split('-')[0].Trim() : rawType;
                            string type = typeMap.ContainsKey(baseType) ? typeMap[baseType] : rawType;
                            string tubs = item.ContainsKey("Tubs") ? item["Tubs"].GetString() ?? "" : "";
                            string comment = item.ContainsKey("Comment") ? item["Comment"].GetString() ?? "" : "";
                            if (!string.IsNullOrEmpty(name))
                                dbDict[name] = (pilot, type, tubs, comment);
                        }
                    }
                }
                catch { }
            }

            var extensions = new[] { "*.h5", "*.tflite", "*.savedmodel", "*.keras", "*.pkl" };
            var files = extensions
                .SelectMany(ext => Directory.GetFiles(winPath, ext))
                .OrderBy(f => f)
                .ToArray();

            var newFiles = files.Select(f => Path.GetFileName(f)).ToList();
            var currentFiles = dgvTrains.Rows
                .Cast<DataGridViewRow>()
                .Select(r => r.Cells[1].Value?.ToString() ?? "")
                .ToList();

            if (currentFiles.SequenceEqual(newFiles))
            {
                // 파일 목록 동일 → DB 데이터만 업데이트 (체크박스 점프 버그 방지)
                foreach (DataGridViewRow row in dgvTrains.Rows)
                {
                    string name = row.Cells[1].Value?.ToString() ?? "";
                    string baseName = Path.GetFileNameWithoutExtension(name);
                    if (dbDict.ContainsKey(baseName))
                    {
                        row.Cells[2].Value = dbDict[baseName].pilot;
                        row.Cells[3].Value = dbDict[baseName].type;
                        row.Cells[4].Value = dbDict[baseName].tubs;
                        row.Cells[7].Value = dbDict[baseName].comment;
                    }
                }
                return;
            }

            dgvTrains.Rows.Clear();
            foreach (var file in files)
            {
                string name = Path.GetFileName(file);
                string baseName = Path.GetFileNameWithoutExtension(name);
                var info = new FileInfo(file);
                string pilot = "", type = "", tubs = "", comment = "";
                if (dbDict.ContainsKey(baseName))
                {
                    pilot = dbDict[baseName].pilot;
                    type = dbDict[baseName].type;
                    tubs = dbDict[baseName].tubs;
                    comment = dbDict[baseName].comment;
                }
                dgvTrains.Rows.Add(false, name, pilot, type, tubs,
                    info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"), "", comment);
            }
        }

        private DataGridViewRow GetCheckedRow()
        {
            foreach (DataGridViewRow row in dgvTrains.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                    return row;
            }
            return null;
        }

        private void pnlScroll_Paint(object sender, PaintEventArgs e) { }

        private void lstPresets_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font,
                new SolidBrush(e.ForeColor), e.Bounds);
            e.Graphics.DrawLine(Pens.LightGray,
                e.Bounds.Left, e.Bounds.Bottom - 1,
                e.Bounds.Right, e.Bounds.Bottom - 1);
        }
    }
}