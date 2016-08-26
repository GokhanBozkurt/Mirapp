using System;
using System.Json;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mirapp
{
    public static class Translater
    {
          

        public static async Task<TranslatedWord> TranslateFromRestService(string word)
        {
            try
            {
                string url = "http://cevir.ws/v1?q="+ word+ "&m=1&p=exact&l=en";
                JsonValue json = await FetchWeatherAsync(url);
                var data = JsonConvert.DeserializeObject<TranslatedWord>(json.ToString());
                return data;
            }
            catch (Exception ex)
            {

            }

            return null;
        }


        private static async Task<JsonValue> FetchWeatherAsync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            using (WebResponse response = await request.GetResponseAsync())
            {
                using (System.IO.Stream stream = response.GetResponseStream())
                {
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    return jsonDoc;
                }
            }
        }
    }
}