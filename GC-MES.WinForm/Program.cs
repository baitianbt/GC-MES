using GC_MES.WinForm.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using SqlSugar;
using System.Reflection;

namespace GC_MES.WinForm
{
    public static class Program
    {
        public static IServiceProvider Services { get; private set; }
        public static IConfiguration Configuration { get; private set; }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // 设置异常处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);






            // 创建服务容器
            var services = new ServiceCollection();

            // 注册日志服务


            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logFilePath); // 确保日志目录存在



            // 配置 Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(
                    Path.Combine(logFilePath, "log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            // 注入 Serilog 到 Microsoft.Extensions.Logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });



            // 注册配置


            // 加载 config.ini
            //Configuration = new ConfigurationBuilder()
            //    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //    .AddIniFile("config.ini", optional: false, reloadOnChange: true)
            //    .Build();


            Configuration = new ConfigurationBuilder()
      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .Build();

            services.AddSingleton<IConfiguration>(Configuration);







            // 注册 SqlSugar 配置
            services.AddScoped<ISqlSugarClient>(provider =>
            {  // 获取日志服务
                var logger = provider.GetRequiredService<ILogger<SqlSugarClient>>();
                var config = provider.GetRequiredService<IConfiguration>();
                var dbType = config["Connection:DBType"];
                var connStr = config["Connection:DbConnectionString"];

                if (string.IsNullOrEmpty(connStr))
                {
                    throw new Exception("未能读取到数据库连接字符串，请检查 appsettings.json 配置！");
                }
                var db = new SqlSugarClient(new ConnectionConfig
                {
                    ConnectionString = config["Connection:DbConnectionString"], // 设置数据库连接字符串
                    DbType = DbType.SqlServer, // 或其他数据库类型
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                });
                db.Aop.OnLogExecuting = (sql, pars) => { logger.LogInformation("SQL: {sql} | Params: {params}", sql, pars); };
                db.Aop.OnError = ex =>
                {
                    logger.LogError(ex, "SqlSugar 执行异常");
                };
                return db;
            });

            // 注册窗体
            RegisterForms(services);

            // 注册仓储和服务
            RegisterRepositories(services);
            RegisterServices(services);

            // 构建服务提供者
            Services = services.BuildServiceProvider();

            // 启动登录窗体
            using (var scope = Services.CreateScope())
            {
                var loginForm = scope.ServiceProvider.GetRequiredService<LoginForm>();
                Application.Run(loginForm);
            }
        }

        /// <summary>
        /// 注册窗体
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterForms(IServiceCollection services)
        {
            // 注册所有窗体
            var formTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(Form).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var formType in formTypes)
            {
                services.AddTransient(formType);
            }

            // 特别注册登录窗体
            services.AddTransient<LoginForm>();
        }


        /// <summary>
        /// 注册所有的仓储模块
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterRepositories(IServiceCollection services)
        {
            try
            {
                // 注册所有仓储
                var repositoryAssembly = Assembly.Load("GC-MES.DAL");
                var iRepositoryAssembly = Assembly.Load("GC-MES.DAL");

                var types = repositoryAssembly.GetTypes();
                var interfaces = iRepositoryAssembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        foreach (var interfaceType in interfaces)
                        {
                            if (interfaceType.IsInterface && interfaceType.Name.Equals($"I{type.Name}"))
                            {
                                services.AddScoped(interfaceType, type);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"注册仓储时出错: {ex.Message}");
            }
        }


        /// <summary>
        /// 注册所有的服务模块
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterServices(IServiceCollection services)
        {
            try
            {
                // 加载BLL程序集
                var servicesAssembly = Assembly.Load("GC-MES.BLL");



                var types = servicesAssembly.GetTypes();
                var interfaces = servicesAssembly.GetTypes().Where(t => t.IsInterface).ToArray();

                Console.WriteLine($"找到 {types.Length} 个类和 {interfaces.Length} 个接口");

                // 记录日志
                foreach (var type in types)
                {
                    if (type.IsClass && !type.IsAbstract && type.Name.EndsWith("Service"))
                    {
                        Console.WriteLine($"检查服务: {type.FullName}");

                        // 1. 尝试按命名约定匹配
                        var interfaceName = $"I{type.Name}";
                        var interfaceType = interfaces.FirstOrDefault(i => i.Name == interfaceName);

                        // 2. 尝试根据继承关系匹配
                        if (interfaceType == null)
                        {
                            var implementedInterfaces = type.GetInterfaces();
                            interfaceType = implementedInterfaces.FirstOrDefault(i =>
                                i.Name.EndsWith("Service") && interfaces.Contains(i));
                        }

                        if (interfaceType != null)
                        {
                            services.AddScoped(interfaceType, type);
                            Console.WriteLine($"已注册: {interfaceType.Name} -> {type.Name}");
                        }
                        else
                        {
                            Console.WriteLine($"未找到接口: {type.Name}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"注册服务时出错: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            //try
            //{
            //    // 注册所有服务
            //    var servicesAssembly = Assembly.Load("GC-MES.BLL");
            //    var iServicesAssembly = Assembly.Load("GC-MES.BLL");

            //    var types = servicesAssembly.GetTypes();
            //    var interfaces = iServicesAssembly.GetTypes();

            //    foreach (var type in types)
            //    {
            //        if (type.IsClass && !type.IsAbstract)
            //        {
            //            foreach (var interfaceType in interfaces)
            //            {
            //                if (interfaceType.IsInterface && interfaceType.Name.Equals($"I{type.Name}"))
            //                {
            //                    services.AddScoped(interfaceType, type);
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"注册服务时出错: {ex.Message}");
            //}
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs ex)
        {
            var result = MessageBox.Show("系统发生错误，您需要退出系统吗？", "异常", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }


        /// <summary>
        /// 自动更新待测试
        /// </summary>
        /// <returns></returns>
        private static async Task CheckForUpdatesAsync()
        {
            try
            {
                string serverUrl = "http://update.gc-mes.com"; // 可从配置获取
                string mainExePath = Application.ExecutablePath;
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                using (var updateManager = new GC_MES.Upgrade.UpdateManager(serverUrl, mainExePath, currentVersion))
                {
                    var result = await updateManager.CheckForUpdatesAsync();

                    if (!result.Success)
                    {
                        MessageBox.Show($"检查更新失败: {result.ErrorMessage}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!result.HasUpdate)
                    {
                        MessageBox.Show("当前已经是最新版本。", "更新检查", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    using (var form = new GC_MES.Upgrade.UpdateProgressForm(updateManager, result.UpdateInfo))
                    {
                        form.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"检查更新时发生错误: {ex.Message}", "更新错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}