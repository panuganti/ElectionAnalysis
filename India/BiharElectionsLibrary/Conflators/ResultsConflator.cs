using System.Collections.Generic;
using System.Linq;

namespace BiharElectionsLibrary
{
    public class ResultsConflator
    {
        public static List<ACResult> Conflate2015Results(
            List<ACResult> ACResults, List<PCResult> PCResults,
            State state)
        {
            foreach (var result in ACResults)
            {
                var pc = PCResults.First(t => t.Constituency.No == state.ACs.First(ac => ac.No == result.Constituency.No).PC.No);
                var pcNamePartyDict = pc.Votes.ToDictionary(x=>Utils.GetNormalizedName(x.Candidate.Name), y=>y.Candidate.Party);
                foreach (var candVote in result.Votes)
                {
                    candVote.Candidate.Party = pcNamePartyDict[candVote.Candidate.Name];
                }
            }
            return ACResults;
        }
    }
}
