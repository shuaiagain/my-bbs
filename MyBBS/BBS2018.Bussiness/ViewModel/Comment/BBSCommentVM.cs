using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    /// <summary>
    /// 评论视图
    /// </summary>
    public class BBSCommentVM
    {
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
        /// 绑定的表ID
        /// </summary>
        public long? BindTableID { get; set; }

        /// <summary>
        /// 绑定的表名称
        /// </summary>
        public string BindTableName { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime? InputTime { get; set; }

        #region 扩展字段

        public string InputTimeStr
        {
            get
            {
                if (!this.InputTime.HasValue) return string.Empty;
                return this.InputTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        #endregion
    }
}
