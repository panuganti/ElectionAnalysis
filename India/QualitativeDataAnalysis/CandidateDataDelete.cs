using System;
using System.Collections.Generic;
using System.Linq;
using BiharElectionsLibrary;

namespace QualitativeDataAnalysis
{
    public class CandidateDataDelete
    {
        public string District { get; set; }
        public int AssemblyId { get; set; }
        public string AssemblyName { get; set; }
        public string ParliamentaryName { get; set; }
        public string CandidateName { get; set; }
        public PoliticalParty Party { get; set; }
        public string CandidateCaste { get; set; }
        public Dictionary<string,int> CandidateParameterScores { get; set; }
        public bool IsWon { get; set; }

        public static List<CandidateDataDelete> BuildCandidatesList(string[] candidatesData)
        {
            var candidates = new List<CandidateDataDelete>();
            
            var candidateDataForAnAssembly = new List<string> {candidatesData[1]};
            var columns = candidatesData[1].Split('\t');
            var currentCandidateName = columns[4];
            var currentCandidateParty = columns[5];
            var currentCandidateAssemblyId = columns[1];

            for (var i = 2; i < candidatesData.Length; i++)
            {
                columns = candidatesData[i].Split('\t');
                if (currentCandidateName != columns[4] || currentCandidateParty != columns[5] ||
                    currentCandidateAssemblyId != columns[1])
                {
                    candidates.Add(BuildSingleCandidate(candidateDataForAnAssembly));
                    candidateDataForAnAssembly = new List<string>();
                }
                candidateDataForAnAssembly.Add(candidatesData[i]);
            }
            return candidates;
        }

        public static CandidateDataDelete BuildSingleCandidate(IEnumerable<string> candidateDataForAnAssembly)
        {
            bool first = true;
            CandidateDataDelete candidateDataDelete = null;
            var wrongCandidateNames = new List<string> {"new", "new seat", "new face"};
            foreach (string[] columns in candidateDataForAnAssembly.Select(entry => entry.Split('\t')))
            {
                if (wrongCandidateNames.Contains(columns[4].Trim().ToLower())            )
                {
                    return null;
                }
                if (first)
                {
                    first = false;
                    candidateDataDelete = new CandidateDataDelete
                    {
                        District = columns[0],
                        AssemblyId = Int32.Parse(columns[1]),
                        AssemblyName = columns[2],
                        ParliamentaryName = columns[3],
                        CandidateName = columns[4],
                        Party = Utils.GetParty(columns[5]),
                        CandidateCaste = columns[6],
                        IsWon = columns[9] == "1",
                        CandidateParameterScores = new Dictionary<string, int>()
                    };
                }
                if (columns[8].Equals(String.Empty))
                {
                    // Console.WriteLine("Empty CandidateData Value: {0}", String.Join("\t", columns));
                }

                if (candidateDataDelete.CandidateParameterScores.ContainsKey(columns[7])) {continue;}

                candidateDataDelete.CandidateParameterScores.Add(columns[7],
                    columns[8].Equals(String.Empty) ? 0 : Int32.Parse(columns[8]));
            }
            return candidateDataDelete;
        }

    }
}
