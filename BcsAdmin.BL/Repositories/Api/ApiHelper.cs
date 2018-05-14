using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                var sw1 = new Stopwatch();
                sw1.Start();
                var response = await httpClient.GetAsync($"{appUrl}/{repoName}", cancellationToken);
                sw1.Stop();
                string responseBody = await response.Content.ReadAsStringAsync();

                var sw2 = new Stopwatch();
                sw2.Start();
                var responseData = JsonConvert.DeserializeObject<EntityResponseData<TSourceEntity[]>>(responseBody);
                sw2.Start();

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiDownException(responseData != null ? $"{responseData.Code}: {responseData.Message}" : "Empty response.");
                }

                return (responseData?.Data ?? new TSourceEntity[] { }).ToList();
            }
        }
    }
}
