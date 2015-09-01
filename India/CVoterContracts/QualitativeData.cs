using System.Collections.Generic;

namespace CVoterContracts
{
    public class QualitativeData
    {
        public List<AcQualitative> AcQualitatives { get; set; }
    }

    public class AcQualitative
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public int No { get; set; }
        public List<Rating> LocalIssues { get; set; }
        public List<Rating> DevParams { get; set; }
        public List<CandidateRating> CandidateRatings { get; set; }
        public List<PartyRating> PartyRatings { get; set; }
        public List<CasteShare> CasteShares { get; set; }
    }

    public class Rating
    {
        public string Feature { get; set; }
        public int Score { get; set; }
    }

    public class CandidateRating : PartyRating
    {
        public string Name { get; set; }
    }

    public class PartyRating
    {
        public string PartyName { get; set; }
        public List<Rating> Ratings { get; set; }
    }

    public class CasteShare : CastePerCents
    {
        public List<Rating> CasteShares { get; set; }
    }

    public class PartyCasteShare : CastePerCents
    {
        public string PartyName { get; set; }
    }

    public class CastePerCents
    {
        public List<Rating> PerCents { get; set; }        
    }
}
