using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class DanhMuc
{
    public int MaDanhMuc { get; set; }

    public string? TenDanhMuc { get; set; }

    public string? Mota { get; set; }

    public int? SoTour { get; set; }

    public string? Alias { get; set; }

    public string? HinhAnh { get; set; }

    public virtual ICollection<Tour> Tours { get; } = new List<Tour>();
}
