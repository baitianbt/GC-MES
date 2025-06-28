using GC_MES.BLL.System.IService;
using GC_MES.DAL.System.IRepository;
using GC_MES.DAL.UnitOfWork;
using GC_MES.Model.System;

namespace GC_MES.BLL.System.Service
{


    public class Sys_DictionaryListService : BaseServices<Sys_DictionaryList>, ISys_DictionaryListService
    {
        private readonly ISys_DictionaryListRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public Sys_DictionaryListService(IUnitOfWork unitOfWork, ISys_DictionaryListRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
