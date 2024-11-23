using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();
            return View();
        }
        public ActionResult DangNhap()
        {

            return View();
        }
    }
}