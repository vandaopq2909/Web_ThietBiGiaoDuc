using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class KhuyenMai
    {
        public KhuyenMai()
        {
            MaKM = "KM" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        [Key]
        public string MaKM { get; set; }
        public string TenKM {  get; set; }
        public string MoTa { get; set; }
        public double TiLeGiamGia {  get; set; }
        public string TrangThai { get; set; }
        public virtual ICollection<ApDungKhuyenMai> ApDungKhuyenMais { get; set; }
    }
}