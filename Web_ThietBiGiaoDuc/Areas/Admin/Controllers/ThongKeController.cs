using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ThietBiGiaoDuc.Models;

namespace Web_ThietBiGiaoDuc.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetThongKeTheo12Thang()
        {
            DatabaseContext db = new DatabaseContext();

            var currentYear = DateTime.Now.Year;

            var monthlyRevenue = db.donHangs
                .Where(x => x.NgayDatHang.Year == currentYear)
                .GroupBy(x => x.NgayDatHang.Month)
                .Select(group => new
                {
                    Month = group.Key,
                    Revenue = group.Sum(x => x.TongTien)
                })
                .ToList();

            // Đảm bảo có đủ 12 tháng
            var fullYearRevenue = Enumerable.Range(1, 12)
                .Select(month => new
                {
                    Month = month,
                    Revenue = monthlyRevenue.FirstOrDefault(m => m.Month == month)?.Revenue ?? 0
                })
                .ToList();

            return Json(fullYearRevenue, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetThongKeTheoNgayTrongThang(int year, int month)
        {
            DatabaseContext db = new DatabaseContext();

            // Truy xuất doanh thu theo ngày
            var dailyRevenue = db.donHangs
                .Where(x => x.NgayDatHang.Year == year && x.NgayDatHang.Month == month)
                .GroupBy(x => x.NgayDatHang.Day)
                .Select(group => new
                {
                    Day = group.Key,
                    Revenue = group.Sum(x => x.TongTien)
                })
                .ToList();

            // Đảm bảo có đủ số ngày trong tháng
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var fullMonthRevenue = Enumerable.Range(1, daysInMonth)
                .Select(day => new
                {
                    Day = day,
                    Revenue = dailyRevenue.FirstOrDefault(d => d.Day == day)?.Revenue ?? 0
                })
                .ToList();

            return Json(fullMonthRevenue, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetThongKeTheoNam()
        {
            DatabaseContext db = new DatabaseContext();
            // Lấy dữ liệu thống kê doanh thu theo năm từ đơn hàng
            var revenueByYear = db.donHangs
                .Where(dh => dh.NgayDatHang != null) // Đảm bảo ngày đặt hàng không null
                .GroupBy(dh => dh.NgayDatHang.Year) // Nhóm theo năm
                .Select(group => new
                {
                    Year = group.Key, // Năm
                    Revenue = group.Sum(dh => dh.ChiTietDonHangs.Sum(ct => ct.TongTien)) // Tính tổng doanh thu
                })
                .OrderBy(item => item.Year) // Sắp xếp theo năm
                .ToList();

            return Json(revenueByYear, JsonRequestBehavior.AllowGet);
        }
    }
}