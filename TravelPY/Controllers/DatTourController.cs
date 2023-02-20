using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelPY.ModelViews;

using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using TravelPY.Helpper;
using TravelPY.Models;

namespace TravelPY.Controllers
{
    public class DatTourController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; }
        public DatTourController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("dat-tour.html", Name = "DatTour")]
        public IActionResult DatTour()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dat-tour.html", Name = "DatTour")]
        public async Task<IActionResult> DatTour(DatTourVM taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    DatTour khachhang = new DatTour
                    {
                        /* MaKhachHangNavigation = taikhoan.TenKhachHang,
                         Email = taikhoan.Email,
                         Sdt = taikhoan.SDT, 
                         DiaChi = taikhoan.DiaChi*/

                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        //Lưu Session MaKh
                        HttpContext.Session.SetString("MaKhachHang", khachhang.MaKhachHang.ToString());
                        var taikhoanID = HttpContext.Session.GetString("MaKhachHang");

                        //Identity
                        var claims = new List<Claim>
                        {
                            //new Claim(ClaimTypes.Name,khachhang.TenKhachHang),
                            new Claim("MaKhachHang", khachhang.MaKhachHang.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "dattour");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đặt Tour thành công");
                        return RedirectToAction("Dashboard", "Account");
                    }
                    catch
                    {
                        return RedirectToAction("DangkyTaiKhoan", "Account");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }
    }
}
