using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace ScrapePollingBooths
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int acNo = 2; acNo <= 2; acNo++)
            {
                var output = HtmlUtilities.GetPostResponse(acNo);
                var booths = HtmlUtilities.ParseResponse(output);
                WriteToFile(booths.options, String.Format("./Booths_{0}.txt", acNo));
                Thread.Sleep(1000);
            }
        }

        public static void WriteToFile(Dictionary<int,string> Booths, string filename)
        {
            File.WriteAllText(filename, String.Join("\n", Booths.Select(x => String.Format("{0}\t{1}", x.Key, x.Value))));
        }
    }
}
