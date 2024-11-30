using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
            DatabaseContext db = new DatabaseContext();
            string tenDangNhap = Request.Cookies["auth"]?.Value;
            var nv = db.nhanViens.Where(x => x.TenDangNhap == tenDangNhap).FirstOrDefault();
            if (nv == null)
            {
                return RedirectToAction("DangNhap", "NhanVien");
            }
            var listSP = db.sanPhams.ToList();
            return View(listSP);
        }
        [HttpGet]
        public JsonResult GetListLoaiSP(string maDM)
        {
            DatabaseContext db = new DatabaseContext();
            var listLoaiSP = db.loaiSanPhams
                             .Where(p => p.MaDM == maDM) // Lọc theo danh mục
                             .Select(p => new { Key = p.MaLoai, Value = p.TenLoai }) // Lấy Mã và Tên của loại sản phẩm
                             .ToList();

            return Json(listLoaiSP, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Them()
        {
            DatabaseContext db = new DatabaseContext();
            ViewBag.listDM = db.danhMucs.Where(x => x.TrangThai == "hoatdong").ToList();
            ViewBag.listLoaiSP = db.loaiSanPhams.Where(x => x.TrangThai == "hoatdong").ToList();
            ViewBag.listTH = db.thuongHieus.Where(x => x.TrangThai == "hoatdong").ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Them(SanPham sp, IEnumerable<HttpPostedFileBase> HinhAnh)
        {
            DatabaseContext db = new DatabaseContext();

            if (sp == null)
            {
                //return RedirectToAction("Index");
            }
            else
            {
                db.sanPhams.Add(sp);
                db.SaveChanges();

                // Nếu có hình ảnh được gửi lên
                if (HinhAnh != null && HinhAnh.Any())
                {
                    foreach (var file in HinhAnh)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            // Tạo tên hình ảnh duy nhất và lưu vào thư mục
                            var fileName = Path.GetFileName(file.FileName);
                            var filePath = Path.Combine(Server.MapPath("~/Images/SanPham/"), fileName);
                            file.SaveAs(filePath); // Lưu hình ảnh vào thư mục đã chỉ định

                            // Tạo bản ghi hình ảnh mới trong bảng HinhAnh
                            HinhAnh image = new HinhAnh
                            {
                                TenHinhAnh = fileName,
                                MaSP = sp.MaSP
                            };

                            db.hinhAnhs.Add(image);
                        }
                    }
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult Sua(string maSP)
        {
            var listMauSac = new List<SelectListItem> {
                new SelectListItem { Value = "do", Text = "Đỏ" },
                new SelectListItem { Value = "cam", Text = "Cam" },
                new SelectListItem { Value = "vang", Text = "Vàng" },
                new SelectListItem { Value = "luc", Text = "Lục" },
                new SelectListItem { Value = "xanh-duong", Text = "Xanh Dương" },
                new SelectListItem { Value = "tim", Text = "Tím" },
                new SelectListItem { Value = "do-cam", Text = "Đỏ Cam" },
                new SelectListItem { Value = "vang-cam", Text = "Vàng Cam" },
                new SelectListItem { Value = "vang-luc", Text = "Vàng Lục" },
                new SelectListItem { Value = "xanh-luc", Text = "Xanh Lục" },
                new SelectListItem { Value = "xanh-tim", Text = "Xanh Tím" },
                new SelectListItem { Value = "tim-hong", Text = "Tím Hồng" },
                new SelectListItem { Value = "do-hong", Text = "Đỏ Hồng" },
                new SelectListItem { Value = "hong", Text = "Hồng" },
                new SelectListItem { Value = "hong-dam", Text = "Hồng Đậm" },
                new SelectListItem { Value = "nau", Text = "Nâu" },
                new SelectListItem { Value = "xam-nhat", Text = "Xám Nhạt" },
                new SelectListItem { Value = "xam", Text = "Xám" },
                new SelectListItem { Value = "luc-olive", Text = "Lục Olive" },
                new SelectListItem { Value = "luc-nhat", Text = "Lục Nhạt" },
                new SelectListItem { Value = "xanh-nhat", Text = "Xanh Nhạt" },
                new SelectListItem { Value = "tim-dam", Text = "Tím Đậm" },
                new SelectListItem { Value = "be", Text = "Be" },
                new SelectListItem { Value = "den", Text = "Đen" },
                new SelectListItem { Value = "trang", Text = "Trắng" }
            };
            var listDVT = new List<SelectListItem> {
                new SelectListItem { Value = "cai", Text = "Cái" },
                new SelectListItem { Value = "bo", Text = "Bộ" },
                new SelectListItem { Value = "chiec", Text = "Chiếc" },
                new SelectListItem { Value = "hop", Text = "Hộp" },
                new SelectListItem { Value = "goi", Text = "Gói" },
                new SelectListItem { Value = "cuon", Text = "Cuốn" },
                new SelectListItem { Value = "to", Text = "Tờ" },
                new SelectListItem { Value = "thung", Text = "Thùng" },
                new SelectListItem { Value = "lo", Text = "Lọ" },
                new SelectListItem { Value = "chai", Text = "Chai" },
                new SelectListItem { Value = "met", Text = "Mét" },
                new SelectListItem { Value = "cap", Text = "Cặp" },
                new SelectListItem { Value = "kg", Text = "Kg" },
                new SelectListItem { Value = "lit", Text = "Lít" },
                new SelectListItem { Value = "ban", Text = "Bản" }
            };
            DatabaseContext db = new DatabaseContext();
            var sp = db.sanPhams.Where(x => x.MaSP == maSP).FirstOrDefault();

            ViewBag.listDM = db.danhMucs.Where(x => x.TrangThai == "hoatdong").ToList();
            ViewBag.listLoaiSP = db.loaiSanPhams.Where(x => x.TrangThai == "hoatdong").ToList();
            ViewBag.listTH = db.thuongHieus.Where(x => x.TrangThai == "hoatdong").ToList();
            ViewBag.listHA = db.hinhAnhs.Where(x => x.MaSP == maSP).ToList();
            ViewBag.listMauSac = listMauSac;
            ViewBag.listDVT = listDVT;

            return View(sp);
        }
        [HttpPost]
        public ActionResult Sua(SanPham sp, IEnumerable<HttpPostedFileBase> HinhAnh)
        {
            DatabaseContext db = new DatabaseContext();

            if (sp == null)
            {
                //return RedirectToAction("Index");
            }
            else
            {
                var sanPham = db.sanPhams.Where(x => x.MaSP == sp.MaSP).FirstOrDefault();
                sanPham.TenSanPham = sp.TenSanPham;
                sanPham.MauSac = sp.MauSac;
                sanPham.KichThuoc = sp.KichThuoc;
                sanPham.MaLoai = sp.MaLoai;
                sanPham.MaTH = sp.MaTH;
                sanPham.CachDongGoi = sp.CachDongGoi;
                sanPham.DonViTinh = sp.DonViTinh;
                sanPham.TinhTrangSanPham = sp.TinhTrangSanPham;
                sanPham.Gia = sp.Gia;
                sanPham.SoLuongTonKho = sp.SoLuongTonKho;
                sanPham.TrangThai = sp.TrangThai;
                sanPham.MoTa = sp.MoTa;

                db.SaveChanges();

                // Nếu có hình ảnh được gửi lên
                if (HinhAnh != null && HinhAnh.Any())
                {
                    var itemsToRemove = db.hinhAnhs.Where(h => h.MaSP == sp.MaSP).ToList();
                    if (itemsToRemove.Any())
                    {
                        db.hinhAnhs.RemoveRange(itemsToRemove);  // Xóa tất cả đối tượng tìm thấy
                        db.SaveChanges();
                    }
                    foreach (var file in HinhAnh)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            // Tạo tên hình ảnh duy nhất và lưu vào thư mục
                            var fileName = Path.GetFileName(file.FileName);
                            var filePath = Path.Combine(Server.MapPath("~/Images/SanPham/"), fileName);
                            file.SaveAs(filePath); // Lưu hình ảnh vào thư mục đã chỉ định

                            // Tạo bản ghi hình ảnh mới trong bảng HinhAnh
                            HinhAnh image = new HinhAnh
                            {
                                TenHinhAnh = fileName,
                                MaSP = sp.MaSP
                            };

                            db.hinhAnhs.Add(image);
                        }
                    }
                    db.SaveChanges();
                }                              
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Xoa(string maSP)
        {
            DatabaseContext db = new DatabaseContext();
            var sp = db.sanPhams.Where(x => x.MaSP == maSP).FirstOrDefault();
            sp.TrangThai = "daxoa";

            //db.sanPhams.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}