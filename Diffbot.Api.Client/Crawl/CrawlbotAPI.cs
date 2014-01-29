using Diffbot.Api.Client.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Diffbot.Api.Client.Crawlbot
{
    public class CrawlbotAPI
    {
        private HttpClient httpClient;
        private DiffbotCall diffbotCall;
        private string baseApiUrl;
        private int? version;
        
        /// <summary>
        /// Create an instance of the CrawlbotAPI.
        /// </summary>
        /// <param name="baseApiUrl">URL of the Diffbot Api. (ex. http://api.diffbot.com )</param>
        /// <param name="version">Vesion of the API</param>
        public CrawlbotAPI(string baseApiUrl, string version)
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
        /// Create and start a Crawl Job with the specific parameters.
        /// If the job already exists, updates its status
        /// </summary>
        /// <param name="settings">Settings for the crawl</param>
        /// <returns>An instance of CrawlJob with the settings and update status</returns>
        public async Task<CrawlJob> CreateUpdateJobAsync(CrawlbotSettings settings)
        {
            StringBuilder url = new StringBuilder();
            url.AppendFormat("v{0}/crawl", this.version);
            string separator = "?";
            foreach (var k in JObject.FromObject(settings))
            {
                if (k.Value != null)
                {
                    url.AppendFormat("{0}{1}={2}", separator, k.Key, k.Value);
                    if (separator == "?")
                    {
                        separator = "&";
                    }
                }

            }
            var result = await httpClient.GetStringAsync(url.ToString());

            if (!string.IsNullOrWhiteSpace(result))
            {
                CrawlJob job = JsonConvert.DeserializeObject<CrawlJob>(result);
                job.Token = settings.Token;
                return job;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Pause a job
        /// </summary>
        /// <param name="job">An instance of the CrawlJob</param>
        /// <returns>Returns the updated job</returns>
        public async Task<CrawlJob> PauseAsync(CrawlJob job)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("pause", "1");
            return await CrawlOperations(job, parameters);
        }

        /// <summary>
        /// Resume a job
        /// </summary>
        /// <param name="job">An instance of the CrawlJob</param>
        /// <returns>Returns the updated job</returns>
        public async Task<CrawlJob> ResumeAsync(CrawlJob job)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("pause", "0");
            return await CrawlOperations(job, parameters);
        }

        /// <summary>
        /// Restart a job
        /// </summary>
        /// <param name="job">An instance of the CrawlJob</param>
        /// <returns>Returns the updated job</returns>
        public async Task<CrawlJob> RestartAsync(CrawlJob job)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("restart", "1");
            return await CrawlOperations(job, parameters);
        }

        /// <summary>
        /// Delete a job
        /// </summary>
        /// <param name="job">An instance of the CrawlJob</param>
        /// <returns>Returns the updated job</returns>
        public async Task<CrawlJob> DeleteAsync(CrawlJob job)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("delete", "1");
            return await CrawlOperations(job, parameters);
        }

        private async Task<CrawlJob> CrawlOperations(CrawlJob job, Dictionary<string, string> parameters)
        {
            try
            {
                parameters.Add("token", job.Token);
                parameters.Add("name", job.Name);
                var response = await diffbotCall.GetAsync("bulk", this.version.Value, parameters);
                if (response["Error"] != null)
                {
                    CrawlJob jobResponse = new CrawlJob();
                    jobResponse.Error = response["StatusCode"].ToString() + ": " + response["Error"].ToString();
                    return jobResponse;
                }
                else
                {
                    CrawlJob jobResponse = JsonConvert.DeserializeObject<CrawlJob>(response.ToString());
                    jobResponse.Token = job.Token;
                    return jobResponse;
                }
            }
            catch (Exception ex)
            {
                CrawlJob jobResponse = new CrawlJob();
                jobResponse.Error = ex.InnerException != null ? " " + ex.InnerException.Message : ex.Message;
                return jobResponse;
            }
        }
    }
}
