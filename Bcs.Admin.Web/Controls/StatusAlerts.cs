using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Compilation.ControlTree;
using DotVVM.Framework.Compilation.Javascript.Ast;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.Controls
{
    public class StatusAlerts : HtmlGenericControl
    {
        [MarkupOptions(Required = true)]
        public IValueBinding SuccessMessageBinding
        {
            get { return (IValueBinding)GetValue(SuccessMessageBindingProperty); }
            set { SetValue(SuccessMessageBindingProperty, value); }
        }
        public static readonly DotvvmProperty SuccessMessageBindingProperty
            = DotvvmProperty.Register<IValueBinding, StatusAlerts>(c => c.SuccessMessageBinding, null);

        public IValueBinding ErrorsBinding
        {
            get { return (IValueBinding)GetValue(ErrorsBindingProperty); }
            set { SetValue(ErrorsBindingProperty, value); }
        }
        public static readonly DotvvmProperty ErrorsBindingProperty
            = DotvvmProperty.Register<IValueBinding, StatusAlerts>(c => c.ErrorsBinding, null);

        public StatusAlerts()
            :base("div")
        {

        }

        //      <div Visible = "{value: _control.SuccessMessage != null}" class="alert alert-success">
        //          {{value: _control.SuccessMessage}}
        //      </div>
        //      <div Visible = "{value: _control.Errors.Count > 0}" class="alert alert-danger">
        //          <strong>Error(s) occured:</strong>
        //          <dot:Repeater WrapperTagName = "ul" class="list" DataSource="{value: _control.Errors}">
        //              <li>{{value: _this}}</li>
        //          </dot:Repeater>
        //      </div>
        protected override void OnInit(IDotvvmRequestContext context)
        {
            var bindingService = (BindingCompilationService)context.Services.GetService(typeof(BindingCompilationService));

            var successMassage = new Literal();
            successMassage.SetBinding(Literal.TextProperty, SuccessMessageBinding);

            var successDiv = new HtmlGenericControl("div");
            successDiv.Attributes["class"] = "alert alert-success";


            var successNotNull = new JsBinaryExpression(new JsIdentifierExpression("$data.SuccessMessage()"),BinaryOperatorType.StricltyNotEqual, new JsLiteral(null));
            var errorsCountNotNull = 
                new JsBinaryExpression(
                    
                        new JsMemberAccessExpression(
                            new JsIdentifierExpression("$data.Errors()"), 
                            new JsIdentifier("length")), 
                    BinaryOperatorType.StricltyNotEqual, 
                    new JsLiteral(0));

            successDiv.Children.Add(successMassage);
            successDiv.SetBinding(VisibleProperty, 
                ValueBindingExpression.CreateBinding<bool>(
                    bindingService.WithoutInitialization(), 
                    h => (string)GetValueBinding(SuccessMessageBindingProperty).Evaluate(this) != null, 
                    successNotNull,
                    this.GetDataContextType()));

            var strong = new HtmlGenericControl("string");
            strong.Children.Add(new Literal("Error(s) occured:"));

            var repeater = new Repeater();
            repeater.WrapperTagName = "ul";
            repeater.Attributes["class"] = "list";
            repeater.SetBinding(ItemsControl.DataSourceProperty, ErrorsBinding);

            repeater.ItemTemplate = new DelegateTemplate(
                (f, sp, c) =>
                {
                    var dcts = DataContextStack.Create(ErrorsBinding.ResultType.GenericTypeArguments[0], this.GetDataContextType());

                    c.SetDataContextType(dcts);                  

                    var li = new HtmlGenericControl("li");
                    var literal = new Literal();
                    literal.SetBinding(Literal.TextProperty, ValueBindingExpression.CreateBinding(bindingService, h => ((string)h[0]), dcts));

                    li.Children.Add(literal);
                    c.Children.Add(li);
                });

            var failDiv = new HtmlGenericControl("div");
            failDiv.Attributes["class"] = "alert alert-danger";
            failDiv.Children.Add(strong);
            failDiv.Children.Add(repeater);
            failDiv.SetBinding(VisibleProperty, ValueBindingExpression.CreateBinding(
                bindingService, 
                h => ((IEnumerable<string>)GetValueBinding(ErrorsBindingProperty).Evaluate(this)).Count() != 0, 
                errorsCountNotNull,
                this.GetDataContextType()));

            Children.Add(failDiv);
            Children.Add(successDiv);
        }

    }
}
