using GiyimSitesi.Context.Entity;

namespace GiyimSitesi.Models
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; } // Ürün listesi
        public int PageNumber { get; set; } // Şu anki sayfa numarası
        public int PageCount { get; set; } // Toplam sayfa sayısı
        public int? categoryId { get; set; } // Toplam sayfa sayısı
        public int? minPrice { get; set; } // Toplam sayfa sayısı
        public int? maxPrice { get; set; } // Toplam sayfa sayısı
        public string searchTerm { get; set; } // Toplam sayfa sayısı


    }
}
