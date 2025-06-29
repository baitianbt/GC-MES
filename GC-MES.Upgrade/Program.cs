using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GC_MES.Upgrade
{
    internal class Program
    {
        private const string DEFAULT_UPDATE_SERVER = "http://update.gc-mes.com";
        private const string MAIN_EXE_NAME = "GC-MES.WinForm.exe";
        
        static async Task Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // 创建服务容器
                var services = new ServiceCollection();
                ConfigureServices(services);
                var serviceProvider = services.BuildServiceProvider();
                
                // 获取配置和日志
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                
                // 解析命令行参数
                var parameters = ParseCommandLineArgs(args);
                
                // 获取默认参数，优先使用命令行参数，其次使用配置文件，最后使用默认值
                string action = GetParameterValue(parameters, "action", configuration.GetValue<string>("Updater:DefaultAction") ?? "check");
                string serverUrl = GetParameterValue(parameters, "server", configuration.GetValue<string>("Updater:ServerUrl") ?? DEFAULT_UPDATE_SERVER);
                string mainExePath = GetParameterValue(parameters, "app", configuration.GetValue<string>("Updater:MainExePath") ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MAIN_EXE_NAME));
                
                // 获取当前版本
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                if (parameters.ContainsKey("current"))
                {
                    if (Version.TryParse(parameters["current"], out Version parsedVersion))
                    {
                        currentVersion = parsedVersion;
                    }
                }
                
                // 显示启动信息
                logger.LogInformation("GC-MES 自动更新工具启动");
                logger.LogInformation($"操作: {action}");
                logger.LogInformation($"当前版本: {currentVersion}");
                logger.LogInformation($"更新服务器: {serverUrl}");
                
                // 创建更新管理器
                using (var updateManager = new UpdateManager(serverUrl, mainExePath, currentVersion))
                {
                    // 注册日志事件处理
                    updateManager.LogMessage += (sender, e) => 
                    {
                        switch (e.Level)
                        {
                            case LogLevel.Info:
                                logger.LogInformation(e.Message);
                                break;
                            case LogLevel.Warning:
                                logger.LogWarning(e.Message);
                                break;
                            case LogLevel.Error:
                                logger.LogError(e.Exception, e.Message);
                                break;
                        }
                    };
                    
                    // 根据操作类型执行不同的功能
                    switch (action.ToLower())
                    {
                        case "check":
                            await CheckForUpdates(updateManager, logger);
                            break;
                        case "apply":
                            await ApplyUpdate(updateManager, logger, parameters);
                            break;
                        case "rollback":
                            await RollbackUpdate(updateManager, logger, parameters);
                            break;
                        case "install":
                            await InstallUpdate(updateManager, logger, parameters);
                            break;
                        case "silent-update":
                            await SilentUpdate(updateManager, logger, parameters);
                            break;
                        default:
                            logger.LogError($"无效的操作类型: {action}。有效操作: check, apply, rollback, install, silent-update");
                            Environment.Exit(1);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误: {ex.Message}\n\n{ex.StackTrace}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }
        
        private static void ConfigureServices(ServiceCollection services)
        {
            // 添加配置
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            
            services.AddSingleton<IConfiguration>(configuration);
            
            // 添加日志
            services.AddLogging(builder => 
            {
                builder.AddConfiguration(configuration.GetSection("Logging"))
                       .AddConsole()
                       .AddDebug();
            });
        }
        
        private static Dictionary<string, string> ParseCommandLineArgs(string[] args)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    string key = args[i].Substring(2).ToLower();
                    string value = string.Empty;
                    
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("--") && !args[i + 1].StartsWith("-"))
                    {
                        value = args[i + 1];
                        i++;
                    }
                    
                    parameters[key] = value;
                }
                else if (args[i].StartsWith("-"))
                {
                    string key = args[i].Substring(1).ToLower();
                    string value = string.Empty;
                    
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("--") && !args[i + 1].StartsWith("-"))
                    {
                        value = args[i + 1];
                        i++;
                    }
                    
                    parameters[key] = value;
                }
            }
            
            return parameters;
        }
        
        private static string GetParameterValue(Dictionary<string, string> parameters, string key, string defaultValue)
        {
            return parameters.TryGetValue(key, out string value) ? value : defaultValue;
        }
        
        private static async Task CheckForUpdates(UpdateManager updateManager, ILogger logger)
        {
            logger.LogInformation("执行操作: 检查更新");
            
            var result = await updateManager.CheckForUpdatesAsync();
            
            if (!result.Success)
            {
                logger.LogError($"检查更新失败: {result.ErrorMessage}");
                MessageBox.Show($"检查更新失败: {result.ErrorMessage}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
                return;
            }
            
            if (!result.HasUpdate)
            {
                logger.LogInformation("当前已经是最新版本。");
                MessageBox.Show("当前已经是最新版本。", "更新检查", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(0);
                return;
            }
            
            if (!result.MeetsMinVersionRequirement)
            {
                logger.LogWarning($"当前版本过低，无法使用增量更新，需要重新安装。最低要求版本: {result.UpdateInfo.MinRequiredVersion}");
                MessageBox.Show($"当前版本过低，无法使用增量更新，需要重新安装最新版本。\n\n最低要求版本: {result.UpdateInfo.MinRequiredVersion}", 
                    "更新检查", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // TODO: 打开下载页面
                Environment.Exit(1);
                return;
            }
            
            logger.LogInformation($"发现新版本: {result.UpdateInfo.Version}");
            
            // 如果是强制更新
            if (result.UpdateInfo.IsForceUpdate)
            {
                logger.LogInformation("此版本为强制更新版本，将自动下载并安装。");
                MessageBox.Show("发现重要更新，系统将自动下载并安装此版本。", "强制更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            // 显示更新窗口
            using (UpdateProgressForm form = new UpdateProgressForm(updateManager, result.UpdateInfo))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    logger.LogInformation("更新已成功安装，应用程序将重新启动。");
                    Environment.Exit(0);
                }
                else
                {
                    logger.LogInformation("用户取消了更新。");
                    Environment.Exit(0);
                }
            }
        }
        
        private static async Task ApplyUpdate(UpdateManager updateManager, ILogger logger, Dictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("file", out string updateFile))
            {
                logger.LogError("缺少必要参数: --file <更新包文件路径>");
                MessageBox.Show("缺少必要参数: --file <更新包文件路径>", "参数错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
                return;
            }
            
            logger.LogInformation($"执行操作: 应用更新 {updateFile}");
            
            try
            {
                var result = await updateManager.ApplyUpdateAsync(updateFile);
                
                if (result.Success)
                {
                    logger.LogInformation($"更新已成功应用，备份路径: {result.BackupPath}");
                    Environment.Exit(0);
                }
                else
                {
                    logger.LogError($"应用更新失败: {result.ErrorMessage}");
                    MessageBox.Show($"应用更新失败: {result.ErrorMessage}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "应用更新时发生错误");
                MessageBox.Show($"应用更新时发生错误: {ex.Message}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }
        
        private static async Task RollbackUpdate(UpdateManager updateManager, ILogger logger, Dictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("backup", out string backupFolder))
            {
                logger.LogError("缺少必要参数: --backup <备份文件夹名称>");
                MessageBox.Show("缺少必要参数: --backup <备份文件夹名称>", "参数错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
                return;
            }
            
            logger.LogInformation($"执行操作: 回滚到备份 {backupFolder}");
            
            try
            {
                var result = await updateManager.RollbackAsync(backupFolder);
                
                if (result.Success)
                {
                    logger.LogInformation("回滚已完成。");
                    MessageBox.Show("回滚已成功完成，应用程序将重新启动。", "回滚完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start(updateManager._mainExePath);
                    Environment.Exit(0);
                }
                else
                {
                    logger.LogError($"回滚失败: {result.ErrorMessage}");
                    MessageBox.Show($"回滚失败: {result.ErrorMessage}", "回滚错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "回滚时发生错误");
                MessageBox.Show($"回滚时发生错误: {ex.Message}", "回滚错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }
        
        private static async Task InstallUpdate(UpdateManager updateManager, ILogger logger, Dictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("file", out string updateFile))
            {
                logger.LogError("缺少必要参数: --file <更新包文件路径>");
                MessageBox.Show("缺少必要参数: --file <更新包文件路径>", "参数错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
                return;
            }
            
            if (!File.Exists(updateFile))
            {
                logger.LogError($"更新文件不存在: {updateFile}");
                MessageBox.Show($"更新文件不存在: {updateFile}", "文件错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
                return;
            }
            
            logger.LogInformation($"执行操作: 安装更新包 {updateFile}");
            
            try
            {
                // 首先尝试解析更新包内的信息
                // TODO: 从zip中提取update_info.xml
                
                bool confirmed = true;
                
                // 如果不是静默安装，询问用户
                if (!parameters.ContainsKey("silent"))
                {
                    confirmed = MessageBox.Show(
                        "确定要安装此更新吗？安装过程中应用程序将会关闭，完成后会自动重启。",
                        "安装确认",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes;
                }
                
                if (confirmed)
                {
                    var result = await updateManager.ApplyUpdateAsync(updateFile);
                    
                    if (result.Success)
                    {
                        logger.LogInformation($"更新已成功安装，备份路径: {result.BackupPath}");
                        if (!parameters.ContainsKey("silent"))
                        {
                            MessageBox.Show("更新已成功安装，应用程序将重新启动。", "安装完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        Environment.Exit(0);
                    }
                    else
                    {
                        logger.LogError($"安装更新失败: {result.ErrorMessage}");
                        MessageBox.Show($"安装更新失败: {result.ErrorMessage}", "安装错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(1);
                    }
                }
                else
                {
                    logger.LogInformation("用户取消了更新安装。");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "安装更新时发生错误");
                MessageBox.Show($"安装更新时发生错误: {ex.Message}", "安装错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }
        
        private static async Task SilentUpdate(UpdateManager updateManager, ILogger logger, Dictionary<string, string> parameters)
        {
            logger.LogInformation("执行操作: 静默更新检查");
            
            try
            {
                var result = await updateManager.CheckForUpdatesAsync();
                
                if (!result.Success)
                {
                    logger.LogError($"静默更新检查失败: {result.ErrorMessage}");
                    Environment.Exit(1);
                    return;
                }
                
                if (!result.HasUpdate)
                {
                    logger.LogInformation("当前已经是最新版本。");
                    Environment.Exit(0);
                    return;
                }
                
                if (!result.MeetsMinVersionRequirement)
                {
                    logger.LogWarning($"当前版本过低，无法使用增量更新。最低要求版本: {result.UpdateInfo.MinRequiredVersion}");
                    Environment.Exit(1);
                    return;
                }
                
                // 只有强制更新或明确要求下载才会自动下载更新
                bool shouldDownload = result.UpdateInfo.IsForceUpdate || parameters.ContainsKey("download");
                if (!shouldDownload)
                {
                    logger.LogInformation($"发现新版本 {result.UpdateInfo.Version}，但不是强制更新且未开启自动下载。");
                    Environment.Exit(0);
                    return;
                }
                
                logger.LogInformation($"开始下载新版本: {result.UpdateInfo.Version}");
                
                var downloadResult = await updateManager.DownloadUpdateAsync(result.UpdateInfo);
                
                if (!downloadResult.Success)
                {
                    logger.LogError($"下载更新失败: {downloadResult.ErrorMessage}");
                    Environment.Exit(1);
                    return;
                }
                
                logger.LogInformation($"更新下载完成: {downloadResult.FilePath}");
                
                // 只有强制更新或明确要求安装才会自动安装更新
                bool shouldInstall = result.UpdateInfo.IsForceUpdate || parameters.ContainsKey("install");
                if (!shouldInstall)
                {
                    logger.LogInformation("更新已下载，但未开启自动安装。");
                    Environment.Exit(0);
                    return;
                }
                
                logger.LogInformation("开始安装更新...");
                
                var installResult = await updateManager.ApplyUpdateAsync(downloadResult.FilePath);
                
                if (installResult.Success)
                {
                    logger.LogInformation($"更新已成功安装，备份路径: {installResult.BackupPath}");
                    Environment.Exit(0);
                }
                else
                {
                    logger.LogError($"安装更新失败: {installResult.ErrorMessage}");
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "静默更新过程中发生错误");
                Environment.Exit(1);
            }
        }
    }
}
