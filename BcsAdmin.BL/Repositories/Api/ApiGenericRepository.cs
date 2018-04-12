using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL.Repositories.Api
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Riganti.Utils.Infrastructure.Core;
    using Riganti.Utils.Infrastructure.EntityFrameworkCore;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Net.Http;
    using Newtonsoft.Json;
    using global::BcsAdmin.DAL.Api;

    namespace BcsAdmin.BL.Repositories
    {
        public class ApiEntityRepository : ApiGenericRepository<ApiEntity>
        {
            public ApiEntityRepository(IDateTimeProvider dateTimeProvider)
                : base(dateTimeProvider)
            {
                RepoName = "entities";
            }
        }

        public class ApiClassificationRepository : ApiGenericRepository<ApiClassification>
        {
            public ApiClassificationRepository(IDateTimeProvider dateTimeProvider)
                : base(dateTimeProvider)
            {
                RepoName = "classifications";
            }
        }

        public class ApiOrganismsRepository : ApiGenericRepository<ApiOrganism>
        {
            public ApiOrganismsRepository(IDateTimeProvider dateTimeProvider)
                : base(dateTimeProvider)
            {
                RepoName = "organisms";
            }
        }

        public class ApiRulesRepository : ApiGenericRepository<ApiOrganism>
        {
            public ApiRulesRepository(IDateTimeProvider dateTimeProvider)
                : base(dateTimeProvider)
            {
                RepoName = "rules";
            }
        }

        public class ApiGenericRepository<TEntity> : IRepository<TEntity, int>
            where TEntity : class, IEntity<int>, new()
        {
            public string AppUrl { get; set; } = "https://api.e-cyanobacterium.org/";
            public string RepoName { get; set; }
            protected virtual string GetFullUrl(int id) => $"{AppUrl}/{RepoName}/{id}";
            protected virtual string GetFullUrl() => $"{AppUrl}/{RepoName}";

            protected HttpClient HttpClient { get; set; } = new HttpClient();


            public ApiGenericRepository(IDateTimeProvider dateTimeProvider)
            {
            }

            public TEntity InitializeNew()
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
                HttpClient.DeleteAsync(GetFullUrl(id)).Wait();
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
                HttpClient.PutAsync(GetFullUrl(entity.Id), new StringContent(JsonConvert.SerializeObject(entity))).Wait();
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
                HttpClient.PostAsync(GetFullUrl(), new StringContent(JsonConvert.SerializeObject(entity))).Wait();
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
                var response = await HttpClient.GetAsync(GetFullUrl(id), cancellationToken);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<ResponseData<TEntity>>(responseBody);

                return responseData.Data.Data;
            }

            public async Task<TEntity> GetByIdAsync(int id, IIncludeDefinition<TEntity>[] includes)
            {
                return await GetByIdAsync(id);
            }

            public Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, int id, IIncludeDefinition<TEntity>[] includes)
            {
                throw new NotImplementedException();
            }

            public IList<TEntity> GetByIds(IEnumerable<int> ids, params Expression<Func<TEntity, object>>[] includes)
            {
                throw new NotImplementedException();
            }

            public IList<TEntity> GetByIds(IEnumerable<int> ids, IIncludeDefinition<TEntity>[] includes)
            {
                throw new NotImplementedException();
            }

            public Task<IList<TEntity>> GetByIdsAsync(IEnumerable<int> ids, params Expression<Func<TEntity, object>>[] includes)
            {
                throw new NotImplementedException();
            }

            public Task<IList<TEntity>> GetByIdsAsync(CancellationToken cancellationToken, IEnumerable<int> ids, params Expression<Func<TEntity, object>>[] includes)
            {
                throw new NotImplementedException();
            }

            public Task<IList<TEntity>> GetByIdsAsync(IEnumerable<int> ids, IIncludeDefinition<TEntity>[] includes)
            {
                throw new NotImplementedException();
            }

            public Task<IList<TEntity>> GetByIdsAsync(CancellationToken cancellationToken, IEnumerable<int> ids, IIncludeDefinition<TEntity>[] includes)
            {
                throw new NotImplementedException();
            }

            private class ResponseData<TTEntity> where TTEntity : class, IEntity<int>, new()
            {
                public ResponseStatus Status { get; set; }
                public string Messsage { get; set; }
                public int Code { get; set; }
                public WtfData<TTEntity> Data { get; set; }
            }

            private class WtfData<TTEntity> where TTEntity : class, IEntity<int>, new()
            {
                public TEntity Data { get; set; }
            }
            public enum ResponseStatus
            {
                Ok,
                Error
            }
        }
    }

}
