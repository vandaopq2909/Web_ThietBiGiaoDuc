﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_ThietBiGiaoDuc.Models
{
    public class NhaCungCap
    {
        public NhaCungCap() 
        {
            MaNCC = "NCC" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        [Key]
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string TrangThai { get; set; }
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; }

    }
}