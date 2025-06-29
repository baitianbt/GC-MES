using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GC_MES.Upgrade
{
    public class UpdateManager : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _serverUrl;
        private readonly string _updateInfoFile = "update_info.xml";
        private readonly string _backupDir;
        private readonly string _tempDir;
        private readonly string _logDir;
        public  string _mainExePath;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private UpdateInfo _latestUpdateInfo;
        
        // 版本信息
        public Version CurrentVersion { get; private set; }
        public Version NewVersion { get; private set; }
        public string ServerUrl => _serverUrl;
        
        // 事件
        public event EventHandler<DownloadProgressEventArgs> DownloadProgressChanged;
        public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;
        public event EventHandler<InstallProgressEventArgs> InstallProgressChanged;
        public event EventHandler<InstallCompletedEventArgs> InstallCompleted;
        public event EventHandler<LogEventArgs> LogMessage;

        public UpdateManager(string serverUrl, string mainExePath, Version currentVersion)
        {
            _serverUrl = serverUrl;
            _mainExePath = mainExePath;
            CurrentVersion = currentVersion;
            _httpClient = new HttpClient();
            _cancellationTokenSource = new CancellationTokenSource();
            
            // 创建必要目录
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _tempDir = Path.Combine(baseDir, "temp");
            _backupDir = Path.Combine(baseDir, "backup");
            _logDir = Path.Combine(baseDir, "logs");
            
            EnsureDirectoryExists(_tempDir);
            EnsureDirectoryExists(_backupDir);
            EnsureDirectoryExists(_logDir);
            
            // 配置HTTP客户端
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", $"GC-MES-Updater/{currentVersion}");
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _httpClient.Dispose();
        }

        // 检查更新
        public async Task<UpdateCheckResult> CheckForUpdatesAsync()
        {
            try
            {
                LogInfo("开始检查更新");
                
                // 构建更新信息文件的URL
                string updateInfoUrl = $"{_serverUrl.TrimEnd('/')}/{_updateInfoFile}";
                string tempUpdateInfoPath = Path.Combine(_tempDir, _updateInfoFile);
                
                // 下载更新信息文件
                using (var response = await _httpClient.GetAsync(updateInfoUrl, _cancellationTokenSource.Token))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        LogWarning($"下载更新信息失败: HTTP {(int)response.StatusCode} {response.ReasonPhrase}");
                        return new UpdateCheckResult 
                        { 
                            Success = false,
                            ErrorMessage = $"服务器返回错误: {(int)response.StatusCode} {response.ReasonPhrase}" 
                        };
                    }
                    
                    // 保存更新信息文件
                    using (var fs = new FileStream(tempUpdateInfoPath, FileMode.Create))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }
                
                // 解析更新信息
                _latestUpdateInfo = UpdateInfo.FromFile(tempUpdateInfoPath);
                NewVersion = _latestUpdateInfo.Version;
                
                // 检查是否需要更新
                bool hasUpdate = NewVersion > CurrentVersion;
                LogInfo($"检查更新完成: 当前版本 {CurrentVersion}, 最新版本 {NewVersion}, 需要更新: {hasUpdate}");
                
                // 检查最低版本要求
                bool meetMinRequirement = _latestUpdateInfo.CheckMinVersionRequirement(CurrentVersion);
                
                return new UpdateCheckResult 
                { 
                    Success = true,
                    HasUpdate = hasUpdate,
                    UpdateInfo = _latestUpdateInfo,
                    MeetsMinVersionRequirement = meetMinRequirement
                };
            }
            catch (OperationCanceledException)
            {
                LogWarning("检查更新已取消");
                return new UpdateCheckResult 
                { 
                    Success = false, 
                    ErrorMessage = "检查更新已取消" 
                };
            }
            catch (Exception ex)
            {
                LogError($"检查更新失败: {ex.Message}", ex);
                return new UpdateCheckResult 
                { 
                    Success = false, 
                    ErrorMessage = $"检查更新失败: {ex.Message}" 
                };
            }
        }
        
        // 下载更新
        public async Task<DownloadResult> DownloadUpdateAsync(UpdateInfo updateInfo)
        {
            if (updateInfo == null)
            {
                throw new ArgumentNullException(nameof(updateInfo), "更新信息不能为空");
            }
            
            string tempFilePath = Path.Combine(_tempDir, updateInfo.FileName);
            
            try
            {
                LogInfo($"开始下载更新包: {updateInfo.FileName}");
                
                // 创建下载请求
                using (var response = await _httpClient.GetAsync(updateInfo.DownloadUrl, HttpCompletionOption.ResponseHeadersRead, _cancellationTokenSource.Token))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        LogWarning($"下载更新包失败: HTTP {(int)response.StatusCode} {response.ReasonPhrase}");
                        return new DownloadResult
                        {
                            Success = false,
                            ErrorMessage = $"服务器返回错误: {(int)response.StatusCode} {response.ReasonPhrase}"
                        };
                    }
                    
                    long totalBytes = response.Content.Headers.ContentLength ?? -1;
                    long downloadedBytes = 0;
                    byte[] buffer = new byte[8192];
                    int bytesRead;
                    
                    // 创建文件流
                    using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None, buffer.Length, true))
                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    {
                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, _cancellationTokenSource.Token)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead, _cancellationTokenSource.Token);
                            
                            // 更新进度
                            downloadedBytes += bytesRead;
                            int progressPercentage = totalBytes > 0 
                                ? (int)((double)downloadedBytes / totalBytes * 100) 
                                : -1;
                            
                            OnDownloadProgressChanged(new DownloadProgressEventArgs
                            {
                                BytesReceived = downloadedBytes,
                                TotalBytesToReceive = totalBytes,
                                ProgressPercentage = progressPercentage
                            });
                        }
                    }
                }
                
                // 验证下载的文件
                if (!string.IsNullOrEmpty(updateInfo.MD5Checksum))
                {
                    LogInfo("校验更新包完整性...");
                    bool isValid = updateInfo.VerifyFileChecksum(tempFilePath);
                    if (!isValid)
                    {
                        LogWarning("更新包校验失败");
                        return new DownloadResult
                        {
                            Success = false,
                            ErrorMessage = "下载的更新包校验失败，文件可能已损坏"
                        };
                    }
                }
                
                LogInfo($"更新包下载完成: {updateInfo.FileName}");
                
                OnDownloadCompleted(new DownloadCompletedEventArgs
                {
                    Cancelled = false,
                    Error = null,
                    FilePath = tempFilePath
                });
                
                return new DownloadResult
                {
                    Success = true,
                    FilePath = tempFilePath
                };
            }
            catch (OperationCanceledException)
            {
                LogWarning("下载更新已取消");
                
                OnDownloadCompleted(new DownloadCompletedEventArgs
                {
                    Cancelled = true,
                    Error = null,
                    FilePath = null
                });
                
                // 清理部分下载的文件
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
                
                return new DownloadResult
                {
                    Success = false,
                    ErrorMessage = "下载更新已取消"
                };
            }
            catch (Exception ex)
            {
                LogError($"下载更新包失败: {ex.Message}", ex);
                
                OnDownloadCompleted(new DownloadCompletedEventArgs
                {
                    Cancelled = false,
                    Error = ex,
                    FilePath = null
                });
                
                // 清理部分下载的文件
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
                
                return new DownloadResult
                {
                    Success = false,
                    ErrorMessage = $"下载更新包失败: {ex.Message}"
                };
            }
        }
        
        // 应用更新
        public async Task<InstallResult> ApplyUpdateAsync(string updatePackagePath)
        {
            try
            {
                if (!File.Exists(updatePackagePath))
                {
                    throw new FileNotFoundException("更新包文件不存在", updatePackagePath);
                }
                
                LogInfo($"开始应用更新: {Path.GetFileName(updatePackagePath)}");
                
                // 创建备份
                string backupFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupPath = Path.Combine(_backupDir, backupFolderName);
                
                OnInstallProgressChanged(new InstallProgressEventArgs
                {
                    Phase = InstallPhase.CreatingBackup,
                    ProgressPercentage = 0,
                    StatusMessage = "正在创建备份..."
                });
                
                await Task.Run(() => BackupCurrentFiles(backupPath));
                
                // 解压更新包
                string extractDir = Path.Combine(_tempDir, "extract");
                if (Directory.Exists(extractDir))
                {
                    Directory.Delete(extractDir, true);
                }
                
                Directory.CreateDirectory(extractDir);
                
                OnInstallProgressChanged(new InstallProgressEventArgs
                {
                    Phase = InstallPhase.Extracting,
                    ProgressPercentage = 0,
                    StatusMessage = "正在解压更新包..."
                });
                
                await Task.Run(() => ZipFile.ExtractToDirectory(updatePackagePath, extractDir));
                
                // 应用更新文件
                OnInstallProgressChanged(new InstallProgressEventArgs
                {
                    Phase = InstallPhase.Updating,
                    ProgressPercentage = 0,
                    StatusMessage = "正在更新文件..."
                });
                
                await Task.Run(() => ReplaceFiles(extractDir));
                
                LogInfo("更新应用完成，即将重启应用程序");
                
                OnInstallCompleted(new InstallCompletedEventArgs
                {
                    Success = true,
                    BackupPath = backupPath
                });
                
                // 重启应用程序
                if (!string.IsNullOrEmpty(_mainExePath) && File.Exists(_mainExePath))
                {
                    Process.Start(_mainExePath);
                }
                
                return new InstallResult
                {
                    Success = true,
                    BackupPath = backupPath
                };
            }
            catch (Exception ex)
            {
                LogError($"应用更新失败: {ex.Message}", ex);
                
                OnInstallCompleted(new InstallCompletedEventArgs
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
                
                return new InstallResult
                {
                    Success = false,
                    ErrorMessage = $"应用更新失败: {ex.Message}"
                };
            }
        }
        
        // 回滚更新
        public async Task<RollbackResult> RollbackAsync(string backupFolderName)
        {
            try
            {
                string backupDir = Path.Combine(_backupDir, backupFolderName);
                if (!Directory.Exists(backupDir))
                {
                    LogWarning($"备份文件夹不存在: {backupFolderName}");
                    return new RollbackResult
                    {
                        Success = false,
                        ErrorMessage = $"备份文件夹不存在: {backupFolderName}"
                    };
                }
                
                LogInfo($"开始回滚到备份: {backupFolderName}");
                
                // 执行回滚
                await Task.Run(() => {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    foreach (string file in Directory.GetFiles(backupDir, "*.*", SearchOption.AllDirectories))
                    {
                        string relativePath = GetRelativePath(file, backupDir);
                        string targetPath = Path.Combine(baseDir, relativePath);
                        
                        string targetDir = Path.GetDirectoryName(targetPath);
                        if (!Directory.Exists(targetDir))
                        {
                            Directory.CreateDirectory(targetDir);
                        }
                        
                        if (File.Exists(targetPath))
                        {
                            File.Delete(targetPath);
                        }
                        
                        File.Copy(file, targetPath);
                    }
                });
                
                LogInfo("回滚完成");
                
                return new RollbackResult
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                LogError($"回滚失败: {ex.Message}", ex);
                return new RollbackResult
                {
                    Success = false,
                    ErrorMessage = $"回滚失败: {ex.Message}"
                };
            }
        }
        
        // 取消当前操作
        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
        
        #region 私有辅助方法
        
        // 创建目录（如果不存在）
        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        
        // 备份当前文件
        private void BackupCurrentFiles(string backupPath)
        {
            Directory.CreateDirectory(backupPath);
            
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var excludePaths = new[] { "temp", "backup", "logs" };
            
            foreach (string file in Directory.GetFiles(baseDir, "*.*", SearchOption.AllDirectories))
            {
                // 排除特定目录
                bool shouldExclude = false;
                foreach (var excludePath in excludePaths)
                {
                    if (file.Contains(Path.DirectorySeparatorChar + excludePath + Path.DirectorySeparatorChar) ||
                        file.EndsWith(Path.DirectorySeparatorChar + excludePath))
                    {
                        shouldExclude = true;
                        break;
                    }
                }
                
                if (shouldExclude)
                    continue;
                
                string relativePath = GetRelativePath(file, baseDir);
                string targetPath = Path.Combine(backupPath, relativePath);
                
                string targetDir = Path.GetDirectoryName(targetPath);
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
                
                File.Copy(file, targetPath);
            }
        }
        
        // 获取相对路径
        private string GetRelativePath(string fullPath, string basePath)
        {
            // 确保路径以目录分隔符结束
            if (!basePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                basePath += Path.DirectorySeparatorChar;
            }
            
            // 获取相对路径
            if (fullPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
            {
                return fullPath.Substring(basePath.Length);
            }
            
            return fullPath;
        }
        
        // 替换文件
        private void ReplaceFiles(string sourceDir)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            foreach (string file in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = GetRelativePath(file, sourceDir);
                string targetPath = Path.Combine(baseDir, relativePath);
                
                string targetDir = Path.GetDirectoryName(targetPath);
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
                
                if (File.Exists(targetPath))
                {
                    // 对于配置文件，可能需要保留用户配置
                    if (Path.GetExtension(targetPath).ToLower() == ".config" ||
                        Path.GetExtension(targetPath).ToLower() == ".json" ||
                        Path.GetFileName(targetPath).ToLower() == "appsettings.json")
                    {
                        // 备份原配置文件
                        string backupFile = targetPath + ".bak";
                        if (File.Exists(backupFile))
                        {
                            File.Delete(backupFile);
                        }
                        File.Move(targetPath, backupFile);
                    }
                    else
                    {
                        File.Delete(targetPath);
                    }
                }
                
                File.Copy(file, targetPath);
            }
        }
        
        // 记录日志
        private void LogInfo(string message)
        {
            Log(LogLevel.Info, message);
        }
        
        private void LogWarning(string message)
        {
            Log(LogLevel.Warning, message);
        }
        
        private void LogError(string message, Exception exception = null)
        {
            Log(LogLevel.Error, message, exception);
        }
        
        private void Log(LogLevel level, string message, Exception exception = null)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            if (exception != null)
            {
                logMessage += Environment.NewLine + exception.ToString();
            }
            
            OnLogMessage(new LogEventArgs
            {
                Level = level,
                Message = message,
                Exception = exception,
                Timestamp = DateTime.Now
            });
            
            try
            {
                string logFile = Path.Combine(_logDir, $"update_{DateTime.Now:yyyyMMdd}.log");
                File.AppendAllText(logFile, logMessage + Environment.NewLine);
            }
            catch
            {
                // 忽略日志记录错误
            }
        }
        
        // 触发事件
        private void OnDownloadProgressChanged(DownloadProgressEventArgs e)
        {
            DownloadProgressChanged?.Invoke(this, e);
        }
        
        private void OnDownloadCompleted(DownloadCompletedEventArgs e)
        {
            DownloadCompleted?.Invoke(this, e);
        }
        
        private void OnInstallProgressChanged(InstallProgressEventArgs e)
        {
            InstallProgressChanged?.Invoke(this, e);
        }
        
        private void OnInstallCompleted(InstallCompletedEventArgs e)
        {
            InstallCompleted?.Invoke(this, e);
        }
        
        private void OnLogMessage(LogEventArgs e)
        {
            LogMessage?.Invoke(this, e);
        }
        
        #endregion
    }
    
    #region 事件参数和结果类型
    
    // 日志级别
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }
    
    // 安装阶段
    public enum InstallPhase
    {
        CreatingBackup,
        Extracting,
        Updating,
        Restarting
    }
    
    // 日志事件参数
    public class LogEventArgs : EventArgs
    {
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public DateTime Timestamp { get; set; }
    }
    
    // 下载进度事件参数
    public class DownloadProgressEventArgs : EventArgs
    {
        public long BytesReceived { get; set; }
        public long TotalBytesToReceive { get; set; }
        public int ProgressPercentage { get; set; }
    }
    
    // 下载完成事件参数
    public class DownloadCompletedEventArgs : EventArgs
    {
        public bool Cancelled { get; set; }
        public Exception Error { get; set; }
        public string FilePath { get; set; }
    }
    
    // 安装进度事件参数
    public class InstallProgressEventArgs : EventArgs
    {
        public InstallPhase Phase { get; set; }
        public int ProgressPercentage { get; set; }
        public string StatusMessage { get; set; }
    }
    
    // 安装完成事件参数
    public class InstallCompletedEventArgs : EventArgs
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string BackupPath { get; set; }
    }
    
    // 检查更新结果
    public class UpdateCheckResult
    {
        public bool Success { get; set; }
        public bool HasUpdate { get; set; }
        public UpdateInfo UpdateInfo { get; set; }
        public bool MeetsMinVersionRequirement { get; set; }
        public string ErrorMessage { get; set; }
    }
    
    // 下载结果
    public class DownloadResult
    {
        public bool Success { get; set; }
        public string FilePath { get; set; }
        public string ErrorMessage { get; set; }
    }
    
    // 安装结果
    public class InstallResult
    {
        public bool Success { get; set; }
        public string BackupPath { get; set; }
        public string ErrorMessage { get; set; }
    }
    
    // 回滚结果
    public class RollbackResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
    
    #endregion
} 