using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using TravelPY.Models;

namespace TravelPY.Controllers
{
    public class KhachSanController : Controller
    {
        private readonly DbToursContext _context;

        public KhachSanController(DbToursContext context)
        {
            _context = context;
        }

        // GET: KhachSan
        [Route("khach-san.html", Name = ("KhachSan"))]
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 9;
            var lsKhachSans = _context.KhachSans
                
                .AsNoTracking()
                .OrderByDescending(x => x.MaKhachSan);
            PagedList<KhachSan> models = new PagedList<KhachSan>(lsKhachSans, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: KhachSan/Details/5
        [Route("/khach-san/{Alias}/{id}.html", Name = ("ChiTietKhachSan"))]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var khachsan = _context.KhachSans
                    .FirstOrDefault(x => x.MaKhachSan == id);
                if (khachsan == null)
                {
                    return RedirectToAction("Index");
                }
                var lsKhachSan = _context.KhachSans
                    .AsNoTracking()
                    
                    .Take(3)
                    .OrderByDescending(x => x.MaKhachSan)
                    .ToList();
                ViewBag.lsKhachSan = lsKhachSan;
                return View(khachsan);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        
        
    }
}
