using AppCore.Business.Services.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Entities;
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
            // Repository üzerinden entity sorgusunu (Query) oluşturup, sorguya ürünün kategorisini ve ürün mağaza ilişkilerini de parametre üzerinden dahil ediyoruz ki
            // (Entity Framework Eager Loading yani ihtiyaca göre ilişkili entity referanslarını sorguya dahil etme) aşağıda kategori adına ve mağaza adları ile id'lerine ulaşabilelim.
            // Eğer istenirse Entity Framework Lazy Loading projede aktif hale getirilerek hiç bir include (sorguya dahil etme) işlemi yapılmadan
            // tüm ilişkili entity referans verileri çekilebilir.
            // Daha sonra Select ile sorgu kolleksiyonundaki her bir entity için model dönüşümünü gerçekleştiriyoruz (projeksiyon işlemi).
            var query = _productRepo.Query(product => product.Category, product => product.ProductStores).Select(product => new ProductModel()
            {
                // Entity özelliklerinin modeldeki karşılıklarının atanması (mapping işlemi), istenirse mapping işlemleri için AutoMapper kütüphanesi kullanılabilir.
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

                CategoryNameDisplay = product.Category.Name, // Ürünün ilişkili kategori referansı üzerinden adına ulaşıp özelliğe atadık.

                StoreNamesDisplay = string.Join("<br />", product.ProductStores.Select(productStore => productStore.Store.Name + (productStore.Store.IsDeleted ? " (Deleted)" : ""))), 
                // ürünün mağaza adlarını string Join methodu ile <br /> (alt satır HTML tag'i) ayracı üzerinden tek bir string olarak birleştirir,
                // Select methodundaki Lambda Expression ile ürünün her bir ilişkili ürün mağazası üzerinden ilişkili mağazasına ulaşıp adlarını string tipinde
                // bir kolleksiyon olarak çektik, kolleksiyonda silinen mağazalar için mağaza adı sonuna parantez içerisinde Deleted (silindi) bilgisini yazdırdık

                StoreIds = product.ProductStores.Select(productStore => productStore.StoreId).ToList()
                // ürün ekleme ve güncelleme işlemlerinde doldurulan tüm mağaza listesinde kullanıcının daha önce kaydetmiş olduğu mağazaları id'leri üzerinden çekiyoruz,
                // Select methodundaki Lambda Expression ile ürünün her bir ilişkili ürün mağazası üzerinden mağaza id'lerine ulaşıp int listesi tipinde
                // bir kolleksiyon olarak çektik
            });

            //query = query.OrderBy(product => product.CategoryNameDisplay).ThenBy(product => product.Name); // Önce kategori adına göre artan daha sonra da ürün adına göre artan sıralar.
            query = query.OrderBy(product => product.Name); // Ürün adına göre artan sıralar.
            // model özellikleri üzerinden sıralama yapılmak isteniyorsa ilk önce OrderBy (veya azalan sıra için OrderByDescending), başka özellikler de sıralamaya dahil
            // edilmek isteniyorsa bir veya daha fazla ThenBy (veya azalan sıra için ThenByDescending) LINQ methodları kullanılabilir.

            return query;
        }

        public Result Add(ProductModel model) // Create işlemi: model kullanıcının view üzerinden doldurup gönderdiği objedir 
        {
            // Önce model üzerinden girilen ürün adına sahip entity veritabanındaki tabloda var mı diye kontrol ediyoruz, eğer varsa işleme izin vermeyip ErrorResult objesi dönüyoruz
            // 1. yöntem:
            //var product = Query().SingleOrDefault(p => p.Name.ToUpper() == model.Name.ToUpper().Trim()); 
            // büyük küçük harf duyarlılığını ortadan kaldırmak için iki tarafta da ToUpper kullandık (ToLower da kullanılabilir) ve kullanıcının gönderdiği
            // verinin Trim ile başındaki ve sonundaki boşlukları temizledik

            //if (product != null) // if (product is not null) da kullanılabilir, eğer bu ada sahip ürün objesi varsa
            //    return new ErrorResult("Product with the same name exists!"); // bu ürün adına sahip kayıt bulunmaktadır mesajını içeren ErrorResult objesini dönüyoruz
                                                                                // ki ilgili controller action'ında kullanabilelim.



            // 2. yöntem:
            if (Query().Any(p => p.Name.ToUpper() == model.Name.ToUpper().Trim())) // Any LINQ methodu belirtilen koşul veya koşullara sahip herhangi bir kayıt var mı diye
                                                                                   // veritabanındaki tabloda kontrol eder, varsa true yoksa false döner.
                                                                                   // All LINQ methodu ise belirtilen koşul veya koşullara veritabanındaki tabloda tüm kayıtlar
                                                                                   // uyuyor mu diye kontrol eder, uyuyorsa true uymuyorsa false döner.
                return new ErrorResult("Product can't be added because product with the same name exists!"); 
                // bu ürün adına sahip kayıt bulunmaktadır mesajını içeren ErrorResult objesini dönüyoruz ki ilgili controller action'ında kullanabilelim.



            Product entity = new Product() // bu satırda yukarıdaki koşula uyan kayıt bulunmadığı için kullanıcının gönderdiği verileri içeren model objesi
                                           // üzerinden bir entity objesi oluşturuyoruz (mapping işlemi, istenirse mapping işlemleri için AutoMapper kütüphanesi kullanılabilir).
            {
                // 1. yöntem:
                //CategoryId = model.CategoryId ?? 0, // entity CategoryId özelliğine eğer modelin CategoryId'si null gelirse 0, dolu gelirse gelen değeri ata,
                                                      // bu yöntemde dikkat edilmesi gereken veritabanındaki tabloya insert işlemi yapıldığında CategoryId'nin 0 atanması durumunda
                                                      // kategori tablosunda 0 Id'li bir kategori bulunmadığından exception alınacağıdır.
                // 2. yöntem:
                CategoryId = model.CategoryId, 

                // 1. yöntem: 
                //Description = model.Description == null ? null : model.Description.Trim(), // Description verisinin null gelme ihtimali olduğundan ternary operator kullanarak
                // null gelirse entity Description özelliğine null atanmasını, null gelmezse ise entity Description özelliğine değerinin trim'lenerek atanmasını sağlıyoruz.
                // 2. yöntem:
                Description = model.Description?.Trim(), // Description verisinin null gelme ihtimali olduğundan sonuna ? ekliyoruz ki null geldiğinde Trim methodunu çalıştırmasın
                                                         // ve entity Description özelliğine null atasın, null gelmediğinde de gelen değeri entity Description özelliğine
                                                         // trim'leyerek atasın.

                ExpirationDate = model.ExpirationDate,

                Name = model.Name.Trim(), // Name ProductModel'de zorunlu olarak tanımlandığından direkt olarak değerini entity Name özelliğine atayabiliriz.

                // 1. yöntem:
                //StockAmount = model.StockAmount ?? 0, // entity StockAmount özelliğine eğer modelin StockAmount'u null gelirse 0, dolu gelirse gelen değeri ata
                // 2. yöntem:
                StockAmount = model.StockAmount.Value, // StockAmount ProductModel'de zorunlu olarak tanımlandığından direkt olarak Value ile değerine ulaşıp entity StockAmount
                                                       // özelliğine atayabiliriz.

                // 1. yöntem:
                //UnitPrice = model.UnitPrice ?? 0, // entity UnitPrice özelliğine eğer modelin UnitPrice'ı null gelirse 0, dolu gelirse gelen değeri ata
                // 2. yöntem:
                UnitPrice = model.UnitPrice.Value, // UnitPrice ProductModel'de zorunlu olarak tanımlandığından direkt olarak Value ile değerine ulaşıp entity UnitPrice
                                                   // özelliğine atayabiliriz.

                ProductStores = model.StoreIds?.Select(sId => new ProductStore()
                { 
                    StoreId = sId
                }).ToList() // ürün mağaza ilişkisi için kullanıcı tarafından model üzerinden liste olarak gönderilen her bir sId (store id) delegesi için
                            // ürünle ilişkili bir ProductStore oluşturup StoreId özelliğini delege üzerinden set ediyoruz,
                            // kullanıcının mağaza seçmemesi durumunda StoreIds null geleceği için sonuna ? ekledik
            };

            _productRepo.Add(entity); // repository üzerinden oluşturulan ürün entity'sinin save parametresini de göndermeyerek (default true göndererek)
                                      // veritabanındaki tablosuna insert edilmesini sağlıyoruz.

            return new SuccessResult("Product added successfully."); // bu satırda ekleme işlemi başarıyla bittiğinden SuccessResult objesini mesajıyla beraber dönüyoruz ki
                                                                     // ilgili controller action'ında kullanabilelim.
        }

        public Result Update(ProductModel model) // Update işlemi: model kullanıcının view üzerinden doldurup gönderdiği objedir 
		{
			if (Query().Any(p => p.Name.ToUpper() == model.Name.ToUpper().Trim() && p.Id != model.Id)) 
				return new ErrorResult("Product can't be updated because product with the same name exists!");
                // güncellenen ürün dışında (yukarıda Id üzerinden bu koşulu ekledik) bu ürün adına sahip kayıt bulunmaktadır mesajını içeren ErrorResult objesini
                // dönüyoruz ki ilgili controller action'ında kullanabilelim.

            _productRepo.Delete<ProductStore>(ps => ps.ProductId == model.Id); // önce ürünün ilişkili ürün mağaza kayıtlarını repository üzerinden siliyoruz

			Product entity = new Product() // bu satırda yukarıdaki koşullara uyan kayıt bulunmadığı için kullanıcının gönderdiği verileri içeren model objesi
										   // üzerinden bir entity objesi oluşturuyoruz (mapping işlemi).
			{
                Id = model.Id, // entity'nin Id özelliğini mutlaka atamalıyız ki Entity Framework veritabanı tablosunda hangi kaydın güncelleneceğini bilsin.

				CategoryId = model.CategoryId, 
				
				Description = model.Description?.Trim(), // Description verisinin null gelme ihtimali olduğundan sonuna ? ekliyoruz ki null geldiğinde Trim methodunu çalıştırmasın
														 // ve entity Description özelliğine null atasın, null gelmediğinde de gelen değeri entity Description özelliğine
														 // trim'leyerek atasın.

				ExpirationDate = model.ExpirationDate,

				Name = model.Name.Trim(), // Name ProductModel'de zorunlu olarak tanımlandığından direkt olarak değerini entity Name özelliğine atayabiliriz.
				
				StockAmount = model.StockAmount.Value, // StockAmount ProductModel'de zorunlu olarak tanımlandığından direkt olarak Value ile değerine ulaşıp entity StockAmount
													   // özelliğine atayabiliriz.
				
				UnitPrice = model.UnitPrice.Value, // UnitPrice ProductModel'de zorunlu olarak tanımlandığından direkt olarak Value ile değerine ulaşıp entity UnitPrice
												   // özelliğine atayabiliriz.

                ProductStores = model.StoreIds?.Select(sId => new ProductStore()
                {
                    StoreId = sId
                }).ToList() // ürün mağaza ilişkisi için kullanıcı tarafından model üzerinden liste olarak gönderilen her bir sId (store id) delegesi için
							// ürünle ilişkili bir ProductStore oluşturup StoreId özelliğini delege üzerinden set ediyoruz,
							// kullanıcının mağaza seçmemesi durumunda StoreIds null geleceği için sonuna ? ekledik
			};

			_productRepo.Update(entity); // repository üzerinden oluşturulan ürün entity'sinin save parametresini de göndermeyerek (default true göndererek)
									     // veritabanındaki tablosunda update edilmesini sağlıyoruz.

			return new SuccessResult("Product updated successfully."); // bu satırda güncelleme işlemi başarıyla bittiğinden SuccessResult objesini mesajıyla beraber dönüyoruz ki
																	   // ilgili controller action'ında kullanabilelim.
		}

		public Result Delete(int id) // Delete işlemi: Genelde id üzerinden yapılır
        {
            _productRepo.Delete<ProductStore>(ps => ps.ProductId == id); // önce ürünün ilişkili ürün mağaza kayıtlarını repository üzerinden siliyoruz

            // 1. yöntem:
            //_productRepo.Delete(p => p.Id == id); // repository'de koşul (predicate) parametresi kullanan Delete methodunu Lambda Expression parametresi ile çağırabiliriz.
            // 2. yöntem:
            _productRepo.Delete(id); // silme işlemleri genelde id üzerinden yapıldığı ve kendimize kolaylık olması için RepoBase'de id parametresi üzerinden
                                     // veritabanından kayıt silme işlemi yapan bir method oluşturmuştuk, bu methodu id parametresi ile çağırmak daha uygun olacaktır.

            return new SuccessResult("Product deleted successfully."); // bu satırda silme işlemi başarıyla bittiğinden SuccessResult objesini mesajıyla beraber dönüyoruz ki
                                                                       // ilgili controller action'ında kullanabilelim.
        }

        public void Dispose() // Bu servisle işimiz bittiğinde IoC Container'da belirttiğimiz obje yaşam döngüsü üzerinden dispose edilecek (çöpe atılacak), dolayısıyla
                              // bu servis dispose edilirken repository'i de dispose ediyoruz ki içerisindeki dbContext objesi de dispose edilsin.
        {
            _productRepo.Dispose();
        }
    }
}
