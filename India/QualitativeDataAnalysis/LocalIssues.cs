using System;
using System.Collections.Generic;
using System.Linq;
using BiharElectionsLibrary;

namespace QualitativeDataAnalysis
{
    public class LocalIssues
    {
        public int AssemblyId { get; set; }
        public string AssemblyName { get; set; }
        public string ParliamentaryName { get; set; }
        public Dictionary<LocalIssuesCategory, int> LocalIssuesParameterScores { get; set; }

        public static List<LocalIssues> BuildLocalIssueRatings(string[] localIssuesData)
        {
            var localIssuesLookups = new List<LocalIssues>();

            var localIssuesDataForAnAssembly = new List<string> { localIssuesData[1] };
            string currentAssemblyId = localIssuesData[1].Split('\t').First();

            for (var i = 2; i < localIssuesData.Length; i++)
            {
                if (currentAssemblyId != localIssuesData[i].Split('\t').First())
                {
                    localIssuesLookups.Add(BuildLocalIssuesForAnAssembly(localIssuesDataForAnAssembly));
                    localIssuesDataForAnAssembly = new List<string>();
                }
                localIssuesDataForAnAssembly.Add(localIssuesData[i]);
            }
            return localIssuesLookups;
        }

        public static LocalIssues BuildLocalIssuesForAnAssembly(IEnumerable<string> localIssuesDataForAnAssembly)
        {
            var first = true;
            LocalIssues localIssues = null;
            foreach (string[] columns in localIssuesDataForAnAssembly.Select(entry => entry.Split('\t')))
            {
                if (first)
                {
                    first = false;
                    localIssues = new LocalIssues
                    {
                        AssemblyId = Int32.Parse(columns[0]),
                        AssemblyName = columns[1],
                        ParliamentaryName = columns[2],
                        LocalIssuesParameterScores = new Dictionary<LocalIssuesCategory, int>()
                    };
                }
                if (columns[4].Equals(String.Empty))
                {
                    //Console.WriteLine("Empty LocalIssues Value: {0}", String.Join("\t", columns));
                }
                var localIssueCategory = Utils.GetLocalIssuesCategory(columns[3]);

                if (localIssueCategory == LocalIssuesCategory.Error ||
                    localIssues.LocalIssuesParameterScores.ContainsKey(localIssueCategory))
                {
                    continue;
                }

                localIssues.LocalIssuesParameterScores.Add(Utils.GetLocalIssuesCategory(columns[3]),
                    columns[4].Equals(String.Empty) ? 0 : Int32.Parse(columns[4]));
            }
            return localIssues;
        }
    }
}
