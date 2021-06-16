using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class CardValidation
    {
        public int IDCard { get; set; }
        public string MaCard { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public int NVNTID { get; set; }
    }
}