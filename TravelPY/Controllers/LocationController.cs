using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelPY.Models;

namespace TravelPY.Controllers
{
    public class LocationController : Controller
    {
        private readonly DbToursContext _context;

        public LocationController(DbToursContext context)
        {
            _context = context;
        }

        // GET: Location
        public async Task<IActionResult> Index()
        {
              return View(await _context.Locations.ToListAsync());
        }

        public ActionResult QuanHuyenList(int LocationId)
        {
            var QuanHuyens = _context.Locations.OrderBy(x => x.LocationId)
                .Where(x=>x.Parent == LocationId && x.Levels== 2)
                .OrderBy(x=>x.Name).ToList();
            return Json(QuanHuyens);
        }
        public ActionResult PhuongXaList(int LocationId)
        {
            var PhuongXas = _context.Locations.OrderBy(x => x.LocationId)
                .Where(x => x.Parent == LocationId && x.Levels == 3)
                .OrderBy(x => x.Name).ToList();
            return Json(PhuongXas);
        }
        // GET: Location/Details/5
        
    }
}
