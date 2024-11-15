using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Web_ThietBiGiaoDuc.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("MyConnectionString") { }
        public DbSet<DanhMuc> danhMucs { get; set; }
        public DbSet<LoaiSanPham> loaiSanPhams { get; set; }
        public DbSet<ThuongHieu> thuongHieus { get; set; }
        public DbSet<SanPham> sanPhams { get; set; }
        public DbSet<Quyen> quyens { get; set; }
        public DbSet<NhanVien> nhanViens { get; set; }
        public DbSet<KhachHang> khachHangs { get; set; }
    }
}