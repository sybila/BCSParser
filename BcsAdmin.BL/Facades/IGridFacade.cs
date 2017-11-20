using System;
using System.Collections.Generic;
using AutoMapper;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using DotVVM.Framework.Controls;

namespace BcsAdmin.BL.Facades
{
    public interface IGridFacade<TEntityDto> : ICrudFilteredListFacade<TEntityDto, IdFilter>
        where TEntityDto : IEntity<int>
    {
        void FillDataSet(GridViewDataSet<TEntityDto> dataSet, IdFilter filter);
        void CreateAndLink(TEntityDto entity, int detailId);
        void Edit(TEntityDto entityDto);
        void Link(EntityLinkDto link);
        void Unlink(int intermediateId);
        TEntityDto CreateAssociated();
    }
}