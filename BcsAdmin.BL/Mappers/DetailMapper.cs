using AutoMapper;
using BcsAdmin.BL.Dto;
using BcsAdmin.DAL.Api;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BcsAdmin.BL.Mappers
{
    public class DetailMapper : IEntityDTOMapper<ApiEntity, BiochemicalEntityDetailDto>
    {
        private readonly IMapper mapper;

        public DetailMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public BiochemicalEntityDetailDto MapToDTO(ApiEntity entity)
        {
            return new BiochemicalEntityDetailDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                SelectedHierarchyType = (int)entity.Type,
                Status = (int)entity.Status
            };
        }

        public ApiEntity MapToEntity(BiochemicalEntityDetailDto source)
        {
            var target = new ApiEntity();
            PopulateEntity(source, target);
            return target;
        }

        public void PopulateEntity(BiochemicalEntityDetailDto source, ApiEntity target)
        {
            target.Code = source.Code;
            target.Name = source.Name;
            target.Description = source.Description;
            target.Type = (DAL.Api.ApiEntityType)source.SelectedHierarchyType;
            target.Status = (DAL.Api.ApiEntityStatus)source.Status;

        }
    }
}
