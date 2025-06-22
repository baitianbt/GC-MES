using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GC_MES.WinForm.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AutoRegisterServices(this IServiceCollection services, string assemblyName, string classSuffix)
        {
            var assembly = Assembly.Load(assemblyName);
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith(classSuffix));

            foreach (var type in types)
            {
                var interfaceType = type.GetInterface("I" + type.Name); // 按接口命名习惯匹配
                if (interfaceType != null)
                    services.AddScoped(interfaceType, type);
                else
                    services.AddScoped(type); // 无接口也注入类型本身
            }
        }

        public static void AutoRegisterForms(this IServiceCollection services, string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
            var forms = assembly.GetTypes()
                .Where(t => typeof(Form).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var formType in forms)
            {
                services.AddScoped(formType);
            }
        }
    }

}
