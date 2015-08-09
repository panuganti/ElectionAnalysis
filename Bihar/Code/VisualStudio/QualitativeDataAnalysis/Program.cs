using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BiharElectionsLibrary;

namespace QualitativeDataAnalysis
{
    class Program
    {
        private static void Main(string[] args)
        {
            #region FilesWithData

            const string parentPath = @"E:\NMW\SurveyAnalytics\Bihar\";

            string candidateQualitativeData = Path.Combine(parentPath,
                @"CVoterData\BiharQualitativeData_2010_CandidateParameters.tsv");
            string developmentQualitativeData = Path.Combine(parentPath,
                @"CVoterData\BiharQualitativeData_2010_DevelopmentParameters.tsv");
            string localIssuesQualitativeData = Path.Combine(parentPath,
                @"CVoterData\BiharQualitativeData_2010_LocalIssues.tsv");
            string casteShareQualitativeData = Path.Combine(parentPath,
                @"CVoterData\BiharQualitativeData_2010_CasteShare.tsv");
            string partyParametersQualitativeData = Path.Combine(parentPath,
                @"CVoterData\BiharQualitativeData_2010_PartyParameters.tsv");
            const string bihar2010Results = @"E:\NMW\SurveyAnalytics\Bihar\Bihar2010ElectionData\Bihar2010_AssemblyResultsDetailed_AE2010_8913.tsv";
            const string bihar2005Results = @"E:\NMW\SurveyAnalytics\Bihar\Bihar2005ElectionData\2005ElectionResults.tsv";

            #endregion FilesWithData

            #region ReadData

            string[] candidateData = File.ReadAllLines(candidateQualitativeData);
            string[] developmentData = File.ReadAllLines(developmentQualitativeData);
            string[] localIssuesData = File.ReadAllLines(localIssuesQualitativeData);
            string[] casteShareData = File.ReadAllLines(casteShareQualitativeData);
            string[] partyData = File.ReadAllLines(partyParametersQualitativeData);
            string[] resultInfo = File.ReadAllLines(bihar2010Results);

            /*
            var developmentRatings = DevelopmentRating.BuildDevelopmentRatingForAnAssembly(developmentData.Where(t=> Int32));
            var localIssuesRatings = LocalIssues.BuildLocalIssueRatings(localIssuesData); 
            var casteShares = CasteShare.BuildCasteShareLookups(casteShareData); 
            var partyInfoLookups = Party.BuildPartyInfoLookups(partyData); 
            */

            // Build Qualitative Data

            var results = new Dictionary<int, AssemblyConstituencyResult>();
            var constituencies = new List<Constituency>();
            for (int acNo = 1; acNo <= 243; acNo++)
            {
                int assemblyId = acNo;
                var resultOfAConstituency = AssemblyConstituencyResult.Load2010ElectionResults(
                        resultInfo.Skip(1).Where(t => Int32.Parse(t.Split('\t')[5]) == assemblyId).ToArray());
                results.Add(acNo,resultOfAConstituency);
                var constituencyCandidatesInfo =
                    candidateData.Skip(1).Where(t => Int32.Parse(t.Split('\t')[1]) == assemblyId);
                // 
                var constituencyCandidates = (from p in constituencyCandidatesInfo
                    group p by p.Split('\t')[4]
                    into g
                    select CandidateDataDelete.BuildSingleCandidate(g.ToList())).Where(t => t != null).ToList();

                var constituencyDevelopmentData =
                    DevelopmentRating.BuildDevelopmentRatingForAnAssembly(
                        developmentData.Skip(1).Where(t => Int32.Parse(t.Split('\t')[1]) == assemblyId).ToArray());
                var constituencyLocalIssuesData =
                    LocalIssues.BuildLocalIssuesForAnAssembly(
                        localIssuesData.Skip(1).Where(t => Int32.Parse(t.Split('\t')[0]) == assemblyId).ToArray());
                var constituencyPartyData =
                    Party.BuildPartyForAnAssembly(
                        partyData.Skip(1).Where(t => Int32.Parse(t.Split('\t')[0]) == assemblyId).ToArray());
                var constituencyCasteShareData =
                    CasteShare.BuildCasteShareDataForAnAssembly(
                        casteShareData.Skip(1).Where(t => Int32.Parse(t.Split('\t')[1]) == assemblyId).ToArray());
                constituencies.Add(new Constituency(acNo, constituencyCandidates,
                    constituencyCasteShareData,
                    constituencyDevelopmentData,
                    constituencyLocalIssuesData,
                    constituencyPartyData, resultOfAConstituency.Votes.ToDictionary(x => x.Candidate, y => y.Votes)));
            }

            // Load Previous Results
            var results2005 = AssemblyConstituencyResult.Load2005ResultsFromFile(bihar2005Results);

            // Data Integrity Checks
            foreach (var constituency in constituencies)
            {
                constituency.CheckCandidateInfoIntegrityLevel();
                constituency.CheckCasteShareInfoIntegrityLevel();
                constituency.CheckDevelopmentInfoIntegrityLevel();
                constituency.LocalIssuesInfoIntegrityLevel();
                constituency.PartyInfoIntegrityLevel();
            }


            // Pick Perfect Data Constituencies
            var noIssuesConstituencies = constituencies.Where(t =>
                !t.CasteDataIssues.Any() &&
                !t.CandidatesDataIssues.Any()
                && !t.DevelopmentDataIssues.Any() 
                && !t.LocalIssuesDataIssues.Any() // && 
                && !t.PartyDataIssues.Any()
                ).ToArray();

            /*
            Console.WriteLine("No. Of NoIssueConstituencies:{0}", noIssuesConstituencies.Count());
            Console.WriteLine("No. Of CasteData Issue Constituencies:{0}",
                constituencies.Count(t => t.CasteDataIssues.Any()));
            Console.WriteLine("No. Of CandidateData Issue Constituencies:{0}",
                constituencies.Count(t => t.CandidatesDataIssues.Any()));
            Console.WriteLine("No. Of DevelopmentData Issue Constituencies:{0}",
                constituencies.Count(t => t.DevelopmentDataIssues.Any()));
            Console.WriteLine("No. Of LocalIssuesDataIssues Issue Constituencies:{0}",
                constituencies.Count(t => t.LocalIssuesDataIssues.Any()));
            Console.WriteLine("No. Of PartyDataIssues Issue Constituencies:{0}",
                constituencies.Count(t => t.PartyDataIssues.Any()));
            */
            //Console.WriteLine(String.Join(";", noIssuesConstituencies.Select(t => t.No)));

            // Extract Features
            var writer = new StreamWriter("./2010_Extraction.tsv");
            LocalUtils.AllFeatures = new List<string>();
            var dict = new Dictionary<string, Dictionary<string, int>>();
            foreach (var constituency in noIssuesConstituencies)
            {
                foreach (var candidate in constituency.Candidates)
                {
                    /*
                    Console.WriteLine("Extracting features for No:{0}\tCandidate:{1}", constituency.No,
                        candidate.CandidateName);
                    */
                    var featureExtraction = new ExtractFeatures(constituency, candidate);
                    dict.Add(
                        String.Format("{0}\t{1}\t{2}", constituency.AcNo, candidate.Party, candidate.CandidateName),
                        featureExtraction.FeatureVector);
                }
            }

            writer.WriteLine(GetExtractionHeader());
            foreach (var keyValuePair in dict)
            {
                int constId = Int32.Parse(keyValuePair.Key.Split('\t')[0]);
                string candidateName = keyValuePair.Key.Split('\t')[2];
                string result = Utils.GetCandidatePositionRating(constId, candidateName, results);
                writer.WriteLine("{0}\t{2}{1}",keyValuePair.Key, GetExtractionVector(keyValuePair.Value), result);
                writer.Flush();
            }
            writer.Close();

            #endregion ReadData
        }

        private static string GetExtractionHeader()
        {
            const string featureVectorString = "No\tParty\tCandidate\tResult";
            return LocalUtils.AllFeatures.Aggregate(featureVectorString,
                (current, feature) => String.Format("{0}\t{1}", current, feature));
        }


        private static string GetExtractionVector(Dictionary<string, int> featureVector)
        {
            const string featureVectorString = "";
            return LocalUtils.AllFeatures.Aggregate(featureVectorString,
                (current, feature) =>
                    current + String.Format("\t{0}", featureVector.ContainsKey(feature) ? featureVector[feature] : 0));
        }
    }
}
