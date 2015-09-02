using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using CVoterContracts;
using HtmlUtils;
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
            // HtmlAgilityPack by default leaves few elements empty. So, remove option tag from that list
            HtmlNode.ElementsFlags.Remove("option"); 
            var doc = new HtmlWeb().Load(filename);
            // TODO: Instead, just look for all the option nodes
            var q1Div = doc.DocumentNode.Descendants("div").First(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "q1"); 
            // Read dictionary of id, name
            var acDict = GetAcDictionary(q1Div);
            // ReSharper disable once PossibleNullReferenceException
            var acName = Path.GetFileNameWithoutExtension(filename).ToLower();
            var key = acDict.First(t =>
            {
                var parts = acName.Split('_');
                if (parts.Length == 2) return t.Key.ToLower().Equals(acName);
                return t.Key.Split('_')[0].ToLower().Equals(acName);
            }).Key;
            acData1.Name = key.Split('_')[0];
            acData2.Name = acData1.Name;
            acData1.No = acDict[key];
            acData2.No = acData1.No;
            // Read each table of the file
            ParsePage(doc, acData1, acData2);
            return new Tuple<AcQualitative,AcQualitative>(acData1,acData2); // Two AcDatas are returned
        }

        public static Dictionary<string, int> GetAcDictionary(HtmlNode qDiv)
        {
            var optionNodes = qDiv.Descendants("option").Where(t =>
            {
                int n;
                return t.Attributes.Contains("value") && int.TryParse(t.Attributes["value"].Value, out n);
            }).ToArray();

            return optionNodes.ToDictionary(x => String.Format("{0}_{1}", x.InnerText.Replace(" ", ""), x.Attributes["value"].Value),
                y => int.Parse(y.Attributes["value"].Value));
        }

        public static void ParsePage(HtmlDocument doc, AcQualitative acData1, AcQualitative acData2)
        {
            // First two are local issues
            acData1.LocalIssues = ParseLocalIssuesData(doc.GetElementbyId("ContentPlaceHolder1_GridView1"));
            acData2.LocalIssues = ParseLocalIssuesData(doc.GetElementbyId("ContentPlaceHolder1_GridView2"));
            // Next two are dev params
            acData1.DevParams = ParseDevParamsData(doc.GetElementbyId("ContentPlaceHolder1_GridView3"));
            acData2.DevParams = ParseDevParamsData(doc.GetElementbyId("ContentPlaceHolder1_GridView4"));
            // Next two are cand params
            acData1.CandidateRatings = ParseCandidateParamsData(doc.GetElementbyId("ContentPlaceHolder1_GridView5"));
            acData2.CandidateRatings = ParseCandidateParamsData(doc.GetElementbyId("ContentPlaceHolder1_GridView6"));
            // next two are party params
            acData1.PartyRatings = ParsePartyParamsData(doc.GetElementbyId("ContentPlaceHolder1_GridView7"));
            acData2.PartyRatings = ParsePartyParamsData(doc.GetElementbyId("ContentPlaceHolder1_GridView8"));
            // next two are caste params
            var tuple = ParseCasteShareData(doc.GetElementbyId("ContentPlaceHolder1_GridView9"));
            acData1.CasteShares = tuple.Item1;
            acData1.PartyCasteShares = tuple.Item2;
            tuple = ParseCasteShareData(doc.GetElementbyId("ContentPlaceHolder1_GridView10"));
            acData2.CasteShares = tuple.Item1;
            acData2.PartyCasteShares = tuple.Item2;
            // TODO: Handle cases of empty tables...
        }

        public static List<Rating> ParseLocalIssuesData(HtmlNode localIssuesNode)
        {
            var table = TableExtractor.ExtractTableFromDiv(localIssuesNode);
            return table.Rows.Select(row => new Rating {Feature = row[0], Score = int.Parse(row[1])}).ToList();
        }

        public static List<Rating> ParseDevParamsData(HtmlNode devParamsNode)
        {
            var table = TableExtractor.ExtractTableFromDiv(devParamsNode);
            return table.Rows.Select(row => new Rating {Feature = row[0], Score = int.Parse(row[1])}).ToList();
        }

        public static List<CandidateRating> ParseCandidateParamsData(HtmlNode candidateParamsNode)
        {
            var ratings = new List<CandidateRating>();
            var table = TableExtractor.ExtractTableFromDiv(candidateParamsNode);
            var headers = table.Headers.Skip(2).ToArray();
            foreach (var row in table.Rows)
            {
                var candRating = new CandidateRating { Name = row[1].Trim(), PartyName = row[0].Replace("\n", "").Trim(), Ratings = new List<Rating>() };
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
                var partyRating = new PartyRating { PartyName = row[0].Replace("\n","").Trim(), Ratings = new List<Rating>() };
                for (int i = 0; i < headers.Length; i++)
                {
                    partyRating.Ratings.Add(new Rating { Feature = headers[i], Score = int.Parse(row[i + 1]) });
                }
                ratings.Add(partyRating);
            }
            return ratings;
        }

        public static Tuple<CasteShare, List<PartyCasteShare>> ParseCasteShareData(HtmlNode casteSharesNode)
        {
            var table = TableExtractor.ExtractTableFromDiv(casteSharesNode);
            var casteShare = new CasteShare { CasteShares = new List<Rating>(), PerCents = new List<Rating>() };
            var headers = table.Headers.Skip(3).ToArray();
            var partyCasteShares = headers.Select(header => new PartyCasteShare {PartyName = header, PerCents = new List<Rating>()}).ToList();

            foreach (var row in table.Rows)
            {
                casteShare.CasteShares.Add(new Rating { Feature = row[0], Score = int.Parse(row[1]) });
                casteShare.PerCents.Add(new Rating { Feature = row[0], Score = int.Parse(row[2]) });
                for (int i=0; i < headers.Length; i++)
                {
                    partyCasteShares[i].PerCents.Add(new Rating { Feature = row[0], Score = int.Parse(row[i+3])});
                }
            }
            return new Tuple<CasteShare,List<PartyCasteShare>>(casteShare, partyCasteShares);
        }
    }
}
