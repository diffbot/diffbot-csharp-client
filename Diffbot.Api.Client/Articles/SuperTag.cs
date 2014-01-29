using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Articles
{
    public class SuperTag
    {
        public Dictionary<string, string> Categories { get; set; }
        public double ContentMatch { get; set; }
        public double Depth { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<int[]> Positions { get; set; }
        public string Scope { get; set; }
        public double Score { get; set; }
        public int SenseRank { get; set; }
        public int Type { get; set; }
        public double Variety { get; set; }
    }
}
