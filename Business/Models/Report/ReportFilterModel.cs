using System.ComponentModel;

namespace Business.Models.Report
{
    public class ReportFilterModel // view'da filtreleme kısmında kullanıcıdan alacağımız input'lar
    {
        [DisplayName("Category")]
        public int? CategoryId { get; set; } // view'daki tüm kategoriler işlemi için null geleceğinden nullable tanımladık
    }
}
