using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class Page
{
    public int MaPage { get; set; }

    public string? TenPage { get; set; }

    public string? NoiDung { get; set; }

    public string? HinhAnh { get; set; }

    public int? SoBaiViet { get; set; }

    public string? Alias { get; set; }

    public virtual ICollection<BaiViet> BaiViets { get; } = new List<BaiViet>();
}
