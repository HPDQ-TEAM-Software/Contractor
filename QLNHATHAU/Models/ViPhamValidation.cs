using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ViPhamValidation
    {

        public int? IDViPham { get; set; }


        public string MaNVVP { get; set; }


        public string TenNVVP { get; set; }


        public string TenNhaThau { get; set; }


        public string HangMuc { get; set; }


        public string NDViPham { get; set; }

        public DateTime NgayVP { get; set; }


        public string MucVP { get; set; }


        public int TongVP { get; set; }

        public bool CatThe { get; set; }

    }

      
}