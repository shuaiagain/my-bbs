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

        #region 获取问题列表
        /// <summary>
        /// 获取问题列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<BBSQuestionVM> GetQuestionPageList(QuestionQuery query)
        {
            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {

                return null;
            }
        }
        #endregion

        #region 获取问题详细
        /// <summary>
        /// 获取问题详细
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public BBSQuestionVM GetQuestion(int questionId)
        {
            if (questionId == 0) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {
                BBSQuestionVM vm = dbContext.Sql(@" select * from bbsquestion q where q.ID = @questionId ")
                                          .Parameter("questionId", questionId)
                                          .QuerySingle<BBSQuestionVM>();

                return vm;
            }
        }
        #endregion

        #region 保存问题
        /// <summary>
        /// 保存问题
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public BBSQuestionVM SaveQuestion(BBSQuestionVM vm)
        {
            if (vm == null || string.IsNullOrEmpty(vm.Title)) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {
                vm.ID = dbContext.Insert("bbsquestion").Column("Title", vm.Title)
                                                       .Column("UserName", vm.UserName)
                                                       .Column("UserID", vm.UserID)
                                                       .Column("InputTime", vm.InputTime)
                                                       .ExecuteReturnLastId<long>();

                return vm;
            }
        }
        #endregion
    }
}
