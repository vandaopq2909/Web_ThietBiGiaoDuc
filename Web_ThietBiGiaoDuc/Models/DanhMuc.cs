using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class DanhMuc
    {
        public DanhMuc() 
        {
            MaDM = "DM" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        [Key]
        public string MaDM { get; set; }
        public string TenDanhMuc {  get; set; }
        public string TrangThai { get; set; }
        public virtual ICollection<LoaiSanPham> LoaiSanPhams { get; set; }
    }
}