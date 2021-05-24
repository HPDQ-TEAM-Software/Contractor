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