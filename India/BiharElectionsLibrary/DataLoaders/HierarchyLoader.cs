using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BiharElectionsLibrary
{
    public class HierarchyLoader
    {
        public static State LoadPCsAndACs(string filename, State state)
        {
            // TODO: Some names have (SC) in them.. Remove that from the name
            // Assumes all divisions are already created
            var acNameRegex = new Regex(@"([A-Za-z,\s]+)\(Vidhan Sabha constituency\)");
            var pcNameRegex = new Regex(@"([0-9]+)\s([\w\s]+)(\(SC\)+){0,1}");
            string[] allLines = File.ReadAllLines(filename).Skip(1).ToArray();
            var data = allLines.Select(row =>
            {
                var cols = row.Split('\t');
                return new
                {
                    acNo = Int32.Parse(cols[0]),
                    acName = acNameRegex.Match(cols[1].Trim()).Groups[1].Value.Split(',')[0].Trim(),
                    category = Utils.GetCategory(cols[2].Replace("None", "Gen")),
                    districtName = cols[3],
                    pcNo = Int32.Parse(pcNameRegex.Match(cols[4]).Groups[1].Value),
                    pcName = pcNameRegex.Match(cols[4]).Groups[2].Value.Trim()
                };
            }).ToArray();
            // TODO: Output this data in json to be used on website

            state.PCs = new HashSet<ParliamentaryConstituency>(
                    data.Select(t => new {Name = t.pcName, No = t.pcNo}).Distinct()
                    .Select(t=>new ParliamentaryConstituency {Name = t.Name, No = t.No, ACs = new HashSet<AssemblyConstituency>()}).ToArray());            
            Console.WriteLine("No. of PCs: {0}",state.PCs.Count);
            
            // 1. for each ac, add their pc's, their dist
            foreach (var acData in data)
            {
                var pc = state.PCs.First(t => t.No == acData.pcNo);
                var dist = state.Districts.First(t => Utils.GetNormalizedName(t.Name) == Utils.GetNormalizedName(acData.districtName));
                var ac = new AssemblyConstituency { Name = acData.acName, No = acData.acNo, PC = pc, District = dist};
                pc.ACs.Add(ac);
                dist.ACs.Add(ac);
            }
            Console.WriteLine("No. of ACs: {0}", state.ACs.Count());
            return state;
        }

        public static State LoadDivisionsAndDistrictsFromFile(string filename, State state)
        {
            state.Divisions = new HashSet<Division>();
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
                div.Districts.Add(new District
                {
                    Name = Utils.GetNormalizedName(districtName),
                    Division = div,
                    Params = new DistrictParameters(),
                    ACs = new HashSet<AssemblyConstituency>()
                });
            }
            return state;
        }
    }
}
