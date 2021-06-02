using ExcelDataReader;
using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class ContractorStaffController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: ContractorStaff
        public ActionResult Index(int? page)
        {
            var res = (from a in db_context.NhanVienNT_select()
                       select new ContractorStaffValidation
                       {
                           IDNVNT = a.IDNhanVienNT,
                           MaNV = a.MaNV,
                           CMND = a.SCMND,
                           HovaTen = a.HoTen,
                           NgaySinh = a.NgaySinh,
                           GioiTinh = (Gender)a.GioiTinh,
                           QuocTich = a.QuocTich,
                       }).ToList();

            if (page == null) page = 1;
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(res.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            List<HopDong> hd = db_context.HopDongs.ToList();
            ViewBag.HDList = new SelectList(hd, "IDHD", "SoHD");

            List<PhongBan> pb = db_context.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "IDPhongBan", "TenDai");

            var genderList = Enum.GetValues(typeof(Gender))
                .OfType<Gender>()
                .Select(m => new
                {
                    Text = (m == Gender.Male) ? "Nam" : (m == Gender.Female) ? "Nữ" : "Không xác định",
                    Value = (int)m
                }).ToList();
            ViewBag.GTList = new SelectList(genderList, "Value", "Text");
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(ContractorStaffValidation _DO)
        {
            try
            {
                List<ContractorValidation> contractors = (List<ContractorValidation>)ViewNhaThau(_DO.IDHD).Data;
                var IDNhaThau = contractors[0].IDNhaThau;
                string id = GenerateID(contractors[0].MaNT);
                int gender = (_DO.GioiTinh == Gender.Male) ? 0 : (_DO.GioiTinh == Gender.Female) ? 1 : 2;
                InsertKetQuaHoc(id, _DO.CMND, _DO.HovaTen, _DO.ChucVu, (DateTime)_DO.NgaySinh, gender, _DO.QuocTich, _DO.IDHD, IDNhaThau);
                TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
            }   
            catch (Exception e)
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới hợp đồng');</script>";
            }
            return RedirectToAction("Index", "ContractorStaff");
        }

        public ActionResult Edit(int id)
        {
            var res = (from n in db_context.NhanVienNT_SearchByID(id)
                       select new ContractorStaffValidation
                       {
                           IDNVNT = n.IDNhanVienNT,
                           MaNV = n.MaNV,
                           CMND = n.SCMND,
                           HovaTen = n.HoTen,
                           NgaySinh = n.NgaySinh,
                           GioiTinh = (Gender)n.GioiTinh,
                           QuocTich = n.QuocTich
                       }).ToList();

            ContractorStaffValidation DO = new ContractorStaffValidation();
            if (res.Count > 0)
            {
                foreach (var n in res)
                {
                   DO.IDNVNT = n.IDNVNT;
                   DO.MaNV = n.MaNV;
                   DO.CMND = n.CMND;
                   DO.HovaTen = n.HovaTen;
                   DO.NgaySinh = n.NgaySinh;
                   DO.GioiTinh = (Gender)n.GioiTinh;
                   DO.QuocTich = n.QuocTich;
                }
                var genderList = Enum.GetValues(typeof(Gender))
                .OfType<Gender>()
                .Select(m => new
                {
                    Text = (m == Gender.Male) ? "Nam" : (m == Gender.Female) ? "Nữ" : "Không xác định",
                    Value = (int)m
                }).ToList();
                ViewBag.GTList = new SelectList(genderList, "Value", "Text");
                ViewBag.NgaySinh = DO.NgaySinh.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(ContractorStaffValidation _DO)
        {
            try
            {
                int gender = (_DO.GioiTinh == Gender.Male) ? 0 : (_DO.GioiTinh == Gender.Female) ? 1 : 2;
                db_context.NhanVienNT_update(_DO.IDNVNT, _DO.MaNV, _DO.CMND, _DO.HovaTen, _DO.ChucVu, _DO.NgaySinh, gender, _DO.QuocTich);

                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";

            }

            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại');</script>";
            }

            return RedirectToAction("Index", "ContractorStaff");
        }

        public ActionResult Delete(int id)
        {
            db_context.NhanVienNT_Delete(id);

            return RedirectToAction("Index", "ContractorStaff");
        }

        public ActionResult ImportExcel()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ImportExcel(ContractorStaffValidation _DO)
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
                    filePath = path + Path.GetFileName(DateTime.Now.ToString("yyyyMMddHHmm") + "-"+ file.FileName);

                    file.SaveAs(filePath);
                    Stream stream = file.InputStream;
                    // We return the interface, so that  
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
                            string BPQL = dt.Rows[2][1].ToString();
                            string SoHD = dt.Rows[3][1].ToString();
                            var IDHD = db_context.HopDongs.Where(x => x.SoHD == SoHD).Select(g => g.IDHD).FirstOrDefault();
                            List<ContractorValidation> contractors = (List<ContractorValidation>)ViewNhaThau(IDHD).Data;
                            var IDNhaThau = contractors[0].IDNhaThau;
                            var MaNT = contractors[0].MaNT;
                            
                            if (IsAvailable(BPQL, SoHD) == true)
                            {
                                for (int i = 6; i < dt.Rows.Count; i++)
                                {
                                
                                    if (dt.Rows[i] != null)
                                    {
                                        var cmnd = dt.Rows[i][0].ToString();
                                        var ten = dt.Rows[i][1].ToString();
                                        var chucvu = dt.Rows[i][2].ToString();
                                        var ngaysinh = DateTime.ParseExact(dt.Rows[i][3].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        var qt = dt.Rows[i][5].ToString();
                                        var gender = (dt.Rows[i][4].ToString() == "Nam") ? 0 : (dt.Rows[i][4].ToString() == "Nữ") ? 1 : 2;
                                        InsertKetQuaHoc(GenerateID(MaNT), cmnd, ten, chucvu, ngaysinh, gender, qt, IDHD, IDNhaThau);
                                    }
                                    dtc++;
                                }
                                string msg = "";
                                if (dtc != 0 && dtrung != 0)
                                {
                                    msg = "Import được " + dtc + " dòng dữ liệu, " + "Có " + dtrung + " dòng trùng Mã NV cập nhập nội dung";
                                }
                                else if (dtc != 0 && dtrung == 0)
                                {
                                    msg = "Import được " + dtc + " dòng dữ liệu";
                                }
                                else if (dtc == 0 && dtrung != 0)
                                {
                                    msg = "Có " + dtrung + " dòng trùng Mã NV cập nhập nội dung";
                                }
                                else { msg = "File import không có dữ liệu"; }


                                TempData["msgSuccess"] = "<script>alert('" + msg + "');</script>";
                            }
                            else
                            {
                                TempData["msgSuccess"] = "<script>alert('Bộ phận quản lý và mã số hợp đồng không đúng. Vui lòng kiểm tra lại.');</script>";
                            }
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

            return RedirectToAction("Index", "ContractorStaff");
        }

        public bool IsAvailable(string BPQL, string MaHD)
        {
            var IsCheck = (from u in db_context.PhongBans
                           join h in db_context.HopDongs
                           on u.IDPhongBan equals h.PhongBanID
                           where (u.TenVT.ToLower() == BPQL && h.SoHD.ToLower() == MaHD)
                           select new { u.TenVT, h.SoHD }).FirstOrDefault();
            bool status;
            if (IsCheck != null)
            {
                //Already registered  
                status = true;
            }
            else
            {
                //Available to use  
                status = false;
            }
            return status;
        }

        public void InsertKetQuaHoc(string MaNV, string CMND, string HoTen, string ChucVu, DateTime NgaySinh, int Gender, string QuocTich, int IDHD, int IDNhaThau)
        {
            ObjectParameter returnId = new ObjectParameter("IDNVNT", typeof(int));
            db_context.NhanVienNT_insert(MaNV, CMND, HoTen, ChucVu, NgaySinh, Gender, QuocTich, returnId);
            int IDNVNT = Convert.ToInt32(returnId.Value);
            bool kq = false;
            db_context.KetQuaHoc_insert(IDHD, IDNVNT, IDNhaThau, kq);
        }

        public JsonResult ViewNhaThau(int id)
        {
            List<ContractorValidation> NTList = (from h in db_context.HopDongs join n in db_context.NhaThaus on h.NhaThauID equals n.IDNhaThau
                                                 select new ContractorValidation()
                                                 {
                                                     IDHD = h.IDHD,
                                                     MaNT = n.MaNT,
                                                     MST = n.MST, 
                                                     DiaChi = n.DiaChi,
                                                     DienThoai = n.DienThoai,
                                                     Email = n.Email,
                                                     IDNhaThau = n.IDNhaThau,
                                                     Ten = n.Ten
                                                     
                                                 }).Where(x => x.IDHD == id).ToList();

            return Json(NTList, JsonRequestBehavior.AllowGet);
        }

        public string GenerateID(string MaNT)
        {
            int checkExists = db_context.NhanVienNTs.Count();
            var maxID = "00001";
            string idExists;
            if (checkExists > 0)
            {
                idExists = db_context.NhanVienNTs.OrderByDescending(x => x.IDNhanVienNT).FirstOrDefault().IDNhanVienNT.ToString();
                if(idExists.Length == 1)
                {
                    maxID = "0000" + idExists;
                }else if (idExists.Length == 2)
                {
                    maxID = "000" + idExists;
                }
                else if (idExists.Length == 3)
                {
                    maxID = "00" + idExists;
                }
                else if (idExists.Length == 4)
                {
                    maxID = "0" + idExists;
                }else
                {
                    maxID = idExists;
                }
            }

            return MaNT + "-" + maxID;
        }
    }

}