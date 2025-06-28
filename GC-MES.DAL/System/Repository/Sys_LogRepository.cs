

using GC_MES.DAL;
using GC_MES.DAL.System.IRepository;
using SqlSugar;

namespace GC_MES.DAL.System.Repository
{
    public partial class Sys_LogRepository : BaseRepository<Sys_Log>, ISys_LogRepository
    {

        public Sys_LogRepository(ISqlSugarClient sqlSugar) : base(sqlSugar)
        {
        }

    }
}

