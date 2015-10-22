using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiharElectionsLibrary;

namespace ExtractFeatures
{
    class Program
    {
        static void Main(string[] args)
        {
            ExtractAcFeatures();
        }

        private static void ExtractCandidateFeatures()
        {
            var allLines = File.ReadAllLines(@"I:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\CandParamsRefined.txt").Skip(1).Select(
                t =>
                {
                    var parts = t.Split('\t');
                    return new {AcNo = parts[0], CandidateName = parts[1], Availability = parts[2], Honesty = parts[3], Effectiveness = parts[4], Popularity = parts[5], ReligiousInfluence = parts[6], MusclePower = parts[7], FinancialStatus = parts[8], PartyLeadersSupport = parts[9], LocalLeadersSupport = parts[10], Winability = parts[11]};
                });

        }

        private static void ExtractAcFeatures()
        {
            const string devParamsRefinedFile = @"I:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\DevParamsRefined.tsv";
            const string localParamsRefinedFile = @"I:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\LocalIssuesRefined.tsv";
            const string acFeatureVectorFile = @"I:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\AcFeatureVector.tsv";
            var allDevParams = File.ReadAllLines(devParamsRefinedFile).Skip(1).Select(
                t =>
                {
                    var parts = t.Split('\t');
                    return new { AcNo = parts[0], DevParam = parts[1], Score = parts[2]};
                }).ToArray();
            var allLocalIssues = File.ReadAllLines(localParamsRefinedFile).Skip(1).Select(
                t =>
                {
                    var parts = t.Split('\t');
                    return new { AcNo = parts[0], LocalIssue = parts[1], Score = parts[2] };
                }).ToArray();
            var header = new List<string> {"AcNo"};
            var devParamsHeader = allDevParams.Select(t => "dev_" + t.DevParam).Distinct().ToArray();
            var allLocalsHeader = allLocalIssues.Select(t => "local_" + t.LocalIssue).Distinct().ToArray();
            header.AddRange(devParamsHeader);
            header.AddRange(allLocalsHeader);
            var featureVectors = new List<string[]> {header.ToArray()};
            for (int i = 1; i <= 243; i++)
            {
                var acDevParams = allDevParams.Where(t => t.AcNo == i.ToString()).ToArray();
                var acLocalIssues = allLocalIssues.Where(t => t.AcNo == i.ToString()).ToArray();
                var featureVector = header.Select(t => 0.ToString()).ToArray();
                featureVector[0] = i.ToString();
                for(int j=1; j< 1 + devParamsHeader.Length;j++)
                {
                    var devParam = acDevParams.Where(t => "dev_" + t.DevParam == header[j] && t.Score != 0.ToString()).ToArray();
                    switch (devParam.Length)
                    {
                        case 0: break;
                        case 1: featureVector[j] = devParam.First().Score; break;
                        default: featureVector[j] = (devParam.Sum(t => double.Parse(t.Score))/devParam.Length).ToString(); break;
                    }
                }
                for (int j = 1 + devParamsHeader.Length; j < 1 + devParamsHeader.Length + allLocalsHeader.Length; j++)
                {
                    var localParam = acLocalIssues.Where(t => "local_" + t.LocalIssue == header[j] && t.Score != 0.ToString()).ToArray();
                    switch (localParam.Length)
                    {
                        case 0: break;
                        case 1: featureVector[j] = localParam.First().Score; break;
                        default: featureVector[j] = (localParam.Sum(t => double.Parse(t.Score)) / localParam.Length).ToString(); break;
                    }
                }
                featureVectors.Add(featureVector);
            }
            File.WriteAllLines(acFeatureVectorFile, featureVectors.Select(t => String.Join("\t", t)));
        }

        private static void ExtractPartyFeatures()
        {
            /* party features.. + party caste features + alliance party features
             */
            const string partyParamsRefinedFile =
                @"I:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\PartyParamsRefined.tsv";
            const string casteShareParamsRefinedFile =
                @"I:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\CasteSharesRefined.tsv";
            const string acFeatureVectorFile =
                @"I:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\PartyFeatureVector.tsv";

            var allPartyParams = File.ReadAllLines(partyParamsRefinedFile).Skip(1).Select(
                t =>
                {
                    var parts = t.Split('\t');
                    return new {AcNo = parts[0], Party = parts[1], OrgStrength = parts[2], Unity = parts[3], BoothManagement = parts[4]};
                }).ToArray();
            var allCasteShares = File.ReadAllLines(casteShareParamsRefinedFile).Skip(1).Select(
                t =>
                {
                    var parts = t.Split('\t');
                    return new {AcNo = parts[0], Party = parts[1], Caste = parts[2], CasteCategory = parts[3], Percent = parts[4], Support = parts[5]}; // Form this file first
                }).ToArray();

            var featureVectors = new List<string[]> {};
            for (int i = 0; i < 243; i++)
            {

            }
            File.WriteAllLines(acFeatureVectorFile, featureVectors.Select(t => String.Join("\t", t)));
        }

        private static void ExtractFeatures()
        {
            string startingExtraction = @"I:/ArchishaData/ElectionData/Bihar/Predictions/Extraction2010.tsv";
            string candParamsFile = @"I:/ArchishaData/ElectionData/Bihar/CVoterData/2010/Qualitative/CombinedQualitativeData/CandParams.txt";
            string outExtraction = @"I:/ArchishaData/ElectionData/Bihar/Predictions/Extraction2010Out.tsv";
            var extractionHeader = File.ReadAllLines(startingExtraction).First().Split('\t');
            var extraction = File.ReadAllLines(startingExtraction).Skip(1).Select(t =>
            {
                var parts = t.Split('\t');
                return
                    new
                    {
                        AcNo = parts[0],
                        CandidateName = parts[1],
                        Party = parts[2],
                        Result = parts[3],
                        Params = String.Join("\t",parts.Skip(4))
                    };
            }).ToArray();
            var candParamsHeader = File.ReadAllLines(candParamsFile).First().Split('\t');
            var candParams = File.ReadAllLines(candParamsFile).Skip(1).Select(t =>
            {
                var parts = t.Split('\t');
                return
                    new
                    {
                        acId = parts[0],
                        Candidate = parts[1],
                        Party = parts[2],
                        Params = String.Join("\t",parts.Skip(3))
                    };
            }).ToArray();

            var newExtraction = new List<string>();
            var header = String.Format("{0}\t{1}\t{2}\n", "AcNo\tCandidateName\tParty\tResult",String.Join("\t",extractionHeader.Skip(4)), String.Join("\t", candParamsHeader.Skip(3)));
            foreach (var featureVector in extraction)
            {
                var possibleCandidates = candParams.Where(t => t.Candidate.ToLower() == featureVector.CandidateName.ToLower() && featureVector.AcNo == t.acId).ToArray();
                if (possibleCandidates.Length == 1)
                {
                    var cand = possibleCandidates.First();
                    newExtraction.Add(String.Join("\t", new[] { featureVector.AcNo, featureVector.CandidateName, featureVector.Party, featureVector.Result, featureVector.Params, String.Join("\t", cand.Params) }));
                }
                else // Assume no suitable candidate was available
                {
                    newExtraction.Add(String.Join("\t", new string[] { featureVector.AcNo, featureVector.CandidateName, featureVector.Party, featureVector.Result, featureVector.Params, String.Join("\t", candParamsHeader.Skip(3).Select(t=>"")) }));
                }
            }
            File.WriteAllText(outExtraction,header);
            File.AppendAllLines(outExtraction,newExtraction);
        }
    }
}
