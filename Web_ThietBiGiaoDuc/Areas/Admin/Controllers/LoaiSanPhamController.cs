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
        public ActionResult Sua()
        {
            return View();
        }
    }
}