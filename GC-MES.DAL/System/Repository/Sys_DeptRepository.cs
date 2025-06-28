
using GC_MES.DAL.System.IRepository;
using GC_MES.Model.System;
using SqlSugar;

namespace GC_MES.DAL.System.Repositories
{
    public  class Sys_DeptRepository : BaseRepository<Sys_Dept>, ISys_DeptRepository
    {
        public Sys_DeptRepository(ISqlSugarClient sqlSugar) : base(sqlSugar)
        {

        }
    }
}