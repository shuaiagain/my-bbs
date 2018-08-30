using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    public class PageQuery : Query
    {
        /// <summary>
        /// 当前第几页
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int? PageSize { get; set; }
    }
}
