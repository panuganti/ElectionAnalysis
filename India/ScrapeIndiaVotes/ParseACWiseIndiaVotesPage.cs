using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace ScrapeIndiaVotes
{
    class ParseACWiseIndiaVotesPage
    {
        public static void ParsePage(string htmlFile)
        {
            htmlFile = "I:/ArchishaData/ElectionData/RawData/IndiaVotes/AcWise/16_58_1.html";
            var doc = new HtmlWeb().Load(htmlFile);

            // class f1 for overall # results
            // m1 for ac wise tables
            var m1Div = GetNodeOfAClass(doc.DocumentNode, "m1");
            var allTableNodes = m1Div.Descendants("table");
            // Alternative tables .. one of class grid and another grid sortable

        }

        private static void ParseGridTable(HtmlNode gridNode, string filename)
        {
            var nodes = GetNodesOfAClass(gridNode,"tal bg2").ToArray();
            var acName = nodes[0].InnerText;
            var totalAcVotes = nodes[1].InnerText;
            File.WriteAllText(filename, String.Format("{0}\n{1}", acName, totalAcVotes));
        }

        private static void ParseGridSortableTable(HtmlNode gridSortableNode, string filename)
        {
            using (var writer = new StreamWriter(filename))
            {
                // Print header
                // Print each row                
            }
        }

        private static HtmlNode GetNodeOfAClass(HtmlNode node, string className)
        {
            return node.Descendants("div")
                    .First(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals(className));
        }
        private static IEnumerable<HtmlNode> GetNodesOfAClass(HtmlNode node, string className)
        {
            return node.Descendants("div")
                    .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals(className));
        }
    }
}
