﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL.Filters
{
    public class BiochemicalEntityFilter
    {
        public List<string> EntityTypeFilter { get; set; } = new List<string>();
        public string SearchText { get; set; }
    }
}
