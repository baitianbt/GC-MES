using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GC_MES.Upgrade
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddDbContext<MesDbContext>(options =>
                options.UseSqlServer(AppSettings.ConnectionString));

            services.AddScoped<WorkOrderRepository>();
            services.AddScoped<WorkOrderService>();
            services.AddScoped<MainForm>();

            var provider = services.BuildServiceProvider();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(provider.GetRequiredService<MainForm>());
        }
    }
}
