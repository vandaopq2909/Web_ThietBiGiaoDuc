using System;

namespace Web_ThietBiGiaoDuc.Models
{
    public class DanhMucModel
    {
        public DanhMucModel() 
        {
            MaDM= "DM" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        public string MaDM { get; set; }
        public string TenDM {  get; set; }
        public string TrangThai { get; set; }

    }
}