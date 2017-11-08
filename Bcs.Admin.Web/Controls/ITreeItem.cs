using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.Controls
{
    public interface ITreeItem
    {
        object Data { get; }
        IReadOnlyList<ITreeItem> Children { get; }
    }
}
