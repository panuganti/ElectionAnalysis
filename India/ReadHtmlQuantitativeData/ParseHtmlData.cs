using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadHtmlQuantitativeData
{
    public class ParseHtmlData
    {
        const string localIssues2010Id="ContentPlaceHolder1_GridView1";
        const string localIssues2015Id = "ContentPlaceHolder1_GridView2";
        const string devIssues2010Id = "ContentPlaceHolder1_GridView3";
        const string devIssues2015Id = "ContentPlaceHolder1_GridView4";
        const string candParams2010Id = "ContentPlaceHolder1_GridView5";
        const string candParams2015Id = "ContentPlaceHolder1_GridView6";
        const string partyParams2010Id = "ContentPlaceHolder1_GridView7";
        const string partyParams2015Id = "ContentPlaceHolder1_GridView8";
        const string casteParams2010Id = "ContentPlaceHolder1_GridView9";
        const string casteParams2015Id = "ContentPlaceHolder1_GridView10";

        public static ParsedData ParseHtml(string filename)
        {
            var parsedData = new ParsedData();

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;
            
            // filePath is a path to a file containing the html
            htmlDoc.Load(filename, Encoding.UTF8);
            // ParseErrors is an ArrayList containing any errors from the Load statement
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required
                //throw new Exception("Cannot parse");
            }

            var localIssues2010Nodes = htmlDoc.GetElementbyId(localIssues2010Id).ChildNodes[1].ChildNodes.Skip(1).ToArray();
            var localIssues2015Nodes = htmlDoc.GetElementbyId(localIssues2015Id).ChildNodes[1].ChildNodes.Skip(1).ToArray();

            var localIssues2010Dict = new Dictionary<string, string>();
            for (int i = 0; i < localIssues2010Nodes.Length - 1; i++)
            {
                localIssues2010Dict.Add(localIssues2010Nodes[i].ChildNodes[1].InnerText, localIssues2010Nodes[i].ChildNodes[2].InnerText);
            }

            var localIssues2015Dict = new Dictionary<string, string>();
            for (int i = 0; i < localIssues2015Nodes.Length - 1; i++)
            {
                localIssues2015Dict.Add(localIssues2015Nodes[i].ChildNodes[1].InnerText, localIssues2015Nodes[i].ChildNodes[2].InnerText);
            }

            var devIssues2010Nodes = htmlDoc.GetElementbyId(devIssues2010Id).ChildNodes[1].ChildNodes.Skip(1).ToArray();
            var devIssues2015Nodes = htmlDoc.GetElementbyId(devIssues2015Id).ChildNodes[1].ChildNodes.Skip(1).ToArray();

            var devIssues2010Dict = new Dictionary<string, string>();
            for (int i = 0; i < devIssues2010Nodes.Length - 1; i++)
            {
                if (devIssues2010Dict.ContainsKey(devIssues2010Nodes[i].ChildNodes[1].InnerText)) { continue;  }
                devIssues2010Dict.Add(devIssues2010Nodes[i].ChildNodes[1].InnerText, devIssues2010Nodes[i].ChildNodes[2].InnerText);
            }

            var devIssues2015Dict = new Dictionary<string, string>();
            for (int i = 0; i < devIssues2015Nodes.Length - 1; i++)
            {
                devIssues2015Dict.Add(devIssues2015Nodes[i].ChildNodes[1].InnerText, devIssues2015Nodes[i].ChildNodes[2].InnerText);
            }

            var candParams2010Nodes = htmlDoc.GetElementbyId(candParams2010Id).ChildNodes[1].ChildNodes.ToArray();
            var candParams2015Nodes = htmlDoc.GetElementbyId(candParams2015Id).ChildNodes[1].ChildNodes.ToArray();

            var candParams2010Dict = new Dictionary<string, Dictionary<string, string>>();
            var candParams2010 = candParams2010Nodes[0].ChildNodes.Skip(1).Take(candParams2010Nodes[0].ChildNodes.Count() - 2).Select(t => t.InnerText).ToArray();
            for (int i = 1; i < candParams2010Nodes.Length - 1; i++)
            {
                var dict = new Dictionary<string, string>();
                for (int j = 1; j < candParams2010Nodes[0].ChildNodes.Count() - 2; j++)
                {
                    if (j == 2) { continue; }
                    dict.Add(candParams2010[j-1], candParams2010Nodes[i].ChildNodes[j].InnerText);
                }
                candParams2010Dict.Add(candParams2010Nodes[i].ChildNodes[2].InnerText, dict);
            }

            var candParams2015Dict = new Dictionary<string, Dictionary<string, string>>();
            var candParams2015 = candParams2015Nodes[0].ChildNodes.Skip(1).Take(candParams2015Nodes[0].ChildNodes.Count() - 2).Select(t => t.InnerText).ToArray();
            for (int i = 1; i < candParams2015Nodes.Length - 1; i++)
            {
                var dict = new Dictionary<string, string>();
                for (int j = 1; j < candParams2015Nodes[0].ChildNodes.Count() - 2; j++)
                {
                    if (j == 2) { continue; }
                    dict.Add(candParams2015[j - 1], candParams2015Nodes[i].ChildNodes[j].InnerText);
                }
                candParams2015Dict.Add(candParams2015Nodes[i].ChildNodes[2].InnerText, dict);
            }

            var partyParams2010Nodes = htmlDoc.GetElementbyId(partyParams2010Id).ChildNodes[1].ChildNodes.ToArray();
            var partyParams2015Nodes = htmlDoc.GetElementbyId(partyParams2015Id).ChildNodes[1].ChildNodes.ToArray();

            var partyParams2010Dict = new Dictionary<string, Dictionary<string, string>>();
            var partyParams2010 = partyParams2010Nodes[0].ChildNodes.Skip(2).Take(partyParams2010Nodes[0].ChildNodes.Count() - 3).Select(t => t.InnerText).ToArray();
            for (int i = 1; i < partyParams2010Nodes.Length - 1; i++)
            {
                var dict = new Dictionary<string, string>();
                for (int j = 2; j < partyParams2010Nodes[0].ChildNodes.Count() - 2; j++)
                {
                    dict.Add(partyParams2010[j-2], partyParams2010Nodes[i].ChildNodes[j].InnerText);
                }
                partyParams2010Dict.Add(partyParams2010Nodes[i].ChildNodes[1].InnerText, dict);
            }

            var partyParams2015Dict = new Dictionary<string, Dictionary<string, string>>();
            var partyParams2015 = partyParams2015Nodes[0].ChildNodes.Skip(2).Take(partyParams2015Nodes[0].ChildNodes.Count() - 3).Select(t => t.InnerText).ToArray();
            for (int i = 1; i < partyParams2015Nodes.Length - 1; i++)
            {
                var dict = new Dictionary<string, string>();
                for (int j = 2; j < partyParams2015Nodes[0].ChildNodes.Count() - 2; j++)
                {
                    dict.Add(partyParams2015[j - 2], partyParams2015Nodes[i].ChildNodes[j].InnerText);
                }
                partyParams2015Dict.Add(partyParams2015Nodes[i].ChildNodes[1].InnerText, dict);
            }

            var casteParams2010Nodes = htmlDoc.GetElementbyId(casteParams2010Id).ChildNodes[1].ChildNodes.ToArray();
            var casteParams2015Nodes = htmlDoc.GetElementbyId(casteParams2015Id).ChildNodes[1].ChildNodes.ToArray();

            var casteParams2010Dict = new Dictionary<string, Dictionary<string, string>>();
            var casteParams2010 = casteParams2010Nodes[0].ChildNodes.Skip(2).Take(casteParams2010Nodes[0].ChildNodes.Count() - 3).Select(t => t.InnerText).ToArray();
            for (int i = 1; i < casteParams2010Nodes.Length - 1; i++)
            {
                if (casteParams2010Dict.ContainsKey(casteParams2010Nodes[i].ChildNodes[1].InnerText)) { continue; }
                var dict = new Dictionary<string, string>();
                for (int j = 2; j < casteParams2010Nodes[0].ChildNodes.Count() - 3; j++)
                {
                    dict.Add(casteParams2010[j - 2], casteParams2010Nodes[i].ChildNodes[j].InnerText);
                }
                casteParams2010Dict.Add(casteParams2010Nodes[i].ChildNodes[1].InnerText, dict);
            }

            var casteParams2015Dict = new Dictionary<string, Dictionary<string, string>>();
            var casteParams2015 = casteParams2015Nodes[0].ChildNodes.Skip(2).Take(casteParams2015Nodes[0].ChildNodes.Count() - 3).Select(t => t.InnerText).ToArray();
            for (int i = 1; i < casteParams2015Nodes.Length - 1; i++)
            {
                var dict = new Dictionary<string, string>();
                for (int j = 2; j < casteParams2015Nodes[0].ChildNodes.Count() - 3; j++)
                {
                    dict.Add(casteParams2015[j - 2], casteParams2015Nodes[i].ChildNodes[j].InnerText);
                }
                casteParams2015Dict.Add(casteParams2015Nodes[i].ChildNodes[1].InnerText, dict);
            }

            parsedData.localIssues2010Dict = localIssues2010Dict;
            parsedData.localIssues2015Dict = localIssues2015Dict;

            parsedData.devIssues2010Dict = devIssues2010Dict;
            parsedData.devIssues2015Dict = devIssues2015Dict;

            parsedData.candParams2010Dict = candParams2010Dict;
            parsedData.candParams2015Dict = candParams2015Dict;

            parsedData.partyParams2010Dict = partyParams2010Dict;
            parsedData.partyParams2015Dict = partyParams2015Dict;

            parsedData.casteParams2010Dict = casteParams2010Dict;
            parsedData.casteParams2015Dict = casteParams2015Dict;

            return parsedData;
        }
    }

    public class ParsedData
    {
        public Dictionary<string, string> localIssues2010Dict {get; set;}
        public Dictionary<string, string> localIssues2015Dict { get; set; }

        public Dictionary<string, string> devIssues2010Dict { get; set; }
        public Dictionary<string, string> devIssues2015Dict { get; set; }

        public Dictionary<string, Dictionary<string, string>> candParams2010Dict { get; set; }
        public Dictionary<string, Dictionary<string, string>> candParams2015Dict { get; set; }

        public Dictionary<string, Dictionary<string, string>> partyParams2010Dict { get; set; }
        public Dictionary<string, Dictionary<string, string>> partyParams2015Dict { get; set; }

        public Dictionary<string, Dictionary<string, string>> casteParams2010Dict { get; set; }
        public Dictionary<string, Dictionary<string, string>> casteParams2015Dict { get; set; }
    }
}
