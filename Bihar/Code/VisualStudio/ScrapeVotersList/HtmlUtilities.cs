using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Net;

namespace ScrapeVotersList
{
    public class HtmlUtilities
    {
        public static void ParseAndWriteToFile(string responseFromServer, string filename)
        {
            // Parse into TSV
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseFromServer);

            StreamWriter writer = new StreamWriter(filename);
            if (htmlDoc.GetElementbyId("GridView1") != null)
            {
                var rows = htmlDoc.GetElementbyId("GridView1").ChildNodes;

                foreach (var item in rows)
                {
                    if (item is HtmlNode)
                    {
                        var colNodes = ((HtmlNode)item).ChildNodes;
                        string row = String.Join("\t", colNodes
                            .Select(t => { var text = t.InnerText.Trim('\n','\t','\r',' '); return text; })
                            .Where(t => t != String.Empty));

                        if (!row.Equals(String.Empty))
                        {
                            writer.WriteLine(row);
                        }
                    }
                }
            }
            writer.Close();
        }

        public static string QueryServer(int districtId, string firstName, string relFirstName, string bodyPattern)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://210.212.18.115/electorsearch/default.aspx?dist_id={0}", districtId));
            //var postData = String.Format("ScriptManager1=UpdatePanel1%7CcmdShow&__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwUJMzcxOTQ5ODY2DxYCHgV0YWJsZQUYc2VhcmNoMXRvMjQzLmRiby5tZXJnZWQxFgICAw9kFgYCBw8QDxYCHgdDaGVja2VkZ2RkZGQCCw9kFgJmD2QWCAIBDxBkDxYBZhYBEAUFQmloYXIFA1MwNGcWAWZkAgMPEGQPFgFmFgEQBRRQVVJWSSBDSEFNUEFSQU4gLSAwMgUCMDJnFgFmZAIFDxAPFgYeDURhdGFUZXh0RmllbGQFCmFjX25hbWVfZW4eDkRhdGFWYWx1ZUZpZWxkBQVhY19ubx4LXyFEYXRhQm91bmRnZBAVDQNBbGwJMTAtUmF4YXVsCjExLVN1Z2F1bGkLMTItTmFya2F0aWERMTMtSGFyc2lkaGkgKFMuQykNMTQtR292aW5kZ2FuagoxNS1LZXNhcmlhDDE2LUthbHlhbnB1cggxNy1QaXByYQsxOC1NYWRodWJhbgsxOS1Nb3RpaGFyaQsyMC1DaGlyYWl5YQgyMS1EaGFrYRUNATACMTACMTECMTICMTMCMTQCMTUCMTYCMTcCMTgCMTkCMjACMjEUKwMNZ2dnZ2dnZ2dnZ2dnZxYBZmQCBw8QDxYGHwMFB3BhcnRfbm8fAgUHcGFydF9ubx8EZ2QQFQEJQWxsIFBhcnRzFQEBMBQrAwFnZGQCDQ9kFgICAQ9kFgJmD2QWBAIBDzwrAA0BAA8WBB8EZx4LXyFJdGVtQ291bnRmZGQCAw9kFgICBQ88KwANAGQYAwUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFCk9wdEVuZ2xpc2gFCUdyaWRWaWV3Mg9nZAUJR3JpZFZpZXcxDzwrAAoBCGZkOvDGJsRODsq%2FOdp%2BC4tRKKU6EeU%3D&__VIEWSTATEGENERATOR=AC10789D&__EVENTVALIDATION=%2FwEWKgK9r4fDAwLM9PumDwLA9%2FajAwLewfbnCwLwv%2FzCCALZitS8CwLJ5bbRBwLLoaqEDQLbzoDqAQLEzsDpAQLEzszpAQLEzsjpAQLEzvTpAQLEzvDpAQLEzvzpAQLEzvjpAQLEzuTpAQLEzqDqAQLEzqzqAQLFzsDpAQLFzszpAQLOxI%2F9CQKG%2B7bfAQK%2BtuD6CAKf4ZauAQLn4b%2F9CgLm4bPjCgLf4bPjCgKHvOeYCAK7zqbBBwKbzu7HAgLT276FCALJltihDAK489pYAsabrIkDAqa0tJQPAsbZl%2BALAt%2Bju%2F8PAqa0%2FJIPAsbZ3%2BELAv749OwOAsOu2qgKEGzzrI%2FCOARRN%2Fl3qrxjjPMdkYw%3D&Type=OptEnglish&ddstate=S04&dddist={2}&ddcons=0&ddPart=0&txthno=&txtAgeFrom=&txtAgeTo=&ddsex=NA&txtidcard=&txtfName={0}&txtLname=&txtRelFname={1}&txtRelLName=&__ASYNCPOST=true&cmdShow=Show", firstName, relFirstName, districtId);
            var postData = String.Format(bodyPattern, firstName, relFirstName);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = String.Format("http://210.212.18.115/electorsearch/default.aspx?dist_id={0}", districtId);
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
            catch(TimeoutException exception)
            {
                // Display the status.
                Console.WriteLine(exception.Message);
                return null;
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
        }
    }
}
