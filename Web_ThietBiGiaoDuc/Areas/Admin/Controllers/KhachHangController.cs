using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: Admin/KhachHang
        public ActionResult Index()
        {
            //test
            DatabaseContext db = new DatabaseContext();
            string tenDangNhap = Request.Cookies["auth"]?.Value;
            var nv = db.nhanViens.Where(x => x.TenDangNhap == tenDangNhap).FirstOrDefault();
            if (nv == null)
            {
                return RedirectToAction("DangNhap", "NhanVien");
            }
            var listKH = db.khachHangs.ToList();
            return View(listKH);
        }
        public ActionResult Them()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Them(KhachHang kh)
        {
            if (kh == null)
            {
            }
            else
            {
                DatabaseContext db = new DatabaseContext();
                db.khachHangs.Add(kh);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Sua(string maKH)
        {
            DatabaseContext db = new DatabaseContext();
            var kh = db.khachHangs.Where(x => x.MaKH == maKH).FirstOrDefault();
            return View(kh);
        }
        [HttpPost]
        public ActionResult Sua(KhachHang kh)
        {
            DatabaseContext db = new DatabaseContext();
            var khachHang = db.khachHangs.Where(x => x.MaKH == kh.MaKH).FirstOrDefault();

            //update
            khachHang.HoTen = kh.HoTen;
            khachHang.SDT = kh.SDT;
            khachHang.DiaChi = kh.DiaChi;
            khachHang.TrangThai = kh.TrangThai;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Xoa(string maKH)
        {
            DatabaseContext db = new DatabaseContext();
            var kh = db.khachHangs.Where(x => x.MaKH == maKH).FirstOrDefault();

            //cập nhật trạng thái tài khoản khách hàng
            //kh.TrangThai = "daxoa";

            db.khachHangs.Remove(kh);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}