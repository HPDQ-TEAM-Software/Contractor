using ExcelDataReader;
using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class DangKyController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: DangKy
        public ActionResult Index(int? page)
        {
            var res = (from a in db_context.Khaches
                       join c in db_context.Congs on a.CongID equals c.IDCONG
                       join n in db_context.NhaThaus  on a.NhaThauID equals n.IDNhaThau
                       join k in db_context.LoaiKhaches on a.LoaiKhachID equals k.IDLoaiKhach
                       join b in db_context.PhongBans on a.PhongBanID  equals b.IDPhongBan
                       select new DangKyValidation()
                       {
                           IDKhach = a.IDKhach,
                           NhaThauID = (int)a.NhaThauID,
                           TenNhaThau = n.Ten,
                           CongID = (int)a.CongID,
                           TenCong = c.TenCong,
                           LoaiKhachID = (int)a.LoaiKhachID,
                           TenKhach = k.TenLoai,
                           PhongBanID = (int)a.PhongBanID,
                           TenPhongBan = b.TenDai,
                           Ngay = (DateTime)a.Ngay,
                           HoTen = a.HoTen,
                           PhuongTien = a.PhuongTien,
                           BienSo = a.BienSo,
                           GhiChu = a.GhiChu
                       }).ToList();
            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            List<Cong> c = db_context.Congs.ToList();
            ViewBag.CongList = new SelectList(c, "IDCONG", "TenCong");

            List<NhaThau> nt = db_context.NhaThaus.ToList();
            ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten");

            List<LoaiKhach> lk = db_context.LoaiKhaches.ToList();
            ViewBag.LKList = new SelectList(lk, "IDLoaiKhach", "TenLoai");

            List<PhongBan> pb = db_context.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "IDPhongBan", "TenDai");
            var model = (from p in db_context.PhongBan_ISPCHN()
                         select new PhongBan
                         {
                             IDPhongBan = p.IDPhongBan,
                             TenDai = p.TenDai
                         }).ToList();
            ViewBag.HCDNList = new SelectList(model, "IDPhongBan", "TenDai");
            return PartialView();
        }

        [HttpPost]

        public ActionResult Create(DangKyValidation _DO)
        {
            
                if (ModelState.IsValid)
                {
                    try
                    {
                    db_context.Khach_Insert(_DO.CongID, _DO.LoaiKhachID, _DO.NhaThauID, _DO.PhongBanID, _DO.PHCDNID,  _DO.Ngay, _DO.HoTen, _DO.PhuongTien, _DO.BienSo, _DO.GhiChu);

                    }
                    catch (Exception e)
                    {
                        TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                    }
                }
            return RedirectToAction("Index", "DangKy");
        }

        public ActionResult Edit(int id)
        {

            var res = (from dk in db_context.Khach_SearchByID(id)
                       select new DangKyValidation
                       {
                           IDKhach = dk.IDKhach,
                           CongID = (int)dk.CongID,
                           LoaiKhachID = (int)dk.LoaiKhachID,
                           NhaThauID = (int)dk.NhaThauID,
                           Ngay = (DateTime)dk.Ngay,
                           HoTen = dk.HoTen,
                           PhuongTien = dk.PhuongTien,
                           BienSo = dk.BienSo,
                           GhiChu = dk.GhiChu,
                           PhongBanID = (int)dk.PhongBanID,
                           //PHCDNID = (int)dk.PHCDNID,
                       }).ToList();

            DangKyValidation DO = new DangKyValidation();

            if (res.Count > 0)
            {
                foreach (var dk in res)
                {
                    DO.IDKhach = dk.IDKhach;
                    DO.NhaThauID = (int)dk.NhaThauID;
                    DO.CongID = (int)dk.CongID;
                    DO.LoaiKhachID = (int)dk.LoaiKhachID;
                    DO.PhongBanID = (int)dk.PhongBanID;
                    DO.PHCDNID = (int)dk.PHCDNID;
                    DO.Ngay = (DateTime)dk.Ngay;
                    DO.HoTen = dk.HoTen;
                    DO.PhuongTien = dk.PhuongTien;
                    DO.BienSo = dk.BienSo;
                    DO.GhiChu = dk.GhiChu;
                }

                List<Cong> c = db_context.Congs.ToList();
                ViewBag.CongList = new SelectList(c, "IDCONG", "TenCong", DO.CongID);

                List<NhaThau> nt = db_context.NhaThaus.ToList();
                ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten", DO.NhaThauID);

                List<LoaiKhach> lk = db_context.LoaiKhaches.ToList();
                ViewBag.LKList = new SelectList(lk, "IDLoaiKhach", "TenLoai", DO.LoaiKhachID);

                List<PhongBan> phcdn = db_context.PhongBans.Where(x => x.PCHN == true).ToList();
                ViewBag.PHCDNList = new SelectList(phcdn, "IDPhongBan", "TenDai", DO.PHCDNID);

                List<PhongBan> pb = db_context.PhongBans.ToList();
                ViewBag.PBList = new SelectList(pb, "IDPhongBan", "TenDai", DO.PhongBanID);
            }
            else
            {
                return HttpNotFound();
            }

            return PartialView(DO);
        }

        [HttpPost]

        public ActionResult Edit(DangKyValidation _DO)
        {
            try
            {
                db_context.Khach_Update(_DO.IDKhach, _DO.CongID, _DO.LoaiKhachID,_DO.NhaThauID,_DO.PhongBanID,_DO.Ngay, _DO.HoTen, _DO.PhuongTien, _DO.BienSo, _DO.GhiChu);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";

            }

            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "DangKy");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                db_context.Khach_Delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "DangKy");
        }
        public ActionResult ImportExcel()
        {
            return PartialView();
        }
       [HttpPost]
        public ActionResult ImportExcel(DangKyValidation _DO)
        {
            string filePath = string.Empty;
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string path = Server.MapPath("~/UploadedFiles/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filePath = path + Path.GetFileName(DateTime.Now.ToString("yyyyMMddHHmm") + "-" + file.FileName);

                    file.SaveAs(filePath);
                    Stream stream = file.InputStream;

                    IExcelDataReader reader = null;
                    if (file.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (file.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('Vui lòng chọn đúng định dạng file Excel');</script>";
                        return View();
                    }
                    DataSet result = reader.AsDataSet();
                    DataTable dt = result.Tables[0];
                    reader.Close();
                    int dtc = 0, dtrung = 0;

                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            for (int i = 1; i < dt.Rows.Count; i++)
                            {
                                if (CheckHoTen(dt.Rows[i][0].ToString()))
                                {
                                    //db_context.Khach_Insert(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), dt.Rows[i][3].ToString(), dt.Rows[i][4].ToString(), dt.Rows[i][5].ToString());
                                    dtc++;
                                }    

                            }

                            string msg = "";
                            if (dtc != 0 && dtrung != 0)
                            {
                                msg = "Import được " + dtc + " dòng dữ liệu, " + "Có " + dtrung + " dòng trùng LK cập nhập nội dung";
                            }
                            else if (dtc != 0 && dtrung == 0)
                            {
                                msg = "Import được " + dtc + " dòng dữ liệu";
                            }
                            else if (dtc == 0 && dtrung != 0)
                            {
                                msg = "Có " + dtrung + " dòng trùng LK cập nhập nội dung";
                            }
                            else { msg = "File import không có dữ liệu"; }

                            TempData["msgSuccess"] = "<script>alert('" + msg + "');</script>";

                        }
                        catch
                        {
                            TempData["msgSuccess"] = "<script>alert('File import không đúng định dạng. Vui lòng tải biểu mẫu file import');</script>";
                        }
                    }
                    else
                    {
                        TempData["msgSuccess"] = "<script>alert('File import không đúng định dạng. Vui lòng tải biểu mẫu file import');</script>";
                    }    
                }
                else
                {
                    TempData["msgSuccess"] = "<script>alert('Vui lòng nhập file Import');</script>";
                }    
            }
            else
            {
                TempData["msgSuccess"] = "<script>alert('Vui lòng nhập file Import');</script>";
            }    

                return RedirectToAction("Index", "DangKy");
        }

        public bool CheckHoTen(string HoTen)
        {
            var Regsbb = (from u in db_context.Khaches
                          where u.HoTen.ToLower() == HoTen.ToLower()
                          select new { u.HoTen }).FirstOrDefault();
            bool status;
            if (Regsbb != null)
            {
                //Already registered  
                status = false;
            }
            else
            {
                //Available to use  
                status = true;
            }
            return status;
        }

        public ActionResult CheckInformation(int id)
        {
            CheckInforDK _DO = new CheckInforDK();
            var res = (from dk in db_context.Khach_SearchByID(id)

                       select new DangKyValidation
                       {
                           IDKhach = dk.IDKhach,
                           CongID = (int)dk.CongID,
                           LoaiKhachID = (int)dk.LoaiKhachID,
                           NhaThauID = (int)dk.NhaThauID,
                           Ngay = (DateTime)dk.Ngay,
                           HoTen = dk.HoTen,
                           PhuongTien = dk.PhuongTien,
                           BienSo = dk.BienSo,
                           GhiChu = dk.GhiChu,
                           PhongBanID = (int)dk.PhongBanID,
                          // PHCDNID = (int)dk.PHCDNID,
                       }).ToList();

            DangKyValidation DO = new DangKyValidation();

            if (res.Count > 0)
            {
                foreach (var dk in res)
                {
                    DO.IDKhach = dk.IDKhach;
                    DO.NhaThauID = (int)dk.NhaThauID;
                    DO.CongID = (int)dk.CongID;
                    DO.LoaiKhachID = (int)dk.LoaiKhachID;
                    DO.PhongBanID = (int)dk.PhongBanID;
                    DO.PHCDNID = (int)dk.PHCDNID;
                    DO.Ngay = (DateTime)dk.Ngay;
                    DO.HoTen = dk.HoTen;
                    DO.PhuongTien = dk.PhuongTien;
                    DO.BienSo = dk.BienSo;
                    DO.GhiChu = dk.GhiChu;
                   
                }
                //ID loại khách Vip : 11246
                if (_DO.LoaiKhachID == 11246 )
                {

                    var TrBPql = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == DO.PhongBanID)
                                  join a in db_context.NhanViens on ql.NhanVienID equals a.IDNhanVien
                                  select new CheckInforNguoiDung
                                  {
                                      IDNguoiDung = ql.IDNguoiDung,
                                      TenNV = a.TenNV + " - User: " + ql.TenDangNhap,
                                  }).ToList();

                    //ID phòng ban của PCN
                    var TrPHCDN = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == DO.PHCDNID )
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
                    ViewBag.TrPPHCDNList = new SelectList(TrPHCDN, "IDNguoiDung", "TenNV");
                    ViewBag.BGDList = new SelectList(BGD, "IDNguoiDung", "TenNV");

                    _DO.LoaiKhachID = id;
                } 
                else
                {
                    var TrBPql = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == DO.PhongBanID)
                                  join a in db_context.NhanViens on ql.NhanVienID equals a.IDNhanVien
                                  select new CheckInforNguoiDung
                                  {
                                      IDNguoiDung = ql.IDNguoiDung,
                                      TenNV = a.TenNV + " - User: " + ql.TenDangNhap,
                                  }).ToList();

                    var TrPHCDN = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == DO.PHCDNID)
                                  join a in db_context.NhanViens on ql.NhanVienID equals a.IDNhanVien
                                  select new CheckInforNguoiDung
                                  {
                                      IDNguoiDung = ql.IDNguoiDung,
                                      TenNV = a.TenNV + " - User: " + ql.TenDangNhap,
                                  }).ToList();

                    var BGD = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == 43)
                               join a in db_context.NhanViens on ql.NhanVienID equals a.IDNhanVien
                               select new CheckInforNguoiDung
                               {
                                   IDNguoiDung = ql.IDNguoiDung,
                                   TenNV = a.TenNV + " - User: " + ql.TenDangNhap,
                               }).ToList();

                    ViewBag.TrPBPQLList = new SelectList(TrBPql, "IDNguoiDung", "TenNV");
                    ViewBag.TrPPHCDNList = new SelectList(TrPHCDN, "IDNguoiDung", "TenNV");
                    ViewBag.BGDList = new SelectList(BGD, "IDNguoiDung", "TenNV");

                    _DO.LoaiKhachID = id;

                }
            }

            return PartialView(_DO);
        }
        [HttpPost]

        public ActionResult CheckInformation(CheckInforDK _DO)
        {
            try
            {
                //insert Phòng Ban QL
                db_context.PheDuyet_Insert(_DO.IDTrPBPQL, 1, _DO.LoaiKhachID, false, null);
                //insert Phòng Ban HCDN
                db_context.PheDuyet_Insert(_DO.IDTrPPHCDN, 1, _DO.LoaiKhachID, false, null);
                //insert BGĐ
                db_context.PheDuyet_Insert(_DO.IDBGD, 1, _DO.LoaiKhachID, false, null);

                TempData["msgSuccess"] = "<script>alert('Trình ký thành công');</script>";

            }

            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Trình ký thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "DangKy");
        }

    }    
}
