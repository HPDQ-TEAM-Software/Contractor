using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QLNHATHAU.Models;

namespace QLNHATHAU.Controllers
{
    public class ContractorsController : Controller
    {
        // GET: Contractors
       
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        public ActionResult Index(int? page)
        {
            var res = from a in db_context.NhaThau_list()
                      select new NhaThauValidation
                      {
                          IDNhaThau = a.IDNhaThau,
                          MaNT = a.MaNT,
                          MST = a.MST,
                          Ten = a.Ten,
                          DiaChi=a.DiaChi,
                          DienThoai=a.DienThoai,
                          Email=a.Email
                      };
            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(NhaThauValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db_context.Nhathau_insert(_DO.MaNT,_DO.MST,_DO.Ten,_DO.DiaChi,_DO.DienThoai,_DO.Email);
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới nhà thầu: " + e.Message + "');</script>";
                }
            }
            return RedirectToAction("Index", "Contractors");
        }

        public ActionResult Edit(int id)
        {
            var res = (from dm in db_context.Nhathau_SearchByID(id)
                       select new NhaThauValidation
                       {
                           IDNhaThau = dm.IDNhaThau,
                           MaNT = dm.MaNT,
                           MST = dm.MST,
                           Ten = dm.Ten,
                           DiaChi=dm.DiaChi,
                           DienThoai=dm.DienThoai,
                           Email=dm.Email
                       }).ToList();
            NhaThauValidation DO = new NhaThauValidation();
            if (res.Count > 0)
            {
                foreach (var dm in res)
                {
                    DO.IDNhaThau = dm.IDNhaThau;
                    DO.MaNT = dm.MaNT;
                    DO.MST = dm.MST;
                    DO.Ten = dm.Ten;
                    DO.DiaChi = dm.DiaChi;
                    DO.DienThoai = dm.DienThoai;
                    DO.Email = dm.Email;
                }
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(NhaThauValidation _DO)
        {
            try
            {
                db_context.Nhathau_update(_DO.IDNhaThau,_DO.MaNT, _DO.MST, _DO.Ten, _DO.DiaChi, _DO.DienThoai, _DO.Email);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Contractors");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                db_context.Nhathau_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Contractors");
        }
    }
}