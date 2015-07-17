using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BiharElectionsLibrary
{
    [DataContract]
    public class DistrictParameters : Parameters
    {
        #region 2011 vs 2001
        [DataMember]
        public Dictionary<int, int> ActualPopulationByYear { get; set; }
        [DataMember]
        public Dictionary<Gender, Dictionary<int, int>> GenderWisePopulationByYear { get; set; }
        [DataMember]
        public Dictionary<int, double> PopulationGrowth { get; set; }
        [DataMember]
        public Dictionary<int, int> AreaInSqKm { get; set; }
        [DataMember]
        public Dictionary<int, int> Density { get; set; }
        [DataMember]
        public Dictionary<int, int> SexRatioByYear { get; set; }
        [DataMember]
        public Dictionary<int, int> ChildSexRatioByYear { get; set; }
        [DataMember]
        public Dictionary<int, double> AvgLiteracyByYear { get; set; }
        [DataMember]
        public Dictionary<Gender, Dictionary<int, double>> LiteracyByYear { get; set; }
        [DataMember]
        public Dictionary<int, int> TotalChildPopulation { get; set; }
        [DataMember]
        public Dictionary<Gender, Dictionary<int, int>> ChildPopulationByYear { get; set; }
        [DataMember]
        public Dictionary<int, int> TotalLiteratesByYear { get; set; }
        [DataMember]
        public Dictionary<Gender, Dictionary<int, int>> LiteratesByYear { get; set; }
        #endregion 2011 vs 2001

        #region Rural Vs Urban
        [DataMember]
        public Dictionary<LocationType, double> PopulatinPercent { get; set; }
        [DataMember]
        public Dictionary<LocationType, int> TotalPopulation { get; set; }
        [DataMember]
        public Dictionary<LocationType, int> SexRatio { get; set; }
        [DataMember]
        public Dictionary<LocationType, int> ChildSexRatio { get; set; }
        [DataMember]
        public Dictionary<LocationType, int> ChildPopulation { get; set; }
        [DataMember]
        public Dictionary<LocationType, double> AvgChildPercent { get; set; }
        [DataMember]
        public Dictionary<Gender, Dictionary<LocationType, double>> ChildPercent { get; set; }
        [DataMember]
        public Dictionary<LocationType, int> TotalLiterates { get; set; }
        [DataMember]
        public Dictionary<Gender, Dictionary<LocationType, int>> Literates { get; set; }
        [DataMember]
        public Dictionary<LocationType, double> AvgLiteracy { get; set; }
        [DataMember]
        public Dictionary<Gender, Dictionary<LocationType, double>> Literacy { get; set; }

        #endregion Rural Vs Urban
    }
}
