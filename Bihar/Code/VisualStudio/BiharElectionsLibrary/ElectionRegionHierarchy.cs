using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace BiharElectionsLibrary
{
  
    #region ElectionHierarchy

    public class AssemblyConstituency : Constituency
    {
        public HashSet<PollingBooth> Booths { get; set; }

        public ParliamentaryConstituency PC { get; set; }

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

    public class ParliamentaryConstituency : Constituency
    {
        public District District { get; set; }

        public HashSet<AssemblyConstituency> Constituencies { get; set; }

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

    public class Constituency : RegionWithId
    {
        public ConstituencyCasteCategory Category { get; set; }
    }
  
    public class PollingBooth : RegionWithId
    {
        public HashSet<House> Houses { get; set; }

        public AssemblyConstituency AC { get; set; }
    }

    public class House : RegionWithId
    {
        public PollingBooth Booth { get; set; }

        public HashSet<Voter> Voters { get; set; }
    }

    public class Voter : Person
    {
        public House House { get; set; }

        public int Age { get; set; }

        public List<Relative> Relatives { get; set; }
    }

    public class Person : Region
    {
        public Caste Caste { get; set; }

        public Gender Gender { get; set; }

    }

    public class Relative
    {
        public RelationType RelationType { get; set; }

        public Person Person { get; set; }
    }

    #endregion ElectionHierarchy

    #region AdminHierarchy

    public class Block : RegionWithId
    {
        public HashSet<Village> Villages { get; set; }
        public HashSet<CensusTown> CensusTowns { get; set; }

        public HashSet<MunicipalCorp> MinicipalCorp { get; set; } // Very likely, there would be only 1

        public District District { get; set; }
    }

    public class Village : RegionWithId
    {
        public Block Block { get; set; }

        public int TotalPopulation { get; set; }
        public int MalePopulation { get; set; }
        public int FemalePopulation { get; set; }

        public VillageParameters Parameters { get; set; }
    }

    public class VillageParameters
    {
        public int NoOfHouses { get; set; }
        public Dictionary<Gender, int> Population { get; set; }
        public Dictionary<Gender, int> Children { get; set; }
        public Dictionary<Gender, int> SCs { get; set; }
        public Dictionary<Gender, int> STs { get; set; }
        public Dictionary<Gender, int> Literacy { get; set; }
        public Dictionary<Gender, int> TotalWorkers { get; set; }
        public int MainWorkers { get; set; } // Earning 6 or more months
        public int MarginalWorkers { get; set; } // Less than 6 months earning
    }

    public class CensusTownParams
    {
        public int NoOfHouses { get; set; }
        public Dictionary<Gender, int> Population { get; set; }
        public Dictionary<Gender, int> Children { get; set; }
        public Dictionary<Gender, int> Literacy { get; set; }

        public double SCs { get; set; } // %
        public double STs { get; set; } // %

        public Dictionary<Gender, int> Workers { get; set; } // Worker: somone who does business,job,service,cultivator or labor
        public double MainWorkers { get; set; } // %
        public double MarginalWorkers { get; set; } // %
    }

    public class Ward : RegionWithId
    {
        public MunicipalCorp MunicipalCorp { get; set; }
    }

    public class CensusTown : RegionWithId
    {
        public CensusTownParams Params { get; set; }
        public Block Block { get; set; }
        
    }

    public class MunicipalCorp : RegionWithId
    {
        public int TotalPopulation { get; set; }
        public HashSet<Ward> Wards { get; set; }
        public Block Block { get; set; }
    }

    #endregion AdminHierarchy

    #region Common Hierarchy

    public class District : RegionWithId
    {
        public Division Division { get; set; }

        public HashSet<Block> Blocks { get; set; }

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
    }

    public class Division : RegionWithId
    {
        public State State { get; set; }
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

    public class State : Region
    {
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

    public abstract class RegionWithId : Region
    {
        public int No { get; set; }
    }

    public abstract class Region
    {
        public string Name { get; set; }
    }

    #endregion generic
}
