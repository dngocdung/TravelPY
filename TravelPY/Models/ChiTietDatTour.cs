using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class ChiTietDatTour
{
    public int MaChiTiet { get; set; }

    public int? MaDatTour { get; set; }

    public int? MaTour { get; set; }

    public int? Gia { get; set; }

    public int? GiaGiam { get; set; }

    public int? ThanhToan { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual DatTour? MaDatTourNavigation { get; set; }

    public virtual Tour? MaTourNavigation { get; set; }
}
