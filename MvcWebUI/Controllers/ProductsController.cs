using AppCore.Results.Bases;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcWebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService; // controller'da ürünle ilgili işleri gerçekleştirebilmek için servis alanı tanımlanır ve constructor üzerinden enjekte edilir.

        private readonly ICategoryService _categoryService; // controller'da kategori ile ilgili işleri gerçekleştirebilmek için servis alanı tanımlanır ve constructor üzerinden enjekte edilir.

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }



        public IActionResult Index() // tarayıcıda ~/Products/Index adresi girilerek veya herhangi bir view'da (örneğin Views -> Shared klasörü altındaki _Layout.cshtml)
                                     // bu adres için link oluşturularak çağrılabilir.
        {
            var products = _productService.Query().ToList(); // ToList LINQ (Language Integrated Query) methodu sorgunun çalıştırılmasını ve sonucunun IQueryable'da kullanılan tip
                                                             // üzerinden bir liste olarak dönmesini sağlar.

            //return View("ProductList", products); // *1
            return View(products); // *2
            // *1: Eğer istenirse dönen liste Views altındaki Products klasörüne ProductList.cshtml adlı view oluşturularak bu view'a model olarak gönderilebilir
            // *2: veya dönen liste Views altındaki Products klasöründeki Index.cshtml view'ına model olarak gönderilir.
            // View'a action'dan sadece tek bir model gönderilebilir.
        }



        public IActionResult Details(int id) // örneğin tarayıcıda ~/Products/Details/1 adresi girilerek veya herhangi bir view'da (örneğin Views -> Products -> Index.cshtml)
                                             // bu adres için id parametresini de gönderen bir link oluşturularak çağrılabilir.
        {
            var product = _productService.Query().SingleOrDefault(p => p.Id == id); // parametre olarak bu action'a gönderilen id üzerinden kayıt sorgulanır
            /*
            SingleOrDefault LINQ methodu kullanılarak ID üzerinden tek bir kayda ulaşılabilir.
            SingleOrDefault lambda expression kullanılarak belirtilen koşul veya koşullar üzerinden tek bir kayıt döner,
            eğer sorgu sonucunda birden çok kayıt dönerse exception fırlatır, eğer belirtilen koşula sahip
            kayıt bulamazsa null referansı döner.
            Single, SingleOrDefault yerine kullanılabilir, eğer belirtilen koşula sahip birden çok kayıt bulursa
            veya kayıt bulamazsa exception fırlatır.
            */
            /*
            SingleOrDefault yerine FirstOrDefault LINQ methodu da kullanılabilir.
            FirstOrDefault lamda expression kullanılarak belirtilen koşul veya koşullar üzerinden sorgu sonucunda
            tek kayıt dönse de birden çok kayıt dönse de her zaman ilk kaydı döner,
            eğer kayıt bulunamazsa null referansı döner.
            First, FirstOrDefault yerine kullanılabilir, eğer belirtilen koşula sahip kayıt bulunamazsa
            exception fırlatır.
            LastOrDefault ve Last methodları da FirstOrDefault ve First methodlarının tersi düşünülerek
            belirtilen koşul veya koşullara göre bulunan son kayıt üzerinden işleme devam eder.
            */
            /*
            DbContext objesindeki DbSet'ler üzerinden SingleOrDefault'a alternatif olarak Find methodu kullanılabilir ve 
            parametre olarak bir veya daha fazla anahtar (primary key) kullanılarak tek bir kayda (objeye) ulaşılabilir. 
            */
            /*
            Where LINQ methodu ile kayıtlar lamda expression kullanılarak bir veya daha fazla
            koşul üzerinden filtrelenerek kolleksiyon olarak geri dönülebilir.
            Koşullarda && (and), || (or) ile ! (not) operatorleri istenirse bir arada kullanılabilir.
            Bu operatörler ile oluşturulan koşullar SingleOrDefault, Single, FirstOrDefault, First,
            LastOrDefault ve Last gibi methodlarda da kullanılabilir.
            */

            //if (product == null) // bu veya bir alt satırdaki şekilde null kontrolü yapılabilir
            if (product is null) // eğer sorgu sonucunda kayıt bulunamadıysa
            {
                //return NotFound(); // 404 HTTP durum kodu üzerinden kaynak bulunamadı HTTP response'u (yanıtı) dönülebilir
                return View("_Error", "Product not found!"); // alternatif olarak tüm projede tüm controller action'larında alınabilecek hatalar için Views -> Shared klasörü altına
                                                             // _Error.cshtml paylaşılan view'ı oluşturularak alınan hatalar yazdırılabilir
            }
            return View(product); // kayıt bulunduysa Details view'ına model olarak gönderilir
        }



        [HttpGet] // Bu aksiyonun HTTP GET yani sunucudan kaynak getirme işlemini yapacağını belirten Action Method Selector'ıdır.
                  // Yazmak zorunlu değildir çünkü yazılmazsa default HttpGet methodu action'larda kullanılır.
        public IActionResult Create() // tarayıcıda ~/Products/Create adresi girilerek veya herhangi bir view'da (örneğin Views -> Producs klasörü altındaki Index.cshtml)
                                      // bu adres için link oluşturularak çağrılabilir. Create view'ındaki form kullanıcıya dönülür ki kullanıcı veri girip sunucuya gönderebilsin.
                                      // Veritabanında yeni kayıt oluşturmak için kullanılır.
        {
            ViewBag.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            // Eğer view'da kullanılacak model'den farklı bir tipte veriye ihtiyaç varsa ViewBag veya ViewData üzerinden gerek action'dan view'a gerekse view'lar arası
            // model verisi dışındaki veriler taşınabilir.
            // ViewBag ile ViewData aynı yapı olarak birbirlerinin yerlerine kullanılabilir, sadece kullanımları farklıdır. Örneğin ViewData["Categories"] olarak da burada kullanabilirdik.
            // View'da kategoriler için bir Drop Down List (HTML select tag'i) kullanacağımızdan yeni bir SelectList objesi oluştururken içerisine parametre olarak sırasıyla
            // kategori listemizi, listenin tipi (CategoryModel) üzerinden arka planda tutacağımız (yani kullanıcının görmeyeceği) özellik ismini (Id) ve
            // listenin tipi (CategoryModel) üzerinden kullanıcıya göstereceğimiz özellik ismini (Name) belirtiyoruz.

            #region IActionResult'ı implemente eden class'lar ve bu class tiplerini dönen methodlar
            //return new ViewResult(); // ViewResult ActionResult'tan miras aldığı için ve ActionResult da IActionResult'ı implemente ettiği için dönülebilir.
                                       // Ancak bu şekilde ViewResult objesini new'leyerek dönmek yerine aşağıdaki ViewResult dönen View methodu kullanılır.
                                       // Detaylı bilgi için aşağıdaki aynı isme sahip region'a bakılabilir.

            return View(); // Views altındaki Products klasöründeki Create.cshtml view'ını döner.
            #endregion
        }



        [HttpPost] // post methodu ile veri gönderen HTML form'unun veya isteklerin (request) verilerinin sunucu tarafından alınmasını sağlar. post işlemleri için yazmak zorunludur.
        [ValidateAntiForgeryToken] // View'da AntiforgeryToken HTML Helper'ı ile oluşturulan token'ın validasyonunu sağlayan attribute'tur. 
        //public IActionResult Create(string Name, string Description, double UnitPrice, int StockAmount, DateTime? ExpirationDate, int? CategoryId)
        public IActionResult Create(ProductModel product)
        // form verileri name ile belirtilen input HTML elemanları üzerinden parametre olarak alınabildiği gibi bu özellikler ProductModel'in içerisinde olduğundan
        // parametre olarak ProductModel tipinde bir product parametresi (model) de kullanılabilir. Genelde model kullanımı tercih edilir.
        {
            if (ModelState.IsValid) // eğer kullanıcıdan parametre olarak gelen product model verilerinde data annotation'lar üzerinden bir validasyon hatası yoksa
            {
                Result result = _productService.Add(product); // product model'i eklenmesi için servisin ekleme (Add) methoduna gönderiyoruz ve Result tipindeki değişkene sonucu atıyoruz
                if (result.IsSuccessful) // eğer sonuç başarılıysa (servisten SuccessResult tipinde obje dönülmüş demektir)
                {
                    TempData["Message"] = result.Message; // başarılı işlem sonucunun mesajını başka bir action'a yönlendirdiğimiz için ViewBag
                                                          // veya ViewData ile taşıyamayacağımızdan TempData üzerinden taşıyoruz

                    //return RedirectToAction("Index"); // son olarak bu controller'ın Index action'ına yönlendiriyoruz ki veriler o action'da
                                                        // veritabanından tekrar çekilip Index view'ında listelenebilsin
                    return RedirectToAction(nameof(Index)); // string olarak Index yazmak yerine Index methodunun adını nameof ile alıp kullanmak hata yapmamızı ortadan kaldırır
                }

                //ViewBag.Message = result.Message; // bu satırda servisten ErrorResult objesi dönmüş demektir, dolayısıyla sonucun mesajını Create view'ına bu şekilde taşıyabiliriz
                ModelState.AddModelError("", result.Message); // view'da validation summary kullandığımız için hata sonucunun mesajının bu şekilde validation summary'de
                                                              // gösterimini sağlayabiliriz
            }
            ViewBag.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId); 
            // bu satırda model validasyondan geçememiş demektir
            // Create view'ını tekrar döneceğimiz için view'da select HTML tag'inde (drop down list) kullandığımız kategori listesini tekrar doldurmak zorundayız,
            // new SelectList'teki son parametre kategori listesinde kullanıcının product model üzerinden seçmiş olduğu kategorinin CategoryId üzerinden seçilmesini sağlar

            return View(product); // bu action'a parametre olarak gelen ve kullanıcının view üzerinden doldurduğu product modelini tekrar kullanıcıya gönderiyoruz ki
                                  // kullanıcı view'da girdiği verileri kaybetmesin ve hataları giderip tekrar işlem yapabilsin
        }



        public IActionResult Edit(int id) // örneğin tarayıcıda ~/Products/Edit/1 adresi girilerek veya herhangi bir view'da (örneğin Views -> Products -> Index.cshtml)
                                          // bu adres için id parametresini de gönderen bir link oluşturularak çağrılabilir.
        {
            var product = _productService.Query().SingleOrDefault(p => p.Id == id); // önce action'a gelen id parametresine göre ürün verisini çekiyoruz

            if (product is null) // eğer gelen id'ye göre ürün bulunamadıysa
            {
                return View("_Error", "Product not found!"); // ürün bulunamadı mesajını daha önce oluşturduğumuz _Error.cshtml view'ına gönderiyoruz
            }

            ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
            // view'da select HTML tag'inde (drop down list) kullandığımız kategori listesini SelectList objesine doldurarak ViewBag'e atıyoruz,
            // new SelectList'teki son parametre kategori listesinde kullanıcının product model üzerinden daha önce kaydetmiş olduğu kategorinin
            // CategoryId üzerinden seçilmesini sağlar

            return View(product); // Veritabanından çektiğimiz ürünü Edit.cshtml view'ına gönderiyoruz.
                                  // Edit.cshtml view'ını scaffolding (controller ve istenirse view'larının kodlarının şablonlara göre otomatik oluşturulması)
                                  // ile bu action'ın herhangi bir yerinde fare ile sağ tıklanarak Add View -> Add Razor View ->
                                  // View name: Edit (action adı Edit olduğu için), Template: Edit (diğer aksiyonlar için List, Details, Create, Delete veya Empty kullanılabilir),
                                  // Model class: Product (mutlaka entity seçilmeli) -> Data context class: ETradeContext,
                                  // Options olarak da Create as a partial view (Edit view'ı bir partial view olmamalı) ve Reference script libraries
                                  // (seçilirse client side yani tarayıcı üzerinden Javascript ile validation aktif olur, seçilmezse server side yani
                                  // sunucudaki controller action'ları üzerinden validation aktif olur) seçmeden ve
                                  // Use a layout page'i işaretleyip boş bırakarak (Views -> _ViewStart.cshtml altında tanımlanan projenin _Layout.cshtml view'ını kullan,
                                  // istenirse sağdaki üç noktaya tıklanarak başka bir layout view da seçilebilir) oluşturuyoruz.
                                  // Burada yaptığımız gibi Entity Framework üzerinden scaffolding yapabilmek için DataAccess -> Contexts altına
                                  // projenin DbContext class'ı (ETradeContext) için bir factory (ETradeContextFactory, fabrika: örneğin scaffolding işlemi için
                                  // ETradeContext objesini oluşturup kullanılmasını sağlayacak) class'ı oluşturulmalıdır.
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductModel product)
        {
			if (ModelState.IsValid) // eğer kullanıcıdan parametre olarak gelen product model verilerinde data annotation'lar üzerinden bir validasyon hatası yoksa
			{
				Result result = _productService.Update(product); // product model'i güncellenmesi için servisin güncelleme (Update) methoduna gönderiyoruz ve Result tipindeki değişkene sonucu atıyoruz
				if (result.IsSuccessful) // eğer sonuç başarılıysa (servisten SuccessResult tipinde obje dönülmüş demektir)
				{
					TempData["Message"] = result.Message; // başarılı işlem sonucunun mesajını başka bir action'a yönlendirdiğimiz için ViewBag
														  // veya ViewData ile taşıyamayacağımızdan TempData üzerinden taşıyoruz

					return RedirectToAction(nameof(Index)); // son olarak bu controller'ın Index action'ına yönlendiriyoruz ki veriler o action'da
					                                        // veritabanından tekrar çekilip Index view'ında listelenebilsin
				}

				ModelState.AddModelError("", result.Message); // view'da validation summary kullandığımız için hata sonucunun mesajının bu şekilde validation summary'de
															  // gösterimini sağlayabiliriz
			}
			ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId); 
            // bu satırda model validasyondan geçememiş demektir
			// Edit view'ını tekrar döneceğimiz için view'da select HTML tag'inde (drop down list) kullandığımız kategori listesini tekrar doldurmak zorundayız,
			// new SelectList'teki son parametre kategori listesinde kullanıcının product model üzerinden seçmiş olduğu kategorinin CategoryId üzerinden seçilmesini sağlar

			return View(product); // bu action'a parametre olarak gelen ve kullanıcının view üzerinden doldurduğu product modelini tekrar kullanıcıya gönderiyoruz ki
								  // kullanıcı view'da girdiği verileri kaybetmesin ve hataları giderip tekrar işlem yapabilsin
		}



        public IActionResult Delete(int id) // örneğin tarayıcıda ~/Products/Delete/1 adresi girilerek veya herhangi bir view'da (örneğin Views -> Products -> Index.cshtml)
                                            // bu adres için id parametresini de gönderen bir link oluşturularak çağrılabilir.
        {
            var product = _productService.Query().SingleOrDefault(p => p.Id == id); // önce action'a gelen id parametresine göre ürün verisini çekiyoruz

            if (product is null) // eğer gelen id'ye göre ürün bulunamadıysa
            {
                return View("_Error", "Product not found!"); // ürün bulunamadı mesajını daha önce oluşturduğumuz _Error.cshtml view'ına gönderiyoruz
            }

            return View(product); // Veritabanından çektiğimiz ürünü Delete.cshtml view'ına gönderiyoruz ki kullanıcı ürünü görüp silme işlemini onaylayabilsin.
                                  // Delete view'ını da scaffolding ile template'ı Delete ve model class'ı da Product entity'si seçerek oluşturduk.
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")] // Yukarıda id parametresi alan Delete methodu olduğundan aynı parametreyi alan başka bir Delete adında method oluşturamayız.
                               // Bu yüzden action'ın adını DeleteConfirmed olarak değiştirdik. Ancak Delete view'ı üzerinden form ile id verisini
                               // bu action'a Delete route'u üzerinden taşıyabilmek için ActionName selector'ı ile action çağrımını Delete olarak değiştirdik.
                               // Eğer ActionName kullanmasaydık bu action'ı DeleteConfirmed olarak çağırmamız gerekecekti.
        public IActionResult DeleteConfirmed(int id)
        {
            Result result = _productService.Delete(id); // view'dan parametre olarak gelen id üzerinden ürün kaydını siliyoruz.
            TempData["Message"] = result.Message; // Index view'ında silme sonucunu gösterebilmek için dönen sonuç mesajını TempData'ya atıyoruz.
            return RedirectToAction(nameof(Index)); // Index action'ına yönlendirme yaparak verilerin tekrar veritabanından çekilip listelenmesini sağlıyoruz.
        }



		#region IActionResult'ı implemente eden class'lar ve bu class tiplerini dönen methodlar
		/*
        IActionResult
        |
        ActionResult
        |
        ViewResult (View()) - ContentResult (Content()) - EmptyResult - FileContentResult (File()) - HttpResults - JavaScriptResult (JavaScript()) - JsonResult (Json()) - RedirectResults
        */
		public ContentResult GetHtmlContent() // tarayıcıda çağrılması: ~/Products/GetHtmlContent
        {
            //return new ContentResult(); // ContentResult objesini new'leyerek dönmek yerine aşağıdaki methodu kullanılmalıdır.
            return Content("<b><i>Content result.</i></b>", "text/html"); // içerik tipi text/html belirtilmelidir ki tarayıcı HTML olarak yorumlayabilsin,
                                                                          // Türkçe karakterlerde problem olursa son parametre olarak Encoding.UTF8 de kullanılabilir.
        }

        public ActionResult GetProductsXmlContent() // tarayıcıda çağrılması: ~/Products/GetProductsXmlContent, XML döndürme işlemleri genelde bu şekilde yapılmaz, web servisler üzerinden döndürülür.
        {
            List<ProductModel> products = _productService.Query().ToList();
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            xml += "<Products>";
            foreach (var product in products)
            {
                xml += "<Product>";
                xml += "<Id>" + product.Id + "</Id>";
                xml += "<Name>" + product.Name + "</Name>";
                xml += "<Description>" + product.Description + "</Description>";
                xml += "<UnitPrice>" + product.UnitPriceDisplay + "</UnitPrice>";
                xml += "<StockAmount>" + product.StockAmount + "</StockAmount>";
                xml += "<ExpirationDate>" + product.ExpirationDateDisplay + "</ExpirationDate>";
                xml += "<Category>" + product.CategoryNameDisplay + "</Category>";
                xml += "</Product>";
            }
            xml += "</Products>";
            return Content(xml, "application/xml"); // XML verileri için içerik tipi application/xml belirtilmelidir ki tarayıcı XML olarak yorumlayabilsin.
        }

        public string GetString() // tarayıcıda çağrılması: ~/Products/GetString
        {
            return "String."; // sayfaya "String." düz yazısını (plain text) döner 
        }

        public EmptyResult GetEmpty() // tarayıcıda çağrılması: ~/Products/GetEmpty
        {
            return new EmptyResult(); // içerisinde hiç bir veri olmayan boş bir sayfa döner
        }

        public RedirectResult RedirectToMicrosoft() // tarayıcıda çağrılması: ~/Products/RedirectToMicrosoft
        {
            //return new RedirectResult(); // RedirectResult objesini new'leyerek dönmek yerine aşağıdaki methodu kullanılmalıdır.
            return Redirect("https://microsoft.com"); // parametre olarak belirtilen adrese yönlendirir
        }
        #endregion
    }
}
