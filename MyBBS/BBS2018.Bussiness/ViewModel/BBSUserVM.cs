using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    /// <summary>
    /// 用户视图
    /// </summary>
    public class BBSUserVM
    {
        public int? ID { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 头像url
        /// </summary>
        public string HeadImageUrl { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime? InputTime { get; set; }
    }
}
