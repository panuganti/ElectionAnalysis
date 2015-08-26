using System;
using System.Net;
using System.IO.Compression;
using System.IO;

namespace ScrapeIndiaVotes
{
    class Program
    {
        static void Main(string[] args)
        {
            const int startPC = 7824;
            const int electionId = 212;
            const int stateId = 58;
            const string dirPath = @"I:\ArchishaData\ElectionData\RawData\IndiaVotes";
            const string outDir = @"I:\ArchishaData\ElectionData\RawData\IndiaVotes\2010BiharAC\";

            for(int acNo = 1; acNo<=243; acNo++)
            {
                var response = GetACResult(stateId, electionId, acNo, 31403);
                string filename = String.Format(@"{0}\\{1}_{2}_{3}.html", outDir, electionId, stateId, acNo);
                File.WriteAllText(filename,response);
                //ParseACWiseIndiaVotesPage.ParsePage(filename, Path.Combine(outDir, "AcWise"));
            }
        }

        public static string GetACResult(int stateId, int electionId, int acNo, int startAC)
        {
            return ScrapeIndiaVotes(String.Format("http://www.indiavotes.com/ac/details/{0}/{1}/{2}", stateId, startAC + acNo, electionId));            
        }

        public static string GetPCResult(int pcNo, int startPC, int electionId, int stateId)
        {
            return ScrapeIndiaVotes(String.Format("http://www.indiavotes.com/pc/detail/{0}/{1}/{2}", startPC + pcNo, stateId, electionId));
        }

        public static string GetACWiseResultsOfPC(int pcNo, int startPC, int electionId, int stateId)
        {
            return ScrapeIndiaVotes(String.Format("http://www.indiavotes.com/pc/acwisedetails/{0}/{1}/{2}", stateId, startPC + pcNo, electionId));
        }


        public static string ScrapeIndiaVotes(string scrapeUrl)
        {
            var request = (HttpWebRequest)WebRequest.Create(scrapeUrl);
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Host = "www.indiavotes.com";

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
