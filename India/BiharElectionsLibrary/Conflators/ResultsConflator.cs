using System;
using System.Collections.Generic;
using System.Linq;

namespace BiharElectionsLibrary
{
    public class ResultsConflator
    {
        public static List<Result> ConflateResults(List<ACResult> ACResults, State state)
        {
            var results = new List<Result>();
            foreach (var acresult in ACResults)
            {
                var pc = state.ACs.Where(t => Utils.LevenshteinDistance(Utils.GetNormalizedName(t.PC.Name), Utils.GetNormalizedName(acresult.Constituency.PC.Name)) < 2).ToArray();
                if (!pc.Any()) { throw new Exception("can't find ac"); }
                
                var ac = pc.OrderBy(t => Utils.LevenshteinDistance(t.Name,Utils.GetNormalizedName(acresult.Constituency.Name))).First();
                var result = new Result { 
                    Name = acresult.Constituency.Name, 
                    Id = ac.No, 
                    Votes = new List<CandidateVote>() 
                };
                int position = 0;
                result.Votes = acresult.Votes.OrderByDescending(t=>t.Votes).Select(t=> 
                    {
                        position++;
                        return new CandidateVote
                        {
                            Name = t.Candidate.Name,
                            Party = t.Candidate.Party,
                            Votes = t.Votes,
                            Position = position
                        };
                    }).ToList();
                results.Add(result);
            }
            return results;
        }

        public static List<Result> ConflateResultsAndDistrictInfo(List<ACResult> acResults, State state)
        {
            var results = new List<Result>();
            foreach (var acresult in acResults)
            {
                var possibleACs = state.ACs.Where(t => Utils.LevenshteinDistance(Utils.GetNormalizedName(t.District.Name), Utils.GetNormalizedName(acresult.Constituency.District.Name)) < 3).ToArray();
                if (!possibleACs.Any()) { throw new Exception("can't find ac"); }

                var ac = possibleACs.OrderBy(t => Utils.LevenshteinDistance(Utils.GetNormalizedName(t.Name), Utils.GetNormalizedName(acresult.Constituency.Name))).First();

                var result = new Result
                {
                    Name = acresult.Constituency.Name,
                    Id = ac.No,
                    Votes = new List<CandidateVote>()
                };
                result.Votes = acresult.Votes.Select(t =>
                    new CandidateVote
                    {
                        Name = t.Candidate.Name,
                        Party = t.Candidate.Party,
                        Votes = t.Votes,
                        Position = t.Position
                    }).ToList();
                results.Add(result);
            }
            return results;
        }
    }
}