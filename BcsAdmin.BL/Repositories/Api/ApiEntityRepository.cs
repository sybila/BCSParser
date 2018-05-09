using BcsAdmin.DAL.Api;
using Newtonsoft.Json;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Repositories.Api
{

    namespace BcsAdmin.BL.Repositories
    {
        public class ApiEntityRepository : ApiGenericRepository<ApiEntity>
        {
            public ApiEntityRepository()
                : base()
            {
                RepoName = "entities";
            }

            public override ApiEntity InitializeNew()
            {
                var @new = base.InitializeNew();
                @new.Type = ApiEntityType.Atomic;
                @new.Status = ApiEntityStatus.Inactive;
                return @new;
            }
        }
        public class ApiEntityCodeRepository : ApiGenericReadonlyRepository<ApiEntity, string>
        {
            public ApiEntityCodeRepository() : base()
            {
                RepoName = "entities";
            }

            public override async Task<ApiEntity> GetByIdAsync(CancellationToken cancellationToken, string key)
            {
                var response = await HttpClient.GetAsync(GetFullUrl(key), cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string responseBody = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<EntityResponseData<ApiEntity>>(responseBody, JsonSerializerSettings);

                return responseData.Data;
            }
        }
    }

}
