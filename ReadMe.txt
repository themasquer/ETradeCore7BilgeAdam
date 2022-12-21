MvcWebUI (ASP.NET Core Web App Model View Controller), Business (Class Library), DataAccess (Class Library) ve AppCore (Class Library) projeleri olu�turulduktan sonra 
solution build edilir ve DataAccess projesine AppCore, Business projesine AppCore ve DataAccess, MvcWebUI projesine ise Business, DataAccess ve AppCore
projeleri referans olarak eklenir:

1) �ster solution alt�nda AppCore ad�nda yeni bir proje olu�turulur, istenirse de AppCore projesi d��ar�dan solution'a eklenebilir.

2) DataAccess katman�nda AppCore katman�ndaki RecordBase'den miras alan Entity'ler olu�turulur.

3) AppCore katman�na Microsoft.EntityFrameworkCore.SqlServer ile DataAccess katman�na Microsoft.EntityFrameworkCore.Tools paketleri NuGet'ten indirilir.
.NET versiyonu hangisi ise NuGet'ten o versiyon numaras� ile ba�layan paketlerin son versiyonlar� indirilmelidir.

4) DataAccess katman�nda DbContext'ten t�reyen Context ve i�erisindeki DbSet'ler ile connection string bilgisini de i�eren DbContextOptions tipindeki objenin 
Context'e constructor �zerinden enjekte edilmesini sa�layacak parametreli constructor olu�turulur, daha sonra MvcWebUI katman�nda Program.cs'teki
IoC Container'da DbContext i�in ba��ml�l�k y�netimi ger�ekle�tirilir.

5) MvcWebUI katman�ndaki appsettings.json ile e�er istenirse appsettings.Development.json i�erisine connection string 
server=.\\SQLEXPRESS;database=BA_ETradeCore7;user id=sa;password=sa;multipleactiveresultsets=true;trustservercertificate=true; 
formatta yaz�l�r. appsettings.json dosyas� Properties klas�r�ndeki launchSettings.json dosyas�nda konfig�re edilen production (canl�) profilinde, 
appsettings.Development.json dosyas� ise development (geli�tirme) profilinde proje �al��t�r�ld���nda kullan�lacakt�r.
Ayr�ca launchSettings.json dosyas�na view'larda de�i�iklik yap�ld���nda projenin tekrar ba�lat�lma gereklili�ini ortadan kald�rmak i�in
"ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation" sat�r�n�n eklenmesi
ve NuGet �zerinden Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation k�t�phanesinin MvcWebUI projesine indirilmesi faydal� olacakt�r.

6) MvcWebUI katman�na Microsoft.EntityFrameworkCore.Design paketi NuGet'ten indirilerek MvcWebUI projesi Startup Project yap�l�r. 
Tools -> NuGet Package Manager -> Package Manager Console a��l�r, Default project DataAccess se�ilir 
ve �nce �rne�in add-migration v1 daha sonra update-database komutlar� �al��t�r�l�r.
Entity'ler veya DbSet'ler �zerinde yap�lan her de�i�iklik i�in �rne�in add-migration v2 daha sonra da update-database �al��t�r�lmal�d�r.

7) DataAccess katman�nda entity'ler �zerinden AppCore'daki RepoBase'den miras alan abstract (soyut) base repository'ler ile
bu base repository'lerden miras alan concrete (somut) repository'ler olu�turulur ve MvcWebUI katman�nda Program.cs'teki IoC Container'da ba��ml�l�klar� y�netilir.

8) Business katman�nda entity'ler �zerinden model class'lar� AppCore katman�ndaki RecordBase'den miras alacak �ekilde olu�turulur,
e�er istenirse MvcWebUI katman�nda view'larda kullan�lmak �zere formatlama, ili�kili referans �zellikleri kullanma, vb. i�in yeni �zellikler eklenebilir.

9) Business katman�nda model'ler �zerinden entity <-> model d�n���mlerini ger�ekle�tirip DataAccess katman�ndaki repository'ler �zerinden
veritaban� i�lemleri ger�ekle�tirmek i�in AppCore'daki IService'i implemente eden interface'ler ile bu interface'leri implemente
eden concrete (somut) service'ler olu�turulur ve MvcWebUI katman�nda Program.cs'teki IoC Container'da ba��ml�l�klar� y�netilir.

10) MvcWebUI katman�nda y�netilecek model i�in controller ile ilgili action ve view'lar� olu�turularak ilgili service constructor �zerinden enjekte edilir
ve controller action'lar�nda methodlar� kullan�larak model objeleri �zerinden i�lemler (�rne�in CRUD) ger�ekle�tirilir.

View <-> Controller (Action) <-> Service (Model) <-> Repository (Entity) <-> DbContext (Entity) <-> Database

11) MvcWebUI katman�ndaki Views -> Shared klas�r� alt�ndaki projede t�m olu�turulan view'lar�n bir �ablon i�erisinde g�sterilmesini
sa�layan _Layout.cshtml view'� i�erisinde controller ve action'lar �zerinden, �rne�in men�ye link'ler eklenir.

12) E�er istenirse veritaban�ndaki t�m verilerin s�f�rdan olu�turulmas�n� sa�layan, �rne�in MvcWebUI katman�nda Areas klas�r�ndeki Database area's� i�erisinde,
bir controller ve aksiyonu yaz�labilir.

Konu Anlat�ml� Proje Geli�tirme A�amalar�:
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

Kullan�c� Y�netimi: �stenirse kullan�c� y�netimi i�in Microsoft'un Identity Framework k�t�phanesi kullan�labilir.
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
47) MvcWebUI -> Areas -> Accounts -> Controllers -> Home -> Register Action'lar� ve View'�
48) MvcWebUI -> Areas -> Accounts -> Controllers -> Home -> Login Action'lar� ve View'� (MvcWebUI -> Program.cs -> Authentication)