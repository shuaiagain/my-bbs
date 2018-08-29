using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Principal;
using System.Web.Security;
using Newtonsoft.Json;

namespace BBS2018.Web
{
    #region MyFormPrincipal
    public class MyFormPrincipal : IPrincipal
    {
        public MyFormPrincipal(FormsAuthenticationTicket ticket, MyFormsAuthentication userData)
        {
            if (ticket == null) throw new ArgumentNullException("ticket");

            if (userData == null) throw new ArgumentNullException("userData");

            this.Identity = new FormsIdentity(ticket);
            this.UserData = userData;
        }

        public MyFormsAuthentication UserData { get; set; }
        public IIdentity Identity { get; set; }

        public bool IsInRole(string role)
        {
            return false;
        }
    } 
    #endregion

    #region MyFormsAuthentication
    public class MyFormsAuthentication
    {
        public int? UserID { get; set; }

        public string UserName { get; set; }

        public string Roles { get; set; }

        public static void SetAuthCookie(string userName, MyFormsAuthentication userData, bool isPersistent)
        {

            if (userData == null) throw new ArgumentNullException("userData");

            string data = JsonConvert.SerializeObject(userData);
            DateTime expiration = isPersistent ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(1);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, expiration, isPersistent, data);
            string cookieValue = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)
            {
                HttpOnly = false,
                Domain = FormsAuthentication.CookieDomain,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath,
                Expires = expiration
            };

            HttpContext curHttp = HttpContext.Current;
            if (curHttp == null) new InvalidOperationException();

            curHttp.Response.Cookies.Remove(cookie.Name);
            curHttp.Response.Cookies.Add(cookie);
        }

        public static void SignOut()
        {
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
            {
                HttpOnly = true,
                Domain = FormsAuthentication.CookieDomain,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath,
                Expires = DateTime.Now.AddDays(-1)
            };

            HttpContext curHttp = HttpContext.Current;
            if (curHttp == null) throw new InvalidOperationException();

            curHttp.Response.Cookies.Add(cookie);
        }

        #region TryParsePrincipal（用于Global.asax中）
        public static MyFormPrincipal TryParsePrincipal(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");

            try
            {
                HttpCookie cookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie == null) return null;

                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket == null || string.IsNullOrEmpty(ticket.UserData)) return null;

                MyFormsAuthentication userData = JsonConvert.DeserializeObject<MyFormsAuthentication>(ticket.UserData);
                if (userData == null) return null;

                return new MyFormPrincipal(ticket, userData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    } 
    #endregion

}