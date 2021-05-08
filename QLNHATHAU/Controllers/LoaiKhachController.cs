using PagedList;
using QLNHATHAU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNHATHAU.Controllers
{
    public class LoaiKhachController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: LoaiKhach
        public ActionResult Index(int? page)
        {
            var res = (from a in db_context.LK_Select()
                       select new LoaiKhachValidation
                       {
                           Id = a.IDLoaiKhach,
                           TenKhach = a.TenKhach

                       }).ToList();
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(res.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}