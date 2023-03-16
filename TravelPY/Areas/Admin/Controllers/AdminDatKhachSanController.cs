using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using TravelPY.Helpper;
using TravelPY.Models;

namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDatKhachSanController : Controller
    {
        private readonly DbToursContext _context;

        public AdminDatKhachSanController(DbToursContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminDatKhachSan
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var lsDatTours = _context.DatKhachSans
                .Include(d => d.MaKhachHangNavigation)
                .AsNoTracking()
                .OrderByDescending(x => x.NgayDat);
            PagedList<DatKhachSan> models = new PagedList<DatKhachSan>(lsDatTours, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);

            //var dbToursContext = _context.DatKhachSans.Include(d => d.MaChiTietKsNavigation).Include(d => d.MaKhachHangNavigation);
            //return View(await dbToursContext.ToListAsync());
        }

        // GET: Admin/AdminDatKhachSan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DatKhachSans == null)
            {
                return NotFound();
            }

            var datKhachSan = await _context.DatKhachSans
                //.Include(d => d.MaChiTietKsNavigation)
                .Include(d => d.MaKhachHangNavigation)
                .FirstOrDefaultAsync(m => m.MaDatKs == id);
            if (datKhachSan == null)
            {
                return NotFound();
            }

            return View(datKhachSan);
        }

        // GET: Admin/AdminDatKhachSan/Create
        public IActionResult Create()
        {
            //ViewData["MaChiTietKs"] = new SelectList(_context.ChiTietDatKs, "MaChiTietKs", "MaChiTietKs");
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "MaKhachHang");
            return View();
        }

        // POST: Admin/AdminDatKhachSan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDatKs,MaKhachHang,MaKhachSan,MaChiTietKs,NgayDat,NgayDen,NgayDi,NumAdults,NumChildrens,Gia")] DatKhachSan datKhachSan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datKhachSan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["MaChiTietKs"] = new SelectList(_context.ChiTietDatKs, "MaChiTietKs", "MaChiTietKs", datKhachSan.MaChiTietKs);
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "MaKhachHang", datKhachSan.MaKhachHang);
            return View(datKhachSan);
        }

        // GET: Admin/AdminDatKhachSan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DatKhachSans == null)
            {
                return NotFound();
            }

            var datKhachSan = await _context.DatKhachSans.FindAsync(id);
            if (datKhachSan == null)
            {
                return NotFound();
            }
            //ViewData["MaChiTietKs"] = new SelectList(_context.ChiTietDatKs, "MaChiTietKs", "MaChiTietKs", datKhachSan.MaChiTietKs);
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "MaKhachHang", datKhachSan.MaKhachHang);
            return View(datKhachSan);
        }

        // POST: Admin/AdminDatKhachSan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDatKs,MaKhachHang,MaKhachSan,MaChiTietKs,NgayDat,NgayDen,NgayDi,NumAdults,NumChildrens,Gia")] DatKhachSan datKhachSan)
        {
            if (id != datKhachSan.MaDatKs)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datKhachSan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatKhachSanExists(datKhachSan.MaDatKs))
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
            //ViewData["MaChiTietKs"] = new SelectList(_context.ChiTietDatKs, "MaChiTietKs", "MaChiTietKs", datKhachSan.MaChiTietKs);
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "MaKhachHang", datKhachSan.MaKhachHang);
            return View(datKhachSan);
        }

        // GET: Admin/AdminDatKhachSan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DatKhachSans == null)
            {
                return NotFound();
            }

            var datKhachSan = await _context.DatKhachSans
                //.Include(d => d.MaChiTietKsNavigation)
                .Include(d => d.MaKhachHangNavigation)
                .FirstOrDefaultAsync(m => m.MaDatKs == id);
            if (datKhachSan == null)
            {
                return NotFound();
            }

            return View(datKhachSan);
        }

        // POST: Admin/AdminDatKhachSan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DatKhachSans == null)
            {
                return Problem("Entity set 'DbToursContext.DatKhachSans'  is null.");
            }
            var datKhachSan = await _context.DatKhachSans.FindAsync(id);
            if (datKhachSan != null)
            {
                _context.DatKhachSans.Remove(datKhachSan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatKhachSanExists(int id)
        {
          return (_context.DatKhachSans?.Any(e => e.MaDatKs == id)).GetValueOrDefault();
        }
    }
}
