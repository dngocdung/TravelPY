﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPY.Models;
using TravelPY.ModelViews;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelPY.Controllers
{
    public class SearchController : Controller
    {
        private readonly DbToursContext _context;

        public SearchController(DbToursContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        [HttpPost]
        public IActionResult FindTours(string keyword)
        {
            List<Tour> ls = new List<Tour>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("~/Views/Search/ListToursSearchPartial.cshtml", null);
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
                return PartialView("~/Views/Search/ListToursSearchPartial.cshtml", null);

            }
            else
            {
                return PartialView("~/Views/Search/ListToursSearchPartial.cshtml", ls);
            }
        }

    }
}
