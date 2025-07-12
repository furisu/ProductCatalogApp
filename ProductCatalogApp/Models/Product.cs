using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Models
{
    public class Product
    {
        public int Id { get; set; } // 主キー（自動採番）

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

    }
}

