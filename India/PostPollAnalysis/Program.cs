using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiharElectionsLibrary;
using Newtonsoft.Json;

namespace PostPollAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            var results2015 = LoadCorrectness();
            File.WriteAllText(@"I:\ArchishaData\ElectionData\Bihar\Website\correctness2015AcWise.json", JsonConvert.SerializeObject(results2015, Formatting.Indented));
            /*
             * Post Mortem:
Correlation between yadav votes & rjd wins
Correlation between muslim pop & wins
Correlation between dalit pop & wins
yadav + muslim to wins
dalit + uch & wins

Seat distribution:
vote % to rjd, jdu split
dalit % to bjp, dalit parties split
candidate caste to majority group correlation
anti-incumbency … how many each party replaced & where
repeat candidates performance
2014 voting preference to 2015 correlation
2014 won..did not contest 2009 but, still won.. (for bjp & jdu)


Sitting candidates who lost .. (anti-incumbency)
Simple arithmetic w.r.t 2014 .. seats where bjp alliance > rjd + jdu + cong and won etc..
             */
        }

        public static IEnumerable<Result> Load2015Results()
        {
            string resultsFile = @"I:\ArchishaData\ElectionData\RawData\BiharResults.tsv";
            return File.ReadAllLines(resultsFile).Skip(1).Select(x =>
            {
                var parts = x.Split('\t');
                var result = new Result {Id = int.Parse(parts[0]), Name = parts[2], Votes = new List<CandidateVote>() };
                result.Votes.Add(new CandidateVote {Name = parts[3], Party = (PoliticalParty)Enum.Parse(typeof(PoliticalParty),parts[4].ToLower()), Votes = int.Parse(parts[5]), Position = 1});
                result.Votes.Add(new CandidateVote { Name = parts[6], Party = (PoliticalParty)Enum.Parse(typeof(PoliticalParty), parts[7].ToLower()), Votes = int.Parse(parts[8]), Position = 2 });
                return result;
            });
        }

        public static IEnumerable<Result> LoadCorrectness()
        {
            string resultsFile = @"I:\ArchishaData\ElectionData\RawData\BiharPredictionAccuracy.tsv";
            return File.ReadAllLines(resultsFile).Skip(1).Select(x =>
            {
                var parts = x.Split('\t');
                var result = new Result { Id = int.Parse(parts[0]), Name = parts[1], Votes = new List<CandidateVote>() };
                result.Votes.Add(new CandidateVote { Name = parts[8], Party = parts[16].Equals("Correct") ? PoliticalParty.rjd : PoliticalParty.cpi, Votes = int.Parse(parts[10]), Position = 1 });
                result.Votes.Add(new CandidateVote { Name = parts[11], Party = PoliticalParty.others, Votes = int.Parse(parts[13]), Position = 2 });
                return result;
            });            
        }

        public static void CheckArithmeticTheory()
        {
            /* 1. BJP alliance votes in 2014
             * 2. UPA alliance votes sum-up in 2014
             * Check Wins vs Losses correlation
             */

        }
    }
}
