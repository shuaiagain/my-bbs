using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BBS2018.Bussiness.Service;
using BBS2018.Bussiness.ViewModel;

namespace BBS2018.Web.Controllers
{
    public class AnswerController : BaseController
    {
        #region Index
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region WriteAnswer
        public ActionResult WriteAnswer()
        {
            return View();
        }
        #endregion

        #region SaveAnswer
        public ActionResult SaveAnswer(BBSAnswerVM answerVM)
        {
            if (answerVM == null || string.IsNullOrEmpty(answerVM.Content))
            {
                return Json(new
                {
                    Code = -400,
                    Msg = "回答内容不能为空",
                    Data = ""
                });
            }

            if (!answerVM.QuestionID.HasValue)
            {
                return Json(new
               {
                   Code = -400,
                   Msg = "问题ID不能为空",
                   Data = ""
               });
            }

            try
            {
                answerVM.UserName = this.UserData.UserName;
                answerVM.UserID = this.UserData.UserID;
                answerVM.InputTime = DateTime.Now;

                BBSAnswerService answerSV = new BBSAnswerService();
                answerSV.SaveAnswer(answerVM);

                return Json(new
                {
                    Code = 200,
                    Msg = "保存成功",
                    Data = answerVM
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
