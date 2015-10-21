using System.IO;
using System.Linq;

namespace ReadHtmlQuantitativeData
{
    class Program
    {
        static void Main(string[] args)
        {
            const string filesPath = @"E:\NMW\SurveyAnalytics\Bihar\CVoterData\2015QualitativeData";
            var htmlFiles = Directory.GetFiles(filesPath).Where(t=>t.EndsWith(".html"));

            var dict = htmlFiles.ToDictionary(Path.GetFileNameWithoutExtension, ParseHtmlData.ParseHtml);


        }
    }
}
