using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class ThuongHieuController : Controller
    {
        // GET: Admin/ThuongHieu
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();

            var listTH = db.thuongHieus.ToList();
            return View(listTH);
        }
        public ActionResult Them()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Them(ThuongHieu th)
        {
            DatabaseContext db = new DatabaseContext();
            if (th == null)
            {
                //return RedirectToAction("Index");
            }
            else
            {
                db.thuongHieus.Add(th);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Sua(string maTH)
        {
            DatabaseContext db = new DatabaseContext();
            var th = db.thuongHieus.Where(x => x.MaTH == maTH).FirstOrDefault();
            return View(th);
        }
        [HttpPost]
        public ActionResult Sua(ThuongHieu th)
        {
            DatabaseContext db = new DatabaseContext();
            var thuongHieu = db.thuongHieus.Where(x => x.MaTH == th.MaTH).FirstOrDefault();

            //update
            thuongHieu.TenThuongHieu = th.TenThuongHieu;
            thuongHieu.TrangThai = th.TrangThai;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Xoa(string maTH)
        {
            DatabaseContext db = new DatabaseContext();
            var th = db.thuongHieus.Where(x => x.MaTH == maTH).FirstOrDefault();
            th.TrangThai = "daxoa";

            //db.thuongHieus.Remove(th);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}