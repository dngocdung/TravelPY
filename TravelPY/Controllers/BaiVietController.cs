using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using TravelPY.Helpper;
using TravelPY.Models;

namespace TravelPY.Controllers
{
    public class BaiVietController : Controller
    {
        private readonly DbToursContext _context;
        public BaiVietController(DbToursContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        [Route("kinh-nghiem-du-lich.html", Name = ("BaiViet"))]
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 9;
            var lsBaiViets = _context.BaiViets
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)
                
                .Where(b => b.Publisher == true)
                .AsNoTracking()
                .OrderByDescending(x => x.NgayTao);
            PagedList<BaiViet> models = new PagedList<BaiViet>(lsBaiViets, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [Route("/an-uong.html", Name = ("AnUong"))]
        public IActionResult AnUong(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 9;
            var lsPage = _context.Pages.AsNoTracking();
            var lsBaiViets = _context.BaiViets
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)

                .Where(b => b.Publisher == true && b.MaPageNavigation.MaPage == 1)
                .AsNoTracking()
                .OrderByDescending(x => x.NgayTao);
            PagedList<BaiViet> models = new PagedList<BaiViet>(lsBaiViets, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [Route("/dia-diem.html", Name = ("DiaDiem"))]
        public IActionResult DiaDiem(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 9;
            var lsBaiViets = _context.BaiViets
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)

                .Where(b => b.Publisher == true && b.MaPageNavigation.MaPage == 2)
                .AsNoTracking()
                .OrderByDescending(x => x.NgayTao);
            PagedList<BaiViet> models = new PagedList<BaiViet>(lsBaiViets, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [Route("/khach-san.html", Name = ("BaiVietKhachSan"))]
        public IActionResult KhachSan(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 9;
            var lsBaiViets = _context.BaiViets
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)

                .Where(b => b.Publisher == true && b.MaPageNavigation.MaPage == 3)
                .AsNoTracking()
                .OrderByDescending(x => x.NgayTao);
            PagedList<BaiViet> models = new PagedList<BaiViet>(lsBaiViets, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        [Route("/{Alias}/{id}.html", Name = "BaiVietChiTiet")]
        public IActionResult Details(int id)
        {
            var baiviet = _context.BaiViets.AsNoTracking().Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)
                
                .SingleOrDefault(x => x.MaBaiViet == id);
            if (baiviet == null)
            {
                return RedirectToAction("Index");
            }
            var lsBaivietlienquan = _context.BaiViets.Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)
                .AsNoTracking()
                .Where(x => x.Publisher == true && x.MaBaiViet != id)

                .Take(3)
                .OrderByDescending(x => x.NgayTao).ToList();
            ViewBag.Baivietlienquan = lsBaivietlienquan;
            return View(baiviet);
        }
    }
}
