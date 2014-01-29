using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Products
{
    public class PriceDetails
    {
        public double Amount { get; set; }
        public string Text { get; set; }
        public string Symbol { get; set; }
        public bool? Percentage { get; set; }
    }
}
