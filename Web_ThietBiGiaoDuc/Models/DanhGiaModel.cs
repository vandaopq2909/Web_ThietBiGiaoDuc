using System;

namespace Web_ThietBiGiaoDuc.Models
{
    public class DanhGiaModel
    {
        public DanhGiaModel() 
        {
            MaDG= "DG" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        public string MaDG { get; set; }
        public string DanhGia { get; set; }
        public DateTimeOffset NgayDanhGia { get; set; }
    }
}