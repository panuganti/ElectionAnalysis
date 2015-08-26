using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace BiharElectionsLibrary
{
  
    #region ElectionHierarchy

    [DataContract]
    public class AssemblyConstituency : Constituency
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<PollingBooth> Booths { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ParliamentaryConstituency PC { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public District District { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<AssemblyConstituency> Neighbors { get; set; }

        public IEnumerable<House> Houses
        {
            get { return Booths.SelectMany(x => x.Houses); }
        }

        public IEnumerable<Voter> Voters
        {
            get { return Houses.SelectMany(x => x.Voters); }
        }
    }

    [DataContract]
    public class ParliamentaryConstituency : Constituency
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public District District { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<AssemblyConstituency> ACs { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<ParliamentaryConstituency> Neighbors { get; set; }
    }

    [DataContract]
    public class Constituency : RegionWithId
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ConstituencyCasteCategory Category { get; set; }
    }

    [DataContract]
    public class PollingBooth : RegionWithId
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<House> Houses { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AssemblyConstituency AC { get; set; }
    }

    [DataContract]
    public class House : RegionWithId
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PollingBooth Booth { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<Voter> Voters { get; set; }
    }

    [DataContract]
    public class Voter : Person
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public House House { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Age { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Relative> Relatives { get; set; }
    }

    [DataContract]
    public class Person : Region
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Caste Caste { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Gender Gender { get; set; }

    }

    [DataContract]
    public class Relative
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RelationType RelationType { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Person Person { get; set; }
    }

    #endregion ElectionHierarchy

    #region AdminHierarchy

    [DataContract]
    public class Block : RegionWithId
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<Village> Villages { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<CensusTown> CensusTowns { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<MunicipalCorp> MunicipalCorps { get; set; } // Very likely, there would be only 1
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public District District { get; set; }

        #region Info Add methods

        public MunicipalCorp AddMunicipalCorp(string name, int id)
        {
            if (MunicipalCorps == null)
            {
                MunicipalCorps = new HashSet<MunicipalCorp>();
            }
            if (!MunicipalCorps.Any(x => x.Name.Equals(name)))
            {
                var newMC = new MunicipalCorp {Block = this, Name = name, No = id};
                MunicipalCorps.Add(newMC);
                return newMC;
            }
            return null;            
        }

        public CensusTown AddCensusTown(string name, int id)
        {
            if (CensusTowns == null)
            {
                CensusTowns = new HashSet<CensusTown>();
            }
            if (!CensusTowns.Any(x => x.Name.Equals(name)))
            {
                var newCensusTown = new CensusTown { Block = this, Name = name, No = id };
                CensusTowns.Add(newCensusTown);
                return newCensusTown;
            }
            return null;
        }

        public Village AddVillage(string name, int id)
        {
            if (Villages == null)
            {
                Villages = new HashSet<Village>();
            }
            if (!Villages.Any(x => x.Name.Equals(name)))
            {
                var newVillage = new Village { Block = this, Name = name, No = id };
                Villages.Add(newVillage);
                return newVillage;
            }
            return null;
        }

        #endregion Info Add methods
    }

    [DataContract]
    public class Village : RegionWithId
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Block Block { get; set; }
    }

    [DataContract]
    public class Ward : RegionWithId
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MunicipalCorp MunicipalCorp { get; set; }
    }

    [DataContract]
    public class CensusTown : RegionWithId
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Block Block { get; set; }        
    }

    [DataContract]
    public class MunicipalCorp : RegionWithId
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int TotalPopulation { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<Ward> Wards { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Block Block { get; set; }

        #region Add Ward Info

        public Ward AddWard(string name)
        {
            if (Wards == null)
            {
                Wards = new HashSet<Ward>();
            }
            int no = Int32.Parse(new Regex(@"(\d+)").Match(name).Groups[1].Value);
            if (!Wards.Any(x => x.No.Equals(no)))
            {
                var newWard = new Ward {No = no, Name = name, MunicipalCorp = this};
                Wards.Add(newWard);
                return newWard;
            }
            return null;
        }
        #endregion Add Ward Info
    }

    #endregion AdminHierarchy

    #region Common Hierarchy

    [DataContract]
    public class District : RegionWithId
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Division Division { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<Block> Blocks { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<AssemblyConstituency> ACs { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DistrictParameters Params { get; set; }

        public IEnumerable<PollingBooth> Booths
        {
            get { return ACs.SelectMany(x => x.Booths); }
        }

        public IEnumerable<House> Houses
        {
            get { return Booths.SelectMany(x => x.Houses); }
        }

        public IEnumerable<Voter> Voters
        {
            get { return Houses.SelectMany(x => x.Voters); }
        }

        #region Info Add methods

        public Block AddBlock(string name, int id)
        {
            if (Blocks == null)
            {
                Blocks = new HashSet<Block>();
            }
            if (!Blocks.Any(x => x.Name.Equals(name)))
            {
                var newBlock = new Block {District = this, Name = name, No = id};
                Blocks.Add(newBlock);
                return newBlock;
            }
            return null;
        }

        #endregion Info Add methods

    }

    [DataContract]
    public class Division : RegionWithId
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public State State { get; set; }
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<District> Districts { get; set; }
    }

    [DataContract]
    public class State : Region
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<Division> Divisions { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HashSet<ParliamentaryConstituency> PCs { get; set; } // TODO: Move it to Division class

        #region ChildNodes
        public IEnumerable<District> Districts
        {
            get { return Divisions.SelectMany(x => x.Districts); }
        }

        public IEnumerable<AssemblyConstituency> ACs
        {
            get { return PCs.SelectMany(x => x.ACs); }
        }

        public IEnumerable<PollingBooth> Booths
        {
            get { return ACs.SelectMany(x => x.Booths); }
        }

        public IEnumerable<House> Houses
        {
            get { return Booths.SelectMany(x => x.Houses); }
        }

        public IEnumerable<Voter> Voters
        {
            get { return Houses.SelectMany(x => x.Voters); }
        }

        public IEnumerable<Block> Blocks
        {
            get { return Districts.SelectMany(x => x.Blocks); }
        }
        public IEnumerable<Village> Villages
        {
            get { return Blocks.SelectMany(x => x.Villages); }
        }

        public IEnumerable<MunicipalCorp> MCs
        {
            get { return Blocks.SelectMany(x => x.MunicipalCorps); }
        }

        public IEnumerable<CensusTown> CensusTowns
        {
            get { return Blocks.SelectMany(x => x.CensusTowns); }
        }
        #endregion ChildNodes
    }

    #endregion Common Hierarchy

    #region generic

    [DataContract]
    public abstract class RegionWithId : Region
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int No { get; set; }
    }

    [DataContract]
    public abstract class Region
    {
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    #endregion generic
}
