using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AnalyzeJudgements
{
    class Program
    {
        private static void Main(string[] args)
        {
            // 1.  Load Judgements
            const string filename = @"E:\NMW\GitHub\ElectionAnalysis\US\Judgements\Judgements.tsv";
            const string judgementsFile = @"E:\NMW\GitHub\ElectionAnalysis\US\Judgements\AgreedJudgements.tsv";
            const string disagreementsFile = @"E:\NMW\GitHub\ElectionAnalysis\US\Judgements\DisagreedJudgements.tsv";

            var judgements = ReadJudgements(filename);
            ConflateJudgements(judgements, judgementsFile, disagreementsFile);
            // 1.5 Conflate judgements

            // 2. Load corresponding profiles & all tweets for that profile

            // 3. Generate tweetText\tJugement file

            // 4. Filter out good data (we need to slowly expand this to 100%)

            // 5. Write ExtractioinFile (+ve/-ve) 
        }

        private static void ConflateJudgements(IEnumerable<ProfileJudgement> rawJudgements, string judgementsFile, string disagreementsFile)
        {
            var writer = new StreamWriter(judgementsFile);
            var disagreedJudgements = new StreamWriter(disagreementsFile);
            var judgementsByProfile = rawJudgements.GroupBy(t => t.Profile);
            foreach (var profileJudgement in judgementsByProfile)
            {
                var nJudgements = profileJudgement.Count();
                if (nJudgements == 1)
                {
                    foreach (var tweetJudgement in profileJudgement.First().TweetJudgements)
                    {
                        // Modify it to use tweet\tjudgement\tfeatures
                        writer.WriteLine(tweetJudgement);
                    }
                }
                if (nJudgements == 2)
                {
                    var firstJudgement = profileJudgement.First();
                    var secondJudgement = profileJudgement.Skip(1).First();
                    var min = Math.Min(firstJudgement.TweetJudgements.Count, secondJudgement.TweetJudgements.Count);
                    for (int i = 0; i < min; i++)
                    {
                        if (firstJudgement.TweetJudgements[i] == secondJudgement.TweetJudgements[i])
                        {
                            // Modify it to use tweet\tjudgement\tfeatures
                            writer.WriteLine(firstJudgement.TweetJudgements[i]);
                        }
                        else
                        {
                            disagreedJudgements.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", firstJudgement.Judge,
                                firstJudgement.Profile, firstJudgement.TweetJudgements[i], 
                                secondJudgement.Judge, secondJudgement.Profile, secondJudgement.TweetJudgements[i]);
                        }
                    }
                }
            }
            writer.Close();
            disagreedJudgements.Close();
        }

        private static IEnumerable<ProfileJudgement> ReadJudgements(string filename)
        {
            var profileJudgements = new List<ProfileJudgement>();
            var allLines = File.ReadAllLines(filename);
            foreach (var line in allLines)
            {
                var parts = line.Split('\t');
                var judgements = parts[4].Split(';').Skip(1).ToList();
                var judgement = new ProfileJudgement
                {
                    Judge = parts[0],
                    Profile = parts[1],
                    PicJudgement = parts[2],
                    TweetJudgements = judgements
                };
                profileJudgements.Add(judgement);
            }
            return profileJudgements;
        }
    }
}
