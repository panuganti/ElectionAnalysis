using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BiharElectionsLibrary
{
    public class ExitPoll
    {
        public int Year { get; set; }
        public AssemblyConstituency AC { get; set; }
        public LocationType Location { get; set; }
        public string Phase { get; set; }
        public List<ExitPollSurveyResponse> Responses { get; set; }
    }

    public class ExitPollSurveyResponse
    {
        public Gender Gender { get; set; }
        public int AgeGroup { get; set; }
        public int Age { get; set; }
        public EducationLevel Education { get; set; }
        public Occupation Occupation { get; set; }
        public FamilyIncome MonthlyFamilyIncome { get; set; }
        public Caste Caste { get; set; }
        public LocationType LocationType { get; set; }
        public PoliticalParty PreviousACVote { get; set; }
        public PoliticalParty PreviousPCVote { get; set; }
        public Issues MainIssue { get; set; }
        public PoliticalParty CurrentACVote { get; set; }
        public PoliticalParty CurrentPCVote { get; set; }
    }

    public static class ExitPoll2010
    {
        #region Lookups

        #region Issues Map

        private static readonly Dictionary<int, Issues> IdToIssuesMap = new Dictionary<int, Issues>
        {
            {1, Issues.Development},
            {2, Issues.Inflation},
            {3, Issues.Party},
            {4, Issues.SittingMLAPerformance},
            {5, Issues.FavoriteMLA},
            {6, Issues.PoliticalIssue},
            {7, Issues.LawAndOrder},
            {8, Issues.CMCandidate},
            {9, Issues.LocalCandidate},
            {10, Issues.Others},
            {0, Issues.NoIssue}
        };

        #endregion Issues Map

        #region Party Map

        private static readonly Dictionary<int, PoliticalParty> IdToPoliticalPartyMap = new Dictionary
            <int, PoliticalParty>
        {
            {1, PoliticalParty.inc},
            {2, PoliticalParty.bjp},
            {3, PoliticalParty.cpi},
            {4, PoliticalParty.jdu},
            {5, PoliticalParty.rjd},
            {6, PoliticalParty.ljp},
            {7, PoliticalParty.bsp},
            {8, PoliticalParty.others},
            {9, PoliticalParty.cantSay},
            {99, PoliticalParty.missingVote},
            {0, PoliticalParty.nota}
        };

        #endregion Party Map

        #region CasteId Map

        private static readonly Dictionary<int, Caste> CasteIdMap = new Dictionary<int, Caste>
        {
            {11, Caste.chamar},
            {12, Caste.nunia},
            {13, Caste.koli},
            {14, Caste.paswan},
            {15, Caste.musahar},
            {16, Caste.pasi},
            {17, Caste.dom},
            {18, Caste.ramvasi},
            {10, Caste.sc},

            {20, Caste.st},
            {21, Caste.kol},
            {22, Caste.bheel},
            {23, Caste.gond},
            {24, Caste.muriya},
            {25, Caste.nut},

            {30, Caste.obc},
            {31, Caste.baniya},
            {32, Caste.yadav},
            {33, Caste.teli},
            {34, Caste.dhobhi},
            {35, Caste.kurmi},
            {36, Caste.koiri},
            {37, Caste.dhanuk},
            {38, Caste.panwari},
            {39, Caste.mallaha},

            {40, Caste.uch},
            {41, Caste.vaishya},
            {42, Caste.brahmin},
            {43, Caste.thakur},
            {44, Caste.kayastha},
            {45, Caste.bhumihar},
            {46, Caste.jain},
            {47, Caste.tyagi},

            {50, Caste.muslim},
            {51, Caste.shia},
            {52, Caste.sunni},

            {60, Caste.christian},
            {61, Caste.christian},

            {71, Caste.punjabi},

            {81, Caste.buddhist},
        };

        #endregion CasteId Map

        #endregion Lookups

        #region Helper Methods

        public static Issues GetIssue(int id)
        {
            return IdToIssuesMap[id];
        }

        public static PoliticalParty GetPreviousElectionVote(int id)
        {
            return IdToPoliticalPartyMap[id];
        }

        public static Caste GetCaste(int casteId)
        {
            return CasteIdMap[casteId];
        }

        public static CasteCategory GetCasteCategory(int casteId)
        {
            return LookUps.CasteToCasteCategoryMapping[CasteIdMap[casteId]];
        }

        #endregion Helper Methods

        #region LoadFromFile

        public static List<ExitPoll> LoadExitPollDataFromFile(string filename)
        {
            var exitPollResponses = new List<ExitPoll>();

            string[] allLines = File.ReadAllLines(filename).Skip(1).ToArray();
            foreach (var line in allLines)
            {
                ExitPoll constituencyResponse;
                var cols = line.Split('\t');
                if (exitPollResponses.Any(x => x.AC.No == Int32.Parse(cols[4])))
                {
                    constituencyResponse = exitPollResponses.First(x => x.AC.No == Int32.Parse(cols[4]));
                }
                else
                {
                    constituencyResponse = new ExitPoll
                    {
                        AC = new AssemblyConstituency {No = Int32.Parse(cols[4])},
                        Location = (LocationType) Enum.Parse(typeof (LocationType), cols[14]),
                        Phase = cols[6]
                    };
                    exitPollResponses.Add(constituencyResponse);
                }
                var response = new ExitPollSurveyResponse
                {
                    Gender = Utils.GetGender(cols[7]),
                    Age = Int32.Parse(cols[8]),
                    Caste = GetCaste(Int32.Parse(cols[9])),
                    PreviousACVote = GetPreviousElectionVote(Int32.Parse(cols[10])),
                    PreviousPCVote = GetPreviousElectionVote(Int32.Parse(cols[11])),
                    CurrentACVote = GetPreviousElectionVote(Int32.Parse(cols[12])),
                    MainIssue = GetIssue(Int32.Parse(cols[13])),
                };
                constituencyResponse.Responses.Add(response);
            }
            return exitPollResponses;
        }

        #endregion LoadFromFile
    }
    
    public static class ExitPoll2014
    {
        #region Lookups

        #region Smaller Lookups

        public static Dictionary<int, Issues> IdToIssuesMap = new Dictionary<int, Issues>
        {
            {1, Issues.Inflation},
            {2, Issues.Stability},
            {3, Issues.FarmerIssues},
            {4, Issues.Development},
            {5, Issues.MLAPerformance},
            {6, Issues.Corruption},
            {7, Issues.LocalCandidate},
            {8, Issues.StateGovtPerformance},
            {9, Issues.CentralGovtPerformance},
            {0, Issues.Others}
        };

        public static Dictionary<int, EducationLevel> IdToEducationLevels = new Dictionary<int, EducationLevel>
        {
            {1, EducationLevel.Literate},
            {2, EducationLevel.Primary},
            {3, EducationLevel.HighSchool},
            {4, EducationLevel.HigherSecondary},
            {5, EducationLevel.Graduate},
            {6, EducationLevel.PostGraduate},
            {7, EducationLevel.Professional},
            {0, EducationLevel.Illiterate}
        };

        public static Dictionary<int, Occupation> IdToOccupations = new Dictionary<int, Occupation>
        {
            {1, Occupation.StudentUnemployed},
            {2, Occupation.HouseWife},
            {3, Occupation.Farmer},
            {4, Occupation.FarmLaborer},
            {5, Occupation.FishingDiary},
            {6, Occupation.GovtEmployee},
            {7, Occupation.PrivateEmployee},
            {8, Occupation.SelfEmployed},
            {9, Occupation.CommonLaborer},
            {0, Occupation.Others}
        };

        public static Dictionary<int, FamilyIncome> IdToFamilyIncomes = new Dictionary<int, FamilyIncome>
        {
            {1, FamilyIncome.LT3K},
            {2, FamilyIncome.GT3KLT6k},
            {3, FamilyIncome.GT6KLT10K},
            {4, FamilyIncome.GT10KLT20K},
            {5, FamilyIncome.GT20KLT50K},
            {6, FamilyIncome.GT50KLT1Lac},
            {7, FamilyIncome.GT1Lac},
            {0, FamilyIncome.CantSay}
        };

        #endregion Smaller Lookups

        #region Caste Lookup

        #region CasteId Map

        public static readonly Dictionary<int, Caste> CasteIdMap = new Dictionary<int, Caste>
        {
            {11, Caste.chamar},
            {12, Caste.jatav},
            {13, Caste.pasi},
            {14, Caste.dhobhi},
            {15, Caste.koli},
            {16, Caste.valmiki},
            {17, Caste.dhanuk},
            {18, Caste.dom},
            {19, Caste.majhabi},
            {20, Caste.sc},

            {21, Caste.gond},
            {22, Caste.bheel},
            {23, Caste.sahariya},
            {24, Caste.st},


            {25, Caste.yadav},
            {26, Caste.jat},
            {27, Caste.kurmi},
            {28, Caste.koiri},
            {29, Caste.teli},
            {30, Caste.sunnar},
            {31, Caste.nishad},
            {32, Caste.rajbhar},
            {33, Caste.bind},
            {34, Caste.naayi},
            {35, Caste.kushwaha},
            {36, Caste.saini},
            {37, Caste.maali},
            {38, Caste.lohar},
            {39, Caste.obc},

            {41, Caste.brahmin},
            {42, Caste.thakur},
            {43, Caste.kayastha},
            {44, Caste.vaishya},
            {45, Caste.bhumihar},
            {46, Caste.arora},
            {47, Caste.khatri},
            {48, Caste.jain},
            {49, Caste.uch},

            {51, Caste.pathan},
            {52, Caste.shaikh},
            {51, Caste.sayed},
            {52, Caste.siddiqui},
            {51, Caste.ansari},
            {52, Caste.mansoori},
            {52, Caste.qureshi},
            {51, Caste.siya},
            {59, Caste.muslim},

            {60, Caste.others},
            {61, Caste.sikh},
            {62, Caste.christian},
            {63, Caste.buddhist},
        };

        #endregion CasteId Map

        #endregion Caste Lookup

        #region Party Map

        public static readonly Dictionary<int, PoliticalParty> IdToPoliticalPartyMap = new Dictionary
            <int, PoliticalParty>
        {
            {11, PoliticalParty.inc},
            {12, PoliticalParty.bjp},
            {13, PoliticalParty.bsp},
            {14, PoliticalParty.cpi},
            {15, PoliticalParty.cpi},
            {16, PoliticalParty.sp},
            {17, PoliticalParty.ncp},
            {18, PoliticalParty.aap},
            {19, PoliticalParty.others},
            {20, PoliticalParty.rjd},

            {21, PoliticalParty.others},
            {22, PoliticalParty.others},
            {23, PoliticalParty.cpi},
            {24, PoliticalParty.others},
            {25, PoliticalParty.others},
            {26, PoliticalParty.others},
            {27, PoliticalParty.others},
            {28, PoliticalParty.others},
            {29, PoliticalParty.others},
            {30, PoliticalParty.others},

            {31, PoliticalParty.others},
            {32, PoliticalParty.others},
            {33, PoliticalParty.others},
            {34, PoliticalParty.others},
            {35, PoliticalParty.others},
            {36, PoliticalParty.others},
            {37, PoliticalParty.others},
            {38, PoliticalParty.others},
            {39, PoliticalParty.others},
            {40, PoliticalParty.jmm},

            {41, PoliticalParty.others},
            {42, PoliticalParty.others},
            {43, PoliticalParty.others},
            {44, PoliticalParty.others},
            {45, PoliticalParty.ljp},
            {46, PoliticalParty.others},
            {47, PoliticalParty.others},
            {48, PoliticalParty.others},
            {49, PoliticalParty.others},
            {50, PoliticalParty.others},

            {51, PoliticalParty.others},
            {52, PoliticalParty.others},
            {53, PoliticalParty.others},
            {54, PoliticalParty.others},
            {55, PoliticalParty.others},
            {56, PoliticalParty.others},
            {57, PoliticalParty.others},
            {58, PoliticalParty.others},
            {59, PoliticalParty.others},
            {60, PoliticalParty.others},

            {61, PoliticalParty.others},
            {62, PoliticalParty.others},
            {63, PoliticalParty.others},
            {64, PoliticalParty.others},
            {65, PoliticalParty.others},
            {66, PoliticalParty.others},
            {67, PoliticalParty.others},
            {68, PoliticalParty.others},
            {69, PoliticalParty.others},
            {70, PoliticalParty.others},

            {71, PoliticalParty.others},
            {72, PoliticalParty.others},
            {73, PoliticalParty.others},
            {74, PoliticalParty.others},
            {75, PoliticalParty.others},

            {91, PoliticalParty.others},
            {92, PoliticalParty.others},
            {93, PoliticalParty.others},
            {94, PoliticalParty.others},
            {95, PoliticalParty.others},
            {96, PoliticalParty.others},
            {97, PoliticalParty.didNotVote},
            {98, PoliticalParty.missingVote},

            {999, PoliticalParty.nota},
            {98, PoliticalParty.missingVote},
            {99, PoliticalParty.cantSay},
            {0, PoliticalParty.didNotVote},
        };

        #endregion Party Map

        #endregion Lookups

        #region LoadFromFile

        public static List<ExitPoll> LoadExitPoll2014(string filename)
        {
            var exitPollResponses = new List<ExitPoll>();

            string[] allLines = File.ReadAllLines(filename).Skip(1).ToArray();
            foreach (var line in allLines)
            {
                ExitPoll constituencyResponse;
                var cols = line.Split('\t');
                var acNo = Int32.Parse(cols[5]);
                if (exitPollResponses.Any(x => x.AC.No == acNo))
                {
                    constituencyResponse = exitPollResponses.First(x => x.AC.No == acNo);
                }
                else
                {
                    constituencyResponse = new ExitPoll
                    {
                        AC = new AssemblyConstituency {No = acNo},
                        Location = (LocationType) Enum.Parse(typeof (LocationType), cols[17]),
                        Phase = cols[20]
                    };
                    exitPollResponses.Add(constituencyResponse);
                }
                var response = new ExitPollSurveyResponse
                {
                    Gender = Utils.GetGender(cols[8]),
                    Age = Int32.Parse(cols[9]),
                    Caste = CasteIdMap[Int32.Parse(cols[13])],
                    PreviousACVote = IdToPoliticalPartyMap[Int32.Parse(cols[14])],
                    PreviousPCVote = IdToPoliticalPartyMap[Int32.Parse(cols[16])],
                    CurrentACVote = IdToPoliticalPartyMap[Int32.Parse(cols[15])],
                    CurrentPCVote = IdToPoliticalPartyMap[Int32.Parse(cols[17])],
                    MainIssue = IdToIssuesMap[Int32.Parse(cols[7])],
                    Education = IdToEducationLevels[Int32.Parse(cols[10])],
                    Occupation = IdToOccupations[Int32.Parse(cols[11])],
                    MonthlyFamilyIncome = IdToFamilyIncomes[Int32.Parse(cols[12])]
                };
                constituencyResponse.Responses.Add(response);
            }
            return exitPollResponses;

        }

        #endregion LoadFromFile
    }

    public static class ExitPoll2009
    {
    }
}
