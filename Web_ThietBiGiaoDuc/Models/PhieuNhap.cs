using System;
namespace Web_ThietBiGiaoDuc.Models
{
    public class PhieuNhap
    {
        public PhieuNhap() 
        {
            MaPN = "PN" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        public string MaPN { get; set; }
        public DateTimeOffset NgayNhapHang { get; set; }
        public int TongSoLuong { get; set; } = 0;
        public double TongTien { get; set; } = 0;
        public string TrangThai { get; set; }
    }
}