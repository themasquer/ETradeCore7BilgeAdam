#nullable disable
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MvcWebUI.Areas.Account.Controllers
{
    [Area("Account")]
    public class UsersController : Controller
    {
        // Add service injections here
        private readonly IAccountService _accountService;

        public UsersController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: Account/Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(AccountRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Register(model);
                if (result.IsSuccessful)
                    return RedirectToAction("Index", "Home", new { area = "" }); // projenin Home controller'ının Index action'ına dönebilmek için
                                                                                 // anonim bir objede area özelliğini "" atayarak yönlendirme yapmalıyız
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        // GET: Account/Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginModel model)
        {
            if (ModelState.IsValid) 
            {
                UserModel userResult = new UserModel(); // UserModel tipindeki userResult'ı burada tanımlayıp new'liyoruz ki Login methodu  
                                                        // başarılı olursa userResult referans tip olduğu için Login methodu içerisinde atansın 
                                                        // ve bu methodda kullanabilelim

                var result = _accountService.Login(model, userResult); // modeldeki kullanıcı adı ve şifreyi aktiflik durumuyla birlikte kontrol ediyoruz
                if (result.IsSuccessful) // eğer modeldeki kullanıcı adı ve şifreye sahip aktif kullanıcı varsa
                {
                    // Login methodunda doldurulan userResult objesinin istediğimiz özelliklerindeki verileri bir claim (talep) listesinde dolduruyoruz ki
                    // bu liste üzerinden şifreli bir şekilde bir cookie (çerez) oluşup client'a geri dönülsün ve kullanıcı bilgilerini içeren
                    // bu cookie üzerinden web uygulamamızda authorization (yetki kontrülü) yapabilelim,
                    // claim'lerde asla şifre gibi kritik veriler saklanmamalıdır
                    List<Claim> claims = new List<Claim>()
                    {
                        //new Claim("Name", userResult.UserName), // Claim Dictionary veri tipine benzer bir tipi ve o tipe karşılık değeri olan bir yapıdır,
                                                                  // constructor'ının ilk parametresi olan tipi elle yazmak yerine ClaimTypes üzerinden
                                                                  // kullanmak daha uygundur, ikinci parametre ise bu tipe atanmak istenen değerdir
                        new Claim(ClaimTypes.Name, userResult.UserName),

                        new Claim(ClaimTypes.Role, userResult.RoleNameDisplay)
                    };

                    // oluşturduğumuz claim listesi üzerinden Cookie Authentication default'larını kullanarak bir identity (kimlik) oluşturuyoruz
                    var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                    // oluşturduğumuz kimlik üzerinden de MVC'de authentication (kimlik doğrulama) için kullanacağımız bir principal oluşturuyoruz 
                    var principal = new ClaimsPrincipal(identity);

                    // son olarak oluşturduğumuz principal üzerinden MVC'de kimlik giriş işlemini tamamlıyoruz,
                    // SignInAsync methodu bir asenkron method olduğu için başına await (asynchronous wait) yazmalıyız
                    // ve methodun dönüş tipinin başına async yazarak dönüş tipini de bir Task tipi içerisinde tip olarak (IActionResult) tanımlamalıyız
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // giriş işlemi başarılı olduğu için kullanıcıyı ana hoşgeldin sayfasına yönlendiriyoruz,
                    // bu sayfa bir area'nın içerisinde olmadığı için de area özelliğini içeren anonim tipteki objeyi route value parametresi olarak oluşturuyoruz
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }
    }
}
