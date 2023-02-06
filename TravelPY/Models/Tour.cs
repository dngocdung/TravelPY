using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class Tour
{
    public int MaTour { get; set; }

    public string? TenTour { get; set; }

    public string? NgayKhoiHanh { get; set; }

    public int? GioKhoiHanh { get; set; }

    public int? Gia { get; set; }

    public int? GiaGiam { get; set; }

    public string? HinhAnh { get; set; }

    public string? PhuongTien { get; set; }

    public string? SoNgay { get; set; }

    public int? SoCho { get; set; }

    public string? MoTa { get; set; }

    public int? MaDanhMuc { get; set; }

    public int? MaHdv { get; set; }

    public bool TrangThai { get; set; }

    public string? NoiKhoiHanh { get; set; }

    public string? Alias { get; set; }

    public virtual ICollection<ChiTietDatTour> ChiTietDatTours { get; } = new List<ChiTietDatTour>();

    public virtual DanhMuc? MaDanhMucNavigation { get; set; }

    public virtual HuongDanVien? MaHdvNavigation { get; set; }
}
