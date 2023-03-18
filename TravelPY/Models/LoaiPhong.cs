using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class LoaiPhong
{
    public int MaLoai { get; set; }

    public string? LoaiPhong1 { get; set; }

    public int? Gia { get; set; }

    public int? MaKhachSan { get; set; }

    public virtual KhachSan? MaKhachSanNavigation { get; set; }

    public virtual ICollection<Phong> Phongs { get; } = new List<Phong>();
}
