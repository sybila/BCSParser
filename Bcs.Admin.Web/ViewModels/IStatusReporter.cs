using System.Collections.Generic;

namespace Bcs.Admin.Web.ViewModels
{
    public interface IStatusReporter
    {
        List<string> Errors { get; set; }
        string SuccessMessage { get; set; }
    }
}