using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace QLNHATHAU.Controllers
{
    public class ContractStatisticsController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();

        public ActionResult Index(int? page)
        {

            var dataList = (from hd in db_context.HopDongs
                            join nt in db_context.NhaThaus on hd.NhaThauID equals nt.IDNhaThau 
                            join kq in db_context.KeQuaHocs on hd.IDHD equals kq.HDID into HDIDNV
                            select new HopDongValidation()
                            {
                                IDHD = hd.IDHD,
                                SoHD = hd.SoHD,
                                TenHD = hd.TenHD,
                                TenNhaThau = nt.Ten,
                                SLNhanVien = HDIDNV.Count(),

                            }).OrderBy(x => x.IDHD).ToList();

            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(dataList.ToPagedList(pageNumber, pageSize));


        }


    }
}