using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Batch
{
    public class BatchResult
    {
        public List<BatchHeader> Headers { get; set; }
        public string Method { get; set; }
        public int Code { get; set; }
        
        [JsonProperty("relative_url")]
        public string RelativeUrl { get; set; }
        public JObject Body { get; set; }
    }
}
