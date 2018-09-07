using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    public class CommentQuery : PageQuery
    {
        public long? AnswerID { get; set; }

        public int? UserID { get; set; }
    }
}
