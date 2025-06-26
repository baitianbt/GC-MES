using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_MES.Core.Extensions
{
    public static class ServiceProviderManagerExtension
    {
        public static object GetService(this Type serviceType)
        {
            // HttpContext.Current.RequestServices.GetRequiredService<T>(serviceType);
            return Utilities.HttpContext.Current.RequestServices.GetService(serviceType);
        }

    }
}
