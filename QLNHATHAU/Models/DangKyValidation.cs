using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class DangKyValidation
    {
        public int IDKhach { get; set; }
        [Required(ErrorMessage = "Nhập Cổng")]
        public int CongID { get; set; }
        public string TenCong { get; set; }

        [Required(ErrorMessage = "Nhập Loại Khách")]
        public int LoaiKhachID { get; set; }
        public string TenKhach { get; set; }

        [Required(ErrorMessage = "Nhập Nhà Thầu")]
        public int NhaThauID { get; set; }
        public string TenNhaThau { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu")]
        public DateTime? Ngay { get; set; }

        [Required(ErrorMessage = "Nhập Họ Tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Nhập Phương Tiện")]
        public string PhuongTien { get; set; }

        [Required(ErrorMessage = "Nhập Biển Số")]
        public string BienSo { get; set; }

        [Required(ErrorMessage = "Nhập Ghi Chú")]
        public string GhiChu { get; set; }
        public int PhongBanID { get; set; }
        public string TenPhongBan { get; set; }
        public int PHCDNID { get; set; }
    }
}