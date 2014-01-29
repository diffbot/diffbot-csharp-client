using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.FrontPages
{
    public class FrontPageItem 
    {
        public string Cluster { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string XRoot { get; set; }
        public DateTime? PubDate { get; set; }
        public string Link { get; set; }
        public FrontPageItemType Type { get; set; }
        public string Img { get; set; }
        public string TextSummary { get; set; }
        public double Sp { get; set; }
        public double Sr { get; set; }
        public double Fresh { get; set; }
        public string Geometry { get; set; }
        public Dictionary<string, string> Properties { get; private set; }

        public FrontPageItem()
        {
            this.Properties = new Dictionary<string, string>();
        }
    }
}
