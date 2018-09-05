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
    public class BBSCommentService
    {

        public PageVM<BBSCommentVM> GetComments(CommentQuery query)
        {
            if (!query.AnswerID.HasValue) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {

                string sql = @" select * from bbscomment c where c.BindTableName = 'bbsanswer' and c.BindTableID = @answerId ";

                string sqlPage = string.Format(@" limit {0},{1}", (query.PageIndex - 1) * query.PageSize, query.PageSize);
                List<BBSCommentVM> commentList = dbContext.Sql(sql)
                                                        .Parameter("answerId", query.AnswerID)
                                                        .QueryMany<BBSCommentVM>((BBSCommentVM vm, IDataReader reader) =>
                                                        {
                                                            vm.BindTableID = reader.GetInt64("BindTableID");
                                                            vm.BindTableName = reader.GetString("BindTableName");
                                                            vm.UserID = reader.GetInt32("UserID");
                                                            vm.UserName = reader.GetString("UserName");
                                                            vm.Content = reader.GetString("Content");
                                                            vm.InputTime = reader.GetDateTime("InputTime");
                                                        });

                if (commentList == null || commentList.Count == 0) return null;

                //总条数
                int totalCount = dbContext.Sql(sql).QueryMany<int>().Count;
                //总页数
                int totalPages = (int)Math.Ceiling(((double)totalCount / query.PageSize.Value));

                PageVM<BBSCommentVM> pgVM = new PageVM<BBSCommentVM>()
                {
                    Data = commentList,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };

                return pgVM;
            }
        }

    }
}
