using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ScrapeIndiaVotes
{
    class ParseACWiseIndiaVotesPage
    {
        public static void ParseAcResultsPage(string htmlFile, string dirPath)
        {
            var doc = new HtmlWeb().Load(htmlFile);
            var acName =
                doc.DocumentNode.ChildNodes.First(t => t.Name == "h2")
                    .ChildNodes.First(t => t.Name == "span")
                    .InnerText.Trim();
            var distName =
                doc.DocumentNode.Descendants()
                    .First(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "searchBoxRight")
                    .Descendants("p")
                    .First(t => t.InnerText.Contains("District"))
                    .Descendants("a")
                    .First()
                    .InnerText;
            var m1Div = doc.GetElementbyId("m1");
            var tableNode = m1Div.Descendants("table").First();
            var filename = Path.Combine(dirPath,
                String.Format("{0} {1}.txt", acName.Trim().Replace(" ", "_"), distName.Trim().Replace(" ", "_")));
            ParseAndWriteGridSortableTable(tableNode, filename);
        }

        public static void ParseAcWiseResultsPage(string htmlFile, string dirPath, int year)
        {
            var doc = new HtmlWeb().Load(htmlFile);
            var pcNameText = doc.DocumentNode.ChildNodes.First(t => t.Name == "h2").ChildNodes.First(t=>t.Name == "span").InnerText;
            var regex = new Regex(String.Format(@"([\w\s]+){0}", year));
            var pcName = regex.Match(pcNameText).Groups[1].Value.Trim();
            // class f1 for overall # results
            // m1 for ac wise tables
            var m1Div = doc.GetElementbyId("m1");
            var allTableNodes = m1Div.Descendants("table").ToArray();
            // Alternative tables .. one of class grid and another grid sortable
            for (int i = 0; i < allTableNodes.Count() / 2; i++)
            {
                var filename = Path.Combine(dirPath, String.Format("{0} {1}.txt", 
                    ParseGridTable(allTableNodes[i*2]).Trim().Replace(" ","_"), pcName.Trim().Replace(" ","_")));
                ParseAndWriteGridSortableTable(allTableNodes[i*2 + 1], filename);
            }
        }

        private static string ParseGridTable(HtmlNode gridNode)
        {
            var regex = new Regex(@"AC Name: ([a-zA-z\(\)\s]+)");
            var nodes = gridNode.Descendants("td").ToArray();
            return regex.Match(nodes[0].InnerText).Groups[1].Value; 
        }

        private static void ParseAndWriteGridSortableTable(HtmlNode gridSortableNode, string filename)
        {
            var table = Utils.ExtractTableFromDiv(gridSortableNode);
            if (File.Exists(filename))
            {
                Console.Write("File already exists");
            }
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
    }
}
