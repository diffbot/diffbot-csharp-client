using Diffbot.Api.Client.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffbot.Api.Client.Articles
{
    public class Article : IPageResult
    {
        internal static readonly string[] PropertyNames = new string[] 
                { "icon", "author", "text", "title", "images", "html", "date", "type", 
                  "url", "resolved_url", "querystring", "links", "numPages", "tags", "supertags",
                  "humanLanguage", "videos" };
        public string Icon { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public List<ArticleImage> Images { get; set; }
        public string Html { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Resolved_Url { get; set; }
        public Dictionary<string,string> QueryString { get; set; }
        public List<string> Links { get; set; }
        public int NumPages { get; set; }
        public List<string> Tags { get; set; }
        //public Dictionary<string, double> Stats { get; set; }
        public string HumanLanguage { get; set; }
        public List<ArticleVideo> Videos { get; set; }
        public List<SuperTag> SuperTags { get; set; }
        public Dictionary<string, object> Properties { get; set; }

    }
}
