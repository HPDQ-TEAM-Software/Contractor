using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class SignerController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Signer
        public ActionResult Approved(int? page)

        {
            {
                var res = (from a in db_context.PheDuyet_select(MyAuthentication.IDNguoidung)
                           select new SignerValidation()
                           {
                               ID = a.ID,
                               TenHoSo = a.TenHD,
                               TenNhaThau = a.Ten,

                           }).ToList();

                if (page == null) page = 1;
                int pageSize = 5;
                int pageNumber = (page ?? 1);

                return View(res.ToPagedList(pageNumber, pageSize));

            }
        }


        public ActionResult GHHD(int? page)

        {


            {
                var res = (from a in db_context.PheDuyet_selectGH(MyAuthentication.IDNguoidung)
                           select new SignerValidation()
                           {
                               ID = a.ID,
                               TenHoSo = a.TenHD,
                               TenNhaThau = a.Ten,

                           }).ToList();

                if (page == null) page = 1;
                int pageSize = 5;
                int pageNumber = (page ?? 1);

                return PartialView(res.ToPagedList(pageNumber, pageSize));

            }
        }
        public ActionResult HAT(int? page)

        {
            var res = (from a in db_context.PheDuyet_select(MyAuthentication.IDNguoidung)
                       select new SignerValidation()
                       {
                           ID = a.ID,
                           TenHoSo = a.TenHD,
                           TenNhaThau = a.Ten,

                       }).ToList();

            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(res.ToPagedList(pageNumber, pageSize));
            //{
            //    var res = (from a in db_context.PheDuyet_selectAT(MyAuthentication.IDNguoidung)
            //               select new SignerValidation()
            //               {
            //                   ID = a.ID,
            //                   TenHoSo = a.TenHD,
            //                   TenNhaThau = a.Ten,

            //               }).ToList();

            //    if (page == null) page = 1;
            //    int pageSize = 5;
            //    int pageNumber = (page ?? 1);

            //    return PartialView(res.ToPagedList(pageNumber, pageSize));
            //}
        }


        public ActionResult Pending(int? page)

        {
            {
                var res = (from a in db_context.PheDuyet_SelectAll(MyAuthentication.IDNguoidung)
                           select new SignerValidation()
                           {
                               ID = a.ID,
                               TenHoSo = a.TenHD,
                               TenNhaThau = a.Ten,
                               GhiChu = a.GhiChu

                           }).ToList();

                if (page == null) page = 1;
                int pageSize = 50;
                int pageNumber = (page ?? 1);

                return View(res.ToPagedList(pageNumber, pageSize));
            }

        }

        public ActionResult GHHD1(int? page)

        {
            {
                var res = (from a in db_context.PheDuyet_SelectAllGH(MyAuthentication.IDNguoidung)
                           select new SignerValidation()
                           {
                               ID = a.ID,
                               TenHoSo = a.TenHD,
                               TenNhaThau = a.Ten,
                               GhiChu = a.GhiChu

                           }).ToList();

                if (page == null) page = 1;
                int pageSize = 5;
                int pageNumber = (page ?? 1);

                return View(res.ToPagedList(pageNumber, pageSize));
            }

        }

        public ActionResult HAT1(int? page)

        {
            {
                var res = (from a in db_context.PheDuyet_SelectAllAT(MyAuthentication.IDNguoidung)
                           select new SignerValidation()
                           {
                               ID = a.ID,
                               TenHoSo = a.TenHD,
                               TenNhaThau = a.Ten,
                               GhiChu = a.GhiChu

                           }).ToList();

                if (page == null) page = 1;
                int pageSize = 5;
                int pageNumber = (page ?? 1);

                return View(res.ToPagedList(pageNumber, pageSize));
            }

        }


        public ActionResult Signerl(int id)
        {
            List<NguoiDung> nd = db_context.NguoiDungs.ToList();
            ViewBag.NDList = new SelectList(nd, "IDNguoiDung", "TenDangNhap");

            List<LoaiH> hs = db_context.LoaiHS.ToList();
            ViewBag.HSList = new SelectList(hs, "ID", "TenLoai");

            List<HopDong> hd = db_context.HopDongs.ToList();
            ViewBag.HDList = new SelectList(hd, "IDHD", "TenHD");

            var res = (from a in db_context.PheDuyet_SearchByID(id)
                       select new SignerValidation
                       {
                           ID = a.ID,
                           NguoiDungID = (int)a.NguoiDungID,
                           LoaiHSID = (int)a.LoaiHSID,
                           HoSoID = (int)a.HoSoID,
                           GhiChu = a.GhiChu,
                           TinhTrang = a.TinhTrang

                       }).ToList();

            SignerValidation DO = new SignerValidation();


            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.ID = a.ID;
                    DO.NguoiDungID = (int)a.NguoiDungID;
                    DO.LoaiHSID = (int)a.LoaiHSID;
                    DO.HoSoID = (int)a.HoSoID;
                    DO.GhiChu = a.GhiChu;
                    DO.TinhTrang = a.TinhTrang;

                }
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Signerl(SignerValidation _DO)
        {
            try
            {
                db_context.PheDuyet_Updata(_DO.ID, true, _DO.GhiChu);

                TempData["msgSuccess"] = "<script>alert('Phê Duyệt Thành Công');</script>";

            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Phê Duyệt Thất Bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Approved", "Signer");
        }
    }
}