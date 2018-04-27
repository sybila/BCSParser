using BcsAdmin.DAL.Api;
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

namespace BcsAdmin.BL.Repositories.Api
{

    namespace BcsAdmin.BL.Repositories
    {
        public abstract class ApiGenericRepository<TEntity> : IRepository<TEntity, int>
            where TEntity : class, IEntity<int>, new()
        {
            public string AppUrl { get; set; } = "https://api.e-cyanobacterium.org";
            public string RepoName { get; set; }
            protected virtual string GetFullUrl(IEnumerable<int> ids) => $"{AppUrl}/{RepoName}/{string.Join(",", ids)}";
            protected virtual string GetFullUrl(int id) => $"{AppUrl}/{RepoName}/{id}";
            protected virtual string GetFullUrl() => $"{AppUrl}/{RepoName}";

            protected HttpClient HttpClient { get; set; } = new HttpClient();
            public JsonSerializerSettings JsonSerializerSettings { get; }

            public ApiGenericRepository(IDateTimeProvider dateTimeProvider)
            {
                var contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy
                    {
                        OverrideSpecifiedNames = false
                    }
                };

                JsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };
            }

            public virtual TEntity InitializeNew()
            {
                return new TEntity();
            }

            public void Delete(TEntity entity)
            {
                Delete(entity.Id);
            }

            public void Delete(IEnumerable<TEntity> entities)
            {
                foreach (var entity in entities)
                {
                    Delete(entity);
                }
            }

            public virtual void Delete(int id)
            {
                DeteleAsync(id).Wait();
            }

            private async Task DeteleAsync(int id)
            {
                var response = await HttpClient.DeleteAsync(GetFullUrl(id));
                await HandleResponseAsync(response);
            }

            public void Delete(IEnumerable<int> ids)
            {
                foreach (int id in ids)
                {
                    Delete(id);
                }
            }

            public void Insert(TEntity entity)
            {
                InsertAsync(entity).Wait();
            }

            private async Task InsertAsync(TEntity entity)
            {
                var response = await HttpClient.PostAsync(GetFullUrl(), PrepareContent(entity));
                var newId = await HandleResponseAsync(response);
                entity.Id = newId ?? 0;
            }

            public void Insert(IEnumerable<TEntity> entities)
            {
                foreach (var entity in entities)
                {
                    Insert(entity);
                }
            }

            public void Update(TEntity entity)
            {
                UpdateAsync(entity).Wait();
            }

            private async Task UpdateAsync(TEntity entity)
            {
                var response = await HttpClient.PutAsync(GetFullUrl(entity.Id), PrepareContent(entity));
                await HandleResponseAsync(response);
            }

            public void Update(IEnumerable<TEntity> entities)
            {
                foreach (var entity in entities)
                {
                    Update(entity);
                }
            }

            public TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includes)
            {
                return GetByIdAsync(id).Result;
            }

            public TEntity GetById(int id, IIncludeDefinition<TEntity>[] includes)
            {
                return GetByIdAsync(id).Result;
            }

            public async Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
            {
                var cancelationToken = new CancellationToken();
                return await GetByIdAsync(cancelationToken, id, includes);
            }

            public async Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, int id, params Expression<Func<TEntity, object>>[] includes)
            {
                var r = await GetByIdsAsync(cancellationToken, new int[] { id });
                return r?.FirstOrDefault();
            }

            public async Task<TEntity> GetByIdAsync(int id, IIncludeDefinition<TEntity>[] includes)
            {
                return await GetByIdAsync(id);
            }

            public async Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, int id, IIncludeDefinition<TEntity>[] includes)
            {
                return await GetByIdAsync(cancellationToken, id);
            }

            public IList<TEntity> GetByIds(IEnumerable<int> ids, params Expression<Func<TEntity, object>>[] includes)
            {
                throw new NotImplementedException();
            }

            public IList<TEntity> GetByIds(IEnumerable<int> ids, IIncludeDefinition<TEntity>[] includes)
            {
                throw new NotImplementedException();
            }

            public async Task<IList<TEntity>> GetByIdsAsync(IEnumerable<int> ids, params Expression<Func<TEntity, object>>[] includes)
            {
                var token = new CancellationToken();
                return await GetByIdsAsync(token, ids);
            }

            public virtual async Task<IList<TEntity>> GetByIdsAsync(CancellationToken cancellationToken, IEnumerable<int> ids, params Expression<Func<TEntity, object>>[] includes)
            {
                return await GetByIdsCore(ids, cancellationToken);
            }

            private async Task<IList<TEntity>> GetByIdsCore(IEnumerable<int> ids, CancellationToken cancellationToken)
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

            public async Task<IList<TEntity>> GetByIdsAsync(IEnumerable<int> ids, IIncludeDefinition<TEntity>[] includes)
            {
                return await GetByIdsAsync(ids);
            }

            public async Task<IList<TEntity>> GetByIdsAsync(CancellationToken cancellationToken, IEnumerable<int> ids, IIncludeDefinition<TEntity>[] includes)
            {
                return await GetByIdsAsync(cancellationToken, ids);
            }

            private async Task<int?> HandleResponseAsync(HttpResponseMessage response)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                var responseObject = JsonConvert.DeserializeObject<ResponseData>(responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"{responseObject.Code}: {responseObject.Message}");
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
