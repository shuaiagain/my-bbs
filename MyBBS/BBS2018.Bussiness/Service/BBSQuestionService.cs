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
				                                            (select count(1) from bbspraisetread p where p.BindTableID = a.ID and p.BindTableName = 'bbsanswer' and p.PraiseOrTread = 2)as TotalTread,
                                                            (
			                                                    select count(*) from bbspraisetread p where p.BindTableID = a.ID and p.BindTableName = 'bbsanswer' and p.PraiseOrTread = 1 
			                                                    and p.UserID = {0}
		                                                     )as IsPraised,
                                                             (
			                                                    select count(*) from bbspraisetread p where p.BindTableID = a.ID and p.BindTableName = 'bbsanswer' and p.PraiseOrTread = 2 
			                                                    and p.UserID = {0}
		                                                     )as IsTreaded
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
	                                            t.TotalTread,
                                                t.IsPraised,
                                                t.IsTreaded
	                                        from bbsquestion q left join tempAnswer t on q.ID = t.QuestionID 
	                                        left join bbsanswer a on t.AnswerID = a.ID 
	                                        Where 1=1 ", query.UserID);

                if (!string.IsNullOrEmpty(query.KeyWord))
                {
                    sql += string.Format(@" AND q.Title Like '%{0}%' ", query.KeyWord);
                }

                sql += "  ORDER BY q.InputTime DESC ";

                string sqlPage = string.Format(" Limit {0},{1} ", (query.PageIndex - 1) * query.PageSize, query.PageSize);
                List<QuestionItemVM> quList = dbContext.Sql(sql + sqlPage).QueryMany<QuestionItemVM>((QuestionItemVM vm, IDataReader reader) =>
                {
                    vm.QuestionID = reader.GetInt64("QuestionID");
                    vm.AnswerID = reader.GetInt64("AnswerID");
                    vm.Title = reader.GetString("Title");
                    vm.Content = string.IsNullOrEmpty(reader.GetString("Content")) ? "" : reader.GetString("Content");
                    vm.TotalPraise = Convert.ToInt32(reader["TotalPraise"]);
                    vm.TotalTread = Convert.ToInt32(reader["TotalTread"]);
                    vm.UserID = Convert.ToInt32(reader["UserID"]);
                    vm.UserName = reader.GetString("UserName");
                    vm.IsPraised = Convert.ToInt32(reader["IsPraised"]);
                    vm.IsTreaded = Convert.ToInt32(reader["IsTreaded"]);
                });

                //总条数
                int totalCount = dbContext.Sql(sql).QueryMany<int>().Count;
                //总页数
                int totalPages = (int)Math.Ceiling(((double)totalCount / query.PageSize.Value));

                PageVM<QuestionItemVM> pageData = new PageVM<QuestionItemVM>()
                {
                    TotalCount = totalPages,
                    TotalPages = totalPages,
                    Data = quList
                };

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
        public QuestionDetailVM GetQuestionByID(int questionId)
        {

            if (questionId == 0) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {
                QuestionDetailVM question = dbContext.Sql(@" select 
	                                                            ID,	                                                            Title,
	                                                            (select count(1) from bbsanswer a where a.QuestionID = q.ID )as AnswerCount
                                                              from bbsquestion q where q.ID = @questionId ")
                                                  .Parameter("questionId", questionId)
                                                  .QuerySingle<QuestionDetailVM>((QuestionDetailVM vm, IDataReader reader) =>
                                                  {
                                                      vm.ID = reader.GetInt64("ID");
                                                      vm.Title = reader.GetString("Title");
                                                      vm.AnswerCount = Convert.ToInt32(reader["AnswerCount"]);
                                                  });

                if (question == null) return null;

                //todo
                List<QuesitonDetailItemVM> itemList = dbContext.Sql(@"select 
	                                                                    a.ID as AnswerID,
	                                                                    a.Content,
	                                                                    a.InputTime as EditTime,
	                                                                    (select Count(1) from bbspraisetread p where p.BindTableID = a.ID and p.BindTableName = 'bbsanswer' and p.PraiseOrTread = 1)as PraiseCount,
	                                                                    (select Count(1) from bbspraisetread p where p.BindTableID = a.ID and p.BindTableName = 'bbsanswer' and p.PraiseOrTread = 2)as TreadCount,
	                                                                    u.ID as UserID,
	                                                                    u.LoginName as UserName,
	                                                                    u.HeadImageUrl as LogoUrl
                                                                    from bbsanswer a join bbsuser u on a.UserID = u.ID
                                                                    where a.QuestionID = @questionID ")
                                                                    .Parameter("questionID", question.ID)
                                                                    .QueryMany<QuesitonDetailItemVM>
                                                                    ((QuesitonDetailItemVM vm, IDataReader reader) =>
                                                                    {
                                                                        vm.AnswerID = reader.GetInt64("AnswerID");
                                                                        vm.Content = reader.GetString("Content");
                                                                        vm.EditTime = reader.GetDateTime("EditTime");
                                                                        vm.PraiseCount = Convert.ToInt32(reader["PraiseCount"]);
                                                                        vm.TreadCount = Convert.ToInt32(reader["TreadCount"]);
                                                                        vm.UserID = reader.GetInt32("UserID");
                                                                        vm.UserName = reader.GetString("UserName");
                                                                        vm.LogoUrl = reader.GetString("LogoUrl");
                                                                    });

                question.Items = itemList;
                return question;
            }
        }
        #endregion

        #region 获取问题详细页列表
        public PageVM<QuesitonDetailItemVM> GetQAnswerPageList(QuestionQuery query)
        {

            if (!query.QuestionID.HasValue) return null;

            if (!query.PageSize.HasValue) query.PageSize = 10;

            if (!query.PageIndex.HasValue) query.PageIndex = 1;

            string sql = string.Format(@"select * from 
                                        (
	                                        select 
		                                        a.ID as AnswerID,
		                                        a.Content,
		                                        a.InputTime as EditTime,
		                                        (select Count(1) from bbspraisetread p where p.BindTableID = a.ID and p.BindTableName = 'bbsanswer' and p.PraiseOrTread = 1)as PraiseCount,
		                                        (select Count(1) from bbspraisetread p where p.BindTableID = a.ID and p.BindTableName = 'bbsanswer' and p.PraiseOrTread = 2)as TreadCount,
                                                (
			                                        select count(*) from bbspraisetread p where  p.BindTableID = a.ID and p.BindTableName = 'bbsanswer' and p.PraiseOrTread = 1 
			                                        and p.UserID = {1}
		                                        )as IsPraised,
		                                        (
			                                        select count(*) from bbspraisetread p where  p.BindTableID = a.ID and p.BindTableName = 'bbsanswer' and p.PraiseOrTread = 2 
			                                        and p.UserID = {1} 
		                                        )as IsTreaded,
		                                        u.ID as UserID,
		                                        u.LoginName as UserName,
		                                        u.HeadImageUrl as LogoUrl
	                                        from bbsanswer a join bbsuser u on a.UserID = u.ID
	                                        where a.QuestionID = {0}
                                        )x  ", query.QuestionID, query.UserID);
            sql += " ORDER BY x.EditTime desc ,x.PraiseCount desc ";

            string sqlPage = string.Format(" Limit {0},{1}", query.PageSize * (query.PageIndex - 1), query.PageSize);

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {

                List<QuesitonDetailItemVM> itemList = dbContext.Sql(sql)
                                                               .QueryMany<QuesitonDetailItemVM>
                                                               ((QuesitonDetailItemVM vm, IDataReader reader) =>
                                                               {
                                                                   vm.AnswerID = reader.GetInt64("AnswerID");
                                                                   vm.Content = reader.GetString("Content");
                                                                   vm.EditTime = reader.GetDateTime("EditTime");
                                                                   vm.PraiseCount = Convert.ToInt32(reader["PraiseCount"]);
                                                                   vm.TreadCount = Convert.ToInt32(reader["TreadCount"]);
                                                                   vm.UserID = reader.GetInt32("UserID");
                                                                   vm.UserName = reader.GetString("UserName");
                                                                   vm.LogoUrl = reader.GetString("LogoUrl");
                                                               });

                if (itemList == null || itemList.Count == 0) return null;

                //总条数
                int totalCount = dbContext.Sql(sql).QueryMany<int>().Count;
                //总页数
                int totalPages = (int)Math.Ceiling(((double)totalCount / query.PageSize.Value));

                PageVM<QuesitonDetailItemVM> pageVM = new PageVM<QuesitonDetailItemVM>()
                {
                    Data = itemList,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };

                return pageVM;
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
