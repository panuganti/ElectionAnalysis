using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using BiharElectionsLibrary;
using GenerateDataForR;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using CVoterContracts;
using CVoterLibrary;

namespace GetBihar2010Results
{
    class Program
    {
        private static void Main(string[] args)
        {
            Startup();
        }


        private static void CustomExecution(CVoterContracts.QualitativeData data1, CVoterContracts.QualitativeData data2)
        {
            var filename1 = @"D:\ArchishaData\ElectionData\Bihar\Website\cvoterQualitative2010.json";
            var filename2 = @"D:\ArchishaData\ElectionData\Bihar\Website\cvoterQualitative2015.json";
            File.WriteAllText(filename1, JsonConvert.SerializeObject(data1, Formatting.Indented));
            File.WriteAllText(filename2, JsonConvert.SerializeObject(data2, Formatting.Indented));
        }

        private static void Startup()
        {
            #region Config

            string rootDir = File.ReadAllLines(@".\Config.ini").First();
            string stateDivisionsFilename = Path.Combine(rootDir,ConfigurationManager.AppSettings["StateDivisionsFilename"]);
            string acInfoFilename = Path.Combine(rootDir, ConfigurationManager.AppSettings["ACs"]);
            string censusDataDir = Path.Combine(rootDir, ConfigurationManager.AppSettings["CensusDataDirectory"]);
            string distListRelPath = Path.Combine(rootDir, ConfigurationManager.AppSettings["BiharDistrictsHtmlPage"]);

            string ACBye2014 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2014ByeACWiseResults"]);
            string AC2014 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2014ACWiseResults"]);
            string AC2010 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2010ACWiseResults"]);
            string AC2009 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2009ACWiseResults"]);
            string AC2005 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2005ACWiseResults"]);
            string PC2015 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2015PCResults"]);

            string Booth2014 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2014PollingBoothWiseResults"]);
            string Booth2009 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2009PollingBoothWiseResults"]);
            string Booth2010 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2010PollingBoothWiseResults"]);
            string Booth2005 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2005PollingBoothWiseResults"]);

            string Qual2015 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2015Qualitative"]);
            string Qual2010Cand = Path.Combine(rootDir, ConfigurationManager.AppSettings["2010QualitativeCandParams"]);
            string Qual2010Caste = Path.Combine(rootDir, ConfigurationManager.AppSettings["2010QualitativeCasteShares"]);
            string Qual2010Dev = Path.Combine(rootDir, ConfigurationManager.AppSettings["2010QualitativeDevelopmentParams"]);
            string Qual2010Issues = Path.Combine(rootDir, ConfigurationManager.AppSettings["2010QualitativeLocalIssues"]);
            string Qual2010Party = Path.Combine(rootDir, ConfigurationManager.AppSettings["2010QualitativePartyParams"]);

            string Exit2014 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2014ExitPoll"]);
            string Exit2010 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2010ExitPoll"]);
            string PrePoll2010 = Path.Combine(rootDir, ConfigurationManager.AppSettings["2010Prepoll"]);

            string musPopDistWise = Path.Combine(rootDir, ConfigurationManager.AppSettings["MuslimPopulationDistrictWise"]);
            string musPopACWise = Path.Combine(rootDir, ConfigurationManager.AppSettings["MuslimPopulationACWise"]);

            string stateJsonStore = Path.Combine(rootDir, ConfigurationManager.AppSettings["StateJson"]);
            string results2005Store = Path.Combine(rootDir, ConfigurationManager.AppSettings["Results2005Json"]);
            string results2010Store = Path.Combine(rootDir, ConfigurationManager.AppSettings["Results2010Json"]);
            string results2014Store = Path.Combine(rootDir, ConfigurationManager.AppSettings["Results2014Json"]);
            string villageParamsJsonStore = Path.Combine(rootDir, ConfigurationManager.AppSettings["VillageParamsJson"]);
            string districtParamsJsonStore = Path.Combine(rootDir, ConfigurationManager.AppSettings["DistrictParamsJson"]);
            string censusTownParamsJsonStore = Path.Combine(rootDir, ConfigurationManager.AppSettings["CensusTownParamsJson"]);
            
            string indiaVotesResults2014Dir = Path.Combine(rootDir, ConfigurationManager.AppSettings["2014ACWiseResultsIndiaVotes"]);
            string indiaVotesResults2010Dir = Path.Combine(rootDir, ConfigurationManager.AppSettings["2010ACWiseResultsIndiaVotes"]);
            string indiaVotesResults2009Dir = Path.Combine(rootDir, ConfigurationManager.AppSettings["2009ACWiseResultsIndiaVotes"]);

            string cVoter2015QualitativeDir = Path.Combine(rootDir, ConfigurationManager.AppSettings["cVoter2015QualitativeDir"]);
            
            #endregion Config

            var legend = Colors.LoadColors();

            #region Populate Info

            State state = PopulateInfo.LoadElectionHierarchy(stateDivisionsFilename, acInfoFilename);
                //PopulateInfo.LoadCensusData(state, censusDataDir, distListRelPath);
                File.WriteAllText(stateJsonStore, 
                    JsonConvert.SerializeObject(state,
                    new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, Formatting = Formatting.Indented})); //
            
