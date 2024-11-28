using System;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class HinhAnh
    { 
        public HinhAnh()
        {
            MaHinhAnh= "HA" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        [Key]
        public string MaHinhAnh { get; set; }
        public string TenHinhAnh {get; set; }
        public string MaSP { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}