using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelPY.Extension;
using TravelPY.Helpper;
using TravelPY.Models;
using TravelPY.ModelViews;

namespace TravelPY.Controllers
{

    public class AccountController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; }
        public AccountController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Sdt)
        {
            try
            {
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.Sdt.ToLower() == Sdt.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện thoại : " + Sdt + "đã được sử dụng");

                return Json(data: true);

            }
            catch
            {
                return Json(data: true);
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: "Email : " + Email + " đã được sử dụng");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("MaKhachHang");
            if (taikhoanID != null)
            {
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.MaKhachHang == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    var lsDonHang = _context.DatTours  
                        .Include(x=>x.MaTrangThaiNavigation)
                        .Include(x=>x.MaKhachHangNavigation)
                        .AsNoTracking()
                        .Where(x => x.MaKhachHang == khachhang.MaKhachHang)
                        .OrderByDescending(x => x.NgayDatTour)
                        .ToList();
                    ViewBag.DonHang = lsDonHang;
                    return View(khachhang);
                }

            }
            return RedirectToAction("DangNhap");
        }
        //[Route("cap-nhap-thong-tin.html", Name = "Update")]
        /*public IActionResult Update(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("MaKhachHang"); //Check tai khoan da login chua
            DatTourVM model = new DatTourVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.KhachHangs.AsNoTracking().Include(x => x.Location).SingleOrDefault(x => x.MaKhachHang == Convert.ToInt32(taikhoanID));
                model.MaKhachHang = khachhang.MaKhachHang;
                model.TenKhachHang = khachhang.TenKhachHang;
                model.Email = khachhang.Email;
                model.SDT = khachhang.Sdt;
                model.DiaChi = khachhang.DiaChi;

            }
            ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "LocationId", "Name");
            //ViewBag.GioHang = cart;
            return View(model);
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("cap-nhap-thong-tin.html", Name = "Update")]
        public IActionResult Update(DatTourVM muaHang)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("MaKhachHang");
            DatTourVM model = new DatTourVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.KhachHangs.AsNoTracking().Include(x => x.Location).SingleOrDefault(x => x.MaKhachHang == Convert.ToInt32(taikhoanID));
                
                khachhang.DiaChi = muaHang.DiaChi;
                khachhang.TenKhachHang = muaHang.TenKhachHang;
                khachhang.Email = muaHang.Email;
                khachhang.Sdt = muaHang.SDT;
                khachhang.NgaySinh = muaHang.NgaySinh;

                /*model.MaKhachHang = khachhang.MaKhachHang;
                model.TenKhachHang = khachhang.TenKhachHang;
                model.Email = khachhang.Email;
                model.SDT = khachhang.Sdt;
                model.DiaChi = khachhang.DiaChi;*/
                _context.Update(khachhang);
                _context.SaveChanges();
                return RedirectToAction("Dashboard","Account");
            }
            //ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "Name", khachHang.LocationId);
            return View(model);
        }
        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs.Any(e => e.MaKhachHang == id);
        }

        /*[Route("thong-tin-don-hang.html", Name = "ThongTin")]
        public IActionResult TourPartialView()
        { return PartialView("_TourPartialView"); }*/

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult DangkyTaiKhoan()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> DangkyTaiKhoan(DangKyVM taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    KhachHang khachhang = new KhachHang
                    {
                        TenKhachHang = taikhoan.TenKhachHang,
                        Sdt = taikhoan.Sdt.Trim().ToLower(),
                        Email = taikhoan.Email.Trim().ToLower(),
                        MatKhau = (taikhoan.Password + salt.Trim()).ToMD5(),
                        Salt = salt,
                        NgayTao = DateTime.Now
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
                            new Claim(ClaimTypes.Name,khachhang.TenKhachHang),
                            new Claim("MaKhachHang", khachhang.MaKhachHang.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "dangnhap");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        _notyfService.Success("Đăng ký thành công");
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



        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public IActionResult DangNhap(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("MaKhachHang");
            if (taikhoanID != null)
            {
                return RedirectToAction("Dashboard", "Account");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> DangNhap(DangNhapVM customer, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    if (!isEmail) return View(customer);

                    var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == customer.UserName);

                    if (khachhang == null) return RedirectToAction("DangkyTaiKhoan");
                    string pass = (customer.MatKhau + khachhang.Salt.Trim()).ToMD5();
                    if (khachhang.MatKhau != pass)
                    {
                        _notyfService.Warning("Thông tin đăng nhập chưa chính xác");
                        return View(customer);
                    }
                    //kiem tra xem account co bi disable hay khong

                    /*if (khachhang.Active == false)
                    {
                        return RedirectToAction("ThongBao", "Account");
                    }*/

                    //Luu Session MaKh
                    HttpContext.Session.SetString("MaKhachHang", khachhang.MaKhachHang.ToString());
                    var taikhoanID = HttpContext.Session.GetString("MaKhachHang");

                    //Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.TenKhachHang),
                        new Claim("MaKhachHang", khachhang.MaKhachHang.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "dangnhap");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    _notyfService.Success("Đăng nhập thành công");
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Dashboard", "Account");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("DangkyTaiKhoan", "Account");
            }
            return View(customer);
        }
        [HttpGet]
        [Route("dang-xuat.html", Name = "DangXuat")]
        public IActionResult DangXuat()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("MaKhachHang");
            return RedirectToAction("Index", "Home");
        }

        [Route("/doi-mat-khau.html", Name = "ChangePassword")]
        public IActionResult ChangePassword()
        {
            return PartialView("_ChangePasswordPartialView");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/doi-mat-khau.html", Name = "ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordVM model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("MaKhachHang");
                if (taikhoanID == null)
                {
                    return RedirectToAction("DangNhap", "Account");
                }
                if (ModelState.IsValid)
                {
                    var taikhoan = _context.KhachHangs.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null) return RedirectToAction("DangNhap", "Account");
                    var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
                    {
                        string passnew = (model.Password.Trim() + taikhoan.Salt.Trim()).ToMD5();
                        taikhoan.MatKhau = passnew;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        _notyfService.Success("Đổi mật khẩu thành công");
                        return RedirectToAction("Dashboard", "Account");
                    }
                }
            }
            catch
            {
                _notyfService.Success("Thay đổi mật khẩu không thành công");
                return RedirectToAction("Dashboard", "Account");
            }
            _notyfService.Success("Thay đổi mật khẩu không thành công");
            return RedirectToAction("Dashboard", "Account");
        }

        
        
    }
}
