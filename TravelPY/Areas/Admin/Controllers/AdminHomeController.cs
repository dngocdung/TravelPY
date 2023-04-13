using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPY.Models;

namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize()]
    public class AdminHomeController : Controller
    {
        private readonly DbToursContext _context;

        public AdminHomeController(DbToursContext context)
        {
            _context = context;
        }

        //[AllowAnonymous]
        //[Route("admin.html", Name = "Home")]
        //[Authorize()]
        public IActionResult Index()
        {
            var soKhachHangs = _context.KhachHangs.Count();
            var soTours = _context.Tours.Count();
            var soDatTours = _context.DatTours.Count();
            ViewBag.soKhachHang = soKhachHangs;
            ViewBag.soTour = soTours;
            ViewBag.soDatTour = soDatTours;
            return View();
        }
    }
}
