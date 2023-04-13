using AspNetCoreHero.ToastNotification.Abstractions;
using System;
using System.Collections.Generic;
using TravelPY.Models;

namespace TravelPY.ModelViews
{
    public class TourHomeVM
    {
        
        public DanhMuc danhmuc { get; set; }
        public List<Tour> lsTours { get; set; }
        



    }
}
