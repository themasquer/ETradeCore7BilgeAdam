#nullable disable // eğer istenirse null değer atanabilen referans tiplerle ilgili gelen yeşil uyarıları dosyada devre dışı bırakmak için kullanılabilir

using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class ETradeContext : DbContext // veritabanı tablolarına DbSet'ler üzerinden ulaşarak CRUD işlemleri yapacağımız sınıf
    {
        // tüm entity'ler için DbSet özellikleri oluşturulmalı

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Store> Stores { get; set; }

        public ETradeContext(DbContextOptions options) : base(options) // options parametresi MvcWebUI katmanındaki Program.cs IoC Container'ında AddDbContext methodu ile
                                                                       // bağımlılığı yönetilen ve appsettings.json veya appsettings.Development.json dosyalarında
                                                                       // tanımlanmış connection string'i bu class'ın constructor'ına, dolayısıyla esas veritabanı
                                                                       // işlemlerini yapacak olan DbContext class'ının constructor'ına taşır. Genelde bu kullanım tercih edilir.
        {

        }

        // eğer istenirse connection string DbContext'in OnConfiguring methodu ezilerek de tanımlanıp kullanılabilir, genelde bu kullanım tercih edilmez.
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // 1. yöntem: Windows Authentication
        //    //string connectionString = "server=.\\SQLEXPRESS;database=BA_ETradeCore7;trusted_connection=true;multipleactiveresultsets=true;trustservercertificate=true;";

        //    // 2. yöntem: SQL Server Authentication
        //    string connectionString = "server=.\\SQLEXPRESS;database=BA_ETradeCore7;user id=sa;password=sa;multipleactiveresultsets=true;trustservercertificate=true;";

        //    optionsBuilder.UseSqlServer(connectionString);
        //}
    }
}
