using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BiharElectionsLibrary
{
    [DataContract]
    public class CensusTownParams : Parameters
    {
        [DataMember]
        public Dictionary<Gender, double> LiteracyByGender { get; set; }
        [DataMember]
        public double Literacy { get; set; }
        [DataMember]
        public double SCs { get; set; } // %
        [DataMember]
        public double STs { get; set; } // %
        [DataMember]
        public Dictionary<Gender, int> Workers { get; set; }
        // Worker: somone who does business,job,service,cultivator or labor
        [DataMember]
        public double MainWorkers { get; set; } // %
        [DataMember]
        public double MarginalWorkers { get; set; } // %
    }
}
