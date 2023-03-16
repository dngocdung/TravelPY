
using TravelPY.Models;

namespace TravelPY.ModelViews
{
    public class CartItem
    {
        public Tour product { get; set; }
        public Phong phong { get; set; }
        public int amount { get; set; }
        public double TotalMoney => amount * product.GiaGiam.Value;
        public double RoomMoney => amount* phong.Gia.Value;
        public double Tong => TotalMoney + RoomMoney;
    }
}
