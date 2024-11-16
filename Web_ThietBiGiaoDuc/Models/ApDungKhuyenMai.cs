using System;
namespace Web_ThietBiGiaoDuc.Models
{
    public class ApDungKhuyenMai
    {
        public string MaKM { get; set; }
        public virtual KhuyenMai KhuyenMai { get; set; }
        public string MaSP {  get; set; }
        public virtual SanPham SanPham { get; set; }
        public DateTimeOffset NgayBD { get; set; }
        public DateTimeOffset NgayKT { get; set; }
    }
}