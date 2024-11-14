using Antlr.Runtime.Tree;
using System;

namespace Web_ThietBiGiaoDuc.Models
{
    public class DonHang
    {
        public string MaDH { get; set; }
        public DateTimeOffset NgayDatHang { get; set; }
        public int TongSoLuong { get; set; } = 0;
        public double TongTien { get; set; } = 0;
        public string DiaChiGiaoHang { get; set; }
        public string TrangThai { get; set; }
    }
}