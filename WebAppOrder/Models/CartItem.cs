using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppOrder.Models
{
    [Serializable]
    public class CartItem
    {
        public string MaMonAn { get; set; }
        public string TenMonAn { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien
        {
            get { return SoLuong * DonGia; }
        }
    }
}