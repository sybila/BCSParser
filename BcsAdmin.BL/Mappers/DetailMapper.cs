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
                Parent = mapper.Map<BiochemicalEntityLinkDto>(entity.Parent),
                SelectedHierarchyType = (int)entity.HierarchyType,
                Components =
                        entity.HierarchyType == HierarchyType.Atomic
                        ? entity.Children.Select(mapper.Map<BiochemicalEntityLinkDto>).ToList()
                        : entity.Components.Select(c => mapper.Map<BiochemicalEntityLinkDto>(c.Component)).ToList(),
                HierarchyTypes =
                        Enum.GetValues(typeof(HierarchyType))
                        .Cast<HierarchyType>()
                        .Select(v => new BiochemicalEntityTypeDto
                        {
                            Id = (int)v,
                            Name = v.ToString("F")
                        })
                        .ToList(),
                Locations = entity.Locations.Select(el => mapper.Map<BiochemicalEntityLinkDto>(el.Location)).ToList(),
                Notes = entity.Notes.Select(mapper.Map<EntityNoteDto>).ToList(),
                Classifications = entity.Classifications.Select(ec => mapper.Map<ClassificationDto>(ec.Classification)).ToList(),
                VisualisationXml = entity.VisualisationXml

            };
        }

        public EpEntity MapToEntity(BiochemicalEntityDetailDto source)
        {
            throw new NotImplementedException();
        }

        public void PopulateEntity(BiochemicalEntityDetailDto source, EpEntity target)
        {
            throw new NotImplementedException();
        }
    }
}
