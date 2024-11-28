using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class NhaCungCap
    {
        public NhaCungCap() 
        {
            MaNCC = "NCC" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        [Key]
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string TrangThai { get; set; }
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; }

    }
}