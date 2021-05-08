using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ResultsLearnSafeValidation
    {
        public int IDKQHAT { get; set; }

        [Required(ErrorMessage = "Nhập nhân viên nhà thầu")]
        public int  NhanVienNTID { get; set; }
        [Required(ErrorMessage = "Nhập hợp đồng.")]
        public int HDID { get; set; }
        public int? IDNhaThau { get; set; }
        public DateTime? NgayHoc { get; set; }
        public bool? KetQua { get; set; }
        public string MaNVNT { get; set; }
        public string SoHD { get; set; }
        public string TenNT { get; set; }
        public string TenNVNT { get; set; }
    }
}