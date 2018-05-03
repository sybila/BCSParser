using System;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.DAL.Api;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using System.Collections.Generic;
using BcsAdmin.BL.Queries;
using System.Linq;
using BcsAdmin.BL.Facades.Defs;
using DotVVM.Framework.Controls;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Facades
{
    public class StateGridFacade : IGridFacade<string, StateEntityDto>
    {
        public Func<IFilteredQuery<StateEntityDto, IdFilter>> QueryFactory { get; }
        private readonly IRepository<ApiEntity, int> entityRepository;

        public StateGridFacade(IRepository<ApiEntity, int> entityRepository, Func<OneToManyQuery<ApiEntity, StateEntityDto>> queryFactory)
        {
            this.entityRepository = entityRepository;
            QueryFactory = queryFactory;
        }

        public StateEntityDto InitializeNew()
        {
            return new StateEntityDto();
        }

        public StateEntityDto GetDetail(int parentId, string parentRepositoryName, string code)
        {
            var parent = entityRepository.GetById(parentId);
            var state = parent.States.SingleOrDefault(s => s.Code == code);

            return new StateEntityDto {
                Id = state.Code,
                Description = state.Description
            };
        }

        public StateEntityDto Save(int parentId, string parentRepositoryName, StateEntityDto data)
        {
            var parent = entityRepository.GetById(parentId);
            var state = parent.States.SingleOrDefault(s => s.Code == data.Id);

            var newState = new ApiState
            {
                Code = data.Id,
                Description = data.Description
            };

            if (state != null)
            {
                parent.States.Remove(state);
            }
            parent.States.Add(newState);

            ClearAll(parent);
            entityRepository.Update(parent);
            return data;
        }

        public void Delete(int parentId, string parentRepositoryName, string code)
        {
            var parent = entityRepository.GetById(parentId);
            var state  = parent.States.SingleOrDefault(s => s.Code == code);
            parent.States.Remove(state);

            ClearAll(parent);
            entityRepository.Update(parent);
        }

        private static void ClearAll(ApiEntity parentEntity)
        {
            parentEntity.Annotations = null;
            parentEntity.Children = null;
            parentEntity.Code = null;
            parentEntity.Description = null;
            parentEntity.Name = null;
            parentEntity.Organisms = null;
            parentEntity.Parent = null;
            parentEntity.Parents = null;
            parentEntity.Status = null;
            parentEntity.Type = null;
            parentEntity.Classifications = null;
            parentEntity.Compartments = null;
        }

        public async Task FillDataSetAsync(GridViewDataSet<StateEntityDto> dataSet, IdFilter filter)
        {
            var query = QueryFactory();
            query.Filter = filter;
            await dataSet.LoadFromQueryAsync(query);
        }
    }
}
