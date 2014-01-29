using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Diffbot.Api.Client.Batch
{
    public class BatchApi
    {
        private HttpClient httpClient;
        private DiffbotCall diffbotCall;
        private string baseApiUrl;
        private int? version;

        /// <summary>
        /// Create an instance of the BatchAPI class
        /// </summary>
        /// <param name="baseApiUrl">Base address of the api. See API documentation</param>
        /// <param name="version">Version of the API</param>
        public BatchApi(string baseApiUrl, string version)
        {
            this.baseApiUrl = baseApiUrl;
            if (!string.IsNullOrWhiteSpace(version))
            {
                this.version = int.Parse(version);
            }
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseApiUrl);
            this.diffbotCall = new DiffbotCall(baseApiUrl);
        }

        /// <summary>
        /// Create a batch of up-to 50 individual API calls in a single request
        /// </summary>
        /// <param name="token">API Token</param>
        /// <param name="urls">List of urls APIs to call</param>
        /// <param name="timeout">Timeout of the operation</param>
        /// <returns>Return the result of the calls</returns>
        public async Task<BatchResult> CreateBatchAsync(string token, List<BatchUrlRequest> urls, int? timeout = null)
        {
            StringBuilder parameters = new StringBuilder();
            parameters.AppendFormat("token={0}", token);
            if (timeout.HasValue)
            {
                parameters.AppendFormat("&timeout={0}", timeout.Value);
            }
            parameters.AppendFormat("batch={0}", JsonConvert.SerializeObject(urls));

            var result = await this.httpClient.PostAsync("api/batch", new StringContent(parameters.ToString()));
            if (result.IsSuccessStatusCode)
            {
                string json = await result.Content.ReadAsStringAsync();
                BatchResult batchResult = JsonConvert.DeserializeObject<BatchResult>(json);
                return batchResult;
            }
            else
            {
                BatchResult batchResult = new BatchResult();
                batchResult.Code = (int)result.StatusCode;
                return batchResult;
            }
        }
    }
}
