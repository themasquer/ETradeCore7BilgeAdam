MvcWebUI (ASP.NET Core Web App Model View Controller), Business (Class Library), DataAccess (Class Library) ve AppCore (Class Library) projeleri oluþturulduktan sonra 
solution build edilir ve DataAccess projesine AppCore, Business projesine AppCore ve DataAccess, MvcWebUI projesine ise Business, DataAccess ve AppCore
projeleri referans olarak eklenir:

1) Ýster solution altýnda AppCore adýnda yeni bir proje oluþturulur, istenirse de AppCore projesi dýþarýdan solution'a eklenebilir.
2) DataAccess katmanýnda Entity'ler oluþturulur.
3) AppCore katmanýna Microsoft.EntityFrameworkCore.SqlServer ile DataAccess katmanýna Microsoft.EntityFrameworkCore.Tools paketleri NuGet'ten indirilir.
.NET versiyonu hangisi ise NuGet'ten o versiyon numarasý ile baþlayan paketler indirilmelidir.
4) DataAccess katmanýnda DbContext'ten türeyen Context ve içerisindeki DbSet'ler oluþturulur.
5) MvcWebUI katmanýndaki appsettings.json içerisine connection string 
server=.\\SQLEXPRESS;database=BA_ETradeCore7;user id=sa;password=sa;multipleactiveresultsets=true;trustservercertificate=true; 
formatta yazýlýr.
6) MvcWebUI katmanýnda Program.cs içerisine builder.Services.AddControllersWithViews(); satýrýnýn altýna
IoC Container region'ý eklenir.
MvcWebUI katmanýna Microsoft.EntityFrameworkCore.Design paketi NuGet'ten indirilerek MvcWebUI projesi Startup Project yapýlýr. 
Tools -> NuGet Package Manager -> Package Manager Console açýlýr, Default project DataAccess seçilir 
ve önce add-migration v1 daha sonra update-database komutlarý çalýþtýrýlýr.
7) DataAccess katmanýnda entity'ler üzerinden RepoBase'den miras alan abstract (soyut) base repository'ler ile
bu base repository'lerden miras alan concrete (somut) repository'ler oluþturulur.