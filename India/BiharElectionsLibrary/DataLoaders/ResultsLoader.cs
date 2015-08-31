using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BiharElectionsLibrary
{
    public static class ResultsLoader
    {
        public static List<PCResult> Load2015PCResults(string dirPath, State state)
        {
            var results = new List<PCResult>();
            for(int pcNo=1; pcNo<=40; pcNo++)
            {
                var result = new PCResult();
                string fileName = Path.Combine(dirPath,String.Format("{0}.tsv",pcNo));
                string[] lines = File.ReadAllLines(fileName);
                string[] headers = lines[0].Split('\t');
                result.Constituency = state.PCs.First(t => t.No == pcNo);
                var candidateVotes = new List<CandidateVotes>();
                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split('\t');
                    candidateVotes.Add(new CandidateVotes() { 
                            Votes = int.Parse(parts[2], NumberStyles.AllowThousands), 
                            Candidate = new Candidate { Name = parts[1], Party = Utils.GetParty(parts[4]) } 
                    });
                }
                result.Votes = candidateVotes;
                result.YearOfElection = 2015;
                results.Add(result);
            }
            return results;
        }


        #region Load 2010 Results
        public static List<ACResult> Load2010ResultsFromFile(string filename)
        {
            var all2010Results = new List<ACResult>();
            string[] resultInfo = File.ReadAllLines(filename);
            for (int acNo = 1; acNo <= 243; acNo++)
            {
                all2010Results.Add(Load2010ElectionResults(
                        resultInfo.Skip(1).Where(t => Int32.Parse(t.Split('\t')[5]) == acNo).ToArray()));
            }
            return all2010Results;
        }

        public static ACResult Load2010ElectionResults(string[] constituencyResults)
        {
            var result = new ACResult();
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
                    result.Votes = new List<CandidateVotes>();
                    first = false;
                }
                result.Votes.Add(new CandidateVotes
                {
                    Candidate = new Candidate
                    {
                        Name = Utils.GetNormalizedName(columns[8]),
                        YearOfBirth = 2010 - Int32.Parse(columns[11]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), columns[9]),
                        ConstituencyCasteCategory = Utils.GetCategory(columns[10]),
                        Party = Utils.GetParty(columns[12]),
                    },
                    Votes = Int32.Parse(columns[13])
                });
            }
            return result;
        }

        #endregion Load 2010 Results

        #region Load 2005 Results
        public static List<ACResult> Load2005ResultsForAConstituency(string[] constituencyResults)
        {
            var results = new List<ACResult>();
            bool first = true;
            int acNo = 0;
            foreach (var constituencyResult in constituencyResults)
            {
                var newResult = new ACResult();
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

                newResult.Votes = new List<CandidateVotes>
                {
                    new CandidateVotes {
                        Candidate = new Candidate
                        {
                            Name = Utils.GetNormalizedName(cols[3]),
                            Gender = Utils.GetGender(cols[4]),
                            Party = Utils.GetParty(cols[5])
                        },
                        Votes = Int32.Parse(cols[6])
                    },
                    new CandidateVotes 
                    {
                        Candidate = new Candidate
                        {
                            Name = Utils.GetNormalizedName(cols[7]),
                            Gender = Utils.GetGender(cols[8]),
                            Party = Utils.GetParty(cols[9])
                        },
                        Votes = Int32.Parse(cols[10])
                    }
                };
                results.Add(newResult);
            }
            return results;
        }

        public static List<ACResult> Load2005ResultsFromFile(string filename)
        {
            var all2005Results = new List<ACResult>();
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

        public static List<ACResult> Load2014ResultsFromFile(string dirPath)
        {
            var results2014 = new List<ACResult>();

            var allFiles = Directory.GetFiles(dirPath);
            foreach (var file in allFiles)
            {
                var allLines = File.ReadAllLines(file);
                var candidatesCols = allLines[1].Split('\t');
                var candidates = allLines[1].Split('\t').Skip(1).Take(candidatesCols.Count() - 6).ToArray();
                var allResultLines = allLines.Skip(3).Take(allLines.Count() - 6);
                foreach (var resultLine in allResultLines)
                {
                    if (resultLine.StartsWith("Total")) { break; }
                    var cols = resultLine.Split('\t').Select(x => x.Trim('"')).ToArray();

                    var assemblyResult = new ACResult()
                    {
                        Constituency =
                            new AssemblyConstituency()
                            {
                                No = Int32.Parse(cols[0].Split(' ')[0]),
                                Name = Utils.GetNormalizedName(String.Join(" ", cols[0].Split(' ').Skip(1)))
                            },
                        NOTA = Int32.Parse(cols[cols.Length - 3]),
                        YearOfElection = 2014,
                        Votes = new List<CandidateVotes>(),
                        TotalVotes = Int32.Parse(cols[cols.Length - 1])
                    };
                    var colsWithVotes = cols.Skip(1).Take(cols.Length - 6).ToArray();
                    for (int i = 0; i < colsWithVotes.Length; i++)
                    {
                        var candidateVote = new CandidateVotes()
                        {
                            Candidate = new Candidate() { Name = Utils.GetNormalizedName(candidates[i]) },
                            Votes = Int32.Parse(colsWithVotes[i])
                        };
                        assemblyResult.Votes.Add(candidateVote);
                    }
                    results2014.Add(assemblyResult);
                }
            }

            return results2014;
        }


        #endregion Load 2014 Results

        #region Load AcWise results from IndiaVotes Data

        public static List<ACResult> LoadResultsFromIndiaVotesData(string dirPath, int year)
        {
            var results = new List<ACResult>();
            var allFiles = Directory.GetFiles(dirPath);
            foreach (var filename in allFiles)
            {
                var acPcName = Path.GetFileNameWithoutExtension(filename);
                var acName = acPcName.Split(' ')[0];
                var pcName = acPcName.Split(' ')[1];
                var lines = File.ReadAllLines(filename).Skip(1);
                var acResult = new ACResult
                {
                    YearOfElection = year,
                    Constituency =
                        new AssemblyConstituency {Name = acName, PC = new ParliamentaryConstituency {Name = pcName}},
                    Votes = new List<CandidateVotes>()
                };
                foreach (var line in lines)
                {
                    var parts = line.Split('\t');
                    acResult.Votes.Add(new CandidateVotes()
                    {
                        Candidate = new Candidate {Name = parts[0], Party = Utils.GetParty(parts[1])},
                        Votes = int.Parse(parts[2], NumberStyles.AllowThousands)
                    });
                }
                results.Add(acResult);
            }
            return results;
        }

        #endregion Load AcWise results from IndiaVotes Data


        #region Load AcWise results from IndiaVotes Data

        public static List<ACResult> LoadACResultsFromIndiaVotesData(string dirPath, int year)
        {
            var results = new List<ACResult>();
            var allFiles = Directory.GetFiles(dirPath);
            foreach (var filename in allFiles)
            {
                var acPcName = Path.GetFileNameWithoutExtension(filename);
                var acName = acPcName.Split(' ')[0];
                var district = acPcName.Split(' ')[1].Split('(')[0];
                var lines = File.ReadAllLines(filename).Skip(1);
                var acResult = new ACResult
                {
                    YearOfElection = year,
                    Constituency =
                        new AssemblyConstituency { Name = acName, District = new District { Name = district } },
                    Votes = new List<CandidateVotes>()
                };
                foreach (var line in lines)
                {
                    var parts = line.Split('\t');
                    acResult.Votes.Add(new CandidateVotes()
                    {
                        Candidate = new Candidate { Name = parts[1], Party = Utils.GetParty(parts[4]) },
                        Votes = int.Parse(parts[2], NumberStyles.AllowThousands),
                        Position = int.Parse(parts[0])
                    });
                }
                results.Add(acResult);
            }
            return results;
        }

        #endregion Load AcWise results from IndiaVotes Data

    }
}
