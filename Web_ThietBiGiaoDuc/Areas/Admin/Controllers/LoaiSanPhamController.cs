using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class LoaiSanPhamController : Controller
    {
        // GET: Admin/LoaiSanPham
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();

            var listLoaiSP = db.loaiSanPhams.ToList();
            return View(listLoaiSP);
        }
        public ActionResult Them()
        {
            DatabaseContext db = new DatabaseContext();

            ViewBag.listDM = db.danhMucs.Where(x => x.TrangThai == "hoatdong").ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Them(LoaiSanPham loaiSP)
        {
            DatabaseContext db = new DatabaseContext();
            if (loaiSP == null)
            {
                //return RedirectToAction("Index");
            }
            else
            {
                db.loaiSanPhams.Add(loaiSP);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Sua(string maLoai)
        {
            DatabaseContext db = new DatabaseContext();
            var loaiSP = db.loaiSanPhams.Where(x => x.MaLoai == maLoai).FirstOrDefault();

            ViewBag.listDM = db.danhMucs.Where(x => x.TrangThai == "hoatdong").ToList();
            return View(loaiSP);
        }
        [HttpPost]
        public ActionResult Sua(LoaiSanPham loaiSanPham)
        {
            DatabaseContext db = new DatabaseContext();
            var lsp = db.loaiSanPhams.Where(x => x.MaLoai == loaiSanPham.MaLoai).FirstOrDefault();

            //update
            lsp.TenLoai = loaiSanPham.TenLoai;
            lsp.MaDM = loaiSanPham.MaDM;
            lsp.TrangThai = loaiSanPham.TrangThai;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Xoa(string maLoai)
        {
            DatabaseContext db = new DatabaseContext();
            var lsp = db.loaiSanPhams.Where(x => x.MaLoai == maLoai).FirstOrDefault();
            lsp.TrangThai = "daxoa";

            //db.loaiSanPhams.Remove(lsp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}