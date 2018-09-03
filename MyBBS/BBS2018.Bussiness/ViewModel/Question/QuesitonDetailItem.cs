using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    public class QuesitonDetailItem
    {
        /// <summary>
        /// 回答ID
        /// </summary>
        public long? AnswerID { get; set; }

        /// <summary>
        /// 回答内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 赞总数
        /// </summary>
        public int? TotalPraise { get; set; }

        /// <summary>
        /// 踩总数
        /// </summary>
        public int? TotalTread { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
    }
}
