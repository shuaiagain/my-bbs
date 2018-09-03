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
        List<QuesitonDetailItem> Items { get; set; }
    }
}
