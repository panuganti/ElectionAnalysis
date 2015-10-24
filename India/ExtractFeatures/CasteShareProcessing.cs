using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiharElectionsLibrary;

namespace ExtractFeatures
{
    class CasteShareProcessing
    {
        public static void ProcessCasteShareData()
        {
            const string filename = @"I:\ArchishaData\ElectionData\Bihar\Predictions\casteShareParamsPurified.tsv";
            const string casteFeaturesFilename =
                @"I:\ArchishaData\ElectionData\Bihar\Predictions\casteShareFeatures.tsv";

            var casteShareData = File.ReadAllLines(filename).Skip(1).Select(x =>
            {
                var parts = x.Split('\t');
                return new {AcNo = parts[0], Caste = Category(parts[1]), Percent = double.Parse(parts[2]), Party = parts[3], PartyPercent = double.Parse(parts[4])};
            });
            // For duplicates, %weighting
            /*
            var groupedUpDdata =
                casteShareData.GroupBy(x => x.AcNo).SelectMany(x =>
                {
                    var groups = x.GroupBy(y => y.Caste);
                    return groups.Select(z => new {AcNo = x.Key, Caste = z.Key, Percent = z.Sum(p => p.Percent), Party = });
                });
             */
                // TODO:


            //File.WriteAllLines(casteFeaturesFilename, );

        }

        public static string Category(string caste)
        {
            switch (caste.ToLower())
            {
                case "yadav":
                case "dalit":
                case "muslim":
                case "obc":
                case "others":
                    return caste.ToLower();
                case "bhumihar":
                case "brahmin":
                case "rajput":
                case "baniya":
                case "thakur":
                case "kayastha":
                case "sahni":
                case "chaudhary":
                case "chauhan":
                    return "uch";
                case "kushwaha":
                case "sc":    
                case "bind":
                case "paswan":
                case "kahar":
                case "mallaha":
                case "mandal":
                case "st":
                    return "dalit";
                case "kurmi":
                case "dhanuk":
                case "nunia":
                case "ebc":
                    return "obc";    
                default:
                    throw new Exception();
            }
        }
    }
}
