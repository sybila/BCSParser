using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Repositories.Api
{

    namespace BcsAdmin.BL.Repositories
    {
        public class ApiRulesRepository : ApiGenericRepository<ApiRule>
        {
            public ApiRulesRepository()
                : base()
            {
                RepoName = "rules";
            }
        }
    }

}
