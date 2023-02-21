using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelPY.Models;

namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminHuongDanVienController : Controller
    {
        private readonly DbToursContext _context;

        public AdminHuongDanVienController(DbToursContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminHuongDanVien
        public async Task<IActionResult> Index()
        {
              return View(await _context.HuongDanViens.ToListAsync());
        }

        // GET: Admin/AdminHuongDanVien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HuongDanViens == null)
            {
                return NotFound();
            }

            var huongDanVien = await _context.HuongDanViens
                .FirstOrDefaultAsync(m => m.MaHdv == id);
            if (huongDanVien == null)
            {
                return NotFound();
            }

            return View(huongDanVien);
        }

        // GET: Admin/AdminHuongDanVien/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminHuongDanVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHdv,TenHdv,NgaySinhHdv,DiaChiHdv,Sdt,HinhAnh")] HuongDanVien huongDanVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(huongDanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(huongDanVien);
        }

        // GET: Admin/AdminHuongDanVien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HuongDanViens == null)
            {
                return NotFound();
            }

            var huongDanVien = await _context.HuongDanViens.FindAsync(id);
            if (huongDanVien == null)
            {
                return NotFound();
            }
            return View(huongDanVien);
        }

        // POST: Admin/AdminHuongDanVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHdv,TenHdv,NgaySinhHdv,DiaChiHdv,Sdt,HinhAnh")] HuongDanVien huongDanVien)
        {
            if (id != huongDanVien.MaHdv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(huongDanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HuongDanVienExists(huongDanVien.MaHdv))
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
            return View(huongDanVien);
        }

        // GET: Admin/AdminHuongDanVien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HuongDanViens == null)
            {
                return NotFound();
            }

            var huongDanVien = await _context.HuongDanViens
                .FirstOrDefaultAsync(m => m.MaHdv == id);
            if (huongDanVien == null)
            {
                return NotFound();
            }

            return View(huongDanVien);
        }

        // POST: Admin/AdminHuongDanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HuongDanViens == null)
            {
                return Problem("Entity set 'DbToursContext.HuongDanViens'  is null.");
            }
            var huongDanVien = await _context.HuongDanViens.FindAsync(id);
            if (huongDanVien != null)
            {
                _context.HuongDanViens.Remove(huongDanVien);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HuongDanVienExists(int id)
        {
          return _context.HuongDanViens.Any(e => e.MaHdv == id);
        }
    }
}
