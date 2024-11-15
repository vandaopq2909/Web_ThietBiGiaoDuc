namespace Web_ThietBiGiaoDuc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addModels : DbMigration
    {
        public override void Up()
        {
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
                        Quyen_MaQuyen = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MaNV)
                .ForeignKey("dbo.Quyens", t => t.Quyen_MaQuyen)
                .Index(t => t.Quyen_MaQuyen);
            
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
            DropForeignKey("dbo.NhanViens", "Quyen_MaQuyen", "dbo.Quyens");
            DropIndex("dbo.NhanViens", new[] { "Quyen_MaQuyen" });
            DropTable("dbo.Quyens");
            DropTable("dbo.NhanViens");
            DropTable("dbo.KhachHangs");
        }
    }
}
