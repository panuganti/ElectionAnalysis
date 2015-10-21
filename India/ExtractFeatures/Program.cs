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
            ExtractFeatures();
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
