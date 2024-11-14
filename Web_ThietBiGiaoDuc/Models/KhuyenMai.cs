using System;

namespace Web_ThietBiGiaoDuc.Models
{
    public class KhuyenMai
    {
        public KhuyenMai()
        {
            MaKM = "KM" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        public string MaKM { get; set; }
        public string TenKM {  get; set; }
        public string MoTa { get; set; }
        public double TiLeGiamGia {  get; set; }
        public string TrangThai { get; set; }
    }
}