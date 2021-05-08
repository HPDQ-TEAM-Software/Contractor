using QLNHATHAU.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace QLNHATHAU.Controllers
{
    public class NhanVienController : Controller
    {
        // GET: NhanVien
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        public ActionResult Index(int? page, int? IDNhanVien)
        {
            var res = from a in db_context.NhanVien_select()
                select new NhanVienValidation
                {
                    IDNhanVien = a.IDNhanVien,
                    MaNV = a.MaNV,
                    TenNV = a.TenNV,
                    TenNVKD = a.TenNVKD,
                    DiaChi = a.DiaChi,
                    SDT = a.SDT,
                    Email = a.Email,
                    NgayVaolam = a.NgayVaolam,
                    TinhTrangLV = (bool)a.TinhTrangLV,
                    NgayNghiViec = a.NgayNghiViec,
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
        public ActionResult Create(NhanVienValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db_context.NhanVien_insert(
                        _DO.IDNhanVien, 
                        _DO.MaNV, 
                        _DO.TenNV, 
                        _DO.TenNVKD, 
                        _DO.DiaChi,
                        _DO.SDT, 
                        _DO.Email, 
                        _DO.NgayVaolam, 
                        _DO.TinhTrangLV,
                        _DO.NgayNghiViec,
                        _DO.CreateDate,
                        _DO.UpdateDate,
                        _DO.CreateBy,
                        _DO.UpdateBy);
                    //TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                    //list = db_context.NhanVien_select();

                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới nhân viên');</script>";
                }
            }

            return RedirectToAction("Index", "NhanVien");
        }

        public ActionResult Edit(int id)
        {
            var res = (from nv in db_context.NhanVien_SearchByID(id)
                select new NhanVienValidation
                {
                    IDNhanVien = nv.IDNhanVien,
                    MaNV = nv.MaNV.Trim(),
                    TenNV = nv.TenNV,
                    TenNVKD = nv.TenNVKD,
                    DiaChi = nv.DiaChi,
                    SDT = nv.SDT,
                    Email = nv.Email,
                    NgayVaolam = nv.NgayVaolam,
                    TinhTrangLV = (bool)nv.TinhTrangLV,
                    NgayNghiViec = nv.NgayNghiViec,
                }).ToList();

            NhanVienValidation DO = new NhanVienValidation();
            if (res.Count > 0)
            {
                foreach (var nv in res)
                {
                    DO.IDNhanVien = nv.IDNhanVien;
                    DO.MaNV = nv.MaNV;
                    DO.TenNV = nv.TenNV;
                    DO.TenNVKD = nv.TenNVKD;
                    DO.DiaChi = nv.DiaChi;
                    DO.SDT = nv.SDT;
                    DO.Email = nv.Email;
                    DO.NgayVaolam = nv.NgayVaolam;
                    DO.TinhTrangLV = (bool)nv.TinhTrangLV;
                    DO.NgayNghiViec = nv.NgayNghiViec; ;
                }

                ViewBag.NgayVaoLam = DO.NgayVaolam.HasValue ? DO.NgayVaolam.Value.ToString("yyyy-MM-dd") : "NULL";
                ViewBag.NgayNghiViec = DO.NgayNghiViec.HasValue ? DO.NgayNghiViec.Value.ToString("yyyy-MM-dd") : "NULL";
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(NhanVienValidation _DO)
        {
           try
            {
                db_context.NhanVien_update(_DO.IDNhanVien, _DO.MaNV, _DO.TenNV, _DO.TenNVKD, _DO.DiaChi, _DO.SDT, _DO.Email, _DO.NgayVaolam, _DO.TinhTrangLV, _DO.NgayNghiViec);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";

            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại');</script>";
            }
            
            return RedirectToAction("Index", "NhanVien");
        }

        public ActionResult Delete(int id)
        {
            db_context.NhanVien_delete(id);

            return RedirectToAction("Index", "NhanVien");
        }
    }
}