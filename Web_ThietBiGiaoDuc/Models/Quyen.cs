using System;
namespace Web_ThietBiGiaoDuc.Models
{
    public class Quyen
    {
        Quyen() 
        {
            MaQuyen = "Q" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }    
        public string MaQuyen { get; set; }
        public string TenQuyen { get; set; }
        public string TrangThai { get; set; }
    }
}