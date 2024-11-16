using System;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class HinhAnh
    { 
        public HinhAnh()
        {
            MaHinhAnh= "HA" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        [Key]
        public string MaHinhAnh { get; set; }
        public string TenHinhAnh {get; set; }
        public string MaSP { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}