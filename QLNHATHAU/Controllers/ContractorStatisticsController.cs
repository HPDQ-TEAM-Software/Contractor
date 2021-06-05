using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;






namespace QLNHATHAU.Controllers
{
    public class ContractorStatisticsController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();

        public ActionResult Index(int? page)
        {
            

            var dataList = (from kq in db_context.KeQuaHocs
                            join nv in db_context.NhanVienNTs on kq.NhanVienNTID equals nv.IDNhanVienNT
                            join hd in db_context.HopDongs on kq.HDID equals hd.IDHD
                            join nt in db_context.NhaThaus on kq.NhaThauID equals nt.IDNhaThau
                            select new NhaThauValidation()
                            {
                                IDNhaThau = nt.IDNhaThau,
                                MaNT = nt.MaNT,
                                Ten = nt.Ten,
                                TenHD = hd.TenHD,
                                //SLNhanVien = nv.IDNhanVienNT,
            
                            }).ToList();

            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(dataList.ToPagedList(pageNumber, pageSize));

            
        }

    }
}