using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KarnatakaElections
{
    public class Form20
    {
        public static void ParseAndCleanupForm20CSV(string infile, string outfile)
        {
            var lines = File.ReadAllLines(infile);

            //Lines not starting with a #, merge with previous line
            var fixedLines = FixLines(lines).ToArray();
            var tableLines = GetTableLines(fixedLines);
            var candidates = GetCandidateNames(fixedLines);
            var constituencyName = GetConstituencyName(fixedLines);
            var acid = infile;
            IEnumerable<PollingBooth> booths = FormatPollingBooth(constituencyName, acid, candidates.ToArray(), tableLines);
            booths.ToList().ForEach(b =>
            {
                File.WriteAllText(Path.Combine(outfile,string.Format("{0}_{1}_{2}.json",b.BoothId, b.ACId, b.AssemblyConstituency)), JsonConvert.SerializeObject(b, Formatting.Indented));
            });
        }

        private static IEnumerable<PollingBooth> FormatPollingBooth(string constituencyName, string acid, string[] candidates, IEnumerable<string> table)
        {
            return table.Select(l =>
            {
                var booth = new PollingBooth();
                var parts = l.Split('\t');
                booth.BoothId = parts[0];
                var boothCandidates =
                    candidates.Select((t, i) => new AssemblyCandidate() {Name = t, Votes = int.Parse(parts[i + 1])})
                        .ToList();
                booth.Candidates = boothCandidates;
                booth.ACId = acid;
                booth.AssemblyConstituency = constituencyName;
                var votes = parts.Skip(1).Take(candidates.Length).OrderByDescending(int.Parse);
                booth.MarginOfVictory = int.Parse(votes.First()) - int.Parse(votes.Skip(1).First());
                return booth;
            });
        }

        private static string GetConstituencyName(IEnumerable<string> lines)
        {
            return null;
        }

        private static IEnumerable<string> GetCandidateNames(IEnumerable<string> lines)
        {
            return null;
        }

        private static IEnumerable<string> GetTableLines(IEnumerable<string> lines)
        {
            // Lines that are of same length and all numbers
            int x;
            var numberLines = lines.Where(l => l.Split('\t').All(p => int.TryParse(p, out x))).ToArray();
            var colsCount = numberLines.Select(l => l.Split('\t').Length).GroupBy(p => p).OrderByDescending(p => p.Count()).First().Key;
            return numberLines.Where(l => l.Split('\t').Length == colsCount);
        }

        private static IEnumerable<string> FixLines(string[] lines)
        {
            List<string> outlines = new List<string>();
            List<string> newLine = new List<string>();
            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                var parts = line.Split('\t').ToList();
                int x = 0;
                if (!int.TryParse(parts[0], out x))
                {
                    newLine.AddRange(parts);
                }
                else
                {
                    if (newLine.Any())
                    {
                        outlines.Add(string.Join("\t", newLine.ToArray()));
                        newLine.Clear();
                    }
                    newLine.AddRange(parts);
                }
            }
            if (newLine.Any())
            {
                outlines.Add(string.Join("\t", newLine.ToArray()));
                newLine.Clear();
            }
            return outlines;
        }


    }

    [DataContract]
    public class PollingBooth
    {
        [DataMember]
        public IEnumerable<AssemblyCandidate> Candidates { get; set; }
        [DataMember]
        public int MarginOfVictory { get; set; }
        [DataMember]
        public string AssemblyConstituency { get; set; }
        [DataMember]
        public string ACId { get; set; }
        [DataMember]
        public string BoothId { get; set; }
    }

    [DataContract]
    public class AssemblyCandidate
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Votes { get; set; }
    }
}
