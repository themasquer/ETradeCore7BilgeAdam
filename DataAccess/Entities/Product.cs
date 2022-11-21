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
        public string? Name { get; set; } // özellik tipleri yanındaki ? sadece entity ve modellerde kullanılmalı

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public double? UnitPrice { get; set; }

        [Required]
        public int? StockAmount { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
