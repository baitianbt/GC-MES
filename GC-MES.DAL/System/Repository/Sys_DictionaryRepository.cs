
using GC_MES.DAL;
using GC_MES.DAL.System.IRepository;
using SqlSugar;


namespace GC_MES.DAL.System.Repository
{
    public partial class Sys_DictionaryRepository : BaseRepository<Sys_Dictionary>, ISys_DictionaryRepository
    {
        public Sys_DictionaryRepository(ISqlSugarClient sqlSugar) : base(sqlSugar)
        {

        }
    }
}

