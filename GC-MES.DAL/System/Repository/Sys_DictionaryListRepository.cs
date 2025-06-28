

using GC_MES.DAL;
using GC_MES.DAL.System.IRepository;
using SqlSugar;

namespace iMES.DAL.System.Repositories
{
    public class Sys_DictionaryListRepository : BaseRepository<Sys_DictionaryList>, ISys_DictionaryListRepository
    {
        public Sys_DictionaryListRepository(ISqlSugarClient sqlSugar) : base(sqlSugar)
        {

        }
    }
}
