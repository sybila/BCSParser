using BcsAdmin.DAL.Models;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;

namespace BcsAdmin.BL.Queries
{
    public class NoteQuery : IdFilteredQuery<EntityNoteDto>
    {
        public NoteQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<EntityNoteDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();
            context.EpUser.Load();
            context.EpEntity.Load();
            return context.EpEntityNote.Where(e => e.Entity.Id == Filter.Id).Select(e => new EntityNoteDto
            {
                Id = e.Id,
                Text = e.Text,
                Inserted = e.Inserted,
                Updated = e.Updated,
                UserName = e.User.Name,
                IntermediateEntityId = e.Id
            });
        }
    }
}
