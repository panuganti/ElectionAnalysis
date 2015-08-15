using System;
using System.Collections.Generic;
using System.Linq;
using BiharElectionsLibrary;

namespace QualitativeDataAnalysis
{
    public class CasteShare
    {
        public string District { get; set; }
        public int AssemblyId { get; set; }
        public string AssemblyName { get; set; }
        public string ParliamentaryName { get; set; }
        public Dictionary<CasteCategory, Dictionary<PoliticalParty, double>> CastePartySupport { get; set; }
        public Dictionary<CasteCategory, double> CasteDistribution { get; set; }

        public static List<CasteShare> BuildCasteShareLookups(string[] casteShareData)
        {
            var casteShareLookups = new List<CasteShare>();

            var casteShareDataForAnAssembly = new List<string> { casteShareData[1] };
            string currentAssemblyId = casteShareData[1].Split('\t')[1];

            for (var i = 2; i < casteShareData.Length; i++)
            {
                if (currentAssemblyId != casteShareData[i].Split('\t')[1])
                {
                    casteShareLookups.Add(BuildCasteShareDataForAnAssembly(casteShareDataForAnAssembly));
                    casteShareDataForAnAssembly = new List<string>();
                }
                casteShareDataForAnAssembly.Add(casteShareData[i]);
            }
            return casteShareLookups;
        }

        public static CasteShare BuildCasteShareDataForAnAssembly(IEnumerable<string> casteShareDataForAnAssembly)
        {
            var first = true;
            CasteShare casteShare = null;
            foreach (string[] columns in casteShareDataForAnAssembly.Select(entry => entry.Split('\t')))
            {
                if (first)
                {
                    first = false;
                    casteShare = new CasteShare
                    {
                        District = columns[0],
                        AssemblyId = Int32.Parse(columns[1]),
                        AssemblyName = columns[2],
                        ParliamentaryName = columns[3],
                        CastePartySupport = new Dictionary<CasteCategory, Dictionary<PoliticalParty, double>>(),
                        CasteDistribution = new Dictionary<CasteCategory, double>()
                    };
                }
                if (Utils.GetCasteCategory(columns[4]) == CasteCategory.error) { continue;}
                if (!casteShare.CasteDistribution.ContainsKey(Utils.GetCasteCategory(columns[4])))
                {
                    if (columns[5].Equals(String.Empty) && columns[8].Equals(String.Empty))
                    {
                        // Console.WriteLine("Empty CasteShare Value: {0}", String.Join("\t", columns));
                        casteShare.CasteDistribution.Add(Utils.GetCasteCategory(columns[4]), 0);
                    }
                    else
                    {
                        casteShare.CasteDistribution.Add(Utils.GetCasteCategory(columns[4]),
                            columns[5].Equals(String.Empty) ? double.Parse(columns[8]) : double.Parse(columns[5]));
                    }
                }

                if (!casteShare.CastePartySupport.ContainsKey(Utils.GetCasteCategory(columns[4])))
                {
                    casteShare.CastePartySupport.Add(Utils.GetCasteCategory(columns[4]), new Dictionary<PoliticalParty, double>());
                }

                if (columns[6].Equals(String.Empty))
                {
                    //Console.WriteLine("Empty CasteShare Value: {0}", String.Join("\t", columns));
                }
                if (casteShare.CastePartySupport[Utils.GetCasteCategory(columns[4])].ContainsKey(Utils.GetParty(columns[7])))
                {
                    continue;
                }
                casteShare.CastePartySupport[Utils.GetCasteCategory(columns[4])].Add(Utils.GetParty(columns[7]),
                    columns[6].Equals(String.Empty) ? 0 : double.Parse(columns[6]));
            }
            return casteShare;

            /*
             * TODO:
             * 1. Combine OTHER, OTHERS,OTHR into one
             * 2. Remove AC Name and Seat Details as Castes
             * 3. Combine the following 
             * {"Vaishya", "Vaishiya"}, {"Rest-OBC", "Rest OBC"}, {"Sahni", "Sahanis"}, {"Muslims", "Muslim"}, {"Kayasth","Kayastha"}, {"Dalit","Dalkit","Dali", "SC"},
             * {"Brahman", "Brahmin", "Brahmans", "Bhahman"}, {"Rajput", "Rajpoot"}
             * Q. Ask about how to combine hierarchies..Rajput, Rajput-Bumihar
             */
        }
    }
}
