using AutoMapper;
using BcsAdmin.BL.Dto;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL.Mappers
{
    public class AutoDtoMapper<TEntity, TDto> : IEntityDTOMapper<TEntity, TDto>
        where TEntity : IEntity<int>
        where TDto : IEntity<int>
    {
        private readonly IMapper mapper;

        public AutoDtoMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public TDto MapToDTO(TEntity source) => mapper.Map<TDto>(source);
        public TEntity MapToEntity(TDto source) => mapper.Map<TEntity>(source);
        public void PopulateEntity(TDto source, TEntity target) => mapper.Map(source, target);
    }
}
