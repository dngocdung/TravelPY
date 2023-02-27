using System;
using System.Collections.Generic;

namespace TravelPY.Models;

public partial class TrangThai
{
    public int MaTrangThai { get; set; }

    public string? TenTrangThai { get; set; }

    public string? MoTa { get; set; }

    public virtual ICollection<DatTour> DatTours { get; } = new List<DatTour>();
}
