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

        public ActionResult Security(int? page, int? id)
        {
            var res = (from a in db_context.DKKhaches.Where(x => x.IDDangKyKH == id)
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