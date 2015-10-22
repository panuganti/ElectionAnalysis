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
            //ExtractAcFeatures();
            ProcessCasteShareData();
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

        static void ProcessCasteShareData()
        {
            const string casteShareParamsRefinedFile =
                @"J:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\CasteSharesRefined.tsv";
            const string casteShareParamsPurifiedFile =
                @"J:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\casteShareParamsPurified.tsv";
            var allOrigCasteShares = File.ReadAllLines(casteShareParamsRefinedFile).Skip(1).Select(
                t =>
                {
                    var parts = t.Split('\t');
                return new { AcNo = parts[0], Caste = parts[1], Pop = parts[3], Percent = double.Parse(parts[4]), BJP = double.Parse(parts[5]), INC = double.Parse(parts[6]), RJD = double.Parse(parts[7]), JDU = double.Parse(parts[8]), LJP = double.Parse(parts[9]), Others = double.Parse(parts[10]) }; // Form this file first
                });
            var allCasteSharesSum = allOrigCasteShares.Select(x => new { AcNo = x.AcNo, Caste = x.Caste, Pop = x.Pop, Percent = x.Percent, BJP = x.BJP, INC = x.INC, RJD = x.RJD, JDU = x.JDU, LJP = x.LJP, Others = x.Others, Sum = x.BJP + x.INC + x.RJD + x.JDU + x.LJP + x.Others});
            var allCasteShares = allCasteSharesSum.Where(x => x.Sum > 100).Select(x => new { AcNo = x.AcNo, Caste = x.Caste, Pop = x.Pop, Percent = x.Percent, BJP = 100*x.BJP/x.Sum, INC = 100*x.INC/x.Sum, RJD = 100*x.RJD / x.Sum, JDU = 100*x.JDU / x.Sum, LJP = 100*x.LJP / x.Sum, Others = 100*x.Others/x.Sum }).ToList();
            var allCasteSharesSumLT100 = allCasteSharesSum.Where(x => x.Sum <= 100).Select(x => new { AcNo = x.AcNo, Caste = x.Caste, Pop = x.Pop, Percent = x.Percent, BJP = x.BJP, INC = x.INC, RJD = x.RJD, JDU = x.JDU, LJP = x.LJP, Others = 100 - x.BJP - x.INC - x.RJD - x.JDU - x.LJP});
            allCasteShares.AddRange(allCasteSharesSumLT100);
            var acCasteShares = allCasteShares.GroupBy(t => t.AcNo);
            var nGrouped = acCasteShares.Where(t => t.GroupBy(x=>x.Caste).Any(x=> x.Count()==2)).Select(x => x.Key).ToArray();
            var nPopGt100 = acCasteShares.Where(t => t.Sum(x => x.Percent) > 100).Select(x => new { Ac = x.Key, Sum = x.Sum(c => c.Percent) }).ToArray();
            var nPopEq0 = acCasteShares.Where(t => t.Sum(x => x.Percent) == 0).Select(x => x.Key).ToArray();
            File.WriteAllLines(casteShareParamsPurifiedFile, acCasteShares.SelectMany(acs =>
            {
                var sum = acs.Sum(x => x.Percent);
                var sumPop = acs.Sum(x => double.Parse(x.Pop));
                var localCasteShare = sum == 0 ? acs.Select(y => new { AcNo = y.AcNo, Caste = y.Caste, Pop = y.Pop, Percent = (double.Parse(y.Pop) * 100 / sumPop), BJP = y.BJP, INC = y.INC, RJD = y.RJD, JDU = y.JDU, LJP = y.LJP, Others = y.Others }) : acs; // derive % from total
                sum = sum == 0 ? 100 : sum;
                var casteGroups = localCasteShare.GroupBy(x => x.Caste);
                return casteGroups.Select(x => new { AcNo = x.First().AcNo, Caste = x.Key, Pop = x.First().Pop, Percent = x.Sum(z => z.Percent) * 100 / sum, BJP = x.Sum(z => z.BJP) / x.Count(), INC = x.Sum(z => z.INC) / x.Count(), RJD = x.Sum(z => z.RJD) / x.Count(), JDU = x.Sum(z => z.JDU) / x.Count(), LJP = x.Sum(z => z.LJP) / x.Count(), Others = x.Sum(z => z.Others) });
            }).OrderBy(x=> int.Parse(x.AcNo)).SelectMany(t=> { 
                    var arr = new List<string>();
                    arr.Add(String.Join("\t",new string []{ t.AcNo, t.Caste, t.Percent.ToString(), "bjp", t.BJP.ToString() }));
                    arr.Add(String.Join("\t", new string[] { t.AcNo, t.Caste, t.Percent.ToString(), "inc", t.INC.ToString() }));
                    arr.Add(String.Join("\t", new string[] { t.AcNo, t.Caste, t.Percent.ToString(), "rjd", t.RJD.ToString() }));
                    arr.Add(String.Join("\t", new string[] { t.AcNo, t.Caste, t.Percent.ToString(), "jdu", t.JDU.ToString() }));
                    arr.Add(String.Join("\t", new string[] { t.AcNo, t.Caste, t.Percent.ToString(), "ljp", t.LJP.ToString() }));
                    arr.Add(String.Join("\t", new string[] { t.AcNo, t.Caste, t.Percent.ToString(), "others", t.Others.ToString() }));
                    return arr;
                }));            
        }
    }
}
