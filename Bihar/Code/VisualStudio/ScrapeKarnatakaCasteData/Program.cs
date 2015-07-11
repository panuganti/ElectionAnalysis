using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ScrapeKarnatakaCasteData
{
    class Program
    {
        static void Main(string[] args)
        {
            var allLines = File.ReadAllLines("./Config.ini");
            string state = "29";
            ScrapeData(state, allLines[0], int.Parse(allLines[1]), int.Parse(allLines[2]));
        }

        public static void ScrapeData(string state, string outputPath, int startDist, int endDist)
        {
//            var districts = HtmlUtilities.GetDistricts(state);
            DateTime start = DateTime.Now;
            for (int distId = startDist; distId <= endDist; distId++)
            {
                string dist = distId.ToString();
                var tehsils = HtmlUtilities.GetTehsilList(state, dist);
                if (tehsils == null) { continue; }
                foreach (var tehsil in tehsils)
                {
                    var towns = HtmlUtilities.GetTownList(state, dist, tehsil);
                    if (towns == null) { continue; }
                    foreach (var town in towns)
                    {
                        var wards = HtmlUtilities.GetWardList(state, dist, tehsil, town);
                        if (wards == null) { continue; }
                        foreach (var ward in wards)
                        {
                            var blocks = HtmlUtilities.GetBlockList(state, dist, tehsil, town, ward);
                            if (blocks == null) { continue; }
                            foreach (var block in blocks)
                            {
                                var houses = HtmlUtilities.GetHouseholds(state, dist, tehsil, town, ward, block);
                                if (houses == null) { continue; }
                                foreach (var kvp in houses)
                                {
                                    Console.WriteLine("District: {0}\tTehsil: {1}\tTown: {2}\tWard: {3}\tBlock: {4}\tHouse: {5}", dist, tehsil, town, ward, block, kvp.Key);
                                    var filename = GetFilename(outputPath, state, dist, tehsil, town, ward, block, kvp.Key);
                                    if (File.Exists(filename)) { continue; }
                                    var html = HtmlUtilities.GetHouseInfo(kvp.Value);
                                    if (html == null) { continue; }
                                    SaveFile(html, filename);
                                    Console.WriteLine("Time Elapsed: {0}\n", (DateTime.Now - start).TotalMilliseconds);
                                    start = DateTime.Now;
                                }
                            }
                        }
                    }
                }
            }

        }

        public static string GetFilename(string outputPath, string state, string dist, string tehsil, string town, string ward, string block, string house)
        {
            return Path.Combine(outputPath, String.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}.html", state, dist, tehsil, town, ward, block, house));
        }

        public static void SaveFile(string html, string filename)
        {
            File.WriteAllText(filename, html);
        }
    }

}
