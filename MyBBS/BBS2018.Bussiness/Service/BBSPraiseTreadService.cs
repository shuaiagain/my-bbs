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

            using (var dbContext = new DbContext().ConnectionStringName(ConnectionUtil.connBBS, new MySqlProvider()).UseTransaction(true))
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

                PraiseTreadCountVM ptCountVM = dbContext.Sql(@" 
                                             select 
	                                            SUM(case when pt.PraiseOrTread = 1 then 1 else 0 end)as PraiseCount,
	                                            SUM(case when pt.PraiseOrTread = 2 then 1 else 0 end)as TreadCount
                                             from bbspraisetread pt 
                                             where pt.BindTableName = @bindTableName
                                             and pt.BindTableID = @bindTableID ")
                                     .Parameter("bindTableName", ptVM.BindTableName)
                                     .Parameter("bindTableID", ptVM.BindTableID)
                                     .QuerySingle<PraiseTreadCountVM>((PraiseTreadCountVM vm, IDataReader re) =>
                                     {
                                         vm.PraiseCount = Convert.ToInt32(re["PraiseCount"]);
                                         vm.TreadCount = Convert.ToInt32(re["TreadCount"]);
                                     });

                dbContext.Commit();

                PraiseTreadItemVM itemVM = new PraiseTreadItemVM();
                itemVM.Item = ptVM;
                itemVM.PraiseCount = ptCountVM.PraiseCount;
                itemVM.TreadCount = ptCountVM.TreadCount;

                return itemVM;
            }
        }
        #endregion

    }
}
