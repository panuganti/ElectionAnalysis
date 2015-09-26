using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
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
            const string outputDirPath = @"E:\NMW\GitHub\ElectionAnalysis\US\Judging\Profiles\";
            string[] candidates = File.ReadAllLines(republicanCandidates);
            var handles = new List<string>();
            var writer = new StreamWriter(output,append:true);
            const int startCount = 6;
            const int endcount = 10;
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
                tweets.ForEach(tweet => DownloadTimeline(tweet.CreatedBy.ScreenName, outputDirPath));
                handles.AddRange(tweetsInfoForCandidate);
                tweetsInfoForCandidate.Distinct().ForEach(writer.WriteLine);
                writer.Flush();
                if (count >= endcount) { break;}
                count++;
            }
            writer.Close();
        }

        public static void DownloadTimeline(string screenName, string outputDir)
        {
            WebRequest request = WebRequest.Create(String.Format("https://www.twitter.com/{0}", screenName));
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            var writer = new StreamWriter(Path.Combine(outputDir, String.Format("./{0}.html", screenName)));
            var htmlDoc = new HtmlDocument { OptionFixNestedTags = true };
            htmlDoc.Load(dataStream);
            /*
            var timelineNode = htmlDoc.GetElementbyId("timeline");
            var miniProfileCard =
                htmlDoc.DocumentNode.Descendants("div")
                    .First(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "ProfileCardMini");
            writer.WriteLine(timelineNode.OuterHtml);
            writer.WriteLine(miniProfileCard.OuterHtml);
             */
            writer.WriteLine(htmlDoc.DocumentNode.OuterHtml);
            response.Close();
            writer.Close();
        }

        public static void ModifyProfileDiv(HtmlNode node)
        {
        }

        public static void ModifyTimeline(HtmlNode node)
        {            
        }

        public static void GetHandlesTweetingAboutQuery(string candidate, TwitterCommunicator twitter)
        {
            var tweets = twitter.SearchForTweetStrings("Gun Control", 9000);            
        }
    }
}
