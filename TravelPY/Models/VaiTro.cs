using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class VaiTro
{
    public int MaVaiTro { get; set; }

    public string? TenVaiTro { get; set; }

    public string? MoTa { get; set; }

    public virtual ICollection<TaiKhoan> TaiKhoans { get; } = new List<TaiKhoan>();
}
