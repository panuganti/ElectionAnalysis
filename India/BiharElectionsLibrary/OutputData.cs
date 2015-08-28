using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BiharElectionsLibrary
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<CandidateVote> Votes { get; set; }
    }

    [DataContract]
    public class CandidateVote
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public PoliticalParty Party { get; set; }
        [DataMember]
        public int Votes { get; set; }
    }
}
