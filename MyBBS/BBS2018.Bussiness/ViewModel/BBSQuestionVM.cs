using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    /// <summary>
    /// 问题视图
    /// </summary>
    public class BBSQuestionVM
    {
        /// <summary>
        /// ID
        /// </summary>
        public long? ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 问题名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime? InputTime { get; set; }
    }
}
