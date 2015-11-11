using System;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;


namespace PredictTweetSentiment
{
    /*
    {
  "input": 
  {
    "csvInstance": 
    ["Amy, Charles Schumer call for better gun control"
    ]
  }
}
    */

    [DataContract]
    public class input
    {
        [DataMember]
        public string[] csvInstance { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * POST https://content.googleapis.com/prediction/v1.6/projects/472327146236/trainedmodels/guncontroltweetsentiment20150813v3/predict?key=AIzaSyCFj15TpkchL4OUhLD1Q2zgxQnMb7v3XaM&alt=json HTTP/1.1
Host: content.googleapis.com
Connection: keep-alive
Content-Length: 151
Authorization: Bearer ya29.zwECBYn-2OfdIUZ3l8Ndhj67lYgREVXwEXous8CtQspbNZ4Yy_X_HrkISc6DJINLlr-Vbw
Origin: https://content.googleapis.com
X-Origin: https://developers.google.com
X-ClientDetails: appVersion=5.0%20(Windows%20NT%206.1%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F44.0.2403.155%20Safari%2F537.36&platform=Win32&userAgent=Mozilla%2F5.0%20(Windows%20NT%206.1%3B%20WOW64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F44.0.2403.155%20Safari%2F537.36
X-Goog-Encode-Response-If-Executable: base64
User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.155 Safari/537.36
Content-Type: application/json
X-JavaScript-User-Agent: google-api-javascript-client/1.1.0-beta
X-Referer: https://developers.google.com
Accept: * /*
X-Client-Data: CI62yQEIo7bJAQiptskBCMC2yQEI6ojKAQj9lcoB
DNT: 1
Referer: https://content.googleapis.com/static/proxy.html?jsh=m%3B%2F_%2Fscs%2Fapps-static%2F_%2Fjs%2Fk%3Doz.gapi.en.ZxCX5BzY848.O%2Fm%3D__features__%2Fam%3DEQ%2Frt%3Dj%2Fd%3D1%2Ft%3Dzcms%2Frs%3DAGLTcCNUknOYjzlF7QmqV_z-9KmahtP5_Q
Accept-Encoding: gzip, deflate
Accept-Language: en-US,en;q=0.8,ms;q=0.6

{
  "input": 
  {
    "csvInstance": 
    ["Chicago Moms Take to the Street to Prevent Violence, Doing a Better Job than Cops  Gun Control"
    ]
  }
}
             */

            var tweet = new input() { csvInstance = new string[] { "Amy, Charles Schumer call for better gun control" } };

            var modelName = "guncontroltweetsentiment20150813v3";
            var postData = JsonConvert.SerializeObject(tweet);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                String.Format("https://content.googleapis.com/prediction/v1.6/projects/472327146236/trainedmodels/{0}/predict?key=AIzaSyCFj15TpkchL4OUhLD1Q2zgxQnMb7v3XaM&alt=json", 
                modelName));
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentLength = data.Length;
            request.Host = "content.googleapis.com";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.155 Safari/537.36";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.8,ms;q=0.6");
            request.Referer = "https://content.googleapis.com/static/proxy.html?jsh=m%3B%2F_%2Fscs%2Fapps-static%2F_%2Fjs%2Fk%3Doz.gapi.en.ZxCX5BzY848.O%2Fm%3D__features__%2Fam%3DEQ%2Frt%3Dj%2Fd%3D1%2Ft%3Dzcms%2Frs%3DAGLTcCNUknOYjzlF7QmqV_z-9KmahtP5_Q";
            request.Headers.Add("Authorization", "Bearer ya29.zwECBYn-2OfdIUZ3l8Ndhj67lYgREVXwEXous8CtQspbNZ4Yy_X_HrkISc6DJINLlr-Vbw");


            /*
            var postData = String.Format(bodyPattern, acId, boothId, viewState, eventValidation, viewStateGenerator);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.Host = "210.212.18.115:8880";
            request.ContentLength = data.Length;
            request.UserAgent = " Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,* /*;q=0.8";
            request.Referer = "http://210.212.18.115:8880/";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            //request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.8,ms;q=0.6");

            // Write body data to requestStream
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(data, 0, data.Length);
            dataStream.Close();
            try
            {
                // Get the response.
                WebResponse response = request.GetResponse();
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                //StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                //var responseStream = new GZipStream(dataStream, CompressionMode.Decompress);
                //StreamReader reader = new StreamReader(responseStream);

                //string responseFromServer = reader.ReadToEnd();
                var bytes = ReadFully(dataStream, response.ContentLength);
                File.WriteAllBytes(filename, bytes);
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            }
            catch (TimeoutException exception)
            {
                // Display the status.
                Console.WriteLine(exception.Message);
            }
             * 
             */
        }
    }
}
