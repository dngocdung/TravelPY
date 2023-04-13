using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TravelPY.Models;
using TravelPY.ModelViews;

namespace TravelPY.ModelViews
{
    public class HomeViewVM
    {
        
        public List<BaiViet> BaiViets { get; set; }
        public List<TourHomeVM> Tours { get; set; }
        //public List<DanhMuc> DanhMucs { get; set; }
        
    }
}
