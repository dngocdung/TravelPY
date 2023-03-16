using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelPY.Extension;
using TravelPY.Helpper;
using TravelPY.Models;
using TravelPY.ModelViews;


namespace TravelPY.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; }
        public CheckoutController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }
        [HttpGet]
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("MaKhachHang"); //Check tai khoan da login chua
            DatTourVM model = new DatTourVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.KhachHangs.AsNoTracking().Include(x=>x.Location).SingleOrDefault(x => x.MaKhachHang == Convert.ToInt32(taikhoanID));
                model.MaKhachHang = khachhang.MaKhachHang;
                model.TenKhachHang = khachhang.TenKhachHang;
                model.Email = khachhang.Email;
                model.SDT = khachhang.Sdt;
                model.DiaChi = khachhang.DiaChi;
                
            }
            ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "LocationId", "Name");
            ViewBag.GioHang = cart;
            return View(model);
        }

        [HttpPost]
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index(DatTourVM muaHang)
        {
            //Lay ra gio hang de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("MaKhachHang");
            DatTourVM model = new DatTourVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.KhachHangs.AsNoTracking().Include(x=>x.Location).SingleOrDefault(x => x.MaKhachHang == Convert.ToInt32(taikhoanID));
                khachhang.LocationId = muaHang.TinhThanh;
                khachhang.QuanHuyen = muaHang.QuanHuyen;
                khachhang.PhuongXa = muaHang.PhuongXa;
                //khachhang.DiaChi = muaHang.DiaChi;
                
                
                

                model.MaKhachHang = khachhang.MaKhachHang;
                model.TenKhachHang = khachhang.TenKhachHang;
                model.Email = khachhang.Email;
                model.SDT = khachhang.Sdt;
                model.DiaChi = khachhang.DiaChi;

                

                
                _context.Update(khachhang);
                _context.SaveChanges();
            }
            try
            {
                if (!ModelState.IsValid)
                {
                    //Khoi tao don hang
                    DatTour donhang = new DatTour();
                    donhang.MaKhachHang = model.MaKhachHang;
                    
                    donhang.DiaChi = model.DiaChi;
                    donhang.LocationId = model.TinhThanh;
                    donhang.QuanHuyen = model.QuanHuyen;
                    donhang.PhuongXa = model.PhuongXa;
                    

                    donhang.NgayDatTour = DateTime.Now;
                    donhang.MaTrangThai = 1;//Don hang moi
                    donhang.Deleted = false;
                    donhang.Paid = false;
                    
                    //donhang.GhiChu = Utilities.StripHTML(model.Note);
                    donhang.GhiChu = Utilities.StripHTML(muaHang.Note);
                    donhang.TongTien = Convert.ToInt32(cart.Sum(x => x.TotalMoney));
                    _context.Add(donhang);
                    _context.SaveChanges();
                    //tao danh sach don hang

                    foreach (var item in cart)
                    {
                        ChiTietDatTour orderDetail = new ChiTietDatTour();
                        orderDetail.MaDatTour = donhang.MaDatTour;
                        orderDetail.MaTour = item.product.MaTour;
                        
                        orderDetail.ThanhToan = donhang.TongTien;
                        orderDetail.GiaGiam = item.product.GiaGiam;
                        orderDetail.NgayTao = DateTime.Now;
                        _context.Add(orderDetail);
                    }
                    _context.SaveChanges();
                    //clear gio hang
                    HttpContext.Session.Remove("GioHang");
                    //Xuat thong bao
                    _notyfService.Success("Đơn hàng đặt thành công");
                    //cap nhat thong tin khach hang
                    return RedirectToAction("Success");


                }
            }
            catch
            {
                ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "LocationId", "Name");
                ViewBag.GioHang = cart;
                return View(model);
            }
            ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "LocationId", "Name");
            ViewBag.GioHang = cart;
            return View(model);
        }

        [Route("datks.html", Name = "DatKhachSan")]
        public IActionResult DatKhachSan(string returnUrl = null) 
        {
            /*var taikhoanID = HttpContext.Session.GetString("MaKhachHang"); //Check tai khoan da login chua
            DatKhachSanVM model = new DatKhachSanVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.KhachHangs.AsNoTracking().Include(x => x.Location).SingleOrDefault(x => x.MaKhachHang == Convert.ToInt32(taikhoanID));
                model.MaKhachHang = khachhang.MaKhachHang;
                model.TenKhachHang = khachhang.TenKhachHang;
                model.Email = khachhang.Email;
                model.SDT = khachhang.Sdt;
                //model.DiaChi = khachhang.DiaChi;

            }
            //ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "LocationId", "Name");
            //ViewBag.GioHang = cart;
            return View(model);*/
            return View(); 
        }

        public KhachSan ks { get; set; }
        [HttpPost]
        [Route("datks.html", Name = "DatKhachSan")]
        public IActionResult DatKhachSan(DatTourVM khachsan)
        {
            //var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("MaKhachHang");
            DatKhachSanVM model = new DatKhachSanVM();

            if (taikhoanID != null)
            {
                var khachhang = _context.KhachHangs.AsNoTracking().Include(x => x.Location).SingleOrDefault(x => x.MaKhachHang == Convert.ToInt32(taikhoanID));
                
                //khachhang.DiaChi = muaHang.DiaChi;




                model.MaKhachHang = khachhang.MaKhachHang;
                model.TenKhachHang = khachhang.TenKhachHang;
                model.Email = khachhang.Email;
                model.SDT = khachhang.Sdt;
                //model.DiaChi = khachhang.DiaChi;




                _context.Update(khachhang);
                _context.SaveChanges();
            }
            try
            {
                if (!ModelState.IsValid)
                {
                    //Khoi tao don hang
                    DatKhachSan donhang = new DatKhachSan();
                    donhang.MaKhachHang = model.MaKhachHang;

                    //donhang.DiaChi = model.DiaChi;
                    


                    donhang.NgayDat = DateTime.Now;
                    donhang.NgayDi = model.NgayDen;//Don hang moi
                    donhang.NgayDi = model.NgayDi;
                    //donhang.Paid = false;

                    //donhang.GhiChu = Utilities.StripHTML(model.Note);
                    //donhang.GhiChu = Utilities.StripHTML(khachsan.Note);
                    //donhang.TongTien = Convert.ToInt32(cart.Sum(x => x.TotalMoney));
                    _context.Add(donhang);
                    _context.SaveChanges();
                    //tao danh sach don hang



                    ChiTietDatK orderDetail = new ChiTietDatK();
                    orderDetail.MaDatKs = donhang.MaDatKs;
                    orderDetail.MaKhachSan = ks.MaKhachSan;

                    //orderDetail.ThanhToan = donhang.TongTien;
                    //orderDetail.Gia = ks.Gia;
                    orderDetail.Ngay = DateTime.Now;
                    _context.Add(orderDetail);


                    _context.SaveChanges();
                    //clear gio hang
                    //HttpContext.Session.Remove("GioHang");
                    //Xuat thong bao
                    _notyfService.Success("Đơn hàng đặt thành công");
                    //cap nhat thong tin khach hang
                    return RedirectToAction("Success");


                }
            }
            catch
            {
                
                return View(model);
            }
            
            return View(model);
            
        }

        [Route("dat-hang-thanh-cong.html", Name = "Success")]
        public IActionResult Success()
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("MaKhachHang");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    return RedirectToAction("DangNhap", "Account", new { returnUrl = "/dat-hang-thanh-cong.html" });
                }
                var khachhang = _context.KhachHangs.AsNoTracking().SingleOrDefault(x => x.MaKhachHang == Convert.ToInt32(taikhoanID));
                var donhang = _context.DatTours
                    .Where(x => x.MaKhachHang == Convert.ToInt32(taikhoanID))
                    .OrderByDescending(x => x.NgayDatTour)
                    .FirstOrDefault();
                MuaHangSuccessVM successVM = new MuaHangSuccessVM();
                successVM.FullName = khachhang.TenKhachHang;
                successVM.DonHangID = donhang.MaDatTour;
                successVM.Phone = khachhang.Sdt;
                successVM.Address = khachhang.DiaChi;
                successVM.PhuongXa = GetNameLocation(donhang.PhuongXa.Value);
                successVM.QuanHuyen = GetNameLocation(donhang.QuanHuyen.Value);

                successVM.TinhThanh = GetNameLocation(donhang.LocationId.Value);
                return View(successVM);
            }
            catch
            {
                return View();
            }
        }
        public string GetNameLocation(int idlocation)
        {
            try
            {
                var location = _context.Locations.AsNoTracking().SingleOrDefault(x => x.LocationId == idlocation);
                if (location != null)
                {
                    return location.NameWithType;
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }
    }
}
