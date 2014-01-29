using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffbot.Api.Client.Articles
{
    public class ArticleImage
    {
        public bool Primary { get; set; }
        public string Caption { get; set; }
        public string Url { get; set; }
        public int PixelHeight { get; set; }
        public int PixelWidth { get; set; }
    }
}
