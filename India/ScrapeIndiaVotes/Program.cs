using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO.Compression;
using System.IO;

namespace ScrapeIndiaVotes
{
    class Program
    {
        static void Main(string[] args)
        {
            int startPC = 7824;
            int electionId = 16;
            int stateId = 58;
            string dirPath = @"J:\ArchishaData\ElectionData\RawData\IndiaVotes";
            for(int pcNo = 1; pcNo<=40; pcNo++)
            {
                File.WriteAllText(String.Format(@"{0}\\{1}_{2}_{3}.html", dirPath, electionId, stateId, pcNo), GetPostResponse(pcNo, startPC, electionId, stateId));
            }
        }

        public static string GetPostResponse(int pcNo, int startPC, int electionId, int stateId)
        {
            var request = (HttpWebRequest)WebRequest.Create(String.Format("http://www.indiavotes.com/pc/detail/{0}/{1}/{2}", startPC + pcNo, stateId, electionId));
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Host = "www.indiavotes.com";

            /* Write body data to requestStream
            Stream dataStream = request.GetRequestStream();
            dataStream.Close();
             */
            try
            {
                // Get the response.
                WebResponse response = request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();

                var responseStream = new GZipStream(dataStream, CompressionMode.Decompress);

                // Open the stream using a StreamReader for easy access.

                StreamReader reader = new StreamReader(responseStream);
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
