using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ManageCardValidation
    {
        public int ID { get; set; }
        public int CardID { get; set; }
        public string MaSoCard { get; set; }
        public int NhanVienNTID { get; set; }
        public string MaNhanVienNT { get; set; }
        public string TenNVNT { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public int NhaThauID { get; set; }
        public string NhaThau { get; set; }
    }
}