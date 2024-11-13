using System;
namespace Web_ThietBiGiaoDuc.Models
{
    public class LoaiSanPhamModel
    {
        public LoaiSanPhamModel() 
        {
            MaLoai= "L" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        public string MaLoai { get; set; }
        public string TenLoai {  get; set; }
        public string TrangThai { get; set; }
    }
}