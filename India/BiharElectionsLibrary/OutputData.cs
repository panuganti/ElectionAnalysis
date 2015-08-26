using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
