using Diffbot.Api.Client.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffbot.Api.Client.Bulk
{
    public class BulkApi
    {
        private DiffbotCall diffbotCall;
        private int version;

        #region Constructors

        /// <summary>
        /// Create an instance of the ApiClient.
        /// </summary>
        /// <param name="baseApiUrl">URL of the Diffbot Api. (ex. http://api.diffbot.com )</param>
        /// <param name="token">Diffbot API Token required for using the API.</param>
        /// <param name="version">Vesion of the API</param>
        public BulkApi(string baseApiUrl, string version)
        {
            if (string.IsNullOrWhiteSpace(baseApiUrl))
            {
                throw new ArgumentNullException("baseApiUrl");
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException("Incorrect API version", "version");
            }

            this.version = int.Parse(version);
            diffbotCall = new DiffbotCall(baseApiUrl);
        }

        #endregion

        /// <summary>
        /// Create a job based on the settings recieved. This method returns the data needed for the 
        /// actual call to the API.
        /// </summary>
        /// <param name="settings">Settings for making the call</param>
        /// <returns>An instance of BulkJob with the data need for the call of the API</returns>
        public BulkJob CreateJob(BulkJobSettings settings)
        {
            return new BulkJob(settings);
        }

        /// <summary>
        /// Create a job based on the settings recieved. This method returns the data needed for the 
        /// actual call to the API.
        /// </summary>
        /// <param name="token">API Token</param>
        /// <param name="name">Name of the Job</param>
        /// <param name="urls">Urls to process</param>
        /// <param name="apiUrl">URL of the API to use to process the URLs</param>
        /// <returns>An instance of BulkJob with the data need for the call of the API</returns>
        public BulkJob CreateJob(string token, string name, IEnumerable<string> urls, string apiUrl)
        {
            return new BulkJob(new BulkJobSettings(token, name, urls, apiUrl));
        }

        /// <summary>
        /// Starts the job.
        /// </summary>
        /// <param name="job">An instance of the BulkJob created by the method CreateJob</param>
        public async Task StartAsync(BulkJob job)
        {
            try
            {
                var response = await diffbotCall.PostAsync("bulk", this.version, job.Settings);
                if (response["Error"] != null)
                {
                    job.Error = response["StatusCode"].ToString() + ": " + response["Error"].ToString();
                }
                else
                {
                    job.Error = null;
                    Jobs jobs = JsonConvert.DeserializeObject<Jobs>(response.ToString());
                    job.JobStatus = jobs.AllJobs.FirstOrDefault(j => j.Name == job.Name);
                }
            }
            catch (Exception ex)
            {
                job.Error = ex.InnerException != null ? " " + ex.InnerException.Message : ex.Message;
            }
        }

        /// <summary>
        /// Pause a job
        /// </summary>
        /// <param name="job">An instance of the BulkJob created by the method CreateJob</param>
        public async Task PauseAsync(BulkJob job)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("pause", "1");
            await BulkOperations(job, parameters);

        }

        /// <summary>
        /// Resumes a job
        /// </summary>
        /// <param name="job">An instance of the BulkJob created by the method CreateJob</param>
        public async Task ResumeAsync(BulkJob job)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("pause", "0");
            await BulkOperations(job, parameters);
        }

        /// <summary>
        /// Delete a job
        /// </summary>
        /// <param name="job">An instance of the BulkJob created by the method CreateJob</param>
        public async Task DeleteAsync(BulkJob job)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("delete", "1");
            await BulkOperations(job, parameters);
        }

        /// <summary>
        /// Update the status of a job
        /// </summary>
        /// <param name="job">An instance of the BulkJob created by the method CreateJob</param>
        public async Task UpdateStatusAsync(BulkJob job)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            await BulkOperations(job, parameters);
        }

       private async Task BulkOperations(BulkJob job, Dictionary<string, string> parameters)
        {
            try
            {
                parameters.Add("token", job.Settings.Token);
                parameters.Add("name", job.Name);
                var response = await diffbotCall.GetAsync("bulk", this.version, parameters);
                if (response["Error"] != null)
                {
                    job.Error = response["StatusCode"].ToString() + ": " + response["Error"].ToString();
                }
                else
                {
                    job.Error = null;
                    Jobs jobs = JsonConvert.DeserializeObject<Jobs>(response.ToString());
                    if (jobs.AllJobs != null)
                    {
                        job.JobStatus = jobs.AllJobs.FirstOrDefault(j => j.Name == job.Name);
                    }
                    else
                    {
                        job.JobStatus = null;
                    }
                }
            }
            catch (Exception ex)
            {
                job.Error = ex.InnerException != null ? " " + ex.InnerException.Message : ex.Message;
            }
        }
    }
}
