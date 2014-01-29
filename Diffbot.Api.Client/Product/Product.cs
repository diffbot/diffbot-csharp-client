using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Products
{
    public class Product
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string OfferPrice { get; set; }
        public PriceDetails RegularPriceDetails { get; set; }
        public PriceDetails SaveAmountDetails { get; set; }
        public PriceDetails OfferPriceDetails { get; set; }
        public string SaveAmount { get; set; }
        public List<Medium> Media { get; set; }
        public string RegularPrice { get; set; }
        public bool Availability { get; set; }
        public string ProductId { get; set; }
        public string Sku { get; set; }
        [JsonProperty("color")]
        public List<string> Colors { get; set; }
        public string ISBN { get; set; }
    }
}
