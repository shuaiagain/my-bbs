using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentData;
using BBS2018.Bussiness.ViewModel;
using BBS2018.Bussiness.Utils;
using System.Data.SqlClient;

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
        public PageVM<QuestionItemVM> GetQuestionPageList(QuestionQuery query)
        {
            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {
                List<SqlParameter> paramList = new List<SqlParameter>();
                string sql = string.Format(@"
                                            -- 选取每个问题点赞数量最多的回答
                                            drop table if EXISTS tempAnswer;												
                                            create TEMPORARY table tempAnswer 
                                            (
                                              select * from
	                                            (
			                                            select
				                                            ID as AnswerID,
				                                            QuestionID,
				                                            (select count(1) from bbspraisetread p where p.BindTableID = a.ID and p.BindTableName = 'bbsanswer' and p.PraiseOrTread = 1)as TotalPraise,
				                                            (select count(1) from bbspraisetread p where p.BindTableID = a.ID and p.BindTableName = 'bbsanswer' and p.PraiseOrTread = 2)as TotalTread
			                                            from bbsanswer a where QuestionID in (select ID from bbsquestion)
			                                            ORDER BY TotalPraise DESC
	                                            )x
	                                            GROUP BY x.QuestionID 
                                            );
                                         
	                                        select   
	                                        q.ID as QuestionID,
	                                        q.Title,
                                            q.UserID,
                                            q.UserName,
	                                        a.ID as AnswerID,
	                                        a.Content,
	                                        t.TotalPraise,
	                                        t.TotalTread
	                                        from bbsquestion q left join tempAnswer t on q.ID = t.QuestionID 
	                                        join bbsanswer a on t.AnswerID = a.ID 
	                                        Where 1=1 ");

                if (!string.IsNullOrEmpty(query.KeyWord))
                {
                    sql += " AND q.Title Like '%'+ @keyWord +'%' ";
                    paramList.Add(new SqlParameter("@keyWord", query.KeyWord));
                }

                sql += "  ORDER BY q.InputTime DESC ";

                string sqlPage = string.Format(" Limit {0},{1} ", (query.PageIndex - 1) * query.PageSize, query.PageSize);
                List<QuestionItemVM> quList = dbContext.Sql(sql + sqlPage, paramList.ToArray()).QueryMany<QuestionItemVM>((QuestionItemVM vm, IDataReader reader) =>
                {
                    vm.QuestionID = reader.GetInt64("QuestionID");
                    vm.AnswerID = reader.GetInt64("AnswerID");
                    vm.Title = reader.GetString("Title");
                    vm.Content = reader.GetString("Content");
                    vm.TotalPraise = Convert.ToInt32(reader["TotalPraise"]);
                    vm.TotalTread = Convert.ToInt32(reader["TotalTread"]);
                    vm.UserID = Convert.ToInt32(reader["UserID"]);
                    vm.UserName = reader.GetString("UserName");
                });

                //总条数
                int totalCount = dbContext.Sql(sql).QueryMany<int>().Count;
                //总页数
                int totalPages = (int)Math.Ceiling(((double)totalCount / query.PageSize.Value));

                PageVM<QuestionItemVM> pageData = new PageVM<QuestionItemVM>();
                pageData.TotalCount = totalCount;
                pageData.TotalPages = totalPages;
                pageData.Data = quList;

                return pageData;
            }
        }
        #endregion

        #region 获取问题详细
        /// <summary>
        /// 获取问题详细
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public BBSQuestionVM GetQuestionByID(int questionId)
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
