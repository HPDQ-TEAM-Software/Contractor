using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace QLNHATHAU.Models
{
    public class MyAuthentication
    {
        public static void ClearAuthentication()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
        }

       


        public static string UserRole
        {
            get
            {
                try
                {
                    object obj = HttpContext.Current.User.Identity.Name.Split(';')[4];
                    return (obj == null) ? String.Empty : (string)obj;
                }
                catch
                {
                    return null;
                }
            }

        }

        

        public static string Username
        {
            get
            {
                try
                {
                    object obj = HttpContext.Current.User.Identity.Name.Split(';')[2];
                    return (obj == null) ? String.Empty : (string)obj;
                }
                catch
                {
                    return null;
                }
            }

        }
        public static bool IsLogin
        {
            get
            {
                try
                {

                    object obj = HttpContext.Current.User.Identity.Name.Split(';')[1];
                    return (obj == null) ? false : Convert.ToBoolean(obj);
                }
                catch
                {
                    return false;
                }
            }
        }
        public static int IDLogon
        {
            get
            {
                try
                {
                    object obj = HttpContext.Current.User.Identity.Name.Split(';')[0];
                    return (obj == null) ? 0 : Convert.ToInt32(obj);
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}