using System.Collections.Generic;

namespace BcsResolver.Common
{
    public interface IBcsTreeNode<TNode>
    {
        IEnumerable<TNode> EnumerateChildNodes();
    }
}