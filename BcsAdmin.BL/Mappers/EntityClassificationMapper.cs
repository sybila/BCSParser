using AutoMapper;
using BcsAdmin.BL.Dto;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL.Mappers
{
    public class EntityClassificationMapper : IEntityDTOMapper<EpEntityClassification, ClassificationDto>
    {
        private readonly IMapper mapper;

        public EntityClassificationMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public ClassificationDto MapToDTO(EpEntityClassification source)
        {
            return mapper.Map<ClassificationDto>(source.Classification);
        }

        public EpEntityClassification MapToEntity(ClassificationDto source)
        {
            throw new NotImplementedException();
        }

        public void PopulateEntity(ClassificationDto source, EpEntityClassification target)
        {
            throw new NotImplementedException();
        }
    }
}
