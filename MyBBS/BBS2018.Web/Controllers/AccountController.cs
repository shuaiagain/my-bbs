using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BBS2018.Bussiness.ViewModel;
using BBS2018.Bussiness.Service;
using BBS2018.Web.Filters;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using BBS2018.Bussiness.Utils;

namespace BBS2018.Web.Controllers
{
    public class AccountController : Controller
    {

        #region Register
        [AuthFilter]
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

        #region Login
        [AuthFilter]
        public ActionResult Login()
        {
            return View();
        }
        #endregion

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(BBSUserVM user)
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
                user = userSV.Login(user);
                if (user == null)
                {
                    return Json(new
                    {
                        Code = -200,
                        Msg = "用户名不存在",
                        Data = ""
                    });
                }

                MyFormsAuthentication.SetAuthCookie(user.LoginName, new MyFormsAuthentication() { UserID = user.ID, UserName = user.LoginName }, false);

                return Json(new
                {
                    Code = 200,
                    Msg = "登录成功",
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

        #region 退出
        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            try
            {
                MyFormsAuthentication.SignOut();
                return Redirect("~/Account/Login");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 忘记密码
        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetPwd()
        {
            return View();
        }
        #endregion

        #region 获取验证码图片
        public ActionResult GetVerifyCode()
        {
            VerifyCodeHelper verify = new VerifyCodeHelper();
            string verifyCode = verify.CreateVerifyCode(4);

            Session["VerifyCode"] = verifyCode;

            byte[] graphicByte = verify.CreateGraphic(verifyCode);
            return File(graphicByte, @"image/jepg");
        }
        #endregion

        #region 发邮件重置密码
        [HttpPost]
        public ActionResult SendEmail(string email, string verifyCode)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(verifyCode))
            {
                return Json(new
                {
                    Code = -400,
                    Msg = "参数不能为空",
                    Data = ""
                });
            }

            if (Session["VerifyCode"] != null && Session["VerifyCode"].ToString() != verifyCode)
            {
                return Json(new
                {
                    Code = -400,
                    Msg = "验证码错误",
                    Data = ""
                });
            }

            string supplier = GetEmailSupplier(email);
            if (string.IsNullOrEmpty(supplier))
            {
                return Json(new
                {
                    Code = -200,
                    Msg = "邮箱无效",
                    Data = ""
                });
            }


            bool isSendOk = SendEmail("测试mine", email, "大少你小心了，这只是一个测试！！！");

            return Json(new
            {
                Code = 200,
                Msg = "发送成功",
                Data = new
                {
                    Email = email,
                    EmailSupplier = supplier
                }
            });
        }
        #endregion

        #region 获取邮箱供应商
        public string GetEmailSupplier(string email)
        {

            string temp = email.Split('@')[1];
            string supplier = "";

            switch (temp)
            {
                case "gmail.com":
                    supplier = "https://accounts.google.com";
                    break;
                case "qq.com":
                case "stockstar.com":
                    supplier = "https://mail.qq.com";
                    break;
                case "yahoo.com":
                    supplier = "https://overview.mail.yahoo.com";
                    break;
                case "msn.com":
                    supplier = "https://outlook.live.com/owa";
                    break;
                case "hotmail.com":
                    supplier = "https://outlook.live.com/owa";
                    break;
                case "163.net":
                    supplier = "http://mail1.tom.com";
                    break;
                case "163.com":
                    supplier = "http://mail.163.com";
                    break;
                case "yeah.net":
                    supplier = "https://www.yeah.net";
                    break;
                default:
                    break;
            }

            return supplier;
        }
        #endregion

        #region SendEmail
        public bool SendEmail(string title, string email, string content)
        {
            //使用163代理邮箱服务器（也可是使用qq的代理邮箱服务器，但需要与具体邮箱对相应）
            string senderServerIp = "smtp.163.com";
            //string senderServerIp = "smtp.qq.com";
            //string senderServerIp = "smtp.exmail.qq.com";

            //要发送对象的邮箱
            string toMailAddress = email;
            //你的邮箱
            string fromMailAddress = "manygrass@163.com";
            //string fromMailAddress = "shuaishuai.ding@stockstar.com";
            //主题
            string subjectInfo = title;
            //以Html格式发送的邮件
            string bodyInfo = "<p>" + content + "</p>";
            //登录邮箱的用户名
            string mailUsername = "manygrass";
            //对应的登录邮箱的第三方密码（你的邮箱不论是163还是qq邮箱，都需要自行开通stmp服务）
            string mailPassword = "";
            //发送邮箱的端口号
            int mailPort = 25;
            //int mailPort = 465;

            //创建发送邮箱的对象
            EmailUtil myEmail = new EmailUtil(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);

            return myEmail.SendEmail();
        } 
        #endregion
    }
}
