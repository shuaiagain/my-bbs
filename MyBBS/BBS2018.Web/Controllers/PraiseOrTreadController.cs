using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BBS2018.Bussiness.Service;
using BBS2018.Bussiness.ViewModel;

namespace BBS2018.Web.Controllers
{
    public class PraiseOrTreadController : BaseController
    {

        #region 保存赞/踩
        /// <summary>
        /// 保存赞/踩
        /// </summary>
        /// <param name="ptVM"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SavePraiseOrTread(BBSPraiseTreadVM ptVM)
        {
            if (ptVM == null || !ptVM.BindTableID.HasValue || string.IsNullOrEmpty(ptVM.BindTableName))
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

                ptVM.UserID = this.UserData.UserID;
                ptVM.InputTime = DateTime.Now;
                PraiseTreadItemVM item = new BBSPraiseTreadService().SavePraiseOrTread(ptVM);

                if (item == null)
                {
                    return Json(new
                    {
                        Code = -400,
                        Msg = "参数不能为空",
                        Data = ""
                    });
                }

                return Json(new
                {
                    Code = 200,
                    Msg = "操作成功",
                    Data = item
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
