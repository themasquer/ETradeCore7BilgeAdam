using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MvcWebUI.Areas.Database.Controllers
{
    // Area'lar bir MVC projesindeki küçük MVC modülleri olarak düşünülebilir. Mutlaka oluşturulmaları şart değildir. Oluşturulduğunda projenin daha kolay yönetilmesini sağlar.
    // Herhangi bir area'nın controller'ında mutlaka aşağıdaki Area attribute'u tanımlanmış olmalıdır ve Program.cs içerisinde default MVC route tanımı (controller/action/id?)
    // yapılan yerin üzerine area scaffolding (otomatik yapıyı kodlar veya klasörler üzerinden oluşturma) tarafından projenin içerisine eklenen ScaffoldingReadMe.txt
    // dosyasındaki area'lar için route tanım kod bloğu eklenmelidir.

    [Area("Database")]
    public class HomeController : Controller
    {
        private readonly ETradeContext _db; // bu controller'ı kendimize development ortamında ilk verileri oluşturması bakımından kolaylık sağlaması
                                            // için oluşturduğumuzdan direkt DbContext objesini constructor üzerinden enjekte edip Index action'ında kullanıyoruz.

        public HomeController(ETradeContext db)
        {
            _db = db;
        }

        public IActionResult Index() // tarayıcıda ~/Database/Home/Index, ~/Database/Home veya ~/Database adresi girilerek veya herhangi bir view'da
                                     // (örneğin Views -> Shared klasörü altındaki _Layout.cshtml) bu adres için area da dikkate alınarak link oluşturularak çağrılabilir.
        {
            #region Mevcut verilerin silinmesi
            var products = _db.Products.ToList(); // önce ürün listesini çekip daha sonra ürünler DbSet'i üzerinden RemoveRange methodu ile silinmesini sağlıyoruz
            _db.Products.RemoveRange(products);

            var categories = _db.Categories.ToList();
            _db.Categories.RemoveRange(categories);
            #endregion

            #region İlk verilerin oluşturulması
            _db.Categories.Add(new Category() // daha sonra kategoriler DbSet'i üzerinden içerisindeki ürünlerle beraber kategorileri ekliyoruz
            {
                Name = "Computer",
                Description = "Laptops, desktops and computer peripherals",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Laptop",
                        UnitPrice = 3000.5,
                        ExpirationDate = new DateTime(2032, 1, 27),
                        StockAmount = 10
                    },
                    new Product()
                    {
                        Name = "Mouse",
                        UnitPrice = 20.5,
                        StockAmount = 50,
                        Description = "Computer peripheral"
                    },
                    new Product()
                    {
                        Name = "Keyboard",
                        UnitPrice = 40,
                        StockAmount = 45,
                        Description = "Computer peripheral"
                    },
                    new Product()
                    {
                        Name = "Monitor",
                        UnitPrice = 2500,
                        ExpirationDate = DateTime.Parse("05/19/2027"),
                        StockAmount = 20,
                        Description = "Computer peripheral"
                    }
                }
            });

            _db.Categories.Add(new Category()
            {
                Name = "Home Theater System",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Speaker",
                        UnitPrice = 2500,
                        StockAmount = 70
                    },
                    new Product()
                    {
                        Name = "Receiver",
                        UnitPrice = 5000,
                        StockAmount = 30,
                        Description = "Home theater system component"
                    },
                    new Product()
                    {
                        Name = "Equalizer",
                        UnitPrice = 1000,
                        StockAmount = 40
                    }
                }
            });
            #endregion

            #region DbSet'ler üzerinden yapılan değişikliklerin tek seferde veritabanına yansıtılması
            _db.SaveChanges();
            #endregion

            //return Content("Database seed successful."); // *1
            //return Content("<label style=\"color:red;\"><b>Database seed successful.</b></label>", "text/html"); // *2
            return Content("<label style=\"color:red;\"><b>Database seed successful.</b></label>", "text/html", Encoding.UTF8); // *3
            // Yeni bir view oluşturup işlem sonucunu göstermek yerine Controller base class'ının Content methodunu kullanıp işlem sonucunun boş bir sayfada gösterilmesini sağladık.
            // *1: Metinsel verinin düz yazı (plain text) olarak sayfada gösterilmesini sağlar.
            // *2: HTML verisinin sayfada HTML içerik olarak gösterilmesini sağlar.
            // *3: HTML verisinin sayfada Türkçe karakterleri destekleyen UTF8 karakter kümesi üzerinden HTML içerik olarak gösterilmesini sağlar.
        }
    }
}
