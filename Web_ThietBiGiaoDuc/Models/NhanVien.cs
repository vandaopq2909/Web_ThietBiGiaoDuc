using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class NhanVien
    {
        public NhanVien() 
        {
            MaNV = "NV" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        [Key]
        public string MaNV { get; set; }
        public string TenDangNhap { get; set; }
        public string HoTen {  get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string TrangThai { get; set; }
        public string MaQuyen { get; set; }
        public virtual Quyen Quyen { get; set; }
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; }
    }
}