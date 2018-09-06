using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BBS2018.Bussiness.Service;
using BBS2018.Bussiness.ViewModel;

namespace BBS2018.Web.Controllers
{
    public class CommentController : BaseController
    {

        #region GetCommentList
        public ActionResult GetCommentList(long answerId)
        {
            if (answerId == 0) return HttpNotFound();

            ViewBag.AnswerID = answerId;
            return View("CommentList");
        }
        #endregion

        #region 获取评论
        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetComments(CommentQuery query)
        {
            if (!query.AnswerID.HasValue) return Json(new
            {
                Code = -400,
                Msg = "参数不能为空",
                Data = ""
            });

            if (!query.PageIndex.HasValue) query.PageIndex = 1;
            if (!query.PageSize.HasValue) query.PageSize = 10;

            PageVM<BBSCommentVM> pgVM = new BBSCommentService().GetComments(query);

            if (pgVM == null || pgVM.Data == null || pgVM.Data.Count == 0) return Json(new
            {
                Code = -200,
                Msg = "暂无数据",
                Data = ""
            });

            return Json(new
            {
                Code = 200,
                Msg = "获取成功",
                Data = pgVM
            });
        }
        #endregion

        #region SaveComment
        [HttpPost]
        public ActionResult SaveComment(BBSCommentVM vm)
        {

            if (vm == null || string.IsNullOrEmpty(vm.BindTableName) || !vm.BindTableID.HasValue)
                return Json(new
            {
                Code = -400,
                Msg = "参数不能为空",
                Data = ""
            });

            vm.UserName = this.UserData.UserName;
            vm.UserID = this.UserData.UserID;
            vm.InputTime = DateTime.Now;
            vm = new BBSCommentService().SaveComment(vm);

            return Json(new
            {
                Code = 200,
                Msg = "保存成功",
                Data = vm
            });
        }
        #endregion

    }
}
