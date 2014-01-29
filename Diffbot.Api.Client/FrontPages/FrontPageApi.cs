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

namespace Diffbot.Api.Client.FrontPages
{
    public class FrontPageApi
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
        public FrontPageApi(string baseApiUrl, string token, string version)
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

        #region FrontPage

        /// <summary>
        /// Calls the Article API. 
        /// </summary>
        /// <param name="url">Article URL to process.</param>
        /// <param name="fields">Array with the fields that will be returned by the API.</param>
        /// <returns>Returns information about the primary article content on the submitted page.</returns>
        public async Task<FrontPage> GetFrontPageAsync(string url, Dictionary<string, string> optionalParameters)
        {
            try
            {
                JObject result = await diffbotCall.ApiGetAsync(url, this.token, "frontpage", null, this.version, optionalParameters);

                return CreateFrontPage(result);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: better exception handler
            }
        }

        public async Task<FrontPage> GetFrontPageAsync(string url, Dictionary<string, string> optionalParameters, string html)
        {
            try
            {
                JObject result = await diffbotCall.ApiPostAsync(url, this.token, "frontpage", null, this.version, optionalParameters, html, "text/html");

                return CreateFrontPage(result);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: better exception handler
            }
        }

        public async Task<FrontPage> GetFrontPageAsync(string url, Dictionary<string, string> optionalParameters, Stream htmlStream)
        {
            try
            {
                byte[] buffer = new byte[htmlStream.Length];
                await htmlStream.ReadAsync(buffer, 0, buffer.Length);

                string html = System.Text.UTF8Encoding.UTF8.GetString(buffer);

                return await GetFrontPageAsync(url, optionalParameters, html);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: better exception handler
            }
        }

        internal static FrontPage CreateFrontPage(JObject result)
        {
            FrontPage frontPage = new FrontPage();

            foreach (var item in ((JArray)result["childNodes"]))
            {
                dynamic x = item;
                if (x.tagName.Value == "info")
                {
                    foreach (var cn in x["childNodes"])
                    {
                        var value = cn["childNodes"][0];
                        string tagName = cn["tagName"];
                        switch (tagName)
                        {
                            case "title":
                                frontPage.Title = value;
                                break;
                            case "sourceType":
                                frontPage.Type = value;
                                break;
                            case "sourceURL":
                                frontPage.SourceURL = value;
                                break;
                            case "icon":
                                frontPage.Icon = value;
                                break;
                            case "numItems":
                                frontPage.NumItems = (int)value;
                                break;
                            case "numSpamItems":
                                frontPage.NumSpamItems = (int)value;
                                break;
                            default:
                                frontPage.Properties.Add(tagName, (string)value);
                                break;
                        }
                    }
                }
                else
                {
                    FrontPageItem fpItem = Newtonsoft.Json.JsonConvert.DeserializeObject<FrontPageItem>(x.ToString());
                    foreach (var cn in x["childNodes"])
                    {
                        var value = cn["childNodes"][0];
                        string tagName = cn["tagName"];
                        switch (tagName)
                        {
                            case "pubDate":
                                fpItem.PubDate = (DateTime)value;
                                break;
                            case "description":
                                fpItem.Description = value;
                                break;
                            default:
                                fpItem.Properties.Add(tagName, (string)value);
                                break;
                        }
                    }
                    frontPage.Items.Add(fpItem);
                }
            }
            return frontPage;
        }


        #endregion
    }
}
