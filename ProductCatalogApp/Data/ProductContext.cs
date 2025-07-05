using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Models;

namespace ProductCatalogApp.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext (DbContextOptions<ProductContext> options)
            : base(options)
        {
        }

        public DbSet<ProductCatalogApp.Models.Product> Product { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=productcatalog.db");
    }


}
