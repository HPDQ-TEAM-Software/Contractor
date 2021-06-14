using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;






namespace QLNHATHAU.Controllers
{
    public class ReportAbuseController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: ReportAbuse
        public ActionResult Index(int? page)
        {
            var dataList = (from a in db_context.ViPhams
                            join e in db_context.KeQuaHocs on a.NhanVienNTID equals e.NhanVienNTID
                            join d in db_context.NhanVienNTs on a.NhanVienNTID equals d.IDNhanVienNT
                            join b in db_context.NhaThaus on a.NhaThauID equals b.IDNhaThau
                            join c in db_context.HopDongs on a.HopDongID equals c.IDHD
                            select new ReportAbuseValidation()
                            {
                                ID = a.ID,
                                TenNT = b.Ten,
                                TenHD = c.TenHD,
                                TenNVNT = d.HoTen,
                                NoiDungVP = a.NoiDungVP,
                                NgayVP = (DateTime)a.NgayVP,
                                MucVP = (int)a.MucVP,
                                TSVP = (int)a.TSVP
                            }).ToList();

            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(dataList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {

            //List<NhanVienNT> nv = db_context.NhanVienNTs.ToList();
            //ViewBag.NVNTList = new SelectList(nv, "IDNhanVienNT", "HoTen");

            List<NhaThau> nt = db_context.NhaThaus.ToList();
            ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten");

            //List<HopDong> hd = db_context.HopDongs.ToList();
            //ViewBag.HDList = new SelectList(hd, "IDHD", "TenHD");

            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(ReportAbuseValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //List<ContractorValidation> contractors = (List<ContractorValidation>)ViewNhaThau(_DO.IDNhaThau).Data;
                    //List<HopDongValidation> contracts = (List<HopDongValidation>)GetHopDong(_DO.IDNhaThau).Data;
                    //var HopDongID = contracts[0].IDHD;

                    //List<ContractorStaffValidation> contractorstaffs = (List<ContractorStaffValidation>)GetNhanVienNT(_DO.IDNhaThau).Data;
                    //var NhanVienNTID = contractorstaffs[0].IDNVNT;

                    db_context.ViPham_insert(_DO.NhanVienNTID, _DO.IDNhaThau, _DO.HopDongID, _DO.NoiDungVP, _DO.NgayVP, _DO.MucVP, _DO.TSVP, _DO.TtThe);
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            //return View();
            return RedirectToAction("Index", "ReportAbuse");
        }
        public ActionResult Edit(int id , int? IDNhaThau)
        {
            //List<NhanVienNT> nv = db_context.NhanVienNTs.ToList();
            //ViewBag.NVNTList = new SelectList(nv, "IDNhanVienNT", "HoTen");
            List<NhaThau> nt = db_context.NhaThaus.ToList();
            ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten", IDNhaThau);

            //List<HopDong> hd = db_context.HopDongs.ToList();
            //ViewBag.HDList = new SelectList(hd, "IDHD", "TenHD");

            var res = (from v in db_context.ViPham_searchByID(id)
                       select new ReportAbuseValidation
                       {
                           ID = v.ID,
                           NhanVienNTID = (int)v.NhanVienNTID,
                           NhaThauID = (int)v.NhaThauID,
                           HopDongID = (int)v.HopDongID,
                           NoiDungVP = v.NoiDungVP,
                           NgayVP = (DateTime)v.NgayVP,
                           MucVP = (int)v.MucVP,
                           TSVP = (int)v.TSVP
                       }).ToList();
            ReportAbuseValidation DO = new ReportAbuseValidation();

            if (res.Count > 0)
            {
                foreach (var vp in res)
                {
                    
                    DO.NhaThauID = vp.NhaThauID;
                    DO.HopDongID = vp.HopDongID;
                    DO.NhanVienNTID = vp.NhanVienNTID;
                    DO.NoiDungVP = vp.NoiDungVP;
                    DO.NgayVP = (DateTime)vp.NgayVP;
                    DO.MucVP = vp.MucVP;
                    DO.TSVP = (int)vp.TSVP;
                }


                List<HopDong> h = db_context.HopDongs.ToList();
                ViewBag.HDList = new SelectList(h, "IDHD", "TenHD", DO.HopDongID);

                List<NhanVienNT> nv = db_context.NhanVienNTs.ToList();
                ViewBag.NVNTList = new SelectList(nv, "IDNVNT", "HovaTen", DO.NhanVienNTID);

                ViewBag.NgayVP =  DO.NgayVP.ToString("yyyy-MM-dd");

            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(ReportAbuseValidation _DO)
        {
            try
            {
                //List<ContractorStaffValidation> contractorstaffs = (List<ContractorStaffValidation>)ViewNVNT(_DO.IDNhaThau).Data;
               
                db_context.ViPham_update(_DO.ID, _DO.NhanVienNTID, _DO.IDNhaThau, _DO.HopDongID, _DO.NoiDungVP, _DO.NgayVP, _DO.MucVP, _DO.TSVP, _DO.TtThe);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "ReportAbuse");
        }


        public ActionResult Delete(int id)
        {
            try
            {
                db_context.ViPham_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "ReportAbuse");
        }

        //public JsonResult ViewNhaThau(int id)
        //{
        //    List<ContractorValidation> NTList = (from h in db_context.HopDongs
        //                                         join n in db_context.NhaThaus on h.NhaThauID equals n.IDNhaThau
        //                                         select new ContractorValidation()
        //                                         {
        //                                             IDHD = h.IDHD,
        //                                             MaNT = n.MaNT,
        //                                             MST = n.MST,
        //                                             DiaChi = n.DiaChi,
        //                                             DienThoai = n.DienThoai,
        //                                             Email = n.Email,
        //                                             IDNhaThau = n.IDNhaThau,
        //                                             Ten = n.Ten

        //                                         }).Where(x => x.IDHD == id).ToList();

        //    return Json(NTList, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetHopDong(int id)
        {
            List<HopDongValidation> HDList = (from n in db_context.NhaThaus
                                              join h in db_context.HopDongs on n.IDNhaThau equals h.NhaThauID
                                              select new HopDongValidation()
                                                 {
                                                    IDHD = h.IDHD,
                                                    NhaThauID = n.IDNhaThau,
                                                    TenHD = h.TenHD,

                                                 }).Where(x => x.NhaThauID == id).ToList();

            return Json(HDList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNhanVienNT( int id)
        {
            List<ContractorStaffValidation> NhanVienNTList = (from kq in db_context.KeQuaHocs
                                                              join nv in db_context.NhanVienNTs on kq.NhanVienNTID equals nv.IDNhanVienNT
                                                              join hd in db_context.HopDongs on kq.HDID equals hd.IDHD
                                                              join nt in db_context.NhaThaus on kq.NhaThauID equals nt.IDNhaThau
                                                              select new ContractorStaffValidation()
                                                              {
                                                                  IDNVNT = nv.IDNhanVienNT,
                                                                  HovaTen = nv.HoTen,
                                                                  IDHD = hd.IDHD,
                                                                  IDNhaThau = nt.IDNhaThau,
                                                              }).Where(x => x.IDHD == id).ToList();

            return Json(NhanVienNTList, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetNVNT(int IDNhaThau)
        //{
        //    db_context.Configuration.ProxyCreationEnabled = false;
        //    List<NhanVienNT> NVNTList = db_context.NhanVienNTs.Where(x => x.IDNhaThau == IDNhaThau).ToList();
        //    return Json(NVNTList, JsonRequestBehavior.AllowGet);
        //}

    }
}