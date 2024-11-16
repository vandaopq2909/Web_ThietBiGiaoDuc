namespace Web_ThietBiGiaoDuc.Models
{
    public class ChiTietPhieuNhap
    {
        public string MaPN { get; set; }
        public virtual PhieuNhap PhieuNhap { get; set; }
        public string MaSP { get; set; }
        public virtual SanPham SanPham { get; set; }
        public int SoLuong { get; set; } = 0;
        public double Gia { get; set; } = 0;
        public double TongTien { get; set; } = 0;
        public string GhiChu { get; set; }
        public string TrangThai { get; set; }
    }
}