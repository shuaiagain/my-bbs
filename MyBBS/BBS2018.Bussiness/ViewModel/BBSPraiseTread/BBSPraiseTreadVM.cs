﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    /// <summary>
    /// 赞/踩视图
    /// </summary>
    public class BBSPraiseTreadVM
    {
        public long? ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// 赞/踩（1:赞，2：踩）
        /// </summary>
        public int? PraiseOrTread { get; set; }

        /// <summary>
        /// 绑定的表ID
        /// </summary>
        public long? BindTableID { get; set; }

        /// <summary>
        /// 绑定的表名称
        /// </summary>
        public string BindTableName { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime? InputTime { get; set; }
    }
}
