#define UseNewsApiSample  // Remove or undefine to use your own code to read live data

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Net.Http.Json;
using News.Models;
using News.ModelsSampleData;
using System.Collections.Generic;

namespace News.Services
{
    public class NewsService
    {

        //Here is where you lift in your Service code from Part A
        /*
                public async Task<NewsGroup> GetNewsAsync(NewsCategory category)
                {

        #if UseNewsApiSample      
                    NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync(category);

        #else
                    //https://newsapi.org/docs/endpoints/top-headlines
                    var uri = $"https://newsapi.org/v2/top-headlines?country=se&category={category}&apiKey={apiKey}";


                    //Recommend to use Newtonsoft Json Deserializer as it works best with Android
                    var webclient = new WebClient();
                    var json = await webclient.DownloadStringTaskAsync(uri);
                    NewsApiData nd = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsApiData>(json);

        #endif

        */

        public EventHandler<string> NewsAvailable;

        HttpClient httpClient = new HttpClient();

        readonly string apiKey = "29e1ec1a2f9f47c884cedf0628c44402";

        public async Task<NewsGroup> GetNewsAsync(NewsCategory category)
        {

            // NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync(category);

            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category={category}&apiKey={apiKey}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            NewsApiData nd = await response.Content.ReadFromJsonAsync<NewsApiData>();


            NewsGroup news = new NewsGroup();

            news.Articles = new List<NewsItem>();

            nd.Articles.ForEach(a => { news.Articles.Add(GetNewsItem(a)); });

            OnNewsAvailable($"News in category availble:{category}");


            return news;



        }
        private async Task<NewsGroup> ReadNewsApiAsync(string uri)
        {


            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            NewsApiData nd = await response.Content.ReadFromJsonAsync<NewsApiData>();

            NewsGroup news = new NewsGroup();

            news.Articles = new List<NewsItem>();

            nd.Articles.ForEach(a => { news.Articles.Add(GetNewsItem(a)); });

            return news;
        }





        protected virtual void OnNewsAvailable(string c)
        {
            NewsAvailable?.Invoke(this, c);
        }

        private NewsItem GetNewsItem(Article wdListItem)
        {
            NewsItem newsitem = new NewsItem();

            newsitem.DateTime = wdListItem.PublishedAt;

            newsitem.Title = wdListItem.Title;

            newsitem.UrlToImage = wdListItem.UrlToImage;

            newsitem.Url = wdListItem.Url;
            newsitem.Description = wdListItem.Description;

            return newsitem;

        }



    }
}

