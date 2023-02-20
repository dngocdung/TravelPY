using System;
using System.Collections.Generic;
using TravelPY.Models;

namespace TravelPY.ModelViews
{
    public class XemDonHang
    {
        public DatTour DonHang { get; set; }
        public List<ChiTietDatTour> ChiTietDonHang { get; set; }
    }
}
