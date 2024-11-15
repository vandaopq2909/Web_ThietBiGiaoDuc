using System;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class NhanVien
    {
        public NhanVien() 
        {
            MaNV = "NV" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        [Key]
        public string MaNV { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string TrangThai { get; set; }
        public virtual Quyen Quyen { get; set; }
    }
}