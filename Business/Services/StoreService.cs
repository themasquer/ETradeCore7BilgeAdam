using AppCore.Business.Services.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace Business.Services
{
    public interface IStoreService : IService<StoreModel>
    {
    }

    public class StoreService : IStoreService
    {
        private readonly StoreRepoBase _storeRepo;

        public StoreService(StoreRepoBase storeRepo)
        {
            _storeRepo = storeRepo;
        }

        public Result Add(StoreModel model)
        {
            if (Query().Any(s => s.Name.ToUpper() == model.Name.ToUpper().Trim()))
                return new ErrorResult("Store can't be added because store with the same name exists!");
            var entity = new Store()
            {
                IsVirtual = model.IsVirtual,
                Name = model.Name.Trim()
            };
            _storeRepo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            _storeRepo.Delete(id);
            return new SuccessResult();
        }

        public void Dispose()
        {
            _storeRepo.Dispose();
        }

        public IQueryable<StoreModel> Query()
        {
            return _storeRepo.Query().OrderBy(s => s.IsVirtual).ThenBy(s => s.Name).Select(s => new StoreModel()
            {
                Guid = s.Guid,
                Id = s.Id,
                Name = s.Name,
                IsVirtual = s.IsVirtual,
                VirtualDisplay = s.IsVirtual ? "Yes" : "No"
            });
        }

        public Result Update(StoreModel model)
        {
            if (Query().Any(s => s.Name.ToUpper() == model.Name.ToUpper().Trim() && s.Id != model.Id))
                return new ErrorResult("Store can't be added because store with the same name exists!");
            var entity = new Store()
            {
                Id = model.Id,
                IsVirtual = model.IsVirtual,
                Name = model.Name.Trim()
            };
            _storeRepo.Update(entity);
            return new SuccessResult();
        }
    }
}
