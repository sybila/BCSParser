using BcsAdmin.DAL.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL.Dto
{
    public enum CategoryType
    {
        Default = 0,
        Entity = 1,
        Rule = 2
    }

    public static class CategoryTypeExtensions
    {
        public static bool TypeEquals(this CategoryType categoryType, ApiClassificationType apiClassifications)
        {
            return (categoryType == (CategoryType)apiClassifications)
                || (categoryType == CategoryType.Rule && apiClassifications == ApiClassificationType.Reaction);
        }
    }
}
