using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.ViewModels.Grids
{
    public interface ICollapsible
    {
        bool IsCollapsed { get; set; }
    }
}
