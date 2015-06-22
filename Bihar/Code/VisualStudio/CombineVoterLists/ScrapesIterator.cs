using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CombineVoterLists
{
    public class ScrapesIterator
    {
        // LogFile format: {RelFirstName}_{FirstName}
        StreamWriter _logFile;
        string _outputPath;
        List<string> _parsedFirstNamesCombination = new List<string>();
        int _districtId;
        StreamWriter _combinedScrapesFile;

        public ScrapesIterator(StreamWriter logfile, string outputPath, int districtId, List<string> alreadyParsedCombinations, StreamWriter combinedScrapesFile)
        {
            _logFile = logfile;
            _outputPath = outputPath;
            _parsedFirstNamesCombination = alreadyParsedCombinations;
            _districtId = districtId;
            _combinedScrapesFile = combinedScrapesFile;
        }

        public void IterateThroughScrapes()
        {
            for(char firstNameFirstAlphabet = 'A'; firstNameFirstAlphabet <= 'Z'; firstNameFirstAlphabet++)
            {
                for(char relFirstNameFirstAlphabet = 'A'; relFirstNameFirstAlphabet <= 'Z'; relFirstNameFirstAlphabet++)
                {
                    string scrapeFileName = String.Format("VotersList_{0}_{1}_{2}.tsv", _districtId, firstNameFirstAlphabet, relFirstNameFirstAlphabet);
                    if (_parsedFirstNamesCombination.Contains(String.Format("{0}_{1}", firstNameFirstAlphabet, relFirstNameFirstAlphabet))) {continue;}
                    if (File.Exists(Path.Combine(_outputPath,scrapeFileName)))
                    {
                        // Combine file with the combined file
                        AppendScrapeToCombinedScrapeFile(Path.Combine(_outputPath, scrapeFileName));
                        Console.WriteLine("{0}_{1}", relFirstNameFirstAlphabet, firstNameFirstAlphabet);
                        _logFile.WriteLine("{0}_{1}", relFirstNameFirstAlphabet, firstNameFirstAlphabet); _logFile.Flush();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine(String.Format("File:{0} does not exist", scrapeFileName));
                    }
                    IterateThroughSecondAlphabetScrapes(firstNameFirstAlphabet, relFirstNameFirstAlphabet);
                }
            }
        }

        public void IterateThroughSecondAlphabetScrapes(char firstNameFirstAlphabet, char relFirstNameFirstAlphabet)
        {
            for (char firstNameSecondAlphabet = 'a'; firstNameSecondAlphabet <= 'z'; firstNameSecondAlphabet++)
            {
                string scrapeFileName = String.Format("VotersList_{0}_{1}{3}_{2}.tsv", _districtId, firstNameFirstAlphabet, relFirstNameFirstAlphabet, firstNameSecondAlphabet);
                if (File.Exists(Path.Combine(_outputPath, scrapeFileName)))
                {
                    // Combine file with the combined file
                    AppendScrapeToCombinedScrapeFile(Path.Combine(_outputPath, scrapeFileName));
                    _logFile.WriteLine("{0}_{1}{2}", relFirstNameFirstAlphabet, firstNameFirstAlphabet, firstNameSecondAlphabet); _logFile.Flush();
                    continue;
                }
                else
                {
                    Console.WriteLine(String.Format("File:{0} does not exist", scrapeFileName));
                }
            }
        }

        public void AppendScrapeToCombinedScrapeFile(string fileToAppend)
        {
            Regex sexAgeRegex = new Regex(@"^([MF])\((\d+)\)$");

            // If file is empty, do nothing
            if (new FileInfo(fileToAppend).Length == 0) { return; }

            // Read and parse current scrapes file
            using (StreamReader reader = new StreamReader(fileToAppend))
            {
                string[] lines = reader.ReadToEnd().Split('\n');
                for (int i = 1; i < lines.Length; i++) // Skip the first line which is header
                {
                    if (lines[i].Equals(String.Empty)) { continue; } // Last line might be empty

                    string[] columns = lines[i].Split('\t');
                    if (columns.Length < 12) { throw new Exception(String.Format("No. of columns:{0} < 12", columns.Length)); }

                    string voterId = columns.Length == 13 ? columns[9] + columns[10] : columns[9];
                    string status = columns.Length == 13 ? columns[11] : columns[10];
                    string photo = columns.Length == 13 ? columns[12] : columns[11];
                    if (columns.Length == 13)
                    {
                        voterId += columns[11];
                        photo = columns[12];
                    }


                    Match match = sexAgeRegex.Match(columns[6].Trim());
                    string sex = match.Groups[1].Value;
                    string age = match.Groups[2].Value;
                    string lineToAppend = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}",
                        columns[0].Trim(), columns[1].Trim(), columns[2].Trim(), columns[3].Trim(), columns[4].Trim(), columns[5].Trim(), 
                        sex, age, columns[7].Trim(), columns[8].Trim(), voterId.Trim(), status.Trim(), photo.Trim());

                    _combinedScrapesFile.WriteLine(lineToAppend);
                }
                _combinedScrapesFile.Flush();
            }
        }
    }
}
