﻿using System;
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
    public class AdminPageController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; }
        public AdminPageController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminPage
        public async Task<IActionResult> Index()
        {
            var so = _context.BaiViets
                .AsNoTracking()
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)
                .Where(b => b.MaBaiViet == 1)
                .Count();
            //.OrderBy(x => x.MaBaiViet).ToList();
            ViewBag.So = so;
            return View(await _context.Pages.ToListAsync());
        }

        // GET: Admin/AdminPage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pages == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .FirstOrDefaultAsync(m => m.MaPage == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // GET: Admin/AdminPage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminPage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPage,TenPage,NoiDung,HinhAnh,SoBaiViet,Alias")] Page page, Microsoft.AspNetCore.Http.IFormFile fHinhAnh)
        {
            if (ModelState.IsValid)
            {
                //Xu ly Thumb
                if (fHinhAnh != null)
                {
                    string extension = Path.GetExtension(fHinhAnh.FileName);
                    string imageName = Utilities.SEOUrl(page.TenPage) + extension;
                    page.TenPage = await Utilities.UploadFile(fHinhAnh, @"pages", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(page.HinhAnh)) page.HinhAnh = "default.jpg";
                page.Alias = Utilities.SEOUrl(page.TenPage);
                _context.Add(page);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(page);
        }

        // GET: Admin/AdminPage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pages == null)
            {
                return NotFound();
            }

            var page = await _context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        // POST: Admin/AdminPage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPage,TenPage,NoiDung,HinhAnh,SoBaiViet,Alias")] Page page, Microsoft.AspNetCore.Http.IFormFile fHinhAnh)
        {
            if (id != page.MaPage)
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
                        string imageName = Utilities.SEOUrl(page.TenPage) + extension;
                        page.HinhAnh = await Utilities.UploadFile(fHinhAnh, @"pages", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(page.HinhAnh)) page.HinhAnh = "default.jpg";
                    page.Alias = Utilities.SEOUrl(page.TenPage);
                    _context.Update(page);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(page.MaPage))
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
            return View(page);
        }

        // GET: Admin/AdminPage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pages == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .FirstOrDefaultAsync(m => m.MaPage == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Admin/AdminPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pages == null)
            {
                return Problem("Entity set 'DbToursContext.Pages'  is null.");
            }
            var page = await _context.Pages.FindAsync(id);
            if (page != null)
            {
                _context.Pages.Remove(page);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageExists(int id)
        {
          return _context.Pages.Any(e => e.MaPage == id);
        }
    }
}
