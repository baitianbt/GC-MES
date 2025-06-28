using GC_MES.BLL.System.IService;
using GC_MES.DAL.System.IRepository;
using GC_MES.DAL.UnitOfWork;
using GC_MES.Model.System;

namespace GC_MES.BLL.System.Service
{


    public class Sys_DictionaryService : BaseServices<Sys_Dictionary>, ISys_DictionaryService
    {
        private readonly ISys_DictionaryRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public Sys_DictionaryService(IUnitOfWork unitOfWork, ISys_DictionaryRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
