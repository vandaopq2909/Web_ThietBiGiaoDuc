namespace Web_ThietBiGiaoDuc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                        DanhMuc_MaDM = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaLoai)
                .ForeignKey("dbo.DanhMucs", t => t.DanhMuc_MaDM)
                .Index(t => t.DanhMuc_MaDM);
            
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
                        LoaiSanPham_MaLoai = c.String(maxLength: 128),
                        ThuongHieu_MaTH = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaSP)
                .ForeignKey("dbo.LoaiSanPhams", t => t.LoaiSanPham_MaLoai)
                .ForeignKey("dbo.ThuongHieux", t => t.ThuongHieu_MaTH)
                .Index(t => t.LoaiSanPham_MaLoai)
                .Index(t => t.ThuongHieu_MaTH);
            
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
            DropForeignKey("dbo.SanPhams", "ThuongHieu_MaTH", "dbo.ThuongHieux");
            DropForeignKey("dbo.SanPhams", "LoaiSanPham_MaLoai", "dbo.LoaiSanPhams");
            DropForeignKey("dbo.LoaiSanPhams", "DanhMuc_MaDM", "dbo.DanhMucs");
            DropIndex("dbo.SanPhams", new[] { "ThuongHieu_MaTH" });
            DropIndex("dbo.SanPhams", new[] { "LoaiSanPham_MaLoai" });
            DropIndex("dbo.LoaiSanPhams", new[] { "DanhMuc_MaDM" });
            DropTable("dbo.ThuongHieux");
            DropTable("dbo.SanPhams");
            DropTable("dbo.LoaiSanPhams");
            DropTable("dbo.DanhMucs");
        }
    }
}
