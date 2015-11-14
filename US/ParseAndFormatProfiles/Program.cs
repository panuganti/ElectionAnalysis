using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
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
            //ParseTweets();
            FormatTweets();
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
                var lines = File.ReadAllLines(x);
                var output = lines.Select(y => { count++; return String.Format("{0}__{1}\t{2}", screenName, count, y.Replace('\"', ' ').Replace(","," ")); });
                return output;
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
            const string outputDir = @"I:\ArchishaData\ElectionData\US\Tweets\";
            string[] democraticCandidates = File.ReadAllLines(@"I:\ArchishaData\ElectionData\US\DemocraticCandidates.txt").SelectMany(x => x.Split(' ')).Where(x=>x.Length > 2).ToArray();
            string[] republicanCandidates = File.ReadAllLines(@"I:\ArchishaData\ElectionData\US\RepublicanCandidates.txt").SelectMany(x => x.Split(' ')).Where(x => x.Length > 2).ToArray();
            ParseProfilesForTweets(republicanProfilesDir, outputDir, democraticCandidates, republicanCandidates);
            ParseProfilesForTweets(democraticProfilesDir, outputDir, democraticCandidates, republicanCandidates);
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

        static void ParseProfilesForTweets(string inputDir, string outputDir, string[] democratcandidates, string[] republicancandidates)
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
                    File.WriteAllLines(outfile,
                        ExtractTweets(timelineNode)
                            .Select(x => String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}", 
                                    x.Text, 
                                    x.Images.Count(),
                                    x.HashTags.Any() ? 1 : 0,
                                    x.Links.Any() ? 1:0,
                                    x.Mentions.Count(),
                                    String.Join(" ",x.HashTags.Select(y=> SplitCamelCase(y.TrimStart('#')))),
                                    String.Join(" ",x.HashTags.Select(y=> SplitCamelCase(y.TrimStart('#')))).Split(' ').Select(z=>z.ToLower()).Intersect(democratcandidates.Select(z=>z.ToLower())).Count(),
                                    String.Join(" ",x.HashTags.Select(y=> SplitCamelCase(y.TrimStart('#')))).Split(' ').Select(z=>z.ToLower()).Intersect(republicancandidates.Select(z=>z.ToLower())).Count(),
                                    String.Join(" ",x.Text.Split(' ').Intersect(democratcandidates).Count()),
                                    String.Join(" ",x.Text.Split(' ').Intersect(republicancandidates).Count())
                                    )));
                    count++; 
                    Console.WriteLine("count: {0}:", count);
                }
                catch (Exception exception)
                {
                    throw;
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
            try
            { 
                if (!node.Descendants("li").Any()) { return Enumerable.Empty<Tweet>();}
            }
            catch(Exception ex)
            {
                return Enumerable.Empty<Tweet>();
            }
            if (!node.Descendants("li").Any()) { }
            var tweetNodes = node.Descendants("li")
                .Where(t => (t.Attributes.Contains("class") && t.Attributes["class"].Value.Contains(
                    "js-stream-item stream-item stream-item expanding-stream-item")) ||
                    (t.Attributes.Contains("role") && t.Attributes["role"].Value.Contains(
                    "presentation")));
            var tweets = new List<Tweet>();
            var mentionPattern = "@([a-zA-Z0-9_]+)";
            var hashTagPattern = "#([a-zA-Z0-9_]+)";
            var urlPattern = @"(https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&\/\/=]*))";
            var imagePattern = @"(pic.twitter.com\/[a-zA-Z0-9]+)";
            foreach (var tweetNode in tweetNodes)
            {
                var tweetTextNodesExist = tweetNode.Descendants("div")
                        .Any(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "content");
                if (!tweetTextNodesExist) continue;
                var tweetTextNodes = tweetNode.Descendants("div")
                        .First(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "content")
                        .Descendants("p").ToArray();
                var tweetTextNodeExists = tweetTextNodes.Any(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.StartsWith("TweetTextSize"));
                string tweetText;
                if (tweetTextNodeExists)
                {
                    tweetText = tweetTextNodes.First(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.StartsWith("TweetTextSize")).InnerText.Replace('\n', ' ');
                }
                else
                {
                    tweetText = tweetTextNodes.First().InnerText.Replace('\n', ' ');
                }
                // TODO: Extract mentions, pics, links, correct html encodes, popularity info.
                tweetText = HttpUtility.HtmlDecode(tweetText).Replace('\n', ' ');
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

        static String SplitCamelCase(String s)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return r.Replace(s, " ");
        }

        public static string GetWebPageTitle(string url)
        {
            // Create a request to the url
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;

            // If the request wasn't an HTTP request (like a file), ignore it
            if (request == null) return null;

            // Use the user's credentials
            request.UseDefaultCredentials = true;

            // Obtain a response from the server, if there was an error, return nothing
            HttpWebResponse response = null;
            try { response = request.GetResponse() as HttpWebResponse; }
            catch (WebException) { return null; }

            // Regular expression for an HTML title
            string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";

            // If the correct HTML header exists for HTML text, continue
            if (new List<string>(response.Headers.AllKeys).Contains("Content-Type"))
                if (response.Headers["Content-Type"].StartsWith("text/html"))
                {
                    // Download the page
                    WebClient web = new WebClient();
                    web.UseDefaultCredentials = true;
                    string page = web.DownloadString(url);

                    // Extract the title
                    Regex ex = new Regex(regex, RegexOptions.IgnoreCase);
                    return ex.Match(page).Value.Trim();
                }

            // Not a valid HTML page
            return null;
        }
    }
}
