using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Bulk
{
    public class BulkJob
    {
        public string Name { get { return Settings.Name; } }
        public IEnumerable<string> Urls { get { return Settings.Urls; } }

        public BulkJobSettings Settings { get; private set; }

        public string Error { get; internal set; }

        public bool HasError { get { return !string.IsNullOrEmpty(Error); } }

        public Job JobStatus { get; set; }

        public BulkJob(BulkJobSettings settings)
        {
            this.Settings = settings;
        }
    }
}
