using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class RegistrationlistValidation
    {
        public int IDKhach { get; set; }

        public int DangKyKHID { get; set; }
        public string TenNDD { get; set; }


        [Required(ErrorMessage = "Nhập Họ Tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Nhập CMND")]
        public string CMND { get; set; }
    }
}