            #endregion Populate Info

            #region Load Results


            //var indiaVotesResults2014 = ResultsLoader.LoadResultsFromIndiaVotesData(indiaVotesResults2014Dir, 2014);
            //var indiaVotesResults2014 = ResultsLoader.LoadResultsFromIndiaVotesData(indiaVotesResults2014Dir, 2014);
            // var indiaVotesResults2010 = ResultsLoader.LoadACResultsFromIndiaVotesData(indiaVotesResults2010Dir, 2010);

            /*
            List<ACResult> results2005;
            if (bool.Parse(ConfigurationManager.AppSettings["LoadNonStateJsons"]) && File.Exists(results2005Store))
            {
                var json = File.ReadAllText(results2005Store);
                results2005 = JsonConvert.DeserializeObject<List<ACResult>>(json);                
            }
            else
            {
                results2005 = ResultsLoader.Load2005ResultsFromFile(AC2005);
                File.WriteAllText(results2005Store,
                JsonConvert.SerializeObject(results2005,
                new JsonSerializerSettings
                {
                    //PreserveReferencesHandling = PreserveReferencesHandling.Objects
                })); //
            }

            List<ACResult> results2010;
            if (bool.Parse(ConfigurationManager.AppSettings["LoadNonStateJsons"]) && File.Exists(results2010Store))
            {
                var json = File.ReadAllText(results2010Store);
                results2010 = JsonConvert.DeserializeObject<List<ACResult>>(json);
            }
            else
            {
                results2010 = ResultsLoader.Load2010ResultsFromFile(AC2010);
                File.WriteAllText(results2010Store,
                    JsonConvert.SerializeObject(results2010,
                        new JsonSerializerSettings {PreserveReferencesHandling = PreserveReferencesHandling.Objects}));
            }
            
            List<ACResult> results2014;
            if (bool.Parse(ConfigurationManager.AppSettings["LoadNonStateJsons"]) && File.Exists(results2014Store))
            {
                var json = File.ReadAllText(results2014Store);
                results2014 = JsonConvert.DeserializeObject<List<ACResult>>(json);
            }
            else
            {
                results2014 = ResultsLoader.Load2014ResultsFromFile(AC2014);
                var intList = state.ACs.Select(t => t.No).ToArray();
                var resultList = results2014.Select(t => t.Constituency.No).ToArray();
                var diffList = intList.Except(resultList).Select(t => state.ACs.First(x => x.No == t).PC.No).Distinct().ToArray();
                File.WriteAllText(results2014Store,
                    JsonConvert.SerializeObject(results2014,
                        new JsonSerializerSettings {PreserveReferencesHandling = PreserveReferencesHandling.Objects}));
            }

            var pcResults = ResultsLoader.Load2015PCResults(PC2015, state);
            var results = ResultsConflator.ConflateResults(results2014, state);
            */
            #endregion Load Results
            /*
             #region Load Census Data
                VillageParameters villageParams;
            if (bool.Parse(ConfigurationManager.AppSettings["LoadNonStateJsons"]) && File.Exists(villageParamsJsonStore))
            {
                var json = File.ReadAllText(villageParamsJsonStore);
                villageParams = JsonConvert.DeserializeObject<VillageParameters>(json);                
            }
            else
            {
                File.WriteAllText(villageParamsJsonStore,
                    JsonConvert.SerializeObject(ParamDictionaries.VillageParams,
                    new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects })); //                
            }

            CensusTownParams censusTownParams;
            if (bool.Parse(ConfigurationManager.AppSettings["LoadNonStateJsons"]) && File.Exists(censusTownParamsJsonStore))
            {
                var json = File.ReadAllText(censusTownParamsJsonStore);
                censusTownParams = JsonConvert.DeserializeObject<CensusTownParams>(json);
            }
            else
            {
                File.WriteAllText(censusTownParamsJsonStore,
                    JsonConvert.SerializeObject(ParamDictionaries.CensusTownParams,
                    new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects })); //                
            }

            DistrictParameters districtParams;
            if (bool.Parse(ConfigurationManager.AppSettings["LoadNonStateJsons"]) && File.Exists(districtParamsJsonStore))
            {
                var json = File.ReadAllText(districtParamsJsonStore);
                districtParams = JsonConvert.DeserializeObject<DistrictParameters>(json);
            }
            else
            {
                File.WriteAllText(districtParamsJsonStore,
                    JsonConvert.SerializeObject(ParamDictionaries.DistrictParams,
                    new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects })); //                
            }
            
            #endregion Load Census Data
            */

