using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BiharElectionsLibrary;
using CVoterContracts;
using QualitativeData = CVoterContracts.QualitativeData;

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
                writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}","AC","District","Our Recommendation (Best Candidate)","Party","Candidates We Considered In Our Survey");
                foreach (var district in state.Districts)
                {
                    foreach (var ac in district.ACs)
                    {
                        var candidate = bestCandidates.Find(t => t.AC.No == ac.No);
                        if (candidate == null)
                        {
                            continue;
                        }
                        writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", candidate.AC.Name, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(district.Name.ToLower()),
                            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(candidate.BestCandidate.Name.Split('(')[0].ToLower()), candidate.BestCandidate.PartyName,
                            String.Join(",  ",candidate.CandidatesConsidered.Select(t=> String.Format("{0}({1})", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(t.Name.Split('(')[0].ToLower()), t.PartyName))));
                    }
                }
            }
        }

        private readonly QualitativeData _data;
    }

    public class CandidateSelection
    {
        public AssemblyConstituency AC { get; set; }
        public CandidateRating BestCandidate { get; set; }
        public IEnumerable<CandidateRating> CandidatesConsidered { get; set; }
    }
}
