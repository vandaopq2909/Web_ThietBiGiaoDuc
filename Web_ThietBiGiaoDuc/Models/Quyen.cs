using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Web_ThietBiGiaoDuc.Models
{
    public class Quyen
    {
        Quyen() 
        {
            MaQuyen = "Q" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        [Key]
        public string MaQuyen { get; set; }
        public string TenQuyen { get; set; }
        public string TrangThai { get; set; }
        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}