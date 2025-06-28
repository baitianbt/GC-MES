using GC_MES.BLL.System.IService;
using GC_MES.DAL.System.IRepository;
using GC_MES.DAL.UnitOfWork;
using GC_MES.Model.System;

namespace GC_MES.BLL.System.Service
{
   

    public class Sys_DeptService : BaseServices<Sys_Dept>, ISys_DeptService
    {
        private readonly ISys_DeptRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public Sys_DeptService(IUnitOfWork unitOfWork, ISys_DeptRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
