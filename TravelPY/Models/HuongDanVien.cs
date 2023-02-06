using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class HuongDanVien
{
    public int MaHdv { get; set; }

    public string? TenHdv { get; set; }

    public DateTime? NgaySinhHdv { get; set; }

    public string? DiaChiHdv { get; set; }

    public string? Sdt { get; set; }

    public string? HinhAnh { get; set; }

    public virtual ICollection<Tour> Tours { get; } = new List<Tour>();
}
