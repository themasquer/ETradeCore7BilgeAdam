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
ve önce add-migration v1 daha sonra update-database komutlarý çalýþtýrýlýr.

7) DataAccess katmanýnda entity'ler üzerinden AppCore'daki RepoBase'den miras alan abstract (soyut) base repository'ler ile
bu base repository'lerden miras alan concrete (somut) repository'ler oluþturulur ve MvcWebUI katmanýnda Program.cs'teki IoC Container'da baðýmlýlklarý yönetilir.

8) Business katmanýnda entity'ler üzerinden model class'larý AppCore katmanýndaki RecordBase'den miras alacak þekilde oluþturulur,
eðer istenirse MvcWebUI katmanýnda view'larda kullanýlmak üzere formatlama, iliþkili referans özellikleri kullanma, vb. için yeni özellikler eklenebilir.

9) Business katmanýnda model'ler üzerinden entity <-> model dönüþümlerini gerçekleþtirip DataAccess katmanýndaki repository'ler üzerinden
veritabaný iþlemleri gerçekleþtirmek için AppCore'daki IService'i implemente eden interface'ler ile bu interface'leri implemente
eden concrete (somut) service'ler oluþturulur ve MvcWebUI katmanýnda Program.cs'teki IoC Container'da baðýmlýlklarý yönetilir.

10) MvcWebUI katmanýnda yönetilecek model için controller ve ilgili view'larý oluþturularak ilgili service constructor üzerinden enjekte edilir
ve controller action'larýnda methodlarý kullanýlarak model objeleri üzerinden iþlemler (örneðin CRUD) gerçekleþtirilir.

11) Eðer istenirse MvcWebUI katmanýndaki Views -> Shared klasörü altýndaki projede tüm oluþturulan view'larýn bir þablon içerisinde gösterilmesini
saðlayan _Layout.cshtml view'ý içerisinde controller ve action'lar üzerinden, örneðin menüye link'ler eklenebilir.

12) Eðer istenirse veritabanýndaki tüm verilerin sýfýrdan oluþturulmasýný saðlayan, örneðin MvcWebUI katmanýnda Areas klasöründeki Database area'sý içerisinde,
bir controller ve aksiyonu yazýlabilir.