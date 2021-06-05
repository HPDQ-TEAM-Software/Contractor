using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ContractorStaffValidation
    {
        public int IDNVNT { get; set; }
        [Required(ErrorMessage = "Nhập mã nhân viên")]
        public string MaNV { get; set; }
        [Required(ErrorMessage = "Nhập chứng minh thư/Hộ chiếu")]
        public string CMND { get; set; }
        [Required(ErrorMessage = "Nhập họ và tên")]
        public string HovaTen { get; set; }
        [Required(ErrorMessage = "Nhập chức vụ")]
        public string ChucVu { get; set; }
        [Required(ErrorMessage = "Nhập ngày sinh")]
        public DateTime? NgaySinh { get; set; }
        [Required(ErrorMessage = "Chọn giới tính")]
        public Gender GioiTinh { get; set; }
        [Required(ErrorMessage = "Nhập quốc tịch")]
        public string QuocTich { get; set; }
        public int PhongBanID { get; set; }
        public int IDHD { get; set; }
        public int IDNhaThau { get; set; }
        public string TenHD { get; set; }
        public string TenNhaThau { get; set; }
    }

    public enum Gender
    {
        Male = 0,
        Female = 1,
        NotSpecified = 2
    }
}