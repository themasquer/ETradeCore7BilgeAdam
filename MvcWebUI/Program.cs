#nullable disable

using Business.Services;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region Localization
// Web uygulamasının bölgesel ayarı aşağıdaki şekilde tek seferde konfigüre edilerek tüm projenin bu ayarı kullanması sağlanabilir,
// dolayısıyla veri formatlama veya dönüştürme gibi işlemlerde her seferinde CultureInfo objesinin kullanım gereksinimi ortadan kalkar.
// Bu şekilde sadece tek bir bölgesel ayar projede kullanılabilir.
List<CultureInfo> cultures = new List<CultureInfo>()
{
    new CultureInfo("en-US") // eğer uygulama Türkçe olacaksa CultureInfo constructor'ının parametresini ("tr-TR") yapmak yeterlidir.
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name);
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();

#region IoC Container : Inversion of Control Container (Bağımlılıkların Yönetimi) 
// Alternatif olarak Business katmanında Autofac ve Ninject gibi kütüphaneler de kullanılabilir.

// Unable to resolve service hataları burada çözümlenmelidir.

// AddScoped: istek (request) boyunca objenin referansını (genelde interface veya abstract class) kullandığımız yerde obje (somut class'tan oluşturulacak)
// bir kere oluşturulur ve yanıt (response) dönene kadar bu obje hayatta kalır.
// AddSingleton: web uygulaması başladığında objenin referansnı (genelde interface veya abstract class) kullandığımız yerde obje (somut class'tan oluşturulacak)
// bir kere oluşturulur ve uygulama çalıştığı (IIS üzerinden uygulama durdurulmadığı veya yeniden başlatılmadığı) sürece bu obje hayatta kalır.
// AddTransient: istek (request) bağımsız ihtiyaç olan objenin referansını (genelde interface veya abstract class) kullandığımız her yerde bu objeyi new'ler.
// Genelde AddScoped methodu kullanılır.

string connectionString = builder.Configuration.GetConnectionString("ETradeDb"); // appsettings.json veya appsettings.Development.json dosyalarındaki isim üzerinden atanan
                                                                                 // veritabanı bağlantı string'ini döner.

builder.Services.AddDbContext<ETradeContext>(options => options.UseSqlServer(connectionString)); // projede herhangi bir class'ta ETradeContext tipinde 
                                                                                                 // constructor injection yapıldığında ETradeContext objesini new'leyerek
                                                                                                 // o class'a enjekte eder.       

builder.Services.AddScoped<ProductRepoBase, ProductRepo>(); // projede herhangi bir class'ta ProductRepoBase tipinde constructor injection yapıldığında
                                                            // ProductRepo objesini new'leyerek o class'a enjekte eder.

builder.Services.AddScoped<IProductService, ProductService>(); // projede herhangi bir class'ta IProductService tipinde constructor injection yapıldığında
                                                               // ProductService objesini new'leyerek o class'a enjekte eder.
#endregion

var app = builder.Build();

#region Localization
app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name),
    SupportedCultures = cultures,
    SupportedUICultures = cultures,
});
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

#region Area
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
#endregion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // MVC default route tanımı

app.Run();
