using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SolrNet.Utils;
using TravelPY.Models;


namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly DbToursContext _context;

        public SearchController(DbToursContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult FindTour(string keyword)
        {
            List<Tour> ls = new List<Tour>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("~/Areas/Admin/Views/Search/ListToursSearchPartial.cshtml", null);
            }
            ls = _context.Tours.AsNoTracking()
                                  .Include(a => a.MaDanhMucNavigation)
                                  .Include(t => t.MaHdvNavigation)
                                  .Where(x => x.TenTour.Contains(keyword) || x.MaHdvNavigation.TenHdv.Contains(keyword))
                                  
                                  .OrderByDescending(x => x.TenTour)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("~/Areas/Admin/Views/Search/ListToursSearchPartial.cshtml", null);
                
            }
            else
            {
                return PartialView("~/Areas/Admin/Views/Search/ListToursSearchPartial.cshtml", ls);
            }
        }
        [HttpPost]
        public IActionResult FindBaiViet(string keyword)
        {
            List<BaiViet> ls = new List<BaiViet>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("~/Areas/Admin/Views/Search/ListBaiVietsSearchPartial.cshtml", null);
            }
            ls = _context.BaiViets.AsNoTracking()
                                  .Include(a => a.MaTaiKhoanNavigation)
                                  .Include(t => t.MaPageNavigation)
                                  .Where(x => x.TieuDe.Contains(keyword))
                                  .OrderByDescending(x => x.TieuDe)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("~/Areas/Admin/Views/Search/ListBaiVietsSearchPartial.cshtml", null);

            }
            else
            {
                return PartialView("~/Areas/Admin/Views/Search/ListBaiVietsSearchPartial.cshtml", ls);
            }
        }
    }
}
