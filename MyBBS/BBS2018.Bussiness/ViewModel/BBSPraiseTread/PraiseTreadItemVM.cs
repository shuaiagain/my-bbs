﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBS2018.Bussiness.ViewModel
{
    public class PraiseTreadItemVM
    {
        public int? PraiseCount { get; set; }

        public int? TreadCount { get; set; }

        public BBSPraiseTreadVM Item { get; set; }
    }
}
