using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class KhachSan
{
    public int MaKhachSan { get; set; }

    public int? MaTour { get; set; }

    public string? TenKhachSan { get; set; }

    public string? DiaChi { get; set; }

    public double? Gia { get; set; }

    public virtual ICollection<DatTour> DatTours { get; } = new List<DatTour>();
}
