using BcsAdmin.BL.Repositories.Api.Exceptions;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.ViewModel.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.ViewModels
{
    public class AppViewModelBase : DotvvmViewModelBase, IStatusReporter
    {
        [Bind(Direction.ServerToClient)]
        public List<string> Errors { get; set; } = new List<string>();
        public bool HasError => Errors.Count > 0;
        public bool HasSuccess => SuccessMessage != null;

        [Bind(Direction.ServerToClient)]
        public string SuccessMessage { get; set; }

        protected async Task ExecuteSafeAsync(Func<Task> action, string success = null)
        {
            try
            {
                await action();
                SuccessMessage = success;
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Errors.Add(e.Message);
                }
            }
            catch (InvalidInputException ex)
            {
                Errors.Add(ex.Message);
                SuccessMessage = null;
            }
            catch (Exception ex)
            {
                Errors.Add(ex.Message);
                SuccessMessage = null;
            }
        }
    }
}
