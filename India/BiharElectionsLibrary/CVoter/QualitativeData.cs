using System.Collections.Generic;

namespace BiharElectionsLibrary
{
    public class QualitativeDataOfAConstituencyInAYear
    {
        public int Year { get; set; }
        public AssemblyConstituency AssemblyConstituency { get; set; }

        public List<Candidate> Candidates { get; set; }

        public Dictionary<LocalIssuesCategory, int> LocalIssueRatings { get; set; }

        public Dictionary<DevelopmentIssueCategory, int> DevelopmentIssueRatings { get; set; }

        public Dictionary<Candidate, Dictionary<CandidateParameter, int>> CandidateParameterRatings { get; set; }

        public Dictionary<PoliticalParty, Dictionary<PoliticalPartyParameter, int>> PartyParameterRatings { get; set; }

        public Dictionary<CasteCategory, double> CasteShare { get; set; }

        public Dictionary<CasteCategory, Dictionary<PoliticalParty, double>> CastePartySupport { get; set; }
    }

    public class CasteData
    {
        public Dictionary<Caste, double> CasteShare { get; set; }
        public Dictionary<Caste, Dictionary<PoliticalParty, double>> CastePartySupport { get; set; }
    }

    public class QualitativeDataOfAYear
    {
        public int Year { get; set; }

        public Dictionary<AssemblyConstituency, QualitativeDataOfAConstituencyInAYear> ConstituencyWiseQualitativeData { get; set; }

    }

    public class QualitativeData
    {
        public Dictionary<int, QualitativeDataOfAYear> YearWiseQualitativeData { get; set; }
    }
}
