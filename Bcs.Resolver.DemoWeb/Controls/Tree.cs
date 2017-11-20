using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BcsResolver.Extensions;
using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Compilation.Javascript;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;

namespace Bcs.Analyzer.DemoWeb.Controls
{
    public class KoWithPlaceHolder : DotvvmControl
    {
        private string koExpression;

        public KoWithPlaceHolder(string koExpression)
        {
            this.koExpression = koExpression;
        }

        public override void Render(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            writer.WriteKnockoutWithComment(koExpression);

            foreach (KeyValuePair<DotvvmProperty, object> property in this.properties)
            {
                if (property.Key is ActiveDotvvmProperty)
                    ((ActiveDotvvmProperty)property.Key).AddAttributesToRender(writer, context, this);
            }
            this.AddAttributesToRender(writer, context);
            this.RenderBeginTag(writer, context);
            this.RenderContents(writer, context);
            this.RenderEndTag(writer, context);

            writer.WriteKnockoutDataBindEndComment();
        }
    }

    public class Tree : ItemsControl
    {
        [MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement, Required = true)]
        [ControlPropertyBindingDataContextChange(nameof(DataSource))]
        [CollectionElementDataContextChange(1)]
        public ITemplate ItemTemplate
        {
            get { return (ITemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public static readonly DotvvmProperty ItemTemplateProperty =
            DotvvmProperty.Register<ITemplate, Tree>(t => t.ItemTemplate, null);

        public string NodeCollectionWrapperClass
        {
            get { return (string)GetValue(NodeCollectionWrapperClassProperty); }
            set { SetValue(NodeCollectionWrapperClassProperty, value); }
        }
        public static readonly DotvvmProperty NodeCollectionWrapperClassProperty =
            DotvvmProperty.Register<string, Tree>(t => t.NodeCollectionWrapperClass, "");

        public string NodeCollectionWrapperTag
        {
            get { return (string)GetValue(NodeCollectionWrapperTagProperty); }
            set { SetValue(NodeCollectionWrapperTagProperty, value); }
        }
        public static readonly DotvvmProperty NodeCollectionWrapperTagProperty =
            DotvvmProperty.Register<string, Tree>(t => t.NodeCollectionWrapperTag, "");

        public string NodeWrapperTag
        {
            get { return (string)GetValue(NodeWrapperTagProperty); }
            set { SetValue(NodeWrapperTagProperty, value); }
        }
        public static readonly DotvvmProperty NodeWrapperTagProperty =
            DotvvmProperty.Register<string, Tree>(t => t.NodeWrapperTag, "");

        private int nodeCount = 0;

        public Tree() : base("div")
        {

        }

        protected override void OnLoad(IDotvvmRequestContext context)
        {
            DataBind(context);
            base.OnLoad(context);
        }

        /// <summary>
        /// Occurs after the page commands are executed.
        /// </summary>
        protected override void OnPreRender(IDotvvmRequestContext context)
        {
            DataBind(context);
            // TODO: we should handle observable collection operations to persist controlstate of controls inside the Repeater
            base.OnPreRender(context);
        }


        /// <summary>
        /// Performs the data-binding and builds the controls inside the <see cref="Repeater"/>.
        /// </summary>
        private void DataBind(IDotvvmRequestContext context)
        {
            Children.Clear();

            if (DataSource != null)
            {
                var items = GetIEnumerableFromDataSource().Cast<ITreeItem>().ToList();
                nodeCount = 0;
                CreateChildrenCollectionIfAny(context,items, this, $"{GetDataSourceBinding().GetKnockoutBindingExpression()}()");
            }
            else
            {
                throw new InvalidOperationException("DataSource property has to be set.");
            }
        }

        private DotvvmControl BuildTreeNode(IDotvvmRequestContext context, ITreeItem dataContext, string koExpressionHierarchy)
        {
            nodeCount++;
            var uniqueChildId = GetUniqueIdForDataContainer();

            var nodeContainer = new PlaceHolder();

            var nodeKoWrapper = new KoWithPlaceHolder(koExpressionHierarchy);
            nodeContainer.Children.Add(nodeKoWrapper);

            var nodeDataContainer = new DataItemContainer { DataContext = dataContext};
            nodeDataContainer.SetValue(Internal.PathFragmentProperty, uniqueChildId);
            ItemTemplate.BuildContent(context, nodeDataContainer);
            nodeKoWrapper.Children.Add(nodeDataContainer);

            CreateChildrenCollectionIfAny(context, dataContext.Children, nodeContainer, $"{koExpressionHierarchy}.Children()");

            return nodeContainer;
        }

        private void CreateChildrenCollectionIfAny(IDotvvmRequestContext context, IReadOnlyList<ITreeItem> children, DotvvmControl nodeContainer, string koExpressionHierarchy)
        {
            if (children != null && children.Any())
            {
                var childrenCollectionWrap = GetChildrenCollectionWrap();
                nodeContainer.Children.Add(childrenCollectionWrap);

                var childIndex = 0;
                foreach (var item in children)
                {
                    var childWrap = GetChildWrap();
                    

                    childrenCollectionWrap.Children.Add(childWrap);
                    childWrap.Children.Add(BuildTreeNode(context, item, $"{koExpressionHierarchy}[{childIndex}]()"));
                    childIndex++;
                }
            }
        }

        private DotvvmControl GetChildrenCollectionWrap()
        {
            if (!string.IsNullOrWhiteSpace(NodeCollectionWrapperTag))
            {
                var childrenCollectionWrap =
                    new HtmlGenericControl(NodeCollectionWrapperTag);
                if (!string.IsNullOrWhiteSpace(NodeCollectionWrapperClass))
                {
                    childrenCollectionWrap.Attributes["class"] = NodeCollectionWrapperClass;
                }
                return childrenCollectionWrap;
            }
            return new PlaceHolder();
        }

        private DotvvmControl GetChildWrap()
        {
            return !string.IsNullOrWhiteSpace(NodeWrapperTag)
                ? new HtmlGenericControl(NodeWrapperTag).CastTo<DotvvmControl>()
                : new PlaceHolder();
        }

        protected override void RenderContents(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            if (RenderOnServer)
            {
                // render on server
                foreach (var child in Children)
                {
                    child.Render(writer, context);
                }
            }
            else
            {
                throw new InvalidOperationException("This control works only in server render mode.");
            }
        }

        private string GetUniqueIdForDataContainer()
        {
            var knockoutBindingExpression = GetValueBinding(DataSourceProperty).GetKnockoutBindingExpression();
            var uniqueChildId = $"{nodeCount}_{knockoutBindingExpression}";
            return uniqueChildId;
        }

        public static ITreeItem GetITreeItemFromDataSource(object dataSource)
        {
            if (dataSource == null)
                return (ITreeItem)null;
            if (dataSource is ITreeItem)
                return (ITreeItem)dataSource;
            throw new NotSupportedException(string.Format("The object of type '{0}' is not supported in the DataSource property!", (object)dataSource.GetType()));
        }
    }
}
