using System;
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
                    if (count > 25)
                    {
                        profilesWriter.Close();
                        return;
                    }
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
            string format = "Overall Judgement: <select name=\"party\" ng-model=\"vMain.partySelected\" ng-selected=\"vMain.NotRelevant\" ng-options=\"party for party in vMain.parties\" >{{party}}</select>";
            string genderHtml = "<br>Gender: <select name=\"gender\" ng-model=\"vMain.genderSelected\" ng-selected=\"vMain.NotRelevant\" ng-options=\"gender for gender in vMain.genders\" >{{party}}</select>";
            node.InnerHtml = format + genderHtml + node.InnerHtml;            

        }

        static void ModifyTimelineDiv(HtmlNode node)
        {
            string format = "Tweet Judgement: <select name=\"party\" ng-model=\"vMain.tweetInclination[{0}]\" ng-selected=\"vMain.NotRelevant\" ng-options=\"party for party in vMain.parties\" >{{party}}</select>";
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
    }
}
