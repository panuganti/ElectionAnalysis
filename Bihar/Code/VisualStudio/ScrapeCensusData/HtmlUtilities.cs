using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BiharElectionsLibrary;
using HtmlAgilityPack;

namespace ScrapeCensusData
{
    class HtmlUtilities
    {
        private const string DomainName = "http://www.census2011.co.in/";

        public static HtmlDocument GetHtmlPage(string url)
        {
            return new HtmlWeb().Load(url); // TODO: Save & Load locally if present
        }

        public static void ParseAndPopulateDistrictListPage(string url, State state)
        {
            // Parse
            var districtsListPage = GetHtmlPage(url);
            var districtBlockLinks = ParseDistrictsListPage(districtsListPage, state);
            foreach (var districtBlockLink in districtBlockLinks)
            {
                var blocksListPage = GetHtmlPage(districtBlockLink.Value);
                ParseBlocksListPage(blocksListPage, districtBlockLink.Key);
            }
        }

        public static Dictionary<District, string> ParseDistrictsListPage(HtmlDocument document, State state)
        {
            var dict = new Dictionary<District, string>();

            var table = document.GetElementbyId("tbody");
            foreach (var rowNode in table.ChildNodes)
            {
                var districtName = rowNode.ChildNodes[1].InnerText;
                var district = state.Districts.First(x => x.Name.Equals(districtName));
                var districtInfoLink = rowNode.ChildNodes[1].Attributes["href"].Value;
                    // TODO: Parse Cities & Metropolitan areas
                var blocksListLink = String.Format("{0}{1}", DomainName, rowNode.ChildNodes[2]
                                                                        .Attributes["href"].Value);

                dict.Add(district, blocksListLink);
            }
            return dict;
        }

        public static void ParseBlocksListPage(HtmlDocument document, District district)
        {
            var mainDiv = document.GetElementbyId("main");
            var rowNodes = GetNodesOfAClass(mainDiv, "row");

            foreach (var rowNode in rowNodes)
            {
                var linkNode = rowNode.ChildNodes[1].Descendants("a").First(); // There is only one
                string blockName = linkNode.InnerText;
                int blockId =
                    Int32.Parse(rowNode.Descendants("div").First(x => x.Attributes.Contains("cell mhide")).InnerText);
                string blockLink = String.Format("{0}{1}",DomainName,linkNode.Attributes["href"].Value);

                // Add new block
                AddBlockAndInfo(blockName, blockId, blockLink, district);
            }
        }

        public static void AddBlockAndInfo(string blockName, int blockId, string url, District district)
        {
            var newBlock = district.AddBlock(blockName, blockId);
            var blockInfoDoc = GetHtmlPage(url);
            var tableNodes = GetNodesOfAClass(blockInfoDoc.DocumentNode, "table");

            foreach (var tableNode in tableNodes)
            {
                var townOrVillage = GetNodesOfAClass(tableNode, "headrow").First().ChildNodes[1].InnerText;
                    GetNodesOfAClass(tableNode, "row").ToList().ForEach(x =>
                    {
                        var linkNode = x.Descendants("a").First();
                        string townUrl = linkNode.Attributes["href"].Value;
                        int id = Int32.Parse(x.Descendants("div").First(y => y.Attributes.Contains("cell mhide")).InnerText);
                        string name = linkNode.InnerText;

                        if ( townOrVillage.Equals("Town") && name.Contains("Census Town"))
                        {
                            AddCensusTownInfo(name, id, townUrl, newBlock);                            
                        }
                        else if (townOrVillage.Equals("Town"))
                        {
                            AddMunicipalityInfo(name, id, townUrl, newBlock);                            
                        }
                        else if (townOrVillage.Equals("Village"))
                        {
                            AddVillageInfo(name, id, townUrl, newBlock);
                        }
                    }
                    );
            }
        }

        public static void AddMunicipalityInfo(string townName, int townId, string url, Block block)
        {
            var newMC = block.AddMunicipalCorp(townName, townId);
            if (newMC == null) { return; }
            var municipalityDocument = GetHtmlPage(url);
            ParseAndAddMunicipalityInfo(municipalityDocument, newMC);
        }

