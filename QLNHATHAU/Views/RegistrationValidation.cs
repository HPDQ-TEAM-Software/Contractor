using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Views
{
    public class RegistrationValidation
    {

        public int IDKhach { get; set; }

        public int CongID { get; set; }
        public string TenCong { get; set; }

        public int LoaiKhachID { get; set; }
        public string TenKhach { get; set; }

        public int NhaThauID { get; set; }
        public string TenNhaThau { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu")]
        public DateTime? NgayBD { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc")]
        public DateTime? NgayKT { get; set; }

        public string HoTen { get; set; }

        public string PhuongTien { get; set; }

        public string BienSo { get; set; }

        public string GhiChu { get; set; }
    }
}