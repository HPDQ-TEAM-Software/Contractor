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

            List<NhaThau> nt = db_context.NhaThaus.ToList();
            ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten");

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
                    db_context.ViPham_insert(_DO.NhanVienNTID, _DO.NhaThauID, _DO.HopDongID, _DO.NoiDungVP, _DO.NgayVP, _DO.MucVP, _DO.TSVP);
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            //return View();
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
    }
}