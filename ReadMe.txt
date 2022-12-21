MvcWebUI (ASP.NET Core Web App Model View Controller), Business (Class Library), DataAccess (Class Library) ve AppCore (Class Library) projeleri oluþturulduktan sonra 
solution build edilir ve DataAccess projesine AppCore, Business projesine AppCore ve DataAccess, MvcWebUI projesine ise Business, DataAccess ve AppCore
projeleri referans olarak eklenir:

1) Ýster solution altýnda AppCore adýnda yeni bir proje oluþturulur, istenirse de AppCore projesi dýþarýdan solution'a eklenebilir.

2) DataAccess katmanýnda AppCore katmanýndaki RecordBase'den miras alan Entity'ler oluþturulur.

3) AppCore katmanýna Microsoft.EntityFrameworkCore.SqlServer ile DataAccess katmanýna Microsoft.EntityFrameworkCore.Tools paketleri NuGet'ten indirilir.
.NET versiyonu hangisi ise NuGet'ten o versiyon numarasý ile baþlayan paketlerin son versiyonlarý indirilmelidir.

4) DataAccess katmanýnda DbContext'ten türeyen Context ve içerisindeki DbSet'ler ile connection string bilgisini de içeren DbContextOptions tipindeki objenin 
Context'e constructor üzerinden enjekte edilmesini saðlayacak parametreli constructor oluþturulur, daha sonra MvcWebUI katmanýnda Program.cs'teki
IoC Container'da DbContext için baðýmlýlýk yönetimi gerçekleþtirilir.

5) MvcWebUI katmanýndaki appsettings.json ile eðer istenirse appsettings.Development.json içerisine connection string 
server=.\\SQLEXPRESS;database=BA_ETradeCore7;user id=sa;password=sa;multipleactiveresultsets=true;trustservercertificate=true; 
formatta yazýlýr. appsettings.json dosyasý Properties klasöründeki launchSettings.json dosyasýnda konfigüre edilen production (canlý) profilinde, 
appsettings.Development.json dosyasý ise development (geliþtirme) profilinde proje çalýþtýrýldýðýnda kullanýlacaktýr.
Ayrýca launchSettings.json dosyasýna view'larda deðiþiklik yapýldýðýnda projenin tekrar baþlatýlma gerekliliðini ortadan kaldýrmak için
"ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation" satýrýnýn eklenmesi
ve NuGet üzerinden Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation kütüphanesinin MvcWebUI projesine indirilmesi faydalý olacaktýr.

6) MvcWebUI katmanýna Microsoft.EntityFrameworkCore.Design paketi NuGet'ten indirilerek MvcWebUI projesi Startup Project yapýlýr. 
Tools -> NuGet Package Manager -> Package Manager Console açýlýr, Default project DataAccess seçilir 
ve önce örneðin add-migration v1 daha sonra update-database komutlarý çalýþtýrýlýr.
Entity'ler veya DbSet'ler üzerinde yapýlan her deðiþiklik için örneðin add-migration v2 daha sonra da update-database çalýþtýrýlmalýdýr.

7) DataAccess katmanýnda entity'ler üzerinden AppCore'daki RepoBase'den miras alan abstract (soyut) base repository'ler ile
bu base repository'lerden miras alan concrete (somut) repository'ler oluþturulur ve MvcWebUI katmanýnda Program.cs'teki IoC Container'da baðýmlýlýklarý yönetilir.

8) Business katmanýnda entity'ler üzerinden model class'larý AppCore katmanýndaki RecordBase'den miras alacak þekilde oluþturulur,
eðer istenirse MvcWebUI katmanýnda view'larda kullanýlmak üzere formatlama, iliþkili referans özellikleri kullanma, vb. için yeni özellikler eklenebilir.

9) Business katmanýnda model'ler üzerinden entity <-> model dönüþümlerini gerçekleþtirip DataAccess katmanýndaki repository'ler üzerinden
veritabaný iþlemleri gerçekleþtirmek için AppCore'daki IService'i implemente eden interface'ler ile bu interface'leri implemente
eden concrete (somut) service'ler oluþturulur ve MvcWebUI katmanýnda Program.cs'teki IoC Container'da baðýmlýlýklarý yönetilir.

10) MvcWebUI katmanýnda yönetilecek model için controller ile ilgili action ve view'larý oluþturularak ilgili service constructor üzerinden enjekte edilir
ve controller action'larýnda methodlarý kullanýlarak model objeleri üzerinden iþlemler (örneðin CRUD) gerçekleþtirilir.

View <-> Controller (Action) <-> Service (Model) <-> Repository (Entity) <-> DbContext (Entity) <-> Database

11) MvcWebUI katmanýndaki Views -> Shared klasörü altýndaki projede tüm oluþturulan view'larýn bir þablon içerisinde gösterilmesini
saðlayan _Layout.cshtml view'ý içerisinde controller ve action'lar üzerinden, örneðin menüye link'ler eklenir.

12) Eðer istenirse veritabanýndaki tüm verilerin sýfýrdan oluþturulmasýný saðlayan, örneðin MvcWebUI katmanýnda Areas klasöründeki Database area'sý içerisinde,
bir controller ve aksiyonu yazýlabilir.

Konu Anlatýmlý Proje Geliþtirme Aþamalarý:
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
34) MvcWebUI -> Controllers -> Stores -> MVC Controller Entity Framework Scaffolding

Kullanýcý Yönetimi: Ýstenirse kullanýcý yönetimi için Microsoft'un Identity Framework kütüphanesi kullanýlabilir.
35) DataAccess -> Entities -> User ve Role entity'leri
36) DataAccess -> Contexts -> ETradeContext -> Users ve Roles DbSet'leri
37) DataAccess -> Repositories -> UserRepo (MvcWebUI -> Program.cs -> IoC Container)
38) DataAccess -> Repositories -> RoleRepo (MvcWebUI -> Program.cs -> IoC Container)
39) DataAccess -> Enums -> Roles
40) Business -> Models -> UserModel
41) Business -> Models -> AccountLoginModel
42) Business -> Services -> UserService -> Query (MvcWebUI -> Program.cs -> IoC Container)
43) Business -> Services -> AccountService -> Login (MvcWebUI -> Program.cs -> IoC Container)
44) Business -> Models -> AccountRegisterModel
45) Business -> Services -> UserService -> Add
46) Business -> Services -> AccountService -> Register
47) MvcWebUI -> Areas -> Accounts -> Controllers -> Home -> Register Action'larý ve View'ý
48) MvcWebUI -> Areas -> Accounts -> Controllers -> Home -> Login Action'larý ve View'ý (MvcWebUI -> Program.cs -> Authentication)