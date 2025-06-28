using GC_MES.DAL.System.IRepository;
using SqlSugar;

namespace GC_MES.DAL.System.Repository
{


    public partial class vSys_DictionaryRepository : BaseRepository<vSys_Dictionary>, IvSys_DictionaryRepository
    {

        public vSys_DictionaryRepository(ISqlSugarClient sqlSugar) : base(sqlSugar)
        {
        }

    }
}

