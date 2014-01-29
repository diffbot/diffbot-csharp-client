using Diffbot.Api.Client.Bulk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Crawlbot
{
    public class CrawlJob
    {
        public string Error { get; set; }
        public bool HasError { get { return !string.IsNullOrWhiteSpace(Error); } }
        public string Token { get; set; }
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
        public int MaxToCrawl { get; set; }
        public int MaxToProcess { get; set; }
        public int MaxRounds { get; set; }
        public int ObeyRobots { get; set; }
        public int RestrictDomain { get; set; }
        public int Repeat { get; set; }
        public double CrawlDelay { get; set; }
        public int OnlyProcessIfNew { get; set; }
        public string Seeds { get; set; }
        public int RoundsCompleted { get; set; }
        public int RoundStartTime { get; set; }
        public int CurrentTime { get; set; }
        public string PageProcessPattern { get; set; }
        public string NotifyEmail { get; set; }
        public string NotifyWebHook { get; set; }
        public string DownloadJson { get; set; }
        public string DownloadUrls { get; set; }
        public List<UrlFilter> UrlFilters { get; set; }
    }

    public class UrlFilter
    {
        public string Expression { get; set; }
        public string Action { get; set; }
    }
}
