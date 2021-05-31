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
                          TenLoai = a.TenLoai,
                      };
            if (page == null) page = 1;
            int pageSize = Common.ConfigStatic.pageSize;
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
                    db_context.LoaiKhach_insert(_DO.TenLoai);
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới loại khách: "+e.Message+" ');</script>";
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
                           TenLoai = a.TenLoai,

                       }).ToList();

            LoaiKhachValidation DO = new LoaiKhachValidation();
            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDLoaiKhach = a.IDLoaiKhach;
                    DO.TenLoai = a.TenLoai;

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
                db_context.LoaiKhach_update(_DO.IDLoaiKhach, _DO.TenLoai);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";

            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại: "+e.Message+"');</script>";
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