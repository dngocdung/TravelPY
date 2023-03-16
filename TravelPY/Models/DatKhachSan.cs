using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class DatKhachSan
{
    public int MaDatKs { get; set; }

    public int? MaKhachHang { get; set; }

    public int? MaKhachSan { get; set; }

    public int? MaPhong { get; set; }

    public DateTime? NgayDat { get; set; }

    public DateTime? NgayDen { get; set; }

    public DateTime? NgayDi { get; set; }

    public int? NumAdults { get; set; }

    public int? NumChildrens { get; set; }

    public virtual KhachHang? MaKhachHangNavigation { get; set; }

    public virtual KhachSan? MaKhachSanNavigation { get; set; }

    public virtual Phong? MaPhongNavigation { get; set; }
}
