using System;
using System.Collections.Generic;
using System.Linq;
using CVoterContracts;
using QualitativeData = CVoterContracts.QualitativeData;

namespace BiharElectionsLibrary
{
    public class Features
    {
        private readonly List<ACResult> _results2009;
        private readonly List<ACResult> _results2010;
        private readonly List<ACResult> _results2014;
        private readonly CVoterContracts.QualitativeData _qualitative2010;
        private readonly CVoterContracts.QualitativeData _qualitative2015;

        private Dictionary<string, string> FeatureVector { get; set; }

        public Features(List<ACResult> results2009, List<ACResult> results2010, List<ACResult> results2014, Tuple<CVoterContracts.QualitativeData, CVoterContracts.QualitativeData> qualitativeData)
        {
            _results2009 = results2009;
            _results2010 = results2010;
            _results2014 = results2014;
            _qualitative2010 = qualitativeData.Item1;
            _qualitative2015 = qualitativeData.Item2;
            FeatureVector = new Dictionary<string, string>();
        }

        public List<Stability> StabilityFeatures()
        {
            var acs = _results2014.Select(t => t.Constituency);
            var stabilityData = new List<Stability>();
            
            foreach (var ac in acs)
            {
                var stability = new Stability {AcId = ac.No};
                var winnerParty2009 = _results2009.First(t => t.Constituency.No == ac.No).GetWinningParty();
                var winnerParty2010 = _results2010.First(t => t.Constituency.No == ac.No).GetWinningParty();
                var winnerParty2014 = _results2014.First(t => t.Constituency.No == ac.No).GetWinningParty();

                if ((winnerParty2009 == winnerParty2010) && (winnerParty2009 == winnerParty2014))
                {
                    stability.Party = winnerParty2014.ToString();
                    stability.IsStable = true;
                }
                else
                {
                    stability.Party = winnerParty2014.ToString();
                    stability.IsStable = false;
                }
                stabilityData.Add(stability);
            }
            return stabilityData;
        }

        public void Extract2010Features()
        {
            // acNo, candidate, result, <features>
            for (int i = 1; i <= 243; i++)
            {
                var result2009 = _results2009.First(t => t.Constituency.No == i);
                var result2010 = _results2010.First(t => t.Constituency.No == i);
                var qualitativeData = _qualitative2010.AcQualitatives.First(t=>t.No == i);
                var candidateData = qualitativeData.CandidateRatings;
                foreach (var candidate in candidateData)
                {
                    var featureVector = new FeatureVector();
                    featureVector.acNo = i.ToString();
                    featureVector.candidateName = candidate.Name;
                    featureVector.party = candidate.PartyName;
                    featureVector.winnability = candidate.Ratings.First(t => t.Feature.ToLower().Equals("winability")).Score.ToString();
                    featureVector.availability = candidate.Ratings.First(t => t.Feature.ToLower().Equals("availability")).Score.ToString();
                }
            }
        }

        public void Extract2015Features()
        {

        }
    }

    public class FeatureVector
    {
        public string acNo {get; set;}
        public string candidateName { get; set; }
        public string party {get; set;}
        public string result { get; set; }
        public string winnability { get; set; }
        public string availability { get; set; }
    }

    public class Stability
    {
        public int AcId { get; set; }
        public string Party { get; set; }
        public bool IsStable { get; set; }
    }
}
