using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BiharElectionsLibrary;
using HtmlAgilityPack;

namespace GenerateDataForR
{
    class PopulateCensusData
    {
        private const string DomainName = "http://www.census2011.co.in/";
        private readonly string _baseDataPath;
        public PopulateCensusData(string baseDataPath)
        {
            _baseDataPath = baseDataPath;
        }

        public HtmlDocument GetHtmlPage(string relativeUrl)
        {
            try
            {
                HtmlDocument doc;
                var cachedFile = Path.Combine(_baseDataPath,
                    Path.GetFileName(Path.Combine(_baseDataPath, relativeUrl.Replace("/", "\\")))).Replace("*","");
                if (!File.Exists(cachedFile))
                {
                    string path = String.Format("{0}{1}", DomainName, relativeUrl);
                    var htmlWeb = new HtmlWeb();
                    doc = htmlWeb.Load(path);
                    var html = doc.DocumentNode.OuterHtml;
                    File.WriteAllText(cachedFile, html);
                }
                else
                {
                    doc = new HtmlDocument();
                    doc.Load(cachedFile);
                }
                return doc;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }

        public void ParseAndPopulateDistrictListPage(string relativeUrl, State state)
        {
            // Parse
            var districtsListPage =   GetHtmlPage(relativeUrl);
            var districtBlockLinks = ParseDistrictsListPage(districtsListPage, state);
            foreach (var districtBlockLink in districtBlockLinks)
            {
                var blocksListPage =   GetHtmlPage(districtBlockLink.Value);
                ParseBlocksListPage(blocksListPage, districtBlockLink.Key);
            }
        }

        public Dictionary<District, string> ParseDistrictsListPage(HtmlDocument document, State state)
        {
            try
            {
                var dict = new Dictionary<District, string>();

                var tableNodes = document.GetElementbyId("tbody").ChildNodes;
                for (int i = 1; i < tableNodes.Count; i += 2)
                {
                    var districtNode = tableNodes[i].Descendants("a").First();
                    var subDistrictNode = tableNodes[i].Descendants("a").Skip(1).First();
                    var districtName = Utils.GetNormalizedName(districtNode.InnerText);
                    Console.WriteLine("Populating District: {0}", districtName);
                    var district = state.Districts.FirstOrDefault(x => x.Name.Equals(districtName)) ??
                                   state.Districts.First(
                                       x => Utils.LevenshteinDistance(x.Name, districtName) < 2);
                    var districtInfoLink = districtNode.Attributes["href"].Value;
                    // TODO: Parse Cities & Metropolitan areas
                    var districtInfoDoc = GetHtmlPage(districtInfoLink);
                    ParseDistrictInfo(districtInfoDoc, district);
                    var blocksListLink = subDistrictNode.Attributes["href"].Value;

                    dict.Add(district, blocksListLink);
                }
                return dict;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
        }

        public void ParseDistrictInfo(HtmlDocument document, District district)
        {
            // TODO: multiple tables with same Id .. BAD HTML .. Extract from text ?
            // Solution: String replace to unique ids and reload html
            var tableNode = document.GetElementbyId("table");

            var rows = GetNodesOfAClass(tableNode, "d1").Union(GetNodesOfAClass(tableNode, "d2"))
                    .Union(GetNodesOfAClass(tableNode, "d1 bold"))
                    .Union(GetNodesOfAClass(tableNode, "d2 bold"));
            var districtParams = new DistrictParameters();

            foreach (var rowNode in rows)
            {
                string rowHeader = rowNode.ChildNodes[1].InnerText;

            }            
        }

        public void ParseBlocksListPage(HtmlDocument document, District district)
        {
            var mainDiv = document.GetElementbyId("main");
            var rowNodes = GetNodesOfAClass(mainDiv, "row");

            foreach (var rowNode in rowNodes)
            {
                var linkNode = rowNode.Descendants("a").First(); // There is only one
                string blockName = Utils.GetNormalizedName(linkNode.InnerText);
                int blockId = Int32.Parse(
                        rowNode.Descendants("div")
                            .First(
                                x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Equals("cell mhide"))
                            .InnerText);
                string blockLink = linkNode.Attributes["href"].Value;

                // Add new block
                AddBlockAndInfo(blockName, blockId, blockLink, district);
            }
        }

        public void AddBlockAndInfo(string blockName, int blockId, string url, District district)
        {
            Console.WriteLine("block:{0}\tdistrict:{1}", blockName, district.Name);
            var newBlock = district.AddBlock(blockName, blockId);
            var blockInfoDoc =   GetHtmlPage(url);
            var tableNodes = GetNodesOfAClass(blockInfoDoc.DocumentNode, "table");

            foreach (var tableNode in tableNodes)
            {
                var headRowNodes = GetNodesOfAClass(tableNode, "headrow");
                var townOrVillage = headRowNodes.First().ChildNodes[3].InnerText;
                    GetNodesOfAClass(tableNode, "row").ToList().ForEach(x =>
                    {
                        var linkNode = x.Descendants("a").First();
                        string townUrl = linkNode.Attributes["href"].Value;
                        int id = Int32.Parse(x.Descendants("div").First(
                            y => y.Attributes.Contains("class") && y.Attributes["class"].Value.Equals("cell mhide")).InnerText);
                        string name = linkNode.InnerText;

                        if (townOrVillage.Equals("Town") && name.Contains("Census Town"))
                        {
                            AddCensusTownInfo(name, id, townUrl, newBlock);                            
                        }
                        else if (townOrVillage.Equals("Town"))
                        {
                            AddMunicipalityInfo(name, id, townUrl, newBlock);                            
                        }
                        else if (townOrVillage.Contains("Village"))
                        {
                            AddVillageInfo(name, id, townUrl, newBlock);
                        }
                    }
                    );
            }
        }

        public void AddMunicipalityInfo(string townName, int townId, string url, Block block)
        {
            var newMC = block.AddMunicipalCorp(townName, townId);
            if (newMC == null) { return; }
            var municipalityDocument =   GetHtmlPage(url);
            ParseAndAddMunicipalityInfo(municipalityDocument, newMC);
        }

        public void ParseAndAddMunicipalityInfo(HtmlDocument doc, MunicipalCorp mc)
        {
            var tableNodes = GetNodesOfAClass(doc.DocumentNode, "table");
            foreach (var tableNode in tableNodes)
            {
                var headings = GetNodesOfAClass(tableNode, "headrow").First().ChildNodes.Select(x => x.InnerText);
                var rows = GetNodesOfAClass(tableNode, "row").ToList();
                if (headings.Contains("Ward"))
                {
                    rows.ForEach(x => mc.AddWard(x.Descendants("div").First(y => y.Attributes.Contains("class") 
                        && y.Attributes["class"].Value.Equals("cell")).InnerText));
                }
                else
                {
                    mc.TotalPopulation = Int32.Parse(rows[0].ChildNodes[5].InnerText); // TODO: Indexing by number is very fragile
                }
            }
        }

        public void AddCensusTownInfo(string censusTownName, int id, string url, Block block)
        {
            var newCensusTown = block.AddCensusTown(censusTownName, id);
            if (newCensusTown == null) { return; }
            var censusTownDocument =   GetHtmlPage(url);
            ParseAndAddCensusTownInfo(censusTownDocument, newCensusTown);
        }

        public void ParseAndAddCensusTownInfo(HtmlDocument doc, CensusTown town)
        {
            var allSentences = doc.GetElementbyId("main")
                .Descendants()
                .Select(x => x.InnerText)
                .SelectMany(y => Regex.Split(y, @"(?<=[\.!\?])\s+")).ToArray();

            var popRegex = new Regex(@"population of ([\d+,]+) of which ([\d,]+) are males while ([\d,]+) are females");
            var childPopRegex = new Regex(@"Children with age of 0-6 is ([\d,]+) which is ([\d,\.]+) % of total");
            var sexRatioRegex = new Regex(@"Female Sex Ratio is of ([\d,]+) against");
            var childSexRatioRegex = new Regex(@"Child Sex Ratio in ([a-zA-Z\s]+) is around ([\d,]+)");
            var literacyRateRegex = new Regex(@"Literacy rate of ([a-zA-Z\s]+) is ([\d.]+) %");
            var literacyByGenderRegex = new Regex(@"Male literacy is around ([\d\.]+) % while female literacy rate is ([\d\.]+) %");
            var houseCountRegex = new Regex(@"has total administration over ([\d,]+) houses");
            // TODO: (rapanuga) Handle patterns where STs don't exist
            var casteRegex = new Regex(
                @"Schedule Caste \(SC\) constitutes ([\d\.]+) % while Schedule Tribe \(ST\) were ([\d\.]+) % of total population");

            var workersRegex = new Regex(@"Out of total population, ([\d,]+) were");
            var genderWorkersRegex = new Regex(@"Of this ([\d,]+) were males while ([\d,]+) were females");
            var marginalWorkerRegex =
                new Regex(
                    @"working population, ([\d\.]+) % were engaged in Main Work while ([\d\.]+) % of total workers were engaged in Marginal Work");


            //Console.WriteLine(allSentences.Length);
            var censusTownParams = new CensusTownParams();

            censusTownParams.Population = new Dictionary<Gender, int>
            {
                {Gender.M, Int32.Parse(popRegex.Match(allSentences.First(x=> popRegex.Match(x).Success)).Groups[2].Value, NumberStyles.AllowThousands)},
                {Gender.F, Int32.Parse(popRegex.Match(allSentences.First(x=> popRegex.Match(x).Success)).Groups[3].Value, NumberStyles.AllowThousands)},
            };

            int totalChildren = Int32.Parse(childPopRegex.Match(allSentences.First(x => childPopRegex.Match(x).Success))
                .Groups[1].Value, NumberStyles.AllowThousands);
            int sexRatio = Int32.Parse(childSexRatioRegex.Match(allSentences.First(x => childSexRatioRegex.Match(x).Success))
                    .Groups[2].Value, NumberStyles.AllowThousands);

            censusTownParams.Children = new Dictionary<Gender, int>
            {
                {Gender.M, totalChildren * (1000/(1000+sexRatio))},
                {Gender.F, totalChildren * (sexRatio/(1000+sexRatio))},
            };

            censusTownParams.Literacy = double.Parse(
                    literacyRateRegex.Match(allSentences.First(x => literacyRateRegex.Match(x).Success)).Groups[2].Value);
            censusTownParams.LiteracyByGender = new Dictionary<Gender, double>
            {
                {Gender.M, double.Parse(literacyByGenderRegex.Match(allSentences
                    .First(x=> literacyByGenderRegex.Match(x).Success)).Groups[1].Value)},
                {Gender.F, double.Parse(literacyByGenderRegex.Match(allSentences
                    .First(x=> literacyByGenderRegex.Match(x).Success)).Groups[2].Value)},
            };

            censusTownParams.NoOfHouses =
                Int32.Parse(houseCountRegex.Match(allSentences.First(x => houseCountRegex.Match(x).Success))
                .Groups[1].Value, NumberStyles.AllowThousands);

            // TODO: (rapanuga) Handle patterns where STs don't exist
            censusTownParams.SCs = double.Parse(casteRegex.Match(allSentences.First(x => casteRegex.Match(x).Success)).Groups[1].Value);
            censusTownParams.STs = double.Parse(casteRegex.Match(allSentences.First(x => casteRegex.Match(x).Success)).Groups[2].Value);

            censusTownParams.Workers = new Dictionary<Gender, int>
            {
                {Gender.M, Int32.Parse(genderWorkersRegex.Match(allSentences.First(x=> genderWorkersRegex.Match(x).Success))
                    .Groups[1].Value, NumberStyles.AllowThousands)},
                {Gender.F, Int32.Parse(genderWorkersRegex.Match(allSentences.First(x=> genderWorkersRegex.Match(x).Success))
                    .Groups[2].Value, NumberStyles.AllowThousands)},
            };

            censusTownParams.MainWorkers = double.Parse(marginalWorkerRegex.Match(allSentences
                .First(x => marginalWorkerRegex.Match(x).Success)).Groups[1].Value);
            censusTownParams.MarginalWorkers = double.Parse(marginalWorkerRegex.Match(allSentences
                .First(x => marginalWorkerRegex.Match(x).Success)).Groups[2].Value);
            town.Params = censusTownParams;
        }

        public void AddVillageInfo(string villageName, int id, string url, Block block)
        {
            var newVillage = block.AddVillage(villageName, id);
            if (newVillage == null) { return;}
            var villageDocument =   GetHtmlPage(url);
            ParseAndAddVillageInfo(villageDocument, newVillage);
        }

        public void ParseAndAddVillageInfo(HtmlDocument doc, Village village)
        {
            var doubleRegex = new Regex(@"(\d+.\d+)");
            var tableNode = doc.GetElementbyId("table");
            var rows =
                GetNodesOfAClass(tableNode, "d1")
                    .Union(GetNodesOfAClass(tableNode, "d2"))
                    .Union(GetNodesOfAClass(tableNode, "d1 bold"))
                    .Union(GetNodesOfAClass(tableNode, "d2 bold"));
            var villageParams = new VillageParameters();
            foreach (var rowNode in rows) // TODO:
            {
                string rowHeader = rowNode.ChildNodes[1].InnerText;
                if (rowHeader.Equals("Total No. of Houses"))
                {
                    villageParams.NoOfHouses = Int32.Parse(rowNode.ChildNodes[3].InnerText, NumberStyles.AllowThousands);
                }
                if (rowHeader.Equals("Population"))
                {
                    villageParams.Population = new Dictionary<Gender, int>
                    {
                        {Gender.M, Int32.Parse(rowNode.ChildNodes[5].InnerText, NumberStyles.AllowThousands)},
                        {Gender.F, Int32.Parse(rowNode.ChildNodes[7].InnerText, NumberStyles.AllowThousands)}
                    };
                }
                if (rowHeader.Equals("Child (0-6)"))
                {
                    villageParams.Children = new Dictionary<Gender, int>
                    {
                        {Gender.M, Int32.Parse(rowNode.ChildNodes[5].InnerText, NumberStyles.AllowThousands)},
                        {Gender.F, Int32.Parse(rowNode.ChildNodes[7].InnerText, NumberStyles.AllowThousands)}
                    };
                }
                if (rowHeader.Equals("Schedule Caste"))
                {
                    villageParams.SCs = new Dictionary<Gender, int>
                    {
                        {Gender.M, Int32.Parse(rowNode.ChildNodes[5].InnerText, NumberStyles.AllowThousands)},
                        {Gender.F, Int32.Parse(rowNode.ChildNodes[7].InnerText, NumberStyles.AllowThousands)}
                    };
                }
                if (rowHeader.Equals("Schedule Tribe"))
                {
                    villageParams.STs = new Dictionary<Gender, int>
                    {
                        {Gender.M, Int32.Parse(rowNode.ChildNodes[5].InnerText, NumberStyles.AllowThousands)},
                        {Gender.F, Int32.Parse(rowNode.ChildNodes[7].InnerText, NumberStyles.AllowThousands)}
                    };
                }
                if (rowHeader.Equals("Literacy"))
                {
                    villageParams.LiteracyGenderWise = new Dictionary<Gender, double>
                    {
                        {
                            Gender.M,
                            double.Parse(doubleRegex.Match(rowNode.ChildNodes[5].InnerText).Groups[1].Value)
                        },
                        {
                            Gender.F,
                            double.Parse(doubleRegex.Match(rowNode.ChildNodes[7].InnerText).Groups[1].Value)
                        }
                    };

                    villageParams.Literacy = double.Parse(doubleRegex.Match(rowNode.ChildNodes[3].InnerText).Value);
                }
                if (rowHeader.Equals("Total Workers"))
                {
                    villageParams.TotalWorkers = new Dictionary<Gender, int>
                    {
                        {Gender.M, Int32.Parse(rowNode.ChildNodes[5].InnerText, NumberStyles.AllowThousands)},
                        {Gender.F, Int32.Parse(rowNode.ChildNodes[7].InnerText, NumberStyles.AllowThousands)}
                    };
                }
                if (rowHeader.Equals("Main Worker"))
                {
                    villageParams.MainWorkers = Int32.Parse(rowNode.ChildNodes[3].InnerText, NumberStyles.AllowThousands);
                }
                if (rowHeader.Equals("Marginal Worker"))
                {
                    villageParams.MarginalWorkers = Int32.Parse(rowNode.ChildNodes[3].InnerText,
                        NumberStyles.AllowThousands);
                }
            }
            village.Parameters = villageParams;
        }

        public IEnumerable<HtmlNode> GetNodesOfAClass(HtmlNode node, string className)
        {
            return node.Descendants()
                .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Equals(className));
        }
    }
}
