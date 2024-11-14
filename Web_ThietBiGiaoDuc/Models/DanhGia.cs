using System;

namespace Web_ThietBiGiaoDuc.Models
{
    public class DanhGia
    {
        public DanhGia() 
        {
            MaDG= "DG" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        public string MaDG { get; set; }
        public string NoiDung { get; set; }
        public DateTimeOffset NgayDanhGia { get; set; }
    }
}