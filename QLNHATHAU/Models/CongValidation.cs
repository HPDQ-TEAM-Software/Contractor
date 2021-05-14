using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class CongValidation
    {
        [Required(ErrorMessage = "Nhập ID Công.")]
        public int IDCong { get; set; }

        [Required(ErrorMessage = "Nhập Tên Công.")]
        public string TenCong { get; set; }
    }
}