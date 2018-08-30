using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentData;
using BBS2018.Bussiness.ViewModel;
using BBS2018.Bussiness.Utils;

namespace BBS2018.Bussiness.Service
{
    public class BBSQuestionService
    {

        #region 保存问题
        public BBSQuestionVM SaveQuestion(BBSQuestionVM vm)
        {
            if (vm == null || string.IsNullOrEmpty(vm.Title)) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {
                vm.ID = dbContext.Insert("bbsquestion").Column("Title", vm.Title)
                                                 .Column("UserName", vm.UserName)
                                                 .Column("UserID", vm.UserID)
                                                 .Column("InputTime", vm.InputTime)
                                                 .ExecuteReturnLastId<int>();

                return vm;
            }
        } 
        #endregion
    }
}
