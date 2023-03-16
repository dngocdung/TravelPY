using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelPY.Helpper;
using TravelPY.Models;

namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminKhachSanController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; }
        public AdminKhachSanController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminKhachSan
        public async Task<IActionResult> Index()
        {
              return _context.KhachSans != null ? 
                          View(await _context.KhachSans.ToListAsync()) :
                          Problem("Entity set 'DbToursContext.KhachSans'  is null.");
        }

        // GET: Admin/AdminKhachSan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KhachSans == null)
            {
                return NotFound();
            }

            var khachSan = await _context.KhachSans
                .FirstOrDefaultAsync(m => m.MaKhachSan == id);
            if (khachSan == null)
            {
                return NotFound();
            }

            return View(khachSan);
        }

        // GET: Admin/AdminKhachSan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminKhachSan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachSan,TenKhachSan,DiaChi,SoPhong,Sdt,MoTa,HinhAnh,Alias")] KhachSan khachSan, Microsoft.AspNetCore.Http.IFormFile fHinhAnh)
        {
            if (ModelState.IsValid)
            {
                if (fHinhAnh != null)
                {
                    string extension = Path.GetExtension(fHinhAnh.FileName);
                    string imageName = Utilities.SEOUrl(khachSan.TenKhachSan) + extension;
                    khachSan.HinhAnh = await Utilities.UploadFile(fHinhAnh, @"khachsans", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(khachSan.HinhAnh)) khachSan.HinhAnh = "default.jpg";
                khachSan.Alias = Utilities.SEOUrl(khachSan.TenKhachSan);
                //baiViet.NgayTao = DateTime.Now;

                _context.Add(khachSan);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(khachSan);
        }

        // GET: Admin/AdminKhachSan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KhachSans == null)
            {
                return NotFound();
            }

            var khachSan = await _context.KhachSans.FindAsync(id);
            if (khachSan == null)
            {
                return NotFound();
            }
            return View(khachSan);
        }

        // POST: Admin/AdminKhachSan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKhachSan,TenKhachSan,DiaChi,SoPhong,Sdt,MoTa,HinhAnh,Alias")] KhachSan khachSan, Microsoft.AspNetCore.Http.IFormFile fHinhAnh)
        {
            if (id != khachSan.MaKhachSan)
            {
                return NotFound();
            }

            
                try
                {
                    if (fHinhAnh != null)
                    {
                        string extension = Path.GetExtension(fHinhAnh.FileName);
                        string imageName = Utilities.SEOUrl(khachSan.TenKhachSan) + extension;
                        khachSan.HinhAnh = await Utilities.UploadFile(fHinhAnh, @"khachsans", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(khachSan.HinhAnh)) khachSan.HinhAnh = "default.jpg";
                    khachSan.Alias = Utilities.SEOUrl(khachSan.TenKhachSan);
                    //khachSan.NgayTao = DateTime.Now;
                    _context.Update(khachSan);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Sửa thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachSanExists(khachSan.MaKhachSan))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            //return View(khachSan);
        }

        // GET: Admin/AdminKhachSan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KhachSans == null)
            {
                return NotFound();
            }

            var khachSan = await _context.KhachSans
                .FirstOrDefaultAsync(m => m.MaKhachSan == id);
            if (khachSan == null)
            {
                return NotFound();
            }

            return View(khachSan);
        }

        // POST: Admin/AdminKhachSan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KhachSans == null)
            {
                return Problem("Entity set 'DbToursContext.KhachSans'  is null.");
            }
            var khachSan = await _context.KhachSans.FindAsync(id);
            if (khachSan != null)
            {
                _context.KhachSans.Remove(khachSan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachSanExists(int id)
        {
          return (_context.KhachSans?.Any(e => e.MaKhachSan == id)).GetValueOrDefault();
        }
    }
}
