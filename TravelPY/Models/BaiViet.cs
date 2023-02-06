using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class BaiViet
{
    public int MaBaiViet { get; set; }

    public string? TieuDe { get; set; }

    public string? HinhAnh { get; set; }

    public bool Publisher { get; set; }

    public string? NoiDung { get; set; }

    public DateTime? NgayTao { get; set; }

    public int? MaTaiKhoan { get; set; }

    public int? MaPage { get; set; }

    public string? Alias { get; set; }

    public virtual Page? MaPageNavigation { get; set; }

    public virtual TaiKhoan? MaTaiKhoanNavigation { get; set; }
}
