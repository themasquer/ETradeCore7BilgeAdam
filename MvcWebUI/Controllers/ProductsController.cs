using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService; // controller'da ürünle ilgili işleri gerçekleştirebilmek için servis alanı tanımlanır ve constructor üzerinden enjekte edilir.

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }



        public IActionResult Index() // tarayıcıda ~/Products/Index adresi girilerek veya herhangi bir view'da (örneğin Views -> Shared klasörü altındaki _Layout.cshtml)
                                     // bu adres için link oluşturularak çağrılabilir.
        {
            var products = _productService.Query().ToList(); // ToList LINQ (Language Integrated Query) methodu sorgunun çalıştırılmasını ve sonucunun IQueryable'da kullanılan tip
                                                             // üzerinden bir liste olarak dönmesini sağlar.

            //return View("ProductList", products); // eğer istenirse dönen liste Views altındaki Products klasörüne ProductList.cshtml adlı view oluşturularak bu view'a model olarak gönderilebilir.
            return View(products); // veya dönen liste Views altındaki Products klasöründeki Index.cshtml view'ına model olarak gönderilir.
        }



        [HttpGet] // Bu aksiyonun HTTP GET yani sunucudan kaynak getirme işlemini yapacağını belirten Action Method Selector'ıdır.
                  // Yazmak zorunlu değildir çünkü yazılmazsa default HttpGet methodu action'larda kullanılır.
        public IActionResult Create() // tarayıcıda ~/Products/Create adresi girilerek veya herhangi bir view'da (örneğin Views -> Producs klasörü altındaki Index.cshtml)
                                      // bu adres için link oluşturularak çağrılabilir. Create view'ındaki form kullanıcıya dönülür ki kullanıcı veri girip sunucuya gönderebilsin.
                                      // Veritabanında yeni kayıt oluşturmak için kullanılır.
        {
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
            return View();
        }



        #region IActionResult'ı implemente eden class'lar ve bu class tiplerini dönen methodlar
        /*
        IActionResult
        |
        ActionResult
        |
        ViewResult (View()) - ContentResult (Content()) - EmptyResult - FileContentResult (File()) - HttpResults - JavaScriptResult (JavaScript()) - JsonResult (Json()) - RedirectResults
        */
        public ContentResult GetHtmlContent()
        {
            //return new ContentResult(); // ContentResult objesini new'leyerek dönmek yerine aşağıdaki methodu kullanılmalıdır.
            return Content("<b><i>Content result.</i></b>", "text/html"); // Türkçe karakterlerde problem olursa son parametre olarak Encoding.UTF8 de kullanılabilir.
        }

        public ActionResult GetProductsXmlContent() // XML döndürme işlemleri genelde bu şekilde yapılmaz, web servisler üzerinden döndürülür.
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
            return Content(xml, "application/xml"); // XML verileri için içerik tipi application/xml belirtilmelidir ki tarayıcı yorumlayabilsin.
        }

        public string GetString()
        {
            return "String.";
        }

        public EmptyResult GetEmpty()
        {
            return new EmptyResult();
        }

        public RedirectResult RedirectToMicrosoft()
        {
            return Redirect("https://microsoft.com");
        }
        #endregion
    }
}
