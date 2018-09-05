using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    public class QuestionQuery : PageQuery
    {
        public int? UserID { get; set; }

        public long? QuestionID { get; set; }
    }
}
