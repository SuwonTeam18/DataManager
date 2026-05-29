using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace DonkeyUi
{
    public partial class ucCarConnector : UserControl
    {
        private Process _driveProcess = null;
        private string _carIp = "";
        private bool _isChecking = false;
        private bool _isDriving = false;
        private bool _lastConnected = false;

        public ucCarConnector()
        {
            InitializeComponent();

            btnCreateNewFolder.Click += BtnCreateNewFolder_Click;
            btnPullTubData.Click += BtnPullTubData_Click;
            btnPushPilots.Click += BtnPushPilots_Click;
            btnSyncH5.Click += BtnSyncH5_Click;
            btnSyncSavedModel.Click += BtnSyncSavedModel_Click;
            btnSyncTFlite.Click += BtnSyncTFlite_Click;
            btnDrive.Click += BtnDrive_Click;
            btnStop.Click += BtnStop_Click;

            this.Load += (s, e) => LoadConfigFromMyconfig();

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 5000;
            timer.Tick += (s, e) => CheckConnectionAsync(false);
            timer.Start();
        }

        private void LoadConfigFromMyconfig()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "wsl",
                    Arguments = "bash -c \"grep -E 'PI_USERNAME|PI_HOSTNAME' ~/mycar/myconfig.py | grep -v '^#'\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var p = Process.Start(psi))
                {
                    string output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();

                    string username = "pi";
                    string hostname = "";

                    foreach (string line in output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (line.Contains("PI_USERNAME"))
                            username = ExtractValue(line);
                        else if (line.Contains("PI_HOSTNAME"))
                            hostname = ExtractValue(line);
                    }

                    if (!string.IsNullOrEmpty(hostname))
                    {
                        _carIp = $"{username}@{hostname}";
                        txtCarDirectory.Text = _carIp;
                        CheckConnectionAsync(true);
                    }
                    else
                    {
                        lblConnectionValue.Text = "Config 없음";
                        lblConnectionValue.ForeColor = Color.Orange;
                    }
                }
            }
            catch
            {
                lblConnectionValue.Text = "Config 읽기 실패";
                lblConnectionValue.ForeColor = Color.Red;
            }
        }

        private string ExtractValue(string line)
        {
            var match = Regex.Match(line, "=\\s*['\"](.+)['\"]");
            return match.Success ? match.Groups[1].Value : "";
        }

        private async void CheckConnectionAsync(bool showChecking)
        {
            if (_isChecking) return;
            _isChecking = true;

            if (showChecking && !_isDriving)
            {
                lblConnectionValue.Text = "Checking...";
                lblConnectionValue.ForeColor = Color.Orange;
            }

            bool isConnected = await Task.Run(() =>
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "wsl",
                        Arguments = $"bash -c \"ssh -o ConnectTimeout=2 -o BatchMode=yes {_carIp} exit 0 2>/dev/null && echo connected || echo disconnected\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using (var p = Process.Start(psi))
                    {
                        return p.StandardOutput.ReadToEnd().Trim() == "connected";
                    }
                }
                catch { return false; }
            });

            // 주행 중 연결 끊기면 자동 Stop
            if (_isDriving && !isConnected)
            {
                this.Invoke((Action)(() =>
                {
                    StopDriving();
                    lblConnectionValue.Text = "disconnected";
                    lblConnectionValue.ForeColor = Color.Red;
                    _lastConnected = false;
                }));
                _isChecking = false;
                return;
            }

            if (!_isDriving)
            {
                if (isConnected != _lastConnected || showChecking)
                {
                    lblConnectionValue.Text = isConnected ? "connected" : "disconnected";
                    lblConnectionValue.ForeColor = isConnected ? Color.LimeGreen : Color.Red;
                    _lastConnected = isConnected;
                }

                if (isConnected)
                {
                    RefreshTubList();
                    RefreshModelList(); // 모델 목록도 갱신
                }
            }

            _isChecking = false;
        }

        private async void RefreshTubList()
        {
            await Task.Run(() =>
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "wsl",
                        Arguments = $"bash -c \"ssh -o ConnectTimeout=2 -o BatchMode=yes {_carIp} 'ls ~/mycar/data/' 2>/dev/null\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var p = Process.Start(psi))
                    {
                        string output = p.StandardOutput.ReadToEnd();
                        p.WaitForExit();

                        var folders = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                        this.Invoke((Action)(() =>
                        {
                            string current = cmbSelectTub.Text;
                            cmbSelectTub.Items.Clear();
                            foreach (var folder in folders)
                            {
                                if (!string.IsNullOrWhiteSpace(folder))
                                    cmbSelectTub.Items.Add(folder.Trim());
                            }
                            if (cmbSelectTub.Items.Contains(current))
                                cmbSelectTub.Text = current;
                            else if (cmbSelectTub.Items.Count > 0)
                                cmbSelectTub.SelectedIndex = 0;
                        }));
                    }
                }
                catch { }
            });
        }

        // 차량의 models 폴더 목록을 읽어와서 Drive 라벨에 표시
        private async void RefreshModelList()
        {
            await Task.Run(() =>
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "wsl",
                        Arguments = $"bash -c \"ssh -o ConnectTimeout=2 -o BatchMode=yes {_carIp} 'ls ~/mycar/models/*.h5 2>/dev/null | xargs -I{{}} basename {{}}' 2>/dev/null\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var p = Process.Start(psi))
                    {
                        string output = p.StandardOutput.ReadToEnd();
                        p.WaitForExit();

                        var models = output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                        this.Invoke((Action)(() =>
                        {
                            if (models.Length > 0)
                                lblDrivePilot.Text = models[0].Trim(); // 첫 번째 모델 표시
                        }));
                    }
                }
                catch { }
            });
        }

        private void RestoreConnectedAfterDelay()
        {
            var restoreTimer = new System.Windows.Forms.Timer();
            restoreTimer.Interval = 3000;
            restoreTimer.Tick += (t, te) =>
            {
                restoreTimer.Stop();
                if (!_isDriving)
                {
                    lblConnectionValue.Text = _lastConnected ? "connected" : "disconnected";
                    lblConnectionValue.ForeColor = _lastConnected ? Color.LimeGreen : Color.Red;
                }
            };
            restoreTimer.Start();
        }

        private void StopDriving()
        {
            if (_driveProcess != null && !_driveProcess.HasExited)
            {
                _driveProcess.Kill();
                _driveProcess = null;
            }

            _isDriving = false;
            btnDrive.Enabled = true;
            btnStop.Enabled = false;
            lblDrivePilot.Text = "No pilot";
        }

        private async void BtnPullTubData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_carIp)) return;

            if (!_lastConnected)
            {
                MessageBox.Show("차량이 연결되어 있지 않습니다.");
                return;
            }

            string selectedTub = cmbSelectTub.Text.Trim();
            if (string.IsNullOrEmpty(selectedTub))
            {
                MessageBox.Show("가져올 데이터 폴더를 선택하세요.");
                return;
            }

            btnPullTubData.Enabled = false;
            prgPullStatus.Value = 0;

            var progressTimer = new System.Windows.Forms.Timer();
            progressTimer.Interval = 300;
            progressTimer.Tick += (t, te) => {
                if (prgPullStatus.Value < 90)
                    prgPullStatus.Value += 3;
            };
            progressTimer.Start();

            bool success = await Task.Run(() =>
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "wsl",
                        Arguments = $"bash -c \"scp -o ConnectTimeout=5 -r {_carIp}:~/mycar/data/{selectedTub} ~/mycar/data/\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    var p = Process.Start(psi);
                    p.WaitForExit();
                    return p.ExitCode == 0;
                }
                catch { return false; }
            });

            progressTimer.Stop();
            prgPullStatus.Value = 100;
            btnPullTubData.Enabled = true;

            if (success)
            {
                lblConnectionValue.Text = $"{selectedTub} 가져오기 완료";
                lblConnectionValue.ForeColor = Color.LimeGreen;
                RestoreConnectedAfterDelay();
            }
            else
            {
                MessageBox.Show("데이터 가져오기 실패. 연결을 확인하세요.");
            }
        }

        // Push pilots: PC의 models 폴더 전체를 차량으로 전송
        private async void BtnPushPilots_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_carIp)) return;

            if (!_lastConnected)
            {
                MessageBox.Show("차량이 연결되어 있지 않습니다.");
                return;
            }

            btnPushPilots.Enabled = false;
            prgPushStatus.Value = 0;

            var progressTimer = new System.Windows.Forms.Timer();
            progressTimer.Interval = 300;
            progressTimer.Tick += (t, te) => {
                if (prgPushStatus.Value < 90)
                    prgPushStatus.Value += 3;
            };
            progressTimer.Start();

            bool success = await Task.Run(() =>
            {
                try
                {
                    // models 폴더 안의 모든 파일을 차량으로 전송
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "wsl",
                        Arguments = $"bash -c \"scp -o ConnectTimeout=5 -r ~/mycar/models/ {_carIp}:~/mycar/models/\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    var p = Process.Start(psi);
                    p.WaitForExit();
                    return p.ExitCode == 0;
                }
                catch { return false; }
            });

            progressTimer.Stop();
            prgPushStatus.Value = 100;
            btnPushPilots.Enabled = true;

            if (success)
            {
                lblConnectionValue.Text = "모델 전송 완료";
                lblConnectionValue.ForeColor = Color.LimeGreen;
                RestoreConnectedAfterDelay();
            }
            else
            {
                MessageBox.Show("모델 전송 실패. 연결을 확인하세요.");
            }
        }

        private void BtnSyncH5_Click(object sender, EventArgs e) => SyncFile("h5");
        private void BtnSyncSavedModel_Click(object sender, EventArgs e) => SyncFile("savedmodel");
        private void BtnSyncTFlite_Click(object sender, EventArgs e) => SyncFile("tflite");

        // Sync: 확장자 기반으로 해당 형식 파일 전체 전송
        private async void SyncFile(string ext)
        {
            if (!_lastConnected)
            {
                MessageBox.Show("차량이 연결되어 있지 않습니다.");
                return;
            }

            bool isDirectory = (ext == "savedmodel");

            bool success = await Task.Run(() =>
            {
                try
                {
                    string args;
                    if (isDirectory)
                    {
                        // savedmodel은 폴더 형식 - 폴더 전체 전송
                        args = $"bash -c \"scp -o ConnectTimeout=5 -r ~/mycar/models/*.savedmodel {_carIp}:~/mycar/models/ 2>/dev/null\"";
                    }
                    else
                    {
                        // h5, tflite는 해당 확장자 파일 전체 전송
                        args = $"bash -c \"scp -o ConnectTimeout=5 ~/mycar/models/*.{ext} {_carIp}:~/mycar/models/ 2>/dev/null\"";
                    }

                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "wsl",
                        Arguments = args,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    var p = Process.Start(psi);
                    p.WaitForExit();
                    return p.ExitCode == 0;
                }
                catch { return false; }
            });

            if (success)
            {
                lblConnectionValue.Text = $"{ext} 전송 완료";
                lblConnectionValue.ForeColor = Color.LimeGreen;
                RestoreConnectedAfterDelay();
            }
            else
            {
                MessageBox.Show($"{ext} 전송 실패. 연결을 확인하세요.");
            }
        }

        private void BtnDrive_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_carIp)) return;

            if (!_lastConnected)
            {
                MessageBox.Show("차량이 연결되어 있지 않습니다.");
                return;
            }

            // lblDriveModel에서 모델 타입, lblDrivePilot에서 모델 이름 가져오기
            string modelType = lblDriveModel.Text.Trim();
            string modelName = lblDrivePilot.Text.Trim();

            if (string.IsNullOrEmpty(modelName) || modelName == "No pilot")
            {
                MessageBox.Show("차량에 모델이 없습니다. 먼저 모델을 전송하세요.");
                return;
            }

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "ssh",
                Arguments = $"{_carIp} \"cd ~/mycar && ~/miniconda3/envs/e2e_env/bin/python manage.py drive --model ./models/{modelName} --type {modelType}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _driveProcess = new Process { StartInfo = psi, EnableRaisingEvents = true };
            _driveProcess.OutputDataReceived += (s, ev) => { if (ev.Data != null) AppendDriveLog(ev.Data); };
            _driveProcess.ErrorDataReceived += (s, ev) => { if (ev.Data != null) AppendDriveLog(ev.Data); };

            try
            {
                _driveProcess.Start();
                _driveProcess.BeginOutputReadLine();
                _driveProcess.BeginErrorReadLine();

                _isDriving = true;
                btnDrive.Enabled = false;
                btnStop.Enabled = true;
                lblConnectionValue.Text = "Driving...";
                lblConnectionValue.ForeColor = Color.LimeGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show("주행 시작 오류: " + ex.Message);
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "ssh",
                    Arguments = $"{_carIp} \"pkill -f manage.py\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process.Start(psi).WaitForExit();
            });

            StopDriving();
            lblConnectionValue.Text = "Stopped";
            lblConnectionValue.ForeColor = Color.Gray;
            _lastConnected = false;
        }

        private void AppendDriveLog(string data)
        {
            if (string.IsNullOrEmpty(data)) return;
            this.BeginInvoke((Action)(() =>
            {
                System.Diagnostics.Debug.WriteLine($">>>>> [VEHICLE_DATA]: {data}");
            }));
        }

        private void BtnCreateNewFolder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_carIp) || !_lastConnected)
            {
                MessageBox.Show("차량이 연결되어 있지 않습니다.");
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    // 차량에서 기존 data 폴더 목록 가져오기
                    ProcessStartInfo psiList = new ProcessStartInfo
                    {
                        FileName = "wsl",
                        Arguments = $"bash -c \"ssh -o ConnectTimeout=2 -o BatchMode=yes {_carIp} 'ls ~/mycar/data/' 2>/dev/null\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    string output = "";
                    using (var p = Process.Start(psiList))
                    {
                        output = p.StandardOutput.ReadToEnd();
                        p.WaitForExit();
                    }

                    // 공백 제거하고 HashSet 생성
                    var existing = new System.Collections.Generic.HashSet<string>(
                        output.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(f => f.Trim()));

                    // 폴더 이름 결정 (data, data(1), data(2) ...)
                    string folderName = "data";
                    int count = 1;
                    while (existing.Contains(folderName))
                    {
                        folderName = $"data({count})";
                        count++;
                    }

                    // 괄호 이스케이프 처리
                    string escapedFolder = folderName.Replace("(", "\\(").Replace(")", "\\)");

                    ProcessStartInfo psiMkdir = new ProcessStartInfo
                    {
                        FileName = "wsl",
                        Arguments = $"bash -c \"ssh {_carIp} \\\"mkdir -p ~/mycar/data/{escapedFolder}\\\"\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    var p2 = Process.Start(psiMkdir);
                    p2.WaitForExit();

                    this.Invoke((Action)(() =>
                    {
                        if (p2.ExitCode == 0)
                        {
                            lblConnectionValue.Text = $"{folderName} 생성 완료";
                            lblConnectionValue.ForeColor = Color.LimeGreen;
                            RefreshTubList();
                            _lastConnected = false;
                            RestoreConnectedAfterDelay();
                        }
                        else
                        {
                            MessageBox.Show("폴더 생성 실패. 연결을 확인하세요.");
                        }
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke((Action)(() => MessageBox.Show("오류: " + ex.Message)));
                }
            });
        }
    }
}