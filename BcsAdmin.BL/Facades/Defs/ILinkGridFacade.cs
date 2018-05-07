using System;
using System.Collections.Generic;
using AutoMapper;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using DotVVM.Framework.Controls;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Facades
{
    public interface ILinkGridFacade<TEntityDto> 
        where TEntityDto : IEntity<int>
    {
        Task FillDataSetAsync(GridViewDataSet<TEntityDto> dataSet, IdFilter filter);
        Task CreateAndLinkAsync(int detailId, string paentRepositoryName, TEntityDto entity);
        Task EditAsync(TEntityDto entityDto);
        Task LinkAsync(string paentRepositoryName, EntityLinkDto link);
        Task UnlinkAsync(string paentRepositoryName, EntityLinkDto link);
        TEntityDto CreateAssociated();
    }
}