using AppCore.Business.Services.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Entities;
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
                ProductsCount = c.Products.Count
            });
        }

        public Result Add(CategoryModel model)
        {
            if (_categoryRepo.Query().Any(c => c.Name.ToLower() == model.Name.ToLower().Trim())) // eğer bu ada sahip kategori varsa
                return new ErrorResult("Category can't be added because category with the same name exists!");
            var category = new Category()
            {
                Description = model.Description?.Trim(), // Description'ın null gelebilme ihtimali için sonunda ? kullanıyoruz
                Name = model.Name.Trim() // Name modelde zorunlu olduğundan null gelebilme ihtimali yok
            };
            _categoryRepo.Add(category);
            return new SuccessResult(); // mesaj kullanmadan bir SuccessResult objesi oluşturduk ve döndük
        }

        public Result Update(CategoryModel model)
        {
            if (_categoryRepo.Query().Any(c => c.Name.ToLower() == model.Name.ToLower().Trim() && c.Id != model.Id)) // eğer düzenlediğimiz kategori dışında (Id koşulu üzerinden) bu ada sahip kategori varsa
                return new ErrorResult("Category can't be updated because category with the same name exists!");
            var category = new Category()
            {
                Id = model.Id, // güncelleme işlemi için mutlaka Id set edilmeli
                Description = model.Description?.Trim(), // Description'ın null gelebilme ihtimali için sonunda ? kullanıyoruz
                Name = model.Name.Trim() // Name modelde zorunlu olduğundan null gelebilme ihtimali yok
            };
            _categoryRepo.Update(category);
            return new SuccessResult(); // mesaj kullanmadan bir SuccessResult objesi oluşturduk ve döndük
        }

        public Result Delete(int id)
        {
            var category = Query().SingleOrDefault(c => c.Id == id); // yukarıda daha önce oluşturduğumuz Query methodu üzerinden id ile kategoriyi çekiyoruz
            if (category.ProductsCount > 0) // eğer çektiğimiz kategorinin ürünleri varsa veritabanındaki ürün ve kategori tabloları arasındaki ilişki Entity Framework
                                            // code first yaklaşımında otomatik cascade olarak oluşturulduğundan silinen kategorinin tüm ürünleri silinecektir,
                                            // bunu engellemek için yukarıdaki Query methodunda Products'ı da sorguya dahil etmiş ve bunun sonucunda
                                            // her bir kategori için bir ProductsCount ataması yapmıştık, dolayısıyla bu özellik üzerinden kontrol ederek
                                            // ilişkili ürün kayıtları varsa silme işlemine izin vermiyoruz.
                return new ErrorResult("Category can't be deleted because category has products!");
            _categoryRepo.Delete(id);
            return new SuccessResult("Category deleted successfully.");
        }

        public void Dispose()
        {
            _categoryRepo.Dispose();
        }
    }
}
