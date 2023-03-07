using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPY.Models;
using TravelPY.ModelViews;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelPY.Controllers
{
    public class DonHangController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; }
        public DonHangController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        //[HttpPost]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var taikhoanID = HttpContext.Session.GetString("MaKhachHang");
                if (string.IsNullOrEmpty(taikhoanID)) return RedirectToAction("DangNhap", "Account");
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.MaKhachHang == Convert.ToInt32(taikhoanID));
                if (khachhang == null) return NotFound();
                var donhang = await _context.DatTours
                    .Include(x => x.MaTrangThaiNavigation)
                    .Include(x => x.MaKhachHangNavigation)

                    .FirstOrDefaultAsync(m => m.MaDatTour == id && Convert.ToInt32(taikhoanID) == m.MaKhachHang);
                if (donhang == null) return NotFound();

                var chitietdonhang = _context.ChiTietDatTours
                    .Include(x => x.MaTourNavigation)
                    .Include(x => x.MaDatTourNavigation)
                    .Include(x => x.MaTourNavigation.MaHdvNavigation)
                    .AsNoTracking()
                    .Where(x => x.MaDatTour == id)
                    .OrderBy(x => x.MaChiTiet)
                    .ToList();
                XemDonHang donHang = new XemDonHang();
                donHang.DonHang = donhang;
                donHang.ChiTietDonHang = chitietdonhang;
                return PartialView("Details", donHang);
                
            }
            catch 
            {
                return NotFound();
            }
        }
    }
}
