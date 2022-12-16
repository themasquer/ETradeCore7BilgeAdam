using AppCore.Business.Services.Bases;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Repositories;

namespace Business.Services
{
    public interface ICategoryService : IService<CategoryModel> 
    {

    }

    public class CategoryService : ICategoryService 
    {
        private readonly CategoryRepoBase _categoryRepo; 

        public CategoryService(CategoryRepoBase categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
       
        public IQueryable<CategoryModel> Query() 
        {
            // Query methodu ile sorguyu oluşturup OrderBy LINQ methodu ile kategori adlarına göre artan sıralıyoruz (azalan sıra için OrderByDescending kullanılır).
            return _categoryRepo.Query(c => c.Products).OrderBy(c => c.Name).Select(c => new CategoryModel()
            {
                Description= c.Description,
                Guid= c.Guid,
                Id= c.Id,
                Name= c.Name,
                ProductsCountDisplay = c.Products.Count
            });
        }

        public Result Add(CategoryModel model)
        {
            throw new NotImplementedException();
        }

        public Result Update(CategoryModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _categoryRepo.Dispose();
        }
    }
}
