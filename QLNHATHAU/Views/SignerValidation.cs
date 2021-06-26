using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class SignerValidation
    {
        public int ID { get; set; }

        public int NguoiDungID { get; set; }
        public string TenDangNhap { get; set; }

        public int NhaThauID { get; set; }
        public string TenNhaThau { get; set; }

        public DateTime? Ngay { get; set; }

        public int LoaiHSID { get; set; }
        public string TenLoai { get; set; }

        public int HoSoID { get; set; }
        public string TenHoSo { get; set; }

        public string GhiChu { get; set; }

        public bool? TinhTrang { get; set; }
    }
}