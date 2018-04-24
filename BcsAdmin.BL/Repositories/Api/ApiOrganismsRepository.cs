using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Repositories.Api
{

    namespace BcsAdmin.BL.Repositories
    {
        public class ApiOrganismsRepository : ApiGenericRepository<ApiOrganism>
        {
            public ApiOrganismsRepository(IDateTimeProvider dateTimeProvider)
                : base(dateTimeProvider)
            {
                RepoName = "organisms";
            }
        }
    }

}
