using ProductCatalogApp.Models;
using System.ComponentModel.DataAnnotations;

public class Status
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public List<Product> Products { get; set; } = new();
}
