using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class TaiKhoan
{
    public int MaTaiKhoan { get; set; }

    public string? TenTaiKhoan { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public bool TrangThai { get; set; }

    public int? MaVaiTro { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? MatKhau { get; set; }

    public virtual ICollection<BaiViet> BaiViets { get; } = new List<BaiViet>();

    public virtual VaiTro? MaVaiTroNavigation { get; set; }
}
