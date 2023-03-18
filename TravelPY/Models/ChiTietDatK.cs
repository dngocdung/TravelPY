using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class ChiTietDatK
{
    public int MaChiTietKs { get; set; }

    public int? MaDatKs { get; set; }

    public int? MaKhachSan { get; set; }

    public DateTime? Ngay { get; set; }

    public int? Gia { get; set; }
}
