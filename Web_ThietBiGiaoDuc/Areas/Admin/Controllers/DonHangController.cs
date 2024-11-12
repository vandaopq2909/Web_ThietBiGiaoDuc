using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        // GET: Admin/DonHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Them() { return View(); }
        public ActionResult Sua() { return View(); }
        public ActionResult ChiTiet() { return View(); }
    }
}