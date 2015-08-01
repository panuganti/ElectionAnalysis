using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USElectionAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            var twitter = TwitterCommunicator.GetTwittercommunicator();
            var tweets = twitter.SearchForTweets("Gun Control", 9000);
            File.WriteAllText("./Tweets.txt", String.Join("\n",tweets.ToArray()));
        }
    }
}
