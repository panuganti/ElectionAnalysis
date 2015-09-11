using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BiharElectionsLibrary;
using CVoterContracts;
using QualitativeData = CVoterContracts.QualitativeData;
using Utils = BiharElectionsLibrary.Utils;

namespace CVoterLibrary
{
    public class CandidateSelector
    {
        public CandidateSelector(QualitativeData data)
        {
            _data = data;
        }

        public List<CandidateSelection> WinnableCandidates()
        {
            var bestCandidates = new List<CandidateSelection>();

            var interestedParties = new List<string> {"bjp", "ljp", "rlsp", "ham"};
            // Get candidates with highest winnability
            var validAcData = _data.AcQualitatives.Where(t => t.CandidateRatings.Any());
            foreach (var acQualitative in validAcData)
            {
                // Get bjp, ljp, rlsp, ham candidates
                var candidates =
                    acQualitative.CandidateRatings.Where(
                        t => interestedParties.Contains(t.PartyName.ToLower()) &&
                            t.Ratings.Any(r => r.Feature.ToLower().Equals("winability"))).ToArray();
                if (candidates.Length == 0)
                {
                    continue;
                }
                var bestCandidate =
                    candidates.OrderByDescending(
                        r => r.Ratings.First(t => t.Feature.ToLower().Equals("winability")).Score).First();
                bestCandidates.Add(new CandidateSelection() {AC = new AssemblyConstituency(){Name = acQualitative.Name, No = acQualitative.No}, BestCandidate = bestCandidate, CandidatesConsidered = candidates});
            }
            return bestCandidates;
        }

        public void PrintCandidateSelection(List<CandidateSelection> bestCandidates, State state,
            string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (var writer = new StreamWriter(filename))
            {
                writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}","AC","District","Our Recommendation (Best Candidate)","Party","CurrentMLA","IsIncumbent", "Candidates We Considered In Our Survey");
                foreach (var district in state.Districts)
                {
                    foreach (var ac in district.ACs)
                    {
                        var candidate = bestCandidates.Find(t => t.AC.No == ac.No);
                        if (candidate == null)
                        {
                            continue;
                        }
                        writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", candidate.AC.Name, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(district.Name.ToLower()),
                            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(candidate.BestCandidate.Name.Split('(')[0].ToLower()), candidate.BestCandidate.PartyName,
                            candidate.CurrentMLA,candidate.IsCurrentBest,
                            String.Join(",  ",candidate.CandidatesConsidered.Select(t=> String.Format("{0}({1})", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(t.Name.Split('(')[0].ToLower()), t.PartyName))));
                    }
                }
            }
        }

        public void FillUpRestOfCandidates(List<CandidateSelection> bestCandidates, List<Result> results2010)
        {
            var validParties = new List<string> { "bjp", "rlsp", "ham", "ljp" };
            var acIds = bestCandidates.Select(t => t.AC.No).Distinct().ToArray();
            var allAcIds = results2010.Select(t => t.Id).ToArray();
            var restOfIds = allAcIds.Except(acIds).ToArray();
            foreach(var restOfId in restOfIds)
            {
                var result = results2010.First(t => t.Id == restOfId);
                var bestCand = result.Votes.Where(t => validParties.Contains(t.Party.ToString())).ToArray();
                if (!bestCand.Any()) { writer.WriteLine("Cannot find suitable Cand for {0}: {1}", result.Name, restOfId);  continue; }
                var candSelect = new CandidateSelection
                {
                   AC = new AssemblyConstituency { Name = result.Name, No = result.Id},
                   BestCandidate = new CandidateRating()
                   {
                       Name = bestCand.OrderByDescending(t => t.Votes).First().Name,
                       PartyName = bestCand.OrderByDescending(t => t.Votes).First().Party.ToString()
                   },
               };
                 candSelect.CandidatesConsidered = new List<CandidateRating>() {candSelect.BestCandidate};
                 bestCandidates.Add(candSelect);
            }
        }

        public void FillUpCurrentCandidate(List<CandidateSelection> candidateSelections, List<Result> results2010)
        {
            foreach (var selection in candidateSelections)
            {
                var result = results2010.First(t => t.Id == selection.AC.No);
                var winner = result.Votes.OrderByDescending(t => t.Votes).First();
                selection.CurrentMLA = winner.Name;
                selection.WinningParty = winner.Party.ToString();
                selection.IsCurrentBest = Utils
                    .LevenshteinDistance(Utils.GetNormalizedName(selection.BestCandidate.Name.Split('(')[0].Trim()), 
                    Utils.GetNormalizedName(selection.CurrentMLA)) < 3;
            }
        }

        private readonly QualitativeData _data;
    }

    public class CandidateSelection
    {
        public AssemblyConstituency AC { get; set; }
        public CandidateRating BestCandidate { get; set; }
        public IEnumerable<CandidateRating> CandidatesConsidered { get; set; }
        public string CurrentMLA { get; set; }
        public string WinningParty { get; set; }

        public bool IsCurrentBest { get; set; }
    }
}
