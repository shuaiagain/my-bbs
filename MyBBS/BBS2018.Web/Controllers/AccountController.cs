using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BBS2018.Bussiness.ViewModel;
using BBS2018.Bussiness.Service;

namespace BBS2018.Web.Controllers
{
    public class AccountController : Controller
    {

        #region Register
        public ActionResult Register()
        {
            return View();
        }
        #endregion

        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(BBSUserVM user)
        {

            if (user == null || string.IsNullOrEmpty(user.LoginName) || string.IsNullOrEmpty(user.Password))
            {
                return Json(new
                {
                    Code = -400,
                    Msg = "参数不能为空",
                    Data = ""
                });
            }

            try
            {
                BBSUserService userSV = new BBSUserService();
                if (userSV.IsLoginNameExist(user.LoginName))
                {
                    return Json(new
                    {
                        Code = -200,
                        Msg = "用户名已存在",
                        Data = ""
                    });
                }

                user.InputTime = DateTime.Now;
                user = userSV.Register(user);

                MyFormsAuthentication.SetAuthCookie(user.LoginName, new MyFormsAuthentication() { UserID = user.ID, UserName = user.LoginName }, false);

                return Json(new
                {
                    Code = 200,
                    Msg = "注册成功",
                    Data = user
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion
    }
}
