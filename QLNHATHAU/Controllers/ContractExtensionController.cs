using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ContractExtensionController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: ContractExtension
        public ActionResult Index(int? page)
        {
            var res = (from a in db_context.GiaHanHopDong_select()
                      select new ContractExtensionValidation
                      {
                          IDGHHD = a.IDGHHD,
                          IDHD = (int)a.HDID,
                          TenHD = a.TenHD,
                          LyDo = a.LyDo,
                          NgayKetThuc = a.NgayKetThuc
                      }).ToList();

            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(res.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            List<HopDong> hd = db_context.HopDongs.ToList();
            ViewBag.HDList = new SelectList(hd, "IDHD", "TenHD");

            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(ContractExtensionValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db_context.GiaHanHopDong_Insert(_DO.IDHD, _DO.LyDo, _DO.NgayKetThuc);
                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";

                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới hợp đồng');</script>";
                }
            }

            return RedirectToAction("Index", "ContractExtension");
        }

        public ActionResult Edit(int id)
        {
            List<HopDong> hd = db_context.HopDongs.ToList();
            ViewBag.HDList = new SelectList(hd, "IDHD", "TenHD");

            var res = (from gh in db_context.GiaHanHopDong_SearchByID(id)
                       select new ContractExtensionValidation
                       {
                           IDGHHD = gh.IDGHHD,
                           IDHD = (int)gh.HDID,
                           LyDo = gh.LyDo,
                           NgayKetThuc = gh.NgayKetThuc

                       }).ToList();

            ContractExtensionValidation DO = new ContractExtensionValidation();
            if (res.Count > 0)
            {
                foreach (var gh in res)
                {
                   DO.IDGHHD = gh.IDGHHD;
                   DO.IDHD = (int)gh.IDHD;
                   DO.LyDo = gh.LyDo;
                   DO.NgayKetThuc = gh.NgayKetThuc;
                }
                ViewBag.NgayKetThuc = DO.NgayKetThuc.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(ContractExtensionValidation _DO)
        {
            try
            {
                db_context.GiaHanHopDong_update(_DO.IDGHHD, _DO.IDHD, _DO.LyDo, _DO.NgayKetThuc);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";

            }

            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại');</script>";
            }

            return RedirectToAction("Index", "ContractExtension");
        }

        public ActionResult Delete(int id)
        {
            db_context.GiaHanHopDong_Delete(id);

            return RedirectToAction("Index", "ContractExtension");
        }

    }
}