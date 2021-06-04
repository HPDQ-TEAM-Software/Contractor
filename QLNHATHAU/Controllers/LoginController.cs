using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QLNHATHAU.Models;

namespace QLNHATHAU.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(UserLogin u)
        {
            //UsersEntities usersEntities = new UsersEntities();
            //int? userId = usersEntities.ValidateUser(user.Username, user.Password).FirstOrDefault();

            //string message = string.Empty;
            //switch (userId.Value)
            //{
            //    case -1:
            //        message = "Username and/or password is incorrect.";
            //        break;
            //    case -2:
            //        message = "Account has not been activated.";
            //        break;
            //    default:
            //        FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
            //        return RedirectToAction("Profile");
            //}

            //ViewBag.Message = message;

            //string idName = string.Format("{0};{1};{2};{3};{4};{5}", logon, true, username.ToLower(), group, "", true);
            //FormsAuthentication.SetAuthCookie(idName, true);
            return View();
        }
    }
}