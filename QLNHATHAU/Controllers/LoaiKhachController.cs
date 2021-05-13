using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class LoaiKhachController : Controller
    {
        // GET: LoaiKhach

        QLNhaThauEntities db_context = new QLNhaThauEntities();
        public ActionResult Index(int? page)
        {
            var res = from a in db_context.LoaiKhach_Select()
                      select new LoaiKhachValidation
                      {
                          IDLoaiKhach = a.IDLoaiKhach,
                          TenKhach = a.TenKhach,
                      };
                        if (page == null) page = 1;
                        int pageSize = 5;
                        int pageNumber = (page ?? 1);
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(LoaiKhachValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db_context.LoaiKhaces_insert(
                        _DO.IDLoaiKhach,
                        _DO.TenKhach
                 );
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới khách ');</script>";
                }
            }

            return RedirectToAction("Index", "LoaiKhach");
        }

        public ActionResult Edit(int id)
        {
            var res = (from a in db_context.LoaiKhach_SearchByID(id)
                       select new LoaiKhachValidation
                       {
                           IDLoaiKhach = a.IDLoaiKhach,
                           TenKhach = a.TenKhach,

                       }).ToList();

            LoaiKhachValidation DO = new LoaiKhachValidation();
            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDLoaiKhach = a.IDLoaiKhach;
                    DO.TenKhach = a.TenKhach;

                }

            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(LoaiKhachValidation _DO)
        {
            try
            {
                db_context.LoaiKhach_update(_DO.IDLoaiKhach, _DO.TenKhach);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";

            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại');</script>";
            }

            return RedirectToAction("Index", "LoaiKhach");
        }

        public ActionResult Delete(int id)
        {
            db_context.LoaiKhach_delete(id);

            return RedirectToAction("Index", "LoaiKhach");
        }


    }
}