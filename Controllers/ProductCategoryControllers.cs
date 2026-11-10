using Microsoft.AspNetCore.Mvc;
using day5_new.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace day5_new.Controllers
{
    public class ProductCategoryController : Controller
    {
        // Khai báo số lượng item trên 1 trang
        private readonly int pageSize = 5;
      
        private static List<ProductCategoryViewModel> _items = new List<ProductCategoryViewModel>
        {
            new ProductCategoryViewModel { Id = 1, Name = "Laptop Gaming", Description = "Core i7, RTX 4060", Price = 25000000, CategoryName = "Laptop" },
            new ProductCategoryViewModel { Id = 2, Name = "Laptop Văn Phòng", Description = "Core i5, GTX 1650", Price = 15000000, CategoryName = "Laptop" },
            new ProductCategoryViewModel { Id = 3, Name = "Điện thoại Pro", Description = "Màn hình OLED 120Hz", Price = 18000000, CategoryName = "Điện thoại" },
            new ProductCategoryViewModel { Id = 4, Name = "Điện thoại Cơ Bản", Description = "Màn hình LCD 60Hz", Price = 5000000, CategoryName = "Điện thoại" },
            new ProductCategoryViewModel { Id = 5, Name = "Máy tính bảng", Description = "Màn hình Retina", Price = 12000000, CategoryName = "Máy tính bảng" },
            new ProductCategoryViewModel { Id = 6, Name = "Máy tính bảng Pro", Description = "Màn hình Retina, hỗ trợ bút cảm ứng", Price = 20000000, CategoryName = "Máy tính bảng" }
        };

        // Hàm Index xử lý tìm kiếm và phân trang
        public IActionResult Index(string searchString, int page = 1)
        {
            var items = _items.AsEnumerable();

            // 1. Lọc theo từ khóa tìm kiếm nếu có
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(x => x.Name != null && x.Name.Contains(searchString, System.StringComparison.OrdinalIgnoreCase));
            }

            // 2. Tính toán phân trang
            int totalItems = items.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            
            // Đảm bảo số trang hợp lệ
            page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));

            // Cắt dữ liệu lấy đúng số lượng của trang hiện tại
            var pagedItems = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // 3. Truyền dữ liệu sang View bằng ViewData (hoặc ViewBag)
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;

            return View(pagedItems);
        }

        // 2. Thêm mới (Hiển thị Form)
        public IActionResult Create()
        {
            return View();
        }

        // 2. Thêm mới (Xử lý dữ liệu gửi lên)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = _items.Count > 0 ? _items.Max(x => x.Id) + 1 : 1;
                _items.Add(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // 3. Sửa (Hiển thị Form theo ID)
        public IActionResult Edit(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // 3. Sửa (Xử lý cập nhật)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existing = _items.FirstOrDefault(x => x.Id == model.Id);
                if (existing != null)
                {
                    existing.Name = model.Name;
                    existing.Description = model.Description;
                    existing.Price = model.Price;
                    existing.CategoryName = model.CategoryName;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // 4. Xóa
        public IActionResult Delete(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _items.Remove(item);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}