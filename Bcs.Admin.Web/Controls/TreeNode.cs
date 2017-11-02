using Bcs.Analyzer.DemoWeb.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bcs.Analyzer.DemoWeb.ViewModels
{
    public class TreeNode<TData> :ITreeItem
    {
        public TData Data { get; set; }
        object ITreeItem.Data => Data;
        public List<TreeNode<TData>> Children { get; set; }

        IReadOnlyList<ITreeItem> ITreeItem.Children => Children?.Cast<ITreeItem>()?.ToList();

        public TreeNode()
        {
            
        }
    }
}