using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class ContractExtensionValidation
    {
        [Required(ErrorMessage = "Nhập ID gia hạn hợp đồng")]
        public int IDGHHD { get; set; }

        public string TenHD { get; set; }

        [Required(ErrorMessage = "Nhập ID hợp đồng")]
        public int IDHD { get; set; }

        public int NhaThauID { get; set; }
        public string TenNhaThau { get; set; }

        public string TenPhongBan { get; set; }
        public int PhongBanID { get; set; }

        public int PBCHNID { get; set; }

        [Required(ErrorMessage = "Nhập ID hợp đồng")]
        public string LyDo { get; set; }

        [Required(ErrorMessage = "Nhập ngày kết thúc hợp đồng")]
        public DateTime? NgayKetThuc { get; set; }
    }
}