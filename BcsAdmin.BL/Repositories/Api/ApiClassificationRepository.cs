using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Repositories.Api
{

    namespace BcsAdmin.BL.Repositories
    {
        public class ApiClassificationRepository : ApiGenericRepository<ApiClassification>
        {
            public ApiClassificationRepository()
                : base()
            {
                RepoName = "classifications";
            }
        }
    }

}
