using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Model
{
    public class Stats
    {
        public Dictionary<string, double> Times { get; set; }
        public Dictionary<string, double> Types { get; set; }
    }
}
