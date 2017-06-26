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
        public static void ParseAndCleanupForm20CSV(string infile, string outdir)
        {
            try
            {
                var lines = File.ReadAllLines(infile);

                //Lines not starting with a #, merge with previous line
                var fixedLines = FixLines(lines).ToArray();
                var tableLines = GetTableLines(fixedLines).ToArray();
                var candidates = GetCandidateNames(fixedLines).ToArray();
                //var constituencyName = GetConstituencyName(fixedLines);
                var acid = Path.GetFileNameWithoutExtension(infile).Split('_')[1];
                IEnumerable<PollingBooth> booths = FormatPollingBooth(acid, candidates.ToArray(), tableLines).ToArray();
                booths.ToList().ForEach(b =>
                {
                    File.WriteAllText(Path.Combine(outdir, string.Format("{0}_{1}.json", b.BoothId, b.ACId)), JsonConvert.SerializeObject(b, Formatting.Indented));
                });
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed {0}", Path.GetFileNameWithoutExtension(infile));
            }
        }

        private static IEnumerable<PollingBooth> FormatPollingBooth(string acid, string[] candidates, IEnumerable<string> table)
        {
            return table.Select(l =>
            {
                var booth = new PollingBooth();
                var parts = l.Split('\t');
                booth.BoothId = parts[2];
                var boothCandidates =
                    candidates.Select((t, i) => new AssemblyCandidate() {Name = t, Votes = int.Parse(parts[i + 1])})
                        .ToList();
                booth.Candidates = boothCandidates;
                booth.ACId = acid;
                var votes = parts.Skip(1).Take(candidates.Length).OrderByDescending(int.Parse);
                booth.MarginOfVictory = int.Parse(votes.First()) - int.Parse(votes.Skip(1).First());
                return booth;
            });
        }

        private static string GetConstituencyName(IEnumerable<string> lines)
        {
            var indexSlNo = lines.Select((v, i) => new { v = v, index = i }).First(l => l.v.Split('\t').ToArray()[1].Replace(" ", "").Equals("Sl.No")).index;
            var name = lines.ToArray()[indexSlNo - 1].Trim('\t').Split('\t')[1].Split(':')[1].Trim();
            return name;
        }

        private static IEnumerable<string> GetCandidateNames(IEnumerable<string> lines)
        {
            int tempI;
            //            var indexSlNo = lines.Select((v, i) => new { v = v, index = i }).First(l => l.v.Split('\t').ToArray()[1].Replace(" ","").Equals("Sl.No")).index;
            var tableStartIndex = lines.Select((v, i) => new { v = v, index = i }).First(l => l.v.Split('\t').ToArray()[1].Equals("1")).index;
            /*
            if (tableStartIndex - indexSlNo != 2)
            {
                throw new Exception();
            }
            */
            var candidatesLine = lines.ToArray()[tableStartIndex - 1];
            var candidates = candidatesLine.Split('\t').Where(x => !string.IsNullOrEmpty(x)).Where(x => !int.TryParse(x, out tempI)).ToArray();
            return candidates;
        }

        private static IEnumerable<string> GetTableLines(IEnumerable<string> lines)
        {
            // Lines that are of same length and all numbers
            int x;
            var numberLines = lines.Where(l => {
                var ll = l.Split('\t').Where(p => !string.IsNullOrEmpty(p));
                var z = ll.All(p =>
                {
                    var b = int.TryParse(p, out x);
                    return b;
                    }
                );
                return z;
                }).ToArray();
            var colsCount = numberLines.Select(l => l.Split('\t').Length).GroupBy(p => p).OrderByDescending(p => p.Count()).First().Key;
            return numberLines.Where(l => l.Trim('\t').Split('\t').Length == colsCount);
        }

        private static IEnumerable<string> FixLines(string[] lines)
        {
            List<string> outlines = new List<string>();
            string newLine = "";
            string oldLine = lines[0];
            int x = 0;
            int lineNo = 1;
            for(int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                var parts = line.Split('\t').ToList();
                if (!int.TryParse(parts[0], out x))
                {
                    oldLine += line;
                }
                else
                {
                    if (x == lineNo + 1)
                    {
                        outlines.Add(oldLine);
                        oldLine = line;
                        lineNo++;
                    }
                    else
                    {
                        oldLine += line;
                    }
                }
            }
            if (!oldLine.Equals(""))
            {
                outlines.Add(oldLine);
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
