using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    /// <summary>
    /// 操作日志视图
    /// </summary>
    public class BBSOperateLogVM
    {
        public long? ID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public int? OperatorID { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int? OperateType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? InputTime { get; set; }
    }
}
