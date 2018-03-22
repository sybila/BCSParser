using BcsAdmin.DAL.Models;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;

namespace BcsAdmin.BL.Queries
{
    public class LocationLinkQuery : IdFilteredQuery<LocationLinkDto>
    {
        public LocationLinkQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<LocationLinkDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();
            context.EpEntity.Load();
            return context.EpEntityLocation.Where(e => e.Entity.Id == Filter.Id).Select(e => new LocationLinkDto
            {
                Id = e.Location.Id,
                Code = e.Location.Code,
                Name = e.Location.Name
            });
        }
    }
}
