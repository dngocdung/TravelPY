using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using TravelPY.Helpper;
using TravelPY.Models;

namespace TravelPY.Controllers
{
    public class TourController : Controller
    {
        private readonly DbToursContext _context;

        public TourController(DbToursContext context)
        {
            _context = context;
        }

        // GET: Tour
        [Route("/1-ngay.html", Name = ("Tour1N"))]
        public IActionResult Tour1N(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = Utilities.PAGE_SIZE;
                var lsTours = _context.Tours
                    .Include(t => t.MaDanhMucNavigation)
                .Include(t => t.MaHdvNavigation)
                    .AsNoTracking()
                    .Where(t=>t.MaDanhMuc==1)
                    .OrderByDescending(x => x.MaTour);
                PagedList<Tour> models = new PagedList<Tour>(lsTours, pageNumber, pageSize);

                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }


        }
        [Route("/dai-ngay.html", Name = ("TourDN"))]
        public IActionResult TourDN(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = Utilities.PAGE_SIZE;
                var lsTours = _context.Tours
                    .Include(t => t.MaDanhMucNavigation)
                .Include(t => t.MaHdvNavigation)
                    .AsNoTracking()
                    .Where(t => t.MaDanhMuc == 2)
                    .OrderByDescending(x => x.MaTour);
                PagedList<Tour> models = new PagedList<Tour>(lsTours, pageNumber, pageSize);

                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }


        }
        /*[Route("danhmuc/{Alias}", Name = ("DanhSachTour"))]
        public IActionResult List(string Alias, int page = 1)
        {
            try
            {
                var pageSize = 10;
                var danhmuc = _context.DanhMucs
                    .AsNoTracking().SingleOrDefault(x => x.Alias == Alias);

                var lsTours = _context.Tours
                    .Include(t => t.MaDanhMucNavigation)
                .Include(t => t.MaHdvNavigation)
                    .AsNoTracking()
                    .Where(x => x.MaDanhMuc == danhmuc.MaDanhMuc)
                    .OrderByDescending(x => x.MaTour);
                PagedList<Tour> models = new PagedList<Tour>(lsTours, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = danhmuc;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }


        }*/
        // GET: Tour/Details/5
        [Route("/tour/{Alias}/{id}.html", Name = ("ChiTietTour"))]
        public IActionResult Details(int id)
        {
            try
            {
                var tour = _context.Tours
                    .Include(t => t.MaDanhMucNavigation)
                .Include(t => t.MaHdvNavigation).FirstOrDefault(x => x.MaTour == id);
                if (tour == null)
                {
                    return RedirectToAction("Index");
                }
                var lsTour = _context.Tours.Include(t => t.MaDanhMucNavigation)
                .Include(t => t.MaHdvNavigation)
                    .AsNoTracking()
                    .Where(x => x.MaDanhMuc == tour.MaDanhMuc && x.MaTour != id && x.TrangThai == true)
                    .Take(3)
                    .OrderByDescending(x => x.MaTour)
                    .ToList();
                ViewBag.lsTour = lsTour;
                return View(tour);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }


        }



    }
}
