using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ReportAbuseValidation
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "NV Nha Thau Id")]
        public int NhanVienNTID { get; set; }
        public string TenNVNT { get; set; }


        [Required(ErrorMessage = "Nha Thau Id")]
        public int NhaThauID { get; set; }
        public string TenNT { get; set; }


        [Required(ErrorMessage = "Hop dong Id")]
        public int HopDongID { get; set; }
        public string TenHD { get; set; }

        [Required(ErrorMessage = "Noi dung Vi pham")]
        public string NoiDungVP { get; set; }

        [Required(ErrorMessage = "Ngay VP")]

        public DateTime NgayVP { get; set; }

        [Required(ErrorMessage = "Muc VP")]
        public int MucVP { get; set; }
        public int TSVP { get; set; }
        public bool CatThe { get; set; }

        public int IDNVNT { get; set; }
        public int IDHD { get; set; }
        public int IDNhaThau { get; set; }
    }
}