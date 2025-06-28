using GC_MES.BLL.System.IService;
using GC_MES.DAL.System.IRepository;
using GC_MES.DAL.UnitOfWork;
using GC_MES.Model.System;

namespace GC_MES.BLL.System.Service
{


    public class vSys_DictionaryService : BaseServices<vSys_Dictionary>, IvSys_DictionaryService
    {
        private readonly IvSys_DictionaryRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public vSys_DictionaryService(IUnitOfWork unitOfWork, IvSys_DictionaryRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }
    }
}
