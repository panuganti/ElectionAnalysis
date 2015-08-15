using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

namespace ScrapeVotersList
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader configReader = new StreamReader("./Config.ini");
            List<int> districtIdsToScrape = configReader.ReadLine().Split(',').Select(t => Int32.Parse(t)).ToList();
            string outputPath = configReader.ReadLine();
            int intervalBetweenRequests = Int32.Parse(configReader.ReadLine());
            configReader.Close();

            StreamReader bodyPatternsReader = new StreamReader(Path.Combine(outputPath,"BodyConfig.ini"));
            string[] bodyPatterns = bodyPatternsReader.ReadToEnd().Split('\n');
            bodyPatternsReader.Close();

            StreamWriter logStream = new StreamWriter(Path.Combine(outputPath,String.Format("{0}.log",String.Join("_",districtIdsToScrape))));
            ScrapingIterator scrapingIterator = new ScrapingIterator(logStream, districtIdsToScrape, bodyPatterns, outputPath, intervalBetweenRequests);
            scrapingIterator.StartScraping();
            logStream.Close();
        }
    }
}
