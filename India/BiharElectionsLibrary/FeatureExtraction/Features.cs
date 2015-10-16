using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CVoterContracts;

namespace BiharElectionsLibrary
{
    public class Features
    {
        private readonly List<Result> _results2005;
        private readonly List<Result> _results2009;
        private readonly List<Result> _results2010;
        private readonly List<Result> _results2014;
        private readonly CVoterContracts.QualitativeData _qualitative2010;
        private readonly CVoterContracts.QualitativeData _qualitative2015;

        private Dictionary<string, string> FeatureVectorDict { get; set; }

        public Features(List<Result> results2009, List<Result> results2010, List<Result> results2014, Tuple<CVoterContracts.QualitativeData, CVoterContracts.QualitativeData> qualitativeData)
        {
            _results2009 = results2009;
            _results2010 = results2010;
            _results2014 = results2014;
            _qualitative2010 = qualitativeData.Item1;
            _qualitative2015 = qualitativeData.Item2;
            FeatureVectorDict = new Dictionary<string, string>();
        }

        public List<Stability> StabilityFeatures()
        {
            var acs = _results2014;
            var stabilityData = new List<Stability>();
            var ids = String.Join(";",_results2010.Select(t => t.Id).OrderBy(t => t).ToArray());
            foreach (var ac in _results2014)
            {
                var stability = new Stability {AcId = ac.Id};
                var winnerParty2009 = _results2009.First(t => t.Id == ac.Id).GetWinningParty();
                var winnerParty2010 = _results2010.First(t => t.Id == ac.Id).GetWinningParty();
                var winnerParty2014 = _results2014.First(t => t.Id == ac.Id).GetWinningParty();

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

        public List<FeatureVector> Extract2010Features()
        {
            var extraction = new List<FeatureVector>();
            List<CandidateRating> allCandidateRatings = _qualitative2010.AcQualitatives.SelectMany(t => t.CandidateRatings).ToList();
            var check = String.Join(";",allCandidateRatings.Select(t => String.Format("{0}_{1}", t.Name, t.PartyName)));
            int totalCount = 0, noCandCount = 0, oneCandCount = 0, multipleCandCount = 0, useBestPartyCand = 0;
            // acNo, candidate, result, <features>
            for (int i = 1; i <= 243; i++)
            {
                int acId = i;
                var result2009 = _results2009.First(t => t.Id == acId);
                var result2010 = _results2010.First(t => t.Id == acId);
                var qualitativeData = _qualitative2010.AcQualitatives.Where(t=>t.No == acId).ToArray();
                if (!qualitativeData.Any()) { continue;}

                var winner = _results2009.First(t => t.Id == acId).GetWinner();
                foreach (CandidateVote candidateVote in result2010.Votes)
                {
                    totalCount++;
                    CandidateVote vote = candidateVote;
                    var featureVector = new FeatureVector
                    {
                        ACNo = i.ToString(CultureInfo.InvariantCulture),
                        CandidateName = candidateVote.Name,
                        Party = candidateVote.Party.ToString(),
                        Result = FeatureVector.ParseLabel(candidateVote.Position),
                        IsIncumbent = winner == candidateVote.Name ?  1.ToString() : 0.ToString()
                    };
                    var possibleCandidates = allCandidateRatings.Where(t => t.Name.ToLower() == vote.Name.ToLower()).ToArray();
                    if (!possibleCandidates.Any())
                    {
                        // No entry for the candidate -- How to reduce this count ? relax on name matching
                        if (qualitativeData.Any())
                        {
                            var partyCandidateRatings = qualitativeData.First()
                                .CandidateRatings.Where(
                                    t => t.PartyName.ToLower() == vote.Party.ToString().ToLower()).ToArray();
                            if (partyCandidateRatings.Length == 1)
                            {
                                oneCandCount++;
                                FillFeatureVector(partyCandidateRatings.First(), featureVector);
                            }
                            else if (partyCandidateRatings.Length > 1)
                            {
                                try
                                {
                                    var candidateRating = 
                                    partyCandidateRatings.OrderByDescending(
                                        t => t.Ratings.First(x => x.Feature.ToLower() == "winability").Score).First();
                                    FillFeatureVector(candidateRating, featureVector);
                                }
                                catch (Exception e)
                                {
                                    noCandCount++;
                                    featureVector.Winnability = 0.ToString();
                                    featureVector.Availability = 0.ToString();
                                }
                            }
                            else if (partyCandidateRatings.Length == 0)
                            {
                                noCandCount++;
                                featureVector.Winnability = 0.ToString();
                                featureVector.Availability = 0.ToString();                                
                            }
                        }
                    }
                    if (possibleCandidates.Length == 1)
                    {
                        oneCandCount++;
                        FillFeatureVector(possibleCandidates.First(), featureVector);
                    }
                    if (possibleCandidates.Length > 1)
                    {
                        var samePartyCands = possibleCandidates.Where(t => t.PartyName.ToLower() == vote.Party.ToString().ToLower()).ToArray();
                        if (samePartyCands.Length == 1)
                        {
                            oneCandCount++;
                            FillFeatureVector(samePartyCands.First(), featureVector);
                        }
                        else if (samePartyCands.Length > 1)
                        {
                            multipleCandCount++;
                            continue;
                        }
                        else
                        {
                            var candAsInd = possibleCandidates.Where(t => t.PartyName.ToLower() == "ind" || t.PartyName.ToLower() == "others").ToArray();
                            if (candAsInd.Length == 1)
                            {
                                oneCandCount++;
                                FillFeatureVector(samePartyCands.First(), featureVector);
                            }
                            else
                            {
                                noCandCount++;
                                featureVector.Winnability = 0.ToString();
                                featureVector.Availability = 0.ToString();
                            }
                        }
                        //throw new Exception("");
                    }
                    extraction.Add(featureVector);
                }
            }
            return extraction;
        }

        private void FillFeatureVector(CandidateRating rating, FeatureVector featureVector)
        {
            var winnability = rating.Ratings.Where(t => t.Feature.ToLower().Equals("winability")).ToArray();
            featureVector.Winnability = winnability.Any()
                ? winnability.First().Score.ToString(CultureInfo.InvariantCulture)
                : 0.ToString(CultureInfo.InvariantCulture);
            var availability = rating.Ratings.Where(t => t.Feature.ToLower().Equals("availability")).ToArray();
            featureVector.Availability = availability.Any()
                ? availability.First().Score.ToString(CultureInfo.InvariantCulture)
                : 0.ToString(CultureInfo.InvariantCulture);
            var partyLeaders = rating.Ratings.Where(t => t.Feature.ToLower().Equals("Understing with Party Leaders".ToLower())).ToArray();
            featureVector.UnderstandingWithPartyLeaders = partyLeaders.Any()
                ? partyLeaders.First().Score.ToString(CultureInfo.InvariantCulture)
                : 0.ToString(CultureInfo.InvariantCulture);
            var localLeadersSupport = rating.Ratings.Where(t => t.Feature.ToLower().Equals("Respect in Local Leaders".ToLower())).ToArray();
            featureVector.RespectWithLocalLeaders = localLeadersSupport.Any()
                ? localLeadersSupport.First().Score.ToString(CultureInfo.InvariantCulture)
                : 0.ToString(CultureInfo.InvariantCulture);
        }

        public List<FeatureVector> Extract2015Features()
        {
            var extraction = new List<FeatureVector>();
            for (int i = 1; i <= 243; i++)
            {
                var result2014 = _results2014.First(t => t.Id == i);
                var qualitativeData = _qualitative2015.AcQualitatives.Where(t => t.No == i).ToArray();
                if (!qualitativeData.Any()) { continue; }
                var candidateData = qualitativeData.First().CandidateRatings;
                foreach (var candidate in candidateData)
                {
                    var featureVector = new FeatureVector
                    {
                        ACNo = i.ToString(CultureInfo.InvariantCulture),
                        CandidateName = candidate.Name,
                        Party = candidate.PartyName
                    };

                    #region Candidate Features
                    var winnability = candidate.Ratings.Where(t => t.Feature.ToLower().Equals("winability")).ToArray();
                    featureVector.Winnability = winnability.Any() ? 
                        winnability.First().Score.ToString(CultureInfo.InvariantCulture) : 0.ToString();
                    var availability =
                        candidate.Ratings.Where(t => t.Feature.ToLower().Equals("availability")).ToArray();
                    featureVector.Availability = availability.Any() ? 
                        availability.First().Score.ToString(CultureInfo.InvariantCulture) : 0.ToString();
                    var partyLeaders =
                        candidate.Ratings.Where(t => t.Feature.ToLower().Equals("Understanding with Party Leaders".ToLower())).ToArray();
                    featureVector.UnderstandingWithPartyLeaders = partyLeaders.Any() ?
                        partyLeaders.First().Score.ToString(CultureInfo.InvariantCulture) : 0.ToString();
                    #endregion Candidate Features

                    extraction.Add(featureVector);
                }
            }
            return extraction;
        }
    }

    public class FeatureVector
    {
        public string ACNo {get; set;}
        public string CandidateName { get; set; }
        public string Party {get; set;}
        public Label Result { get; set; }

        #region CandidateFeatures
        public string Winnability { get; set; }
        public string Availability { get; set; }
        public string UnderstandingWithPartyLeaders { get; set; }
        public string RespectWithLocalLeaders { get; set; }
        public string IsIncumbent { get; set; }
        #endregion CandidateFeatures

        #region PartyFeatures
        public string StrengthOfParty { get; set; }
        public string BoothManagement { get; set; }

        #endregion PartyFeatures

        #region CasteFeatures

        #endregion CasteFeatures

        #region DevelopmentFeatures
        public string Electricity { get; set; }
        public string Water { get; set; }
        public string LawAndOrder { get; set; }
        public string Road { get; set; }
        public string EmploymentGrowth { get; set; }
        #endregion DevelopmentFeatures
        // Add IsLocalIncumbent, IsCMIncumbent, IsPMIncumbent

        public string GetHeaders()
        {
            var sb = new StringBuilder();
            foreach (var prop in this.GetType().GetProperties())
            {
                sb.Append(String.Format("\t{0}", prop.Name));
            }
            return sb.ToString().Trim('\t');            
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var prop in this.GetType().GetProperties())
            {
                sb.Append(String.Format("\t{0}", prop.GetValue(this, null)));
            }
            return sb.ToString().Trim('\t');
        }

        public static Label ParseLabel(int position)
        {
            switch (position)
            {
                case 1:
                    return Label.Perfect;
                case 2:
                    return Label.Excellent;
                case 3:
                    return Label.Good;
                case 4:
                    return Label.Fair;
                default:
                    return Label.Bad;
            }
        }
    }

    public class Stability
    {
        public int AcId { get; set; }
        public string Party { get; set; }
        public bool IsStable { get; set; }
    }

    public enum Label
    {
        Perfect = 1,
        Excellent = 2,
        Good = 3,
        Fair = 4,
        Bad = 5
    }
}
