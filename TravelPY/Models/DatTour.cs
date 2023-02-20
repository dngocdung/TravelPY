using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class DatTour
{
    public int MaDatTour { get; set; }

    public int? MaTour { get; set; }

    public int? MaKhachHang { get; set; }

    public int? SoCho { get; set; }

    public DateTime? NgayDatTour { get; set; }

    public string? GhiChu { get; set; }

    public int? MaThanhToan { get; set; }

    public DateTime? NgayThanhToan { get; set; }

    public string? DiaChi { get; set; }

    public int? LocationId { get; set; }

    public int? Tinh { get; set; }

    public int? PhuongXa { get; set; }

    public int? TongTien { get; set; }

    public virtual ICollection<ChiTietDatTour> ChiTietDatTours { get; } = new List<ChiTietDatTour>();

    public virtual KhachHang? MaKhachHangNavigation { get; set; }
}
