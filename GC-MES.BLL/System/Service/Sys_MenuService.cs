using GC_MES.BLL.System.IService;
using GC_MES.DAL.System.IRepository;
using GC_MES.DAL.UnitOfWork;
using GC_MES.Model.System;

namespace GC_MES.BLL.System.Service
{


    public class Sys_MenuService : BaseServices<Sys_Menu>, ISys_MenuService
    {
        private readonly ISys_MenuRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public Sys_MenuService(IUnitOfWork unitOfWork, ISys_MenuRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
