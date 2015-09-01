using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using CVoterContracts;

namespace CVoterLibrary
{
    public class DataLoader
    {
        public static QualitativeData LoadDataFromDir(string dirPath)
        {
            var data = new QualitativeData {AcQualitatives = new List<AcQualitative>()};
            var allFiles = Directory.GetFiles(dirPath);
            foreach (var file in allFiles)
            {
                data.AcQualitatives.Add(LoadDataFromFile(file));
            }
            return data;
        }

        public static AcQualitative LoadDataFromFile(string filename)
        {
            var acData = new AcQualitative();
            var doc = new HtmlWeb().Load(filename);
            // TODO: Instead, just look for all the option nodes
            var q1Div = doc.DocumentNode.Descendants("div").First(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "q1"); 
            // Read dictionary of id, name
            var acDict = GetAcDictionary(q1Div);
            var acName = Path.GetFileNameWithoutExtension(filename);
            acData.Name = acDict.Keys.First(t => t == acName);
            acData.No = acDict[acData.Name]; // TODO: Use relaxed matching
            
            // Read each table of the file

            return acData; // Two AcDatas are returned
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
            ParseLocalIssuesData(tableDivs[0]);
            ParseLocalIssuesData(tableDivs[1]);
            // Next two are dev params
            ParseDevParamsData(tableDivs[2]);
            ParseDevParamsData(tableDivs[3]);
            // Next two are cand params
            ParseCandidateParamsData(tableDivs[4]);
            ParseCandidateParamsData(tableDivs[5]);
            // next two are party params
            ParsePartyParamsData(tableDivs[6]);
            ParsePartyParamsData(tableDivs[7]);
            // next two are caste params
            ParseCasteShareData(tableDivs[8]);
            ParseCasteShareData(tableDivs[9]);
        }

        public static void ParseLocalIssuesData(HtmlNode localIssuesNode)
        {            
        }

        public static void ParseDevParamsData(HtmlNode localIssuesNode)
        {
        }

        public static void ParseCandidateParamsData(HtmlNode localIssuesNode)
        {
        }

        public static void ParsePartyParamsData(HtmlNode localIssuesNode)
        {
        }

        public static void ParseCasteShareData(HtmlNode localIssuesNode)
        {
        }

    }
}
