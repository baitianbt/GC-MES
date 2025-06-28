using GC_MES.BLL.System.IService;
using GC_MES.DAL.System.IRepository;
using GC_MES.DAL.UnitOfWork;
using GC_MES.Model.System;

namespace GC_MES.BLL.System.Service
{


    public class Sys_LogService : BaseServices<Sys_Log>, ISys_LogService
    {
        private readonly ISys_LogRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public Sys_LogService(IUnitOfWork unitOfWork, ISys_LogRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
