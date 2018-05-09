using BcsAdmin.BL.Repositories.Api.BcsAdmin.BL.Repositories;
using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Repositories.Api
{
    public class AnnotationRepository : ApiGenericRepository<ApiAnnotation>
    {
        public AnnotationRepository(IDateTimeProvider dateTimeProvider)
            : base()
        {
        }
    }

    public class OrganismArrayRepository : ApiGenericRepository<OrganismArray>
    {
        public OrganismArrayRepository(IDateTimeProvider dateTimeProvider)
            : base()
        {
        }
    }

    public class ClassificationArrayRepository : ApiGenericRepository<ClassificationArray>
    {
        public ClassificationArrayRepository(IDateTimeProvider dateTimeProvider)
            : base()
        {
        }
    }
}
