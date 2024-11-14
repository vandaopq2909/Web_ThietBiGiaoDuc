using System;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class DanhMuc
    {
        public DanhMuc() 
        {
            MaDM = "DM" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        [Key]
        public string MaDM { get; set; }
        public string TenDanhMuc {  get; set; }
        public string TrangThai { get; set; }

    }
}