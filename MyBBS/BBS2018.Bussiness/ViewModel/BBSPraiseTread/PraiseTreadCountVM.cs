using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    public class PraiseTreadCountVM
    {

        public long? ID { get; set; }

        public int? PraiseCount { get; set; }

        public int? TreadCount { get; set; }

        /// <summary>
        /// 1：赞，2：踩，0：没有投
        /// </summary>
        public int? VoteStatus { get; set; }
    }
}
