using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace BiharElectionsLibrary
{
    [DataContract]
    public class AssemblyConstituencyResult : ConstituencyResult
    {
        [DataMember]
        public AssemblyConstituency Constituency { get; set; }

        #region Load 2010 Results
        public static List<AssemblyConstituencyResult> Load2010ResultsFromFile(string filename)
        {
            var all2010Results = new List<AssemblyConstituencyResult>();
            string[] resultInfo = File.ReadAllLines(filename);
            for (int acNo = 1; acNo <= 243; acNo++)
            {
                all2010Results.Add(Load2010ElectionResults(
                        resultInfo.Skip(1).Where(t => Int32.Parse(t.Split('\t')[5]) == acNo).ToArray()));
            }
            return all2010Results;
        }

        public static AssemblyConstituencyResult Load2010ElectionResults(string[] constituencyResults)
        {
            var result = new AssemblyConstituencyResult();
            bool first = true;
            foreach (
                string[] columns in constituencyResults.Select(constituencyResult => constituencyResult.Split('\t')))
            {
                if (first)
                {
                    result.YearOfElection = 2010;
                    result.Constituency = new AssemblyConstituency
                    {
                        No = Int32.Parse(columns[5]),
                        Name = Utils.GetNormalizedName(columns[6]),
                        Category = Utils.GetCategory(columns[7])
                    };
                    result.Votes = new Dictionary<Candidate, int>();
                    first = false;
                }
                result.Votes.Add(
                    new Candidate
                    {
                        Name = Utils.GetNormalizedName(columns[8]),
                        YearOfBirth = 2010 - Int32.Parse(columns[11]),
                        Gender = (Gender) Enum.Parse(typeof (Gender), columns[9]),
                        ConstituencyCasteCategory = Utils.GetCategory(columns[10]),
                        Party = Utils.GetParty(columns[12]),
                    }, Int32.Parse(columns[13]));
            }
            return result;
        }

        #endregion Load 2010 Results

        #region Load 2005 Results
        public static List<AssemblyConstituencyResult> Load2005ResultsForAConstituency(string[] constituencyResults)
        {
            var results = new List<AssemblyConstituencyResult>();
            bool first = true;
            int acNo = 0;
            foreach (var constituencyResult in constituencyResults)
            {
                var newResult = new AssemblyConstituencyResult();
                string[] cols = constituencyResult.Split('\t');

                if (first)
                {
                    acNo = Int32.Parse(cols[0]);
                    newResult.YearOfElection = 2005;
                    first = false;
                }
                else
                {
                    newResult.YearOfElection = Int32.Parse(cols[0]);
                }
                newResult.Constituency = new AssemblyConstituency
                {
                    No = acNo,
                    Name = Utils.GetNormalizedName(cols[1]),
                    Category = Utils.GetCategory(cols[2])
                };

                newResult.Votes = new Dictionary<Candidate, int>
                {
                    {
                        new Candidate
                        {
                            Name = Utils.GetNormalizedName(cols[3]),
                            Gender = Utils.GetGender(cols[4]),
                            Party = Utils.GetParty(cols[5])
                        },
                        Int32.Parse(cols[6])
                    },
                    {
                        new Candidate
                        {
                            Name = Utils.GetNormalizedName(cols[7]),
                            Gender = Utils.GetGender(cols[8]),
                            Party = Utils.GetParty(cols[9])
                        },
                        Int32.Parse(cols[10])
                    }
                };
                results.Add(newResult);
            }
            return results;
        }

        public static List<AssemblyConstituencyResult> Load2005ResultsFromFile(string filename)
        {
            var all2005Results = new List<AssemblyConstituencyResult>();
            string[] resultInfo = File.ReadAllLines(filename).Skip(1).ToArray();
            bool first = true;
            string prevConstituencyName = "";
            var resultLinesForAConstituency = new List<string>();
            foreach (var resultLine in resultInfo)
            {
                string[] cols = resultLine.Split('\t');
                if (first)
                {
                    prevConstituencyName = Utils.GetNormalizedName(cols[1]);
                    first = false;
                }
                if (!prevConstituencyName.Equals(Utils.GetNormalizedName(cols[1])))
                {
                    all2005Results.AddRange(Load2005ResultsForAConstituency(resultLinesForAConstituency.ToArray()));
                    resultLinesForAConstituency = new List<string>();
                }
                resultLinesForAConstituency.Add(resultLine);
            }
            all2005Results.AddRange(Load2005ResultsForAConstituency(resultLinesForAConstituency.ToArray())); // Last set
            return all2005Results;
        }

        #endregion Load 2005 Results

        #region Load 2014 Results

        public static List<AssemblyConstituencyResult> Load2014ResultsFromFile(string dirPath)
        {
            var results2014 = new List<AssemblyConstituencyResult>();

            var allFiles = Directory.GetFiles(dirPath);
            foreach (var file in allFiles)
            {
                var allLines = File.ReadAllLines(file);
                var candidatesCols = allLines[1].Split('\t');
                var candidates = allLines[1].Split('\t').Skip(1).Take(candidatesCols.Count() - 6).ToArray();
                var allResultLines = allLines.Skip(3).Take(allLines.Count() - 6);
                foreach (var resultLine in allResultLines)
                {
                    if (resultLine.StartsWith("Total")) { break;}
                    var cols = resultLine.Split('\t').Select(x=> x.Trim('"')).ToArray();
                    
                    var assemblyResult = new AssemblyConstituencyResult()
                    {
                        Constituency =
                            new AssemblyConstituency()
                            {
                                No = Int32.Parse(cols[0].Split(' ')[0]),
                                Name = Utils.GetNormalizedName(String.Join(" ", cols[0].Split(' ').Skip(1)))
                            },
                        NOTA = Int32.Parse(cols[cols.Length - 3]),
                        YearOfElection = 2014,
                        Votes = new Dictionary<Candidate, int>(),
                        TotalVotes = Int32.Parse(cols[cols.Length - 1])
                    };
                    var colsWithVotes = cols.Skip(1).Take(cols.Length - 6).ToArray();
                    for (int i = 0; i < colsWithVotes.Length; i++)
                    {
                        assemblyResult.Votes.Add(new Candidate { Name = Utils.GetNormalizedName(candidates[i])}, Int32.Parse(colsWithVotes[i]));
                    }
                    results2014.Add(assemblyResult);
                }
            }

            return results2014;
        }


        #endregion Load 2014 Results
    }

    [DataContract]
    public class ParliamentaryConstituencyResult : ConstituencyResult
    {
        [DataMember]
        public ParliamentaryConstituency Constituency { get; set; }
    }

    [DataContract]
    public abstract class ConstituencyResult
    {
        [DataMember]
        public int YearOfElection { get; set; }
        [DataMember]
        public Dictionary<Candidate, int> Votes { get; set; }
        [DataMember]
        public int TotalVotes { get; set; }
        [DataMember]
        public int NOTA { get; set; }

        public Candidate GetWinner()
        {
            return Votes.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }

        public PoliticalParty GetWinningParty()
        {
            return GetWinner().Party;
        }
    }

    [DataContract]
    public class AssemblyElectionResults : ElectionResults
    {
        [DataMember]
        public List<AssemblyConstituencyResult> ConstituencyResults { get; set; }
    }

    [DataContract]
    public class ParliamentaryElectionResults : ElectionResults
    {
        [DataMember]
        public List<ParliamentaryConstituencyResult> ConstituencyResults { get; set; }
    }

    [DataContract]
    public abstract class ElectionResults
    {
        [DataMember]
        public int PrimaryYearOfElection { get; set; }        
    }
}
