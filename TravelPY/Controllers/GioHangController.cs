using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

using TravelPY.ModelViews;
using TravelPY.Extension;
using TravelPY.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TravelPY.Controllers
{
    public class GioHangController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; }
        public GioHangController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        //Tao giỏ hàng rỗng
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
        //Thêm mới vào giỏ hàng
        [HttpPost]
        [Route("api/cart/add")]
        //[Authorize]
        public IActionResult AddToCart(int productID, int? amount)
        {
            /*if (!User.Identity.IsAuthenticated) Response.Redirect("/dang-nhap.html");
            var taikhoanID = HttpContext.Session.GetString("MaKhachHang");
            if (taikhoanID == null) return RedirectToAction("DangNhap", "Account", new { Area = "" });

            var account = _context.KhachHangs.AsNoTracking().FirstOrDefault(x => x.MaKhachHang == int.Parse(taikhoanID));
            if (account == null) return NotFound();*/
            List<CartItem> cart = GioHang;

            try
            {
                //Them tour vao gio hang
                CartItem item = cart.SingleOrDefault(p => p.product.MaTour == productID);
                if (item != null) // da co => cap nhat so nguoi di
                {
                    item.amount = item.amount + amount.Value;
                    //luu lai session
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                }
                else
                {
                    Tour hh = _context.Tours.SingleOrDefault(p => p.MaTour == productID);
                    
                    item = new CartItem
                    {
                        amount = amount.HasValue ? amount.Value : 1,
                        product = hh,
                        
                    };
                    cart.Add(item);//Them vao gio
                }

                //Luu lai Session
                HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                _notyfService.Success("Thêm tour thành công");
                
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }


        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(int productID, int? amount)
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            try
            {
                if (cart != null)
                {
                    CartItem item = cart.SingleOrDefault(p => p.product.MaTour == productID);
                    if (item != null && amount.HasValue) // da co -> cap nhat so luong
                    {
                        item.amount = amount.Value;
                    }
                    //Luu lai session
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/remove")]
        public ActionResult Remove(int productID)
        {

            try
            {
                List<CartItem> gioHang = GioHang;
                CartItem item = gioHang.SingleOrDefault(p => p.product.MaTour == productID);
                if (item != null)
                {
                    gioHang.Remove(item);
                }
                //luu lai session
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            return View(GioHang);
        }
    }
}
