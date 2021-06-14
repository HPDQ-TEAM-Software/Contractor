using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class WarningNoticeController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: WarningNotice
        public ActionResult Index(int? page)
        {
            var dataList = (from d in db_context.NhanVienNTs
                            join e in db_context.KeQuaHocs on d.IDNhanVienNT equals e.NhanVienNTID
                            join a in db_context.ViPhams on d.IDNhanVienNT equals a.NhanVienNTID into NVNTIDVP
                            select new ReportAbuseValidation()
                            {
                                ID = d.IDNhanVienNT,
                                TenNT = e.NhaThau.Ten,
                                TenHD = e.HopDong.TenHD,
                                TenNVNT = d.HoTen,
                                TSVP = NVNTIDVP.Count(),
                                TtThe = true,
                            }).Where(x=>x.TSVP >= 1).ToList();

            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(dataList.ToPagedList(pageNumber, pageSize));
        }
    }
}