using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Search = Tweetinvi.Search;

namespace USElectionAnalysis
{
    public class TwitterCommunicator
    {
        public IEnumerable<string> SearchForTweets(string query, int maxResults = 1000)
        {
            var tweets = new List<string>();
            var results = Search.SearchTweets(query);
            var maxId = results.Max(t => t.Id);
            int count = 0;
            while (count < maxResults)
            {
                var searchParams = Search.CreateTweetSearchParameter(query);
                searchParams.MaximumNumberOfResults = 1000;
                searchParams.MaxId = maxId;
                results = Search.SearchTweets(searchParams);
                if (results.Count() == 0) { break; }
                tweets.AddRange(results.Select(t => t.Text.Replace("\n", " ")));
                count = tweets.Distinct().Count();
                Console.WriteLine("Count:{0}", count);
                maxId = results.Min(t => t.Id);
            }
            return tweets.Distinct(); ;
        }

        public static TwitterCommunicator GetTwittercommunicator()
        {
            string consumerKey = "Jppu6DRTeDkAt0mia8jDsg";
            string consumerSecret = "v1phw2c0jErxVDJzN8JjYEY16rJhSddYPHQvk4B6Mvs";
            string accessToken = "20416161-RCyzGIEokjVTEkNoT27bNVi0iV521nE980D9Rac1E";
            string accessTokenSecret = "kjjOMIGbPHaSaZqh4vbchnAlAgvz4fbN2QwrpmB7boU";

            TwitterCredentials.SetCredentials(accessToken, accessTokenSecret, consumerKey, consumerSecret);
            return new TwitterCommunicator();
        }
    }
}
