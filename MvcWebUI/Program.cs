#nullable disable

using Business.Services;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region Localization
// Web uygulamas�n�n b�lgesel ayar� a�a��daki �ekilde tek seferde konfig�re edilerek t�m projenin bu ayar� kullanmas� sa�lanabilir,
// dolay�s�yla veri formatlama veya d�n��t�rme gibi i�lemlerde her seferinde CultureInfo objesinin kullan�m gereksinimi ortadan kalkar.
// Bu �ekilde sadece tek bir b�lgesel ayar projede kullan�labilir.
List<CultureInfo> cultures = new List<CultureInfo>()
{
    new CultureInfo("en-US") // e�er uygulama T�rk�e olacaksa CultureInfo constructor'�n�n parametresini ("tr-TR") yapmak yeterlidir.
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

#region IoC Container : Inversion of Control Container (Ba��ml�l�klar�n Y�netimi) 
// Alternatif olarak Business katman�nda Autofac ve Ninject gibi k�t�phaneler de kullan�labilir.

// Unable to resolve service hatalar� burada ��z�mlenmelidir.

// AddScoped: istek (request) boyunca objenin referans�n� (genelde interface veya abstract class) kulland���m�z yerde obje (somut class'tan olu�turulacak)
// bir kere olu�turulur ve yan�t (response) d�nene kadar bu obje hayatta kal�r.
// AddSingleton: web uygulamas� ba�lad���nda objenin referansn� (genelde interface veya abstract class) kulland���m�z yerde obje (somut class'tan olu�turulacak)
// bir kere olu�turulur ve uygulama �al��t��� (IIS �zerinden uygulama durdurulmad��� veya yeniden ba�lat�lmad���) s�rece bu obje hayatta kal�r.
// AddTransient: istek (request) ba��ms�z ihtiya� olan objenin referans�n� (genelde interface veya abstract class) kulland���m�z her yerde bu objeyi new'ler.
// Genelde AddScoped methodu kullan�l�r.

string connectionString = builder.Configuration.GetConnectionString("ETradeDb"); // appsettings.json veya appsettings.Development.json dosyalar�ndaki isim �zerinden atanan
                                                                                 // veritaban� ba�lant� string'ini d�ner.

builder.Services.AddDbContext<ETradeContext>(options => options.UseSqlServer(connectionString)); // projede herhangi bir class'ta ETradeContext tipinde 
                                                                                                 // constructor injection yap�ld���nda ETradeContext objesini new'leyerek
                                                                                                 // o class'a enjekte eder.       

builder.Services.AddScoped<ProductRepoBase, ProductRepo>(); // projede herhangi bir class'ta ProductRepoBase tipinde constructor injection yap�ld���nda
                                                            // ProductRepo objesini new'leyerek o class'a enjekte eder.

builder.Services.AddScoped<CategoryRepoBase, CategoryRepo>(); 
builder.Services.AddScoped<StoreRepoBase, StoreRepo>(); 

builder.Services.AddScoped<IProductService, ProductService>(); // projede herhangi bir class'ta IProductService tipinde constructor injection yap�ld���nda
                                                               // ProductService objesini new'leyerek o class'a enjekte eder.

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStoreService, StoreService>();
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
    pattern: "{controller=Home}/{action=Index}/{id?}"); // MVC default route tan�m�

app.Run();
