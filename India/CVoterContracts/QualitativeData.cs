using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVoterContracts
{
    [DataContract]
    public class QualitativeData
    {
        [DataMember]
        public List<AcQualitative> AcQualitatives { get; set; }
        [DataMember]
        public int Year { get; set; }
    }

    [DataContract]
    public class AcQualitative
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int No { get; set; }
        [DataMember]
        public List<Rating> LocalIssues { get; set; }
        [DataMember]
        public List<Rating> DevParams { get; set; }
        [DataMember]
        public List<CandidateRating> CandidateRatings { get; set; }
        [DataMember]
        public List<PartyRating> PartyRatings { get; set; }
        [DataMember]
        public CasteShare CasteShares { get; set; }
        [DataMember]
        public List<PartyCasteShare> PartyCasteShares { get; set; }
    }

    [DataContract]
    public class Rating
    {
        [DataMember]
        public string Feature { get; set; }
        [DataMember]
        public int Score { get; set; }
    }

    [DataContract]
    public class FeaturePerCent
    {
        [DataMember]
        public string Feature { get; set; }
        [DataMember]
        public double Score { get; set; }
    }

    [DataContract]
    public class CandidateRating : PartyRating
    {
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class PartyRating
    {
        [DataMember]
        public string PartyName { get; set; }
        [DataMember]
        public List<Rating> Ratings { get; set; }
    }

    [DataContract]
    public class CasteShare : CastePerCents
    {
        [DataMember]
        public List<Rating> CasteShares { get; set; }
    }

    [DataContract]
    public class PartyCasteShare : CastePerCents
    {
        [DataMember]
        public string PartyName { get; set; }
    }

    [DataContract]
    public class CastePerCents
    {
        [DataMember]
        public List<FeaturePerCent> PerCents { get; set; }        
    }
}
