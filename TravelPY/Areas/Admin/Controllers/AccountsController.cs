using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelPY.Areas.Admin.Models;
using TravelPY.Models;

namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; }
        public AccountsController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("/login.html", Name = "Login")]
        public IActionResult AdminLogin(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("MaTaiKhoan");
            if (taikhoanID != null) return RedirectToAction("Index", "AdminHome", new { Area = "Admin" });
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("/login.html", Name = "Login")]
        public async Task<IActionResult> AdminLogin(DangNhapViewModel model, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {


                    TaiKhoan kh = _context.TaiKhoans
                    .Include(p => p.MaVaiTroNavigation)
                    .SingleOrDefault(p => p.Email.ToLower() == model.UserName.ToLower().Trim());

                    if (kh == null)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        return View(model);
                    }
                    string pass = (model.Password.Trim());
                    // + kh.Salt.Trim()
                    if (kh.MatKhau.Trim() != pass)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        return View(model);
                    }
                    //đăng nhập thành công

                    //ghi nhận thời gian đăng nhập
                    kh.LastLogin = DateTime.Now;
                    _context.Update(kh);
                    await _context.SaveChangesAsync();


                    var taikhoanID = HttpContext.Session.GetString("MaTaiKhoan");
                    //identity
                    //luuw secsion Makh
                    HttpContext.Session.SetString("MaTaiKhoan", kh.MaTaiKhoan.ToString());

                    //identity
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kh.TenTaiKhoan),
                        new Claim(ClaimTypes.Email, kh.Email),
                        new Claim("MaTaiKhoan", kh.MaTaiKhoan.ToString()),
                        new Claim("MaVaiTro", kh.MaVaiTro.ToString()),
                        new Claim(ClaimTypes.Role, kh.MaVaiTroNavigation.TenVaiTro) //Add TenVaiTro de phan quyen
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "AdminHome", new { Area = "Admin" });
                }
            }
            catch
            {
                return RedirectToAction("AdminLogin", "Accounts", new { Area = "Admin" });
            }
            return RedirectToAction("AdminLogin", "Accounts", new { Area = "Admin" });
        }
        [Route("logout.html", Name = "Logout")]
        [AllowAnonymous]
        public IActionResult AdminLogout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("MaTaiKhoan");
                return RedirectToAction("AdminLogin", "Accounts", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("AdminLogin", "Accounts", new { Area = "Admin" });
                /*return RedirectToAction("Index", "AdminHome", new { Area = "Admin" });*/
            }
        }
    }
}
