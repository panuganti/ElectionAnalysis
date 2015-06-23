using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BiharElectionsLibrary;

namespace GetBihar2010Results
{
    class Program
    {
        private static void Main(string[] args)
        {
            const string fileWith2005Results =
                @"E:\NMW\SurveyAnalytics\Bihar\Data\2005\Results\2005ElectionResults.tsv";
            const string fileWith2010Results =
                @"E:\NMW\SurveyAnalytics\Bihar\Data\2010\Results\Bihar2010_AssemblyResultsDetailed_AE2010_8913.tsv";
            const string stateDivisionsFilename = @"E:\NMW\SurveyAnalytics\Bihar\Data\AdminData\SubDivisions.txt";
            const string acInfoFilename = @"E:\NMW\SurveyAnalytics\Bihar\Data\AdminData\AssemblyConstituencies.txt";
            const string dirWith2014Results =
                @"E:\NMW\SurveyAnalytics\Bihar\Data\2014\Results_ACWise\InTsv";

            const string muslimPopulationFile = @"E:\NMW\SurveyAnalytics\Bihar\Data\2010\Results\MuslimPopulation.tsv";
            const string shapeFileDataFile = @"E:\NMW\SurveyAnalytics\Bihar\Data\AdminData\ShapefileData.tsv";
            const string outputDirPath = @"E:\NMW\SurveyAnalytics\Bihar\R_Analysis\VSOutputData";
            // Load all Files
            var results2005 = AssemblyConstituencyResult.Load2005ResultsFromFile(fileWith2005Results);
            var results2010 = AssemblyConstituencyResult.Load2010ResultsFromFile(fileWith2010Results);
            var results2014 = AssemblyConstituencyResult.Load2014ResultsFromFile(dirWith2014Results);
            var stateHierarchy = State.LoadDivisionsFromFile(stateDivisionsFilename);
            stateHierarchy.PopulateWithACInfoFromFile(acInfoFilename);
            var muslimPopulationData = MuslimPopulationData.LoadFromFile(muslimPopulationFile);
            
            var shapeFileData = ShapeFileData.LoadFromFile(shapeFileDataFile);

            //
            // 1. Generate ac, muslim population mapping
            var muslimPopulationByAcId = muslimPopulationData.GetMuslimPopulationByAcId(stateHierarchy);

            File.WriteAllText(Path.Combine(outputDirPath, "MuslimPopGt20.txt"),
                String.Format("ac\tmus_pop\n{0}",
                    String.Join("\n", stateHierarchy.ACs.OrderBy(x => x.No)
                            .Select( x => String.Format("{0}\t{1}", x.No,
                                        muslimPopulationByAcId.ContainsKey(x.No)
                                            ? Math.Ceiling(muslimPopulationByAcId[x.No]) 
                                            : 0)))));
            
            // 1.5 Print last names of all candidates of 2010

            var candidateLastNames = String.Join(";", stateHierarchy.ACs.OrderBy(x => x.No).Select(x =>  
                String.Join(";", results2010.First(y => y.Constituency.No == x.No).Votes.Keys.Select(z =>
                    CasteUtils.GetCasteFromName(z.Name) == Caste.others
                        ? CasteUtils.GetLastName(z.Name)
                        : String.Empty).Where(a => !a.Equals(String.Empty))))).Split(';').Distinct();
            File.WriteAllText(Path.Combine(outputDirPath, "CandidateLastNames.txt"),String.Join("\n",candidateLastNames));

            // 2. Generate ac, division id mapping
            File.WriteAllText(Path.Combine(outputDirPath,"DivisionIds.txt"),String.Format("ac\tdiv_id\n{0}",
                String.Join("\n",stateHierarchy.ACs.OrderBy(x=>x.No).Select(x=>String.Format("{0}\t{1}", x.No, x.PC.District.Division.No)))));

            File.WriteAllText(Path.Combine(outputDirPath, "Results2010.txt"), String.Format("ac\twinningParty\n{0}", String.Join("\n",
                stateHierarchy.ACs.OrderBy(x => x.No).Select(x => String.Format("{0}\t{1}", x.No, 
                results2010.First(y => y.Constituency.No == x.No).GetWinningParty().ToColor() )))));

            // 3. Generate ac, party which won mapping
            File.WriteAllText(Path.Combine(outputDirPath,"MusConstResults2010.txt"), String.Format("ac\twinningParty\n{0}", String.Join("\n", 
                stateHierarchy.ACs.OrderBy(x=>x.No).Select(x=>String.Format("{0}\t{1}", x.No, muslimPopulationByAcId.ContainsKey(x.No) ? 
                    results2010.First(y=>y.Constituency.No == x.No).GetWinningParty().ToColor() : "white")))));

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
