using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNHATHAU.Models
{
    public class HopDongValidation
    {
        [Required(ErrorMessage = "Nhập ID hợp đồng")]
        public int IDHD{ get; set; }

        [Required(ErrorMessage = "Số hợp đồng")]
        public string SoHD { get; set; }

        [Required(ErrorMessage = "Tên hợp đồng")]
        public string TenHD { get; set; }

        [Required(ErrorMessage = "Người đại diện")]
        public string NguoiDaiDien { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        public DateTime? NgayBD { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        public DateTime? NgayKT { get; set; }

        public string GhiChu { get; set; }

        public string FilePath { get; set; }
        public HttpPostedFileBase UploadFile { get; set; }
        public int NhaThauID { get; set; }
        public string TenNhaThau { get; set; }
        public string TenPhongBan { get; set; }
        public int PhongBanID { get; set; }
        public int PBCHNID { get; set; }
        //public IList<string> PhongBanCN { get; set; }

        public List<string> SelectedValues { get; set; }
    }
}