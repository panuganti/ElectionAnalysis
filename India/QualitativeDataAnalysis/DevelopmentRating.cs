using System;
using System.Collections.Generic;
using System.Linq;

namespace QualitativeDataAnalysis
{
    public class DevelopmentRating
    {
        public string District { get; set; }
        public int AssemblyId { get; set; }
        public string AssemblyName { get; set; }
        public string ParliamentaryName { get; set; }
        public Dictionary<string, int> DevelopmentParameterScores { get; set; }

        public static List<DevelopmentRating> BuildDevelopmentRatingsList(string[] developmentData)
        {
            var developmentRatings = new List<DevelopmentRating>();

            var developmentRatingDataForAnAssembly = new List<string> { developmentData[1] };
            string currentAssemblyId = developmentData[1].Split('\t')[1];

            for (var i = 2; i < developmentData.Length; i++)
            {
                if (currentAssemblyId != developmentData[i].Split('\t')[1])
                {
                    developmentRatings.Add(BuildDevelopmentRatingForAnAssembly(developmentRatingDataForAnAssembly));
                    developmentRatingDataForAnAssembly = new List<string>();
                }
                developmentRatingDataForAnAssembly.Add(developmentData[i]);
            }
            return developmentRatings;
        }

        public static DevelopmentRating BuildDevelopmentRatingForAnAssembly(IEnumerable<string> developmentRatingDataForAnAssembly)
        {
            var first = true;
            DevelopmentRating developmentRating = null;
            foreach (string[] columns in developmentRatingDataForAnAssembly.Select(entry => entry.Split('\t')))
            {
                if (first)
                {
                    first = false;
                    developmentRating = new DevelopmentRating
                    {
                        District = columns[0],
                        AssemblyId = Int32.Parse(columns[1]),
                        AssemblyName = columns[2],
                        ParliamentaryName = columns[3],
                        DevelopmentParameterScores = new Dictionary<string, int>()
                    };
                }
                // Ignore the 2nd entry for Communal polarization
                if (developmentRating.DevelopmentParameterScores.ContainsKey(columns[4])) { continue; }

                if (columns[5].Equals(String.Empty))
                {
                    //Console.WriteLine("Empty DevelopmentData Value: {0}", String.Join("\t", columns));
                }

                developmentRating.DevelopmentParameterScores.Add(columns[4],
                    columns[5].Equals(String.Empty) ? 0 : Int32.Parse(columns[5]));
            }
            return developmentRating;
        }
    }
}
