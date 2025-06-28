using GC_MES.BLL.System.IService;
using GC_MES.DAL.System.IRepository;
using GC_MES.DAL.UnitOfWork;
using GC_MES.Model.System;

namespace GC_MES.BLL.System.Service
{


    public class Sys_UserService : BaseServices<Sys_User>, ISys_UserService
    {
        private readonly ISys_UserRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public Sys_UserService(IUnitOfWork unitOfWork, ISys_UserRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
