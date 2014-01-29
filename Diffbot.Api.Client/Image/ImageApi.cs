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

namespace Diffbot.Api.Client
{
    public class ImageApi
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
        public ImageApi(string baseApiUrl, string token, string version)
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

        #region Image
        /// <summary>
        /// Calls the Article API. 
        /// </summary>
        /// <param name="url">Article URL to process.</param>
        /// <param name="fields">Array with the fields that will be returned by the API.</param>
        /// <returns>Returns information about the primary article content on the submitted page.</returns>
        public async Task<ImageResult> GetImagesAsync(string url, string[] fields, Dictionary<string, string> optionalParameters)
        {
            try
            {
                JObject result = await diffbotCall.ApiGetAsync(url, this.token, "image", fields, this.version, optionalParameters);

                return CreateImages(result);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: better exception handler
            }
        }

        internal static ImageResult CreateImages(JObject result)
        {
            ImageResult images = Newtonsoft.Json.JsonConvert.DeserializeObject<ImageResult>(result.ToString());

            IDictionary<string, JToken> properties = result;
            // Set all properties in the additional field
            // Then remove those properties that have coded properties in the class Article
            // leaving the rest as additional properties.
            images.Properties = properties.ToDictionary(k => k.Key, k => (object)k.Value);
            foreach (var propName in images.Properties.Where(k => ImageResult.PropertyNames.Contains(k.Key)).ToArray())
            {
                images.Properties.Remove(propName.Key);
            }
            return images;
        }
        #endregion

    }
}
