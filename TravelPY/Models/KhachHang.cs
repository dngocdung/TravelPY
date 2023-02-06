using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class KhachHang
{
    public int MaKhachHang { get; set; }

    public string TenKhachHang { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string MatKhau { get; set; } = null!;

    public DateTime? NgaySinh { get; set; }

    public DateTime? NgayTao { get; set; }

    public string Salt { get; set; } = null!;

    public int? LocationId { get; set; }

    public int? Tinh { get; set; }

    public int? PhuongXa { get; set; }

    public virtual ICollection<DatTour> DatTours { get; } = new List<DatTour>();

    public virtual Location? Location { get; set; }
}
