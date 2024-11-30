using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();
            string tenDangNhap = Request.Cookies["auth"]?.Value;
            var nv = db.nhanViens.Where(x => x.TenDangNhap == tenDangNhap).FirstOrDefault();
            if( nv==null)
            {
                return RedirectToAction("DangNhap", "NhanVien");
            }
            var date = DateTime.Now;

            ViewBag.SLKhachHang = db.khachHangs.ToList().Count;


            ViewBag.SLKhachHangDaMuaHang = db.donHangs
            .Where(dh => dh.TongTien > 0) 
            .Select(dh => dh.MaKH) 
            .Distinct() // Loại bỏ trùng lặp
            .Count(); 

            ViewBag.SLDonHangTrongNgay = db.donHangs
            .Where(dh => dh.NgayDatHang.Day == date.Day &&
                dh.NgayDatHang.Month == date.Month &&
                dh.NgayDatHang.Year == date.Year)
            .ToList().Count;
            
            ViewBag.DoanhThuTrongNgay = db.chiTietDonHangs
            .Where(dh => dh.DonHang.NgayDatHang.Day == date.Day &&
                        dh.DonHang.NgayDatHang.Month == date.Month &&
                        dh.DonHang.NgayDatHang.Year == date.Year)
            .Sum(dh => (double?)dh.TongTien) ?? 0;

            ViewBag.DoanhThuTrongThang = db.chiTietDonHangs
            .Where(dh => dh.DonHang.NgayDatHang.Month == date.Month &&
                        dh.DonHang.NgayDatHang.Year == date.Year)
            .Sum(dh => (double?)dh.TongTien) ?? 0;
            return View();
        }
    }
}