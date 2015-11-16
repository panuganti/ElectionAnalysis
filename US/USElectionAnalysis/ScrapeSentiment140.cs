using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace USElectionAnalysis
{
    public class ScrapeSentiment140
    {
        public static async Task<RequestData> PostRequestAsync(RequestData data)
        {
            using (var client = new HttpClient())
            {
                string jsonString = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://www.sentiment140.com/api/bulkClassifyJson?appid=rajkiran.panuganti@gmail.com", content);
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RequestData>(responseString);
            }
        }

        public static WebsiteData PostRequest(RequestData data)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://www.sentiment140.com/api/bulkClassifyJson?appid=rajkiran.panuganti@gmail.com");

            string jsonString = JsonConvert.SerializeObject(data);
            var postData = Encoding.ASCII.GetBytes(jsonString);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var responseData = JsonConvert.DeserializeObject<RequestData>(responseString);
            var websiteData = new WebsiteData();
            websiteData.Summary = responseData.data.GroupBy(t => t.time.Date).Select(y => new Summary() { Time = y.Key.ToShortDateString(), Total = y.Count(), Positive = y.Count(z=> z.polarity == 4), Negative = y.Count(z=> z.polarity == 0)}).ToArray();
            websiteData.Tweets = responseData.data.Select(y => new Tweet() { Time = y.time.ToShortDateString(), Text = y.text, Sentiment = y.polarity.ToString()}).ToArray();
            return websiteData;
        }
    }

    [DataContract]
    public class WebsiteData
    {
        [DataMember]
        public Summary[] Summary { get; set; }
        [DataMember]
        public Tweet[] Tweets { get; set; }
    }

    [DataContract]
    public class Tweet
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Sentiment { get; set; }
        [DataMember]
        public string Time { get; set; }
    }

    [DataContract]
    public class Summary
    {
        [DataMember]
        public string Time { get; set; }
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public int Positive { get; set; }
        [DataMember]
        public int Negative { get; set; }
    }

    [DataContract]
    public enum Polarity
    {
        Positive,
        Negative,
        CantSay,
    }

    [DataContract]
    public class RequestData
    {
        [DataMember]
        public Data[] data { get; set; }
    }

    [DataContract]
    public class Data
    {
        public DateTime time { get; set; }
        [DataMember]
        public string text { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int polarity { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Meta meta { get; set; }
    }

    [DataContract]
    public class Meta
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string headline { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string language { get; set; }
    }

}
