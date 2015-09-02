using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace HtmlUtils
{
    public class TableExtractor
    {
        public static Table ExtractTableFromDiv(HtmlNode node, bool hasHeader = true)
        { // We assume that the input node is a table node

            var table = new Table();
            if (hasHeader)
            {
                table.Headers = node.Descendants("th").Select(t => t.InnerText).ToList();
            }

            var rowNodes = node.Descendants("tbody").First().ChildNodes;
            table.Rows = new List<List<string>>();
            foreach (var rowNode in rowNodes)
            {
                if (rowNode.Name == "tr")
                {
                    table.Rows.Add(rowNode.Descendants("td").Select(t => t.InnerText).ToList());
                }
            }
            return table;
        }
    }

    public class Table
    {
        public List<string> Headers { get; set; }

        public List<List<string>> Rows { get; set; }
    }
}
