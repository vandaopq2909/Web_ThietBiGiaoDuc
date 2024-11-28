using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class ThuongHieu
    {
        public ThuongHieu()
        {
            MaTH = "TH" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        [Key]
        public string MaTH { get; set; }
        public string TenThuongHieu { get; set; }
        public string TrangThai {  get; set; }
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}