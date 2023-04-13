using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelPY.Areas.Admin.Models;
using TravelPY.Extension;
using TravelPY.Helpper;
using TravelPY.Models;

namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class AdminTaiKhoanController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfServices { get; set; }
        public AdminTaiKhoanController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfServices= notyfService;
        }

        // GET: Admin/AdminTaiKhoan
        //[AllowAnonymous]
        
        public async Task<IActionResult> Index(int page = 1, int MaDanhMuc = 0)
        {
            ViewData["QuyenTruyCap"] = new SelectList(_context.VaiTros, "MaVaiTro", "MoTa");
            List<SelectListItem> lsTrangThai = new List<SelectListItem>();
            lsTrangThai.Add(new SelectListItem() { Text = "Đang hoạt động", Value = "1" });
            lsTrangThai.Add(new SelectListItem() { Text = "Không hoạt động", Value = "0" });
            ViewData["lsTrangThai"] = lsTrangThai;
            var dbToursContext = _context.TaiKhoans.Include(t => t.MaVaiTroNavigation);
            return View(await dbToursContext.ToListAsync());
        }

        /*[AllowAnonymous]
        //[Route("/login.html", Name = "Login")]
        public IActionResult AdminLogin(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("MaTaiKhoan");
            if (taikhoanID != null) return RedirectToAction("Index", "AdminHome", new { Area = "Admin" });
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        //[Route("/login.html", Name = "Login")]
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
                    //luuw seccion Makh
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

                    *//*if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }*//*

                    return RedirectToAction("Index", "AdminHome", new { Area = "Admin" });
                }
            }
            catch
            {
                return RedirectToAction("Index", "AdminTaiKhoan", new { Area = "Admin" });
            }
            return RedirectToAction("AdminLogin", "AdminTaiKhoan", new { Area = "Admin" });
        }
        [Route("logout.html", Name = "Logout")]
        [AllowAnonymous]
        public IActionResult AdminLogout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("MaTaiKhoan");
                return RedirectToAction("Index", "AdminHome", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("Index", "AdminHome", new { Area = "Admin" });
            }
        }*/

        // GET: Admin/AdminTaiKhoan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TaiKhoans == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.MaVaiTroNavigation)
                .FirstOrDefaultAsync(m => m.MaTaiKhoan == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // GET: Admin/AdminTaiKhoan/Create
        public IActionResult Create()
        {
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro");
            return View();
        }

        // POST: Admin/AdminTaiKhoan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTaiKhoan,TenTaiKhoan,Sdt,Email,TrangThai,MaVaiTro,NgayTao,LastLogin,MatKhau,Salt")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                //string salt = Utilities.GetRandomKey();
                //taiKhoan.Salt = salt;
                taiKhoan.MatKhau = taiKhoan.Sdt;// + salt.Trim()).ToMD5();
                taiKhoan.NgayTao = DateTime.Now;
                _context.Add(taiKhoan);
                await _context.SaveChangesAsync();
                _notyfServices.Success("Tạo mới tài khoản thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro", taiKhoan.MaVaiTro);
            return View(taiKhoan);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taikhoan = _context.TaiKhoans.AsNoTracking().SingleOrDefault(x=>x.Email==model.Email);
                if (taikhoan == null) return RedirectToAction("DangNhap", "TaiKhoan");
                var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
                {
                    string passnew = (model.Password.Trim() + taikhoan.Salt.Trim()).ToMD5();
                    taikhoan.MatKhau = passnew;
                    taikhoan.LastLogin = DateTime.Now;
                    _context.Update(taikhoan);
                    _context.SaveChanges();
                    _notyfServices.Success("Đổi mật khẩu thành công");
                    return RedirectToAction("DangNhap", "TaiKhoan", new {Area = "Admin"});
                }
            }
            return View(model);
        }

        // GET: Admin/AdminTaiKhoan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TaiKhoans == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro", taiKhoan.MaVaiTro);
            return View(taiKhoan);
        }

        // POST: Admin/AdminTaiKhoan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTaiKhoan,TenTaiKhoan,Sdt,Email,TrangThai,MaVaiTro,NgayTao,LastLogin,MatKhau,Salt")] TaiKhoan taiKhoan)
        {
            if (id != taiKhoan.MaTaiKhoan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taiKhoan);
                    await _context.SaveChangesAsync();
                    _notyfServices.Success("Cập nhập thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(taiKhoan.MaTaiKhoan))
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
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro", taiKhoan.MaVaiTro);
            return View(taiKhoan);
        }

        // GET: Admin/AdminTaiKhoan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TaiKhoans == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.MaVaiTroNavigation)
                .FirstOrDefaultAsync(m => m.MaTaiKhoan == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // POST: Admin/AdminTaiKhoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TaiKhoans == null)
            {
                return Problem("Entity set 'DbToursContext.TaiKhoans'  is null.");
            }
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan != null)
            {
                _context.TaiKhoans.Remove(taiKhoan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanExists(int id)
        {
          return _context.TaiKhoans.Any(e => e.MaTaiKhoan == id);
        }
    }
}
