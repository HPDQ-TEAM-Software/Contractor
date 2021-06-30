using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class SecurityController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Security
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


        public ActionResult Security(int? id)
        {
            var res = (from a in db_context.DKKhaches.Where(x => x.IDDangKyKH == id)
                       join lk in db_context.LoaiKhaches on a.LoaiKhachID equals lk.IDLoaiKhach
                       join c in db_context.Congs on a.CongID equals c.IDCONG
                       join nt in db_context.NhaThaus on a.NhaThauID equals nt.IDNhaThau
                       select new RegistrationValidation()
                       {
                           IDDangKyKH = a.IDDangKyKH,
                           NhaThauID = (int)nt.IDNhaThau,
                           TenNhaThau = nt.Ten,
                           LoaiKhachID = (int)a.LoaiKhachID,
                           TenKhach = lk.TenLoai,
                           CongID = (int)a.CongID,
                           TenCong = c.TenCong,
                           NguoiDaiDien = a.NguoiDaiDien,
                           PhuongTien = a.PhuongTien,
                           BienSo = a.BienSo,
                           NgayBL = a.NgayBL
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
                return View(DO);
            }
            return View();
        }

        public ActionResult List (int id)
        {
            var res = from a in db_context.DSKhaches.Where(x => x.DangKyKHID == id)
                        select new RegistrationlistValidation()
                        {
                            IDKhach = a.IDKhach,
                            DangKyKHID = (int)a.DangKyKHID,
                            CMND = a.CMND,
                            HoTen = a.HoTen
                        };
          return PartialView(res.ToList());
        }
    }
}