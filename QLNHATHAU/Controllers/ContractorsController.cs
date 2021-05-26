using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using PagedList;
using QLNHATHAU.Models;

namespace QLNHATHAU.Controllers
{
    public class ContractorsController : Controller
    {
        // GET: Contractors
       
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        public ActionResult Index(int? page)
        {
            var res = from a in db_context.NhaThau_list()
                      select new NhaThauValidation
                      {
                          IDNhaThau = a.IDNhaThau,
                          MaNT = a.MaNT,
                          MST = a.MST,
                          Ten = a.Ten,
                          DiaChi=a.DiaChi,
                          DienThoai=a.DienThoai,
                          Email=a.Email
                      };
            if (page == null) page = 1;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(res.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(NhaThauValidation _DO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db_context.Nhathau_insert(_DO.MaNT,_DO.MST,_DO.Ten,_DO.DiaChi,_DO.DienThoai,_DO.Email);
                }
                catch (Exception e)
                {
                    TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới nhà thầu: " + e.Message + "');</script>";
                }
            }
            return RedirectToAction("Index", "Contractors");
        }

        public ActionResult Edit(int id)
        {
            var res = (from dm in db_context.Nhathau_SearchByID(id)
                       select new NhaThauValidation
                       {
                           IDNhaThau = dm.IDNhaThau,
                           MaNT = dm.MaNT,
                           MST = dm.MST,
                           Ten = dm.Ten,
                           DiaChi=dm.DiaChi,
                           DienThoai=dm.DienThoai,
                           Email=dm.Email
                       }).ToList();
            NhaThauValidation DO = new NhaThauValidation();
            if (res.Count > 0)
            {
                foreach (var dm in res)
                {
                    DO.IDNhaThau = dm.IDNhaThau;
                    DO.MaNT = dm.MaNT;
                    DO.MST = dm.MST;
                    DO.Ten = dm.Ten;
                    DO.DiaChi = dm.DiaChi;
                    DO.DienThoai = dm.DienThoai;
                    DO.Email = dm.Email;
                }
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(DO);
        }

        [HttpPost]
        public ActionResult Edit(NhaThauValidation _DO)
        {
            try
            {
                db_context.Nhathau_update(_DO.IDNhaThau,_DO.MaNT, _DO.MST, _DO.Ten, _DO.DiaChi, _DO.DienThoai, _DO.Email);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Cập nhập thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Contractors");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                db_context.Nhathau_delete(id);
            }
            catch (Exception e)
            {
                TempData["msgSuccess"] = "<script>alert('Xóa dữ liệu thất bại: " + e.Message + "');</script>";
            }
            return RedirectToAction("Index", "Contractors");
        }
        public ActionResult ImportExcel()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ImportExcel(NhaThauValidation _DO)
        {
            string filePath = string.Empty;
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filePath = path + Path.GetFileName(file.FileName);

                    file.SaveAs(filePath);
                    Stream stream = file.InputStream;
                    // We return the interface, so that  
                    IExcelDataReader reader = null;
                    if (file.FileName.ToLower().EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (file.FileName.ToLower().EndsWith(".xlsx"))
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
                               if(CheckMaNT(dt.Rows[i][0].ToString()))
                                {
                                    db_context.Nhathau_insert(dt.Rows[i][0].ToString(), dt.Rows[i][4].ToString(), dt.Rows[i][2].ToString(), dt.Rows[i][3].ToString(), dt.Rows[i][5].ToString(), dt.Rows[i][6].ToString());
                                    dtc++;
                                }    
                                
                            }

                            string msg = "";
                            if (dtc != 0 && dtrung != 0)
                            {
                                msg = "Import được " + dtc + " dòng dữ liệu, " + "Có " + dtrung + " dòng trùng Mã HM cập nhập nội dung";
                            }
                            else if (dtc != 0 && dtrung == 0)
                            {
                                msg = "Import được " + dtc + " dòng dữ liệu";
                            }
                            else if (dtc == 0 && dtrung != 0)
                            {
                                msg = "Có " + dtrung + " dòng trùng Mã HM cập nhập nội dung";
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

            return RedirectToAction("Index", "Contractors");
        }
        public bool CheckMaNT(string MaNT)
        {
            var Regsbb = (from u in db_context.NhaThaus
                          where u.MaNT.ToLower() == MaNT.ToLower()
                          select new { u.MaNT }).FirstOrDefault();
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
    }
}