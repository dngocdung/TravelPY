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
    public class AdminTourController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; set; }


        public AdminTourController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminTour
        public IActionResult Index(int page = 1, int MaDanhMuc = 0)
        {
            var pageNumber = page;
            var pageSize = Utilities.PAGE_SIZE;

            List<Tour> lsTours = new List<Tour>();
            if (MaDanhMuc != 0)
            {
                lsTours = _context.Tours
                .AsNoTracking()
                .Where(x => x.MaDanhMuc == MaDanhMuc)
                .Include(x => x.MaDanhMucNavigation)
                .Include(t => t.MaHdvNavigation)
                .OrderBy(x => x.MaTour).ToList();
            }
            else
            {
                lsTours = _context.Tours
                .AsNoTracking()
                .Include(x => x.MaDanhMucNavigation)
                .Include(t => t.MaHdvNavigation)
                .OrderBy(x => x.MaTour).ToList();
            }



            PagedList<Tour> models = new PagedList<Tour>(lsTours.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateID = MaDanhMuc;

            ViewBag.CurrentPage = pageNumber;

            ViewData["DanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");

            return View(models);
        }

        public IActionResult Filtter(int MaDanhMuc = 0)
        {
            var url = $"/Admin/AdminTour?MaDanhMuc={MaDanhMuc}";
            if (MaDanhMuc == 0)
            {
                url = $"/Admin/AdminTour";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/AdminTour/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tours == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.MaDanhMucNavigation)
                .Include(t => t.MaHdvNavigation)
                .FirstOrDefaultAsync(m => m.MaTour == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // GET: Admin/AdminTour/Create
        public IActionResult Create()
        {
            ViewData["MaDanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewData["MaHdv"] = new SelectList(_context.HuongDanViens, "MaHdv", "TenHdv");
            return View();
        }

        // POST: Admin/AdminTour/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTour,TenTour,NgayKhoiHanh,GioKhoiHanh,Gia,GiaGiam,HinhAnh,PhuongTien,SoNgay,SoCho,MoTa,MaDanhMuc,MaHdv,TrangThai,NoiKhoiHanh,Alias")] Tour tour, Microsoft.AspNetCore.Http.IFormFile fHinhAnh)
        {
            if (ModelState.IsValid)
            {
                tour.TenTour = Utilities.ToTitleCase(tour.TenTour);
                if (fHinhAnh != null)
                {
                    string extension = Path.GetExtension(fHinhAnh.FileName);
                    string image = Utilities.SEOUrl(tour.TenTour) + extension;
                    tour.HinhAnh = await Utilities.UploadFile(fHinhAnh, @"tours", image.ToLower());
                }
                if (string.IsNullOrEmpty(tour.HinhAnh)) tour.HinhAnh = "default.jpg";
                tour.Alias = Utilities.SEOUrl(tour.TenTour);
                //tour.DateModified = DateTime.Now;
                //tour.DateCreated = DateTime.Now;

                _context.Add(tour);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc", tour.MaDanhMuc);
            ViewData["MaHdv"] = new SelectList(_context.HuongDanViens, "MaHdv", "TenHdv", tour.MaHdv);
            return View(tour);
        }

        // GET: Admin/AdminTour/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tours == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            ViewData["MaDanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc", tour.MaDanhMuc);
            ViewData["MaHdv"] = new SelectList(_context.HuongDanViens, "MaHdv", "TenHdv", tour.MaHdv);
            return View(tour);
        }

        // POST: Admin/AdminTour/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTour,TenTour,NgayKhoiHanh,GioKhoiHanh,Gia,GiaGiam,HinhAnh,PhuongTien,SoNgay,SoCho,MoTa,MaDanhMuc,MaHdv,TrangThai,NoiKhoiHanh,Alias")] Tour tour, Microsoft.AspNetCore.Http.IFormFile fHinhAnh)
        {
            if (id != tour.MaTour)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tour.TenTour = Utilities.ToTitleCase(tour.TenTour);
                    if (fHinhAnh != null)
                    {
                        string extension = Path.GetExtension(fHinhAnh.FileName);
                        string image = Utilities.SEOUrl(tour.TenTour) + extension;
                        tour.HinhAnh = await Utilities.UploadFile(fHinhAnh, @"tours", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(tour.HinhAnh)) tour.HinhAnh = "default.jpg";
                    tour.Alias = Utilities.SEOUrl(tour.TenTour);
                    //tour.DateModified = DateTime.Now;
                    //tour.DateCreated = DateTime.Now;

                    _context.Update(tour);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Thêm mới thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(tour.MaTour))
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
            ViewData["MaDanhMuc"] = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc", tour.MaDanhMuc);
            ViewData["MaHdv"] = new SelectList(_context.HuongDanViens, "MaHdv", "TenHdv", tour.MaHdv);
            return View(tour);
        }

        // GET: Admin/AdminTour/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tours == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.MaDanhMucNavigation)
                .Include(t => t.MaHdvNavigation)
                .FirstOrDefaultAsync(m => m.MaTour == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // POST: Admin/AdminTour/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tours == null)
            {
                return Problem("Entity set 'DbToursContext.Tours'  is null.");
            }
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xoá Tour thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool TourExists(int id)
        {
          return _context.Tours.Any(e => e.MaTour == id);
        }
    }
}
