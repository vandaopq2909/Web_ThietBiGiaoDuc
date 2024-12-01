using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;
using PagedList;
using System.Drawing.Printing;
using System.Web.UI;

namespace Web_ThietBiGiaoDuc.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        DatabaseContext db = new DatabaseContext();
        public class SanPhamVM
        {
            public string MaSP { get; set; }
            public string TenSanPham { get; set; }
            public double Gia { get; set; }
            public string img { get; set; }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChiTiet(string masp)
        {
            SanPham sanPham = db.sanPhams.FirstOrDefault(sp => sp.MaSP == masp);
            if (sanPham == null)
            {
               
                return View("Error");
            }
            ViewBag.DaMuaHang = false;
            // Truy vấn danh sách hình ảnh của sản phẩm
            List<HinhAnh> lstha = db.hinhAnhs.Where(x => x.MaSP == masp).ToList();
            ViewBag.lstHA = lstha;
            string tenDangNhap = Request.Cookies["auth"]?.Value;
            if (string.IsNullOrEmpty(tenDangNhap))
            {
                TempData["Message"] = "Vui lòng đăng nhập để thực hiện hành động này.";
             
            }
            else
            {
                var khach = db.khachHangs.FirstOrDefault(u => u.TenDangNhap == tenDangNhap);
                if (khach == null) {

                }
                else
                {
                    string hoTen = khach?.HoTen;
                    bool daMuaHang = db.chiTietDonHangs.Any(ctdh => ctdh.MaSP == masp
                                    && ctdh.DonHang.MaKH == khach.MaKH
                                    && ctdh.DonHang.TrangThai == "Giao hàng thành công");
                    ViewBag.HoTen = hoTen;
                    ViewBag.DaMuaHang = daMuaHang;
                }
           
            }
           


            var lstDanhGia = db.danhGias.Where(x => x.SanPham.MaSP == masp).ToList();

            var lstSPTuongTu = db.sanPhams.Where(x => x.MaLoai == sanPham.MaLoai)
                       .Take(4)  // Lấy tối đa 4 sản phẩm
                       .Select(sp => new SanPhamVM
                       {
                           MaSP = sp.MaSP,
                           TenSanPham = sp.TenSanPham,
                           Gia = sp.Gia,
                           img = sp.HinhAnhs.Select(h => h.TenHinhAnh).FirstOrDefault()
                       }).ToList();


       
            ViewBag.lstDanhGia = lstDanhGia;
            ViewBag.lstSPTuongTu = lstSPTuongTu;

            return View(sanPham);
        }

        [HttpPost]

        public ActionResult ThemDanhGia(DanhGia danhGia)
        {
            DatabaseContext db = new DatabaseContext();

            if (danhGia == null)
            {
                return RedirectToAction("Index");
            }
            danhGia.NgayDanhGia=DateTime.Now;
            // Lưu vào CSDL
            db.danhGias.Add(danhGia);
            db.SaveChanges();

            TempData["Message"] = "Đánh giá của bạn đã được gửi!";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult TatCa(int page = 1, int pageSize = 16)
        {
            List<dynamic> listSP = db.sanPhams
                    .Select(sp => new SanPhamVM
                    {
                        MaSP = sp.MaSP,
                        TenSanPham = sp.TenSanPham,
                        Gia = sp.Gia,
                        img = sp.HinhAnhs.Select(h => h.TenHinhAnh).FirstOrDefault()
                    })
                    .ToList<dynamic>(); 
            // Sử dụng PagedList để phân trang
            var pagedSanPhams = listSP.ToPagedList(page, pageSize);
            ViewBag.listSP = pagedSanPhams; // Lưu danh sách phân trang vào ViewBag

            return View();
        }
        public ActionResult TimKiem(string s = "", int page = 1, int pageSize = 16)
        {
            ViewBag.TimKiem = s;
            if (string.IsNullOrEmpty(s))
            {
                List<dynamic> listSPTK = db.sanPhams
                    .Select(sp => new SanPhamVM
                    {
                        MaSP = sp.MaSP,
                        TenSanPham = sp.TenSanPham,
                        Gia = sp.Gia,
                        img = sp.HinhAnhs.Select(h => h.TenHinhAnh).FirstOrDefault()
                    })
                    .ToList<dynamic>(); 
                // Sử dụng PagedList để phân trang
                var pagedSanPhams = listSPTK.ToPagedList(page, pageSize);
                ViewBag.listTimKiem = pagedSanPhams; // Lưu danh sách phân trang vào ViewBag
                return View();
            }
            else
            {
                List<dynamic> listSPTK = db.sanPhams
                    .Where(x => x.TenSanPham.Contains(s) ||
                            x.MaSP.Contains(s) ||
                            x.LoaiSanPham.TenLoai.Contains(s) ||
                            x.ThuongHieu.TenThuongHieu.Contains(s))
                    .Select(sp => new SanPhamVM
                    {
                        MaSP = sp.MaSP,
                        TenSanPham = sp.TenSanPham,
                        Gia = sp.Gia,
                        img = sp.HinhAnhs.Select(h => h.TenHinhAnh).FirstOrDefault()
                    })
                    .ToList<dynamic>();

                // Sử dụng PagedList để phân trang
                var pagedSanPhams = listSPTK.ToPagedList(page, pageSize);
                ViewBag.listTimKiem = pagedSanPhams; // Lưu danh sách phân trang vào ViewBag
            }
            return View();
        }
        public ActionResult SanPhamTheoDanhMuc(string maDM, int page = 1, int pageSize = 16)
        {
            ViewBag.TenDM = db.danhMucs.Where(x=> x.MaDM==maDM).Select(x => x.TenDanhMuc).FirstOrDefault();
            ViewBag.MaDM = maDM;
            if (string.IsNullOrEmpty(maDM) || maDM == "")
            {          
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<dynamic> listSPTK = db.sanPhams
                    .Where(x => x.LoaiSanPham.DanhMuc.MaDM == maDM)
                    .Select(sp => new SanPhamVM
                    {
                        MaSP = sp.MaSP,
                        TenSanPham = sp.TenSanPham,
                        Gia = sp.Gia,
                        img = sp.HinhAnhs.Select(h => h.TenHinhAnh).FirstOrDefault()
                    })
                    .ToList<dynamic>();

                // Sử dụng PagedList để phân trang
                var pagedSanPhams = listSPTK.ToPagedList(page, pageSize);
                ViewBag.listSPTheoDM = pagedSanPhams; // Lưu danh sách phân trang vào ViewBag
            }
            return View();
        }
        public ActionResult SanPhamTheoLoai(string maLoai, int page = 1, int pageSize = 16)
        {
            ViewBag.TenLoai = db.loaiSanPhams.Where(x => x.MaLoai == maLoai).Select(x => x.TenLoai).FirstOrDefault();
            ViewBag.MaLoai = maLoai;
            if (string.IsNullOrEmpty(maLoai) || maLoai == "")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<dynamic> listSPTK = db.sanPhams
                    .Where(x => x.LoaiSanPham.MaLoai == maLoai)
                    .Select(sp => new SanPhamVM
                    {
                        MaSP = sp.MaSP,
                        TenSanPham = sp.TenSanPham,
                        Gia = sp.Gia,
                        img = sp.HinhAnhs.Select(h => h.TenHinhAnh).FirstOrDefault()
                    })
                    .ToList<dynamic>();

                // Sử dụng PagedList để phân trang
                var pagedSanPhams = listSPTK.ToPagedList(page, pageSize);
                ViewBag.listSPTheoLoai = pagedSanPhams; // Lưu danh sách phân trang vào ViewBag
            }
            return View();
        }
        [HttpGet]
        public JsonResult GetTiLeGiamGia(string maSP)
        {
            // Lấy mã khuyến mãi dựa trên mã sản phẩm
            string maKM = db.apDungKhuyenMais
                           .Where(x => x.MaSP == maSP)
                           .Select(x => x.MaKM)
                           .FirstOrDefault();

            // Nếu không có mã khuyến mãi, trả về giá trị mặc định
            if (string.IsNullOrEmpty(maKM))
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            // Lấy tỷ lệ giảm giá dựa trên mã khuyến mãi
            double? tileGiamGia = db.khuyenMais
                                    .Where(x => x.MaKM == maKM)
                                    .Select(x => x.TiLeGiamGia)
                                    .FirstOrDefault();

            // Nếu không tìm thấy tỷ lệ giảm giá, trả về giá trị mặc định
            if (!tileGiamGia.HasValue)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            // Trả về tỷ lệ giảm giá nếu hợp lệ
            return Json(tileGiamGia.Value, JsonRequestBehavior.AllowGet);
        }
    }
}