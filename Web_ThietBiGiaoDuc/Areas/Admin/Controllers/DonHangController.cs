using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        // GET: Admin/DonHang
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();
            var listDH = db.donHangs.OrderByDescending(x=>x.NgayDatHang).ToList();
            return View(listDH);
        }
        public ActionResult Them() 
        { 
            return View(); 
        }
        public ActionResult Sua() { return View(); }
        public ActionResult ChiTiet() { return View(); }
    }
}