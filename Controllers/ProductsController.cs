using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Data;
using ProductCatalogApp.Models;

namespace ProductCatalogApp.Controllers
{

    public class ProductsController : Controller
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        // GET: Products    
        public async Task<IActionResult> Index
            (
                string searchString,
                decimal? minPrice,
                decimal? maxPrice,
                int? selectedCategory,
                int? selectedStatus,
                int page = 1,
                string sortOrder = ""
            )
        {
            // 1ページに表示する件数
            int pageSize = 5;

            // クエリの準備
            var products = _context.Product
                .Include(p => p.Category)
                .Include(p => p.Status)
                .AsQueryable();

            ViewBag.CategoryList = _context.Category.Select(c => c.Name).ToList();
            ViewBag.StatusList = _context.Status.Select(s => s.Name).ToList();



            // 製品名検索
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            // 価格フィルター
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            // カテゴリ絞り込み
            if (selectedCategory.HasValue)
            {
                products = products.Where(p => p.CategoryId == selectedCategory.Value);
            }

            // ステータス絞り込み
            if (selectedStatus.HasValue)
            {
                products = products.Where(p => p.StatusId == selectedStatus.Value);
            }


            // ソート設定
            ViewBag.CurrentSort = sortOrder;
            ViewBag.PriceSortParam = sortOrder == "price_asc" ? "price_desc" : "price_asc";

            products = sortOrder switch
            {
                "price_desc" => products.OrderByDescending(p => (double)p.Price),
                "price_asc" => products.OrderBy(p => (double)p.Price),
                _ => products.OrderBy(p => p.Id) // デフォルト：登録順
            };

            // ページネーション
            int totalItems = await products.CountAsync();
            var items = await products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            foreach (var p in items)
            {
                p.CreatedAt = ConvertToJst(p.CreatedAt);
                if (p.UpdatedAt.HasValue)
                {
                    p.UpdatedAt = ConvertToJst(p.UpdatedAt.Value);
                }
            }

            // ビューに渡す情報
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.SearchString = searchString;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SelectedCategory = selectedCategory;
            ViewBag.SelectedStatus = selectedStatus;
            SetSelectLists(selectedCategory, selectedStatus);


            return View(items);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.CreatedAt = ConvertToJst(product.CreatedAt);
            if (product.UpdatedAt.HasValue)
            {
                product.UpdatedAt = ConvertToJst(product.UpdatedAt.Value);
            }


            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var product = new Product
            {
                Price = 0,
                Stock = 0
            };

            SetSelectLists();

            return View(product);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId,Price,Description,ImageUrl,Stock,StatusId")] Product product)
        {
            // バリデーション失敗時のエラー内容を出力
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                foreach (var kv in ModelState)
                {
                    foreach (var error in kv.Value.Errors)
                    {
                        Console.WriteLine($"Error in {kv.Key}: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.UtcNow;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            SetSelectLists();


            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            SetSelectLists();


            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId,Price,Description,ImageUrl,Stock,StatusId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Product.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    product.CreatedAt = existingProduct.CreatedAt;

                    // 更新日時を現在時刻に設定
                    product.UpdatedAt = DateTime.UtcNow;


                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            SetSelectLists();


            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        private void SetSelectLists(int? selectedCategory, int? selectedStatus)
        {
            ViewBag.CategoryList = new SelectList(_context.Category.ToList(), "Id", "Name", selectedCategory);
            ViewBag.StatusList = new SelectList(_context.Status.ToList(), "Id", "Name", selectedStatus);
        }

        private void SetSelectLists()
        {
            SetSelectLists(null, null);
        }

        private static DateTime ConvertToJst(DateTime utcDateTime)
        {
            var jst = TimeZoneInfo.FindSystemTimeZoneById(
                System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows)
                    ? "Tokyo Standard Time" : "Asia/Tokyo"
            );
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, jst);
        }

    }
}
