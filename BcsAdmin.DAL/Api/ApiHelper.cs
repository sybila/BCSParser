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
                
                string responseBody = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<EntityResponseData<TSourceEntity[]>>(responseBody);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiDownException(responseData != null ? $"{responseData.Code}: {responseData.Message}" : "Empty response.");
                }

                return (responseData?.Data ?? new TSourceEntity[] { }).ToList();
            }
        }
    }
}
