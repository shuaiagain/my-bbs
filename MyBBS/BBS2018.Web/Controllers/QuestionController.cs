using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BBS2018.Bussiness.Service;
using BBS2018.Bussiness.ViewModel;

namespace BBS2018.Web.Controllers
{
    public class QuestionController : BaseController
    {

        #region 获取问题列表
        /// <summary>
        /// 获取问题列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetQuestionList(QuestionQuery query)
        {
            if (query == null) new QuestionQuery();

            if (!query.PageSize.HasValue) query.PageSize = 10;
            if (!query.PageIndex.HasValue) query.PageIndex = 1;

            query.UserID = this.UserData.UserID;

            try
            {
                PageVM<QuestionItemVM> quData = new BBSQuestionService().GetQuestionPageList(query);
                if (quData == null || quData.Data == null || quData.Data.Count == 0)
                {
                    return Json(new
                    {
                        Code = -200,
                        Msg = "暂无数据",
                        Data = quData
                    });
                }

                return Json(new
                {
                    Code = 200,
                    Msg = "获取成功",
                    Data = quData
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region AskQuestion
        public ActionResult AskQuestion()
        {
            return View();
        }
        #endregion

        #region 保存提问
        /// <summary>
        /// 保存提问
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveQuestion(BBSQuestionVM vm)
        {
            if (vm == null || string.IsNullOrEmpty(vm.Title))
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
                vm.UserID = this.UserData.UserID;
                vm.UserName = this.UserData.UserName;
                vm.InputTime = DateTime.Now;

                vm = new BBSQuestionService().SaveQuestion(vm);
                return Json(new
                {
                    Code = 200,
                    Msg = "保存成功",
                    Data = vm
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region QuestionDetail
        public ActionResult QuestionDetail(int? questionId)
        {
            if (!questionId.HasValue) return HttpNotFound();

            QuestionDetailVM quVM = new BBSQuestionService().GetQuestionByID(questionId.Value);

            if (quVM == null) return HttpNotFound();

            return View(quVM);
        }
        #endregion

        #region getQAnswerPageList
        /// <summary>
        /// 获取问题答案列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetQAnswerPageList(QuestionQuery query)
        {

            if (query == null) query = new QuestionQuery();
            if (!query.QuestionID.HasValue) return Json(new
                     {
                         Code = -400,
                         Msg = "参数不能为空",
                         Data = ""
                     });

            PageVM<QuesitonDetailItemVM> quVM = new BBSQuestionService().GetQAnswerPageList(query);

            if (quVM == null) return Json(new
            {
                Code = -200,
                Msg = "暂无数据",
                Data = ""
            });

            return Json(new
            {
                Code = 200,
                Msg = "获取成功",
                Data = quVM
            });
        }
        #endregion
    }
}
