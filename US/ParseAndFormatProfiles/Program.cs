using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ParseAndFormatProfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            FormatTweets();
        }

        static void FormatTweets()
        {
            const string tweetsDir = @"D:\ArchishaData\ElectionData\US\Tweets";
            const string outfile = @"D:\ArchishaData\ElectionData\US\FormattedTweets.tsv";
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
            const string judgementsFile = @"D:\ArchishaData\ElectionData\US\Judgements.tsv";
            const string outfile = @"D:\ArchishaData\ElectionData\US\JudgementsParsed.tsv";
            File.WriteAllLines(outfile, File.ReadAllLines(judgementsFile).SelectMany(x =>
            {
                var parts = x.Split('\t');
                var judgements = parts[4].Split(';').Skip(1);
                int count = 0;
                return judgements.Select(y => { count++; return String.Format("{0}__{1}\t{2}", parts[1], count, y); });
            }));
        }

        static void ParseTweets()
        {
            const string republicanProfilesDir = @"D:\ArchishaData\ElectionData\US\RepublicanProfiles";
            const string democraticProfilesDir = @"D:\ArchishaData\ElectionData\US\DemocraticProfiles";
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
                    File.WriteAllLines(outfile, ExtractTweets(timelineNode));
                    count++; Console.WriteLine("count: {0}:", count);
                }
                catch (Exception)
                {  
                }
            }
        }

        private static IEnumerable<string> ExtractTweets(HtmlNode node)
        {
            var tweetNodes = node.Descendants("li")
                .Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value.Contains(
                    "js-stream-item stream-item stream-item expanding-stream-item"));
            var tweets = new List<string>();
            foreach (var tweetNode in tweetNodes)
            {
                var text = String.Join(" ", tweetNode.Descendants("div").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "content").First().Descendants("p").First().InnerText.Replace('\n', ' ').Split(' ').Select(x => x.Trim()).Where(x => x.Length > 0));
                tweets.Add(text);
            }
            return tweets;
        }
    }
}
