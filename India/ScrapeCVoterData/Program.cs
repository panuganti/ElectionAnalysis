using System;
using System.IO;
using HtmlAgilityPack;

namespace ScrapeCVoterData
{
    class Program
    {
        static void Main(string[] args)
        {
            //ScrapeCVoterQualitativeData();
            ParseQualitativeData();
        }

        static void ParseQualitativeData()
        {
            string inPath = @"I:\ArchishaData\ElectionData\RawData\CVoter\QualitativeData";
            string outPath = @"I:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative";
            var doc = new HtmlWeb();
            for (int i = 1; i <= 243; i++)
            {
                var inFile = Path.Combine(inPath, String.Format("{0}.html"));
                doc.Load(inFile);

            }
        }

        static void ScrapeCVoterQualitativeData()
        {
            string outPath = @"D:\ArchishaData\ElectionData\RawData\CVoter\QualitativeData";
            for (int i = 1; i <= 243; i++)
            {
                var response = new HtmlUtils.HttpRequests().GetPostResponse(i);
                File.WriteAllText(Path.Combine(outPath, String.Format("{0}.html", i)), response);
            }
        }
    }
}
