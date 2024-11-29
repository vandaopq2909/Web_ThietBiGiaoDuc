using System;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class DanhGia
    {
        public DanhGia() 
        {
            MaDG= "DG" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        [Key]
        public string MaDG { get; set; }
        public string NoiDung { get; set; }
        public DateTimeOffset NgayDanhGia { get; set; }
        public string MaSP { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}