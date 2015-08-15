using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace FixFormattingOfTweetJudgements
{
    class Program
    {
        static void Main(string[] args)
        {
            var tweetIdRegex = new Regex(@"([0-9]+)");
            List<string> outLines = new List<string>();
            var lines = File.ReadAllLines(@"J:\ArchishaData\ElectionData\US\GunControlTweets.tsv").Skip(1);
            foreach (var line in lines)
            {
                var parts = line.Split('\t');
                if (parts[0].Equals(String.Empty) || tweetIdRegex.IsMatch(parts[0])) { continue; }
                if (parts[0].Contains(","))
                {
                    var judgements = parts[0].Trim(new [] {'\"'}).Split(',');
                    foreach (var judgement in judgements)
                    {
                        outLines.Add(String.Format("{0},{2}", judgement.Trim(), parts[1], parts[2].Replace(","," ")));
                    }
                }
                else {
                    outLines.Add(String.Format("{0},{2}", parts[0], parts[1], parts[2].Replace(","," ")));
                }
            }
            File.WriteAllLines(@"J:\ArchishaData\ElectionData\US\GunControlTweetsInFormat.tsv", outLines);
        }
    }
}
