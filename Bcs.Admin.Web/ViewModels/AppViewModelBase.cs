using DotVVM.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.ViewModels
{
    public class AppViewModelBase : DotvvmViewModelBase
    {
        public List<string> Errors { get; set; } = new List<string>();
        public bool HasError => Errors.Count > 0;
        public bool HasSuccess => SuccessMessage != null;
        public string SuccessMessage { get; set; }

        protected async Task ExecuteSafeAsync(Func<Task> action, string success = null)
        {
            try
            {
                await action();
                SuccessMessage = success;
            }
            catch (Exception ex)
            {
                Errors.Add(ex.Message);
                SuccessMessage = null;
            }
        }
    }
}
