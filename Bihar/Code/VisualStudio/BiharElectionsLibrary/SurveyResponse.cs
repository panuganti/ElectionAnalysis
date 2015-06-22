using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BiharElectionsLibrary
{
    public class SurveyResponse
    {
        public AssemblyConstituency AC { get; set; }
        public DateTime DateOfSurvey { get; set; }
        public PoliticalParty AllianceForIssue { get; set; }
        public Issues Issue { get; set; }
        public DecisiveFactor DecisiveFactor { get; set; }
        public Ratings Ratings { get; set; }
        public VotingPattern VotingPattern { get; set; }
        public ResponderProfile Responder { get; set; }
    }

    public class Ratings
    {
        public string PMCandidate { get; set; }
        public string CMCandidate { get; set; }
        public Dictionary<LocalIssuesCategory, Condition> LocalIssueRatings { get; set; }
        public Dictionary<CandidateParameter, int> LocalMLAPerformance { get; set; }
        public Dictionary<CandidateParameter, int> LocalMPPerformance { get; set; }
        public bool SatisfiedWithSittingCandidate { get; set; }
        public int WillYouVoteForSittingCandidate { get; set; }
        public Dictionary<AdminHierarchy, int> SatisfactionLevel { get; set; }
        public Dictionary<AdminHierarchy, int> WhomToChange { get; set; }
        public AdminHierarchy MostAngryAgainst { get; set; }        
    }

    public class VotingPattern
    {
        public PoliticalParty CurrentOrRecentPCParty { get; set; }
        public PoliticalParty PreviousACParty { get; set; }
        public PoliticalParty NextACParty { get; set; }
        public PoliticalParty PreviousPCParty { get; set; }
    }
    
    public class ResponderProfile
    {
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public Occupation Occupation { get; set; }
        public FamilyIncome MonthlyHouseholdIncome { get; set; }
        public Caste Caste { get; set; }
        public LocationType LocationType { get; set; }
    }

    public static class SurveyResponse2010
    {
        public static Dictionary<int, PoliticalParty> IdToPoliticalParties = new Dictionary<int, PoliticalParty>()
        {
            {1, PoliticalParty.bjp},
            {2, PoliticalParty.jdu},
            {3, PoliticalParty.bsp},
            {4, PoliticalParty.rjd},
            {5, PoliticalParty.ljp},
            {6, PoliticalParty.inc}
        };

        public static Dictionary<int, Issues> IdToIssues = new Dictionary<int, Issues>()
        {
            {1, Issues.Corruption},
            {2, Issues.Others},
            {3, Issues.Unemployment},
            {4, Issues.Poverty},
            {5, Issues.CommunalPolarization},
            {6, Issues.Development},
            {8, Issues.Inflation},
            {9, Issues.Services},
            {10, Issues.Others},
            {11, Issues.Others},
            {12, Issues.CommunalPolarization},
            {13, Issues.LawAndOrder},
            {14, Issues.Services},
            {15, Issues.Others},
            {16, Issues.Corruption},
            {17, Issues.Justice},
            {18, Issues.Naxalism},
            {19, Issues.NaturalCalamity},
            {20, Issues.Services},
            {21, Issues.LawAndOrder},
            {99, Issues.Others},
            {0, Issues.NoIssue}
        };

        public static List<SurveyResponse> LoadFromFile(string filename)
        {
            var responses = new List<SurveyResponse>();
            var allLines = File.ReadAllLines(filename).Skip(1).ToArray();

            foreach (var line in allLines)
            {
                var cols = line.Split('\t');
                var response = new SurveyResponse();

                response.DateOfSurvey = DateTime.Parse(cols[4]);
                response.AC = new AssemblyConstituency() {Name = cols[5], No = Int32.Parse(cols[3])};

                var votingPattern = new VotingPattern()
                {
                    CurrentOrRecentPCParty = IdToPoliticalParties[Int32.Parse(cols[40])],
                    PreviousACParty = IdToPoliticalParties[Int32.Parse(cols[37])],
                    NextACParty = IdToPoliticalParties[Int32.Parse(cols[38])],
                    PreviousPCParty = IdToPoliticalParties[Int32.Parse(cols[39])],
                };

                response.VotingPattern = votingPattern;

                var ratings = new Ratings();
                ratings.LocalIssueRatings = new Dictionary<LocalIssuesCategory, Condition>()
                {
                    {LocalIssuesCategory.Roads, (Condition) Int32.Parse(cols[6])},
                    {LocalIssuesCategory.FarmerIssues, (Condition) Int32.Parse(cols[7])},
                    {LocalIssuesCategory.Services, (Condition) Int32.Parse(cols[8])},
                    {LocalIssuesCategory.Education, (Condition) Int32.Parse(cols[9])},
                    {LocalIssuesCategory.Health, (Condition) Int32.Parse(cols[10])},
                    {LocalIssuesCategory.LawAndOrder, (Condition) Int32.Parse(cols[11])},
                    {LocalIssuesCategory.Corruption, (Condition) Int32.Parse(cols[12])},
                    {LocalIssuesCategory.Unemployment, (Condition) Int32.Parse(cols[9])},
                };

                response.Issue = IdToIssues[Int32.Parse(cols[55])];
                response.DecisiveFactor = (DecisiveFactor) Int32.Parse(cols[17]);

                ratings.SatisfactionLevel = new Dictionary<AdminHierarchy, int>()
                {
                    {AdminHierarchy.CentralGovt,Int32.Parse(cols[41])},
                    {AdminHierarchy.PM, Int32.Parse(cols[42])},
                    {AdminHierarchy.StateGovt, Int32.Parse(cols[46])},
                    {AdminHierarchy.CM, Int32.Parse(cols[47])},
                    {AdminHierarchy.MP, Int32.Parse(cols[49])},
                    {AdminHierarchy.MLA, Int32.Parse(cols[50])}
                };
            
                ratings.WhomToChange = new Dictionary<AdminHierarchy, int>();

                response.Ratings = ratings;

                var responder = new ResponderProfile()
                {
                    Gender = Utils.GetGender(cols[30]),
                    Age = Int32.Parse(cols[31]),
                    EducationLevel = (EducationLevel) Int32.Parse(cols[32]),
                    Occupation = (Occupation) Int32.Parse(cols[33]),
                    MonthlyHouseholdIncome = (FamilyIncome) Int32.Parse(cols[34]),
                    Caste = Utils.GetCaste(cols[36]),
                    LocationType = (LocationType) Int32.Parse(cols[57])
                };
                response.Responder = responder;

                responses.Add(response);
            }
            return responses;
        }
    }

    public static class SurveyResponse2009
    {
        #region Lookups

        public static Dictionary<int, Issues> IdToIssues = new Dictionary<int, Issues>()
        {
            {1, Issues.LawAndOrder},
            {2, Issues.Inflation},
            {3, Issues.Corruption},
            {4, Issues.Reservation},
            {5, Issues.Stability},
            {6, Issues.Services},
            {7, Issues.Services},
            {8, Issues.Unemployment},
            {9, Issues.CommunalPolarization},
            {10, Issues.Others},
            {11, Issues.FarmerIssues},
            {12, Issues.FarmerIssues},
            {0, Issues.Others}
        };

        public static Dictionary<int, DecisiveFactor> IdToDecisiveFactors = new Dictionary<int, DecisiveFactor>()
        {
            {1, DecisiveFactor.LocalCandidate},
            {2, DecisiveFactor.CMCandidate},
            {3, DecisiveFactor.Party},
            {4, DecisiveFactor.Party}
        };

        public static Dictionary<int, PoliticalParty> IdToAlliance = new Dictionary<int, PoliticalParty>()
        {
            {1, PoliticalParty.upa},
            {2, PoliticalParty.nda},
            {3, PoliticalParty.cpi},
            {4, PoliticalParty.others}
        };
        #endregion Lookups

        public static List<SurveyResponse> LoadFromFile(string filename)
        {
            var responses = new List<SurveyResponse>();
            var allLines = File.ReadAllLines(filename).Skip(1).ToArray();

            foreach (var line in allLines)
            {
                var cols = line.Split('\t');
                var response = new SurveyResponse();
                var votingPattern = new VotingPattern()
                {
                    CurrentOrRecentPCParty = Utils.GetParty(cols[6]),
                    PreviousACParty = Utils.GetParty(cols[69]),
                    NextACParty = Utils.GetParty(cols[70]),
                    PreviousPCParty = Utils.GetParty(cols[71]),
                };
                response.VotingPattern = votingPattern;

                response.DecisiveFactor = IdToDecisiveFactors[Int32.Parse(cols[9])]; // Note: Caste is not given as a factor
                response.Issue = IdToIssues[Int32.Parse(cols[10])];
                response.AllianceForIssue = IdToAlliance[Int32.Parse(cols[11])];

                var ratings = new Ratings();
                ratings.LocalIssueRatings = new Dictionary<LocalIssuesCategory, Condition>();
                ratings.LocalMLAPerformance = new Dictionary<CandidateParameter, int>();
                ratings.LocalMPPerformance = new Dictionary<CandidateParameter, int>();
                ratings.SatisfactionLevel = new Dictionary<AdminHierarchy, int>();
                ratings.WhomToChange = new Dictionary<AdminHierarchy, int>();

                response.Ratings = ratings;

                var responder = new ResponderProfile()
                {
                    Gender = Utils.GetGender(cols[62]),
                    Age = Int32.Parse(cols[63]),
                    EducationLevel = (EducationLevel)Int32.Parse(cols[64]),
                    Occupation = (Occupation)Int32.Parse(cols[65]),
                    MonthlyHouseholdIncome = Int32.Parse(cols[66]) == 5 ? FamilyIncome.GT1Lac:(FamilyIncome)Int32.Parse(cols[66]),
                    Caste = Utils.GetCaste(cols[68]),
                    LocationType = (LocationType)Int32.Parse(cols[87])
                };
                response.Responder = responder;

                responses.Add(response);
            }
            return responses;
        }
    }

    public static class SurveyResponse2014
    {
        public static Dictionary<int, AdminHierarchy> IdToAdminHierarchies = new Dictionary<int, AdminHierarchy>()
        {

        };
        public static List<SurveyResponse> LoadFromFile(string filename)
        {
            var responses = new List<SurveyResponse>();
            var allLines = File.ReadAllLines(filename).Skip(1).ToArray();

            foreach (var line in allLines)
            {
                var cols = line.Split('\t');
                var response = new SurveyResponse();
                var votingPattern = new VotingPattern()
                {
                    CurrentOrRecentPCParty = Utils.GetParty(cols[0]),
                    PreviousACParty = Utils.GetParty(cols[0]),
                    NextACParty = Utils.GetParty(cols[0]),
                    PreviousPCParty = Utils.GetParty(cols[0]),
                };

                response.VotingPattern = votingPattern;

                var ratings = new Ratings();
                ratings.LocalIssueRatings = new Dictionary<LocalIssuesCategory, Condition>();
                ratings.LocalMLAPerformance = new Dictionary<CandidateParameter, int>();
                ratings.LocalMPPerformance = new Dictionary<CandidateParameter, int>();
                ratings.SatisfactionLevel = new Dictionary<AdminHierarchy, int>();
                ratings.WhomToChange = new Dictionary<AdminHierarchy, int>();
                ratings.MostAngryAgainst = IdToAdminHierarchies[Int32.Parse(cols[0])];

                response.Ratings = ratings;

                var responder = new ResponderProfile()
                {
                    Gender = Utils.GetGender(cols[0]),
                    Age = Int32.Parse(cols[0]),
                    EducationLevel = (EducationLevel)Enum.Parse(typeof(EducationLevel), cols[0]),
                    Occupation = (Occupation)Enum.Parse(typeof(Occupation), cols[0]),
                    MonthlyHouseholdIncome = (FamilyIncome)Enum.Parse(typeof(FamilyIncome), cols[0]),
                    Caste = Utils.GetCaste(cols[0]),
                    LocationType = (LocationType)Enum.Parse(typeof(LocationType), cols[0])
                };
                response.Responder = responder;

                responses.Add(response);
            }
            return responses;
        }
    }
}

