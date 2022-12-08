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
    }
}