            #region Load CVoter Data

            var qualitativeDataTuple = DataLoader.LoadDataFromDir(cVoter2015QualitativeDir);
            CustomExecution(qualitativeDataTuple.Item1, qualitativeDataTuple.Item2);
            #endregion Load CVoter Data

            #region Load Additional Info

            //var muslimPopulationData = MuslimPopulationData.LoadFromFile(musPopACWise);
            // 

            #endregion Load Additional Info


            #region Custom Execution
            //CustomExecution(results2014);
            #endregion Custom Execution
        }

        private static void OldMain()
        {
            /*
            const string fileWith2005Results =
                @"E:\NMW\SurveyAnalytics\Bihar\Data\2005\Results\2005ElectionResults.tsv";
            const string fileWith2010Results =
                @"E:\NMW\SurveyAnalytics\Bihar\Data\2010\Results\Bihar2010_AssemblyResultsDetailed_AE2010_8913.tsv";
            const string stateDivisionsFilename = @"E:\NMW\SurveyAnalytics\Bihar\Data\AdminData\SubDivisions.txt";
            const string acInfoFilename = @"E:\NMW\SurveyAnalytics\Bihar\Data\AdminData\AssemblyConstituencies.txt";
            const string dirWith2014Results =
                @"E:\NMW\SurveyAnalytics\Bihar\Data\2014\Results_ACWise\InTsv";

            // Load all Files
            var results2005 = ACResult.Load2005ResultsFromFile(fileWith2005Results);
            var results2010 = ACResult.Load2010ResultsFromFile(fileWith2010Results);
            var results2014 = ACResult.Load2014ResultsFromFile(dirWith2014Results);
            var stateHierarchy = State.LoadDivisionsAndDistrictsFromFile(stateDivisionsFilename);
            stateHierarchy.LoadPCsAndACs(acInfoFilename);
            */

            const string muslimPopulationFile = @"E:\NMW\SurveyAnalytics\Bihar\Data\2010\Results\MuslimPopulation.tsv";
            const string shapeFileDataFile = @"E:\NMW\SurveyAnalytics\Bihar\Data\AdminData\ShapefileData.tsv";
            const string outputDirPath = @"E:\NMW\SurveyAnalytics\Bihar\R_Analysis\VSOutputData";

            var muslimPopulationData = MuslimPopulationData.LoadFromFile(muslimPopulationFile);
            
            var shapeFileData = ShapeFileData.LoadFromFile(shapeFileDataFile);

            //
            // 1. Generate ac, muslim population mapping
            /*var muslimPopulationByAcId = muslimPopulationData.GetMuslimPopulationByAcId(stateHierarchy);

            File.WriteAllText(Path.Combine(outputDirPath, "MuslimPopGt20.txt"),
                String.Format("ac\tmus_pop\n{0}",
                    String.Join("\n", stateHierarchy.ACs.OrderBy(x => x.No)
                            .Select( x => String.Format("{0}\t{1}", x.No,
                                        muslimPopulationByAcId.ContainsKey(x.No)
                                            ? Math.Ceiling(muslimPopulationByAcId[x.No]) 
                                            : 0)))));
            */

            // 1.5 Print last names of all candidates of 2010

            /*
            var candidateLastNames = String.Join(";", stateHierarchy.ACs.OrderBy(x => x.No).Select(x =>  
                String.Join(";", results2010.First(y => y.Constituency.No == x.No).Votes.Keys.Select(z =>
                    CasteUtils.GetCasteFromName(z.Name) == Caste.others
                        ? CasteUtils.GetLastName(z.Name)
                        : String.Empty).Where(a => !a.Equals(String.Empty))))).Split(';').Distinct();
            File.WriteAllText(Path.Combine(outputDirPath, "CandidateLastNames.txt"),String.Join("\n",candidateLastNames));
            */

            // 2. Generate ac, division id mapping
            /*
            File.WriteAllText(Path.Combine(outputDirPath,"DivisionIds.txt"),String.Format("ac\tdiv_id\n{0}",
                String.Join("\n",stateHierarchy.ACs.OrderBy(x=>x.No).Select(x=>String.Format("{0}\t{1}", x.No, x.PC.District.Division.No)))));

            File.WriteAllText(Path.Combine(outputDirPath, "Results2010.txt"), String.Format("ac\twinningParty\n{0}", String.Join("\n",
                stateHierarchy.ACs.OrderBy(x => x.No).Select(x => String.Format("{0}\t{1}", x.No, 
                results2010.First(y => y.Constituency.No == x.No).GetWinningParty().ToColor() )))));
            */

            // 3. Generate ac, party which won mapping
            /*
            File.WriteAllText(Path.Combine(outputDirPath,"MusConstResults2010.txt"), String.Format("ac\twinningParty\n{0}", String.Join("\n", 
                stateHierarchy.ACs.OrderBy(x=>x.No).Select(x=>String.Format("{0}\t{1}", x.No, muslimPopulationByAcId.ContainsKey(x.No) ? 
                    results2010.First(y=>y.Constituency.No == x.No).GetWinningParty().ToColor() : "white")))));
            */

            /* 4. 2014 results
            var acs = stateHierarchy.ACs.OrderBy(x => x.No);
            var outputs = new List<string>();
            foreach (var ac in acs)
            {
                outputs.Add(String.Join("{0}\t{1}",ac.No,results2014.First(y => y.Constituency.No == ac.No).GetWinningParty()));
            }
            File.WriteAllText("./xyz.tx",String.Join("\n", outputs));
            File.WriteAllText("./AcIdWinningParty2014.txt", String.Format("ac\twinningParty\n{0}", String.Join("\n", stateHierarchy.ACs.OrderBy(x => x.No).Select(x => String.Format("{0}\t{1}", x.No, results2014.First(y => y.Constituency.No == x.No).GetWinningParty())))));
            */
            //var mapping = DelimitationMapping.GenerateDelimitationMapping(results2010, results2005);
            
        }
    }
}
