using GC_MES.DAL.System.IRepository;
using SqlSugar;

namespace GC_MES.DAL.System.Repository
{


    public partial class Sys_RoleRepository : BaseRepository<Sys_Role>, ISys_RoleRepository
    {

        public Sys_RoleRepository(ISqlSugarClient sqlSugar) : base(sqlSugar)
        {
        }

    }
}

