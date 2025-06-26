using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GC_MES.Upgrade
{
    public class UpdateManager
    {
        private readonly string _serverUrl;
        private readonly string _updateInfoFile = "update_info.xml";
        private readonly string _backupDir;
        private readonly string _tempDir;
        private readonly string _mainExePath;
        
        public Version CurrentVersion { get; private set; }
        public Version NewVersion { get; private set; }
        public string ServerUrl => _serverUrl;
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<AsyncCompletedEventArgs> DownloadCompleted;

        public UpdateManager(string serverUrl, string mainExePath, Version currentVersion)
        {
            _serverUrl = serverUrl;
            _mainExePath = mainExePath;
            CurrentVersion = currentVersion;
            
            // 创建临时目录和备份目录
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _tempDir = Path.Combine(baseDir, "temp");
            _backupDir = Path.Combine(baseDir, "backup");
            
            if (!Directory.Exists(_tempDir))
                Directory.CreateDirectory(_tempDir);
                
            if (!Directory.Exists(_backupDir))
                Directory.CreateDirectory(_backupDir);
        }

        public async Task<bool> CheckForUpdatesAsync()
        {
            try
            {
                // 下载更新信息文件
                string updateInfoUrl = $"{_serverUrl}/{_updateInfoFile}";
                string tempUpdateInfoPath = Path.Combine(_tempDir, _updateInfoFile);
                
                using (WebClient client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(new Uri(updateInfoUrl), tempUpdateInfoPath);
                }
                
                // 解析更新信息文件
                UpdateInfo updateInfo = UpdateInfo.FromFile(tempUpdateInfoPath);
                NewVersion = updateInfo.Version;
                
                // 检查是否需要更新
                return NewVersion > CurrentVersion;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"检查更新时发生错误: {ex.Message}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        public async Task DownloadUpdateAsync(string downloadUrl, string fileName)
        {
            try
            {
                string tempFilePath = Path.Combine(_tempDir, fileName);
                WebClient client = new WebClient();
                
                client.DownloadProgressChanged += (sender, e) => 
                {
                    ProgressChanged?.Invoke(this, e);
                };
                
                client.DownloadFileCompleted += (sender, e) => 
                {
                    DownloadCompleted?.Invoke(this, e);
                };
                
                await client.DownloadFileTaskAsync(new Uri(downloadUrl), tempFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"下载更新时发生错误: {ex.Message}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public bool ApplyUpdate(string updatePackageName)
        {
            try
            {
                string updatePackagePath = Path.Combine(_tempDir, updatePackageName);
                
                // 备份当前文件
                BackupCurrentFiles();
                
                // 解压更新包
                string extractDir = Path.Combine(_tempDir, "extract");
                if (Directory.Exists(extractDir))
                    Directory.Delete(extractDir, true);
                    
                Directory.CreateDirectory(extractDir);
                System.IO.Compression.ZipFile.ExtractToDirectory(updatePackagePath, extractDir);
                
                // 替换文件
                ReplaceFiles(extractDir);
                
                // 启动主程序
                Process.Start(_mainExePath);
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"应用更新时发生错误: {ex.Message}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        private void BackupCurrentFiles()
        {
            string currentBackupDir = Path.Combine(_backupDir, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            Directory.CreateDirectory(currentBackupDir);
            
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            foreach (string file in Directory.GetFiles(baseDir, "*.*", SearchOption.AllDirectories))
            {
                if (file.Contains("temp") || file.Contains("backup"))
                    continue;
                    
                string relativePath = file.Substring(baseDir.Length);
                string targetPath = Path.Combine(currentBackupDir, relativePath);
                
                string targetDir = Path.GetDirectoryName(targetPath);
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);
                    
                File.Copy(file, targetPath);
            }
        }
        
        private void ReplaceFiles(string sourceDir)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            foreach (string file in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = file.Substring(sourceDir.Length + 1);
                string targetPath = Path.Combine(baseDir, relativePath);
                
                string targetDir = Path.GetDirectoryName(targetPath);
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);
                    
                if (File.Exists(targetPath))
                    File.Delete(targetPath);
                    
                File.Copy(file, targetPath);
            }
        }
        
        public void Rollback(string backupFolderName)
        {
            string backupDir = Path.Combine(_backupDir, backupFolderName);
            if (!Directory.Exists(backupDir))
            {
                MessageBox.Show($"备份文件夹 {backupFolderName} 不存在", "回滚失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            foreach (string file in Directory.GetFiles(backupDir, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = file.Substring(backupDir.Length + 1);
                string targetPath = Path.Combine(baseDir, relativePath);
                
                string targetDir = Path.GetDirectoryName(targetPath);
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);
                    
                if (File.Exists(targetPath))
                    File.Delete(targetPath);
                    
                File.Copy(file, targetPath);
            }
            
            MessageBox.Show("回滚成功完成", "回滚", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
} 