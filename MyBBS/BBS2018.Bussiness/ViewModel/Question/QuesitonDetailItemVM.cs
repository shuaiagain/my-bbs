using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    public class QuesitonDetailItemVM
    {
        /// <summary>
        /// 回答者名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 回答者ID
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// 回答者头像Url
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 回答ID
        /// </summary>
        public long? AnswerID { get; set; }

        /// <summary>
        /// 回答的内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 回答日期
        /// </summary>
        public DateTime? EditTime { get; set; }

        /// <summary>
        /// 回答日期
        /// </summary>
        public string EditTimeStr
        {
            get
            {
                if (!this.EditTime.HasValue) return string.Empty;

                return EditTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 最后一个赞的人的名称
        /// </summary>
        public string LastPraiseUserName { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        public int? PraiseCount { get; set; }

        /// <summary>
        /// 踩的数量
        /// </summary>
        public int? TreadCount { get; set; }

        public int? IsPraised { get; set; }

        public int? IsTreaded { get; set; }
    }
}
