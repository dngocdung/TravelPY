using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TravelPY.Models;
using TravelPY.ModelViews;

namespace TravelPY.ModelViews
{
    public class HomeViewVM
    {
        private readonly DbToursContext _context;

        public HomeViewVM(DbToursContext context)
        {
            _context = context;

        }
        public List<BaiViet> BaiViets { get; set; }
        public List<TourHomeVM> Tours { get; set; }
        public List<DanhMuc> DanhMucs { get; set; }
        public List<Tour> DanhSachTheoLoai(int? maDanhMuc)
        {
            var lsTours = _context.Tours
                    .Include(t => t.MaDanhMucNavigation)
                .Include(t => t.MaHdvNavigation)
                    .AsNoTracking()
                    .Where(t => t.MaDanhMuc == maDanhMuc)
                    .OrderByDescending(x => x.MaTour)
                    .ToList();
            return lsTours;
        }
    }
}
