using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: Admin/KhachHang
        public ActionResult Index()
        {
            //test
            DatabaseContext db = new DatabaseContext();
            var t = db.khachHangs.ToList();
            return View();
        }
    }
}