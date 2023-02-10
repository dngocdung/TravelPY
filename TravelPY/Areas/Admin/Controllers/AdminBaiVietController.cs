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
    public class AdminBaiVietController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; }
        public AdminBaiVietController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService= notyfService;
        }

        // GET: Admin/AdminBaiViet
        public IActionResult Index(int page = 1, int MaPage = 0)
        {
            //Danh sach bai viet
            var collection = _context.BaiViets
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)
                .AsNoTracking().ToList();
            //Luu vao database
            foreach (var item in collection)
            {
                if (item.NgayTao == null)
                {
                    item.NgayTao = DateTime.Now;
                    _context.Update(item);
                    _context.SaveChanges();
                }
            }
            //Phan trang cho bai viet
            var pageNumber = page;
            var pageSize = Utilities.PAGE_SIZE;
            List<BaiViet> lsBaiViets = new List<BaiViet>();
            if(MaPage!=0)
            {
                lsBaiViets = _context.BaiViets
                .AsNoTracking()
                .Where(b => b.MaPage == MaPage)
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)               
                .OrderBy(x => x.MaBaiViet).ToList();
            }    
            else
            {
                lsBaiViets = _context.BaiViets
                .AsNoTracking()
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)               
                .OrderBy(x => x.MaBaiViet).ToList();
            }    
            /*var lsBaiViets = _context.BaiViets
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)
                .AsNoTracking()
                .OrderBy(x => x.MaBaiViet);*/
            PagedList<BaiViet> models = new PagedList<BaiViet>(lsBaiViets.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentMaPage = MaPage;
            ViewBag.CurrentPage = pageNumber;
            ViewData["MaPage"] = new SelectList(_context.Pages, "MaPage", "TenPage");
            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "TenTaiKhoan");
            return View(models);
        }

        //Chuan hoa duong dan khi chon danh muc
        public IActionResult Filtter(int MaPage = 0)
        {
            var url = $"/Admin/AdminBaiViet?MaBaiViet={MaPage}";
            if (MaPage == 0)
            {
                url = $"/Admin/AdminBaiViet";
            }
            return Json(new { status = "success", redirectUrl = url });
        }      

        // GET: Admin/AdminBaiViet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BaiViets == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViets
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)
                .FirstOrDefaultAsync(m => m.MaBaiViet == id);
            if (baiViet == null)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        // GET: Admin/AdminBaiViet/Create
        public IActionResult Create()
        {
            ViewData["MaPage"] = new SelectList(_context.Pages, "MaPage", "TenPage");
            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "TenTaiKhoan");
            return View();
        }

        // POST: Admin/AdminBaiViet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaBaiViet,TieuDe,HinhAnh,Publisher,NoiDung,NgayTao,MaTaiKhoan,MaPage,Alias")] BaiViet baiViet, Microsoft.AspNetCore.Http.IFormFile fHinhAnh)
        {
            if (ModelState.IsValid)
            {
                if (fHinhAnh != null)
                {
                    string extension = Path.GetExtension(fHinhAnh.FileName);
                    string imageName = Utilities.SEOUrl(baiViet.TieuDe) + extension;
                    baiViet.HinhAnh = await Utilities.UploadFile(fHinhAnh, @"news", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(baiViet.HinhAnh)) baiViet.HinhAnh = "default.jpg";
                baiViet.Alias = Utilities.SEOUrl(baiViet.TieuDe);
                baiViet.NgayTao = DateTime.Now;

                _context.Add(baiViet);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));

            }
            ViewData["MaPage"] = new SelectList(_context.Pages, "MaPage", "TenPage", baiViet.MaPage);
            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "TenTaiKhoan", baiViet.MaTaiKhoan);
            return View(baiViet);
        }

        // GET: Admin/AdminBaiViet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BaiViets == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViets.FindAsync(id);
            if (baiViet == null)
            {
                return NotFound();
            }
            ViewData["MaPage"] = new SelectList(_context.Pages, "MaPage", "MaPage", baiViet.MaPage);
            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "MaTaiKhoan", baiViet.MaTaiKhoan);
            return View(baiViet);
        }

        // POST: Admin/AdminBaiViet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaBaiViet,TieuDe,HinhAnh,Publisher,NoiDung,NgayTao,MaTaiKhoan,MaPage,Alias")] BaiViet baiViet, Microsoft.AspNetCore.Http.IFormFile fHinhAnh)
        {
            if (id != baiViet.MaBaiViet)
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
                        string imageName = Utilities.SEOUrl(baiViet.TieuDe) + extension;
                        baiViet.HinhAnh = await Utilities.UploadFile(fHinhAnh, @"news", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(baiViet.HinhAnh)) baiViet.HinhAnh = "default.jpg";
                    baiViet.Alias = Utilities.SEOUrl(baiViet.TieuDe);

                    _context.Update(baiViet);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Sửa thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaiVietExists(baiViet.MaBaiViet))
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
            ViewData["MaPage"] = new SelectList(_context.Pages, "MaPage", "TenPage", baiViet.MaPage);
            ViewData["MaTaiKhoan"] = new SelectList(_context.TaiKhoans, "MaTaiKhoan", "TenTaiKhoan", baiViet.MaTaiKhoan);
            return View(baiViet);
        }

        // GET: Admin/AdminBaiViet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BaiViets == null)
            {
                return NotFound();
            }

            var baiViet = await _context.BaiViets
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)
                .FirstOrDefaultAsync(m => m.MaBaiViet == id);
            if (baiViet == null)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        // POST: Admin/AdminBaiViet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BaiViets == null)
            {
                return Problem("Entity set 'dbToursContext.BaiViets'  is null.");
            }
            var baiViet = await _context.BaiViets.FindAsync(id);
            if (baiViet != null)
            {
                _context.BaiViets.Remove(baiViet);
            }
            _notyfService.Success("Xóa thành công");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaiVietExists(int id)
        {
          return _context.BaiViets.Any(e => e.MaBaiViet == id);
        }
    }
}
