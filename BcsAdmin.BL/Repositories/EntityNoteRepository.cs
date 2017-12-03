using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;

namespace BcsAdmin.BL.Repositories
{
    public class EntityNoteRepository : EntityFrameworkRepository<EpEntityNote, int>
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public EntityNoteRepository(IUnitOfWorkProvider unitOfWorkProvider, IDateTimeProvider dateTimeProvider)
            : base(unitOfWorkProvider, dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        public override void Insert(EpEntityNote entity)
        {
            entity.Inserted = dateTimeProvider.Now;
            base.Insert(entity);
        }

        public override void Update(EpEntityNote entity)
        {
            entity.Updated = dateTimeProvider.Now;
            base.Update(entity);
        }
    }
}
