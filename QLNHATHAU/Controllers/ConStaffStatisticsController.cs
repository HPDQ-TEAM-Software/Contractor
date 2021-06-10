using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;





namespace QLNHATHAU.Controllers
{
    public class ConStaffStatisticsController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();

        //public ActionResult Index(int? page)
        //{
        //    List<NhaThau> lnt = db_context.NhaThaus.ToList();
        //    ViewBag.NTList = new SelectList(lnt, "IDNhaThau", "Ten");

        //    List<HopDong> h = db_context.HopDongs.ToList();
        //    ViewBag.HDList = new SelectList(h, "IDHD", "TenHD");

        //    var dataList = (from kq in db_context.KeQuaHocs
        //                    join nv in db_context.NhanVienNTs on kq.NhanVienNTID equals nv.IDNhanVienNT
        //                    join hd in db_context.HopDongs on kq.HDID equals hd.IDHD
        //                    join nt in db_context.NhaThaus on kq.NhaThauID equals nt.IDNhaThau
        //                    select new ContractorStaffValidation()
        //                    {
        //                        IDNVNT = nv.IDNhanVienNT,
        //                        MaNV = nv.MaNV,
        //                        HovaTen = nv.HoTen,
        //                        CMND = nv.SCMND,
        //                        IDHD = hd.IDHD,
        //                        TenHD = hd.TenHD,
        //                        IDNhaThau = nt.IDNhaThau,
        //                        TenNhaThau = nt.Ten
        //                    }).ToList();

        //    if (page == null) page = 1;
        //    int pageSize = 20;
        //    int pageNumber = (page ?? 1);

        //    return View(dataList.ToPagedList(pageNumber, pageSize));
        //}

        public ActionResult Index(int? page, int? IDNhaThau,int ?HDID, string search)
        {
            List<NhaThau> nt = db_context.NhaThaus.ToList();
            if (IDNhaThau == null) IDNhaThau = 0;
            if (search == null) search = "";
            ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten", IDNhaThau);
            List<HopDong> hd = db_context.HopDongs.Where(x=>x.NhaThauID==IDNhaThau).ToList();
            ViewBag.HDList = new SelectList(hd, "IDHD", "TenHD",HDID);
            ViewBag.search = search;
            if (page == null) page = 1;
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(GetlistConStaffStatistics(IDNhaThau,HDID,search).ToPagedList(pageNumber, pageSize));

        }
        public JsonResult GetHopDong(int NhaThauID)
        {
            db_context.Configuration.ProxyCreationEnabled = false;
            List<HopDong> HopdongList = db_context.HopDongs.Where(x => x.NhaThauID == NhaThauID).ToList();
            return Json(HopdongList, JsonRequestBehavior.AllowGet);
        }
        List<ContractorStaffValidation> GetlistConStaffStatistics(int? IDNhaThau, int? HDID ,string search)
        {
            if (IDNhaThau == null) IDNhaThau = 0;
            if (HDID == null) HDID = 0;

            if (search == "")
            {
                if (IDNhaThau != 0 && HDID != 0)
                {
                    var model = (from kq in db_context.KeQuaHocs.Where(x => x.NhaThauID == IDNhaThau && x.HDID == HDID)
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
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();
                }
                else if (IDNhaThau == 0 && HDID != 0)
                {
                    var model = (from kq in db_context.KeQuaHocs.Where(x => x.HDID == HDID)
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
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();
                }
                else if (IDNhaThau != 0 && HDID == 0)
                {
                    var model = (from kq in db_context.KeQuaHocs.Where(x => x.NhaThauID == IDNhaThau)
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
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();

                }
                else
                {
                    var model = (from kq in db_context.KeQuaHocs
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
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();
                }
            }
            else
            {
                if (IDNhaThau != 0 && HDID != 0)
                {
                    var model = (from kq in db_context.KeQuaHocs.Where(x => x.NhaThauID == IDNhaThau && x.HDID == HDID && (x.NhanVienNT.HoTen.Contains(search)))
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
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();
                }
                else if (IDNhaThau == 0 && HDID != 0)
                {
                    var model = (from kq in db_context.KeQuaHocs.Where(x => x.HDID == HDID && (x.NhanVienNT.HoTen.Contains(search)))
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
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();
                }
                else if (IDNhaThau != 0 && HDID == 0)
                {
                    var model = (from kq in db_context.KeQuaHocs.Where(x => x.NhaThauID == IDNhaThau && (x.NhanVienNT.HoTen.Contains(search)))
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
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();

                }
                else
                {
                    var model = (from kq in db_context.KeQuaHocs.Where(x=> x.NhanVienNT.HoTen.Contains(search))
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
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();
                }
            }
        }
        public JsonResult GetNhanvien(string search)
        {
            var result = (from kq in db_context.KeQuaHocs.Where(x => x.NhanVienNT.MaNV.Contains(search) || x.NhanVienNT.HoTen.Contains(search))
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
                          }).Take(30).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }







    }
}