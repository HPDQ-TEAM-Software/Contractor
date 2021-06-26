using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ResultsLearnSafeController : Controller

    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: ResultsSchoolSafe
        public ActionResult Index(int? page)
        {
            var res = (from a in db_context.KetQuaHoc_select()
                       select new ResultsLearnSafeValidation()
                       {
                           IDKQHAT = a.IDKQHAT,
                           TenNVNT = a.HoTen,
                           NhanVienNTID = a.IDNhanVienNT,
                           MaNVNT = a.MaNV,
                           SoHD = a.SoHD,
                           TenNT = a.Ten,
                           NgayHoc = a.NgayHoc,
                           KetQua = a.KetQua
                       }).ToList();

            if (page == null) page = 1;
            int pageSize = 50;
            int pageNumber = (page ?? 1);

            return View(res.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Save(List<ResultAfterChange> data)
        {
            try
            {
                foreach (var i in data)
                {
                    db_context.KetQuaHoc_save(i.IDKQHAT, i.KetQua);
                    if (i.KetQua)
                    {
                        //db_context.EmplIndentityCard_insert(1, i.IDNhanVienNT);
                    }

                }
                TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
            }
            catch(Exception e)
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới hợp đồng');</script>";
            }
            return RedirectToAction("Index", "ResultsLearnSafe");
        }

        public ActionResult UpdateBulk(string data)
        {
            List<HopDong> hd = db_context.HopDongs.ToList();
            ViewBag.HDList = new SelectList(hd, "IDHD", "SoHD");

            return PartialView();
        }

        [HttpPost]
        public ActionResult UpdateBulk(string data, ResultsLearnSafeValidation DO)
        {
            ContractorStaffController contractorStaffController = new ContractorStaffController();
            List<ContractorValidation> contractors = (List<ContractorValidation>)contractorStaffController.ViewNhaThau(DO.HDID).Data;
            var IDNhaThau = contractors[0].IDNhaThau;
            var IDNVNT = data.Split(',');
            try
            {
                foreach (var i in IDNVNT)
                {
                    if (i.Length > 0)
                    {
                        var IDKQHAT = db_context.IDKetQuaHoc_ByMaNV(i).ToList();
                        db_context.KetQuaHoc_update(IDKQHAT[0].IDKQHAT, IDKQHAT[0].IDNhanVienNT, DO.HDID, IDNhaThau, DO.NgayHoc, DO.KetQua);
                        if ((bool)DO.KetQua)
                        {
                            //db_context.EmplIndentityCard_insert(1 ,IDKQHAT[0].IDNhanVienNT);
                        }
                        TempData["msgError"] = "<script>alert('Cập nhật kết quả học tập số lượng nhiều thành công!');</script>";
                    }
                    else
                    {
                        TempData["msgError"] = "<script>alert('Vui lòng chọn nhân viên để cập nhật.');</script>";
                    }
                }
            }catch(Exception e)
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi cập nhật kết quả học tập.');</script>";

            }
            
            return RedirectToAction("Index", "ResultsLearnSafe");
        }
    }
}
