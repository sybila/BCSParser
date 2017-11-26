using AutoMapper;
using BcsAdmin.BL.Dto;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BcsAdmin.BL.Mappers
{
    public class DetailMapper : IEntityDTOMapper<EpEntity, BiochemicalEntityDetailDto>
    {
        private readonly IMapper mapper;

        public DetailMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public BiochemicalEntityDetailDto MapToDTO(EpEntity entity)
        {
            return new BiochemicalEntityDetailDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                Active = entity.Active == 0,
                Description = entity.Description,
                SelectedHierarchyType = (int)entity.HierarchyType,
                VisualisationXml = entity.VisualisationXml

            };
        }

        public EpEntity MapToEntity(BiochemicalEntityDetailDto source)
        {
            var target = new EpEntity();
            PopulateEntity(source, target);
            return target;
        }

        public void PopulateEntity(BiochemicalEntityDetailDto source, EpEntity target)
        {
            target.Active = !source.Active ? (int?)null : 1;
            target.Code = source.Code;
            target.Name = source.Name;
            target.Description = source.Description;
            target.HierarchyType = (HierarchyType)source.SelectedHierarchyType;
            target.VisualisationXml = source.VisualisationXml;
            target.Type = 
                source.SelectedHierarchyType == 0
                ? "state"
                : "entity";


        }
    }
}
