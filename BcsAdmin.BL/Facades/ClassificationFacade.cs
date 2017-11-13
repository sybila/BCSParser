using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.DAL.Models;
using BcsAdmin.BL.Dto;

namespace BcsAdmin.BL.Facades
{
    public class ClassificationFacade : CrudFacadeBase<EpEntityClassification, int, ClassificationDto, ClassificationDto>
    {
        protected ClassificationFacade(
            Func<IQuery<ClassificationDto>> queryFactory,
            IRepository<EpEntityClassification, int> repository, 
            IEntityDTOMapper<EpEntityClassification, ClassificationDto> mapper) 
            : base(queryFactory, repository, mapper)
        {

        }
    }
}
