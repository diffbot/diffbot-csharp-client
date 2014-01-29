using Diffbot.Api.Client.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Products
{
    public class ProductResult : IPageResult
    {
        internal static readonly string[] PropertyNames = new string[] { "leafPage", "products", "links", "type", "url", "date_created" };

        public bool LeafPage { get; set; }
        [JsonProperty("date_created")]
        public string DateCreated { get; set; }
        public string Type { get; set; }
        public List<string> Links { get; set; }
        [JsonProperty("products")]
        public List<Product> Items { get; set; }
        public string Url { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}
