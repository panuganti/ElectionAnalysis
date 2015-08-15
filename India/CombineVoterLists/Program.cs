using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombineVoterLists
{
    class Program
    {        
        static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader("./Config.ini"))
            {
                int districtId = Int32.Parse(reader.ReadLine());
                string outputPath = reader.ReadLine();
                string combinedVotersList = String.Format("VotersList_District_{0}.txt", districtId);
                string logFileName = String.Format("ScrapesCombinerForDistrict_{0}.log", districtId);

                List<string> alreadyParsedCombinations = new List<string>();
                if (File.Exists(Path.Combine(outputPath, logFileName)))
                {
                    StreamReader logFileReader = new StreamReader(Path.Combine(outputPath, logFileName));
                    alreadyParsedCombinations.AddRange(logFileReader.ReadToEnd().Split('\n'));
                    logFileReader.Close();
                }

                StreamWriter combinedScrapesFile = File.AppendText(Path.Combine(outputPath,combinedVotersList));
                StreamWriter logFile = File.AppendText(Path.Combine(outputPath,logFileName));

                ScrapesIterator scrapesIterator = new ScrapesIterator(logFile, outputPath, districtId, alreadyParsedCombinations, combinedScrapesFile);
                scrapesIterator.IterateThroughScrapes();

                combinedScrapesFile.Close();
                logFile.Close();
            }
        }
    }
}
