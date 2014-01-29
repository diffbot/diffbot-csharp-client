using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Bulk
{
    public class Job
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public JobStatus JobStatus { get; set; }
        public int SentJobDoneNotification { get; set; }
        public int ObjectsFound { get; set; }
        public int UrlsHarvested { get; set; }
        public int PageCrawlAttempts { get; set; }
        public int PageCrawlSuccesses { get; set; }
        public int PageProcessAttempts { get; set; }
        public int PageProcessSuccesses { get; set; }
        public int MaxRounds { get; set; }
        public double Repeat { get; set; }
        public double CrawlDelay { get; set; }
        public int RoundsCompleted { get; set; }
        public int RoundStartTime { get; set; }
        public int CurrentTime { get; set; }
        public string ApiUrl { get; set; }
        public string UrlCrawlPattern { get; set; }
        public string UrlProcessPattern { get; set; }
        public string PageProcessPattern { get; set; }
        public string UrlCrawlRegEx { get; set; }
        public string UrlProcessRegEx { get; set; }
        public string DownloadJson { get; set; }
        public string DownloadUrls { get; set; }
        public string NotifyEmail { get; set; }
        public string NotifyWebhook { get; set; }
    }

    public class JobStatus
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }

    internal class Jobs
    {
        [JsonProperty("jobs")]
        public Job[] AllJobs { get; set; }
    }
}
