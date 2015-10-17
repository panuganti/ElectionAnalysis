﻿using System;
using System.Net;
using System.IO.Compression;
using System.IO;

namespace ScrapeIndiaVotes
{
    class Program
    {
        static void Main(string[] args)
        {
            const int startPC = 33590;
            const int electionId = 228;
            const int stateId = 58;
            const string dirPath = @"D:\ArchishaData\ElectionData\RawData\IndiaVotes\2005BiharAC";
            const string outDir = @"D:\ArchishaData\ElectionData\Bihar\Results\2005ACWise\";

            for(int pcNo = 1; pcNo<=243; pcNo++)
            {
                string filename = String.Format(@"{0}\\{1}_{2}_{3}.html", dirPath, electionId, stateId, pcNo);
                ParseACWiseIndiaVotesPage.ParseAcResultsPage(filename, Path.Combine(outDir, "Parsed"));
                //ParseACWiseIndiaVotesPage.ParseAcWiseResultsPage(filename, outDir, 2009);
                
                //var response = GetACResult(stateId, electionId, pcNo, startPC);
                //File.WriteAllText(filename,response);                 
            }
        }

        public static string GetACResult(int stateId, int electionId, int acNo, int startAC)
        {
            return ScrapeIndiaVotes(String.Format("http://www.indiavotes.com/ac/details/{0}/{1}/{2}", stateId, startAC + acNo, electionId));            
        }

        public static string GetPCResult( int stateId, int electionId, int pcNo, int startPC)
        {
            return ScrapeIndiaVotes(String.Format("http://www.indiavotes.com/pc/detail/{0}/{1}/{2}", startPC + pcNo, stateId, electionId));
        }

        public static string GetACWiseResultsOfPC(int stateId, int electionId, int pcNo, int startPC )
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

                var reader = new StreamReader(responseStream);
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
