using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BBS2018.Bussiness.ViewModel;
using FluentData;
using BBS2018.Bussiness.Utils;

namespace BBS2018.Bussiness.Service
{
    public class BBSPraiseTreadService
    {

        #region 保存赞/踩
        /// <summary>
        /// 保存赞/踩
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public PraiseTreadItemVM SavePraiseOrTread(BBSPraiseTreadVM ptVM)
        {
            if (ptVM == null || !ptVM.BindTableID.HasValue || string.IsNullOrEmpty(ptVM.BindTableName)) return null;

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()))
            {

                dbContext.Sql(@" delete from  bbspraisetread where BindTableID = @bindTableID and BindTableName = @bindTableName and UserID = @userID ")
                         .Parameter("bindTableID", ptVM.BindTableID)
                         .Parameter("bindTableName", ptVM.BindTableName)
                         .Parameter("userID", ptVM.UserID)
                         .Execute();

                dbContext.Insert("bbspraisetread").Column("PraiseOrTread", ptVM.PraiseOrTread)
                                                  .Column("UserID", ptVM.UserID)
                                                  .Column("BindTableName", ptVM.BindTableName)
                                                  .Column("BindTableID", ptVM.BindTableID)
                                                  .Column("InputTime", ptVM.InputTime)
                                                  .ExecuteReturnLastId<long>();

                int count = dbContext.Sql(@" select 1 from bbspraisetread pt where pt.BindTableName = @bindTaleName and pt.BindTableID = @bindTableID ")
                                     .Parameter("bindTaleName", ptVM.BindTableName)
                                     .Parameter("bindTableID", ptVM.BindTableID)
                                     .QueryMany<int>().Count;

                PraiseTreadItemVM itemVM = new PraiseTreadItemVM();
                itemVM.Item = ptVM;
                itemVM.Count = count;

                return itemVM;
            }
        }
        #endregion

    }
}
