using Diffbot.Api.Client.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Bulk
{
    public class BulkJobSettings
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonConverter(typeof(UrlsSpaceSeparatedConverter)), JsonProperty("urls")]
        public IEnumerable<string> Urls { get; set; }

        [JsonProperty("apiUrl")]
        public string ApiUrl { get; set; }

        [JsonProperty("notifyEmail")]
        public string NotifyEmail { get; set; }

        [JsonProperty("notifyWebHook")]
        public string NotifyWebHook { get; set; }
        
        [JsonProperty("repeat")]
        public float? Repeat { get; set; }
        
        [JsonProperty("maxRounds")]
        public int? MaxRounds { get; set; }

        [JsonProperty("pageProcessPattern")]
        public string PageProcessPattern { get; set; }

        public BulkJobSettings(string token, string name, IEnumerable<string> urls, string apiUrl)
        {
            Token = token;
            Name = name;
            Urls = urls;
            ApiUrl = apiUrl;
            NotifyEmail = null;
            NotifyWebHook = null;
            Repeat = null;
            MaxRounds = null;
            PageProcessPattern = null;
        }
    }
}
