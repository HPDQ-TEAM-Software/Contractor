using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class RegistrationValidation
    {
        public int IDDangKyKH { get; set; }

   
        public int NhaThauID { get; set; }
        public string TenNhaThau { get; set; }

        [Required(ErrorMessage = "Nhập Phòng Ban")]
        public int PhongBanID { get; set; }
        public string TenPhongBan { get; set; }

        [Required(ErrorMessage = "Nhập Loại Khách")]
        public int LoaiKhachID { get; set; }
        public string TenKhach { get; set; }

        [Required(ErrorMessage = "Nhập Cổng")]
        public int CongID { get; set; }
        public string TenCong { get; set; }

        [Required(ErrorMessage = "Nhập Người Đại Diện")]
        public string NguoiDaiDien { get; set; }

        [Required(ErrorMessage = "Nhập Biển Số")]
        public string BienSo { get; set; }

        [Required(ErrorMessage = "Nhập Phương Tiện")]
        public string PhuongTien { get; set; }

        [Required(ErrorMessage = "Ngày Bảo Lãnh")]
        public DateTime? NgayBL { get; set; }

        public int TinhTrang { get; set; }

        public int DangKyKHID { get; set; }
        public string HoTen { get; set; }
        public string CMNN { get; set; }


    }
}