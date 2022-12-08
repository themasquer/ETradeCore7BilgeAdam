using AppCore.Business.Services.Bases;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Repositories;

namespace Business.Services
{
    public interface IProductService : IService<ProductModel> // IProductService ProductModel tipi üzerinden IService'i implemente eden ve methodlarında
                                                              // ProductModel <-> Product dönüşümlerini yaparak Product Repository'si üzerinden
                                                              // CRUD işlemleri için oluşturulan bir interface'tir.
    {
    }

    public class ProductService : IProductService // ProductService IProductService'i implemente eden ve MVC projesindeki Program.cs IoC Container'ında
                                                  // bağımlılığı IProductService ile yönetilecek ve bu sayede ilgili controller'lara constructor üzerinden
                                                  // new'lenerek enjekte edilerek kullanılacak concrete (somut) bir class'tır.
    {
        private readonly ProductRepoBase _productRepo; // CRUD işlemleri için repository'i tanımlayıp IoC Container ile constructor üzerinden enjekte edilecek objeyi bu repository'e atıyoruz.

        public ProductService(ProductRepoBase productRepo)
        {
            _productRepo = productRepo;
        }

        public IQueryable<ProductModel> Query() // Read işlemi: repository üzerinden entity ile aldığımız verileri modele dönüştürerek veritabanı sorgumuzu oluşturuyoruz.
                                                // Bu method sadece sorgu oluşturur ve döner, çalıştırmaz. Çalıştırmak için ToList, SingleOrDefault vb. methodlar kullanılmalıdır.
        {
            // Repository üzerinden entity sorgusunu (Query) oluşturup, sorguya ürünün kategorisini de parametre üzerinden dahil ediyoruz ki
            // (Entity Framework Eager Loading yani ihtiyaca göre ilişkili entity referanslarını sorguya dahil etme) aşağıda kategori adına ulaşabilelim.
            // Daha sonra Select ile sorgu kolleksiyonundaki her bir entity için model dönüşümünü gerçekleştiriyoruz (projeksiyon işlemi).
            return _productRepo.Query(product => product.Category).Select(product => new ProductModel()
            {
                // Entity özelliklerinin modeldeki karşılıklarının atanması (mapping işlemi), mapping işlemleri için AutoMapper kütüphanesi kullanılabilir.
                CategoryId = product.CategoryId,
                Description = product.Description,
                ExpirationDate = product.ExpirationDate,
                Guid = product.Guid,
                Id = product.Id,
                Name = product.Name,
                StockAmount = product.StockAmount,
                UnitPrice = product.UnitPrice,

                // View'da kullanıcıya gösterilecek özelliklerin (Display ile biten) atanması (mapping işlemi).
                UnitPriceDisplay = product.UnitPrice.ToString("C2"), // C: Currency (para birimi), N: Number (sayı) formatlama için kullanılır.
                                                                     // 2: ondalıktan sonra kaç hane olacağını belirtir.
                                                                     // Bölgesel ayarı MVC katmanında Program.cs'de yönettiğimiz için burada CultureInfo objesini kullanmaya gerek yoktur.

                //ExpirationDateDisplay = product.ExpirationDate != null ? product.ExpirationDate.Value.ToString("yyyy/MM/dd") : "", // nullable özellikler için 1. yöntem 
                ExpirationDateDisplay = product.ExpirationDate.HasValue ? product.ExpirationDate.Value.ToString("yyyy/MM/dd") : "", // nullable özellikler için 2. yöntem
                // Sıralama view'da kullanacağımız Javascript - CSS kütüphanesinde düzgün çalışsın diye yıl/ay/gün formatını kullandık.

                CategoryNameDisplay = product.Category.Name // Ürünün ilişkili kategori referansı üzerinden adına ulaşıp özelliğe atadık.
            });
        }

        public Result Add(ProductModel model)
        {
            throw new NotImplementedException();
        }

        public Result Update(ProductModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose() // Bu servisle işimiz bittiğinde IoC Container'da belirttiğimiz obje yaşam döngüsü üzerinden dispose edilecek (çöpe atılacak), dolayısıyla
                              // bu servis dispose edilirken repository'i de dispose ediyoruz ki içerisindeki dbContext objesi de dispose edilsin.
        {
            _productRepo.Dispose();
        }
    }
}
