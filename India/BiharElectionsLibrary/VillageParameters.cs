using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BiharElectionsLibrary
{
    [DataContract]
    public class VillageParameters : Parameters
    {
        [DataMember]
        public Dictionary<Gender, int> SCs { get; set; }
        [DataMember]
        public Dictionary<Gender, int> STs { get; set; }
        [DataMember]
        public Dictionary<Gender, double> LiteracyGenderWise { get; set; }
        [DataMember]
        public Dictionary<Gender, int> TotalWorkers { get; set; }
        [DataMember]
        public int MainWorkers { get; set; } // Earning 6 or more months
        [DataMember]
        public int MarginalWorkers { get; set; } // Less than 6 months earning
        [DataMember]
        public double Literacy { get; set; }
    }
}
