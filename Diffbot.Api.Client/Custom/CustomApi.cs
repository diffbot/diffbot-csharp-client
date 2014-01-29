using Diffbot.Api.Client.Articles;
using Diffbot.Api.Client.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffbot.Api.Client.CustomApi
{
    public class CustomApi
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
        public CustomApi(string baseApiUrl, string token, string version)
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

        #region Custom API
        /// <summary>
        /// Calls the Custom API. 
        /// </summary>
        /// <param name="url">URL to process.</param>
        /// <param name="fields">Array with the fields that will be returned by the API.</param>
        /// <returns>Returns information about the primary article content on the submitted page.</returns>
        public async Task<JObject> CallCustomAPIAsync(string url, string customApiName, Dictionary<string, string> optionalParameters)
        {
            try
            {
                return await diffbotCall.ApiGetAsync(url, this.token, customApiName, null, this.version, optionalParameters);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: better exception handler
            }
        }
        
        /// <summary>
        /// Calls the API submitting the html to analyse directly.
        /// </summary>
        /// <param name="url">URL to process.</param>
        /// <param name="fields">Array with the fields that will be returned by the API.</param>
        /// <param name="html">Html to be analysed instead of calling the URL. The url parameter is still required.</param>
        /// <returns>Returns information about the primary article content on the submitted page.</returns>
        public async Task<JObject> CallCustomAPIAsync(string url, string customApiName, Dictionary<string, string> optionalParameters, string html)
        {
            try
            {
                return await diffbotCall.ApiPostAsync(url, this.token, customApiName, null, this.version, optionalParameters, html, "text/html");
            }
            catch (Exception ex)
            {
                throw ex; //TODO: better exception handler
            }
        }

        /// <summary>
        /// Calls the API submitting the html to analyse directly. 
        /// </summary>
        /// <param name="url">URL to process.</param>
        /// <param name="fields">Array with the fields that will be returned by the API.</param>
        /// <param name="htmlStream">Stream with the html to be analysed instead of calling the URL. The url parameter is still required.</param>
        /// <returns>Returns information about the primary article content on the submitted page.</returns>
        public async Task<JObject> CallCustomAPIAsync(string url, string customApiName, Dictionary<string, string> optionalParameters, Stream htmlStream)
        {
            byte[] buffer = new byte[htmlStream.Length];
            await htmlStream.ReadAsync(buffer, 0, buffer.Length);

            string html = System.Text.UTF8Encoding.UTF8.GetString(buffer);

            return await CallCustomAPIAsync(url, customApiName, optionalParameters, html);
        }
        #endregion
    }
}
