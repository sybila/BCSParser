using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.DAL.Api
{
    public static class ApiHelper
    {
        public static async Task<IList<TSourceEntity>> GetWebDataAsync<TSourceEntity>(CancellationToken cancellationToken, string appUrl, string repoName)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{appUrl}/{repoName}", cancellationToken);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<EntityResponseData<TSourceEntity[]>>(responseBody);

                return (responseData?.Data ?? new TSourceEntity[] { }).ToList();
            }
        }
    }
}
