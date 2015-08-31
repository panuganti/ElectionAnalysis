using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BiharElectionsLibrary
{
    [DataContract]
    public class ACResult : ConstituencyResult
    {
        [DataMember]
        public AssemblyConstituency Constituency { get; set; }
    }

    [DataContract]
    public class PCResult : ConstituencyResult
    {
        [DataMember]
        public ParliamentaryConstituency Constituency { get; set; }
    }

    [DataContract]
    public class CandidateVotes 
    {
        [DataMember]
        public Candidate Candidate { get; set; }
        [DataMember]
        public int Votes { get; set; }
        [DataMember]
        public int Position { get; set; }
    }

    [DataContract]
    public abstract class ConstituencyResult
    {
        [DataMember]
        public int YearOfElection { get; set; }
        [DataMember]
        public List<CandidateVotes> Votes { get; set; }
        [DataMember]
        public int TotalVotes { get; set; }
        [DataMember]
        public int NOTA { get; set; }

        public Candidate GetWinner()
        {
            return Votes.Aggregate((l, r) => l.Votes > r.Votes ? l : r).Candidate;
        }

        public PoliticalParty GetWinningParty()
        {
            return GetWinner().Party;
        }
    }

    [DataContract]
    public class AssemblyElectionResults : ElectionResults
    {
        [DataMember]
        public List<ACResult> ConstituencyResults { get; set; }
    }

    [DataContract]
    public class ParliamentaryElectionResults : ElectionResults
    {
        [DataMember]
        public List<PCResult> ConstituencyResults { get; set; }
    }

    [DataContract]
    public abstract class ElectionResults
    {
        [DataMember]
        public int PrimaryYearOfElection { get; set; }        
    }
}
