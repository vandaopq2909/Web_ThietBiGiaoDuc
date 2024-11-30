using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class KhuyenMaiController : Controller
    {
        // GET: Admin/KhuyenMai
        public ActionResult Index(string search = "", int page = 1, int pageSize = 10)
        {
            DatabaseContext db = new DatabaseContext();
            string tenDangNhap = Request.Cookies["auth"]?.Value;
            var nv = db.nhanViens.Where(x => x.TenDangNhap == tenDangNhap).FirstOrDefault();
            if (nv == null)
            {
                return RedirectToAction("DangNhap", "NhanVien");
            }
            var listKM = db.khuyenMais.AsQueryable();

            // Lọc theo từ khóa tìm kiếm nếu có
            if (!string.IsNullOrEmpty(search))
            {
                listKM = listKM.Where(km => km.TenKM.Contains(search));
            }

            // Áp dụng phân trang
            var pagedkhuyenMais = listKM.OrderBy(sp => sp.MaKM).ToPagedList(page, pageSize);
            return View(pagedkhuyenMais);
        }
        // Action Search: Xử lý AJAX tìm kiếm và phân trang
        [HttpGet]
        public JsonResult TimKiem(string search = "", int page = 1, int pageSize = 10)
        {
            DatabaseContext db = new DatabaseContext();
            var listKM = db.khuyenMais.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                listKM = listKM.Where(km => km.TenKM.Contains(search) ||
                                            km.MaKM.Contains(search));
            }

            var pagedkhuyenMais = listKM.OrderBy(km => km.MaKM).ToPagedList(page, pageSize);

            var result = new
            {
                Data = pagedkhuyenMais.Select(km => new
                {
                    km.MaKM,
                    km.TenKM,
                    km.TiLeGiamGia,
                    km.MoTa,
                    km.TrangThai
                }).ToList(),
                TotalPages = pagedkhuyenMais.PageCount,
                CurrentPage = pagedkhuyenMais.PageNumber
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Them()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Them(KhuyenMai km)
        {
            DatabaseContext db = new DatabaseContext();
            if (km == null)
            {
                //return RedirectToAction("Index");
            }
            else
            {
                db.khuyenMais.Add(km);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Sua(string maKM)
        {
            DatabaseContext db = new DatabaseContext();
            var km = db.khuyenMais.Where(x => x.MaKM == maKM).FirstOrDefault();
            return View(km);
        }
        [HttpPost]
        public ActionResult Sua(KhuyenMai km)
        {
            DatabaseContext db = new DatabaseContext();
            var danhMuc = db.khuyenMais.Where(x => x.MaKM == km.MaKM).FirstOrDefault();

            //update
            danhMuc.TenKM = km.TenKM;
            danhMuc.TiLeGiamGia = km.TiLeGiamGia;
            danhMuc.MoTa = km.MoTa;
            danhMuc.TrangThai = km.TrangThai;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Xoa(string maKM)
        {
            DatabaseContext db = new DatabaseContext();
            var km = db.khuyenMais.Where(x => x.MaKM == maKM).FirstOrDefault();

            db.khuyenMais.Remove(km);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}