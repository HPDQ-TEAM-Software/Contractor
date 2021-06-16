using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace QLNHATHAU.Controllers
{
    public class ManageCardController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: CardNhanVien
        public ActionResult Index(int? page)
        {
            var dataList = (from a in db_context.CardNhanViens
                            join e in db_context.KeQuaHocs on a.NhanVienNTID equals e.NhanVienNTID
                            join b in db_context.NhanVienNTs on a.NhanVienNTID equals b.IDNhanVienNT
                            select new ManageCardValidation()
                            {
                                ID = a.ID,
                                CardID = (int)a.CardID,
                                MaSoCard = a.MSCard,
                                NhanVienNTID = b.IDNhanVienNT,
                                TenNVNT = b.HoTen,
                                NgayBatDau = (DateTime)a.NgayBatDau,
                                NgayHetHan = (DateTime)a.NgayHetHan,
                                NhaThau = e.NhaThau.Ten,
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

            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(ManageCardValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    db_context.CardNhanVien_insert(_DO.CardID, _DO.NhanVienNTID, _DO.NgayBatDau, _DO.NgayHetHan, _DO.MaSoCard, _DO.NhaThauID);
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            //return View();
            return RedirectToAction("Index", "ManageCard");
        }

        public ActionResult Edit(int id)
        {
            
            var res = (from v in db_context.CardNhanVien_searchByID(id)
                       select new ManageCardValidation
                       {
                           ID = v.ID,
                           CardID = (int)v.CardID,
                           MaSoCard = v.MSCard,
                           NhanVienNTID = (int)v.NhanVienNTID,
                           NgayBatDau = (DateTime)v.NgayBatDau,
                           NgayHetHan = (DateTime)v.NgayHetHan,

                       }).ToList();

            ManageCardValidation DO = new ManageCardValidation();

            if (res.Count > 0)
            {
                foreach (var vp in res)
                {
                    DO.ID = vp.ID;
                    DO.CardID = (int)vp.CardID;
                    DO.MaSoCard = vp.MaSoCard;
                    DO.NhanVienNTID = (int)vp.NhanVienNTID;
                    DO.NgayBatDau = (DateTime)vp.NgayBatDau;
                    DO.NgayHetHan = (DateTime)vp.NgayHetHan; 
                }


                List<NhaThau> nt = db_context.NhaThaus.ToList();
                ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten", DO.NhaThauID);

                List<NhanVienNT> nv = db_context.NhanVienNTs.ToList();
                ViewBag.NVNTList = new SelectList(nv, "IDNVNT", "HovaTen", DO.NhanVienNTID);

                ViewBag.NgayBatDau = DO.NgayBatDau;
                ViewBag.NgayHetHan = DO.NgayHetHan;
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(ManageCardValidation _DO)
        {
            try
            {
                
                db_context.CardNhanVien_update(_DO.ID, _DO.CardID, _DO.NhanVienNTID, _DO.NgayBatDau, _DO.NgayHetHan, _DO.MaSoCard, _DO.NhaThauID);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại " + e.Message + " ');</script>";
            }

            return RedirectToAction("Index", "ManageCard");
        }


        public ActionResult Delete(int id)
        {
            try
            {
                db_context.CardNhanVien_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "ManageCard");
        }


    }
}