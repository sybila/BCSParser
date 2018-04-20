using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure.Core;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.ViewModels.Grids
{
    public interface IEditableGrid<TGridEntity>: IDotvvmViewModel, ICollapsible
        where TGridEntity : class, IEntity<int>
    {
        int ItemsCount { get; }
        int ParentEntityId { get; set; }

        GridViewDataSet<TGridEntity> DataSet { get; }
        TGridEntity NewRow { get; }

        void Edit(TGridEntity entity);
        Task DeleteAsync(TGridEntity entity);
        void Add();
        Task SaveEditAsync(TGridEntity entity);
        Task SaveNewAsync();
        void Cancel();

        Task ReloadDataAsync();
    }
}
