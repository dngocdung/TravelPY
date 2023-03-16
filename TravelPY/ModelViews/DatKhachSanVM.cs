using System.ComponentModel.DataAnnotations;

namespace TravelPY.ModelViews
{
    public class DatKhachSanVM
    {
        [Key]
        public int MaKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ và Tên")]
        public string TenKhachHang { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string SDT { get; set; }
        
        
        [Required(ErrorMessage = "Vui lòng chọn ngày đến")]
        public DateTime NgayDen { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn ngày đi")]
        public DateTime NgayDi { get; set; }
        
        
        public string Note { get; set; }
    }
}
