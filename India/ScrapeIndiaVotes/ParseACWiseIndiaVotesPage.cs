using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace ScrapeIndiaVotes
{
    class ParseACWiseIndiaVotesPage
    {
        public static void ParsePage(string htmlFile, string dirPath)
        {
            var doc = new HtmlWeb().Load(htmlFile);

            // class f1 for overall # results
            // m1 for ac wise tables
            var m1Div = doc.GetElementbyId("m1");
            var allTableNodes = m1Div.Descendants("table").ToArray();
            // Alternative tables .. one of class grid and another grid sortable
            for (int i=0; i<allTableNodes.Count()/2; i++)
            {
                var filename = Path.Combine(dirPath, String.Format("{0}.txt", ParseGridTable(allTableNodes[i * 2])));
                ParseGridSortableTable(allTableNodes[i*2 + 1], filename);
            }

        }

        private static string ParseGridTable(HtmlNode gridNode)
        {
            var nodes = gridNode.Descendants("td").ToArray();
            var acName = nodes[0].InnerText;
            var totalAcVotes = nodes[1].InnerText;
            return acName;
        }

        private static void ParseGridSortableTable(HtmlNode gridSortableNode, string filename)
        {
            var table = Utils.ExtractTableFromDiv(gridSortableNode);
            using (var writer = new StreamWriter(filename))
            {
                // Print header
                writer.WriteLine(String.Join("\t", table.Headers));
                // Print each row                
                foreach (var row in table.Rows)
                {
                    writer.WriteLine(String.Join("\t", row));
                }
            }
        }

        private static HtmlNode GetNodeOfAClass(HtmlNode node, string className)
        {
            var divNode = node.Descendants("div");
            return divNode.First(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals(className));
        }
        private static IEnumerable<HtmlNode> GetNodesOfAClass(HtmlNode node, string className)
        {
            return node.Descendants("div")
                    .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Equals(className));
        }
    }
}
