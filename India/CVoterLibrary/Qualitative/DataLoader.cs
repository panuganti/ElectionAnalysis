using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using CVoterContracts;
using Utils;
using System;

namespace CVoterLibrary
{
    public class DataLoader
    {
        public static Tuple<QualitativeData,QualitativeData> LoadDataFromDir(string dirPath)
        {
            var data1 = new QualitativeData { AcQualitatives = new List<AcQualitative>() };
            var data2 = new QualitativeData { AcQualitatives = new List<AcQualitative>() };

            var allFiles = Directory.GetFiles(dirPath);
            foreach (var file in allFiles)
            {
                var tuple = LoadDataFromFile(file);
                data1.AcQualitatives.Add(tuple.Item1);
                data2.AcQualitatives.Add(tuple.Item2);
            }
            return new Tuple<QualitativeData,QualitativeData>(data1, data2);
        }

        public static Tuple<AcQualitative,AcQualitative> LoadDataFromFile(string filename)
        {
            var acData1 = new AcQualitative();
            var acData2 = new AcQualitative();
            var doc = new HtmlWeb().Load(filename);
            // TODO: Instead, just look for all the option nodes
            var q1Div = doc.DocumentNode.Descendants("div").First(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "q1"); 
            // Read dictionary of id, name
            var acDict = GetAcDictionary(q1Div);
            var acName = Path.GetFileNameWithoutExtension(filename);
            acData1.Name = acDict.Keys.First(t => t == acName);
            acData2.Name = acData1.Name;
            acData1.No = acDict[acData1.Name]; // TODO: Use relaxed matching
            acData2.No = acData1.No;
            // Read each table of the file
            ParsePage(doc.DocumentNode, acData1, acData2);
            return new Tuple<AcQualitative,AcQualitative>(acData1,acData2); // Two AcDatas are returned
        }

        public static Dictionary<string, int> GetAcDictionary(HtmlNode qDiv)
        {
            return qDiv.Descendants("option")
                .Where(t =>
                {
                    int n;
                    return t.Attributes.Contains("value") && int.TryParse(t.Attributes["value"].Value, out n);
                }).ToDictionary(x=>x.InnerText.Replace(" ",""), y=> int.Parse(y.Attributes["value"].Value));
        }

        public static void ParsePage(HtmlNode rootNode, AcQualitative acData1, AcQualitative acData2)
        {
            var tableDivs = rootNode.Descendants("div")
                .Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "divgrds").ToArray();
            // First two are local issues
            acData1.LocalIssues = ParseLocalIssuesData(tableDivs[0]);
            acData2.LocalIssues = ParseLocalIssuesData(tableDivs[1]);
            // Next two are dev params
            acData1.DevParams = ParseDevParamsData(tableDivs[2]);
            acData2.DevParams = ParseDevParamsData(tableDivs[3]);
            // Next two are cand params
            acData1.CandidateRatings = ParseCandidateParamsData(tableDivs[4]);
            acData2.CandidateRatings = ParseCandidateParamsData(tableDivs[5]);
            // next two are party params
            acData1.PartyRatings = ParsePartyParamsData(tableDivs[6]);
            acData2.PartyRatings = ParsePartyParamsData(tableDivs[7]);
            // next two are caste params
            acData1.CasteShares = ParseCasteShareData(tableDivs[8]);
            acData2.CasteShares = ParseCasteShareData(tableDivs[9]);
        }

        public static List<Rating> ParseLocalIssuesData(HtmlNode localIssuesNode)
        {
            var ratings = new List<Rating>();
            var table = TableExtractor.ExtractTableFromDiv(localIssuesNode);
            foreach (var row in table.Rows)
            {
                ratings.Add(new Rating { Feature = row[0], Score = int.Parse(row[1])});
            }
            return ratings;
        }

        public static List<Rating> ParseDevParamsData(HtmlNode devParamsNode)
        {
            var ratings = new List<Rating>();
            var table = TableExtractor.ExtractTableFromDiv(devParamsNode);
            foreach (var row in table.Rows)
            {
                ratings.Add(new Rating { Feature = row[0], Score = int.Parse(row[1]) });
            }
            return ratings;
        }

        public static List<CandidateRating> ParseCandidateParamsData(HtmlNode candidateParamsNode)
        {
            var ratings = new List<CandidateRating>();
            var table = TableExtractor.ExtractTableFromDiv(candidateParamsNode);
            var headers = table.Headers.Skip(2).ToArray();
            foreach (var row in table.Rows)
            {
                var candRating = new CandidateRating { Name = row[1], PartyName = row[0], Ratings = new List<Rating>() };
                for (int i = 0; i < headers.Length; i++ )
                {
                    candRating.Ratings.Add(new Rating { Feature = headers[i], Score = int.Parse(row[i + 2]) });
                }
                    ratings.Add(candRating);
            }
            return ratings;
        }

        public static List<PartyRating> ParsePartyParamsData(HtmlNode partyParamsNode)
        {
            var ratings = new List<PartyRating>();
            var table = TableExtractor.ExtractTableFromDiv(partyParamsNode);
            var headers = table.Headers.Skip(1).ToArray();
            foreach (var row in table.Rows)
            {
                var partyRating = new PartyRating { PartyName = row[0], Ratings = new List<Rating>() };
                for (int i = 0; i < headers.Length; i++)
                {
                    partyRating.Ratings.Add(new Rating { Feature = headers[i], Score = int.Parse(row[i + 1]) });
                }
                ratings.Add(partyRating);
            }
            return ratings;
        }

        public static List<CasteShare> ParseCasteShareData(HtmlNode casteSharesNode)
        {
            throw new NotImplementedException();
        }
    }
}
