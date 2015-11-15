using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces;
using Search = Tweetinvi.Search;

namespace USElectionAnalysis
{
    public class TwitterCommunicator
    {
        public IEnumerable<string> SearchForTweetStrings(string query, int maxResults = 100)
        {
            var tweets = new List<string>();
            ITweet[] results = Search.SearchTweets(query).ToArray();
            var maxId = results.Max(t => t.Id);
            int count = 0;
            while (count < maxResults)
            {
                var searchParams = Search.CreateTweetSearchParameter(query);
                searchParams.MaximumNumberOfResults = 100;
                searchParams.MaxId = maxId;
                results = Search.SearchTweets(searchParams).ToArray();
                if (!results.Any()) { break; }
                tweets.AddRange(results.Select(t => t.Text.Replace("\n", " ").Replace("\t"," ")));
                count += tweets.Distinct().Count();
                Console.WriteLine("Count:{0}", count);
                maxId = results.Min(t => t.Id);
            }
            return tweets.Distinct(); ;
        }

        public IEnumerable<ITweet> SearchForTweets(string query, int maxResults = 100)
        {
            try
            {
                List<ITweet> results = Search.SearchTweets(query).ToList();
                var maxId = results.Max(t => t.Id);
                int count = 0;
                while (count < maxResults)
                {
                    var searchParams = Search.CreateTweetSearchParameter(query);
                    searchParams.MaximumNumberOfResults = maxResults;
                    searchParams.MaxId = maxId;
                    var tweets = Search.SearchTweets(searchParams).ToArray();
                    if (!tweets.Any())
                    {
                        break;
                    }
                    results.AddRange(tweets);
                    count = tweets.Count();
                    Console.WriteLine("Count:{0}", count);
                    maxId = results.Min(t => t.Id);
                }
                return results;
            }
            catch (Exception)
            {
                Console.WriteLine("Exception hit.. waiting for 2 mins..");
                Task.WaitAll(Task.Delay(new TimeSpan(0,0,2,0)));
                return SearchForTweets(query, maxResults);
            }
        }

        public static TwitterCommunicator GetTwittercommunicator()
        {
            const string consumerKey = "Jppu6DRTeDkAt0mia8jDsg";
            const string consumerSecret = "v1phw2c0jErxVDJzN8JjYEY16rJhSddYPHQvk4B6Mvs";
            const string accessToken = "20416161-RCyzGIEokjVTEkNoT27bNVi0iV521nE980D9Rac1E";
            const string accessTokenSecret = "kjjOMIGbPHaSaZqh4vbchnAlAgvz4fbN2QwrpmB7boU";

            var creds = Auth.SetUserCredentials(consumerKey, consumerSecret,accessToken, accessTokenSecret);
            return new TwitterCommunicator();
        }
    }
}
