using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapeCVoterData
{
    class Program
    {
        static void Main(string[] args)
        {
            string outPath = @"D:\ArchishaData\ElectionData\RawData\CVoter\QualitativeData";
            for (int i=1; i<=243; i++)
            {
                var response = new HtmlUtils.HttpRequests().GetPostResponse(i);
                File.WriteAllText(Path.Combine(outPath, String.Format("{0}.html",i)), response);
            }
        }
    }
}
