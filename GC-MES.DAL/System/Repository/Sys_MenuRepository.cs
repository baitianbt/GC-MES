using GC_MES.DAL.System.IRepository;
using SqlSugar;

namespace GC_MES.DAL.System.Repositories
{
   

    public partial class Sys_MenuRepository : BaseRepository<Sys_Menu>, ISys_MenuRepository
    {

        public Sys_MenuRepository(ISqlSugarClient sqlSugar) : base(sqlSugar)
        {
        }

    }
}

