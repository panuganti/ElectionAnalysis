using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using Google.GData.Client;
using HtmlAgilityPack;

namespace ParseAndFormatProfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseTweets();
        }

        static void FormatTweets()
        {
            const string tweetsDir = @"I:\ArchishaData\ElectionData\US\Tweets";
            const string outfile = @"I:\ArchishaData\ElectionData\US\FormattedTweets.tsv";
            var files = Directory.GetFiles(tweetsDir);
            File.WriteAllLines(outfile,files.SelectMany(x =>
            {
                var screenName = Path.GetFileNameWithoutExtension(x);
                int count = 0;
                return File.ReadAllLines(x).Select(y => { count++; return String.Format("{0}__{1}\t{2}", screenName, count, y); });
            }));
        }

        static void ParseJudgements()
        {
            const string judgementsFile = @"I:\ArchishaData\ElectionData\US\Judgements.tsv";
            const string outfile = @"I:\ArchishaData\ElectionData\US\JudgementsParsed.tsv";
            File.WriteAllLines(outfile, File.ReadAllLines(judgementsFile).SelectMany(x =>
            {
                var parts = x.Split('\t');
                var judgements = parts[4].Split(';').Skip(1);
                int count = 0;
                return judgements.Select(y => { count++; return String.Format("{0}__{1}\t{2}\t{3}", parts[1], count, parts[0], y); });
            }));
        }

        static void ParseTweets()
        {
            const string republicanProfilesDir = @"I:\ArchishaData\ElectionData\US\RepublicanProfiles";
            const string democraticProfilesDir = @"I:\ArchishaData\ElectionData\US\DemocraticProfiles";
            const string outputDir = @"D:\ArchishaData\ElectionData\US\Tweets\";
            ParseProfilesForTweets(republicanProfilesDir,outputDir);
            ParseProfilesForTweets(democraticProfilesDir, outputDir);
        }

        static void ModifyProfiles()
        {
            const string republicanProfilesDir = @"I:\ArchishaData\ElectionData\US\RepublicanProfiles";
            const string democraticProfilesDir = @"I:\ArchishaData\ElectionData\US\DemocraticProfiles";
            const string outputDir = @"E:\NMW\GitHub\ElectionAnalysis\US\Judging\profiles";
            const string profilesFile = @"E:\NMW\GitHub\ElectionAnalysis\US\Judging\allhandles.txt";
            ParseProfiles(republicanProfilesDir, outputDir, profilesFile, false);
            ParseProfiles(democraticProfilesDir, outputDir, profilesFile, true);            
        }

        static void ParseProfiles(string inputDir, string outputDir, string profilesFile, bool append = false)
        {
            int count = 0;
            var profilesWriter = new StreamWriter(profilesFile, append) {AutoFlush = true};
            foreach (var file in Directory.GetFiles(inputDir))
            {
                var filename = Path.GetFileName(file);
                var screenName = Path.GetFileNameWithoutExtension(file);
                var outfile = Path.Combine(outputDir, filename);
                try
                {
                    var writer = new StreamWriter(outfile);
                    var htmlDoc = new HtmlDocument { OptionFixNestedTags = true };
                    htmlDoc.Load(file);
                    var timelineNode = htmlDoc.GetElementbyId("timeline");
                    ModifyTimelineDiv(timelineNode);
                    var miniProfileCard =
                        htmlDoc.DocumentNode.Descendants("div")
                            .First(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "ProfileCanopy-navBar");
                    ModifyProfileDiv(miniProfileCard);
                    var profileBio =
                        htmlDoc.DocumentNode.Descendants("div")
                            .First(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "ProfileHeaderCard");
                    writer.WriteLine(miniProfileCard.OuterHtml);
                    writer.WriteLine(profileBio.OuterHtml);
                    writer.WriteLine(timelineNode.OuterHtml);
                    writer.Close();
                    profilesWriter.WriteLine(screenName);
                    count++; 
                    Console.WriteLine("count: {0}:", count);
                }
                catch (Exception)
                {
                    //File.Delete(outfile);
                }
            }
            profilesWriter.Close();
        }

        static void ModifyProfileDiv(HtmlNode node)
        {
            string format = "Overall Judgement: <select name=\"party\" ng-change=\"vMain.overallJudgementSelected()\" ng-model=\"vMain.partySelected\" ng-selected=\"vMain.NotRelevant\" ng-options=\"party for party in vMain.parties\" >{{party}}</select>";
            string genderHtml = "<br>Gender: <select name=\"gender\" ng-change=\"vMain.genderJudgementSelected()\" ng-model=\"vMain.genderSelected\" ng-selected=\"vMain.NotRelevant\" ng-options=\"gender for gender in vMain.genders\" >{{party}}</select>";
            node.InnerHtml = format + genderHtml + node.InnerHtml;            

        }

        static void ModifyTimelineDiv(HtmlNode node)
        {
            string format = "Tweet Judgement: <select name=\"party\" ng-model=\"vMain.tweetInclination[{0}]\" ng-change=\"vMain.tweetJudgementSelected()\" ng-selected=\"vMain.NotRelevant\" ng-options=\"party for party in vMain.parties\" >{{party}}</select>";
            var tweetNodesList = node.Descendants("li").Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value.Contains("js-stream-item stream-item stream-item expanding-stream-item")).ToArray();
            int count = 0;
            foreach (var tweetNode in tweetNodesList)
            {
                var imgNodes = tweetNode.Descendants("div").Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value.Contains(" is-preview"));
                if (imgNodes.Any())
                {
                    var imgNode = imgNodes.First();
                    var pattern = "data-img-src=([\"a-zA-Z0-9\\.\\/\\\":]+)";
                    var url = new Regex(pattern).Match(imgNode.InnerHtml).Groups[1].Value; 
                    imgNode.InnerHtml = String.Format("<img src={0}>", url);
                }
                tweetNode.InnerHtml = String.Format(format, count) + tweetNode.InnerHtml;
                count++;
            }
        }

        static void ParseProfilesForTweets(string inputDir, string outputDir)
        {
            int count = 0;
            foreach (var file in Directory.GetFiles(inputDir))
            {
                try
                {
                    var filename = Path.GetFileNameWithoutExtension(file);
                    var outfile = Path.Combine(outputDir, String.Format("{0}.txt",filename));
                    var htmlDoc = new HtmlDocument { OptionFixNestedTags = true };
                    htmlDoc.Load(file);
                    var timelineNode = htmlDoc.GetElementbyId("timeline");
                    File.WriteAllLines(outfile, ExtractTweets(timelineNode).Where(x=>!x.Text.Equals(String.Empty)).Select(x=>String.Format("{0}",x.Text)));
                    count++; Console.WriteLine("count: {0}:", count);
                }
                catch (Exception)
                {  
                }
            }
        }

        [DataContract]
        public class Tweet
        {
            [DataMember]
            public string Text { get; set; }
            [DataMember]
            public IEnumerable<string> Images { get; set; }
            [DataMember]
            public IEnumerable<string> Links { get; set; }
            [DataMember]
            public IEnumerable<string> Mentions { get; set; }
            [DataMember]
            public IEnumerable<string> HashTags { get; set; }
            [DataMember]
            public int RTs { get; set; }
            [DataMember]
            public int Favs { get; set; }
        }

        private static IEnumerable<Tweet> ExtractTweets(HtmlNode node)
        {
            var tweetNodes = node.Descendants("li")
                .Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value.Contains(
                    "js-stream-item stream-item stream-item expanding-stream-item"));
            var tweets = new List<Tweet>();
            var mentionPattern = "@([a-zA-Z0-9_]+)";
            var hashTagPattern = "#([a-zA-Z0-9_]+)";
            var urlPattern = @"(https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&\/\/=]*))";
            var imagePattern = @"(pic.twitter.com\/[a-zA-Z0-9]+)";
            foreach (var tweetNode in tweetNodes)
            {
                var tweetText =
                    tweetNode.Descendants("div")
                        .First(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "content")
                        .Descendants("p")
                        .First()
                        .InnerText.Replace('\n', ' ');
                // TODO: Extract mentions, pics, links, correct html encodes, popularity info.
                tweetText = HttpUtility.HtmlDecode(tweetText);
                var mentions = Regex.Matches(tweetText, mentionPattern).Cast<Match>().Select(x=>x.Value);
                var hashtags = Regex.Matches(tweetText, hashTagPattern).Cast<Match>().Select(x => x.Value);
                tweetText = Regex.Replace(tweetText, hashTagPattern, String.Empty);
                var urls = Regex.Matches(tweetText, urlPattern).Cast<Match>().Select(x => x.Value);
                tweetText = Regex.Replace(tweetText, urlPattern, String.Empty);
                var images = Regex.Matches(tweetText, imagePattern).Cast<Match>().Select(x => x.Value);
                tweetText = Regex.Replace(tweetText, imagePattern, String.Empty);
                tweetText = Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8,
                        Encoding.GetEncoding(Encoding.ASCII.EncodingName, new EncoderReplacementFallback(string.Empty),
                            new DecoderExceptionFallback()), Encoding.UTF8.GetBytes(tweetText)));
                var text = String.Join(" ", tweetText.Split(' ').Select(x => x.Trim()).Where(x => x.Length > 0));
                tweets.Add(new Tweet { Text = text, Mentions = mentions, Images = images, Links = urls, HashTags = hashtags});
            }
            return tweets;
        }
    }
}
