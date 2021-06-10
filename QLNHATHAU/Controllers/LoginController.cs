using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QLNHATHAU.Common;
using QLNHATHAU.Models;

namespace QLNHATHAU.Controllers
{
    public class LoginController : Controller
    {
        QLNhaThauEntities db_context = new QLNhaThauEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(UserLogin u)
        {
            string mk = Encryptor.MD5Hash(u.Password);
            NguoiDung user = db_context.NguoiDungs.Where(x => x.TenDangNhap == u.Username && x.MatKhau == mk).FirstOrDefault();
            if(user!=null)
            {
                string Cookie = string.Format("{0};{1};{2};{3};{4};{5}",user.IDNguoiDung,user.TenDangNhap,user.NhanVien.TenNV,user.PhongBanID,true, "");
                FormsAuthentication.SetAuthCookie(Cookie, u.RememberMe);
                //switch (userId.Value)
                //{
                //    case -1:
                //        message = "Username and/or password is incorrect.";
                //        break;
                //    case -2:
                //        message = "Account has not been activated.";
                //        break;
                //    default:
                //        return RedirectToAction("Profile");
                //}
               return  RedirectToAction("Index", "Home");
               
            }
            else {
                return View();
            } 
            
            
        }
    }
}