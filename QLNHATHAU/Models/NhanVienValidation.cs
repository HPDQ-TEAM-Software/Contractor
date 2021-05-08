using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNHATHAU.Models
{
    public class NhanVienValidation
    {
        [Required(ErrorMessage ="Nhập ID nhân viên.")]
        public int IDNhanVien { get; set; }

        [Required(ErrorMessage = "Nhập mã nhân viên.")]
        public string MaNV { get; set; }

        [Required(ErrorMessage = "Nhập tên nhân viên.")]
        public string TenNV { get; set; }

        [Required(ErrorMessage = "Nhập tên nhân viên KD.")]
        public string TenNVKD { get; set; }

        [Required(ErrorMessage = "Nhập địa chỉ.")]
        public string DiaChi { get; set; }

        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Required(ErrorMessage = "Số điện thoại.")]
        public string SDT { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nhập ngày vào làm.")]
        [DataType(DataType.Date)]
        public DateTime? NgayVaolam { get; set; }

        public bool TinhTrangLV { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgayNghiViec { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }
}