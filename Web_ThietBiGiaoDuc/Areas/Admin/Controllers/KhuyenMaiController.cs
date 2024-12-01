using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class KhuyenMaiController : Controller
    {
        // GET: Admin/KhuyenMai
        public ActionResult Index(string search = "", int page = 1, int pageSize = 10)
        {
            DatabaseContext db = new DatabaseContext();
            string tenDangNhap = Request.Cookies["auth"]?.Value;
            var nv = db.nhanViens.Where(x => x.TenDangNhap == tenDangNhap).FirstOrDefault();
            if (nv == null)
            {
                return RedirectToAction("DangNhap", "NhanVien");
            }
            var listKM = db.khuyenMais.AsQueryable();

            // Lọc theo từ khóa tìm kiếm nếu có
            if (!string.IsNullOrEmpty(search))
            {
                listKM = listKM.Where(km => km.TenKM.Contains(search));
            }

            // Áp dụng phân trang
            var pagedkhuyenMais = listKM.OrderBy(sp => sp.MaKM).ToPagedList(page, pageSize);
            return View(pagedkhuyenMais);
        }
        // Action Search: Xử lý AJAX tìm kiếm và phân trang
        [HttpGet]
        public JsonResult TimKiem(string search = "", int page = 1, int pageSize = 10)
        {
            DatabaseContext db = new DatabaseContext();
            var listKM = db.khuyenMais.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                listKM = listKM.Where(km => km.TenKM.Contains(search) ||
                                            km.MaKM.Contains(search));
            }

            var pagedkhuyenMais = listKM.OrderBy(km => km.MaKM).ToPagedList(page, pageSize);

            var result = new
            {
                Data = pagedkhuyenMais.Select(km => new
                {
                    km.MaKM,
                    km.TenKM,
                    km.TiLeGiamGia,
                    km.MoTa,
                    km.TrangThai
                }).ToList(),
                TotalPages = pagedkhuyenMais.PageCount,
                CurrentPage = pagedkhuyenMais.PageNumber
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Them()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Them(KhuyenMai km)
        {
            DatabaseContext db = new DatabaseContext();
            if (km == null)
            {
                //return RedirectToAction("Index");
            }
            else
            {
                db.khuyenMais.Add(km);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Sua(string maKM)
        {
            DatabaseContext db = new DatabaseContext();
            var km = db.khuyenMais.Where(x => x.MaKM == maKM).FirstOrDefault();
            return View(km);
        }
        [HttpPost]
        public ActionResult Sua(KhuyenMai km)
        {
            DatabaseContext db = new DatabaseContext();
            var danhMuc = db.khuyenMais.Where(x => x.MaKM == km.MaKM).FirstOrDefault();

            //update
            danhMuc.TenKM = km.TenKM;
            danhMuc.TiLeGiamGia = km.TiLeGiamGia;
            danhMuc.MoTa = km.MoTa;
            danhMuc.TrangThai = km.TrangThai;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Xoa(string maKM)
        {
            DatabaseContext db = new DatabaseContext();
            var km = db.khuyenMais.Where(x => x.MaKM == maKM).FirstOrDefault();

            db.khuyenMais.Remove(km);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ApDung(string maKM)
        {
            DatabaseContext db = new DatabaseContext();

            // Kiểm tra mã khuyến mãi
            if (string.IsNullOrEmpty(maKM))
            {
                return RedirectToAction("Index");
            }

            // Lấy thông tin khuyến mãi được chọn
            var khuyenMai = db.khuyenMais.FirstOrDefault(x => x.MaKM == maKM);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }

            //Lấy ds sản phẩm có MaKM = makm
            ViewBag.listSPKM = db.sanPhams
            .Where(x => x.ApDungKhuyenMais.Any(adkm => adkm.MaKM == maKM))
            .ToList();
            return View(khuyenMai);
        }
        public ActionResult GetSanPhamList()
        {
            DatabaseContext db = new DatabaseContext();
            var sanPhams = db.sanPhams.Select(sp => new
            {
                sp.MaSP,
                sp.TenSanPham
            }).ToList();

            return Json(sanPhams, JsonRequestBehavior.AllowGet);
        }
        // Phương thức này sẽ được gọi từ AJAX để lấy giá sản phẩm
        [HttpGet]
        public JsonResult GetGiaSanPham(string MaSP)
        {
            DatabaseContext db = new DatabaseContext();
            // Tìm sản phẩm theo MaSP
            var product = db.sanPhams.FirstOrDefault(sp => sp.MaSP == MaSP);

            if (product != null)
            {
                // Trả về thông tin sản phẩm bao gồm giá
                return Json(new { Gia = product.Gia }, JsonRequestBehavior.AllowGet);
            }

            // Nếu không tìm thấy sản phẩm, trả về thông báo lỗi
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult TimKiemSPKM(string search = "", int page = 1, int pageSize = 10)
        {
            DatabaseContext db = new DatabaseContext();
            var listSP = db.sanPhams.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                listSP = listSP.Where(sp => sp.TenSanPham.Contains(search));
            }

            var pagedSanPhams = listSP.OrderBy(sp => sp.MaSP).ToPagedList(page, pageSize);

            var result = new
            {
                Data = pagedSanPhams.Select(sp => new
                {
                    sp.MaSP,
                    sp.TenSanPham,
                    DanhMuc = sp.LoaiSanPham.DanhMuc.TenDanhMuc,
                    Loai = sp.LoaiSanPham.TenLoai,
                    Gia = sp.Gia.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")),
                    TonKho = sp.SoLuongTonKho,
                    sp.TrangThai
                }).ToList(),
                TotalPages = pagedSanPhams.PageCount,
                CurrentPage = pagedSanPhams.PageNumber
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ThemSanPhamApDung(string MaKM, string NgayBD, string NgayKT, List<SanPham> products)
        {
            DatabaseContext db = new DatabaseContext();

            // Kiểm tra và xử lý các sản phẩm
            foreach (var product in products)
            {
                var apDungKhuyenMai = new ApDungKhuyenMai
                {
                    MaKM = MaKM,   // Nhận MaKM từ client
                    NgayBD = DateTime.Parse(NgayBD), // Chuyển đổi NgàyBD thành kiểu DateTime
                    NgayKT = DateTime.Parse(NgayKT), // Chuyển đổi NgàyKT thành kiểu DateTime
                    MaSP = product.MaSP
                };

                db.apDungKhuyenMais.Add(apDungKhuyenMai); // Thêm vào bảng ApDungKhuyenMai
            }

            // Lưu thay đổi vào database
            db.SaveChanges();

            return Json(new { success = true });
        }

    }
}