using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;






namespace QLNHATHAU.Controllers
{
    public class ConStaffStatisticsController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        
        public ActionResult Index(int? page)
        {
            List<NhaThau> lnt = db_context.NhaThaus.ToList();
            ViewBag.NTList = new SelectList(lnt, "IDNhaThau", "Ten");

            var dataList = (from kq in db_context.KeQuaHocs
                            join nv in db_context.NhanVienNTs on kq.NhanVienNTID equals nv.IDNhanVienNT
                            join hd in db_context.HopDongs on kq.HDID equals hd.IDHD
                            join nt in db_context.NhaThaus on kq.NhaThauID equals nt.IDNhaThau
                            select new ContractorStaffValidation()
                            {
                                IDNVNT = nv.IDNhanVienNT,
                                MaNV = nv.MaNV,
                                HovaTen = nv.HoTen,
                                CMND = nv.SCMND,
                                IDHD = hd.IDHD,
                                TenHD = hd.TenHD,
                                IDNhaThau = nt.IDNhaThau,
                                TenNhaThau = nt.Ten
                            }).ToList();

            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(dataList.ToPagedList(pageNumber, pageSize));
        }

    }
}