using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BiharElectionsLibrary
{
    [DataContract]
    public class Candidate : IEqualityComparer<Candidate>
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Gender Gender { get; set; }
        [DataMember]
        public ConstituencyCasteCategory ConstituencyCasteCategory { get; set; }
        [DataMember]
        public int YearOfBirth { get; set; }
        [DataMember]
        public PoliticalParty Party { get; set; }
        [DataMember]
        public Dictionary<int, PoliticalParty> PreviousAffiliations { get; set; }

        #region Methods

        public bool Equals(Candidate x, Candidate y)
        {
            //todo: Change equals from string equals to Utils
            return x.Name.Equals(y.Name) && x.Party.Equals(Utils.GetParty(y.Name));
        }

        public int GetHashCode(Candidate obj)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
