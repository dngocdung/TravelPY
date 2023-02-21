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
    public class AdminDatTourController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; set; }


        public AdminDatTourController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminDatTour

        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var lsDatTours = _context.DatTours
                .AsNoTracking()
                .OrderBy(x => x.NgayDatTour);
            PagedList<DatTour> models = new PagedList<DatTour>(lsDatTours, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);

        }

        // GET: Admin/AdminDatTour/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datTour = await _context.DatTours
                .Include(d => d.MaKhachHangNavigation)
                //.Include(d => d.MaKhachSanNavigation)
                .FirstOrDefaultAsync(m => m.MaDatTour == id);
            if (datTour == null)
            {
                return NotFound();
            }
            var Chitietdonhang = _context.ChiTietDatTours
                .Include(x => x.MaTourNavigation)
                .AsNoTracking()
                .Where(x => x.MaDatTour == datTour.MaDatTour)
                .OrderBy(x => x.MaChiTiet)
                .ToList();
            ViewBag.ChiTiet = Chitietdonhang;
            return View(datTour);
        }

        // GET: Admin/AdminDatTour/Create
        public IActionResult Create()
        {
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "MaKhachHang");
            //ViewData["MaKhachSan"] = new SelectList(_context.KhachSans, "MaKhachSan", "MaKhachSan");
            return View();
        }

        // POST: Admin/AdminDatTour/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDatTour,MaTour,MaKhachHang,SoCho,NgayDatTour,GhiChu,ThanhToan,MaThanhToan,NgayThanhToan,DiaChi,LocationId,Tinh,PhuongXa,TongTien")] DatTour datTour)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datTour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenKhachHang", datTour.MaKhachHang);
            //ViewData["MaKhachSan"] = new SelectList(_context.KhachSans, "MaKhachSan", "MaKhachSan", datTour.MaKhachSan);
            return View(datTour);
        }

        // GET: Admin/AdminDatTour/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DatTours == null)
            {
                return NotFound();
            }

            var datTour = await _context.DatTours.FindAsync(id);
            if (datTour == null)
            {
                return NotFound();
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenKhachHang", datTour.MaKhachHang);
            //ViewData["MaKhachSan"] = new SelectList(_context.KhachSans, "MaKhachSan", "MaKhachSan", datTour.MaKhachSan);
            return View(datTour);
        }

        // POST: Admin/AdminDatTour/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDatTour,MaTour,MaKhachHang,SoCho,NgayDatTour,GhiChu,ThanhToan,MaThanhToan,NgayThanhToan,DiaChi,LocationId,Tinh,PhuongXa,TongTien")] DatTour datTour)
        {
            if (id != datTour.MaDatTour)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datTour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatTourExists(datTour.MaDatTour))
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
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenKhachHang", datTour.MaKhachHang);
            //ViewData["MaKhachSan"] = new SelectList(_context.KhachSans, "MaKhachSan", "MaKhachSan", datTour.MaKhachSan);
            return View(datTour);
        }

        // GET: Admin/AdminDatTour/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DatTours == null)
            {
                return NotFound();
            }

            var datTour = await _context.DatTours
                .Include(d => d.MaKhachHangNavigation)
                //.Include(d => d.MaKhachSanNavigation)
                .FirstOrDefaultAsync(m => m.MaDatTour == id);
            if (datTour == null)
            {
                return NotFound();
            }
            var Chitietdonhang = _context.ChiTietDatTours
                .Include(x => x.MaTourNavigation)
                .AsNoTracking()
                .Where(x => x.MaDatTour == datTour.MaDatTour)
                .OrderBy(x => x.MaChiTiet)
                .ToList();
            ViewBag.ChiTiet = Chitietdonhang;
            return View(datTour);
        }

        // POST: Admin/AdminDatTour/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DatTours == null)
            {
                return Problem("Entity set 'DbToursContext.DatTours'  is null.");
            }
            var datTour = await _context.DatTours.FindAsync(id);
            if (datTour != null)
            {
                _context.DatTours.Remove(datTour);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatTourExists(int id)
        {
          return _context.DatTours.Any(e => e.MaDatTour == id);
        }
    }
}
