using GC_MES.BLL.System.IService;
using GC_MES.DAL.System.IRepository;
using GC_MES.DAL.UnitOfWork;
using GC_MES.Model.System;

namespace GC_MES.BLL.System.Service
{


    public class Sys_VersionInfoService : BaseServices<Sys_VersionInfo>, ISys_VersionInfoService
    {
        private readonly ISys_VersionInfoRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public Sys_VersionInfoService(IUnitOfWork unitOfWork, ISys_VersionInfoRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
