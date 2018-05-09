using BcsAdmin.BL.Facades.Exceptions;
using BcsAdmin.DAL.Api;
using BcsAdmin.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BcsAdmin.BL.Repositories.Api.Exceptions;

namespace BcsAdmin.BL.Repositories.Api
{

    namespace BcsAdmin.BL.Repositories
    {
        public abstract class ApiGenericReadonlyRepository<TEntity, TKey> : IAsyncReadonlyRepository<TEntity, TKey>
            where TEntity : class, new()
        {
            public string AppUrl { get; set; } = "https://api.e-cyanobacterium.org";
            public string RepoName { get; set; }
            protected virtual string GetFullUrl(IEnumerable<TKey> ids) => $"{AppUrl}/{RepoName}/{string.Join(",", ids)}";
            protected virtual string GetFullUrl(TKey id) => $"{AppUrl}/{RepoName}/{id}";
            protected virtual string GetFullUrl() => $"{AppUrl}/{RepoName}";
            protected HttpClient HttpClient { get; set; } = new HttpClient();
            public JsonSerializerSettings JsonSerializerSettings { get; }

            public ApiGenericReadonlyRepository()
            {
                var contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        OverrideSpecifiedNames = false,

                    }
                };

                JsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };
            }

            public abstract Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, TKey id);           
        }

        public abstract class ApiGenericRepository<TEntity> : ApiGenericReadonlyRepository<TEntity, int>, IAsyncRepository<TEntity, int>
            where TEntity : class, IEntity<int>, new()
        {
            public override async Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, int id)
            {
                var r = await GetByIdsAsync(cancellationToken, new int[] { id });
                return r?.FirstOrDefault();
        }

        public async Task<IList<TEntity>> GetByIdsAsync(CancellationToken cancellationToken, IEnumerable<int> ids)
        {
            if (!ids.Any()) { return new List<TEntity>(); };

            var response = await HttpClient.GetAsync(GetFullUrl(ids), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            var responseData = JsonConvert.DeserializeObject<EntityResponseData<List<TEntity>>>(responseBody, JsonSerializerSettings);

            return responseData.Data;
        }

        public virtual TEntity InitializeNew()
            {
                return new TEntity();
            }

            public async Task DeteleAsync(int id)
            {
                var response = await HttpClient.DeleteAsync(GetFullUrl(id));
                await HandleResponseAsync(response);
            }        

            public async Task InsertAsync(TEntity entity)
            {
                var response = await HttpClient.PostAsync(GetFullUrl(), PrepareContent(entity));
                var newId = await HandleResponseAsync(response);
                entity.Id = newId ?? 0;
            }

            public async Task UpdateAsync(TEntity entity)
            {
                var response = await HttpClient.PutAsync(GetFullUrl(entity.Id), PrepareContent(entity));
                await HandleResponseAsync(response);
            }

            private async Task<int?> HandleResponseAsync(HttpResponseMessage response)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                var responseObject = JsonConvert.DeserializeObject<ResponseData>(responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    if (responseObject == null)
                    {
                        throw new ApiDownException("Empty response.");
                    }
                    if (responseObject.Code == 500)
                    {
                        throw new ApiDownException($"{responseObject.Code}: {responseObject.Message}");
                    }
                    throw new InvalidInputException(responseObject.Message);
                }
                return responseObject?.Id;
            }

            private HttpContent PrepareContent(TEntity entity)
            {
                var content =  new StringContent(
                    JsonConvert.SerializeObject(entity, JsonSerializerSettings),
                    Encoding.UTF8, "application/json");

                content.Headers.Add("type", "object");

                return content;
            }
        }
    }

}
