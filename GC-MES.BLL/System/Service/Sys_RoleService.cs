using GC_MES.BLL.System.IService;
using GC_MES.DAL.System.IRepository;
using GC_MES.DAL.UnitOfWork;
using GC_MES.Model.System;

namespace GC_MES.BLL.System.Service
{


    public class Sys_RoleService : BaseServices<Sys_Role>, ISys_RoleService
    {
        private readonly ISys_RoleRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public Sys_RoleService(IUnitOfWork unitOfWork, ISys_RoleRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
