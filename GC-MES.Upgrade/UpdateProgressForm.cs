using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GC_MES.Upgrade
{
    public partial class UpdateProgressForm : Form
    {
        private UpdateManager _updateManager;
        private string _updatePackageName;
        private bool _isDownloading = false;
        
        public UpdateProgressForm(UpdateManager updateManager, string serverUrl, Version newVersion)
        {
            InitializeComponent();
            _updateManager = updateManager;
            
            lblCurrentVersion.Text = $"当前版本: {_updateManager.CurrentVersion}";
            lblNewVersion.Text = $"新版本: {newVersion}";
            
            _updatePackageName = $"GC-MES-{newVersion}.zip";
            
            progressBar.Value = 0;
            progressBar.Maximum = 100;
        }
        
        private async void UpdateProgressForm_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "准备下载更新...";
            
            _updateManager.ProgressChanged += UpdateManager_ProgressChanged;
            _updateManager.DownloadCompleted += UpdateManager_DownloadCompleted;
            
            _isDownloading = true;
            
            try
            {
                string downloadUrl = $"{_updateManager.ServerUrl}/{_updatePackageName}";
                await _updateManager.DownloadUpdateAsync(downloadUrl, _updatePackageName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"下载更新时发生错误: {ex.Message}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
        
        private void UpdateManager_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, ProgressChangedEventArgs>(UpdateManager_ProgressChanged), sender, e);
                return;
            }
            
            progressBar.Value = e.ProgressPercentage;
            lblStatus.Text = $"正在下载... {e.ProgressPercentage}%";
        }
        
        private void UpdateManager_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, AsyncCompletedEventArgs>(UpdateManager_DownloadCompleted), sender, e);
                return;
            }
            
            if (e.Cancelled)
            {
                lblStatus.Text = "下载已取消。";
                DialogResult = DialogResult.Cancel;
                return;
            }
            
            if (e.Error != null)
            {
                lblStatus.Text = $"下载错误: {e.Error.Message}";
                MessageBox.Show($"下载更新时发生错误: {e.Error.Message}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                return;
            }
            
            lblStatus.Text = "下载完成，准备安装更新...";
            progressBar.Value = 100;
            
            _isDownloading = false;
            
            btnInstall.Enabled = true;
        }
        
        private void btnInstall_Click(object sender, EventArgs e)
        {
            if (_updateManager.ApplyUpdate(_updatePackageName))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                lblStatus.Text = "更新安装失败。";
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_isDownloading)
            {
                if (MessageBox.Show("确定要取消更新下载吗？", "取消确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // 取消下载
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
} 