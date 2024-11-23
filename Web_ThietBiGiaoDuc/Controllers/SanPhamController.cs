using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        DatabaseContext db = new DatabaseContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChiTiet(string masp)
        {
            SanPham sanPham = db.sanPhams.FirstOrDefault(sp => sp.MaSP == masp);

            //List<SANPHAMCACANH> listsp = db.SANPHAMCACANHs.Where(x => x.LoaiCaID == sp.LoaiCaID)
            //                                                .Take(4)
            //                                                .ToList();
            //ViewBag.listsp = listsp;
            List<HinhAnh> lstha = db.hinhAnhs.Where(x => x.MaSP == masp).ToList();
            ViewBag.lstHA = lstha;

            return View(sanPham);
        }
    }
}