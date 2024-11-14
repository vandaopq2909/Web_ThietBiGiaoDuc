using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: Admin/SanPham
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Them()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Them(SanPham sp, IEnumerable<HttpPostedFileBase> HinhAnh)
        {
            return View();
        }
        public ActionResult Sua()
        {
            return View();
        }
    }
}