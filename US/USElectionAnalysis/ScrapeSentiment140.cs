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

        public static RequestData PostRequest(RequestData data)
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
            return JsonConvert.DeserializeObject<RequestData>(responseString);
        }
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
