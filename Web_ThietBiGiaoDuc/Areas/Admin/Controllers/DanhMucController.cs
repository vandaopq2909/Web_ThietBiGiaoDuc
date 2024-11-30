using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class DanhMucController : Controller
    {
        // GET: Admin/DanhMuc
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();
            string tenDangNhap = Request.Cookies["auth"]?.Value;
            var nv = db.nhanViens.Where(x => x.TenDangNhap == tenDangNhap).FirstOrDefault();
            if (nv == null)
            {
                return RedirectToAction("DangNhap", "NhanVien");
            }
            var listDM = db.danhMucs.ToList();
            return View(listDM);
        }
        public ActionResult Them()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Them(DanhMuc dm)
        {
            DatabaseContext db = new DatabaseContext();
            if (dm == null)
            {
                //return RedirectToAction("Index");
            }
            else
            {
                db.danhMucs.Add(dm);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Sua(string maDM)
        {
            DatabaseContext db = new DatabaseContext();
            var dm = db.danhMucs.Where(x => x.MaDM == maDM).FirstOrDefault();
            return View(dm);
        }
        [HttpPost]
        public ActionResult Sua(DanhMuc dm)
        {
            DatabaseContext db = new DatabaseContext();
            var danhMuc = db.danhMucs.Where(x => x.MaDM == dm.MaDM).FirstOrDefault();

            //update
            danhMuc.TenDanhMuc = dm.TenDanhMuc;
            danhMuc.TrangThai = dm.TrangThai;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Xoa(string maDM)
        {
            DatabaseContext db = new DatabaseContext();
            var dm = db.danhMucs.Where(x => x.MaDM == maDM).FirstOrDefault();
            dm.TrangThai = "daxoa";

            //db.danhMucs.Remove(dm);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}