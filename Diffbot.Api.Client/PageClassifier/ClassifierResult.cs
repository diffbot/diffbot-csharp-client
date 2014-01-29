using Diffbot.Api.Client.Articles;
using Diffbot.Api.Client.FrontPages;
using Diffbot.Api.Client.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diffbot.Api.Client.Model
{
    public class ClassifierResult
    {
        public string Title { get; set; }
        public Stats Stats { get; set; }
        public PageType Type { get; set; }
        [JsonProperty("human_language")]
        public string HumanLanguage { get; set; }
        public string Url { get; set; }

        internal object PageResult { get; set; }

        public T GetPageData<T>() where T : class, IPageResult
        {
            switch (this.Type)
            {
                case PageType.Article:
                    if (typeof(T).Name == typeof(Article).Name)
                    {
                        return (T)PageResult;
                    }
                    break;
                case PageType.FrontPage:
                    if (typeof(T).Name == typeof(FrontPage).Name)
                    {
                        return (T)PageResult;
                    }
                    break;
                case PageType.Image:
                    if (typeof(T).Name == typeof(ImageResult).Name)
                    {
                        return (T)PageResult;
                    }
                    break;
                case PageType.Product:
                    if (typeof(T).Name == typeof(ProductResult).Name)
                    {
                        return (T)PageResult;
                    }
                    break;
                case PageType.None:
                case PageType.Audio:
                case PageType.Chart:
                case PageType.Discussion:
                case PageType.Document:
                case PageType.Download:
                case PageType.Error:
                case PageType.Event:
                case PageType.FAQ:
                case PageType.Game:
                case PageType.Job:
                case PageType.Location:
                case PageType.Other:
                case PageType.Profile:
                case PageType.Recipe:
                case PageType.ReviewsList:
                case PageType.Serp:
                case PageType.Video:
                default:
                    break;
            }

            return (T)null;
        }

    }
}
