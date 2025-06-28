using GC_MES.DAL.System.IRepository;
using SqlSugar;

namespace GC_MES.DAL.System.Repository
{


    public partial class Sys_VersionInfoRepository : BaseRepository<Sys_VersionInfo>, ISys_VersionInfoRepository
    {

        public Sys_VersionInfoRepository(ISqlSugarClient sqlSugar) : base(sqlSugar)
        {
        }

    }
}

