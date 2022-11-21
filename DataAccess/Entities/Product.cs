#nullable disable // null değer atanabilen referans tiplerin sonuna ? yazma uyarısını devre dışı bırakmak 
                  // ve ? konmadığında zorunlu hale gelmelerini engellemek için sadece entity ve modellerde kullanılmalıdır

using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    //[Table("ETradeProducts")] // Product entity'sinin karşılığında kullanılacak tablo ismini değiştirmek için kullanılabilir, ETradeContext OnModelCreating methodunda yazmak daha uygundur
    public class Product : RecordBase // Ürün
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } // zorunlu (tabloya null kaydedilemeyen) özellik

        [StringLength(500)]
        public string Description { get; set; } // zorunlu özellik

        public double UnitPrice { get; set; } // zorunlu özellik

        public int StockAmount { get; set; } // zorunlu özellik

        public DateTime? ExpirationDate { get; set; } // zorunlu olmayan (tabloya null kaydedilebilen) özellik

        public int CategoryId { get; set; } // zorunlu özellik

        public Category Category { get; set; } // başka entity'e referans
    }
}
