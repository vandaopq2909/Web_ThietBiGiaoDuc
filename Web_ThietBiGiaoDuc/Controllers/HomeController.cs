using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Controllers
{
    public class SanPhamVM
    {
        public string MaSP { get; set; }
        public string TenSanPham { get; set; }
        public double Gia { get; set; }
        public string img { get; set; }
    }
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();

            // Lấy tối đa 4 sản phẩm có trạng thái 'hoatdong' và hình ảnh đầu tiên
            ViewBag.listSPNoiBat = db.sanPhams
                .OrderBy(sp => Guid.NewGuid())
                .Take(4)                                  // Lấy tối đa 4 sản phẩm
                .Select(sp => new SanPhamVM
                {
                    MaSP = sp.MaSP,
                    TenSanPham = sp.TenSanPham,
                    Gia = sp.Gia,
                    img = sp.HinhAnhs.Select(h => h.TenHinhAnh).FirstOrDefault()
                }).ToList();

            //list sản phẩm theo loại
            ViewBag.listSPBanGheHS = db.sanPhams
                .Where(sp => sp.LoaiSanPham.MaLoai == "L7")  // Lọc sản phẩm theo loại "Bàn ghế học sinh"
                .Take(4)                                  // Lấy tối đa 4 sản phẩm
                .Select(sp => new SanPhamVM
                {
                    MaSP = sp.MaSP,
                    TenSanPham = sp.TenSanPham,
                    Gia = sp.Gia,
                    img = sp.HinhAnhs.Select(h => h.TenHinhAnh).FirstOrDefault()
                }).ToList();

            //list sản phẩm theo loại
            ViewBag.listSPThietBiDayHoc = db.sanPhams
                .Where(sp => sp.LoaiSanPham.MaLoai == "L15")  // Lọc sản phẩm theo loại "Bàn ghế học sinh"
                .Take(4)                                  // Lấy tối đa 4 sản phẩm
                .Select(sp => new SanPhamVM
                {
                    MaSP = sp.MaSP,
                    TenSanPham = sp.TenSanPham,
                    Gia = sp.Gia,
                    img = sp.HinhAnhs.Select(h => h.TenHinhAnh).FirstOrDefault()
                }).ToList();

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}