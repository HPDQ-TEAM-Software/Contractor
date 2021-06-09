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

            var dataList = (from nt in db_context.NhaThaus
                            join hd in db_context.HopDongs on nt.IDNhaThau equals hd.NhaThauID into NhaThauIDHD
                            join kq in db_context.KeQuaHocs on nt.IDNhaThau equals kq.NhaThauID into NhaThauIDNV
                            select new NhaThauValidation()
                            {
                                IDNhaThau = nt.IDNhaThau,
                                MaNT = nt.MaNT,
                                Ten = nt.Ten,
                                SLHD = NhaThauIDHD.Count(),
                                SLNhanVien = NhaThauIDNV.Count(),

                            }).OrderBy(x => x.IDNhaThau).ToList();

            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(dataList.ToPagedList(pageNumber, pageSize));


        }

    
    }
}