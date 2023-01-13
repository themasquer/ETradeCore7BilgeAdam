MvcWebUI (ASP.NET Core Web App Model View Controller), Business (Class Library), DataAccess (Class Library) ve AppCore (Class Library) projeleri oluşturulduktan sonra 
solution build edilir ve DataAccess projesine AppCore, Business projesine AppCore ve DataAccess, MvcWebUI projesine ise Business, DataAccess ve AppCore
projeleri referans olarak eklenir:

1) İster solution altında AppCore adında yeni bir proje oluşturulur, istenirse de AppCore projesi dışarıdan solution'a eklenebilir.

2) DataAccess katmanında AppCore katmanındaki RecordBase'den miras alan Entity'ler oluşturulur.

3) AppCore katmanına Microsoft.EntityFrameworkCore.SqlServer ile DataAccess katmanına Microsoft.EntityFrameworkCore.Tools paketleri NuGet'ten indirilir.
.NET versiyonu hangisi ise NuGet'ten o versiyon numarası ile başlayan paketlerin son versiyonları indirilmelidir.

4) DataAccess katmanında DbContext'ten türeyen Context ve içerisindeki DbSet'ler ile connection string bilgisini de içeren DbContextOptions tipindeki objenin 
Context'e constructor üzerinden enjekte edilmesini sağlayacak parametreli constructor oluşturulur, daha sonra MvcWebUI katmanında Program.cs'teki
IoC Container'da DbContext için bağımlılık yönetimi gerçekleştirilir.

5) MvcWebUI katmanındaki appsettings.json ile eğer istenirse appsettings.Development.json içerisine connection string 
server=.\\SQLEXPRESS;database=BA_ETradeCore7;user id=sa;password=sa;multipleactiveresultsets=true;trustservercertificate=true; 
formatta yazılır. appsettings.json dosyası Properties klasöründeki launchSettings.json dosyasında konfigüre edilen production (canlı) profilinde, 
appsettings.Development.json dosyası ise development (geliştirme) profilinde proje çalıştırıldığında kullanılacaktır.
Ayrıca launchSettings.json dosyasına view'larda değişiklik yapıldığında projenin tekrar başlatılma gerekliliğini ortadan kaldırmak için
"ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation" satırının eklenmesi
ve NuGet üzerinden Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation kütüphanesinin MvcWebUI projesine indirilmesi faydalı olacaktır.

6) MvcWebUI katmanına Microsoft.EntityFrameworkCore.Design paketi NuGet'ten indirilerek MvcWebUI projesi Startup Project yapılır. 
Tools -> NuGet Package Manager -> Package Manager Console açılır, Default project DataAccess seçilir 
ve önce örneğin add-migration v1 daha sonra update-database komutları çalıştırılır.
Entity'ler veya DbSet'ler üzerinde yapılan her değişiklik için örneğin add-migration v2 daha sonra da update-database çalıştırılmalıdır.

7) DataAccess katmanında entity'ler üzerinden AppCore'daki RepoBase'den miras alan abstract (soyut) base repository'ler ile
bu base repository'lerden miras alan concrete (somut) repository'ler oluşturulur ve MvcWebUI katmanında Program.cs'teki IoC Container'da bağımlılıkları yönetilir.

8) Business katmanında entity'ler üzerinden model class'ları AppCore katmanındaki RecordBase'den miras alacak şekilde oluşturulur,
eğer istenirse MvcWebUI katmanında view'larda kullanılmak üzere formatlama, ilişkili referans özellikleri kullanma, vb. için yeni özellikler eklenebilir.

9) Business katmanında model'ler üzerinden entity <-> model dönüşümlerini gerçekleştirip DataAccess katmanındaki repository'ler üzerinden
veritabanı işlemleri gerçekleştirmek için AppCore'daki IService'i implemente eden interface'ler ile bu interface'leri implemente
eden concrete (somut) service'ler oluşturulur ve MvcWebUI katmanında Program.cs'teki IoC Container'da bağımlılıkları yönetilir.

10) MvcWebUI katmanında yönetilecek model için controller ile ilgili action ve view'ları oluşturularak ilgili service constructor üzerinden enjekte edilir
ve controller action'larında methodları kullanılarak model objeleri üzerinden işlemler (örneğin CRUD) gerçekleştirilir.

View <-> Controller (Action) <-> Service (Model) <-> Repository (Entity) <-> DbContext (Entity) <-> Database

11) MvcWebUI katmanındaki Views -> Shared klasörü altındaki projede tüm oluşturulan view'ların bir şablon içerisinde gösterilmesini
sağlayan _Layout.cshtml view'ı içerisinde controller ve action'lar üzerinden, örneğin menüye link'ler eklenir.

12) Eğer istenirse veritabanındaki tüm verilerin sıfırdan oluşturulmasını sağlayan, örneğin MvcWebUI katmanında Areas klasöründeki Database area'sı içerisinde,
bir controller ve aksiyonu yazılabilir.

