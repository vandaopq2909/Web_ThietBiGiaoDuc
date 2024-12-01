namespace Web_ThietBiGiaoDuc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApDungKhuyenMais",
                c => new
                    {
                        MaKM = c.String(nullable: false, maxLength: 128),
                        MaSP = c.String(nullable: false, maxLength: 128),
                        NgayBD = c.DateTimeOffset(nullable: false, precision: 7),
                        NgayKT = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.MaKM, t.MaSP })
                .ForeignKey("dbo.KhuyenMais", t => t.MaKM, cascadeDelete: true)
                .ForeignKey("dbo.SanPhams", t => t.MaSP, cascadeDelete: true)
                .Index(t => t.MaKM)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.KhuyenMais",
                c => new
                    {
                        MaKM = c.String(nullable: false, maxLength: 128),
                        TenKM = c.String(),
                        MoTa = c.String(),
                        TiLeGiamGia = c.Double(nullable: false),
                        TrangThai = c.String(),
                    })
                .PrimaryKey(t => t.MaKM);
            
            CreateTable(
                "dbo.SanPhams",
                c => new
                    {
                        MaSP = c.String(nullable: false, maxLength: 128),
                        TenSanPham = c.String(),
                        Gia = c.Double(nullable: false),
                        SoLuongTonKho = c.Int(nullable: false),
                        MoTa = c.String(),
                        MauSac = c.String(),
                        KichThuoc = c.String(),
                        DonViTinh = c.String(),
                        CachDongGoi = c.String(),
                        TrangThai = c.String(),
                        TinhTrangSanPham = c.String(),
                        MaLoai = c.String(maxLength: 128),
                        MaTH = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaSP)
                .ForeignKey("dbo.LoaiSanPhams", t => t.MaLoai)
                .ForeignKey("dbo.ThuongHieux", t => t.MaTH)
                .Index(t => t.MaLoai)
                .Index(t => t.MaTH);
            
            CreateTable(
                "dbo.ChiTietDonHangs",
                c => new
                    {
                        MaSP = c.String(nullable: false, maxLength: 128),
                        MaDH = c.String(nullable: false, maxLength: 128),
                        SoLuong = c.Int(nullable: false),
                        Gia = c.Double(nullable: false),
                        TongTien = c.Double(nullable: false),
                        GhiChu = c.String(),
                        TrangThaiDanhGia = c.String(),
                    })
                .PrimaryKey(t => new { t.MaSP, t.MaDH })
                .ForeignKey("dbo.DonHangs", t => t.MaDH, cascadeDelete: true)
                .ForeignKey("dbo.SanPhams", t => t.MaSP, cascadeDelete: true)
                .Index(t => t.MaSP)
                .Index(t => t.MaDH);
            
            CreateTable(
                "dbo.DonHangs",
                c => new
                    {
                        MaDH = c.String(nullable: false, maxLength: 128),
                        NgayDatHang = c.DateTimeOffset(nullable: false, precision: 7),
                        TongSoLuong = c.Int(nullable: false),
                        TongTien = c.Double(nullable: false),
                        DiaChiGiaoHang = c.String(),
                        TrangThai = c.String(),
                        MaKH = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaDH)
                .ForeignKey("dbo.KhachHangs", t => t.MaKH)
                .Index(t => t.MaKH);
            
            CreateTable(
                "dbo.KhachHangs",
                c => new
                    {
                        MaKH = c.String(nullable: false, maxLength: 128),
                        TenDangNhap = c.String(),
                        MatKhau = c.String(),
                        HoTen = c.String(),
                        Email = c.String(),
                        SDT = c.String(),
                        DiaChi = c.String(),
                        TrangThai = c.String(),
                    })
                .PrimaryKey(t => t.MaKH);
            
            CreateTable(
                "dbo.ChiTietPhieuNhaps",
                c => new
                    {
                        MaSP = c.String(nullable: false, maxLength: 128),
                        MaPN = c.String(nullable: false, maxLength: 128),
                        SoLuong = c.Int(nullable: false),
                        Gia = c.Double(nullable: false),
                        TongTien = c.Double(nullable: false),
                        GhiChu = c.String(),
                        TrangThai = c.String(),
                    })
                .PrimaryKey(t => new { t.MaSP, t.MaPN })
                .ForeignKey("dbo.PhieuNhaps", t => t.MaPN, cascadeDelete: true)
                .ForeignKey("dbo.SanPhams", t => t.MaSP, cascadeDelete: true)
                .Index(t => t.MaSP)
                .Index(t => t.MaPN);
            
            CreateTable(
                "dbo.PhieuNhaps",
                c => new
                    {
                        MaPN = c.String(nullable: false, maxLength: 128),
                        NgayNhapHang = c.DateTimeOffset(nullable: false, precision: 7),
                        TongSoLuong = c.Int(nullable: false),
                        TongTien = c.Double(nullable: false),
                        TrangThai = c.String(),
                        MaNCC = c.String(maxLength: 128),
                        MaNV = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaPN)
                .ForeignKey("dbo.NhaCungCaps", t => t.MaNCC)
                .ForeignKey("dbo.NhanViens", t => t.MaNV)
                .Index(t => t.MaNCC)
                .Index(t => t.MaNV);
            
            CreateTable(
                "dbo.NhaCungCaps",
                c => new
                    {
                        MaNCC = c.String(nullable: false, maxLength: 128),
                        TenNCC = c.String(),
                        Email = c.String(),
                        SDT = c.String(),
                        TrangThai = c.String(),
                    })
                .PrimaryKey(t => t.MaNCC);
            
            CreateTable(
                "dbo.NhanViens",
                c => new
                    {
                        MaNV = c.String(nullable: false, maxLength: 128),
                        TenDangNhap = c.String(),
                        HoTen = c.String(),
                        MatKhau = c.String(),
                        Email = c.String(),
                        SDT = c.String(),
                        DiaChi = c.String(),
                        TrangThai = c.String(),
                        MaQuyen = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaNV)
                .ForeignKey("dbo.Quyens", t => t.MaQuyen)
                .Index(t => t.MaQuyen);
            
            CreateTable(
                "dbo.Quyens",
                c => new
                    {
                        MaQuyen = c.String(nullable: false, maxLength: 128),
                        TenQuyen = c.String(),
                        TrangThai = c.String(),
                    })
                .PrimaryKey(t => t.MaQuyen);
            
            CreateTable(
                "dbo.DanhGias",
                c => new
                    {
                        MaDG = c.String(nullable: false, maxLength: 128),
                        NoiDung = c.String(),
                        NgayDanhGia = c.DateTimeOffset(nullable: false, precision: 7),
                        MaSP = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaDG)
                .ForeignKey("dbo.SanPhams", t => t.MaSP)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.HinhAnhs",
                c => new
                    {
                        MaHinhAnh = c.String(nullable: false, maxLength: 128),
                        TenHinhAnh = c.String(),
                        MaSP = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaHinhAnh)
                .ForeignKey("dbo.SanPhams", t => t.MaSP)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.LoaiSanPhams",
                c => new
                    {
                        MaLoai = c.String(nullable: false, maxLength: 128),
                        TenLoai = c.String(),
                        TrangThai = c.String(),
                        MaDM = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaLoai)
                .ForeignKey("dbo.DanhMucs", t => t.MaDM)
                .Index(t => t.MaDM);
            
            CreateTable(
                "dbo.DanhMucs",
                c => new
                    {
                        MaDM = c.String(nullable: false, maxLength: 128),
                        TenDanhMuc = c.String(),
                        TrangThai = c.String(),
                    })
                .PrimaryKey(t => t.MaDM);
            
            CreateTable(
                "dbo.ThuongHieux",
                c => new
                    {
                        MaTH = c.String(nullable: false, maxLength: 128),
                        TenThuongHieu = c.String(),
                        TrangThai = c.String(),
                    })
                .PrimaryKey(t => t.MaTH);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApDungKhuyenMais", "MaSP", "dbo.SanPhams");
            DropForeignKey("dbo.SanPhams", "MaTH", "dbo.ThuongHieux");
            DropForeignKey("dbo.SanPhams", "MaLoai", "dbo.LoaiSanPhams");
            DropForeignKey("dbo.LoaiSanPhams", "MaDM", "dbo.DanhMucs");
            DropForeignKey("dbo.HinhAnhs", "MaSP", "dbo.SanPhams");
            DropForeignKey("dbo.DanhGias", "MaSP", "dbo.SanPhams");
            DropForeignKey("dbo.ChiTietPhieuNhaps", "MaSP", "dbo.SanPhams");
            DropForeignKey("dbo.ChiTietPhieuNhaps", "MaPN", "dbo.PhieuNhaps");
            DropForeignKey("dbo.NhanViens", "MaQuyen", "dbo.Quyens");
            DropForeignKey("dbo.PhieuNhaps", "MaNV", "dbo.NhanViens");
            DropForeignKey("dbo.PhieuNhaps", "MaNCC", "dbo.NhaCungCaps");
            DropForeignKey("dbo.ChiTietDonHangs", "MaSP", "dbo.SanPhams");
            DropForeignKey("dbo.ChiTietDonHangs", "MaDH", "dbo.DonHangs");
            DropForeignKey("dbo.DonHangs", "MaKH", "dbo.KhachHangs");
            DropForeignKey("dbo.ApDungKhuyenMais", "MaKM", "dbo.KhuyenMais");
            DropIndex("dbo.LoaiSanPhams", new[] { "MaDM" });
            DropIndex("dbo.HinhAnhs", new[] { "MaSP" });
            DropIndex("dbo.DanhGias", new[] { "MaSP" });
            DropIndex("dbo.NhanViens", new[] { "MaQuyen" });
            DropIndex("dbo.PhieuNhaps", new[] { "MaNV" });
            DropIndex("dbo.PhieuNhaps", new[] { "MaNCC" });
            DropIndex("dbo.ChiTietPhieuNhaps", new[] { "MaPN" });
            DropIndex("dbo.ChiTietPhieuNhaps", new[] { "MaSP" });
            DropIndex("dbo.DonHangs", new[] { "MaKH" });
            DropIndex("dbo.ChiTietDonHangs", new[] { "MaDH" });
            DropIndex("dbo.ChiTietDonHangs", new[] { "MaSP" });
            DropIndex("dbo.SanPhams", new[] { "MaTH" });
            DropIndex("dbo.SanPhams", new[] { "MaLoai" });
            DropIndex("dbo.ApDungKhuyenMais", new[] { "MaSP" });
            DropIndex("dbo.ApDungKhuyenMais", new[] { "MaKM" });
            DropTable("dbo.ThuongHieux");
            DropTable("dbo.DanhMucs");
            DropTable("dbo.LoaiSanPhams");
            DropTable("dbo.HinhAnhs");
            DropTable("dbo.DanhGias");
            DropTable("dbo.Quyens");
            DropTable("dbo.NhanViens");
            DropTable("dbo.NhaCungCaps");
            DropTable("dbo.PhieuNhaps");
            DropTable("dbo.ChiTietPhieuNhaps");
            DropTable("dbo.KhachHangs");
            DropTable("dbo.DonHangs");
            DropTable("dbo.ChiTietDonHangs");
            DropTable("dbo.SanPhams");
            DropTable("dbo.KhuyenMais");
            DropTable("dbo.ApDungKhuyenMais");
        }
    }
}
