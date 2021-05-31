using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class LoaiKhachValidation
    {
        //[Required]
        public int IDLoaiKhach { get; set; }
        public string TenLoai { get; set; }
    }
} 