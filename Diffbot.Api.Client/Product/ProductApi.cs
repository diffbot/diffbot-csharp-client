using Diffbot.Api.Client.Articles;
using Diffbot.Api.Client.FrontPages;
using Diffbot.Api.Client.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffbot.Api.Client.Products
{
    public class ProductApi
    {
        private DiffbotCall diffbotCall;
        private string token;
        private int version;

         /// <summary>
        /// Create an instance of the ApiClient.
        /// </summary>
        /// <param name="baseApiUrl">URL of the Diffbot Api. (ex. http://api.diffbot.com )</param>
        /// <param name="token">Diffbot API Token required for using the API.</param>
        /// <param name="version">Vesion of the API</param>
        public ProductApi(string baseApiUrl, string token, string version)
        {
            if (string.IsNullOrWhiteSpace(baseApiUrl))
            {
                throw new ArgumentNullException("baseApiUrl");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException("token");
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentException("Incorrect API version", "version");
            }

            this.token = token;
            this.version = int.Parse(version);
            diffbotCall = new DiffbotCall(baseApiUrl);
        }

        #region Product
        /// <summary>
        /// Calls the Products API. 
        /// </summary>
        /// <param name="url">URL to process.</param>
        /// <param name="fields">Array with the fields that will be returned by the API.</param>
        /// <returns>Returns information about the primary article content on the submitted page.</returns>
        public async Task<ProductResult> GetProductsAsync(string url, string[] fields, Dictionary<string, string> optionalParameters)
        {
            try
            {
                JObject result = await diffbotCall.ApiGetAsync(url, this.token, "product", fields, this.version, optionalParameters);

                return CreateProduct(result);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: better exception handler
            }
        }

        internal static ProductResult CreateProduct(JObject result)
        {
            ProductResult products = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductResult>(result.ToString());

            IDictionary<string, JToken> properties = result;
            // Set all properties in the additional field
            // Then remove those properties that have coded properties in the class Article
            // leaving the rest as additional properties.
            products.Properties = properties.ToDictionary(k => k.Key, k => (object)k.Value);
            foreach (var propName in products.Properties.Where(k => ProductResult.PropertyNames.Contains(k.Key)).ToArray())
            {
                products.Properties.Remove(propName.Key); 
            }
            return products;
        }
        #endregion

    }
}
