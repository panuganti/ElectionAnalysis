using System;
using System.Collections.Generic;
using System.IO;
using BiharElectionsLibrary;

namespace QualitativeDataAnalysis
{
    public class PreviousElectionResults
    {
        public static List<Result> Load2005Results(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var prevConstituencyName = "";
            var results = new List<Result>();
            foreach (var line in lines)
            {
                var newResult = new Result();
                string[] cols = line.Split('\t');
                if (cols[1].Equals(prevConstituencyName))
                {
                    newResult.YearOfElection = Int32.Parse(cols[0]);
                }
                else
                {
                    newResult.YearOfElection = 2010;
                    newResult.CandidateVotes = new Dictionary<Candidate2005, int>
                    {
                        {
                            new Candidate2005
                            {
                                Name = cols[3],
                                Gender = (Gender) Enum.Parse(typeof (Gender), cols[4]),
                                Party = Utils.GetParty(cols[5])
                            },
                            Int32.Parse(cols[6])
                        },
                        {
                            new Candidate2005
                            {
                                Name = cols[7],
                                Gender = (Gender) Enum.Parse(typeof (Gender), cols[8]),
                                Party = Utils.GetParty(cols[9])
                            },
                            Int32.Parse(cols[10])
                        }
                    };
                }
                newResult.ConstituencyName = cols[1];
                prevConstituencyName = cols[1];
                results.Add(newResult);
            }
            return results;
        }
    }

    public class Result
    {
        public int YearOfElection { get; set; }

        public int AcNo { get; set; }

        public string ConstituencyName { get; set; }

        public Dictionary<Candidate2005, int> CandidateVotes { get; set; }


    }

    public class Candidate2005
    {
        public string Name { get; set; }

        public Gender Gender { get; set; }

        public PoliticalParty Party { get; set; }

        public int Votes { get; set; }
    }
}
