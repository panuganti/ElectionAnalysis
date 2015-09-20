using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tweetinvi.Core.Extensions;

namespace USElectionAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            var twitter = TwitterCommunicator.GetTwittercommunicator();
            GetRepublicanHandles(twitter);
        }

        public static void GetTweetsForSearch(TwitterCommunicator twitter)
        {
            var tweets = twitter.SearchForTweetStrings("Gun Control", 9000);
            File.WriteAllText("./Tweets.txt", String.Join("\n", tweets.ToArray()));            
        }

        public static void GetRepublicanHandles(TwitterCommunicator twitter)
        {
            const string republicanCandidates = @"I:\ArchishaData\ElectionData\US\RepublicanCandidates.txt";
            const string output = @"I:\ArchishaData\ElectionData\US\RepublicanHandles.txt";
            string[] candidates = File.ReadAllLines(republicanCandidates);
            var handles = new List<string>();
            var writer = new StreamWriter(output,append:true);
            int startCount = 10;
            int endcount = 12;
            int count = 0;
            foreach (var candidate in candidates)
            {
                if (count <= startCount)
                {
                    count++;
                    continue;
                }
                var tweets = twitter.SearchForTweets(candidate).ToArray();
                var tweetsInfoForCandidate = tweets.Select(
                    t => String.Format("{0}\t{1}\t{2}", t.CreatedBy.Id, t.CreatedBy.ScreenName, t.CreatedBy.FollowersCount)).ToArray();
                handles.AddRange(tweetsInfoForCandidate);
                tweetsInfoForCandidate.Distinct().ForEach(writer.WriteLine);
                writer.Flush();
                if (count >= endcount) { break;}
                count++;
            }
            writer.Close();
        }

        public static void GetHandlesTweetingAboutQuery(string candidate, TwitterCommunicator twitter)
        {
            var tweets = twitter.SearchForTweetStrings("Gun Control", 9000);            
        }
    }
}