Konu Anlatımlı Proje Geliştirme Aşamaları:
1) DataAccess -> Entities -> Product ve Category entity'leri
2) DataAccess -> Contexts -> ETradeContext -> Products ve Categories DbSet'leri
(MvcWebUI -> Program.cs -> IoC Container ile MvcWebUI -> appsettings.json veya istenirse appsetting.Development.json -> ConnectionStrings)
3) DataAccess -> Repositories -> ProductRepo (MvcWebUI -> Program.cs -> IoC Container)
4) Business -> Models -> ProductModel
5) Business -> Services -> ProductService -> Query (MvcWebUI -> Program.cs -> IoC Container)
6) MvcWebUI -> Controllers -> ProductsController -> Index
7) MvcWebUI -> Views -> Products -> Index.cshtml
8) MvcWebUI -> Controllers -> ProductsController -> Details
9) MvcWebUI -> Views -> Products -> Details.cshtml
10) Business -> Services -> ProductService -> Add
11) MvcWebUI -> Controllers -> ProductsController -> Create
12) MvcWebUI -> Views -> Products -> Create.cshtml
13) DataAccess -> Repositories -> CategoryRepo (MvcWebUI -> Program.cs -> IoC Container)
14) Business -> Models -> CategoryModel
15) Business -> Services -> CategoryService -> Query (MvcWebUI -> Program.cs -> IoC Container)
16) Business -> Services -> ProductService -> Update
17) MvcWebUI -> Controllers -> ProductsController -> Edit
18) MvcWebUI -> Views -> Products -> Edit.cshtml
19) Business -> Services -> ProductService -> Delete
20) MvcWebUI -> Controllers -> ProductsController -> Delete
21) MvcWebUI -> Views -> Products -> Delete.cshtml

22) MvcWebUI -> Controllers -> Categories -> MVC Controller Entity Framework Scaffolding
23) Business -> Services -> CategoryService -> Add
24) Business -> Services -> CategoryService -> Update
25) Business -> Services -> CategoryService -> Delete

26) DataAccess -> Entities -> Store entity
27) DataAccess -> Contexts -> ETradeContext -> Stores DbSet'i
28) DataAccess -> Repositories -> StoreRepo (MvcWebUI -> Program.cs -> IoC Container)
29) Business -> Models -> StoreModel
30) Business -> Services -> StoreService -> Query (MvcWebUI -> Program.cs -> IoC Container)
31) Business -> Services -> StoreService -> Add
32) Business -> Services -> StoreService -> Update
33) Business -> Services -> StoreService -> Delete
34) MvcWebUI -> Controllers -> Stores -> Index, Details, Create, Edit ve Delete action'ları
35) MvcWebUI -> Views -> Stores -> Index, _Details ve _CreateEdit view'ları

Kullanıcı Yönetimi: İstenirse kullanıcı yönetimi için Microsoft'un Identity Framework kütüphanesi kullanılabilir.
36) DataAccess -> Entities -> User, UserDetail, Country, City ve Role entity'leri
37) DataAccess -> Contexts -> ETradeContext -> Users, UserDetails, Countries, Cities ve Roles DbSet'leri
38) DataAccess -> Repositories -> UserRepo (MvcWebUI -> Program.cs -> IoC Container)
39) DataAccess -> Repositories -> RoleRepo (MvcWebUI -> Program.cs -> IoC Container)
40) DataAccess -> Repositories -> CountryRepo (MvcWebUI -> Program.cs -> IoC Container)
41) DataAccess -> Repositories -> CityRepo (MvcWebUI -> Program.cs -> IoC Container)
42) DataAccess -> Enums -> Roles
43) DataAccess -> Enums -> Sex
44) Business -> Models -> UserModel
45) Business -> Models -> UserDetailModel
46) Business -> Models -> CountryModel
47) Business -> Models -> CityModel
48) Business -> Models -> AccountLoginModel
49) Business -> Services -> UserService -> Query (MvcWebUI -> Program.cs -> IoC Container)
50) Business -> Services -> AccountService -> Login (MvcWebUI -> Program.cs -> IoC Container)
51) Business -> Models -> AccountRegisterModel
52) Business -> Services -> UserService -> Add
53) Business -> Services -> AccountService -> Register
54) Business -> Services -> CountryService -> Query (MvcWebUI -> Program.cs -> IoC Container)
55) Business -> Services -> CountryService -> GetList
56) Business -> Services -> CountryService -> GetItem (id parametreli)
57) Business -> Services -> CityService -> Query (MvcWebUI -> Program.cs -> IoC Container)
58) Business -> Services -> CityService -> GetList (countryId parametreli)
59) Business -> Services -> CityService -> GetList (parametresiz)
60) Business -> Services -> CityService -> GetItem (id parametreli)
61) MvcWebUI -> Controllers -> CitiesController -> GetCities (countryId parametreli)
62) MvcWebUI -> Areas -> Accounts -> Controllers -> Home -> Register Action'ları ve View'ı
63) MvcWebUI -> Areas -> Accounts -> Controllers -> Home -> Login Action'ları ve View'ı (MvcWebUI -> Program.cs -> Authentication)
64) MvcWebUI -> Views -> Shared -> _Layout.cshtml -> User üzerinden giriş yapan kullanıcıya göre menü link'lerinin gösterimi
65) MvcWebUI -> Views -> Home -> Welcome.cshtml -> User üzerinden kullanıcı adı gösterimi
66) MvcWebUI -> Controllers -> Categories, Products ve Stores -> Authorize attribute'ları
67) MvcWebUI -> Views -> Categories ve Products -> Index.cshtml ve Details.cshtml -> User üzerinden giriş yapan kullanıcıya göre link'lerin gösterimi

Raporlama:
68) Business -> Models -> Report -> ReportItemModel ve ReportFilterModel
69) Business -> Services -> Report -> ReportService -> GetList (MvcWebUI -> Program.cs -> IoC Container)
70) MvcWebUI -> Areas -> Report -> Models -> HomeIndexViewModel
71) MvcWebUI -> Areas -> Report -> Controllers -> Home -> Index
72) MvcWebUI -> Areas -> Report -> Views -> Home -> Index
