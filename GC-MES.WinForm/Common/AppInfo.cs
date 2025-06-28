using GC_MES.Model.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_MES.WinForm.Common
{
    public class AppInfo
    {
        public static Sys_User CurrentUser { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }

        public static List<Sys_Menu> Menus { get; set; } = new List<Sys_Menu>();
    }
}
