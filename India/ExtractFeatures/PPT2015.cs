using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BiharElectionsLibrary;
using Newtonsoft.Json;

namespace ExtractFeatures
{
    class PPT2015
    {
        public static void Generate2015PredictionJson(string infile, string outfile)
        {
            var allResults = File.ReadAllLines(infile).Skip(1).Select(x =>
            {
                var parts = x.Split('\t');
                var result = new Result
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Votes =
                        new List<CandidateVote>
                        {
                            new CandidateVote()
                            {
                                Name = parts[2],
                                Party = (PoliticalParty) Enum.Parse(typeof (PoliticalParty), parts[4]),
                                Position = 1,
                                Votes = int.Parse(parts[3])
                            }
                        }
                };
                return result;
            });
            File.WriteAllText(outfile, JsonConvert.SerializeObject(allResults, Formatting.Indented));
        }
    }
}
