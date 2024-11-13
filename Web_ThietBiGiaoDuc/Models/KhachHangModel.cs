using System;

namespace Web_ThietBiGiaoDuc.Models
{
    public class KhachHangModel
    {
        public KhachHangModel() 
        {
            MaKH = "KH" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        public string MaKH { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string TrangThai { get; set; }
    }
}