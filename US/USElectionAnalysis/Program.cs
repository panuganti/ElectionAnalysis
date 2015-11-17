using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;
using Newtonsoft.Json;

namespace USElectionAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            var twitter = TwitterCommunicator.GetTwittercommunicator();
            GetSentiment(twitter, "obama", 4000);
        }

        public static void GetSentiment(TwitterCommunicator twitter, string query, int count)
        {
            var tweets = twitter.SearchForTweets(query, count).ToArray();
            var tweetsByDate = tweets.GroupBy(x => new DateTime(x.CreatedAt.Year, x.CreatedAt.Month, x.CreatedAt.Day));
            var websiteData = new WebsiteData() {Summary = new List<Summary>(), Tweets = new List<Tweet>()};
            foreach (var tweetsOfADate in tweetsByDate)
            {
                var data = new RequestData { data = tweetsOfADate.Select(x => new Data { text = x.Text, time = tweetsOfADate.Key }).ToArray() };
                var daysSummary = ScrapeSentiment140.PostRequest(data);
                websiteData.Summary.AddRange(daysSummary.Summary);
                websiteData.Tweets.AddRange(daysSummary.Tweets);
            }
            File.WriteAllText(String.Format(@"I:\ArchishaData\ElectionData\US\\Sentiment140\\{0}_summary.txt", query), JsonConvert.SerializeObject(websiteData.Summary, Formatting.Indented));
            File.WriteAllText(String.Format(@"I:\ArchishaData\ElectionData\US\\Sentiment140\\{0}_tweets.txt", query), JsonConvert.SerializeObject(websiteData.Tweets, Formatting.Indented));
        }

        public static void GetTweetsForSearch(TwitterCommunicator twitter, string query, int count)
        {
            var tweets = twitter.SearchForTweetStrings(query, count);
            File.WriteAllText("./Tweets.txt", String.Join("\n", tweets.ToArray()));            
        }

        public static void GetRepublicanHandles(TwitterCommunicator twitter)
        {
            const string republicanCandidates = @"I:\ArchishaData\ElectionData\US\DemocraticCandidates.txt";
            const string output = @"I:\ArchishaData\ElectionData\US\DemocraticHandles.txt";
            const string outputDirPath = @"I:\ArchishaData\ElectionData\US\DemocraticProfiles\";
            string[] candidates = File.ReadAllLines(republicanCandidates);
            var handles = new List<string>();
            var writer = new StreamWriter(output,append:true);
            const int startCount = 9;
            const int endcount = 10;
            int count = 0;
            foreach (var candidate in candidates)
            {
                if (count <= startCount)
                {
                    count++;
                    continue;
                }
                ITweet[] tweets = twitter.SearchForTweets(candidate).ToArray();
                var tweetsInfoForCandidate = tweets.Select(
                    t => string.Format("{0}\t{1}\t{2}", t.CreatedBy.Id, t.CreatedBy.ScreenName, t.CreatedBy.FollowersCount));
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

    }
}
