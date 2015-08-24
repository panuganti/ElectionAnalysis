using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BiharElectionsLibrary
{
    [DataContract]
    public class ParamDictionaries
    {
        [DataMember]
        public static Dictionary<Village, VillageParameters> VillageParams { get; set; }
        [DataMember]
        public static Dictionary<CensusTown, CensusTownParams> CensusTownParams { get; set; }
        //public Dictionary<MunicipalCorp>
        [DataMember]
        public static Dictionary<District, DistrictParameters> DistrictParams { get; set; }
    }
}
