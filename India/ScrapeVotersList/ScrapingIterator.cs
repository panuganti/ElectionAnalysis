using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Threading;

namespace ScrapeVotersList
{
    public class ScrapingIterator
    {
        private StreamWriter _logStream;
        string _outputPath;
        int _intervalBetweenRequests;
        List<int> _districtIdsToScrape;
        string[] _bodyPatterns;

        public ScrapingIterator(StreamWriter logStream, List<int> districtIdsToScrape, string[] bodyPatterns, string outputPath, int intervalBetweenRequests)
        {
            _logStream = logStream;
            _districtIdsToScrape = districtIdsToScrape;
            _bodyPatterns = bodyPatterns;
            _outputPath = outputPath;
            _intervalBetweenRequests = intervalBetweenRequests;
        }

        public void StartScraping()
        {
            // Algo: Read from config which districts (from & to) to scrape
            // 1. For A to Z, check if the file already exists, 
            // 2. If not, Check if the Ab kind of files exist
            // 3. If not, try to scrape A A ... 
            // 4. If response is null, try scrape Aa A...


            foreach (int districtId in _districtIdsToScrape)
            {
                for (char firstNameFirstAlphabet = 'A'; firstNameFirstAlphabet <= 'Z'; firstNameFirstAlphabet++)
                {
                    for (char relativeNameFirstAlphabet = 'A'; relativeNameFirstAlphabet <= 'Z'; relativeNameFirstAlphabet++)
                    {
                        Console.WriteLine("Scraping DistrictId:{0}, FirstName:{1}, RelFirstName:{2}", districtId, firstNameFirstAlphabet, relativeNameFirstAlphabet);
                        _logStream.WriteLine("Scraping DistrictId:{0}, FirstName:{1}, RelFirstName:{2}", districtId, firstNameFirstAlphabet, relativeNameFirstAlphabet);
                        AttempStatus status = CheckIfScrapeWasAttempted(districtId, firstNameFirstAlphabet, relativeNameFirstAlphabet, _outputPath);

                        if (status == AttempStatus.Success || status == AttempStatus.SubFilesSuccess) 
                        {
                            _logStream.WriteLine("Already Scraped: DistrictId:{0}, FirstName:{1}, RelFirstName:{2}", districtId, firstNameFirstAlphabet, relativeNameFirstAlphabet);
                            continue; 
                        } // Continue if reqd files already exist

                        DateTime startTime = DateTime.Now;
                        if (status == AttempStatus.NeverAttempted)
                        {
                            string responseFromServer = HtmlUtilities.QueryServer(districtId, firstNameFirstAlphabet.ToString(), relativeNameFirstAlphabet.ToString(), _bodyPatterns[districtId - 1]);
                            if (responseFromServer != null)
                            {
                                string outputFileName = String.Format("VotersList_{0}_{1}_{2}.tsv", districtId, firstNameFirstAlphabet, relativeNameFirstAlphabet);
                                HtmlUtilities.ParseAndWriteToFile(responseFromServer, Path.Combine(_outputPath, outputFileName));
                                DateTime endTime = DateTime.Now;
                                _logStream.WriteLine("TimeElapsed: {0}", (endTime - startTime).TotalSeconds); _logStream.Flush();
                                Thread.Sleep(_intervalBetweenRequests);
                                continue; // Success, let's move on to next iteration
                            }
                        }

                        // Attempt the subfiles
                        IterateAlongSecondAlphabet(_outputPath, districtId, firstNameFirstAlphabet, relativeNameFirstAlphabet);
                        DateTime endTimeSecondAlphabet = DateTime.Now;
                        _logStream.WriteLine("TimeElapsed: {0}", (endTimeSecondAlphabet - startTime).TotalSeconds); _logStream.Flush();
                    }
                }
            }
        }

        private void IterateAlongSecondAlphabet(string outputPath, int districtId, char firstNameFirstAlphabet, char relativeNameFirstAlphabet)
        {
            for (char secondAlphabet = 'a'; secondAlphabet <= 'z'; secondAlphabet++)
            {
                string outputFileName = String.Format("VotersList_{0}_{1}{3}_{2}.tsv", districtId, firstNameFirstAlphabet, 
                    relativeNameFirstAlphabet, secondAlphabet);
                
                if (File.Exists(Path.Combine(outputPath, outputFileName))) { continue;  }

                string responseFromServer = HtmlUtilities.QueryServer(districtId, 
                    String.Format("{0}{1}", firstNameFirstAlphabet, secondAlphabet), relativeNameFirstAlphabet.ToString(), _bodyPatterns[districtId-1]);

                if (responseFromServer == null) 
                {
                    _logStream.WriteLine("Failed: Scraping DistrictId:{0}, FirstName:{1}{2} RelFirstName:{3}", districtId, firstNameFirstAlphabet, secondAlphabet, relativeNameFirstAlphabet);
                    _logStream.WriteLine("Continuing..."); _logStream.Flush();
                }
                HtmlUtilities.ParseAndWriteToFile(responseFromServer, Path.Combine(_outputPath, outputFileName));
                Thread.Sleep(_intervalBetweenRequests);
            }
        }

        private AttempStatus CheckIfScrapeWasAttempted(int districtId, char firstNameFirstAlphabet, char relativeNameFirstAlphabet, string outputPath)
        {
            string outputFileName = String.Format("VotersList_{0}_{1}_{2}.tsv", districtId, firstNameFirstAlphabet, relativeNameFirstAlphabet);
            if (File.Exists(Path.Combine(outputPath, outputFileName)))
            {
                return AttempStatus.Success;
            }
            char secondAlphabet = 'a';
            outputFileName = String.Format("VotersList_{0}_{1}{3}_{2}.tsv", districtId, firstNameFirstAlphabet, relativeNameFirstAlphabet, secondAlphabet);
            if (!(File.Exists(Path.Combine(outputPath, outputFileName))))
            {
                return AttempStatus.NeverAttempted;
            }
            for (secondAlphabet = 'b'; secondAlphabet <= 'z'; secondAlphabet++)
            {
                outputFileName = String.Format("VotersList_{0}_{1}{3}_{2}.tsv", districtId, firstNameFirstAlphabet, relativeNameFirstAlphabet, secondAlphabet);
                if (!(File.Exists(Path.Combine(outputPath, outputFileName))))
                {
                    _logStream.WriteLine("Attempted but not all subfiles present: Scraping DistrictId:{0}, FirstName:{1}{2} RelFirstName:{3}", districtId, 
                        firstNameFirstAlphabet, secondAlphabet, relativeNameFirstAlphabet);
                    return AttempStatus.SubFileAttempted;
                }
            }
            return AttempStatus.SubFilesSuccess;
        }

        public enum AttempStatus
        {
            Success,
            SubFileAttempted,
            SubFilesSuccess,
            NeverAttempted
        }
    }
}
