using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace BBS2018.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public override void Init()
        {
            base.Init();
            base.AuthorizeRequest += MvcApplication_AuthorizeRequest;
        }

        private void MvcApplication_AuthorizeRequest(object sender, EventArgs e)
        {
            MyFormPrincipal principal = MyFormsAuthentication.TryParsePrincipal(this.Context);

            if (principal == null || principal.UserData == null) return;

            this.Context.User = principal;
        }

    }
}