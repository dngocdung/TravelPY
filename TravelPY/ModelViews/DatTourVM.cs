using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TravelPY.ModelViews
{
    public class DatTourVM
    {
        [Key]
        public int MaKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ và Tên")]
        public string TenKhachHang { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string SDT { get; set; }
        [Required(ErrorMessage = "Địa chỉ nhận hàng")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành")]
        public int TinhThanh { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Quận/Huyện")]
        public int QuanHuyen { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Phường/Xã")]
        public int PhuongXa { get; set; }
        public int PaymentID { get; set; }
        public string Note { get; set; }
    }
}
