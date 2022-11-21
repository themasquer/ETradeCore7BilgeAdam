#nullable disable // null değer atanabilen referans tiplerin sonuna ? yazma uyarısını devre dışı bırakmak 
                  // ve ? konmadığında zorunlu hale gelmelerini engellemek için sadece entity ve modellerde kullanılmalıdır

using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Category : RecordBase // Kategori
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } // zorunlu (tabloya null kaydedilemeyen) özellik

        public string Description { get; set; } // zorunlu olmayan (tabloya null kaydedilebilen) özellik

        public List<Product> Products { get; set; } // başka entity kolleksiyonuna referans
    }
}
