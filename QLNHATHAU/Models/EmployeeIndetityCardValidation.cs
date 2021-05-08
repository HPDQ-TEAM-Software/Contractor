using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class EmployeeIndetityCardValidation
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int NhanVienNTID { get; set; }
        public string TenNVNT { get; set; }
        public string MaNVNT { get; set; }
        public string MaCard { get; set; }
        public DateTime? NgayHetHan { get; set; }
    }
}