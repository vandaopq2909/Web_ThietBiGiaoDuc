namespace Web_ThietBiGiaoDuc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDatabse : DbMigration
    {
        public override void Up()
        {
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
                        ThongTinChiTiet = c.String(),
                        TrangThai = c.String(),
                        MaLoai = c.String(maxLength: 128),
                        MaTH = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaSP)
                .ForeignKey("dbo.LoaiSanPhams", t => t.MaLoai)
                .ForeignKey("dbo.ThuongHieux", t => t.MaTH)
                .Index(t => t.MaLoai)
                .Index(t => t.MaTH);
            
            CreateTable(
                "dbo.ThuongHieux",
                c => new
                    {
                        MaTH = c.String(nullable: false, maxLength: 128),
                        TenThuongHieu = c.String(),
                        TrangThai = c.String(),
                    })
                .PrimaryKey(t => t.MaTH);
            
            CreateTable(
                "dbo.KhachHangs",
                c => new
                    {
                        MaKH = c.String(nullable: false, maxLength: 128),
                        TenDangNhap = c.String(),
                        MatKhau = c.String(),
                        Email = c.String(),
                        SDT = c.String(),
                        DiaChi = c.String(),
                        TrangThai = c.String(),
                    })
                .PrimaryKey(t => t.MaKH);
            
            CreateTable(
                "dbo.NhanViens",
                c => new
                    {
                        MaNV = c.String(nullable: false, maxLength: 128),
                        TenDangNhap = c.String(),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NhanViens", "MaQuyen", "dbo.Quyens");
            DropForeignKey("dbo.SanPhams", "MaTH", "dbo.ThuongHieux");
            DropForeignKey("dbo.SanPhams", "MaLoai", "dbo.LoaiSanPhams");
            DropForeignKey("dbo.LoaiSanPhams", "MaDM", "dbo.DanhMucs");
            DropIndex("dbo.NhanViens", new[] { "MaQuyen" });
            DropIndex("dbo.SanPhams", new[] { "MaTH" });
            DropIndex("dbo.SanPhams", new[] { "MaLoai" });
            DropIndex("dbo.LoaiSanPhams", new[] { "MaDM" });
            DropTable("dbo.Quyens");
            DropTable("dbo.NhanViens");
            DropTable("dbo.KhachHangs");
            DropTable("dbo.ThuongHieux");
            DropTable("dbo.SanPhams");
            DropTable("dbo.LoaiSanPhams");
            DropTable("dbo.DanhMucs");
        }
    }
}
