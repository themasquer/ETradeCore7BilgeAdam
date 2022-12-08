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
ve �nce add-migration v1 daha sonra update-database komutlar� �al��t�r�l�r.

7) DataAccess katman�nda entity'ler �zerinden AppCore'daki RepoBase'den miras alan abstract (soyut) base repository'ler ile
bu base repository'lerden miras alan concrete (somut) repository'ler olu�turulur ve MvcWebUI katman�nda Program.cs'teki IoC Container'da ba��ml�lklar� y�netilir.

8) Business katman�nda entity'ler �zerinden model class'lar� AppCore katman�ndaki RecordBase'den miras alacak �ekilde olu�turulur,
e�er istenirse MvcWebUI katman�nda view'larda kullan�lmak �zere formatlama, ili�kili referans �zellikleri kullanma, vb. i�in yeni �zellikler eklenebilir.

9) Business katman�nda model'ler �zerinden entity <-> model d�n���mlerini ger�ekle�tirip DataAccess katman�ndaki repository'ler �zerinden
veritaban� i�lemleri ger�ekle�tirmek i�in AppCore'daki IService'i implemente eden interface'ler ile bu interface'leri implemente
eden concrete (somut) service'ler olu�turulur ve MvcWebUI katman�nda Program.cs'teki IoC Container'da ba��ml�lklar� y�netilir.

10) MvcWebUI katman�nda y�netilecek model i�in controller ve ilgili view'lar� olu�turularak ilgili service constructor �zerinden enjekte edilir
ve controller action'lar�nda methodlar� kullan�larak model objeleri �zerinden i�lemler (�rne�in CRUD) ger�ekle�tirilir.

11) E�er istenirse MvcWebUI katman�ndaki Views -> Shared klas�r� alt�ndaki projede t�m olu�turulan view'lar�n bir �ablon i�erisinde g�sterilmesini
sa�layan _Layout.cshtml view'� i�erisinde controller ve action'lar �zerinden, �rne�in men�ye link'ler eklenebilir.

12) E�er istenirse veritaban�ndaki t�m verilerin s�f�rdan olu�turulmas�n� sa�layan, �rne�in MvcWebUI katman�nda Areas klas�r�ndeki Database area's� i�erisinde,
bir controller ve aksiyonu yaz�labilir.