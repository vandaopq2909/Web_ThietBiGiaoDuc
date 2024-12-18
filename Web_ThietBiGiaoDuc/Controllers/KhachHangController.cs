﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
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
                KhachHang khach = db.khachHangs.Where(u => u.TenDangNhap == khachHang.TenDangNhap).FirstOrDefault();
           
                if (khach != null && khach.TrangThai=="hoatdong")
                {
                    if (BCrypt.Net.BCrypt.Verify(khachHang.MatKhau, khach.MatKhau))
                    {
                        HttpCookie authCookie = new HttpCookie("auth", khach.TenDangNhap);
                        HttpCookie maKHCookie= new HttpCookie("makh", khach.MaKH);
                        authCookie.Expires = DateTime.Now.AddDays(10);
                        maKHCookie.Expires = DateTime.Now.AddDays(10);
                        Response.Cookies.Add(authCookie);
                        Response.Cookies.Add(maKHCookie);
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
            authCookie.Expires = DateTime.Now.AddDays(-10);

            Response.Cookies.Add(authCookie);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Sua(string makh)
        {
            DatabaseContext db = new DatabaseContext();
            var kh = db.khachHangs.Where(x => x.MaKH == makh).FirstOrDefault();
            if (kh == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(kh);
        }
        [HttpPost]
        public ActionResult Sua(KhachHang kh)
        {
            DatabaseContext db = new DatabaseContext();
            if (kh != null)
            {
                var khachhang = db.khachHangs.Where(x => x.MaKH == kh.MaKH).FirstOrDefault();
                khachhang.HoTen = kh.HoTen;
                khachhang.DiaChi = kh.DiaChi;
                khachhang.Email = kh.Email;
                khachhang.SDT = kh.SDT;
                db.SaveChanges();
            }
            return RedirectToAction("Index","Home");
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
                TempData["ErrorMessage"] = "Thông tin không hợp lệ. Vui lòng kiểm tra lại.";
                return View();
            }

            DatabaseContext db = new DatabaseContext();
            KhachHang khach = db.khachHangs.FirstOrDefault(u => u.Email == khachHang.Email);
            if (khach != null)
            {
                TempData["ErrorMessage"] = "Email đã tồn tại. Vui lòng sử dụng email khác.";
                return View();
            }

            khach = db.khachHangs.FirstOrDefault(u => u.TenDangNhap == khachHang.TenDangNhap);
            if (khach != null)
            {
                TempData["ErrorMessage"] = "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.";
                return View();
            }

            // Nếu không có lỗi, thực hiện đăng ký
            khach = new KhachHang
            {
                TenDangNhap = khachHang.TenDangNhap,
                Email = khachHang.Email,
                MatKhau = BCrypt.Net.BCrypt.HashPassword(khachHang.MatKhau),
                TrangThai = "hoatdong"
            };

            db.khachHangs.Add(khach);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Đăng ký thành công. Bạn có thể đăng nhập ngay bây giờ!";
            return RedirectToAction("DangNhap");
        }


        public ActionResult DoiMatKhau(string makh)
        {
            DatabaseContext db = new DatabaseContext();
            var kh = db.khachHangs.Where(x => x.MaKH == makh).FirstOrDefault();
            if (kh == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(kh);
        }
        [HttpPost]
        public ActionResult DoiMatKhau(string MatKhauCu, string MatKhauMoi, string XacNhanMatKhau)
        {
            string tenDangNhap = Request.Cookies["auth"]?.Value;

            if (string.IsNullOrEmpty(tenDangNhap))
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }

            DatabaseContext db = new DatabaseContext();
            var khachHang = db.khachHangs.FirstOrDefault(u => u.TenDangNhap == tenDangNhap);

            if (khachHang != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(MatKhauCu, khachHang.MatKhau))
                {
                    ModelState.AddModelError("MatKhauCu", "Mật khẩu cũ không đúng");
                    return View();
                }
                if (MatKhauMoi != XacNhanMatKhau)
                {
                    ModelState.AddModelError("XacNhanMatKhau", "Mật khẩu mới không khớp");
                    return View();
                }

                // Mã hóa và lưu vào CSDL
                khachHang.MatKhau = BCrypt.Net.BCrypt.HashPassword(MatKhauMoi);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }
        }

    }
}