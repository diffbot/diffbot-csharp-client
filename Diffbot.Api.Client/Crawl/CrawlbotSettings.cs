using Diffbot.Api.Client.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Crawlbot
{
    public class CrawlbotSettings
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonConverter(typeof(UrlsSpaceSeparatedConverter)), JsonProperty("seeds")]
        public List<string> Seeds { get; set; }
        
        [JsonProperty("apiUrl")]
        public string ApiUrl { get; set; }
        
        [JsonProperty("urlCrawlPattern")]
        public string UrlCrawlPattern { get; set; }
        
        [JsonProperty("urlCrawlRegEx")]
        public string UrlCrawlRegEx { get; set; }
        
        [JsonProperty("pageProcessPattern")]
        public string PageProcessPattern { get; set; }
        
        [JsonProperty("maxToCrawl")]
        public int? MaxToCrawl { get; set; }
        
        [JsonProperty("maxToProcess")]
        public int? MaxToProcess { get; set; }
        
        [JsonProperty("restrictDomain")]
        public bool? RestrictDomain { get; set; }
        
        [JsonProperty("notifyEmail")]
        public string NotifyEmail { get; set; }
        
        [JsonProperty("notifyWebHook")]
        public string NotifyWebHook { get; set; }
        
        [JsonProperty("crawlDelay")]
        public double? CrawlDelay { get; set; }
        
        [JsonProperty("repeat")]
        public double? Repeat { get; set; }
        
        [JsonProperty("onlyProcessIfNew")]
        public bool? OnlyProcessIfNew { get; set; }
        
        [JsonProperty("maxRounds")]
        public int? MaxRounds { get; set; }
        
        public CrawlbotSettings()
        {
            Seeds = new List<string>();
        }
    }
}
