using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class NguoiDungValidation
    {
        [Required(ErrorMessage = "Nhập ID người dùng")]
        public int IDNguoiDung { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Email")]
        public string Email { get; set; }

        public int PhongBanID { get; set; }
        public int NhanVienID { get; set; }
        public IList<string> IDQuyen { get; set; }
        public string TenQuyen { get; set; }
        public IList<Quyen> QuyenList { get; set; }
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string Ten { get; set; }
        public string DanhSachQuyen { get; set; }
        public int IDPhanQuyen { get; set; }
    }
}