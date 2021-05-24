using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class PhongBanValidation
    {
        public int? IDPhongBan { get; set; }

        [Required(ErrorMessage = "Nhập tên viết tắt")]
        [MaxLength(10, ErrorMessage = "vượt quá số kí tự 10")]
        public string TenVT { get; set; }

        [Required(ErrorMessage = "Nhập tên phòng ban")]
        [MaxLength(50, ErrorMessage = "vượt quá số kí tự 50")]
        public string TenDai { get; set; }

        public bool PCHN { get; set; }
    }
}