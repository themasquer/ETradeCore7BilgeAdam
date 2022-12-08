#nullable disable // null değer atanabilen referans tiplerin sonuna ? yazma uyarısını devre dışı bırakmak 
                  // ve ? konmadığında zorunlu hale gelmelerini engellemek için sadece entity ve modellerde kullanılmalıdır

using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class ProductModel : RecordBase // modeller de RecordBase'den miras almalıdır ki hem Id ve Guid alanlarını
                                           // miras alsın hem de servislerde tip olarak kullanılabilsin.
    {
        // ilgili entity'de referans olmayan özellikler veya başka bir deyişle veritabanındaki ilgili tablosundaki
        // sütun karşılığı olan özellikler entity'den kopyalanır.



        #region Entity'den Kopyalanan Özellikler
        [Required] // kullanıcıdan gelen model verisi validasyonu için zorunlu olacağını belirtir
        [StringLength(200)] // kullanıcıdan gelen model verisi validasyonu için verinin maksimum 200 karakter olacağını belirtir 
        [DisplayName("Product Name")]  // her view'da elle Product Name yazmak yerine model üzerinden bu özelliğin DisplayName'ini kullanacağız,
                                       // eğer yazılmazsa DisplayName üzerinden özelliğin adı (Name) kullanılır.  
        public string Name { get; set; } 



        [StringLength(500)] // kullanıcıdan gelen model verisi validasyonu için verinin maksimum 500 karakter olacağını belirtir 
        public string Description { get; set; } // kullanıcıdan gelen model verisi validasyonu için zorunlu olmayan özellik



        [DisplayName("Unit Price")]  
        public double UnitPrice { get; set; } // kullanıcıdan gelen model verisi validasyonu için zorunlu özellik



        [DisplayName("Stock Amount")]  
        public int StockAmount { get; set; } // kullanıcıdan gelen model verisi validasyonu için zorunlu özellik



        [DisplayName("Expiration Date")]  
        public DateTime? ExpirationDate { get; set; } // kullanıcıdan gelen model verisi validasyonu için zorunlu olmayan özellik



        [Required]
        [DisplayName("Category")]  
        public int? CategoryId { get; set; } // ürünün kategorisi view'da bir DropDownList üzerinden seçileceği ve Seçiniz item'ı üzerinden null gönderilebileceği
                                             // için CategoryId nullable yapılmalıdır ve eğer mutlaka seçim yapılması isteniyorsa Required attribute'u
                                             // (data annotation) ile işaretlenmelidir.
        #endregion

        #region View'larda Gösterim için Kullanacağımız Özellikler
        [DisplayName("Unit Price")] 
        public string UnitPriceDisplay { get; set; } // Obje üzerinden kolay bir şekilde gösterim amaçlı kullanım için kullanacağımız özellikleri belirtmek ve
                                                     // yukarıda UnitPrice özelliği olduğundan ve aynı özellik ismini kullanamayacağımızdan kendi belirlediğimiz
                                                     // bir son eki (Display) özellik adında kullanıyoruz.
                                                     // Display ile biten özelliklerin verilerini özelleştirerek (formatlama, ilişkili entity referansı üzerinden
                                                     // özellik kullanma, vb.) ilgili servisin Query methodunda atayacağız.



        [DisplayName("Expiration Date")] 
        public string ExpirationDateDisplay { get; set; }



        [DisplayName("Category")]
        public string CategoryNameDisplay { get; set; }
        #endregion
    }
}
