using System;

namespace Web_ThietBiGiaoDuc.Models
{
    public class PhieuDoiTra
    {
        public PhieuDoiTra() 
        {
            MaPDT= "PDT" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        public string MaPDT { get; set; }
        public DateTimeOffset NgayDoiTra { get; set; }
        public string LiDo { get; set; }
        public string TrangThai { get; set; }
    }
}