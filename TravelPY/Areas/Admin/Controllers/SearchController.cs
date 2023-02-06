using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPY.Models;
using TravelPY.Models;

namespace Travel.Areas.Admin.Controllers
{
    public class SearchController : Controller
    {
        private readonly DbToursContext _context;

        public SearchController(DbToursContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<Tour> ls = new List<Tour>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListToursSearchPartial", null);
            }
            ls = _context.Tours.AsNoTracking()
                                  .Include(a => a.MaDanhMucNavigation)
                                    .Include(t => t.MaHdvNavigation)
                                  .Where(x => x.TenTour.Contains(keyword))
                                  .OrderByDescending(x => x.TenTour)
                                  .Take(10)
                                  .ToList();
            if (ls == null)
            {
                return PartialView("ListToursSearchPartial", null);
            }
            else
            {
                return PartialView("ListToursSearchPartial", ls);
            }
        }
    }
}
