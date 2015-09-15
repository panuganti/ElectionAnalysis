using System.Collections.Generic;
using System.Linq;
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

        public string GetWinner()
        {
            return Votes.Aggregate((l, r) => l.Votes > r.Votes ? l : r).Name;
        }

        public PoliticalParty GetWinningParty()
        {
            return Votes.Aggregate((l, r) => l.Votes > r.Votes ? l : r).Party;
        }
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
        [DataMember]
        public int Position { get; set; }
    }
}
