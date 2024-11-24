﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class KhachHang
    {
        public KhachHang() 
        {
            MaKH = "KH" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        [Key]
        public string MaKH { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen {  get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string TrangThai { get; set; }
        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}