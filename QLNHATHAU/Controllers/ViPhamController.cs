using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QLNHATHAU.Models;

namespace QLNHATHAU.Controllers
{
    public class ViPhamController : Controller
    {
        // GET: ViPham
        //QLNhaThauEntities db_context = new QLNhaThauEntities();

        //public ActionResult Index(int? page)
        //{
        //    var res = from a in db_context.ViPham_Select()
        //              select new ViPhamValidation
        //              {
        //                  IDViPham = a.IDViPham,
        //                  MaNVVP = a.MaNVVP,
        //                  TenNVVP = a.TenNVVP,
        //                  TenNhaThau = a.TenNhaThau,
        //                  HangMuc = a.HangMuc,
        //                  NDViPham = a.NDViPham,
        //                  NgayVP = (DateTime)a.NgayVP,
        //                  MucVP = a.MucVP,
        //                  TongVP = (int)a.TongVP,
        //                  CatThe = (bool)a.CatThe,

        //              };
        //    if (page == null) page = 1;
        //    int pageSize = 20;
        //    int pageNumber = (page ?? 1);
        //    return View(res.ToList().ToPagedList(pageNumber, pageSize));
        //}
       
        //public ActionResult Create()
        //{
        //    return PartialView();
        //}

        //[HttpPost]
        //public ActionResult Create(ViPhamValidation _DO)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var data = db_context.ViPham_insert(_DO.IDViPham,_DO.MaNVVP, _DO.TenNVVP, _DO.TenNhaThau, _DO.HangMuc, _DO.NDViPham, _DO.NgayVP, _DO.MucVP, _DO.TongVP, _DO.CatThe);
        //        }
        //        catch (Exception e)
        //        {
        //            TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới vi phạm: " + e.Message + "');</script>";
        //        }
        //    }
        //    //return View();
        //    return RedirectToAction("Index", "ViPham");
        //}

        //public ActionResult Edit(int id)
        //{
        //    var res = (from dm in db_context.ViPham_SearchByID(id)
        //               select new ViPhamValidation
        //               {
        //                   IDViPham = dm.IDViPham,
        //                   MaNVVP = dm.MaNVVP,
        //                   TenNVVP = dm.TenNVVP,
        //                   TenNhaThau = dm.TenNhaThau,
        //                   HangMuc = dm.HangMuc,
        //                   NDViPham = dm.NDViPham,
        //                   NgayVP = (DateTime)dm.NgayVP,
        //                   MucVP = dm.MucVP,
        //                   TongVP = (int)dm.TongVP,
        //                   CatThe = (bool)dm.CatThe
        //               }).ToList();
        //    ViPhamValidation DO = new ViPhamValidation();
        //    if (res.Count > 0)
        //    {
        //        foreach (var dm in res)
        //        {
        //            DO.IDViPham = dm.IDViPham;
        //            DO.MaNVVP = dm.MaNVVP;
        //            DO.TenNVVP = dm.TenNVVP;
        //            DO.TenNhaThau = dm.TenNhaThau;
        //            DO.HangMuc = dm.HangMuc;
        //            DO.NDViPham = dm.NDViPham;
        //            DO.NgayVP = dm.NgayVP;
        //            DO.MucVP = dm.MucVP;
        //            DO.TongVP = dm.TongVP;
        //            DO.CatThe = dm.CatThe;
        //        }
        //    }
        //    else
        //    {
        //        return HttpNotFound();
        //    }
        //    return PartialView(DO);
        //}

        //[HttpPost]
        //public ActionResult Edit(ViPhamValidation _DO)
        //{
        //    try
        //    {
        //        db_context.ViPham_update(_DO.IDViPham, _DO.MaNVVP, _DO.TenNVVP, _DO.TenNhaThau, _DO.HangMuc, _DO.NDViPham, _DO.NgayVP, _DO.MucVP, _DO.TongVP, _DO.CatThe);
        //        TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
        //    }
        //    catch (Exception e)
        //    {
        //        TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại: " + e.Message + "');</script>";
        //    }
        //    return RedirectToAction("Index", "ViPham");
        //}

        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        db_context.ViPham_delete(id);
        //    }
        //    catch (Exception e)
        //    {
        //        TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
        //    }
        //    return RedirectToAction("Index", "ViPham");
        //}
    }
}