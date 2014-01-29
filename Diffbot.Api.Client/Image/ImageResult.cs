using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Model
{
    public class ImageResult : IPageResult
    {
        internal static readonly string[] PropertyNames = new string[] 
                { "title", "images", "links", "type", "resolved_url", "url", "date_created" };

        public string Title { get; set; }
        [JsonProperty("images")]
        public List<Image> Items { get; set; }
        public List<string> Links { get; set; }
        public string Type { get; set; }

        [JsonProperty("resolved_url")]
        public string ResolvedUrl { get; set; }
        public string Url { get; set; }
        public string Date_Created { get; set; }
        public Dictionary<string, object> Properties { get; internal set; }

    }
}
