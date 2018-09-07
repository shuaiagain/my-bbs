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

        #region GetComments
        public PageVM<BBSCommentVM> GetComments(CommentQuery query)
        {
            if (!query.AnswerID.HasValue) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {

                string sql = string.Format(@" 
                                DROP PROCEDURE IF EXISTS `Pro_GetComments`;
                                DROP PROCEDURE IF EXISTS `Pro_RecurseGetReply`;

                                -- 获取所有评论的存储过程
                                CREATE PROCEDURE Pro_GetComments(IN answerId BIGINT)
                                BEGIN
	
	                                DECLARE Finish INT DEFAULT 0;
	                                DECLARE CommentID BIGINT DEFAULT 0;

	                                DECLARE cur_comment CURSOR FOR  SELECT ID FROM bbscomment WHERE BindTableName = 'bbscomment' AND BindTableID = answerId ;
	                                DECLARE CONTINUE HANDLER FOR NOT FOUND SET Finish = 1;
	
	                                DROP TABLE IF EXISTS TempComment;
	                                CREATE TEMPORARY TABLE TempComment SELECT ID FROM bbscomment WHERE BindTableName = 'bbsanswer' AND BindTableID = answerId;
	
	                                OPEN cur_comment;
	                                FETCH cur_comment INTO CommentID ;
	
	                                CALL Pro_RecurseGetReply(CommentID);
	                                CLOSE cur_comment;
	
	                              	SELECT  * FROM bbscomment b where b.ID in 
	                                (
		                                select ID from TempComment
	                                );
                                END
                                ;
                                -- 递归获取评论
                                CREATE PROCEDURE Pro_RecurseGetReply(in rootId BIGINT)
                                BEGIN
		
		                                DECLARE Done BIGINT DEFAULT 1 ;
		                                DECLARE TempID BIGINT DEFAULT 0;
		
		                                DECLARE cur_reply CURSOR FOR SELECT c.ID FROM bbscomment c
		                                WHERE c.BindTableName = 'bbscomment' and c.BindTableID = rootId;
		                                DECLARE CONTINUE HANDLER FOR NOT FOUND SET Done = 1;
		                                OPEN cur_reply;
		                                FETCH cur_reply INTO TempID ;
			                                INSERT INTO TempComment VALUES(TempID);
		                                WHILE TempID!=NULL DO
			                                CALL  Pro_RecurseFindComment(TempID);
		                                END WHILE;

		                                CLOSE cur_reply;
                                END
                                ;
                                call Pro_GetComments({0}); ", query.AnswerID);

                string sqlPage = string.Format(@" limit {0},{1}", (query.PageIndex - 1) * query.PageSize, query.PageSize);
                List<BBSCommentVM> commentList = dbContext.Sql(sql)
                                                        .QueryMany<BBSCommentVM>((BBSCommentVM vm, IDataReader reader) =>
                                                        {
                                                            vm.ID = reader.GetInt64("ID");
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
        #endregion

        #region SaveComment
        public BBSCommentVM SaveComment(BBSCommentVM vm)
        {

            if (vm == null || string.IsNullOrEmpty(vm.BindTableName) || !vm.BindTableID.HasValue) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {

                vm.ID = dbContext.Insert("bbscomment").Column("BindTableID", vm.BindTableID)
                                                      .Column("BindTableName", vm.BindTableName)
                                                      .Column("UserID", vm.UserID)
                                                      .Column("UserName", vm.UserName)
                                                      .Column("InputTime", vm.InputTime)
                                                      .Column("Content", vm.Content)
                                                      .ExecuteReturnLastId<long>();

                return vm;
            }
        }
        #endregion
    }
}
