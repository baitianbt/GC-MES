using GC_MES.DAL.DbContexts;
using GC_MES.WinForm.Extensions;
using GC_MES.WinForm.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;

namespace GC_MES.WinForm
{
    public static class Program
    {
        // 添加一个可以全局访问的ServiceProvider
        public static IServiceProvider ServiceProvider { get; private set; }
        
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // .NET 8 Windows Forms应用初始化
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // 1. 构建配置
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2. 创建服务容器
            var services = new ServiceCollection();

            // 3. 注册配置对象，可用IConfiguration注入，也可用强类型绑定
            services.AddSingleton<IConfiguration>(config);
            // 或者
            // services.Configure<MySettings>(config.GetSection("MySettings"));

            // 4. 注册日志（控制台示例）
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(config.GetSection("Logging"));
                loggingBuilder.AddConsole();
            });

            // 5. 注册你的服务、仓储、窗体
            services.AddScoped<MesDbContext>(provider =>
            {
                var connectionString = config.GetConnectionString("DbConnectionString");
                // var optionsBuilder = new System.Data.Entity.DbContext("name=DefaultConnection"); // EF6示例
                return new MesDbContext(); // EF6不需optionsBuilder，连接字符串配置在App.config
            });
            
            // 使用扩展方法自动注册所有仓储和服务
            services.AutoRegisterServices("GC-MES.DAL", "Repository");
            services.AutoRegisterServices("GC-MES.BLL", "Service");
            
            // 手动注册窗体
            services.AddScoped<LoginForm>();
            services.AddScoped<MainForm>();
            services.AddScoped<UserManagementForm>();
            services.AddScoped<WorkOrderManagementForm>();
            services.AddScoped<RoleManagementForm>();
            services.AddScoped<PermissionManagementForm>();
            services.AddScoped<RolePermissionForm>();
            services.AddScoped<RoleEditForm>();
            services.AddScoped<ProductManagementForm>();
            services.AddScoped<ProductEditForm>();
            services.AddScoped<BOMManagementForm>();
            services.AddScoped<BOMEditForm>();
            services.AddScoped<RoutingManagementForm>();
            services.AddScoped<RoutingEditForm>();
            
            // 质量模块相关窗体
            // 在完成迁移后添加这些窗体
            /* 
            services.AddScoped<QualityInspectionManagementForm>();
            services.AddScoped<QualityInspectionEditForm>();
            services.AddScoped<QualityInspectionItemEditForm>();
            services.AddScoped<NonconformingProductForm>();
            services.AddScoped<QualityStandardManagementForm>(); 
            */
            
            // 也可以自动注册所有窗体
            // services.AutoRegisterForms("GC_MES.WinForm");

            // 构建服务提供者
            ServiceProvider = services.BuildServiceProvider();

            // 从登录窗体启动应用
            var loginForm = ServiceProvider.GetRequiredService<LoginForm>();
            Application.Run(loginForm);
        }
    }
}
