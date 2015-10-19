using System;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace ReadHtmlQuantitativeData
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseHtmlFiles();
        }

        private static void ParseHtmlFiles()
        {
            string inPath = @"I:\ArchishaData\ElectionData\RawData\CVoter\QualitativeData";
            string outPath = @"I:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative";
            for (int i = 1; i <= 243; i++)
            {
                var inFile = Path.Combine(inPath, String.Format("{0}.html",i));
                //var parsedData = ParseHtmlData.ParseHtml(inFile);
                Console.ReadKey();
            }             
        }

        private static void ParseManuallyStoredHtmlFiles()
        {
            const string filesPath = @"I:\ArchishaData\ElectionData\RawData\CVoter\QualitativeData";
            var htmlFiles = Directory.GetFiles(filesPath).Where(t => t.EndsWith(".html"));

            var dict = htmlFiles.ToDictionary(Path.GetFileNameWithoutExtension, ParseHtmlData.ParseHtml);
        }
    }
}
