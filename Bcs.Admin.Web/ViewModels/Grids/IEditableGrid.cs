﻿using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure.Core;

namespace Bcs.Admin.Web.ViewModels.Grids
{
    public interface IEditableGrid<TGridEntity>: IDotvvmViewModel
        where TGridEntity : class, IEntity<int>
    {
        int ParentEntityId { get; set; }

        GridViewDataSet<TGridEntity> DataSet { get; }
        TGridEntity NewRow { get; }

        void Edit(int id);
        void Delete(int id);
        void Add();
        void SaveEdit(TGridEntity entity);
        void SaveNew();
        void Cancel();
    }
}