        public static void ParseAndAddMunicipalityInfo(HtmlDocument doc, MunicipalCorp mc)
        {
            var tableNodes = GetNodesOfAClass(doc.DocumentNode, "table");
            foreach (var tableNode in tableNodes)
            {
                var headings = GetNodesOfAClass(tableNode, "headrow").First().ChildNodes.Select(x => x.InnerText);
                var rows = GetNodesOfAClass(tableNode, "row").ToList();
                if (headings.Contains("Ward"))
                {
                    rows.ForEach(x => mc.AddWard(x.ChildNodes[0].InnerText));
                }
                else
                {
                    mc.TotalPopulation = Int32.Parse(rows[0].ChildNodes[2].InnerText);
                }
            }
        }

        public static void AddCensusTownInfo(string censusTownName, int id, string url, Block block)
        {
            var newCensusTown = block.AddCensusTown(censusTownName, id);
            if (newCensusTown == null) { return; }
            var censusTownDocument = GetHtmlPage(url);
            ParseAndAddCensusTownInfo(censusTownDocument, newCensusTown);
        }

        public static void ParseAndAddCensusTownInfo(HtmlDocument doc, CensusTown town)
        {
            var allSentences = doc.GetElementbyId("main")
                .Descendants()
                .Select(x => x.InnerText)
                .SelectMany(y => Regex.Split(y, @"(?<=[\.!\?])\s+"));
            var censusTownParams = new CensusTownParams();
            /* TODO: Extract
             * total pop, #males, #females "population of 3,531 of which 1,825 are males while 1,706 are females"
             * children pop, "age of 0-6 is 282"
             * compute m,f child # from "Child Sex Ratio in Pataliputra Housing Colony is around 763" .. or skip
             * Literacy rate from "Literacy rate of Pataliputra Housing Colony city is 95.48 %"
             * #houses from "administration over 761 houses"
             * SC, ST from "Schedule Caste (SC) constitutes 2.29 % while Schedule Tribe (ST) were 0.51 %" .. if absent none present
             * Main Workers from "1,294 were engaged in work or business activity"
             * m/f breakup from "Of this 956 were males while 338 were females"
             * main vs marginal from "Of total 1294 working population, 94.59 % were engaged in Main Work while 5.41 % of total workers were engaged in Marginal Work"
            */
            town.Params = censusTownParams;
        }

        public static void AddVillageInfo(string villageName, int id, string url, Block block)
        {
            var newVillage = block.AddVillage(villageName, id);
            if (newVillage == null) { return;}
            var villageDocument = GetHtmlPage(url);
            ParseAndAddVillageInfo(villageDocument, newVillage);
        }

        public static void ParseAndAddVillageInfo(HtmlDocument doc, Village village)
        {
            var tableNode = doc.GetElementbyId("table");
            var rows = GetNodesOfAClass(tableNode, "d1").Union(GetNodesOfAClass(tableNode, "d2"));
            var villageParams = new VillageParameters();
            foreach (var rowNode in rows) // TODO:
            {
                string rowHeader = rowNode.ChildNodes[0].InnerText;
                if (rowHeader.Equals("Total No. of Houses"))
                {
                }
                if (rowHeader.Equals("Population"))
                {
                }
                if (rowHeader.Equals("Child (0-6)"))
                {
                }
                if (rowHeader.Equals("Schedule Caste"))
                {
                }
                if (rowHeader.Equals("Schedule Tribe"))
                {
                }
                if (rowHeader.Equals("Literacy"))
                {
                }
                if (rowHeader.Equals("Total Workers"))
                {
                }
                if (rowHeader.Equals("Main Worker"))
                {
                }
                if (rowHeader.Equals("Marginal Worker"))
                {
                }
            }
            village.Parameters = villageParams;
        }

        public static IEnumerable<HtmlNode> GetNodesOfAClass(HtmlNode node, string className)
        {
            return node.Descendants("div")
                .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Equals(className));
        }
    }
}
