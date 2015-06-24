using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScrapePollingBooths
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int acNo = 1; acNo <= 243; acNo++)
            {
                var output = HtmlUtilities.GetPostResponse(acNo);
                var booths = HtmlUtilities.ParseResponse(output);
                WriteToFile(booths.options, String.Format("./Booths_{0}.txt", acNo));
                Thread.Sleep(1000);
            }
        }

        public static void WriteToFile(Dictionary<int,string> Booths, string filename)
        {
            File.WriteAllText(filename, String.Join("\n", Booths.Select(x => String.Format("{0}\t{1}", x.Key, x.Value))));
        }

        public static ResponseState GetFirstResponse()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://210.212.18.115:8880/default2.aspx");
            var response = request.GetResponse().GetResponseStream();
            var reader = new StreamReader(response,Encoding.UTF8);
            var responseState = HtmlUtilities.ParseResponse(reader.ReadToEnd());
            return responseState;
        }

        public static string Scrape()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://210.212.18.115:8880/default2.aspx");
            //var postData = String.Format("ScriptManager1=UpdatePanel1%7CcmdShow&__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwUJMzcxOTQ5ODY2DxYCHgV0YWJsZQUYc2VhcmNoMXRvMjQzLmRiby5tZXJnZWQxFgICAw9kFgYCBw8QDxYCHgdDaGVja2VkZ2RkZGQCCw9kFgJmD2QWCAIBDxBkDxYBZhYBEAUFQmloYXIFA1MwNGcWAWZkAgMPEGQPFgFmFgEQBRRQVVJWSSBDSEFNUEFSQU4gLSAwMgUCMDJnFgFmZAIFDxAPFgYeDURhdGFUZXh0RmllbGQFCmFjX25hbWVfZW4eDkRhdGFWYWx1ZUZpZWxkBQVhY19ubx4LXyFEYXRhQm91bmRnZBAVDQNBbGwJMTAtUmF4YXVsCjExLVN1Z2F1bGkLMTItTmFya2F0aWERMTMtSGFyc2lkaGkgKFMuQykNMTQtR292aW5kZ2FuagoxNS1LZXNhcmlhDDE2LUthbHlhbnB1cggxNy1QaXByYQsxOC1NYWRodWJhbgsxOS1Nb3RpaGFyaQsyMC1DaGlyYWl5YQgyMS1EaGFrYRUNATACMTACMTECMTICMTMCMTQCMTUCMTYCMTcCMTgCMTkCMjACMjEUKwMNZ2dnZ2dnZ2dnZ2dnZxYBZmQCBw8QDxYGHwMFB3BhcnRfbm8fAgUHcGFydF9ubx8EZ2QQFQEJQWxsIFBhcnRzFQEBMBQrAwFnZGQCDQ9kFgICAQ9kFgJmD2QWBAIBDzwrAA0BAA8WBB8EZx4LXyFJdGVtQ291bnRmZGQCAw9kFgICBQ88KwANAGQYAwUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFCk9wdEVuZ2xpc2gFCUdyaWRWaWV3Mg9nZAUJR3JpZFZpZXcxDzwrAAoBCGZkOvDGJsRODsq%2FOdp%2BC4tRKKU6EeU%3D&__VIEWSTATEGENERATOR=AC10789D&__EVENTVALIDATION=%2FwEWKgK9r4fDAwLM9PumDwLA9%2FajAwLewfbnCwLwv%2FzCCALZitS8CwLJ5bbRBwLLoaqEDQLbzoDqAQLEzsDpAQLEzszpAQLEzsjpAQLEzvTpAQLEzvDpAQLEzvzpAQLEzvjpAQLEzuTpAQLEzqDqAQLEzqzqAQLFzsDpAQLFzszpAQLOxI%2F9CQKG%2B7bfAQK%2BtuD6CAKf4ZauAQLn4b%2F9CgLm4bPjCgLf4bPjCgKHvOeYCAK7zqbBBwKbzu7HAgLT276FCALJltihDAK489pYAsabrIkDAqa0tJQPAsbZl%2BALAt%2Bju%2F8PAqa0%2FJIPAsbZ3%2BELAv749OwOAsOu2qgKEGzzrI%2FCOARRN%2Fl3qrxjjPMdkYw%3D&Type=OptEnglish&ddstate=S04&dddist={2}&ddcons=0&ddPart=0&txthno=&txtAgeFrom=&txtAgeTo=&ddsex=NA&txtidcard=&txtfName={0}&txtLname=&txtRelFname={1}&txtRelLName=&__ASYNCPOST=true&cmdShow=Show", firstName, relFirstName, districtId);
            var postData = String.Empty;
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = String.Format("http://210.212.18.115/electorsearch/default.aspx?dist_id");
            request.Accept = "*/*";
            request.UserAgent = " Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36";
            request.Host = "210.212.18.115";
            request.ContentLength = data.Length;

            // Increase timeout
            request.Timeout = 100000;
            request.ReadWriteTimeout = 100000;

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
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                return responseFromServer;
            }
            catch (TimeoutException exception)
            {
                // Display the status.
                Console.WriteLine(exception.Message);
                return null;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }

        }
    }
}
