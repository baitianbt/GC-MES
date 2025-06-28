using GC_MES.DAL.System.IRepository;
using SqlSugar;

namespace GC_MES.DAL.System.Repository
{


    public partial class Sys_UserRepository : BaseRepository<Sys_User>, ISys_UserRepository
    {

        public Sys_UserRepository(ISqlSugarClient sqlSugar) : base(sqlSugar)
        {
        }

    }
}

