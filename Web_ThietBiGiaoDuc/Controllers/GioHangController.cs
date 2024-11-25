﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Controllers
{
    public class GioHangController : Controller
    {
        public class ItemSanPham
        {
            public string MaSP { get; set; }
            public string TenSP { get; set; }
            public int SoLuong { get; set; }
            public double Gia { get; set; }
            public string Img { get; set; }  

            // Tổng giá trị của sản phẩm (số lượng * đơn giá)
            public double TongTien => SoLuong * Gia;
        }


        // GET: GioHang
        public ActionResult Index()
        {
            ViewBag.Cart = LoadGioHang();
            return View();
        }
        public ActionResult ThemVaoGio(string maSP, int soLuong = 1)
        {
            DatabaseContext db = new DatabaseContext();

            // Lấy giỏ hàng từ session (nếu có), nếu không có thì khởi tạo một giỏ hàng mới
            List<ItemSanPham> cart = Session["Cart"] as List<ItemSanPham>;
            if (cart == null)
            {
                cart = new List<ItemSanPham>();
            }

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
            ItemSanPham existingItem = cart.FirstOrDefault(x => x.MaSP == maSP);
            if (existingItem != null)
            {
                // Nếu sản phẩm đã tồn tại, cập nhật số lượng
                existingItem.SoLuong += soLuong;
            }
            else
            {
                // Lấy thông tin sản phẩm từ database
                var sp = db.sanPhams
                           .Include("HinhAnhs")  // Nếu bạn đang sử dụng Lazy Loading, có thể cần Include để tải dữ liệu hình ảnh
                           .FirstOrDefault(x => x.MaSP == maSP);

                // Kiểm tra nếu sản phẩm tồn tại trong database
                if (sp != null)
                {
                    // Tạo đối tượng ItemSanPham mới
                    var newItem = new ItemSanPham
                    {
                        MaSP = sp.MaSP,
                        TenSP = sp.TenSanPham,
                        Gia = sp.Gia,
                        Img = sp.HinhAnhs.Select(h => h.TenHinhAnh).FirstOrDefault(), // Lấy hình ảnh đầu tiên
                        SoLuong = soLuong
                    };

                    // Nếu sản phẩm chưa tồn tại trong giỏ hàng, thêm mới vào giỏ hàng
                    cart.Add(newItem);
                }
                else
                {
                    // Nếu không tìm thấy sản phẩm trong database, xử lý thông báo lỗi (nếu cần)
                    return RedirectToAction("ErrorPage"); // Ví dụ: điều hướng đến trang lỗi
                }
            }

            // Lưu giỏ hàng lại vào session
            Session["Cart"] = cart;
            ViewBag.Cart = Session["Cart"];

            return RedirectToAction("Index");
        }

        public List<ItemSanPham> LoadGioHang()
        {
            // Lấy giỏ hàng từ session
            List<ItemSanPham> cart = Session["Cart"] as List<ItemSanPham>;
            if (cart == null)
            {
                // Nếu giỏ hàng chưa có, khởi tạo một danh sách trống
                Session["Cart"] = new List<ItemSanPham>();            
            }
            ViewBag.cart = Session["Cart"];
            return cart;
        }
        public void XoaSPKhoiGioHang(string maSP)
        {
            List<ItemSanPham> cart = Session["Cart"] as List<ItemSanPham>;
            if (cart != null)
            {
                // Tìm sản phẩm cần xóa
                ItemSanPham itemToRemove = cart.FirstOrDefault(x => x.MaSP == maSP);
                if (itemToRemove != null)
                {
                    // Xóa sản phẩm khỏi giỏ hàng
                    cart.Remove(itemToRemove);
                }

                // Lưu lại giỏ hàng vào session
                Session["Cart"] = cart;
            }
        }
        public void CapNhatGioHang(string maSP, int soLuong)
        {
            List<ItemSanPham> cart = Session["Cart"] as List<ItemSanPham>;
            if (cart != null)
            {
                // Tìm sản phẩm cần cập nhật số lượng
                ItemSanPham existingItem = cart.FirstOrDefault(x => x.MaSP == maSP);
                if (existingItem != null)
                {
                    // Cập nhật số lượng sản phẩm
                    existingItem.SoLuong = soLuong;
                }

                // Lưu lại giỏ hàng vào session
                Session["Cart"] = cart;
            }
        }

    }
}