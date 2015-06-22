using System;
using System.Collections.Generic;
using System.Linq;
using BiharElectionsLibrary;

namespace QualitativeDataAnalysis
{
    public class ExtractFeatures
    {
        public Dictionary<string, int> FeatureVector { get; set; }

        private readonly Constituency _constituency;
        private readonly CandidateDataDelete _candidate;
        private PoliticalParty _candidateParty;

        public ExtractFeatures(Constituency constituency, CandidateDataDelete candidate)
        {
            _constituency = constituency;
            _candidate = candidate;
            FeatureVector = new Dictionary<string, int>();
            ComputeFeatures();
        }

        private void ComputeFeatures()
        {
            _candidateParty = _candidate.Party;
            ComputeCandidateFeatures();
            ComputeCasteShareFeatures();
            ComputeLocalIssuesFeatures();
            ComputeDevelopmentFeatures();
            ComputePartyFeatures();
        }

        private void ComputePastElectionFeatures()
        {
            // 0. Did he contest previous election in this constituency

            // 1. Is LocalCandidate incumbent
            // 2. Years of incumbency
            // 3. Did candidate lose earlier election
            // 4. Did constituency vote differently in by-election
        }

        private void ComputeCandidateFeatures()
        {
            foreach (var parameterScore in _candidate.CandidateParameterScores)
            {
                if (!Utils.AllFeatures.Contains(Utils.GetNormalizedName(parameterScore.Key)))
                {
                    Utils.AllFeatures.Add(Utils.GetNormalizedName(parameterScore.Key));
                }
                FeatureVector.Add(Utils.GetNormalizedName(parameterScore.Key),
                    Convert.ToInt32(parameterScore.Value*255/10));
            }
        }

        private void ComputeCasteShareFeatures()
        {
            var sum = _constituency.CasteShareLookups.CasteDistribution.Values.Sum();
            if (sum > 100)
            {
                var keys = new List<CasteCategory>(_constituency.CasteShareLookups.CasteDistribution.Keys);
                foreach (var key in keys)
                {
                    _constituency.CasteShareLookups.CasteDistribution[key] =
                        2.55 * (_constituency.CasteShareLookups.CasteDistribution[key]*100)/sum;
                }
            }

            foreach (var casteParameter in _constituency.CasteShareLookups.CasteDistribution)
            {
                int featureValue = 0;
                if (_constituency.CasteShareLookups.CastePartySupport.ContainsKey(casteParameter.Key)
                    && _constituency.CasteShareLookups.CastePartySupport[casteParameter.Key].ContainsKey(_candidateParty))
                {
                    featureValue = Convert.ToInt32(casteParameter.Value/100 *
                                    (_constituency.CasteShareLookups.CastePartySupport[casteParameter.Key][_candidateParty]));
                }
                if (!Utils.AllFeatures.Contains(Utils.GetNormalizedName(String.Format("{0}_CastePercent_X_CastePartyPercent", casteParameter.Key))))
                {
                    Utils.AllFeatures.Add(Utils.GetNormalizedName(String.Format("{0}_CastePercent_X_CastePartyPercent", casteParameter.Key)));
                }

                FeatureVector.Add(Utils.GetNormalizedName(String.Format("{0}_CastePercent_X_CastePartyPercent", casteParameter.Key)), featureValue);
            }
        }

        private void ComputeLocalIssuesFeatures()
        {
            foreach (var localIssuesParameterScore in _constituency.LocalIssues.LocalIssuesParameterScores)
            {
                if (!Utils.AllFeatures.Contains(Utils.GetNormalizedName(String.Format("LocalIssues_{0}", localIssuesParameterScore.Key))))
                {
                    Utils.AllFeatures.Add(Utils.GetNormalizedName(String.Format("LocalIssues_{0}", localIssuesParameterScore.Key)));
                }
                FeatureVector.Add(Utils.GetNormalizedName(String.Format("LocalIssues_{0}", localIssuesParameterScore.Key)),
                    Convert.ToInt32(localIssuesParameterScore.Value*255/10));
            }
        }

        private void ComputeDevelopmentFeatures()
        {
            foreach (var rating in _constituency.DevelopmentRatings.DevelopmentParameterScores)
            {
                if (!Utils.AllFeatures.Contains(
                        Utils.GetNormalizedName(String.Format("DevelopmentRating_{0}", rating.Key))))
                {
                    Utils.AllFeatures.Add(Utils.GetNormalizedName(String.Format("DevelopmentRating_{0}", rating.Key)));
                }
                FeatureVector.Add(Utils.GetNormalizedName(String.Format("DevelopmentRating_{0}", rating.Key)),
                    Convert.ToInt32(rating.Value*255/10));
            }
        }

        private void ComputePartyFeatures()
        {
            if (_constituency.PartyInfo.PartyParameterScores.ContainsKey(_candidateParty))
            {
                foreach (var partyParameter in _constituency.PartyInfo.PartyParameterScores[_candidateParty])
                {
                    if (Utils.AllFeatures.Contains(Utils.GetNormalizedName(partyParameter.Key)))
                    {
                        Utils.AllFeatures.Add(Utils.GetNormalizedName(partyParameter.Key));
                    }
                    FeatureVector.Add(Utils.GetNormalizedName(partyParameter.Key), Convert.ToInt32(partyParameter.Value * 255 / 10));
                }
            }
        }
    }
}
