using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Helpers;
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
        public class GioHangData 
        {
            public int TongSL { get; set; } = 0;
            public double TongTien { get; set; } = 0;
        }

        // GET: GioHang
        public ActionResult Index()
        {
            var cart = LoadGioHang();
            int tongSLSP = TinhTongSoLuongSP();
            double tongTienTatCaSP = TinhTongTienTatCaSP();
            ViewBag.Cart = cart;
            ViewBag.TongSLSP = tongSLSP;
            ViewBag.tongTienTatCaSP = tongTienTatCaSP;
          
            if (Request.Cookies["makh"] == null) {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public void ThemVaoGio(string maSP, int soLuong = 1)
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
                    return; // Ví dụ: điều hướng đến trang lỗi
                }
            }

            // Lưu giỏ hàng lại vào session
            Session["Cart"] = cart;
            ViewBag.Cart = Session["Cart"];

            return;
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
        public double TinhTongTienTatCaSP()
        {
            // Lấy giỏ hàng từ session
            List<ItemSanPham> cart = Session["Cart"] as List<ItemSanPham>;
            double tongTien = 0;
            if (cart != null)
            {              
                foreach(var item in cart)
                {
                    tongTien += item.TongTien;
                }
            }
            return tongTien;
        }
        [HttpPost]
        public ActionResult XoaSPKhoiGioHang(string maSP)
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
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult CapNhatSPGioHang(string maSP, int soLuong)
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
                ViewBag.Cart = Session["Cart"];
            }
            return RedirectToAction("Index");
        }
        public ActionResult LamTrongGioHang()
        {
            // Lấy giỏ hàng từ session
            List<ItemSanPham> cart = Session["Cart"] as List<ItemSanPham>;

            if (cart != null)
            {
                // Xóa tất cả các sản phẩm trong giỏ hàng
                cart.Clear();

                Session["Cart"] = cart;
                ViewBag.Cart = Session["Cart"];
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public JsonResult LayThongTinNguoiDung(string maKH)
        {
            DatabaseContext db = new DatabaseContext();
            var userInfo = db.khachHangs.Where(x=> x.MaKH == maKH).FirstOrDefault();

            // Kiểm tra xem userInfo có dữ liệu không
            if (userInfo != null)
            {
                return Json(new
                {
                    phone_number = userInfo.SDT ?? "",
                    delivery_address = userInfo.DiaChi ?? ""
                }, JsonRequestBehavior.AllowGet);
            }

            // Nếu không tìm thấy thông tin, trả về dữ liệu trống
            return Json(new
            {
                phone_number = "",
                delivery_address = ""
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ThanhToan(string maKH, string phone_number, string delivery_address)
        {
            DatabaseContext db = new DatabaseContext();
            try
            {
                // Lấy giỏ hàng từ session
                List<ItemSanPham> cart = Session["Cart"] as List<ItemSanPham>;

                //lấy thông tin khách hàng
                var CustomerInfo = db.khachHangs.Where(x=> x.MaKH == maKH).FirstOrDefault();

                double tongTien = 0;
                if (cart != null)
                {
                    foreach (var item in cart)
                    {
                        tongTien += item.TongTien;
                    }
                }

                DonHang donHang = new DonHang
                {
                    MaKH = maKH,
                    NgayDatHang = DateTimeOffset.Now,
                    TongSoLuong = TinhTongSoLuongSP(),
                    TongTien = tongTien,
                    DiaChiGiaoHang = delivery_address,
                    TrangThai = "Đang xử lý"
                };

                if (tongTien > 0) {
                    db.donHangs.Add(donHang);
                    db.SaveChanges();

                    ThemCTDonHang(donHang.MaDH);

                    var listSP = new List<ItemSanPham>(); 
                    foreach (var item in cart)
                    {
                        listSP.Add(new ItemSanPham
                        {
                            TenSP = item.TenSP,
                            SoLuong = item.SoLuong,
                            Gia = item.Gia,
                            Img = item.Img
                        });
                    }
                    Order order = new Order
                    {
                        CustomerName = CustomerInfo.HoTen,
                        CustomerEmail = CustomerInfo.Email,
                        Items = listSP,
                        TotalPrice = tongTien
                    };

                    EmailHelper emailHelper = new EmailHelper();
                    emailHelper.SendOrderConfirmationEmail(order);

                    Session["Cart"] = null;
                    ViewBag.Cart = null;

                    // Nếu thanh toán thành công, chuyển hướng đến trang thông báo thành công
                    return RedirectToAction("ThanhToanThanhCong", new {maDH = donHang.MaDH});
                }
                // Xử lý lỗi và trả về thông báo lỗi nếu có
                return RedirectToAction("Index");
            }
            catch
            {
                // Xử lý lỗi và trả về thông báo lỗi nếu có
                return RedirectToAction("Index");
            }
        }

        private void ThemCTDonHang(string maDH)
        {
            DatabaseContext db = new DatabaseContext();
            // Lấy giỏ hàng từ session
            List<ItemSanPham> cart = Session["Cart"] as List<ItemSanPham>;
            if(cart != null && cart.Count > 0)
            {
                foreach (var item in cart)
                {
                    ChiTietDonHang ctdh = new ChiTietDonHang
                    {
                        MaDH = maDH,
                        MaSP = item.MaSP,
                        SoLuong = item.SoLuong,
                        Gia = item.Gia,
                        TongTien = item.TongTien,
                        GhiChu = "",
                        TrangThaiDanhGia = "chuadanhgia"
                    };
                    db.chiTietDonHangs.Add(ctdh);
                    var sp = db.sanPhams.Where(x => x.MaSP == ctdh.MaSP && x.SoLuongTonKho > 0).FirstOrDefault();
                    sp.SoLuongTonKho = sp.SoLuongTonKho - 1;
                }
                db.SaveChanges();
            } 
        }

        private int TinhTongSoLuongSP()
        {

            // Lấy giỏ hàng từ session
            List<ItemSanPham> cart = Session["Cart"] as List<ItemSanPham>;
            int tongSL = 0;
            if (cart != null)
            {
                foreach(var item in cart)
                {
                    tongSL += item.SoLuong;

                }
            }
            return tongSL;
        }
        public ActionResult ThanhToanThanhCong(string maDH)
        {
            DatabaseContext db = new DatabaseContext();
            //load thông tin đơn hàng
            var donHang = db.donHangs.Where(x => x.MaDH == maDH).FirstOrDefault();
            return View(donHang);
        }
        public ActionResult LichSuMuaHang(string makh, int page = 1, int pageSize = 6)
        {
            DatabaseContext db = new DatabaseContext();
            if (makh == null) 
            {
                return RedirectToAction("Index", "Home");
            }
            List<DonHang> lstDonHang = db.donHangs
                .Where(dh => dh.MaKH == makh)
                .OrderByDescending(dh => dh.NgayDatHang)
                .ToList();
            var pagedSanPhams = lstDonHang.ToPagedList(page, pageSize);
            ViewBag.lstDonHang = pagedSanPhams;
            ViewBag.makh = makh;
            return View();
        }
        public class ChiTietDonHangViewModel
        {
            public string MaSP { get; set; }
            public string TenSanPham { get; set; }
            public int SoLuong { get; set; }
            public double Gia { get; set; }
            public double TongTien { get; set; }
        }
        public ActionResult ChiTietDonHang(string madh)
        {
            DatabaseContext db = new DatabaseContext();
            if (madh == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var tongtien = db.donHangs.Where(don=>don.MaDH==madh).FirstOrDefault().TongTien;
            // Lấy danh sách chi tiết đơn hàng kèm thông tin sản phẩm
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
            ViewBag.lstCTDH= lstCTDH;
            ViewBag.madh = madh;
            ViewBag.tongTien = tongtien;
            return View();
        }
        public JsonResult GetGioHangData()
        {
            var gioHangData = new GioHangData()
            {
                TongSL = TinhTongSoLuongSP(),
                TongTien = TinhTongTienTatCaSP()
            };

            // Trả về dữ liệu JSON
            return Json(gioHangData, JsonRequestBehavior.AllowGet);
        }
        public class EmailHelper
        {
            public void SendOrderConfirmationEmail(Order order)
            {
                // Cấu hình thông tin email
                string fromEmail = "thietbigiaoduc.dacn@gmail.com";
                string subject = "Đơn hàng đã đặt thành công";

                // Tạo nội dung email từ hàm GetOrderConfirmationEmailBody
                AlternateView emailBody = GetOrderConfirmationEmailBody(order);

                try
                {
                    // Cấu hình SMTP Client
                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential("thietbigiaoduc.dacn@gmail.com", "mzazrwkdbrugholx"),
                        EnableSsl = true
                    };

                    // Tạo email
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = subject,
                        IsBodyHtml = true // Đảm bảo email sẽ được gửi dưới dạng HTML
                    };

                    // Thêm nội dung HTML vào email
                    mailMessage.AlternateViews.Add(emailBody);

                    // Thêm địa chỉ người nhận
                    mailMessage.To.Add(order.CustomerEmail);

                    // Gửi email
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                    Console.WriteLine("Lỗi gửi email: " + ex.Message);
                }
            }

            private AlternateView GetOrderConfirmationEmailBody(Order order)
            {
                StringBuilder body = new StringBuilder();
                body.Append("<h2>Đơn hàng đã đặt thành công</h2>");
                body.Append("<p>Xin chào " + order.CustomerName + ",</p>");
                body.Append("<p>Cảm ơn bạn đã đặt hàng tại cửa hàng của chúng tôi.</p>");
                body.Append("<h3>Danh sách sản phẩm:</h3>");
                body.Append("<table border='1' cellpadding='5' cellspacing='0'>");
                body.Append("<tr><th>Hình ảnh</th><th>Tên sản phẩm</th><th>Số lượng</th><th>Giá</th></tr>");

                // Thêm LinkedResource vào danh sách ảnh để nhúng inline
                List<LinkedResource> linkedImages = new List<LinkedResource>();

                foreach (ItemSanPham item in order.Items)
                {
                    // Đường dẫn tới hình ảnh trên máy chủ
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images/SanPham", item.Img);

                    string contentId = Guid.NewGuid().ToString(); // Tạo Content-ID duy nhất cho ảnh

                    if (System.IO.File.Exists(imagePath))
                    {
                        // Tạo LinkedResource từ ảnh
                        LinkedResource linkedImage = new LinkedResource(imagePath, "image/jpeg")
                        {
                            ContentId = contentId,
                            TransferEncoding = System.Net.Mime.TransferEncoding.Base64
                        };

                        linkedImages.Add(linkedImage);

                        // Thêm dòng vào bảng email, tham chiếu ảnh qua Content-ID
                        body.Append("<tr>");
                        body.Append($"<td style='padding: 8px;'><img src='cid:{contentId}' alt='{item.TenSP}' style='width: 100px; height: auto;' /></td>");
                        body.Append("<td>" + item.TenSP + "</td>");
                        body.Append("<td>" + item.SoLuong + "</td>");
                        body.Append("<td>" + item.Gia.ToString("N0") + " VNĐ</td>");
                        body.Append("</tr>");
                    }
                    else
                    {
                        body.Append("<tr>");
                        body.Append("<td style='padding: 8px;'>Hình ảnh không tìm thấy</td>");
                        body.Append("<td>" + item.TenSP + "</td>");
                        body.Append("<td>" + item.SoLuong + "</td>");
                        body.Append("<td>" + item.Gia.ToString("N0") + " VNĐ</td>");
                        body.Append("</tr>");
                    }
                }

                body.Append("</table>");
                body.Append("<p>Tổng tiền: " + order.TotalPrice.ToString("N0") + " VNĐ</p>");
                body.Append("<p>Xin cảm ơn và chúc bạn có một ngày tốt lành!</p>");

                // Tạo AlternateView cho nội dung email
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body.ToString(), null, "text/html");

                // Thêm LinkedResource vào AlternateView
                foreach (var linkedImage in linkedImages)
                {
                    htmlView.LinkedResources.Add(linkedImage);
                }

                return htmlView;
            }

        }

        public class Order
        {
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public List<ItemSanPham> Items { get; set; }
            public double TotalPrice { get; set; }
        }

        public class OrderItem
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }
    }
}