using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Web_ThietBiGiaoDuc.Models
{
    public class PhieuNhap
    {
        public PhieuNhap() 
        {
            MaPN = "PN" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        [Key]
        public string MaPN { get; set; }
        public DateTimeOffset NgayNhapHang { get; set; }
        public int TongSoLuong { get; set; } = 0;
        public double TongTien { get; set; } = 0;
        public string TrangThai { get; set; }
        public string MaNCC { get; set; }
        public virtual NhaCungCap NhaCungCap { get; set; }
        public string MaNV { get; set; }
        public virtual NhanVien NhanVien { get; set; }
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
    }
}