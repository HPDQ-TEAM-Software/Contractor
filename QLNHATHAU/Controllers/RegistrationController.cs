using ExcelDataReader;
using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class RegistrationController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Registration
        public ActionResult Index(int? page)
        {
            var res = (from a in db_context.DKKhaches
                       join nt in db_context.NhaThaus on a.NhaThauID equals nt.IDNhaThau
                       join pb in db_context.PhongBans on a.PhongBanID equals pb.IDPhongBan
                       join lk in db_context.LoaiKhaches on a.LoaiKhachID equals lk.IDLoaiKhach
                       join c in db_context.Congs on a.CongID equals c.IDCONG
                       select new RegistrationValidation()
                       {
                           IDDangKyKH = a.IDDangKyKH,
                           NhaThauID = (int)a.NhaThauID,
                           TenNhaThau = nt.Ten,
                           PhongBanID = (int)a.PhongBanID,
                           TenPhongBan = pb.TenDai,
                           LoaiKhachID = (int)a.LoaiKhachID,
                           TenKhach = lk.TenLoai,
                           CongID = (int)a.CongID,
                           TenCong = c.TenCong,
                           NguoiDaiDien = a.NguoiDaiDien,
                           BienSo = a.BienSo,
                           PhuongTien = a.PhuongTien,
                           NgayBL = a.NgayBL,
                           TinhTrang = (int)a.TinhTrang
                       }).ToList();

            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = ((int)(page ?? 1));
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            List<Cong> c = db_context.Congs.ToList();
            ViewBag.CongList = new SelectList(c, "IDCONG", "TenCong");

            List<LoaiKhach> lk = db_context.LoaiKhaches.ToList();
            ViewBag.LKList = new SelectList(lk, "IDLoaiKhach", "TenLoai");

            List<NhaThau> nt = db_context.NhaThaus.ToList();
            ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten");

            List<PhongBan> pb = db_context.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "IDPhongBan", "TenDai");
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(RegistrationValidation _DO)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    db_context.DKKhach_InsertKH(_DO.NhaThauID, _DO.PhongBanID, _DO.LoaiKhachID, _DO.CongID, _DO.NguoiDaiDien, _DO.PhuongTien, _DO.BienSo, _DO.NgayBL, _DO.TinhTrang);
                    TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới: " + e.Message + "');</script>";
                }
            }
            return RedirectToAction("Index", "Registration");
        }

        public ActionResult Edit(int id)
        {
            var res = (from a in db_context.DKKhach_SearchByID(id)
                       select new RegistrationValidation
                       {
                           IDDangKyKH = a.IDDangKyKH,
                           NhaThauID = (int)a.NhaThauID,
                           PhongBanID = (int)a.PhongBanID,
                           LoaiKhachID = (int)a.LoaiKhachID,
                           CongID = (int)a.CongID,
                           NguoiDaiDien = a.NguoiDaiDien,
                           BienSo = a.BienSo,
                           PhuongTien = a.PhuongTien,
                           NgayBL = a.NgayBL,
                           TinhTrang = (int)a.TinhTrang
                       }).ToList();

            RegistrationValidation DO = new RegistrationValidation();
            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDDangKyKH = a.IDDangKyKH;
                    DO.NhaThauID = (int)a.NhaThauID;
                    DO.PhongBanID = (int)a.PhongBanID;
                    DO.LoaiKhachID = (int)a.LoaiKhachID;
                    DO.CongID = (int)a.CongID;
                    DO.NguoiDaiDien = a.NguoiDaiDien;
                    DO.BienSo = a.BienSo;
                    DO.PhuongTien = a.PhuongTien;
                    DO.NgayBL = a.NgayBL;
                    DO.TinhTrang = (int)a.TinhTrang;
                }

                List<Cong> c = db_context.Congs.ToList();
                ViewBag.CongList = new SelectList(c, "IDCONG", "TenCong", DO.CongID);

                List<LoaiKhach> lk = db_context.LoaiKhaches.ToList();
                ViewBag.LKList = new SelectList(lk, "IDLoaiKhach", "TenLoai", DO.LoaiKhachID);

                List<NhaThau> nt = db_context.NhaThaus.ToList();
                ViewBag.NTList = new SelectList(nt, "IDNhaThau", "Ten", DO.NhaThauID);

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

        public ActionResult Edit(RegistrationValidation _DO)
        {
            try
            {

                db_context.DKKhach_Update(_DO.IDDangKyKH, _DO.NhaThauID, _DO.PhongBanID, _DO.LoaiKhachID, _DO.CongID, _DO.NguoiDaiDien,
                                          _DO.PhuongTien, _DO.BienSo, _DO.NgayBL, _DO.TinhTrang);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";

            }

            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại: " + e.Message + "');</script>";
            }

            return RedirectToAction("Index", "Registration");
        }
        public ActionResult Delete(int id)
        {
            db_context.DKKhach_delete(id);

            return RedirectToAction("Index", "Registration");
        }

        public ActionResult ImportExcel()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ImportExcel(RegistrationValidation _DO)
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
                    int dtc = 0;

                    if (dt.Rows.Count > 10)
                    {
                        try
                        {
                            string NhaThau = dt.Rows[2][1].ToString();
                            var CheckNT = db_context.NhaThaus.Where(x => x.Ten == NhaThau).ToList();
                            string NguoiDaiDien = dt.Rows[3][1].ToString();
                            string PhongBan = dt.Rows[4][1].ToString();
                            var CheckPB = db_context.PhongBans.Where(x => x.TenDai == PhongBan).ToList();
                            string LoaiKhach = dt.Rows[5][1].ToString();
                            var CheckLK = db_context.LoaiKhaches.Where(x => x.TenLoai == LoaiKhach).ToList();
                            string Cong = dt.Rows[6][1].ToString();
                            var CheckC = db_context.Congs.Where(x => x.TenCong == Cong).ToList();
                            string PhuongTien = dt.Rows[7][1].ToString();
                            string BienSo = dt.Rows[8][1].ToString();
                            string NgayBL = dt.Rows[9][1].ToString();

                            if(CheckNT.Count > 0 && CheckPB.Count > 0)
                            {
                                NhaThauValidation _DONT = new NhaThauValidation();
                                PhongBanValidation _DOPB = new PhongBanValidation();
                                LoaiKhachValidation _DOLK = new LoaiKhachValidation();
                                CongValidation _DOC = new CongValidation();
                                foreach (var nt in CheckNT)
                                {
                                    _DONT.IDNhaThau = nt.IDNhaThau;
                                    _DONT.Ten = nt.Ten;
                                }
                                foreach (var pb in CheckPB)
                                {
                                    _DOPB.IDPhongBan = pb.IDPhongBan;
                                    _DOPB.TenDai = pb.TenDai;
                                   
                                }
                                foreach (var lk in CheckLK)
                                {
                                    _DOLK.IDLoaiKhach = lk.IDLoaiKhach;
                                    _DOLK.TenLoai = lk.TenLoai;
                                }
                                foreach (var c in CheckC)
                                {
                                    _DOC.IDCong = c.IDCONG;
                                    _DOC.TenCong = c.TenCong;
                                }    

                                DateTime NBL = DateTime.ParseExact(NgayBL, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                System.Data.Entity.Core.Objects.ObjectParameter returnIDDangKyKH = new System.Data.Entity.Core.Objects.ObjectParameter("IDDangKyKH", typeof(int));
                                db_context.DKKhach_Insert(_DONT.IDNhaThau, _DOPB.IDPhongBan, _DOLK.IDLoaiKhach, _DOC.IDCong, NguoiDaiDien, PhuongTien, BienSo, NBL, 0, returnIDDangKyKH);
                                int IDDangKyKH = Convert.ToInt32(returnIDDangKyKH.Value);
                                for (int i = 12; i < dt.Rows.Count; i++)
                                {
                                    if (NVHP(dt.Rows[i][0].ToString()))
                                    {
                                        db_context.DSKhach_Insert(IDDangKyKH, dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
                                    }    
                                }
                            }    
                            

                        }
                        catch(Exception ex)
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

            return RedirectToAction("Index", "Registration");
        }

        private bool NVHP(string CMND)
        {
            if (CMND != "")
                return true;
            else
                return false;
        }
        // Trình ký khach vip
        public ActionResult CheckInformation(int id)
        {
            CheckInforRegistration _DO = new CheckInforRegistration();

            var res = (from a in db_context.DKKhaches
                       join nt in db_context.NhaThaus on a.NhaThauID equals nt.IDNhaThau
                       join pb in db_context.PhongBans on a.PhongBanID equals pb.IDPhongBan
                       join lk in db_context.LoaiKhaches on a.LoaiKhachID equals lk.IDLoaiKhach
                       join c in db_context.Congs on a.CongID equals c.IDCONG
                       select new RegistrationValidation()
                       {
                           IDDangKyKH = a.IDDangKyKH,
                           NhaThauID = (int)a.NhaThauID,
                           PhongBanID = (int)a.PhongBanID,
                           LoaiKhachID = (int)a.LoaiKhachID,
                           CongID = (int)a.CongID,
                           NguoiDaiDien = a.NguoiDaiDien,
                           BienSo = a.BienSo,
                           PhuongTien = a.PhuongTien,
                           NgayBL = a.NgayBL,
                           TinhTrang = (int)a.TinhTrang
                       }).ToList();
            RegistrationValidation DO = new RegistrationValidation();
            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDDangKyKH = a.IDDangKyKH;
                    DO.NhaThauID = (int)a.NhaThauID;
                    DO.PhongBanID = (int)a.PhongBanID;
                    DO.LoaiKhachID = (int)a.LoaiKhachID;
                    DO.CongID = (int)a.CongID;
                    DO.NguoiDaiDien = a.NguoiDaiDien;
                    DO.BienSo = a.BienSo;
                    DO.PhuongTien = a.PhuongTien;
                    DO.NgayBL = a.NgayBL;
                    DO.TinhTrang = (int)a.TinhTrang;
                }
          
                    //ID phòng ban quản lý
                    var TrBPql = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == DO.PhongBanID)
                                  join a in db_context.NhanViens on ql.NhanVienID equals a.IDNhanVien
                                  select new CheckInforNguoiDung
                                  {
                                      IDNguoiDung = ql.IDNguoiDung,
                                      TenNV = a.TenNV + " - User: " + ql.TenDangNhap,
                                  }).ToList();
                    //ID phòng ban của HCDN:43
                    var TrPCHN = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == 23)
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
        public ActionResult CheckInformation(CheckInforRegistration _DO)
        {
            try
            {
                //Loại HS của đăng ký khách là 4
                //insert BP QL
                db_context.PheDuyet_Insert(_DO.IDTrPBPQL, 4, _DO.HoSoID, false, null);
                //insert Phòng Hành Chính Đối Ngoại
                db_context.PheDuyet_Insert(_DO.IDTrPPCHN, 4, _DO.HoSoID, false, null);
                //insert BGĐ
                db_context.PheDuyet_Insert(_DO.IDBGD, 4, _DO.HoSoID, false, null);

                TempData["msgSuccess"] = "<script>alert('Trình ký thành công');</script>";

            }

            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Trình ký thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Registration");
        }
        // Trình Ký Khách Thường
        public ActionResult CheckInformationoften(int id)
        {
            CheckInforRegistration _DO = new CheckInforRegistration();

            var res = (from a in db_context.DKKhaches
                       join nt in db_context.NhaThaus on a.NhaThauID equals nt.IDNhaThau
                       join pb in db_context.PhongBans on a.PhongBanID equals pb.IDPhongBan
                       join lk in db_context.LoaiKhaches on a.LoaiKhachID equals lk.IDLoaiKhach
                       join c in db_context.Congs on a.CongID equals c.IDCONG
                       select new RegistrationValidation()
                       {
                           IDDangKyKH = a.IDDangKyKH,
                           NhaThauID = (int)a.NhaThauID,
                           PhongBanID = (int)a.PhongBanID,
                           LoaiKhachID = (int)a.LoaiKhachID,
                           CongID = (int)a.CongID,
                           NguoiDaiDien = a.NguoiDaiDien,
                           BienSo = a.BienSo,
                           PhuongTien = a.PhuongTien,
                           NgayBL = a.NgayBL,
                           TinhTrang = (int)a.TinhTrang
                       }).ToList();
            RegistrationValidation DO = new RegistrationValidation();
            if (res.Count > 0)
            {
                foreach (var a in res)
                {
                    DO.IDDangKyKH = a.IDDangKyKH;
                    DO.NhaThauID = (int)a.NhaThauID;
                    DO.PhongBanID = (int)a.PhongBanID;
                    DO.LoaiKhachID = (int)a.LoaiKhachID;
                    DO.CongID = (int)a.CongID;
                    DO.NguoiDaiDien = a.NguoiDaiDien;
                    DO.BienSo = a.BienSo;
                    DO.PhuongTien = a.PhuongTien;
                    DO.NgayBL = a.NgayBL;
                    DO.TinhTrang = (int)a.TinhTrang;
                }

                //ID phòng ban quản lý
                var TrBPql = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == DO.PhongBanID)
                              join a in db_context.NhanViens on ql.NhanVienID equals a.IDNhanVien
                              select new CheckInforNguoiDung
                              {
                                  IDNguoiDung = ql.IDNguoiDung,
                                  TenNV = a.TenNV + " - User: " + ql.TenDangNhap,
                              }).ToList();
                //ID phòng ban của HCDN:43
                var TrPCHN = (from ql in db_context.NguoiDungs.Where(x => x.PhongBanID == 23)
                              join a in db_context.NhanViens on ql.NhanVienID equals a.IDNhanVien
                              select new CheckInforNguoiDung
                              {
                                  IDNguoiDung = ql.IDNguoiDung,
                                  TenNV = a.TenNV + " - User: " + ql.TenDangNhap,
                              }).ToList();
                ViewBag.TrPBPQLList = new SelectList(TrBPql, "IDNguoiDung", "TenNV");
                ViewBag.TrPPCHNList = new SelectList(TrPCHN, "IDNguoiDung", "TenNV");

                _DO.HoSoID = id;

            }
            return PartialView(_DO);
        }

        [HttpPost]
        public ActionResult CheckInformationoften(CheckInforRegistration _DO)
        {
            try
            {
                //Loại HS của đăng ký khách là 4
                //insert BP QL
                db_context.PheDuyet_Insert(_DO.IDTrPBPQL, 4, _DO.HoSoID, false, null);
                //insert Phòng Hành Chính Đối Ngoại
                db_context.PheDuyet_Insert(_DO.IDTrPPCHN, 4, _DO.HoSoID, false, null);

                TempData["msgSuccess"] = "<script>alert('Trình ký thành công');</script>";

            }

            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Trình ký thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Registration");
        }


        public ActionResult Detail(int? page, int? id)
        {
            var res = (from a in db_context.DKKhaches.Where(x=> x.IDDangKyKH == id)
                       join lk in db_context.LoaiKhaches on a.LoaiKhachID equals lk.IDLoaiKhach
                       join c in db_context.Congs on a.CongID equals c.IDCONG
                       join nt in db_context.NhaThaus on a.NhaThauID equals nt.IDNhaThau
                       join ds in db_context.DSKhaches on a.IDDangKyKH equals ds.DangKyKHID
                       select new RegistrationValidation()
                       {
                           IDDangKyKH = a.IDDangKyKH,
                           NhaThauID = (int)nt.IDNhaThau,
                           TenNhaThau = nt.Ten,
                           LoaiKhachID = (int)a.LoaiKhachID,
                           TenKhach = lk.TenLoai,
                           CongID = (int)a.CongID,
                           HoTen = ds.HoTen,
                           CMNN = ds.CMND,
                           TenCong = c.TenCong,
                           NguoiDaiDien = a.NguoiDaiDien,
                           PhuongTien = a.PhuongTien,
                           BienSo = a.BienSo,
                           NgayBL = a.NgayBL


                       }).ToList();

            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return PartialView(res.ToPagedList(pageNumber, pageSize));
          
        }
    }
  }