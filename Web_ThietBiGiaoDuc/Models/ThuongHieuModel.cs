using System;

namespace Web_ThietBiGiaoDuc.Models
{
    public class ThuongHieuModel
    {
        public ThuongHieuModel()
        {
            MaThuongHieu= "TH" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        public string MaThuongHieu { get; set; }
        public string TenThuongHieu { get; set; }
        public string TrangThai {  get; set; }
    }
}