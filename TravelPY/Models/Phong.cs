using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class Phong
{
    public int MaPhong { get; set; }

    public int? PhongSo { get; set; }

    public int? MaLoai { get; set; }

    public int? MaKhachSan { get; set; }

    public int? Gia { get; set; }

    public virtual KhachSan? MaKhachSanNavigation { get; set; }

    public virtual LoaiPhong? MaLoaiNavigation { get; set; }
}
