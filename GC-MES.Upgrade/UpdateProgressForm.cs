using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace GC_MES.Upgrade
{
    public partial class UpdateProgressForm : Form
    {
        private readonly UpdateManager _updateManager;
        private readonly UpdateInfo _updateInfo;
        private string _updatePackagePath;
        private bool _isDownloading = false;
        private bool _isInstalling = false;
        
        public UpdateProgressForm(UpdateManager updateManager, UpdateInfo updateInfo)
        {
            InitializeComponent();
            
            _updateManager = updateManager;
            _updateInfo = updateInfo;
            
            // 设置初始UI
            SetupInitialUI();
            
            // 注册事件
            RegisterEvents();
        }
        
        private void SetupInitialUI()
        {
            // 设置版本信息
            lblCurrentVersion.Text = $"当前版本: {_updateManager.CurrentVersion}";
            lblNewVersion.Text = $"新版本: {_updateInfo.Version}";
            
            // 设置更新说明
            txtReleaseNotes.Text = _updateInfo.ReleaseNotes ?? "暂无更新说明";
            
            // 设置其他信息
            lblReleaseDate.Text = $"发布日期: {_updateInfo.ReleaseDate:yyyy-MM-dd}";
            lblFileSize.Text = $"文件大小: {FormatFileSize(_updateInfo.FileSize)}";
            
            // 设置进度条
            progressBar.Value = 0;
            progressBar.Maximum = 100;
            progressBar.Style = ProgressBarStyle.Blocks;
            
            // 设置按钮状态
            btnInstall.Enabled = false;
            btnDownload.Enabled = true;
            btnCancel.Enabled = true;
        }
        
        private void RegisterEvents()
        {
            _updateManager.DownloadProgressChanged += UpdateManager_DownloadProgressChanged;
            _updateManager.DownloadCompleted += UpdateManager_DownloadCompleted;
            _updateManager.InstallProgressChanged += UpdateManager_InstallProgressChanged;
            _updateManager.InstallCompleted += UpdateManager_InstallCompleted;
            _updateManager.LogMessage += UpdateManager_LogMessage;
        }
        
        private void UnregisterEvents()
        {
            _updateManager.DownloadProgressChanged -= UpdateManager_DownloadProgressChanged;
            _updateManager.DownloadCompleted -= UpdateManager_DownloadCompleted;
            _updateManager.InstallProgressChanged -= UpdateManager_InstallProgressChanged;
            _updateManager.InstallCompleted -= UpdateManager_InstallCompleted;
            _updateManager.LogMessage -= UpdateManager_LogMessage;
        }
        
        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (_isDownloading)
                return;
            
            _isDownloading = true;
            btnDownload.Enabled = false;
            progressBar.Value = 0;
            lblStatus.Text = "正在准备下载...";
            
            try
            {
                var result = await _updateManager.DownloadUpdateAsync(_updateInfo);
                
                if (result.Success)
                {
                    _updatePackagePath = result.FilePath;
                    btnInstall.Enabled = true;
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "下载失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"下载更新时发生错误: {ex.Message}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isDownloading = false;
                btnDownload.Enabled = true;
            }
        }
        
        private async void btnInstall_Click(object sender, EventArgs e)
        {
            if (_isInstalling || string.IsNullOrEmpty(_updatePackagePath))
                return;
            
            if (MessageBox.Show("确定要安装此更新吗？安装过程中应用程序将会关闭，完成后会自动重启。", "安装确认", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            
            _isInstalling = true;
            btnInstall.Enabled = false;
            btnDownload.Enabled = false;
            btnCancel.Enabled = false;
            
            try
            {
                var result = await _updateManager.ApplyUpdateAsync(_updatePackagePath);
                
                if (result.Success)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.ErrorMessage, "安装失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnInstall.Enabled = true;
                    btnCancel.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"安装更新时发生错误: {ex.Message}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnInstall.Enabled = true;
                btnCancel.Enabled = true;
            }
            finally
            {
                _isInstalling = false;
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_isDownloading)
            {
                if (MessageBox.Show("确定要取消更新下载吗？", "取消确认", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _updateManager.Cancel();
                }
                return;
            }
            
            DialogResult = DialogResult.Cancel;
            Close();
        }
        
        private void UpdateProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 如果正在安装，不允许关闭窗口
            if (_isInstalling)
            {
                e.Cancel = true;
                return;
            }
            
            // 取消下载
            if (_isDownloading)
            {
                _updateManager.Cancel();
            }
            
            // 注销事件
            UnregisterEvents();
        }
        
        #region 事件处理
        
        private void UpdateManager_DownloadProgressChanged(object sender, DownloadProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, DownloadProgressEventArgs>(UpdateManager_DownloadProgressChanged), sender, e);
                return;
            }
            
            if (e.ProgressPercentage >= 0)
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = Math.Min(e.ProgressPercentage, 100);
                lblStatus.Text = $"正在下载... {e.ProgressPercentage}% ({FormatFileSize(e.BytesReceived)} / {FormatFileSize(e.TotalBytesToReceive)})";
            }
            else
            {
                progressBar.Style = ProgressBarStyle.Marquee;
                lblStatus.Text = $"正在下载... ({FormatFileSize(e.BytesReceived)} 已下载)";
            }
            
            Application.DoEvents();
        }
        
        private void UpdateManager_DownloadCompleted(object sender, DownloadCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, DownloadCompletedEventArgs>(UpdateManager_DownloadCompleted), sender, e);
                return;
            }
            
            if (e.Cancelled)
            {
                lblStatus.Text = "下载已取消";
                progressBar.Value = 0;
                btnDownload.Enabled = true;
                _isDownloading = false;
                return;
            }
            
            if (e.Error != null)
            {
                lblStatus.Text = $"下载错误: {e.Error.Message}";
                progressBar.Value = 0;
                btnDownload.Enabled = true;
                _isDownloading = false;
                return;
            }
            
            lblStatus.Text = "下载完成，准备安装更新";
            progressBar.Value = 100;
            btnInstall.Enabled = true;
            _isDownloading = false;
        }
        
        private void UpdateManager_InstallProgressChanged(object sender, InstallProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, InstallProgressEventArgs>(UpdateManager_InstallProgressChanged), sender, e);
                return;
            }
            
            lblStatus.Text = e.StatusMessage;
            
            switch (e.Phase)
            {
                case InstallPhase.CreatingBackup:
                    lblPhase.Text = "阶段: 创建备份";
                    break;
                case InstallPhase.Extracting:
                    lblPhase.Text = "阶段: 解压更新包";
                    break;
                case InstallPhase.Updating:
                    lblPhase.Text = "阶段: 更新文件";
                    break;
                case InstallPhase.Restarting:
                    lblPhase.Text = "阶段: 准备重启";
                    break;
            }
            
            if (e.ProgressPercentage >= 0)
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = Math.Min(e.ProgressPercentage, 100);
            }
            else
            {
                progressBar.Style = ProgressBarStyle.Marquee;
            }
            
            Application.DoEvents();
        }
        
        private void UpdateManager_InstallCompleted(object sender, InstallCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, InstallCompletedEventArgs>(UpdateManager_InstallCompleted), sender, e);
                return;
            }
            
            if (e.Success)
            {
                lblStatus.Text = "更新已成功安装，应用程序即将重启";
                progressBar.Value = 100;
                
                // 关闭当前窗口并返回OK结果
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                lblStatus.Text = $"安装失败: {e.ErrorMessage}";
                progressBar.Value = 0;
                btnInstall.Enabled = true;
                btnCancel.Enabled = true;
                _isInstalling = false;
            }
        }
        
        private void UpdateManager_LogMessage(object sender, LogEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, LogEventArgs>(UpdateManager_LogMessage), sender, e);
                return;
            }
            
            // 可选：将日志消息添加到日志控件中
            if (txtLog.Text.Length > 10000)
            {
                txtLog.Text = txtLog.Text.Substring(txtLog.Text.Length - 5000);
            }
            
            txtLog.AppendText($"[{e.Timestamp:HH:mm:ss}] [{e.Level}] {e.Message}{Environment.NewLine}");
            txtLog.ScrollToCaret();
        }
        
        #endregion
        
        #region 辅助方法
        
        private string FormatFileSize(long bytes)
        {
            if (bytes < 0)
                return "未知";
                
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;
            double size = bytes;
            
            while (size >= 1024 && i < suffixes.Length - 1)
            {
                size /= 1024;
                i++;
            }
            
            return $"{size:0.##} {suffixes[i]}";
        }
        
        #endregion
    }
} 