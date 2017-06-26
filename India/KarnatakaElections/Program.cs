using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnatakaElections
{
    class Program
    {
        static void Main(string[] args)
        {
            string infile = @"C:\Projects\GitHub\ElectionDataAnalytics\src\assets\data\predelimitation.karnataka.geo.json";
            string infile2 = @"C:\Projects\GitHub\ElectionDataAnalytics\src\assets\data\karnataka.assembly.geo.json";
            string outfile = @".\outfile.txt";
            string outfile2 = @".\outfile2.txt";
            //GeoJsonUtils.PrintIdName(infile, outfile);
            //GeoJson.GeoJsonUtils.PrintIdName(infile2, outfile2);

            var files = Directory.GetFiles(@"J:\ArchishaData\ElectionData\Karnataka\EC\Form20_2008_csv\").ToList();
            string output = @"J:\ArchishaData\ElectionData\Karnataka\EC\Form20_2008_Cleanedup\";
            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }
            files.ToList().ForEach(input => Form20.ParseAndCleanupForm20CSV(input, output));
            Console.WriteLine("Done");
            Console.ReadKey();
        }

    }
}
