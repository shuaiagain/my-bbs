using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    public class QuestionDetailVM
    {
        /// <summary>
        /// 问题ID
        /// </summary>
        public long? ID { get; set; }

        /// <summary>
        /// 问题名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 总共的回答数量
        /// </summary>
        public int? AnswerCount { get; set; }

        /// <summary>
        /// 问题的回答
        /// </summary>
        public List<QuesitonDetailItemVM> Items { get; set; }
    }
}
