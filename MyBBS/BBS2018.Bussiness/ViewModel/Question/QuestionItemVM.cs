using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    public class QuestionItemVM
    {
        /// <summary>
        /// 问题ID
        /// </summary>
        public long? QuestionID { get; set; }

        /// <summary>
        /// 问题名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 问题的回答
        /// </summary>
        //List<QuesitonDetailItem> Items { get; set; }

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
