using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Web_ThietBiGiaoDuc.Models;
using static Web_ThietBiGiaoDuc.Controllers.GioHangController;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        // GET: Admin/DonHang
        public ActionResult Index(string trangthai = "", int page = 1, int pageSize = 8)
        {
            DatabaseContext db = new DatabaseContext();

            var listDH = db.donHangs.AsQueryable();

            if (!string.IsNullOrEmpty(trangthai))
            {
                listDH = listDH.Where(x => x.TrangThai == trangthai);
            }

            var pagedDonHang = listDH.OrderByDescending(x => x.NgayDatHang).ToPagedList(page, pageSize);

            ViewBag.lstDonHang = pagedDonHang;
            ViewBag.SelectedStatus = trangthai; // Lưu trạng thái đã chọn
            return View();
        }

        public ActionResult Them() 
        { 
            return View(); 
        }
        [HttpGet]
        public ActionResult Sua(string madh) 
        {
            DatabaseContext db = new DatabaseContext();
            if (madh == null)
            {
                return RedirectToAction("Index", "DonHang");
            }
            var dh = db.donHangs.Where(x => x.MaDH == madh).FirstOrDefault();

            var lstCTDH = db.chiTietDonHangs
                  .Where(ct => ct.MaDH == madh)
                  .Select(ct => new ChiTietDonHangViewModel
                  {
                      MaSP = ct.MaSP,
                      TenSanPham = db.sanPhams
                                    .Where(sp => sp.MaSP == ct.MaSP)
                                    .Select(sp => sp.TenSanPham)
                                    .FirstOrDefault(),
                      SoLuong = ct.SoLuong,
                      Gia = ct.Gia,
                      TongTien = ct.TongTien,
                  })
                  .ToList();
            ViewBag.SelectedStatus = dh.TrangThai;
            ViewBag.lstCTDH = lstCTDH;
            return View(dh);
        }
        [HttpPost]
        public ActionResult Sua(DonHang dh)
        {
            if (dh == null)
            {
                return RedirectToAction("Index", "DonHang");
            }

            DatabaseContext db = new DatabaseContext();
            var donHang = db.donHangs.Where(x => x.MaDH == dh.MaDH).FirstOrDefault();

            donHang.DiaChiGiaoHang = dh.DiaChiGiaoHang; 
            donHang.TrangThai = dh.TrangThai;

            db.SaveChanges();

            return RedirectToAction("Index", "DonHang");
        }
        [HttpPost]
        public ActionResult Xoa(string madh)
        {
            DatabaseContext db = new DatabaseContext();
            var donHang = db.donHangs.FirstOrDefault(x => x.MaDH == madh);

            // Kiểm tra đơn hàng có tồn tại hay không
            if (donHang == null)
            {
                return HttpNotFound("Đơn hàng không tồn tại.");
            }

            var chiTietDonHangLienQuan = db.chiTietDonHangs.Where(ct => ct.MaDH == madh).ToList();
            if (chiTietDonHangLienQuan.Any())
            {
                db.chiTietDonHangs.RemoveRange(chiTietDonHangLienQuan);
            }
            db.donHangs.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ChiTiet() { return View(); }
    }
}