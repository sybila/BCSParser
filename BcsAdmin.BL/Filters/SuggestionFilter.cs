using BcsAdmin.BL.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL.Filters
{
    public class SuggestionFilter
    {
        public string SearchText { get; set; }
        public IList<HierarchyType> AllowedEntityTypes { get; set; }
        public CategoryType Category { get; set; }
    }
}

