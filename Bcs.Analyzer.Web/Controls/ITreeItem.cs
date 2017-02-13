using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsAnalysisWeb.Controls
{
    public interface ITreeItem
    {
        object Data { get; }
        IReadOnlyList<ITreeItem> Children { get; }
    }
}
