using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Diffbot.Api.Client
{
    internal class DiffbotCall
    {
        HttpClient httpClient;

        public DiffbotCall(string baseApiUrl) // http://api.diffbot.com
        {
            if (string.IsNullOrWhiteSpace(baseApiUrl))
            {
                throw new ArgumentNullException("baseApiUrl");
            }

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseApiUrl);
        }

        public async Task<JObject> ApiGetAsync(string url, string token, string api, string[] fields, int version, Dictionary<string, string> optionalParameters)
        {
            #region Argument Validation
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException("token");
            }

            if (string.IsNullOrWhiteSpace(api))
            {
                throw new ArgumentNullException("api");
            }

            #endregion

            StringBuilder apiUrl = CreateUrl(url, token, api, fields, version, optionalParameters, version != 0);
            if (version == 0)
            {
                apiUrl.Insert(0, "api/");
                apiUrl.Append("&format=json");
            }

            var result = await httpClient.GetStringAsync(apiUrl.ToString());

            return JObject.Parse(result);
        }

        private static StringBuilder CreateUrl(string url, string token, string api, string[] fields, int version, Dictionary<string, string> optionalParameters, bool addVersion)
        {
            StringBuilder apiUrl = new StringBuilder();
            if (addVersion)
            {
                apiUrl.AppendFormat("v{0}/", version);
            }
            apiUrl.AppendFormat("{0}?token={1}", api, token);
            if (fields != null && fields.Length > 0)
            {
                apiUrl.AppendFormat("&fields={0}", string.Join(",", fields));
            }
            if (optionalParameters != null && optionalParameters.Count > 0)
            {
                foreach (var k in optionalParameters)
                {
                    apiUrl.AppendFormat("&{0}={1}", k.Key, k.Value);
                }
            }
            apiUrl.AppendFormat("&url={0}", url);
            return apiUrl;
        }

        public async Task<JObject> ApiPostAsync(string url, string token, string api, string[] fields, int version, Dictionary<string, string> optionalParameters, string html, string contentType)
        {
            #region Argument Validation
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException("token");
            }

            if (string.IsNullOrWhiteSpace(api))
            {
                throw new ArgumentNullException("api");
            }

            if (string.IsNullOrWhiteSpace(html))
            {
                throw new ArgumentNullException("html");
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentNullException("contentType");
            }

            // TODO: Validate this is a valid check
            if (version < 2)
            {
                throw new ArgumentException("Invalid api version");
            }
            #endregion

            StringBuilder apiUrl = CreateUrl(url, token, api, fields, version, optionalParameters, false);

            var formatter = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            var result = await httpClient.PostAsync<string>(apiUrl.ToString(), html, formatter);

            if (result.IsSuccessStatusCode)
            {
                return JObject.Parse(await result.Content.ReadAsStringAsync());
            }
            else
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }
        }

        public async Task<JObject> PostAsync(string api, int version, object data)
        {
            StringBuilder apiUrl = new StringBuilder();
            apiUrl.AppendFormat("v{0}/", version);
            apiUrl.Append(api);

            JObject requestObject = JObject.FromObject(data);
            StringBuilder request = new StringBuilder();
            foreach (var k in requestObject)
            {
                request.AppendFormat("{0}={1}&", k.Key, k.Value.ToString());
            }            

            var result = await httpClient.PostAsync(apiUrl.ToString(), new StringContent(request.ToString().TrimEnd('&')));
            if (result.IsSuccessStatusCode)
            {
                string response = await result.Content.ReadAsStringAsync();
                return JObject.Parse(response);
            }
            else
            {
                return JObject.FromObject(new { Error = result.ReasonPhrase, StatusCode = result.StatusCode});
            }
        }

        public async Task<JObject> GetAsync(string api, int version, Dictionary<string, string> parameters)
        {
            StringBuilder apiUrl = new StringBuilder();
            apiUrl.AppendFormat("v{0}/", version);
            apiUrl.Append(api);
            if (parameters != null)
            {
                apiUrl.Append("?");
                apiUrl.Append(string.Join("&", parameters.Select(p => string.Format("{0}={1}", p.Key, p.Value)).ToArray()));
            }

            var result = await httpClient.GetAsync(apiUrl.ToString());
            if (result.IsSuccessStatusCode)
            {
                string response = await result.Content.ReadAsStringAsync();
                return JObject.Parse(response);
            }
            else
            {
                return JObject.FromObject(new { Error = result.ReasonPhrase, StatusCode = result.StatusCode });
            }
        }
    }
}
