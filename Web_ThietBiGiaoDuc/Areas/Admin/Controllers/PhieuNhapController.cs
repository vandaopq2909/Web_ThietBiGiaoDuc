using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class PhieuNhapController : Controller
    {
        // GET: Admin/PhieuNhap
        public ActionResult Index(string trangthai = "", int page = 1, int pageSize = 8)
        {
            DatabaseContext db = new DatabaseContext();
            string tenDangNhap = Request.Cookies["auth"]?.Value;
            var nv = db.nhanViens.Where(x => x.TenDangNhap == tenDangNhap).FirstOrDefault();
            if (nv == null)
            {
                return RedirectToAction("DangNhap", "NhanVien");
            }

            var listPN = db.phieuNhaps.AsQueryable();

            if (!string.IsNullOrEmpty(trangthai))
            {
                listPN = listPN.Where(x => x.TrangThai == trangthai);
            }
            var pagedPhieuNhap = listPN.OrderByDescending(x => x.NgayNhapHang).ToPagedList(page, pageSize);

            ViewBag.lstPhieuNhap = pagedPhieuNhap;
            ViewBag.SelectedStatus = trangthai; // Lưu trạng thái đã chọn
            return View();
        }
        [HttpGet]
        public ActionResult Them()
        {
            DatabaseContext db=new DatabaseContext();
            var lstSanPham=db.sanPhams.ToList();
            var lstNCC=db.nhaCungCaps.ToList();
            string tenDangNhap = Request.Cookies["auth"]?.Value;

            if (string.IsNullOrEmpty(tenDangNhap))
            {
                // Xử lý khi cookie không tồn tại
                TempData["Error"] = "Không tìm thấy thông tin đăng nhập. Vui lòng đăng nhập lại.";
                return RedirectToAction("DangNhap", "NhanVien");
            }

            var nhanVien = db.nhanViens.FirstOrDefault(x => x.TenDangNhap == tenDangNhap);
            ViewBag.HoTen=nhanVien.HoTen;

            ViewBag.lstSanPham = lstSanPham;
            ViewBag.lstNCC = lstNCC;
            return View();
        }
        [HttpPost]
        public ActionResult Them(PhieuNhap viewModel)
        {
            DatabaseContext db = new DatabaseContext();
            if (ModelState.IsValid)
            {
                var phieuNhap = new PhieuNhap
                {
                    MaNV = viewModel.MaNV,
                    MaNCC = viewModel.MaNCC,
                    NgayNhapHang = viewModel.NgayNhapHang,
                    TrangThai = viewModel.TrangThai,
                    ChiTietPhieuNhaps = new List<ChiTietPhieuNhap>()
                };

                foreach (var item in viewModel.ChiTietPhieuNhaps)
                {
                    var chiTiet = new ChiTietPhieuNhap
                    {
                        MaSP = item.MaSP,
                        SoLuong = item.SoLuong,
                        Gia = item.Gia,
                        TongTien = item.TongTien
                    };
                    phieuNhap.ChiTietPhieuNhaps.Add(chiTiet);
                }

                db.phieuNhaps.Add(phieuNhap);
                db.SaveChanges();
                TempData["Success"] = "Phiếu nhập đã được lưu thành công.";
                return RedirectToAction("Index", "PhieuNhap");
            }

            return View(viewModel); 
        }
    }
}