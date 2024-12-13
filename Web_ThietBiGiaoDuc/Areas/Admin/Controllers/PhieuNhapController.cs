using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            if (nhanVien == null)
            {
                return RedirectToAction("DangNhap", "NhanVien");
            }
            else
            {
                ViewBag.HoTen = nhanVien.HoTen;
            }

            ViewBag.NgayNhapHang = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
            ViewBag.lstSanPham = lstSanPham;
            ViewBag.lstNCC = lstNCC;
            return View();
        }
        [HttpPost]
        public ActionResult Them(PhieuNhap viewModel)
        {
            DatabaseContext db = new DatabaseContext();
            string tenDangNhap = Request.Cookies["auth"]?.Value;
            string maNV = db.nhanViens.FirstOrDefault(x => x.TenDangNhap == tenDangNhap).MaNV;
            if (ModelState.IsValid)
            {
                var phieuNhap = new PhieuNhap
                {
                    MaNV = maNV,
                    MaNCC = viewModel.MaNCC,
                    NgayNhapHang = DateTime.Now,
                    TrangThai = viewModel.TrangThai,
                    ChiTietPhieuNhaps = new List<ChiTietPhieuNhap>()
                };
                double TongTien = 0;
                int TongSoLuong=0;
                foreach (var item in viewModel.ChiTietPhieuNhaps)
                {
                    // Kiểm tra nếu sản phẩm đã có trong ChiTietPhieuNhaps
                    var existingProduct = phieuNhap.ChiTietPhieuNhaps
                                                  .FirstOrDefault(x => x.MaSP == item.MaSP);

                    if (existingProduct != null)
                    {
                        // Nếu đã có sản phẩm, cộng dồn số lượng và tính lại tổng tiền
                        existingProduct.SoLuong += item.SoLuong;
                        existingProduct.TongTien = existingProduct.SoLuong * existingProduct.Gia;
                    }
                    else
                    {
                        // Nếu chưa có sản phẩm, thêm sản phẩm mới vào danh sách
                        var chiTiet = new ChiTietPhieuNhap
                        {
                            MaSP = item.MaSP,
                            SoLuong = item.SoLuong,
                            Gia = item.Gia,
                            TongTien = item.TongTien
                        };
                        TongTien += chiTiet.TongTien;
                        TongSoLuong+= chiTiet.SoLuong;
                        phieuNhap.ChiTietPhieuNhaps.Add(chiTiet);
                       
                    }
                }
                phieuNhap.TongSoLuong = TongSoLuong;
                phieuNhap.TongTien = TongTien;


                db.phieuNhaps.Add(phieuNhap);
                db.SaveChanges();
                TempData["Success"] = "Phiếu nhập đã được lưu thành công.";
                return RedirectToAction("Index", "PhieuNhap");
            }

            return View(viewModel); 
        }
        [HttpGet]
        public ActionResult Sua(string mapn)
        {
            DatabaseContext db = new DatabaseContext();
            var phieuNhap = db.phieuNhaps.Include(p => p.ChiTietPhieuNhaps).FirstOrDefault(p => p.MaPN == mapn);

            if (phieuNhap == null)
            {
                return HttpNotFound(); 
            }

            ViewBag.MaPN = mapn;
            ViewBag.lstNCC = db.nhaCungCaps.ToList();
            ViewBag.lstSanPham = db.sanPhams.ToList();
            ViewBag.HoTen = phieuNhap.NhanVien.HoTen;
      
            ViewBag.SelectedStatus = phieuNhap.TrangThai;

            return View(phieuNhap);
        }
        [HttpPost]
        public ActionResult Sua( PhieuNhap model)
        {
            DatabaseContext db = new DatabaseContext();
            var phieuNhap = db.phieuNhaps
                .Include(p => p.ChiTietPhieuNhaps)
                .FirstOrDefault(x => x.MaPN == model.MaPN);

            if (phieuNhap == null)
            {
                return HttpNotFound();
            }

            phieuNhap.MaNCC = model.MaNCC;
            phieuNhap.NgayNhapHang = DateTime.Now;
            phieuNhap.TrangThai = model.TrangThai;

            if (model.TrangThai == "Đã nhập hàng")
            {
                foreach (var chiTiet in phieuNhap.ChiTietPhieuNhaps)
                {
                    var sanPham = db.sanPhams.FirstOrDefault(sp => sp.MaSP == chiTiet.MaSP);
                    if (sanPham != null)
                    {
                        sanPham.SoLuongTonKho += chiTiet.SoLuong; 
                    }
                }
            }


            db.SaveChanges();

            return RedirectToAction("Index", "PhieuNhap");
        }
        [HttpPost]
        public ActionResult Xoa(string maPN)
        {

            var db = new DatabaseContext();
            var phieuNhap = db.phieuNhaps
                .Include(p => p.ChiTietPhieuNhaps)
                .FirstOrDefault(p => p.MaPN == maPN);

            if (phieuNhap == null)
            {
                return HttpNotFound("Không tìm thấy phiếu nhập");
            }
            db.chiTietPhieuNhaps.RemoveRange(phieuNhap.ChiTietPhieuNhaps);

            db.phieuNhaps.Remove(phieuNhap);
            db.SaveChanges();

            TempData["Message"] = "Xóa phiếu nhập thành công!";
            return RedirectToAction("Index", "PhieuNhap");
        }

    }

}