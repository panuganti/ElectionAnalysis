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

        private static void ExtractPartyFeatureVector()
        {

            const string acFeatureVectorFile = @"I:\ArchishaData\ElectionData\Bihar\Predictions\AcFeatureVector.tsv";
            const string casteShareParamsPurifiedFile =
                @"I:\ArchishaData\ElectionData\Bihar\Predictions\casteShareParamsPurified.tsv";
            const string extractionFile = @"I:\ArchishaData\ElectionData\Bihar\Predictions\Extraction2010.tsv";
            const string candParamsFile = @"I:\ArchishaData\ElectionData\Bihar\Predictions\CandParamsPurified.txt";
            const string partyParamsFile = @"I:\ArchishaData\ElectionData\Bihar\Predictions\PartyParamsRefined.tsv";

            var allResults = File.ReadAllLines(extractionFile).Skip(1).Select(
                t =>
                {
                    var parts = t.Split('\t');
                    return new { AcNo = parts[0], CandidateName = parts[1], Party = parts[2], Result = parts[3]};
                }).ToArray();

            // 1. Dev & Local are combined for every AcId
            var acFeatures = File.ReadAllLines(acFeatureVectorFile).Skip(1).Select(
                t =>
                {
                    var parts = t.Split('\t');
                    return new { AcNo = parts[0], Params = String.Join("\t",parts.Skip(1))};
                }).ToArray();

            var casteShareFeatures = File.ReadAllLines(casteShareParamsPurifiedFile).Select(
                t =>
                {
                    var parts = t.Split('\t');
                    return new { AcNo = parts[0], Caste = parts[1], Percent = parts[2], Party = parts[3], PartyPercent = parts[4]};
                }).ToArray();

            var paramFeatures = File.ReadAllLines(partyParamsFile).Skip(1).Select(
                t =>
                {
                    var parts = t.Split('\t');
                    return new { AcNo = parts[0], Party = parts[1], Params = String.Join("\t",parts.Skip(2)) };
                }).ToArray();

            var partyFeatures = File.ReadAllLines(candParamsFile).Skip(1).Select(
                t =>
                {
                    var parts = t.Split('\t');
                    return new { AcNo = parts[0], CandidateName = parts[1], Party = parts[2], ActualParty = parts[parts.Length - 1], ActualAcNo = parts[parts.Length - 1], Params = String.Join("\t", parts.Skip(1).Take(parts.Length - 5)) };
                }).ToArray();
            /*
            // 1.5 Join results & acfeatures
            var results_dev_local = allResults.Select(x =>
            {
                var acFeatureV = acFeatures.Where(y => y.AcNo == x.AcNo);
                var Params =
                acFeatureV.Any() ? acFeatureV.First() : String.Join("\t", );
                return new {AcNo = x.AcNo, CandName = x.CandidateName, Party = x.Party, Result = x.Result, AcParams = acFeatureV.};
            }
                );
             */
            //  2. Next join data for every Ac, Party


            // 3. Join for every Ac, Party, Cand

            // 4. Look for combined featuers
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
                @"D:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\CasteSharesRefined.tsv";
            const string casteSharePerAcParamsFile =
                @"D:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\casteSharePerAcParams.tsv";
            const string casteSharePerAcPartyParamsFile =
                @"D:\ArchishaData\ElectionData\Bihar\CVoterData\2010\Qualitative\CombinedQualitativeData\casteSharePerAcPartyParams.tsv";
            var allOrigCasteShares = File.ReadAllLines(casteShareParamsRefinedFile).Skip(1).Select(
                t =>
                {
                    var parts = t.Split('\t');
                return new { AcNo = parts[0], Caste = Category(parts[1]), Pop = parts[3], Percent = double.Parse(parts[4]), BJP = double.Parse(parts[5]), INC = double.Parse(parts[6]), RJD = double.Parse(parts[7]), JDU = double.Parse(parts[8]), LJP = double.Parse(parts[9]), Others = double.Parse(parts[10]) }; // Form this file first
                });
            var allCasteSharesSum = allOrigCasteShares.Select(x => new { AcNo = x.AcNo, Caste = x.Caste, Pop = x.Pop, Percent = x.Percent, BJP = x.BJP, INC = x.INC, RJD = x.RJD, JDU = x.JDU, LJP = x.LJP, Others = x.Others, Sum = x.BJP + x.INC + x.RJD + x.JDU + x.LJP + x.Others});
            var allCasteShares = allCasteSharesSum.Where(x => x.Sum > 100).Select(x => new { AcNo = x.AcNo, Caste = x.Caste, Pop = x.Pop, Percent = x.Percent, BJP = 100*x.BJP/x.Sum, INC = 100*x.INC/x.Sum, RJD = 100*x.RJD / x.Sum, JDU = 100*x.JDU / x.Sum, LJP = 100*x.LJP / x.Sum, Others = 100*x.Others/x.Sum }).ToList();
            var allCasteSharesSumLT100 = allCasteSharesSum.Where(x => x.Sum <= 100).Select(x => new { AcNo = x.AcNo, Caste = x.Caste, Pop = x.Pop, Percent = x.Percent, BJP = x.BJP, INC = x.INC, RJD = x.RJD, JDU = x.JDU, LJP = x.LJP, Others = 100 - x.BJP - x.INC - x.RJD - x.JDU - x.LJP});
            allCasteShares.AddRange(allCasteSharesSumLT100);
            var acCasteShares = allCasteShares.GroupBy(t => t.AcNo);
            var nGrouped = acCasteShares.Where(t => t.GroupBy(x=>x.Caste).Any(x=> x.Count()==2)).Select(x => x.Key).ToArray();
            var nPopGt100 = acCasteShares.Where(t => t.Sum(x => x.Percent) > 100).Select(x => new { Ac = x.Key, Sum = x.Sum(c => c.Percent) }).ToArray();
            var nPopEq0 = acCasteShares.Where(t => t.Sum(x => x.Percent) == 0).Select(x => x.Key).ToArray();
            var formattedData = new List<string>();
            formattedData.Add(String.Join("\t", new string[] { "AcNo", "Party", "UCHPercent", "YadavPercent", "OBCPercent", "DalitPercent", "MuslimPercent", "OthersPercent", "UCHSupport", "YadavSupport", "OBCSupport", "DalitSupport", "MuslimSupport", "OthersSupport" }));

            formattedData.AddRange(acCasteShares.Select(acs =>
            {
                var sum = acs.Sum(x => x.Percent);
                var sumPop = acs.Sum(x => double.Parse(x.Pop));
                var localCasteShare = sum == 0 ? acs.Select(y => new {
                                AcNo = y.AcNo,
                                Caste = y.Caste,
                                Pop = y.Pop,
                                Percent = (double.Parse(y.Pop) * 100 / sumPop),
                                BJP = y.BJP,
                                INC = y.INC,
                                RJD = y.RJD,
                                JDU = y.JDU,
                                LJP = y.LJP,
                                Others = y.Others
                            })
                    : acs; // derive % from total
                sum = sum == 0 ? 100 : sum;
                var casteGroups = localCasteShare.GroupBy(x => x.Caste);
                return  casteGroups.Select(x => new {
                                AcNo = x.First().AcNo,
                                Caste = x.Key,
                                Pop = x.First().Pop,
                                Percent = x.Sum(z => z.Percent) * 100 / sum,
                                BJP = x.Sum(z => z.BJP * z.Percent) / x.Sum(z => z.Percent),
                                INC = x.Sum(z => z.INC * z.Percent) / x.Sum(z => z.Percent),
                                RJD = x.Sum(z => z.RJD * z.Percent) / x.Sum(z => z.Percent),
                                JDU = x.Sum(z => z.JDU * z.Percent) / x.Sum(z => z.Percent),
                                LJP = x.Sum(z => z.LJP * z.Percent) / x.Sum(z => z.Percent),
                                Others = x.Sum(z => z.Others * z.Percent) / x.Sum(z => z.Percent)
                            });
            })
            .SelectMany(x =>
            {
                var xyz = new List<string>();
                xyz.Add(String.Join("\t", new string[] { x.First().AcNo, "bjp", x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").BJP.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").BJP.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").BJP.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").BJP.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").BJP.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").BJP.ToString() : 0.ToString() }));
                xyz.Add(String.Join("\t", new string[] { x.First().AcNo, "inc", x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").INC.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").INC.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").INC.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").INC.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").INC.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").INC.ToString() : 0.ToString() }));
                xyz.Add(String.Join("\t", new string[] { x.First().AcNo, "rjd", x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").RJD.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").RJD.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").RJD.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").RJD.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").RJD.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").RJD.ToString() : 0.ToString() }));
                xyz.Add(String.Join("\t", new string[] { x.First().AcNo, "jdu", x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").JDU.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").JDU.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").JDU.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").JDU.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").JDU.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").JDU.ToString() : 0.ToString() }));
                xyz.Add(String.Join("\t", new string[] { x.First().AcNo, "ljp", x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").LJP.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").LJP.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").LJP.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").LJP.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").LJP.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").LJP.ToString() : 0.ToString() }));
                xyz.Add(String.Join("\t", new string[] { x.First().AcNo, "others", x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").Percent.ToString() : 0.ToString(), x.Any(y => y.Caste == "uch") ? x.First(y => y.Caste == "uch").Others.ToString() : 0.ToString(), x.Any(y => y.Caste == "yadav") ? x.First(y => y.Caste == "yadav").Others.ToString() : 0.ToString(), x.Any(y => y.Caste == "obc") ? x.First(y => y.Caste == "obc").Others.ToString() : 0.ToString(), x.Any(y => y.Caste == "dalit") ? x.First(y => y.Caste == "dalit").Others.ToString() : 0.ToString(), x.Any(y => y.Caste == "muslim") ? x.First(y => y.Caste == "muslim").Others.ToString() : 0.ToString(), x.Any(y => y.Caste == "others") ? x.First(y => y.Caste == "others").Others.ToString() : 0.ToString() }));  
              return xyz;
            }
                ));
            File.WriteAllLines(casteSharePerAcPartyParamsFile, formattedData);
        }

        public static string Category(string caste)
        {
            switch (caste.ToLower())
            {
                case "yadav":
                case "dalit":
                case "muslim":
                case "obc":
                case "others":
                    return caste.ToLower();
                case "bhumihar":
                case "brahmin":
                case "rajput":
                case "baniya":
                case "thakur":
                case "kayastha":
                case "sahni":
                case "chaudhary":
                case "chauhan":
                    return "uch";
                case "kushwaha":
                case "sc":
                case "bind":
                case "paswan":
                case "kahar":
                case "mallaha":
                case "mandal":
                case "st":
                    return "dalit";
                case "kurmi":
                case "dhanuk":
                case "dhanuk/bin":
                case "nunia":
                case "ebc":
                    return "obc";
                default:
                    throw new Exception();
            }
        }
    }
}
