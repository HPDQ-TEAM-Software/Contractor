using PagedList;
using QLNHATHAU.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class EmployeeIndetityCardController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Quyen
        //public ActionResult Index(int? page, int? id)
        //{
        //    var res = from a in db_context.EmplIndentityCard_select()
        //              select new EmployeeIndetityCardValidation
        //              {
        //                   Id = a.ID,
        //                   CardId = (int)a.CardID,
        //                   NhanVienNTID = (int)a.NhanVienNTID,
        //                   TenNVNT = a.HoTen,
        //                   MaNVNT = a.MaNV,
        //                   MaCard = a.MaCard,
        //                   NgayHetHan = a.NgayHetHan
        //              };

        //    if (page == null) page = 1;
        //    int pageSize = 5;
        //    int pageNumber = (page ?? 1);

        //    return View(res.ToList().ToPagedList(pageNumber, pageSize));
        //}

        // GET: Create Quyen
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(CardValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db_context.Card_insert(_DO.MaCard, _DO.NgayHetHan);
                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";

                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới card');</script>";
                }
            }

            return RedirectToAction("Index", "Card");
        }

        public ActionResult Edit(int id)
        {
            var res = (from a in db_context.Card_SearchByID(id)
                       select new CardValidation
                       {
                           IDCard = a.IDCard,
                           MaCard = a.MaCard,
                           NgayHetHan = a.NgayHetHan
                       }).ToList();

            CardValidation DO = new CardValidation();
            if (res.Count > 0)
            {
                foreach (var c in res)
                {
                    DO.IDCard = c.IDCard;
                    DO.MaCard = c.MaCard;
                    DO.NgayHetHan = c.NgayHetHan;
                }
            }
            else
            {
                return HttpNotFound();
            }

            ViewBag.NgayHetHan = DO.NgayHetHan.HasValue ? DO.NgayHetHan.Value.ToString("yyyy-MM-dd") : "NULL";

            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(CardValidation _DO)
        {
            try
            {
                db_context.Card_update(_DO.IDCard, _DO.MaCard, _DO.NgayHetHan);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại');</script>";
            }

            return RedirectToAction("Index", "Card");
        }

        public ActionResult Delete(int id)
        {
            db_context.Card_Delete(id);

            return RedirectToAction("Index", "Card");
        }
    }
}