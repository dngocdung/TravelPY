using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelPY.Models;
using TravelPY.ModelViews;

namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TaiKhoanController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; }
        public TaiKhoanController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        //[Route("login.html", Name = "Login")]
        public IActionResult AdminLogin(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("MaTaiKhoan");
            if (taikhoanID != null) return RedirectToAction("Index", "Home", new { Area = "Admin" });
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        //[Route("login.html", Name = "Login")]
        public async Task<IActionResult> AdminLogin(DangNhapVM model, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    TaiKhoan kh = _context.TaiKhoans
                    .Include(p => p.MaVaiTroNavigation)
                    .SingleOrDefault(p => p.Email.ToLower() == model.UserName.ToLower().Trim());

                    if (kh == null)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                    }
                    string pass = (model.MatKhau.Trim());
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
                    //luuw seccion Makh
                    HttpContext.Session.SetString("MaTaiKhoan", kh.MaTaiKhoan.ToString());

                    //identity
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kh.TenTaiKhoan),
                        new Claim(ClaimTypes.Email, kh.Email),
                        new Claim("MaTaiKhoan", kh.MaTaiKhoan.ToString()),
                        new Claim("MaVaiTro", kh.MaVaiTro.ToString()),
                        new Claim(ClaimTypes.Role, kh.MaVaiTroNavigation.TenVaiTro)
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);



                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
            }
            catch
            {
                return RedirectToAction("AdminLogin", "TaiKhoan", new { Area = "Admin" });
            }
            return RedirectToAction("AdminLogin", "TaiKhoan", new { Area = "Admin" });
        }
        //[Route("logout.html", Name = "Logout")]
        public IActionResult AdminLogout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("MaTaiKhoan");
                return RedirectToAction("AdminLogin", "TaiKhoan", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("AdminLogin", "TaiKhoan", new { Area = "Admin" });
            }
        }

    }
}
