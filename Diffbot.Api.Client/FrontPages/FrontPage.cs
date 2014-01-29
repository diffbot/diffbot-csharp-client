using Diffbot.Api.Client.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.FrontPages
{
    public class FrontPage : IPageResult
    {
        public string Title { get; set; }
        public string SourceURL { get; set; }
        
        [JsonProperty("sourceType")]
        public string Type { get; set; }
        public string Icon { get; set; }
        public int NumItems { get; set; }
        public int NumSpamItems { get; set; }
        public Dictionary<string, string> Properties { get; private set; }
        public List<FrontPageItem> Items { get; private set; }

        public FrontPage()
        {
            Items = new List<FrontPageItem>();
            Properties = new Dictionary<string, string>();
        }
    }
}
