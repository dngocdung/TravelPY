using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System.Diagnostics;
using TravelPY.Models;
using TravelPY.ModelViews;


namespace TravelPY.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbToursContext _context;

        public HomeController(ILogger<HomeController> logger, DbToursContext context)
        {
            _logger = logger;
            _context = context;
        }
        // Trang chủ
        //[Route("/trang-chu.html", Name = "Home")]
        public IActionResult Index()
        {
            HomeViewVM model = new HomeViewVM();
            //Danh sách Tour đang hoạt động
            var lsTours = _context.Tours.AsNoTracking().Include(x => x.MaDanhMucNavigation)
                .Where(x => x.TrangThai == true)
                .OrderByDescending(x => x.MaTour)
                .ToList();

            List<TourHomeVM> lsTourViews = new List<TourHomeVM>();
            var lsCats = _context.DanhMucs
                .AsNoTracking()
                //.Where(x => x.Published == true)
                .OrderByDescending(x => x.TenDanhMuc)
                .ToList();

            foreach (var item in lsCats)
            {
                TourHomeVM TourHome = new TourHomeVM();
                TourHome.danhmuc = item;
                TourHome.lsTours = lsTours.Where(x => x.MaDanhMuc == item.MaDanhMuc).ToList();
                lsTourViews.Add(TourHome);



                var TinTuc = _context.BaiViets
                    .Include(b => b.MaPageNavigation)
                .Include(b => b.MaTaiKhoanNavigation)
                    .AsNoTracking()

                    .Where(x => x.Publisher == true)
                    .OrderByDescending(x => x.NgayTao)
                    .Take(3)
                    .ToList();
                model.Tours = lsTourViews;

                model.BaiViets = TinTuc;
                ViewBag.AllTours = lsTours;
            }
            return View(model);
        }



        [Route("lien-he.html", Name = "Contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("gioi-thieu.html", Name = "About")]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}