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
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Status> Status { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=productcatalog.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // カテゴリ初期データ
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "電子機器" },
                new Category { Id = 2, Name = "家具" },
                new Category { Id = 3, Name = "衣料" },
                new Category { Id = 4, Name = "書籍" },
                new Category { Id = 5, Name = "食品" }
            );

            // ステータス初期データ
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "在庫あり" },
                new Status { Id = 2, Name = "在庫切れ" },
                new Status { Id = 3, Name = "販売終了" }
            );
        }
    }
}

