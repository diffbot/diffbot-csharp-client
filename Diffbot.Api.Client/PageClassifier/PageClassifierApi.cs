using Diffbot.Api.Client.Articles;
using Diffbot.Api.Client.FrontPages;
using Diffbot.Api.Client.Model;
using Diffbot.Api.Client.Products;
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
    public class PageClassifierApi
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
        public PageClassifierApi(string baseApiUrl, string token, string version)
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

        #region Page Classifier

        /// <summary>
        /// Calls the API. 
        /// </summary>
        /// <param name="url">URL to process.</param>
        /// <param name="fields">Array with the fields that will be returned by the API.</param>
        /// <returns>Returns information about the primary article content on the submitted page.</returns>
        public async Task<ClassifierResult> GetPageClassificationAsync(string url, string[] fields, PageType? mode = null, bool stats = false)
        {
            try
            {
                Dictionary<string, string> optionalParameters = new Dictionary<string, string>();
                if (mode != null)
                {
                    optionalParameters.Add("mode", mode.Value.ToString().ToLowerInvariant());
                }

                if (stats)
                {
                    optionalParameters.Add("mode", "true");
                }

                JObject result = await diffbotCall.ApiGetAsync(url, this.token, "analyze", fields, this.version, optionalParameters);

                return CreateClassifierResult(result);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: better exception handler
            }
        }

        private static ClassifierResult CreateClassifierResult(JObject result)
        {
            ClassifierResult classifierResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ClassifierResult>(result.ToString());
            switch (classifierResult.Type)
            {
                case PageType.Article:
                    classifierResult.PageResult = ArticleApi.CreateArticle(result);
                    break;
                case PageType.FrontPage:
                    classifierResult.PageResult = FrontPageApi.CreateFrontPage(result);
                    break;
                case PageType.Image:
                    classifierResult.PageResult = ImageApi.CreateImages(result);
                    break;
                case PageType.Product:
                    classifierResult.PageResult = ProductApi.CreateProduct(result);
                    break;
            }
            
            return classifierResult; 
        }
        #endregion
    }
}
