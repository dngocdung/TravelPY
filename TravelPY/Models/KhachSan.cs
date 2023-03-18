using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class KhachSan
{
    public int MaKhachSan { get; set; }

    public int? MaLoai { get; set; }

    public string? TenKhachSan { get; set; }

    public string? DiaChi { get; set; }

    public int? SoPhong { get; set; }

    public string? Sdt { get; set; }

    public string? MoTa { get; set; }

    public string? HinhAnh { get; set; }

    public string? Alias { get; set; }

    public virtual ICollection<DatKhachSan> DatKhachSans { get; } = new List<DatKhachSan>();

    public virtual ICollection<LoaiPhong> LoaiPhongs { get; } = new List<LoaiPhong>();
}
