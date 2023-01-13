using Business.Models;
using Business.Models.Report;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace Business.Services.Report
{
    public interface IReportService // bu interface'in IService'i implemente etmesine gerek yoktur çünkü bu servis CRUD işlemleri yapmayacak.
    {
        // ReportService class'ındaki method tanımları
        List<ReportItemModel> GetList(ReportFilterModel filter, bool useInnerJoin = false);
    }

    public class ReportService : IReportService
    {
        private readonly ProductRepoBase _repo;

        public ReportService(ProductRepoBase repo)
        {
            _repo = repo;
        }

        /* SQL sorgusu örnek:
        select p.Name [Product Name], p.Description [Product Description], FORMAT(p.UnitPrice, 'C2') [Unit Price],
        CAST(p.StockAmount as varchar) + ' units' [Stock Amount], CONVERT(varchar(10), p.ExpirationDate, 101) [Expiration Date],
        c.Name Category, c.Description [Category Description], s.Name + case when s.IsVirtual = 1 then ' (Virtual)' else ' (Real)' end as Store
        from Products p inner join Categories c
        on p.CategoryId = c.Id
        inner join ProductStores ps
        on ps.ProductId = p.Id
        inner join Stores s
        on ps.StoreId = s.Id
        --where p.UnitPrice < 3000 and c.Id = 1 and s.Id = 2 -- category id ve store id uygulama üzerinden Seed işlemi yapıldıktan sonra değişebilir
        --order by s.Name, c.Name, p.Name
        */
        public List<ReportItemModel> GetList(ReportFilterModel filter, bool useInnerJoin = false) // SQL Inner Join iki tablo arasında sadece primary key ve foreign key id'leri üzerinden eşleşenleri getirir. 
        {
            #region Sorgu oluşturma
            var productQuery = _repo.Query(); // Products sorgusu
            var categoryQuery = _repo.Query<Category>(); // Categories sorgusu,
                                                         // RepoBase'deki Query methodunu Product tipi dışında başka bir tip (örneğin burada Category) tanımlayarak çağırabiliriz.
            var storeQuery = _repo.Query<Store>(); // Stores sorgusu
            var productStoreQuery = _repo.Query<ProductStore>(); // ProductStores sorgusu

            var query = from product in productQuery // product: Product tipindeki delege
                        join category in categoryQuery // category: Category tipindeki delege
                        on product.CategoryId equals category.Id // Category -> Id primary key'i ile Product -> CategoryId foreign key'lerini eşleştiriyoruz
                        join productStore in productStoreQuery // productStore: ProductStore tipindeki delege
                        on product.Id equals productStore.ProductId // Product -> Id primary key'i ile ProductStore -> ProductId foreign key'lerini eşleştiriyoruz
                        join store in storeQuery // store: Store tipindeki delege
                        on productStore.StoreId equals store.Id // Store -> Id primary key'i ile ProductStore -> StoreId foreign key'lerini eşleştiriyoruz

                        //where product.UnitPrice < 3000 && category.Id == 1 && store.Id == 2 // eğer istenirse sorguya where koşulu eklenebilir
                        //orderby store.Name, category.Name, product.Name // eğer istenirse sorguya order by eklenebilir

                        select new ReportItemModel() // entity delegeleri üzerinden çektiğimiz veri üzerinden modeli oluşturuyoruz
                        {
                            CategoryDescription = category.Description,
                            CategoryName = category.Name,
                            ExpirationDate = product.ExpirationDate.HasValue ? product.ExpirationDate.Value.ToString("MM/dd/yyyy") : "",
                            ProductDescription = product.Description,
                            ProductName = product.Name,
                            StockAmount = product.StockAmount + " units",
                            StoreName = store.Name + (store.IsVirtual ? " (Virtual)" : " (Real)"),
                            UnitPrice = product.UnitPrice.ToString("C2"),

                            CategoryId = category.Id // aşağıda CategoryId üzerinden filtreleme yapabilmek için atamalıyız,
                                                     // bunun için de ReportItemModel'a CategoryId özelliğini eklemeliyiz
                        };
            #endregion

            #region Sıralama
            // sorgu üzerinden where, order by, vb. işlemleri sorguyu oluşturduktan sonra uygulamak daha uygundur,
            // önce mağaza adına, mağaza adı aynı olanlar için sonra kategori adına, mağaza adı ve kategori adı aynı olanlar için de
            // en son ürün adına göre artan sıralıyoruz
            query = query.OrderBy(q => q.StoreName).ThenBy(q => q.CategoryName).ThenBy(q => q.ProductName);
            #endregion

            #region Filtreleme
            if (filter is not null)
            {
                if (filter.CategoryId.HasValue)
                {
                    query = query.Where(q => q.CategoryId == filter.CategoryId);
                    // q yukarıda oluşturduğumuz sorguya (query) delegelik yapıyor dolayısıyla yukarıdaki sorguda
                    // Select içerisinde CategoryId ataması (mapping işlemi) yapmalıyız ki burada kullanabilelim,
                    // bunun için de öncelikle ReportItemModel'a CategoryId özelliği eklemeliyiz
                }
            }
            #endregion

            return query.ToList(); // ToList methodu ile sorgumuzu çalıştırıp sonucu List<ReportModel> tipinde methoddan dönüyoruz 
        }
    }
}
