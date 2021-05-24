using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QLNHATHAU.Models;
namespace QLNHATHAU.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        public ActionResult Index(int? page)
        {
            var res = from a in db_context.PhongBan_list()
                      select new PhongBanValidation
                      {
                          IDPhongBan = a.IDPhongBan,
                          TenVT = a.TenVT,
                          TenDai = a.TenDai,
                          PCHN = (bool)a.PCHN

                      };
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }
       
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(PhongBanValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db_context.PhongBan_insert(_DO.TenVT, _DO.TenDai, _DO.PCHN);
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới phòng ban: " + e.Message + "');</script>";
                }
            }
            //return View();
            return RedirectToAction("Index", "Department");
        }

        public ActionResult Edit(int id)
        {
            var res = (from dm in db_context.PhongBan_SearchByID(id)
                       select new PhongBanValidation
                       {
                           IDPhongBan = dm.IDPhongBan,
                           TenVT = dm.TenVT,
                           TenDai = dm.TenDai,
                           PCHN = (bool)dm.PCHN
                       }).ToList();
            PhongBanValidation DO = new PhongBanValidation();
            if (res.Count > 0)
            {
                foreach (var dm in res)
                {
                    DO.IDPhongBan = dm.IDPhongBan;
                    DO.TenVT = dm.TenVT;
                    DO.TenDai = dm.TenDai;
                    DO.PCHN = dm.PCHN;
                }
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(PhongBanValidation _DO)
        {
            try
            {
                db_context.PhongBan_update(_DO.IDPhongBan, _DO.TenVT, _DO.TenDai, _DO.PCHN);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Department");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                db_context.PhongBan_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Department");
        }
    }
}