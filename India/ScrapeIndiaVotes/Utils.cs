using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ScrapeIndiaVotes
{
    class Utils
    {
        public static Table ExtractTableFromDiv(HtmlNode node, bool hasHeader = true)
        { // We assume that the input node is a table node

            var table = new Table();
            if (hasHeader)
            {
                table.Headers = node.Descendants("th").Select(t=>t.InnerText).ToList();
            }

            var rowNodes = node.Descendants("tr");
            table.Rows = new List<List<string>>();
            foreach (var rowNode in rowNodes)
            {
                table.Rows.Add(rowNode.Descendants("td").Select(t => t.InnerText).ToList());
            }
            return table;
        }
    }

    class Table
    {
        public List<string> Headers { get; set; }

        public List<List<string>> Rows { get; set; }
    }
}
