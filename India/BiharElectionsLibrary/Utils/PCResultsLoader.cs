using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiharElectionsLibrary
{
    public static class PCResultsLoader
    {
        public static List<ParliamentaryConstituencyResult> Load2015PCResults(string dirPath, State state)
        {
            var results = new List<ParliamentaryConstituencyResult>();
            for(int pcNo=1; pcNo<=40; pcNo++)
            {
                var result = new ParliamentaryConstituencyResult();
                string fileName = Path.Combine(dirPath,String.Format("{0}.tsv",pcNo));
                string[] lines = File.ReadAllLines(fileName);
                string[] headers = lines[0].Split('\t');
                result.Constituency = state.PCs.First(t => t.No == pcNo);
                var candidateVotes = new List<CandidateVotes>();
                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split('\t');
                    candidateVotes.Add(new CandidateVotes() { 
                            Votes = int.Parse(parts[2], NumberStyles.AllowThousands), 
                            Candidate = new Candidate { Name = parts[1], Party = Utils.GetParty(parts[4]) } 
                    });
                }
                result.Votes = candidateVotes;
                result.YearOfElection = 2015;
                results.Add(result);
            }
            return results;
        }
    }
}
