﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BBS2018.Web.Controllers
{
    public class HomeController : BaseController
    {

        #region Index
        public ActionResult Index()
        {
            return View();
        } 
        #endregion
     
    }
}
