using System;
using System.Collections.Generic;

namespace BiharElectionsLibrary
{
    public class Candidate : IEqualityComparer<Candidate>
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public ConstituencyCasteCategory ConstituencyCasteCategory { get; set; }
        public int YearOfBirth { get; set; }
        public PoliticalParty Party { get; set; }
        public Dictionary<int, PoliticalParty> PreviousAffiliations { get; set; }

        public bool Equals(Candidate x, Candidate y)
        {
            //todo: Change equals from string equals to Utils
            return x.Name.Equals(y.Name) && x.Party.Equals(Utils.GetParty(y.Name));
        }

        public int GetHashCode(Candidate obj)
        {
            throw new NotImplementedException();
        }
    }
}
