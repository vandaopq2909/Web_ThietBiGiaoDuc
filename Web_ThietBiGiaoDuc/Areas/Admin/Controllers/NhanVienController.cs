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
            var t = db.nhanViens.ToList();
            return View();
        }
        public ActionResult DangNhap() { return View(); }
        [HttpPost]
        public ActionResult DangNhap(NhanVien nhanVien)
        {
            if (nhanVien != null)
            {
                DatabaseContext db = new DatabaseContext();
                NhanVien nv = db.nhanViens.Where(u => u.TenDangNhap == u.TenDangNhap).FirstOrDefault();
                if (nv != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(nhanVien.MatKhau, nv.MatKhau))
                    {
                        HttpCookie authCookie = new HttpCookie("auth", nv.TenDangNhap);
                        HttpCookie roleCookie = new HttpCookie("role", nv.Quyen.TenQuyen);
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
    }
}