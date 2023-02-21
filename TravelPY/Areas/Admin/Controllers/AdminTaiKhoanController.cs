using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelPY.Models;

namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminTaiKhoanController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfServices { get; set; }
        public AdminTaiKhoanController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfServices= notyfService;
        }

        // GET: Admin/AdminTaiKhoan
        public async Task<IActionResult> Index(int page = 1, int MaDanhMuc = 0)
        {
            ViewData["QuyenTruyCap"] = new SelectList(_context.VaiTros, "MaVaiTro", "MoTa");
            List<SelectListItem> lsTrangThai = new List<SelectListItem>();
            lsTrangThai.Add(new SelectListItem() { Text = "Đang hoạt động", Value = "1" });
            lsTrangThai.Add(new SelectListItem() { Text = "Không hoạt động", Value = "0" });
            ViewData["lsTrangThai"] = lsTrangThai;
            var dbToursContext = _context.TaiKhoans.Include(t => t.MaVaiTroNavigation);
            return View(await dbToursContext.ToListAsync());
        }

        // GET: Admin/AdminTaiKhoan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TaiKhoans == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.MaVaiTroNavigation)
                .FirstOrDefaultAsync(m => m.MaTaiKhoan == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // GET: Admin/AdminTaiKhoan/Create
        public IActionResult Create()
        {
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro");
            return View();
        }

        // POST: Admin/AdminTaiKhoan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTaiKhoan,TenTaiKhoan,Sdt,Email,TrangThai,MaVaiTro,NgayTao,LastLogin,MatKhau")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taiKhoan);
                await _context.SaveChangesAsync();
                _notyfServices.Success("Tạo mới tài khoản thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro", taiKhoan.MaVaiTro);
            return View(taiKhoan);
        }

        // GET: Admin/AdminTaiKhoan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TaiKhoans == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro", taiKhoan.MaVaiTro);
            return View(taiKhoan);
        }

        // POST: Admin/AdminTaiKhoan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTaiKhoan,TenTaiKhoan,Sdt,Email,TrangThai,MaVaiTro,NgayTao,LastLogin,MatKhau")] TaiKhoan taiKhoan)
        {
            if (id != taiKhoan.MaTaiKhoan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taiKhoan);
                    await _context.SaveChangesAsync();
                    _notyfServices.Success("Cập nhập thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(taiKhoan.MaTaiKhoan))
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
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro", taiKhoan.MaVaiTro);
            return View(taiKhoan);
        }

        // GET: Admin/AdminTaiKhoan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TaiKhoans == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.MaVaiTroNavigation)
                .FirstOrDefaultAsync(m => m.MaTaiKhoan == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // POST: Admin/AdminTaiKhoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TaiKhoans == null)
            {
                return Problem("Entity set 'DbToursContext.TaiKhoans'  is null.");
            }
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan != null)
            {
                _context.TaiKhoans.Remove(taiKhoan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanExists(int id)
        {
          return _context.TaiKhoans.Any(e => e.MaTaiKhoan == id);
        }
    }
}
