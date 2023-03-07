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
    public class AdminDanhMucController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; }
        public AdminDanhMucController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminDanhMuc
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var lsDanhMucs = _context.DanhMucs
                .AsNoTracking()
                .OrderBy(x => x.MaDanhMuc);
            var lsTour = _context.Tours.AsNoTracking().OrderBy(x => x.MaTour);
            var soTour = 0;
            foreach(var item in lsTour)
            {
                
                soTour++;
            }    
            PagedList<DanhMuc> models = new PagedList<DanhMuc>(lsDanhMucs, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.Total = soTour;
            return View(models);

        }

        // GET: Admin/AdminDanhMuc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DanhMucs == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs
                .FirstOrDefaultAsync(m => m.MaDanhMuc == id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // GET: Admin/AdminDanhMuc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminDanhMuc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDanhMuc,TenDanhMuc,Mota,SoTour,Alias,HinhAnh")] DanhMuc danhMuc, Microsoft.AspNetCore.Http.IFormFile fHinhAnh)
        {
            if (ModelState.IsValid)
            {
                //Xu ly Thumb
                if (fHinhAnh != null)
                {
                    string extension = Path.GetExtension(fHinhAnh.FileName);
                    string imageName = Utilities.SEOUrl(danhMuc.TenDanhMuc) + extension;
                    danhMuc.HinhAnh = await Utilities.UploadFile(fHinhAnh, @"hinhanh", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(danhMuc.HinhAnh)) danhMuc.HinhAnh = "default.jpg";
                danhMuc.Alias = Utilities.SEOUrl(danhMuc.TenDanhMuc);
                _context.Add(danhMuc);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(danhMuc);
        }

        // GET: Admin/AdminDanhMuc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DanhMucs == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc == null)
            {
                return NotFound();
            }
            return View(danhMuc);
        }

        // POST: Admin/AdminDanhMuc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDanhMuc,TenDanhMuc,Mota,SoTour,Alias,HinhAnh")] DanhMuc danhMuc, Microsoft.AspNetCore.Http.IFormFile fHinhAnh)
        {
            if (id != danhMuc.MaDanhMuc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (fHinhAnh != null)
                    {
                        string extension = Path.GetExtension(fHinhAnh.FileName);
                        string imageName = Utilities.SEOUrl(danhMuc.TenDanhMuc) + extension;
                        danhMuc.HinhAnh = await Utilities.UploadFile(fHinhAnh, @"hinhanh", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(danhMuc.HinhAnh)) danhMuc.HinhAnh = "default.jpg";

                    _context.Update(danhMuc);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Thêm mới thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucExists(danhMuc.MaDanhMuc))
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
            return View(danhMuc);
        }

        // GET: Admin/AdminDanhMuc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DanhMucs == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs
                .FirstOrDefaultAsync(m => m.MaDanhMuc == id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // POST: Admin/AdminDanhMuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DanhMucs == null)
            {
                return Problem("Entity set 'dbToursContext.DanhMucs'  is null.");
            }
            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc != null)
            {
                _context.DanhMucs.Remove(danhMuc);
            }

            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool DanhMucExists(int id)
        {
          return _context.DanhMucs.Any(e => e.MaDanhMuc == id);
        }
    }
}
