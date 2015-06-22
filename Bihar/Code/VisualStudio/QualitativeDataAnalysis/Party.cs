using System;
using System.Collections.Generic;
using System.Linq;
using BiharElectionsLibrary;

namespace QualitativeDataAnalysis
{
    public class Party
    {
        public int AssemblyId { get; set; }
        public string AssemblyName { get; set; }
        public string ParliamentaryName { get; set; }
        public Dictionary<PoliticalParty, Dictionary<string, double>> PartyParameterScores { get; set; }

        public static List<Party> BuildPartyInfoLookups(string[] partyData)
        {
            var partyInfoLookups = new List<Party>();

            var partyDataForAnAssembly = new List<string> { partyData[1] };
            string[] columns = partyData[1].Split('\t');
            string currentParty = columns[4].ToLower();
            string currentCandidateAssemblyId = columns[0];

            for (var i = 2; i < partyData.Length; i++)
            {
                columns = partyData[i].Split('\t');
                if (currentParty != columns[4] || currentCandidateAssemblyId != columns[0])
                {
                    partyInfoLookups.Add(BuildPartyForAnAssembly(partyDataForAnAssembly));
                    partyDataForAnAssembly = new List<string>();
                }
                partyDataForAnAssembly.Add(partyData[i]);
            }
            return partyInfoLookups;
        }

        public static Party BuildPartyForAnAssembly(IEnumerable<string> partyDataForAnAssembly)
        {
            var first = true;
            Party party = null;
            foreach (string[] columns in partyDataForAnAssembly.Select(entry => entry.Split('\t')))
            {
                if (first)
                {
                    first = false;
                    party = new Party
                    {
                        AssemblyId = Int32.Parse(columns[0]),
                        AssemblyName = columns[2],
                        ParliamentaryName = columns[3],
                        PartyParameterScores = new Dictionary<PoliticalParty, Dictionary<string, double>>()
                    };
                }
                if (columns[6].Equals(String.Empty))
                {
                    //Console.WriteLine("Empty PartyData Value: {0}", String.Join("\t", columns));
                }
                PoliticalParty politicalParty = Utils.GetParty(columns[4]);
                if (politicalParty == PoliticalParty.Error)
                {
                    continue;
                }
                if (!party.PartyParameterScores.ContainsKey(politicalParty))
                {
                    party.PartyParameterScores.Add(politicalParty, new Dictionary<string, double>());
                }

                if (party.PartyParameterScores[politicalParty].ContainsKey(columns[5])) { continue; }

                party.PartyParameterScores[politicalParty].Add(columns[5],
                    columns[6].Equals(String.Empty) ? 0 : Int32.Parse(columns[6]));
            }
            return party;
        }
    }
}
