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
        public DbSet<DanhGia> danhGias { get; set; }
        public DbSet<DonHang> donHangs { get; set; } 
        public DbSet<ChiTietDonHang> chiTietDonHangs { get; set; } 
        public DbSet<HinhAnh> hinhAnhs { get; set; }
        public DbSet<KhuyenMai> khuyenMais { get; set; }
        public DbSet<NhaCungCap> nhaCungCaps { get; set; }
        public DbSet<PhieuNhap> phieuNhaps { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Bảng ApDungKhuyenMai: 
            modelBuilder.Entity<ApDungKhuyenMai>()
                .HasKey(ad => new { ad.MaKM, ad.MaSP });
            // Cấu hình quan hệ giữa ApDungKhuyenMai và SanPham
            modelBuilder.Entity<ApDungKhuyenMai>()
               .HasRequired(sp => sp.SanPham)
               .WithMany(ad => ad.ApDungKhuyenMais)
               .HasForeignKey(s => s.MaSP);
            // Cấu hình quan hệ giữa ApDungKhuyenMai và KhuyenMai
            modelBuilder.Entity<ApDungKhuyenMai>()
                .HasRequired(km => km.KhuyenMai)
                .WithMany(ad => ad.ApDungKhuyenMais)
                .HasForeignKey(s => s.MaKM);

            // Bảng ChiTietDonHang: 
            modelBuilder.Entity<ChiTietDonHang>()
                .HasKey(ad => new { ad.MaSP, ad.MaDH });
            // Cấu hình quan hệ giữa ChiTietDonHang và SanPham
            modelBuilder.Entity<ChiTietDonHang>()
               .HasRequired(sp => sp.SanPham)
               .WithMany(ad => ad.ChiTietDonHangs)
               .HasForeignKey(s => s.MaSP);
            // Cấu hình quan hệ giữa ChiTietDonHang và DonHang
            modelBuilder.Entity<ChiTietDonHang>()
                .HasRequired(km => km.DonHang)
                .WithMany(ad => ad.ChiTietDonHangs)
                .HasForeignKey(s => s.MaDH);

            // Bảng ChiTietPhieuNhap: 
            modelBuilder.Entity<ChiTietPhieuNhap>()
                .HasKey(ad => new { ad.MaSP, ad.MaPN });
            // Cấu hình quan hệ giữa ChiTietPhieuNhap và SanPham
            modelBuilder.Entity<ChiTietPhieuNhap>()
               .HasRequired(sp => sp.SanPham)
               .WithMany(ad => ad.ChiTietPhieuNhaps)
               .HasForeignKey(s => s.MaSP);
            // Cấu hình quan hệ giữa ChiTietPhieuNhap và PhieuNhap
            modelBuilder.Entity<ChiTietPhieuNhap>()
                .HasRequired(km => km.PhieuNhap)
                .WithMany(ad => ad.ChiTietPhieuNhaps)
                .HasForeignKey(s => s.MaPN);
        }

    }
}