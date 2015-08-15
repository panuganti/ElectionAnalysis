using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BiharElectionsLibrary
{
    [DataContract]
    public class Parameters
    {
        [DataMember]
        public int NoOfHouses { get; set; }
        [DataMember]
        public Dictionary<Gender, int> Population { get; set; }
        [DataMember]
        public Dictionary<Gender, int> Children { get; set; }
    }
}
