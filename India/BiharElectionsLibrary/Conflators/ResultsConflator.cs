using System;
using System.Collections.Generic;
using System.Linq;

namespace BiharElectionsLibrary
{
    public class ResultsConflator
    {
        public static List<Result> Conflate2014Results(List<ACResult> ACResults, State state)
        {
            var results = new List<Result>();
            var pcNames = String.Join(",", state.PCs.Select(t => t.Name).Distinct().ToArray());
            Console.WriteLine(pcNames);
            Console.WriteLine(pcNames.Distinct().Count());
            foreach (var acresult in ACResults)
            {
                var pc = state.ACs.Where(t => Utils.LevenshteinDistance(t.PC.Name, Utils.GetNormalizedName(acresult.Constituency.PC.Name)) < 2);
                if (!pc.Any()) { throw new Exception("can't find ac"); }
                
                var ac = pc.OrderBy(t => Utils.LevenshteinDistance(t.Name,Utils.GetNormalizedName(acresult.Constituency.Name))).First();

                var result = new Result { 
                    Name = acresult.Constituency.Name, 
                    Id = ac.No, 
                    Votes = new List<CandidateVote>() 
                };
                result.Votes = acresult.Votes.Select(t=> 
                    new CandidateVote { 
                        Name = t.Candidate.Name, 
                        Party = t.Candidate.Party,
                        Votes = t.Votes
                }).ToList();
                results.Add(result);
            }
            return results;
        }
    }
}