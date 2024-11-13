using System;

namespace Web_ThietBiGiaoDuc.Models
{
    public class SanPhamModel
    {
        public SanPhamModel() 
        {
            MaSanPham= "SP" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        public string MaSanPham { get; set; }
        public string TenSanPham { get;set; }
        public double Gia { get; set; } = 0;
        public int SoLuongTonKho { get; set; } = 0;
        public string MoTa { get; set; }
        public string MauSac { get; set; }
        public string KichThuoc { get; set; }
        public string DonViTinh { get; set; }
        public string CachDongGoi { get; set; }
        public string ThongTinChiTiet { get; set; }
        public string TrangThai { get; set; }
    }
}