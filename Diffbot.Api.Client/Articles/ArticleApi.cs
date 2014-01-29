using Diffbot.Api.Client.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffbot.Api.Client.Articles
{
    public class ArticleApi
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
        public ArticleApi(string baseApiUrl, string token, string version)
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

        #region Article
        /// <summary>
        /// Calls the Article API. 
        /// </summary>
        /// <param name="url">Article URL to process.</param>
        /// <param name="fields">Array with the fields that will be returned by the API.</param>
        /// <returns>Returns information about the primary article content on the submitted page.</returns>
        public async Task<Article> GetArticleAsync(string url, string[] fields, Dictionary<string, string> optionalParameters)
        {
            try
            {
                JObject result = await diffbotCall.ApiGetAsync(url, this.token, "article", fields, this.version, optionalParameters);

                return  CreateArticle(result);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: better exception handler
            }
        }

        internal static Article CreateArticle(JObject result)
        {
            Article article = Newtonsoft.Json.JsonConvert.DeserializeObject<Article>(result.ToString());

            IDictionary<string, JToken> properties = result;
            // Set all properties in the additional field
            // Then remove those properties that have coded properties in the class Article
            // leaving the rest as additional properties.
            article.Properties = properties.ToDictionary(k => k.Key, k => (object)k.Value);
            foreach (var propName in article.Properties.Where(k => Article.PropertyNames.Contains(k.Key)).ToArray())
            {
                article.Properties.Remove(propName.Key);
            }
            return article;
        }

        /// <summary>
        /// Calls the Article API submitting the html to analyse directly.
        /// </summary>
        /// <param name="url">Article URL to process.</param>
        /// <param name="fields">Array with the fields that will be returned by the API.</param>
        /// <param name="html">Html to be analysed instead of calling the URL. The url parameter is still required.</param>
        /// <returns>Returns information about the primary article content on the submitted page.</returns>
        public async Task<Article> GetArticleAsync(string url, string[] fields, Dictionary<string, string> optionalParameters, string html)
        {
            try
            {
                JObject result = await diffbotCall.ApiPostAsync(url, this.token, "article", fields, this.version, optionalParameters, html, "text/html");

                return CreateArticle(result);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: better exception handler
            }
        }

        /// <summary>
        /// Calls the Article API  submitting the html to analyse directly. 
        /// </summary>
        /// <param name="url">Article URL to process.</param>
        /// <param name="fields">Array with the fields that will be returned by the API.</param>
        /// <param name="htmlStream">Stream with the html to be analysed instead of calling the URL. The url parameter is still required.</param>
        /// <returns>Returns information about the primary article content on the submitted page.</returns>
        public async Task<Article> GetArticleAsync(string url, string[] fields, Dictionary<string, string> optionalParameters, Stream htmlStream)
        {
            byte[] buffer = new byte[htmlStream.Length];
            await htmlStream.ReadAsync(buffer, 0, buffer.Length);

            string html = System.Text.UTF8Encoding.UTF8.GetString(buffer);

            return await GetArticleAsync(url, fields, optionalParameters, html);
        }
        #endregion
    }
}
