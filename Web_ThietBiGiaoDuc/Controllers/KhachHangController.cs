using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(KhachHang khachHang)
        {
            if (khachHang != null)
            {
                DatabaseContext db = new DatabaseContext();
                KhachHang khach = db.khachHangs.Where(u => u.TenDangNhap == u.TenDangNhap).FirstOrDefault();
                if (khach != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(khachHang.MatKhau, khach.MatKhau))
                    {
                        HttpCookie authCookie = new HttpCookie("auth", khach.TenDangNhap);
                        Response.Cookies.Add(authCookie);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("MatKhau", "Mật khẩu không đúng");
                }

            }
            return View();
        }
        public ActionResult DangXuat()
        {
            HttpCookie authCookie = new HttpCookie("auth");
            authCookie.Expires = DateTime.Now.AddDays(-1);

            Response.Cookies.Add(authCookie);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DangKi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKi(KhachHang khachHang)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            DatabaseContext db = new DatabaseContext();
            KhachHang khach = db.khachHangs.Where(u => u.Email == khachHang.Email).FirstOrDefault();
            if (khach != null)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại");
                return View();
            }
            khach = db.khachHangs.Where(u => u.TenDangNhap == khachHang.TenDangNhap).FirstOrDefault();
            if (khach != null)
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại");
                return View();
            }
            khach = new KhachHang();
            khach.TenDangNhap = khachHang.TenDangNhap;
            khach.Email = khachHang.Email;
            khach.MatKhau = BCrypt.Net.BCrypt.HashPassword(khachHang.MatKhau);
            db.khachHangs.Add(khach);
            db.SaveChanges();

            return RedirectToAction("DangNhap");
        }
    }
}