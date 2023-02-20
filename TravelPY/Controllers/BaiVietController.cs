using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

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
        [Route("baiviets.html", Name = ("BaiViet"))]
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            var lsBaiViets = _context.BaiViets
                .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)
                .AsNoTracking()
                .OrderByDescending(x => x.NgayTao);
            PagedList<BaiViet> models = new PagedList<BaiViet>(lsBaiViets, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
        [Route("/tin-tuc/{Alias}/{id}.html", Name = "BaiVietChiTiet")]
        public IActionResult Details(int id)
        {
            var baiviet = _context.BaiViets.AsNoTracking().Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation).SingleOrDefault(x => x.MaBaiViet == id);
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
