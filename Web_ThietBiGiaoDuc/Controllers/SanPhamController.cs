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

            //List<SANPHAMCACANH> listsp = db.SANPHAMCACANHs.Where(x => x.LoaiCaID == sp.LoaiCaID)
            //                                                .Take(4)
            //                                                .ToList();
            //ViewBag.listsp = listsp;
            List<HinhAnh> lstha = db.hinhAnhs.Where(x => x.MaSP == masp).ToList();
            ViewBag.lstHA = lstha;
            ViewBag.lstSPTuongTu = db.sanPhams.Where(x=>x.MaLoai==sanPham.MaLoai)
                   .Take(4)    // Lấy tối đa 5 sản phẩm
                   .Select(sp => new SanPhamVM
                   {
                       MaSP = sp.MaSP,
                       TenSanPham = sp.TenSanPham,
                       Gia = sp.Gia,
                       img = sp.HinhAnhs.Select(h => h.TenHinhAnh).FirstOrDefault()
                   }).ToList();
            return View(sanPham);
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

    }
}