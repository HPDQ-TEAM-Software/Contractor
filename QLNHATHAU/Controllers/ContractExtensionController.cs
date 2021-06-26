using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ContractExtensionController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: ContractExtension
        public ActionResult Index(int? page)
        {
            var res = (from a in db_context.GiaHanHopDong_select()
                       select new ContractExtensionValidation
                       {
                           IDGHHD = a.IDGHHD,
                           IDHD = (int)a.HDID,
                           TenHD = a.TenHD,
                           LyDo = a.LyDo,
                           NgayKetThuc = a.NgayKetThuc
                       }).ToList();

            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(res.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            List<HopDong> hd = db_context.HopDongs.ToList();
            ViewBag.HDList = new SelectList(hd, "IDHD", "TenHD");

            List<NhaThau> nt = db_context.NhaThaus.ToList();
            ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten");

            List<PhongBan> pb = db_context.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "IDPhongBan", "TenDai");
            var model = (from p in db_context.PhongBan_ISPCHN()
                         select new PhongBan
                         {
                             IDPhongBan = p.IDPhongBan,
                             TenDai = p.TenDai
                         }).ToList();
            ViewBag.PCHNList = new SelectList(model, "IDPhongBan", "TenDai");
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(ContractExtensionValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db_context.GiaHanHopDong_Insert(_DO.IDHD, _DO.LyDo, _DO.PBCHNID, _DO.PhongBanID, _DO.NhaThauID, _DO.NgayKetThuc);

                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";

                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới hợp đồng');</script>";
                }
            }

            return RedirectToAction("Index", "ContractExtension");
        }

        public ActionResult Edit(int id)
        {
            List<HopDong> hd = db_context.HopDongs.ToList();
            ViewBag.HDList = new SelectList(hd, "IDHD", "TenHD");


            CheckInforContractExtension _DO = new CheckInforContractExtension();

            var res = (from gh in db_context.GiaHanHDs.Where(x => x.IDGHHD == id)
                       join a in db_context.HopDongs on gh.HDID equals a.IDHD
                       select new ContractExtensionValidation
                       {
                           IDGHHD = gh.IDGHHD,
                           IDHD = (int)gh.HDID,
                           NhaThauID = (int)a.NhaThauID,
                           PhongBanID = (int)a.PhongBanID,
                           PBCHNID = (int)a.PBCHNID,
                           LyDo = gh.LyDo,
                           NgayKetThuc = gh.NgayKetThuc

                       }).ToList();

            ContractExtensionValidation DO = new ContractExtensionValidation();
            if (res.Count > 0)
            {
                foreach (var gh in res)
                {
                    DO.IDGHHD = gh.IDGHHD;
                    DO.IDHD = (int)gh.IDHD;
                    DO.LyDo = gh.LyDo;
                    DO.PBCHNID = (int)gh.PBCHNID;
                    DO.PhongBanID = (int)gh.PhongBanID;
                    DO.NhaThauID = (int)gh.NhaThauID;
                    DO.NgayKetThuc = gh.NgayKetThuc;

                }

                List<PhongBan> pbchn = db_context.PhongBans.Where(x => x.PCHN == true).ToList();
                ViewBag.CHNList = new SelectList(pbchn, "IDPhongBan", "TenDai", DO.PBCHNID);

                List<NhaThau> nt = db_context.NhaThaus.ToList();
                ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten", DO.NhaThauID);

                List<PhongBan> pb = db_context.PhongBans.ToList();
                ViewBag.PBList = new SelectList(pb, "IDPhongBan", "TenDai", DO.PhongBanID);

                ViewBag.NgayKetThuc = DO.NgayKetThuc.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(ContractExtensionValidation _DO)
        {
            try
            {
                //db_context.GiaHanHopDong_update(_DO.IDGHHD, _DO.IDHD, _DO.LyDo, _DO.PBCHNID, _DO.PhongBanID, _DO.NhaThauID, _DO.NgayKetThuc);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";

            }

            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại');</script>";
            }

            return RedirectToAction("Index", "ContractExtension");
        }

        public ActionResult Delete(int id)
        {
            db_context.GiaHanHopDong_Delete(id);

            return RedirectToAction("Index", "ContractExtension");
        }

        public ActionResult CheckInformationExtension(int id)
        {
            CheckInforContractExtension _DO = new CheckInforContractExtension();
            var res = (from gh in db_context.GiaHanHDs.Where(x => x.IDGHHD == id)
                       join a in db_context.HopDongs on gh.HDID equals a.IDHD
                       select new ContractExtensionValidation
                       {
                           IDGHHD = gh.IDGHHD,
                           IDHD = (int)gh.HDID,
                           NhaThauID = (int)a.NhaThauID,
                           PhongBanID = (int)a.PhongBanID,
                           PBCHNID = (int)a.PBCHNID,
                           LyDo = gh.LyDo,
                           NgayKetThuc = gh.NgayKetThuc

                       }).ToList();
            ContractExtensionValidation DO = new ContractExtensionValidation();
            if (res.Count > 0)
            {
                foreach (var gh in res)
                {
                    DO.IDGHHD = gh.IDGHHD;
                    DO.IDHD = (int)gh.IDHD;
                    DO.NhaThauID = (int)gh.NhaThauID;
                    DO.PhongBanID = (int)gh.PhongBanID;
                    DO.PBCHNID = (int)gh.PBCHNID;
                    DO.LyDo = gh.LyDo;
                    DO.NgayKetThuc = gh.NgayKetThuc;

                }

                var TrBPql = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == DO.PhongBanID)
                              join a in db_context.NhanViens on ql.NhanVienID equals a.IDNhanVien
                              select new CheckInforNguoiDung
                              {
                                  IDNguoiDung = ql.IDNguoiDung,
                                  TenNV = a.TenNV + " - User: " + ql.TenDangNhap,
                              }).ToList();

                var TrPCHN = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == DO.PBCHNID)
                              join a in db_context.NhanViens on ql.NhanVienID equals a.IDNhanVien
                              select new CheckInforNguoiDung
                              {
                                  IDNguoiDung = ql.IDNguoiDung,
                                  TenNV = a.TenNV + " - User: " + ql.TenDangNhap,
                              }).ToList();
                //ID phòng ban của BGĐ:43
                var BGD = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == 43)
                           join a in db_context.NhanViens on ql.NhanVienID equals a.IDNhanVien
                           select new CheckInforNguoiDung
                           {
                               IDNguoiDung = ql.IDNguoiDung,
                               TenNV = a.TenNV + " - User: " + ql.TenDangNhap,
                           }).ToList();
                ViewBag.TrPBPQLList = new SelectList(TrBPql, "IDNguoiDung", "TenNV");
                ViewBag.TrPPCHNList = new SelectList(TrPCHN, "IDNguoiDung", "TenNV");
                ViewBag.BGDList = new SelectList(BGD, "IDNguoiDung", "TenNV");

                _DO.HoSoID = id;
            }
            return PartialView(_DO);
        }

        [HttpPost]
        public ActionResult CheckInformationExtension(CheckInforContractExtension _DO)
        {
            try
            {
                //Loại HS của hợp đồng là 2

                //insert BP QL
                db_context.PheDuyet_Insert(_DO.IDTrPBPQL, 2, _DO.HoSoID, false, null);
                //insert Phòng Ban Chức Năng
                db_context.PheDuyet_Insert(_DO.IDTrPPCHN, 2, _DO.HoSoID, false, null);
                //insert BGĐ
                db_context.PheDuyet_Insert(_DO.IDBGD, 2, _DO.HoSoID, false, null);

                TempData["msgSuccess"] = "<script>alert('Trình ký thành công');</script>";

            }

            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Trình ký thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "ContractExtension");
        }

    }
}