using System;

namespace Web_ThietBiGiaoDuc.Models
{
    public class HinhAnhModel
    { 
        public HinhAnhModel()
        {
            MaHinhAnh= "HA" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        public string MaHinhAnh { get; set; }
        public string TenHinhAnh {get; set; }
    }
}