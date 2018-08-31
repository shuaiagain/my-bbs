using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BBS2018.Bussiness.ViewModel;
using FluentData;
using BBS2018.Bussiness.Utils;

namespace BBS2018.Bussiness.Service
{
    public class BBSAnswerService
    {
        #region 保存问题答案
        /// <summary>
        /// 保存问题答案
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public BBSAnswerVM SaveAnswer(BBSAnswerVM vm)
        {
            if (vm == null || string.IsNullOrEmpty(vm.Content)) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {
                vm.ID = dbContext.Insert("bbsanswer").Column("Content", vm.Content)
                                                     .Column("UserName", vm.UserName)
                                                     .Column("UserID", vm.UserID)
                                                     .Column("QuestionID", vm.QuestionID)
                                                     .Column("InputTime", vm.InputTime)
                                                     .ExecuteReturnLastId<long>();

                return vm;
            }
        }
        #endregion
    }
}
