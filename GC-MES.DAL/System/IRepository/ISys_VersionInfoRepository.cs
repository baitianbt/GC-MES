
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_MES.Core.BaseProvider;
using GC_MES.Core.Extensions.AutofacManager;
using GC_MES.Model.System;


namespace GC_MES.System.IRepositories
{
    public partial interface ISys_VersionInfoRepository : IDependency,IRepository<Sys_VersionInfo>
    {
    }
}
