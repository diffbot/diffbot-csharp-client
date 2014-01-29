using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Model
{
    public class Image
    {
        public string AttrAlt { get; set; }
        public string Faces { get; set; }
        public List<string> Colors { get; set; }
        public string Caption { get; set; }
        public int PixelWidth { get; set; }
        public string Date { get; set; }
        public int DisplayWidth { get; set; }
        public string Ocr { get; set; }
        public string Url { get; set; }
        public string XPath { get; set; }
        public int DisplayHeight { get; set; }
        public int Size { get; set; }
        public string TextNode { get; set; }
        public int PixelHeight { get; set; }
        public string Mime { get; set; }
        public List<Mention> Mentions { get; set; }
        public List<string> Meta { get; set; }
        public string AnchorUrl { get; set; }
    }
}
