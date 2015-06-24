using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace BiharElectionsLibrary
{
  
    #region ElectionHierarchy

    [DataContract]
    public class AssemblyConstituency : Constituency
    {
        [DataMember]
        public HashSet<PollingBooth> Booths { get; set; }
        [DataMember]
        public ParliamentaryConstituency PC { get; set; }
        [DataMember]
        public HashSet<AssemblyConstituency> Neighbors { get; set; }

        public IEnumerable<House> Houses
        {
            get { return Booths.SelectMany(x => x.Houses); }
        }

        public IEnumerable<Voter> Voters
        {
            get { return Houses.SelectMany(x => x.Voters); }
        }

        public void Merge(AssemblyConstituency ac)
        {
            if (Name == null && ac.Name != null)
            {
                Name = ac.Name;
            }
            if (No == 0 && ac.No != 0)
            {
                No = ac.No;
            }
            if (ac.Category != ConstituencyCasteCategory.Error && Category == ConstituencyCasteCategory.Error)
            {
                Category = ac.Category;
            }
        }
    }

    [DataContract]
    public class ParliamentaryConstituency : Constituency
    {
        [DataMember]
        public District District { get; set; }
        [DataMember]
        public HashSet<AssemblyConstituency> Constituencies { get; set; }
        [DataMember]
        public HashSet<ParliamentaryConstituency> Neighbors { get; set; }

        public void Merge(ParliamentaryConstituency pc)
        {
            if (Name == null && pc.Name != null)
            {
                Name = pc.Name;
            }
            if (No == 0 && pc.No != 0)
            {
                No = pc.No;
            }
            foreach (var constituency in pc.Constituencies)
            {
                if (Constituencies == null)
                {
                    Constituencies = new HashSet<AssemblyConstituency> { constituency };
                }
                if (!(Constituencies.Any(
                    x => x.Name == constituency.Name || (x.No != 0 && x.No == constituency.No))))
                {
                    constituency.PC = this;
                    Constituencies.Add(constituency);
                    return;
                }
                Constituencies.First(x => x.Name == constituency.Name || (x.No != 0 && x.No == constituency.No))
                    .Merge(constituency);
            }
        }
    }

    [DataContract]
    public class Constituency : RegionWithId
    {
        [DataMember]
        public ConstituencyCasteCategory Category { get; set; }
    }

    [DataContract]
    public class PollingBooth : RegionWithId
    {
        [DataMember]
        public HashSet<House> Houses { get; set; }
        [DataMember]
        public AssemblyConstituency AC { get; set; }
    }

    [DataContract]
    public class House : RegionWithId
    {
        [DataMember]
        public PollingBooth Booth { get; set; }
        [DataMember]
        public HashSet<Voter> Voters { get; set; }
    }

    [DataContract]
    public class Voter : Person
    {
        [DataMember]
        public House House { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public List<Relative> Relatives { get; set; }
    }

    [DataContract]
    public class Person : Region
    {
        [DataMember]
        public Caste Caste { get; set; }

        [DataMember]
        public Gender Gender { get; set; }

    }

    [DataContract]
    public class Relative
    {
        [DataMember]
        public RelationType RelationType { get; set; }
        [DataMember]
        public Person Person { get; set; }
    }

    #endregion ElectionHierarchy

    #region AdminHierarchy

    [DataContract]
    public class Block : RegionWithId
    {
        [DataMember]
        public HashSet<Village> Villages { get; set; }
        [DataMember]
        public HashSet<CensusTown> CensusTowns { get; set; }
        [DataMember]
        public HashSet<MunicipalCorp> MunicipalCorps { get; set; } // Very likely, there would be only 1
        [DataMember]
        public District District { get; set; }

        #region Info Add methods

        public MunicipalCorp AddMunicipalCorp(string name, int id)
        {
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
        public Block Block { get; set; }
        [DataMember]
        public VillageParameters Parameters { get; set; }
    }

    [DataContract]
    public class VillageParameters
    {
        [DataMember]
        public int NoOfHouses { get; set; }
        [DataMember]
        public Dictionary<Gender, int> Population { get; set; }
        [DataMember]
        public Dictionary<Gender, int> Children { get; set; }
        [DataMember]
        public Dictionary<Gender, int> SCs { get; set; }
        [DataMember]
        public Dictionary<Gender, int> STs { get; set; }
        [DataMember]
        public Dictionary<Gender, int> Literacy { get; set; }
        [DataMember]
        public Dictionary<Gender, int> TotalWorkers { get; set; }
        [DataMember]
        public int MainWorkers { get; set; } // Earning 6 or more months
        [DataMember]
        public int MarginalWorkers { get; set; } // Less than 6 months earning
    }

    [DataContract]
    public class CensusTownParams
    {
        [DataMember]
        public int NoOfHouses { get; set; }
        [DataMember]
        public Dictionary<Gender, int> Population { get; set; }
        [DataMember]
        public Dictionary<Gender, int> Children { get; set; }
        [DataMember]
        public Dictionary<Gender, int> Literacy { get; set; }

        [DataMember]
        public double SCs { get; set; } // %
        [DataMember]
        public double STs { get; set; } // %

        [DataMember]
        public Dictionary<Gender, int> Workers { get; set; } // Worker: somone who does business,job,service,cultivator or labor
        [DataMember]
        public double MainWorkers { get; set; } // %
        public double MarginalWorkers { get; set; } // %
    }

    [DataContract]
    public class Ward : RegionWithId
    {
        [DataMember]
        public MunicipalCorp MunicipalCorp { get; set; }
    }

    [DataContract]
    public class CensusTown : RegionWithId
    {
        [DataMember]
        public CensusTownParams Params { get; set; }
        [DataMember]
        public Block Block { get; set; }        
    }

    [DataContract]
    public class MunicipalCorp : RegionWithId
    {
        [DataMember]
        public int TotalPopulation { get; set; }
        [DataMember]
        public HashSet<Ward> Wards { get; set; }
        [DataMember]
        public Block Block { get; set; }

        #region Add Ward Info

        public Ward AddWard(string name)
        {
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
        public Division Division { get; set; }
        [DataMember]
        public HashSet<Block> Blocks { get; set; }
        [DataMember]
        public HashSet<ParliamentaryConstituency> PCs { get; set; }

        public IEnumerable<AssemblyConstituency> ACs
        {
            get { return PCs.SelectMany(x => x.Constituencies); }
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

        #region Info Add methods

        public Block AddBlock(string name, int id)
        {
            if (!Blocks.Any(x => x.Name.Equals(name)))
            {
                var newBlock = new Block {District = this, Name = name, No = id};
                Blocks.Add(newBlock);
                return newBlock;
            }
            return null;
        }

        public void MergeDistrictInfo(District district)
        {
            if (Name == null && district.Name != null)
            {
                Name = district.Name;
            }
            if (No == 0 && district.No != 0)
            {
                No = district.No;
            }
            foreach (var constituency in district.PCs)
            {
                if (PCs == null)
                {
                    PCs = new HashSet<ParliamentaryConstituency> { constituency };
                }
                if (!PCs.Any(
                    x => x.Name == constituency.Name || (x.No != 0 && x.No == constituency.No)))
                {
                    constituency.District = this;
                    PCs.Add(constituency);

                    return;
                }
                PCs.First(
                    x => x.Name == constituency.Name || (x.No != 0 && x.No == constituency.No)).Merge(constituency);
            }
        }

        #endregion Info Add methods

    }

    [DataContract]
    public class Division : RegionWithId
    {
        [DataMember]
        public State State { get; set; }
        [DataMember]
        public HashSet<District> Districts { get; set; }

        public void MergeDistrictInfo(District district)
        {
            if (!Districts.Any(x => x.Name.Equals(district.Name)))
            {
                district.Division = this;
                Districts.Add(district);
                return;
            }
            Districts.First(x => x.Name.Equals(district.Name)).MergeDistrictInfo(district);
        }
    }

    [DataContract]
    public class State : Region
    {
        [DataMember]
        public HashSet<Division> Divisions { get; set; }
        public IEnumerable<District> Districts
        {
            get { return Divisions.SelectMany(x => x.Districts); }
        }

        public IEnumerable<ParliamentaryConstituency> PCs
        {
            get { return Districts.SelectMany(x => x.PCs); }
        }

        public IEnumerable<AssemblyConstituency> ACs
        {
            get { return PCs.SelectMany(x => x.Constituencies); }
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

        public void MergeDistrictInfo(District district)
        {
            Divisions.First(t => t.Districts.Any(x => x.Name.Equals(district.Name))).MergeDistrictInfo(district);
        }

        public void PopulateWithACInfoFromFile(string filename)
        {
            // TODO: Some names have (SC) in them.. Remove that from the name
            // Assumes all divisions are already created
            var acNameRegex = new Regex(@"([A-Za-z,\s]+)\(Vidhan Sabha constituency\)");
            string[] allLines = File.ReadAllLines(filename).Skip(1).ToArray();
            foreach (var line in allLines)
            {
                string[] cols = line.Split('\t');
                int acNo = Int32.Parse(cols[0]);
                string acName = acNameRegex.Match(cols[1].Trim()).Groups[1].Value.Split(',')[0];
                string acNameNormalized = Utils.GetNormalizedName(acName);
                var category = Utils.GetCategory(cols[2].Replace("None", "Gen"));
                string districtName = Utils.GetNormalizedName(cols[3]);
                int pcNo = Int32.Parse(cols[4].Split(' ')[0]);
                string pcName = Utils.GetNormalizedName(String.Join(" ", cols[4].Split(' ').Skip(1)));

                var acConstituency = new AssemblyConstituency { Category = category, Name = acNameNormalized, No = acNo };
                var pc = new ParliamentaryConstituency
                {
                    Name = pcName,
                    No = pcNo,
                    Constituencies = new HashSet<AssemblyConstituency> { acConstituency }
                };
                var district = new District
                {
                    Name = districtName,
                    PCs = new HashSet<ParliamentaryConstituency> { pc },
                    Division = Divisions.First(x => x.Districts.Any(y => y.Name == districtName))
                };
                acConstituency.PC = pc;
                pc.District = district;
                MergeDistrictInfo(district);
            }
        }

        public static State LoadDivisionsFromFile(string filename)
        {
            var state = new State { Name = "Bihar", Divisions = new HashSet<Division>() };
            string[] allLines = File.ReadAllLines(filename).Skip(1).ToArray();
            int divisionId = 1;
            foreach (var line in allLines)
            {
                string[] cols = line.Split('\t');
                string districtName = Utils.GetNormalizedName(cols[0]);
                string divisionName = Utils.GetNormalizedName(cols[1]);

                if (!state.Divisions.Any(t => t.Name.Equals(divisionName)))
                {
                    var division = new Division { Name = divisionName, Districts = new HashSet<District>(), State = state, No = divisionId };
                    state.Divisions.Add(division);
                    divisionId++;
                }
                var div = state.Divisions.First(t => t.Name == divisionName);
                div.Districts.Add(new District { Name = Utils.GetNormalizedName(districtName), Division = div });
            }
            return state;
        }
    }



    #endregion Common Hierarchy

    #region generic

    [DataContract]
    public abstract class RegionWithId : Region
    {
        [DataMember]
        public int No { get; set; }
    }

    [DataContract]
    public abstract class Region
    {
        [DataMember]
        public string Name { get; set; }
    }

    #endregion generic
}
