using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class NhaThauValidation
    {
        public int? IDNhaThau { get; set; }
        public string MaNT { get; set; }
        public string MST { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public int IDHD { get; set; }
        public int SLNhanVien { get; set; }
        public string TenHD { get; set; }
        public int SLHD { get; set; }
    }
}