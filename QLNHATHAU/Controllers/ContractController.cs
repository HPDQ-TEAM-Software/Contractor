using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ContractController : Controller
    {
        // GET: Contract
        QLNhaThauEntities db_context = new QLNhaThauEntities();
      
        public ActionResult Index(int? page)
        {
            var res = (from a in db_context.HopDongs
                       join p in db_context.PhongBans
                       on a.PhongBanID equals p.IDPhongBan
                       join n in db_context.NhaThaus
                       on a.NhaThauID equals n.IDNhaThau
                       select new HopDongValidation()
                       {
                           IDHD = a.IDHD,
                           SoHD = a.SoHD,
                           TenHD = a.TenHD,
                           NguoiDaiDien = a.NguoiDaiDien,
                           NgayBD = a.NgayBD,
                           NgayKT = a.NgayKT,
                           GhiChu = a.GhiChu,
                           FilePath = a.File,
                           NhaThauID = (int)a.NhaThauID,
                           TenNhaThau = n.Ten,
                           PhongBanID = (int)a.PhongBanID,
                           TenPhongBan = p.TenDai
                       }).ToList();

            if (page == null) page = 1;

            int pageNumber = (page ?? 1);
            return View(res.ToList().ToPagedList(pageNumber, Common.ConfigStatic.pageSize));
        }

        public ActionResult Create()
        {
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
        public ActionResult Create(HopDongValidation _DO)
        {
            try
            {
                ObjectParameter returnId = new ObjectParameter("IDHD", typeof(int));
                db_context.HopDong_insert(_DO.SoHD, _DO.TenHD, _DO.NguoiDaiDien, _DO.NgayBD, _DO.NgayKT, _DO.GhiChu, UploadFile(_DO), _DO.NhaThauID, _DO.PhongBanID, returnId);
                int idHD = Convert.ToInt32(returnId.Value);
                //Hiện tại chỉ cho 1 phòng ban chức năng
                db_context.PCHN_HopDong_Insert(_DO.PCHN,idHD);
                TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới hợp đồng: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Contract");
        }

        public ActionResult Edit(int id)
        {
            var res = (from hd in db_context.HopDong_SearchByID(id)
                       select new HopDongValidation
                       {
                           IDHD = hd.IDHD,
                           SoHD = hd.SoHD,
                           TenHD = hd.TenHD,
                           NguoiDaiDien = hd.NguoiDaiDien,
                           NgayBD = hd.NgayBD,
                           NgayKT = hd.NgayKT,
                           GhiChu = hd.GhiChu,
                           FilePath = hd.File,
                           NhaThauID = (int)hd.NhaThauID,
                           PhongBanID = (int)hd.PhongBanID
                       }).ToList();

            HopDongValidation DO = new HopDongValidation();
            if (res.Count > 0)
            {
                foreach (var hd in res)
                {
                    DO.IDHD = hd.IDHD;
                    DO.SoHD = hd.SoHD;
                    DO.TenHD = hd.TenHD;
                    DO.NguoiDaiDien = hd.NguoiDaiDien;
                    DO.NgayBD = hd.NgayBD;
                    DO.NgayKT = hd.NgayKT;
                    DO.GhiChu = hd.GhiChu;
                    DO.FilePath = hd.FilePath;
                    DO.NhaThauID = (int)hd.NhaThauID;
                    DO.PhongBanID = (int)hd.PhongBanID;
                }


                List<NhaThau> nt = db_context.NhaThaus.ToList();
                ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten", DO.NhaThauID);

                List<PhongBan> pb = db_context.PhongBans.ToList();
                ViewBag.PBList = new SelectList(pb, "IDPhongBan", "TenDai", DO.PhongBanID);

                //var model = (from p in db_context.PhongBan_ISPCHN()
                //             select new PCHNValidation
                //             {
                //                 ID = p.IDPhongBan,
                //                 TenDai = p.TenDai,
                //                 TenVT = p.TenVT

                //             });
                // List<PCHNValidation> pbcn = model.ToList();
                //List<PCHN> phcn = db_context.PCHNs.ToList(); 
                List<PCHN> pbchn = db_context.PCHNs.ToList();
                //var pbcn = db_context.PhongBan_PCHN_loadedit(DO.IDHD).SingleOrDefault();
                ViewBag.PBCHNList = new SelectList(pbchn, "ID", "TenDai", 3);

                if (DO.FilePath != null)
                {
                    ViewBag.FilePath = (Regex.Match(DO.FilePath, "(?<=-).*").Value);
                }
                ViewBag.NgayBD = DO.NgayBD.HasValue ? DO.NgayBD.Value.ToString("yyyy-MM-dd") : "NULL";
                ViewBag.NgayKT = DO.NgayKT.HasValue ? DO.NgayKT.Value.ToString("yyyy-MM-dd") : "NULL";
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(HopDongValidation _DO)
        {
            try
            {
                db_context.HopDong_update(
                    _DO.IDHD, _DO.SoHD, _DO.TenHD, _DO.NguoiDaiDien, _DO.NgayBD, _DO.NgayKT, _DO.GhiChu, UploadFile(_DO), _DO.NhaThauID, _DO.PhongBanID);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";

            }

            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại: " + e.Message + "');</script>";
            }

            return RedirectToAction("Index", "Contract");
        }

        public ActionResult Delete(int id)
        {
            db_context.HopDong_delete(id);

            return RedirectToAction("Index", "Contract");
        }

        public string UploadFile(HopDongValidation _DO)
        {
            if ((_DO.UploadFile != null && _DO.FilePath == null) || (_DO.UploadFile != null && _DO.FilePath != null))
            {
                string FileName = Path.GetFileNameWithoutExtension(_DO.UploadFile.FileName);
                string FileExtension = Path.GetExtension(_DO.UploadFile.FileName);
                FileName = DateTime.Now.ToString("yyyyMMddHHmm") + "-" + FileName.Trim() + FileExtension;
                string path = Path.Combine(Server.MapPath("~/UploadedFiles"), FileName);
                _DO.FilePath = Url.Content(Path.Combine("~/UploadedFiles/", FileName));
                _DO.UploadFile.SaveAs(path);
            }
            else if (_DO.UploadFile == null && _DO.FilePath != null)
            {
                _DO.FilePath = "~/UploadedFiles/" + _DO.FilePath;
            }
            else
            {
                _DO.FilePath = null;
            }
            return _DO.FilePath;
        }

        public ActionResult DownloadFile(int id)
        {
            var res = db_context.HopDong_SearchByID(id).ToList();
            string FullName = Server.MapPath("~" + res[0].File);
            string FileName = (Regex.Match(FullName, "(?<=-).*").Value);
            byte[] FileBytes = GetFile(FullName);
            return File(
                    FileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }

        byte[] GetFile(string FullName)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(FullName);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(FullName);
            return data;
        }
    }
}