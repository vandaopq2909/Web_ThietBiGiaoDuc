using Antlr.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class DonHang
    {
        [Key]
        public string MaDH { get; set; }
        public DateTimeOffset NgayDatHang { get; set; }
        public int TongSoLuong { get; set; } = 0;
        public double TongTien { get; set; } = 0;
        public string DiaChiGiaoHang { get; set; }
        public string TrangThai { get; set; }
        public string MaKH { get; set; }
        public virtual KhachHang KhachHang { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

    }
}