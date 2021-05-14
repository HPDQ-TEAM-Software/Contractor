using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class CongController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Cong
        public ActionResult Index(int? page)
        {
        var res = from a in db_context.Cong_Select()
                  select new CongValidation
                  {
                      IDCong = a.IDCong,
                      TenCong = a.TenCong

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
        public ActionResult Create(CongValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db_context.Cong_insert(_DO.TenCong);
                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";

                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới công');</script>";
                }
            }
                    
            return RedirectToAction("Index", "Cong");
        }

        public ActionResult Edit(int id)
        {
            var res = (from q in db_context.Cong_SearchByID(id)
                       select new CongValidation
                       {
                           IDCong = q.IDCONG,
                           TenCong = q.TenCong,
                       }).ToList();

            CongValidation DO = new CongValidation();
            if (res.Count > 0)
            {
                foreach (var nv in res)
                {
                    DO.IDCong = nv.IDCong;
                    DO.TenCong = nv.TenCong;
                }
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(CongValidation _DO)
        {
            try
            {
                db_context.Cong_update(_DO.IDCong, _DO.TenCong);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại');</script>";
            }

            return RedirectToAction("Index", "Cong");
        }

        public ActionResult Delete(int id)
        {
            db_context.Cong_delete(id);

            return RedirectToAction("Index", "Cong");
        }
    }
}