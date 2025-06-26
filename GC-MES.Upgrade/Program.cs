using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GC_MES.Upgrade
{
    internal class Program
    {
        private const string DEFAULT_UPDATE_SERVER = "http://update.gc-mes.com";
        private const string MAIN_EXE_NAME = "GC-MES.WinForm.exe";
        
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // 解析命令行参数
                Dictionary<string, string> argsDictionary = ParseCommandLineArgs(args);
                
                // 默认操作为检查更新
                string action = GetArgumentValue(argsDictionary, "action", "check");
                string serverUrl = GetArgumentValue(argsDictionary, "server", DEFAULT_UPDATE_SERVER);
                string mainExePath = GetArgumentValue(argsDictionary, "app", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MAIN_EXE_NAME));
                
                // 获取当前版本
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                if (argsDictionary.ContainsKey("current"))
                {
                    if (Version.TryParse(argsDictionary["current"], out Version parsedVersion))
                    {
                        currentVersion = parsedVersion;
                    }
                }
                
                Console.WriteLine("GC-MES 自动更新工具");
                Console.WriteLine($"当前版本: {currentVersion}");
                Console.WriteLine($"更新服务器: {serverUrl}");
                Console.WriteLine();
                
                UpdateManager updateManager = new UpdateManager(serverUrl, mainExePath, currentVersion);
                
                // 根据操作类型执行不同的功能
                switch (action.ToLower())
                {
                    case "check":
                        CheckForUpdates(updateManager);
                        break;
                    case "download":
                        DownloadUpdate(updateManager, argsDictionary);
                        break;
                    case "apply":
                        ApplyUpdate(updateManager, argsDictionary);
                        break;
                    case "rollback":
                        RollbackUpdate(updateManager, argsDictionary);
                        break;
                    default:
                        Console.WriteLine("无效的操作类型。有效操作: check, download, apply, rollback");
                        Environment.Exit(1);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
                Environment.Exit(1);
            }
        }
        
        private static Dictionary<string, string> ParseCommandLineArgs(string[] args)
        {
            Dictionary<string, string> argsDictionary = new Dictionary<string, string>();
            
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    string key = args[i].Substring(2);
                    string value = string.Empty;
                    
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("--"))
                    {
                        value = args[i + 1];
                        i++;
                    }
                    
                    argsDictionary[key] = value;
                }
                else if (args[i].StartsWith("-"))
                {
                    string key = args[i].Substring(1);
                    string value = string.Empty;
                    
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                    {
                        value = args[i + 1];
                        i++;
                    }
                    
                    argsDictionary[key] = value;
                }
            }
            
            return argsDictionary;
        }
        
        private static string GetArgumentValue(Dictionary<string, string> args, string key, string defaultValue)
        {
            return args.ContainsKey(key) ? args[key] : defaultValue;
        }
        
        private static async void CheckForUpdates(UpdateManager updateManager)
        {
            Console.WriteLine("检查更新...");
            
            bool hasUpdate = await updateManager.CheckForUpdatesAsync();
            
            if (hasUpdate)
            {
                Console.WriteLine($"发现新版本: {updateManager.NewVersion}");
                
                // 显示更新窗口
                using (UpdateProgressForm form = new UpdateProgressForm(updateManager, updateManager.ServerUrl, updateManager.NewVersion))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        Console.WriteLine("更新已成功安装。应用程序将重新启动。");
                        Environment.Exit(0);
                    }
                }
            }
            else
            {
                Console.WriteLine("当前已经是最新版本。");
                Environment.Exit(0);
            }
        }
        
        private static void DownloadUpdate(UpdateManager updateManager, Dictionary<string, string> args)
        {
            Console.WriteLine("此功能通过界面提供，请使用 --action check 来检查和下载更新。");
            Environment.Exit(1);
        }
        
        private static void ApplyUpdate(UpdateManager updateManager, Dictionary<string, string> args)
        {
            if (!args.ContainsKey("file"))
            {
                Console.WriteLine("缺少必要参数: --file <更新包文件名>");
                Environment.Exit(1);
                return;
            }
            
            string updateFile = args["file"];
            Console.WriteLine($"应用更新: {updateFile}");
            
            if (updateManager.ApplyUpdate(updateFile))
            {
                Console.WriteLine("更新已成功应用。应用程序将重新启动。");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("应用更新失败。");
                Environment.Exit(1);
            }
        }
        
        private static void RollbackUpdate(UpdateManager updateManager, Dictionary<string, string> args)
        {
            if (!args.ContainsKey("backup"))
            {
                Console.WriteLine("缺少必要参数: --backup <备份文件夹名称>");
                Environment.Exit(1);
                return;
            }
            
            string backupFolder = args["backup"];
            Console.WriteLine($"回滚到备份: {backupFolder}");
            
            updateManager.Rollback(backupFolder);
            
            Console.WriteLine("回滚已完成。");
        }
    }
}
