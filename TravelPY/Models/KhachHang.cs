using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class KhachHang
{
    public int MaKhachHang { get; set; }

    public string? TenKhachHang { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public string? MatKhau { get; set; }

    public DateTime? NgaySinh { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? Salt { get; set; }

    public int? LocationId { get; set; }

    public int? QuanHuyen { get; set; }

    public int? PhuongXa { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? Avatar { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<DatTour> DatTours { get; } = new List<DatTour>();

    public virtual Location? Location { get; set; }
}
