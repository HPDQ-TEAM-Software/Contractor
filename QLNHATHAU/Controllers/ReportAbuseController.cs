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
                            join d in db_context.NhanVienNTs on a.NhanVienNTID equals d.IDNhanVienNT
                            join b in db_context.NhaThaus on a.NhaThauID equals b.IDNhaThau
                            join c in db_context.HopDongs on a.HopDongID equals c.IDHD
                            select new ReportAbuseValidation()
                            {
                                ID = a.ID,
                                TenNVNT = d.HoTen,
                                TenNT = b.Ten,
                                TenHD = c.TenHD,
                                NoiDungVP = a.NoiDungVP,
                                NgayVP = (DateTime)a.NgayVP,
                                MucVP = a.MucVP,
                                TSVP = (int)a.TSVP
                            }).ToList();

            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(dataList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {

            List<NhanVienNT> nv = db_context.NhanVienNTs.ToList();
            ViewBag.NVNTList = new SelectList(nv, "IDNhanVienNT", "HoTen");

            //List<NhaThau> nt = db_context.NhaThaus.ToList();
            //ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten");

            List<HopDong> hd = db_context.HopDongs.ToList();
            ViewBag.HDList = new SelectList(hd, "IDHD", "TenHD");

            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(ReportAbuseValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<ContractorValidation> contractors = (List<ContractorValidation>)ViewNhaThau(_DO.HopDongID).Data;
                    var IDNhaThau = contractors[0].IDNhaThau;
                    db_context.ViPham_insert(_DO.NhanVienNTID, IDNhaThau, _DO.HopDongID, _DO.NoiDungVP, _DO.NgayVP, _DO.MucVP, _DO.TSVP);
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            //return View();
            return RedirectToAction("Index", "ReportAbuse");
        }
        public ActionResult Edit(int id)
        {
            List<NhanVienNT> nv = db_context.NhanVienNTs.ToList();
            ViewBag.NVNTList = new SelectList(nv, "IDNhanVienNT", "HoTen");

            //List<NhaThau> nt = db_context.NhaThaus.ToList();
            //ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten");

            List<HopDong> hd = db_context.HopDongs.ToList();
            ViewBag.HDList = new SelectList(hd, "IDHD", "TenHD");

            var res = (from v in db_context.ViPham_searchByID(id)
                       select new ReportAbuseValidation
                       {
                           ID = v.ID,
                           NhanVienNTID = (int)v.NhanVienNTID,
                           NhaThauID = (int)v.NhaThauID,
                           HopDongID = (int)v.HopDongID,
                           NoiDungVP = v.NoiDungVP,
                           NgayVP = (DateTime)v.NgayVP,
                           MucVP = v.MucVP,
                           TSVP = (int)v.TSVP
                       }).ToList();
            ReportAbuseValidation DO = new ReportAbuseValidation();

            if (res.Count > 0)
            {
                foreach (var vp in res)
                {
                    DO.NhanVienNTID = vp.NhanVienNTID;
                    DO.NhaThauID = vp.NhaThauID;
                    DO.HopDongID = vp.HopDongID;
                    DO.NoiDungVP = vp.NoiDungVP;
                    DO.NgayVP = (DateTime)vp.NgayVP;
                    DO.MucVP = vp.MucVP;
                    DO.TSVP = (int)vp.TSVP;
                }
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
                List<ContractorValidation> contractors = (List<ContractorValidation>)ViewNhaThau(_DO.HopDongID).Data;
                db_context.ViPham_update(_DO.ID, _DO.NhanVienNTID, _DO.NhaThauID, _DO.HopDongID, _DO.NoiDungVP, _DO.NgayVP, _DO.MucVP, _DO.TSVP);

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

        public JsonResult ViewNhaThau(int id)
        {
            List<ContractorValidation> NTList = (from h in db_context.HopDongs join n in db_context.NhaThaus on h.NhaThauID equals n.IDNhaThau
                                                 select new ContractorValidation()
                                                 {
                                                     IDHD = h.IDHD,
                                                     MaNT = n.MaNT,
                                                     MST = n.MST, 
                                                     DiaChi = n.DiaChi,
                                                     DienThoai = n.DienThoai,
                                                     Email = n.Email,
                                                     IDNhaThau = n.IDNhaThau,
                                                     Ten = n.Ten
                                                     
                                                 }).Where(x => x.IDHD == id).ToList();

            return Json(NTList, JsonRequestBehavior.AllowGet);
        }

        

    }
}