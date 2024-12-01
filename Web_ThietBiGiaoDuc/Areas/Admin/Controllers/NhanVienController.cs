using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class NhanVienController : Controller
    {
        // GET: Admin/NhanVien
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
            var listNV = db.nhanViens.ToList();
            return View(listNV);
        }
        public ActionResult DangXuat() 
        {
            HttpCookie authCookie = new HttpCookie("auth");
            authCookie.Expires = DateTime.Now.AddDays(-10);

            Response.Cookies.Add(authCookie);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DangNhap() { return View(); }
        [HttpPost]
        public ActionResult DangNhap(NhanVien nhanVien)
        {
            if (nhanVien != null)
            {
                DatabaseContext db = new DatabaseContext();
                NhanVien nv = db.nhanViens.Where(u => u.TenDangNhap == nhanVien.TenDangNhap).FirstOrDefault();
                if (nv != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(nhanVien.MatKhau, nv.MatKhau))
                    {
                        HttpCookie authCookie = new HttpCookie("auth", nv.TenDangNhap);
                        HttpCookie roleCookie = new HttpCookie("role", nv.Quyen.TenQuyen);
                        authCookie.Expires = DateTime.Now.AddDays(30);
                        roleCookie.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(authCookie);
                        Response.Cookies.Add(roleCookie);
                        if (nv.Quyen.TenQuyen == "quanli")
                        {
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Products");
                        }

                    }
                }
                else
                {
                    ModelState.AddModelError("MatKhau", "Mật khẩu không đúng");
                }

            }
            return View();
        }
        public ActionResult Them()
        {
            DatabaseContext db = new DatabaseContext();

            // Lấy danh sách quyền hoạt động
            ViewBag.listQ = db.quyens.Where(q => q.TrangThai == "hoatdong").ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Them(NhanVien nv)
        {
            if (nv == null)
            {
            }
            else
            {
                DatabaseContext db = new DatabaseContext();
                db.nhanViens.Add(nv);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public ActionResult Sua(string maNV)
        {
            DatabaseContext db = new DatabaseContext();
            var nv = db.nhanViens.Where(x => x.MaNV == maNV).FirstOrDefault();
            ViewBag.listQ = db.quyens.Where(q => q.TrangThai == "hoatdong").ToList();
            return View(nv);
        }
        [HttpPost]
        public ActionResult Sua(NhanVien nv)
        {
            DatabaseContext db = new DatabaseContext();
            var nhanVien = db.nhanViens.Where(x => x.MaNV == nv.MaNV).FirstOrDefault();

            //update
            nhanVien.SDT = nv.SDT;
            nhanVien.DiaChi = nv.DiaChi;
            nhanVien.TrangThai = nv.TrangThai;
            nhanVien.MaQuyen = nv.MaQuyen;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Xoa(string maNV)
        {
            DatabaseContext db = new DatabaseContext();
            var nv = db.nhanViens.FirstOrDefault(x => x.MaNV == maNV);
            //cập nhật trạng thái tài khoản khách hàng
            //nv.TrangThai = "Đã xóa";
  
            db.nhanViens.Remove(nv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}