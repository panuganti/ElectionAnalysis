using System;
using System.Collections.Generic;
using System.Linq;
using BiharElectionsLibrary;

namespace QualitativeDataAnalysis
{
    public class Constituency
    {
        public int AcNo { get; set; }
        public List<string> CasteDataIssues { get; set; }
        public List<string> DevelopmentDataIssues { get; set; }
        public List<string> CandidatesDataIssues { get; set; }
        public List<string> LocalIssuesDataIssues { get; set; }
        public List<string> PartyDataIssues { get; set; }

        public Constituency(int acNo, List<CandidateDataDelete> candidates, CasteShare casteShareLookups,
            DevelopmentRating developmentRatings, LocalIssues localIssues, Party partyInfo, Dictionary<Candidate, int> resultInfo)
        {
            _candidates = candidates;
            _casteShareLookups = casteShareLookups;
            _developmentRatings = developmentRatings;
            _localIssues = localIssues;
            _partyInfo = partyInfo;
            _resultInfo = resultInfo;
            AcNo =  acNo;
            CasteDataIssues = new List<string>();
            DevelopmentDataIssues = new List<string>();
            CandidatesDataIssues = new List<string>();
            LocalIssuesDataIssues = new List<string>();
            PartyDataIssues = new List<string>();
        }

        private IEnumerable<Candidate> GetTopThreeCandidates()
        {
            return _resultInfo.OrderByDescending(t => t.Value).Select(t=>t.Key).Take(1);
        }

        public void CheckCandidateInfoIntegrityLevel()
        {
            /* List of Checks
             * 1. Check if all parties candidates who fought are there & all of their information is there
             * 2. Check if at least top 3 candidates are there & all important parameters are there. If top 3 candidates are there but imp. parameters are missing
             * 3. Check for New CandidateDataDelete info
             */
            var wrongCandidateNames = new List<string> {"new", "new seat", "new face"};
            if (_candidates.Select(t=>t.CandidateName.Trim().ToLower()).Intersect(wrongCandidateNames).Any())
            {
                //Console.WriteLine("No: {0}, Error: {1}", No, "WrongCandidateName");
                CandidatesDataIssues.Add("WrongCandidateName");
            }


            // TODO: Q. What to do if a candidate info is found in another 'nearby' constituency
            var topThreeCandidates = GetTopThreeCandidates().ToArray();
            bool isCandidateInList = true;           
            foreach (var topThreeCandidate in topThreeCandidates)
            {
                if (!(_candidates.Any(t => Utils.CheckNameSimilarity(topThreeCandidate.Name, t.CandidateName))))
                {
                    isCandidateInList = false;
                }
            }
            if (!isCandidateInList)
            {
                //Console.WriteLine("Top 3 Candidates in {1}: {0}", String.Join(";",topThreeCandidates.Select(t=>t.Name.ToLower().Trim())), No);
                //Console.WriteLine("All Candidates in Survey:{1} {0}", String.Join(";",_candidates.Select(t=>t.CandidateName.ToLower())), No);
                CandidatesDataIssues.Add("SomeOfTop3CandidateInfoMissing");
            }            
        }

        public void CheckCasteShareInfoIntegrityLevel()
        {
            /* List of Checks
             * 1. Check if caste share estimates are available
             * 2. Check if caste share voting patter is available for at least top 3 candidates
             * 3. Check if the total adds up to 100
             */

            if (_casteShareLookups == null)
            {
                //Console.WriteLine("CasteDataNull for {0}",No);
                CasteDataIssues.Add("CasteDataMissing"); return;
            }

            if (_casteShareLookups.CasteDistribution == null || !_casteShareLookups.CasteDistribution.Any())
            {
                Console.WriteLine("CasteDistributionDataNull for {0}", AcNo);
                CasteDataIssues.Add("CasteDistributionDataMissing");
            }
            if (_casteShareLookups.CastePartySupport == null || !_casteShareLookups.CastePartySupport.Any())
            {
                Console.WriteLine("CastePartySupportNull for {0}", AcNo);
                CasteDataIssues.Add("CastePartySupportDataMissing");
            }              

        }

        public void CheckDevelopmentInfoIntegrityLevel()
        {
            if (_developmentRatings == null)
            {
                DevelopmentDataIssues.Add("DevelopmentRatingsDataMissing"); return;
            }

            if (_developmentRatings.DevelopmentParameterScores.Max(t=>t.Value) == 0)
            {
                DevelopmentDataIssues.Add("DevelopmentRatingsDataMissing");                
            }
        }

        public void LocalIssuesInfoIntegrityLevel()
        {
            if (_localIssues == null || _localIssues.LocalIssuesParameterScores.Values.Contains(0))
            {
                LocalIssuesDataIssues.Add("LocalIssuesDataMissing");
            }            
        }

        private PoliticalParty GetWinningParty()
        {
            return _resultInfo.OrderByDescending(t => t.Value).First().Key.Party;
        }


        public void PartyInfoIntegrityLevel()
        {
            PoliticalParty winningParty = GetWinningParty();

            if (_partyInfo == null)// 0 is strict..let's loosen it.
            {
                //Console.WriteLine("WinningParty: {0}", winningParty);
                PartyDataIssues.Add("PartyInfoDataMissing"); return;
            }

            if (!_partyInfo.PartyParameterScores.ContainsKey(winningParty)
                || (_partyInfo.PartyParameterScores[winningParty].Values.Count(t => t == 0) > 2))
            {
                /*
                if (!_partyInfo.PartyParameterScores.ContainsKey(winningParty))
                {
                    Console.WriteLine("WinningParty: {0}, ACNo: {1}", winningParty, No);
                    Console.WriteLine("PartyParameters Info does not exist");
                }
                 */
                PartyDataIssues.Add("PartyInfoDataMissing");                 
            }
        }

        public Dictionary<Candidate, int> ResultInfo
        {
            get { return _resultInfo; }
        }

        public List<CandidateDataDelete> Candidates
        {
            get { return _candidates; }
        }

        public CasteShare CasteShareLookups
        {
            get { return _casteShareLookups; }
        }

        public DevelopmentRating DevelopmentRatings
        {
            get { return _developmentRatings; }
        }

        public LocalIssues LocalIssues
        {
            get { return _localIssues; }
        }

        public Party PartyInfo
        {
            get { return _partyInfo; }
        }

        private readonly Dictionary<Candidate, int> _resultInfo;
        private readonly List<CandidateDataDelete> _candidates;
        private readonly CasteShare _casteShareLookups;
        private readonly DevelopmentRating _developmentRatings;
        private readonly LocalIssues _localIssues;
        private readonly Party _partyInfo;
    }
}
