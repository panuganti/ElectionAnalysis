using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiharElectionsLibrary.CVoter
{
    public class CATISurvey
    {
        public string State { get; set; }
        public string Dist { get; set; }
        public string AcName { get; set; }
        public int AcId { get; set; }
        public BiggestIssue BiggestIssue { get; set; }
        public PoliticalParty PartyThatCanSolve { get; set; }
        public YesNo AngryWithStateGovt { get; set; }
        public YesNo AngryWithCM { get; set; }
        public YesNo AngryWithMLA { get; set; }
        public YesNo AngryWithCentralGovt { get; set; }
        public YesNo AngryWithPM { get; set; }
        public YesNo AngryWithMP { get; set; }
        public PoliticalParty WhichPartyWillWin { get; set; }
        public CMCandidate CMPreference { get; set; }
        public PMCandidate BestPMCandidate { get; set; }
#region C Questions
        public Gender Gender { get; set; }
        public int ActualAge { get; set; }
        public EducationLevel Education { get; set; }
        public EducationLevel EducationChiefWageEarner { get; set; }
        public Occupation Occupation { get; set; }
        public FamilyIncome MonthlyFamilyIncome { get; set; }
        public Caste SocialGroup { get; set; }
        public PoliticalParty PrevAssemblyVote { get; set; }
        public PoliticalParty CurrentAssemblyVote { get; set; }
        public PoliticalParty AllianceVote { get; set; }
        public PoliticalParty LS14Vote { get; set; }
        public PoliticalParty LokSabhaVote { get; set; }
        public Condition LivingStandardLastOneYear { get; set; }
        public Condition LivingStandardNextYear { get; set; }
        public GrowthViews ViewsAboutTodaysState { get; set; }
        public ConnectionWithRural ConnectionWithVillageLife { get; set; }
        public BornAndBroughtUpInSamePlace BornInSameCityOrVillage { get; set; }

        #endregion C Questions

        #region CVoter Calculations
        public CVoterAge Age { get; set; }
        public CvoterEducation EducationRecorded { get; set;}
        public CVoterIncome IncomeRecorded { get; set; }
        public CVoterWorkingGroup WorkingGroup { get; set; }
        public CasteCategory SocialGroup { get; set; }
        public PoliticalParty AC10 { get; set; }
        public PoliticalParty LS14 { get; set; }
        public PoliticalParty VSVoteWithAlliance2015 { get; set; }
        public double Weight { get; set; }

        #endregion CVoter Calculations

        #region Enums
        public enum BiggestIssue
        { 
            Others = 0,
            ElectricySupply = 11,
            WaterSupply = 12,

        }

        public enum Gender
        {
            M = 1,
            F = 2,
            O,
            Error = 0
        }

        public enum EducationLevel
        {
            Illiterate = 0,
            Literate = 1,
            Primary = 2,
            HighSchool = 3,
            HigherSecondary = 4,
            Graduate = 5,
            PostGraduate = 6,
            Professional = 7,
            NoAnswer = 99
        }

        public enum CMCandidate
        {
            CantSay = 0,
            NitishKumar = 11,
            SushilModi = 12,
            JitanRamManjhi = 13,
            MishaYadav = 14,
            ShatrughanSinha = 15,
            RamVilasPaswan = 16,
            SyedShahnawazHussain = 17,
            RabriDevi = 18,
            NandKishoreYadav = 19,
            PappuYadav = 20,
            ShakeelAhmand = 21,
            AbdulBariSiddiqui = 22,
            TariqAnwar = 23,
            UpendraKushwaha = 24,
            RajivPratapRudy = 25,
            LaluPrasadYadav = 26,
            Others = 27
        }

        public enum PMCandidate
        {
            RahulGandhi = 11,
            NarendraModi = 12,
            NitishKumar = 13,
            SoniaGandhi = 14,
            LKAdvani = 15,
            Mayavati = 16,
            SushmaSwaraj = 17,
            MulayamSingh = 18,
            ManmohanSingh = 19,
            Jayalalitha = 20,
            MamtaBanerjee = 21,
            NaveenPatnaik = 22,
            PrakashSinghBadal = 23,
            MeiraKumar = 24,
            PriyankaGandhi = 25,
            ShivrajSinghChauhan = 26,
            ShielaDikshit = 27,
            ArunJaitley = 28,
            RajnathSingh = 29,
            LaluYadav = 30,
            ArvindKejriwal = 31,
            ChandrababuNaidu = 32,
            SharadPawar = 33,
            Chidambaram = 34,
            PrakashKarat = 35,
            Others = 98,
            CantSay = 99
        }


        #region CasteId Map

        private static readonly Dictionary<int, Caste> CasteIdMap = new Dictionary<int, Caste>
        {
            {11, Caste.chamar},
            {12, Caste.manjhi},
            {13, Caste.pasi},
            {14, Caste.dhobhi},
            {15, Caste.koiri},
            {16, Caste.paswan},
            {17, Caste.dhanuk},
            {18, Caste.dom},
            {19, Caste.tanti},
            {20, Caste.sc},

            {21, Caste.gond},
            {22, Caste.bheel},
            {23, Caste.sahariya},
            {24, Caste.st},

            {25, Caste.ahir},
            {26, Caste.kahar}, // It should be kanhar
            {27, Caste.kurmi},
            {28, Caste.koiri},
            {29, Caste.teli},
            {30, Caste.sunnar},
            {31, Caste.mallaha},
            {32, Caste.rajbhar},
            {33, Caste.noniya},
            {34, Caste.naayi},
            {35, Caste.kushwaha},
            {36, Caste.saini},
            {37, Caste.maali},
            {38, Caste.vishvakarma},
            {39, Caste.obc},

            //{40, Caste.uch},
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
            {53, Caste.sayed},
            {54, Caste.siddiqui},
            {55, Caste.ansari},
            {56, Caste.mansoori},
            {57, Caste.qureshi},
            {58, Caste.siya},
            {59, Caste.muslim},

            {60, Caste.others},
            {61, Caste.sikh},
            {62, Caste.christian},
            {63, Caste.buddhist}
        };

        #endregion CasteId Map
        #region Party Map

        private static readonly Dictionary<int, PoliticalParty> IdToPoliticalPartyMap = new Dictionary
            <int, PoliticalParty>
        {
            {0, PoliticalParty.didNotVote},
            {11, PoliticalParty.inc},
            {12, PoliticalParty.bjp},
            {14, PoliticalParty.cpi},
            {15, PoliticalParty.cpm},
            {16, PoliticalParty.sp},
            {17, PoliticalParty.ncp},
            {18, PoliticalParty.aap},
            {19, PoliticalParty.tmc},

            {4, PoliticalParty.jdu},
            {20, PoliticalParty.rjd},
            {22, PoliticalParty.aidmk},
            {23, PoliticalParty.aifb},
            {24, PoliticalParty.ainrc},
            {25, PoliticalParty.ajsu},
            {26, PoliticalParty.audf},
            {27, PoliticalParty.bjd},
            {28, PoliticalParty.bpf},
            {29, PoliticalParty.dmdk},
            {30, PoliticalParty.dmk},
            {31, PoliticalParty.ggp},
            {32, PoliticalParty.hjc},
            {33, PoliticalParty.hspdp},
            {34, PoliticalParty.inld},
            {35, PoliticalParty.mul},
            {36, PoliticalParty.jds},
            {37, PoliticalParty.jdu},
            {38, PoliticalParty.jknc},
            {39, PoliticalParty.jknpp},
            {40, PoliticalParty.jmm},
            {41, PoliticalParty.jvm},
            {42, PoliticalParty.kecm},
            {43, PoliticalParty.kjp},
            {44, PoliticalParty.kmk},
            {45, PoliticalParty.ljp},
            {46, PoliticalParty.mag},
            {47, PoliticalParty.mmk},
            {48, PoliticalParty.mdmk},
            {49, PoliticalParty.mnf},
            {50, PoliticalParty.mnf},
            {51, PoliticalParty.mpc},
            {52, PoliticalParty.mscp},
            {53, PoliticalParty.npf},
            {54, PoliticalParty.npp},
            {55, PoliticalParty.pda},
            {56, PoliticalParty.ppp},
            {57, PoliticalParty.pdp},
            {58, PoliticalParty.pmk},
            {59, PoliticalParty.ppi},
            {60, PoliticalParty.ppa},
            {61, PoliticalParty.pt},
            {62, PoliticalParty.rld},
            {63, PoliticalParty.rpia},
            {64, PoliticalParty.rpib},
            {65, PoliticalParty.rsp},
            {66, PoliticalParty.sad},
            {67, PoliticalParty.sdf},
            {68, PoliticalParty.shs},
            {69, PoliticalParty.suci},
            {70, PoliticalParty.tdp},
            {71, PoliticalParty.trs},
            {72, PoliticalParty.vck},
            {73, PoliticalParty.ysr},
            {74, PoliticalParty.udp},
            {75, PoliticalParty.znp},
            {76, PoliticalParty.aimim},
            {77, PoliticalParty.ham},
            {78, PoliticalParty.jan},
            {79, PoliticalParty.rlsp},

            {80, PoliticalParty.nda},
            {81, PoliticalParty.upa},
            {91, PoliticalParty.cai},
            {92, PoliticalParty.upai},
            {93, PoliticalParty.bai},
            {94, PoliticalParty.ndai},
            {95, PoliticalParty.ldfi},
            {96, PoliticalParty.others},
            {98, PoliticalParty.missingVote},
            {99, PoliticalParty.cantSay},
            {997, PoliticalParty.undecided},
            {999, PoliticalParty.nota},
        };

        private static readonly Dictionary<int, PoliticalParty> IdToAllianceMap = new Dictionary
    <int, PoliticalParty>
        {
            {0, PoliticalParty.nda},
            {0, PoliticalParty.upa},
            {0, PoliticalParty.nda},
            {0, PoliticalParty.nda},
        };
        #endregion Party Map

        public enum ConnectionWithRural 
        {
            CantSay = 0,
            LivingInVillage = 1,
            LiveInCityHasConnections = 2,
            No = 2,
        }
        public enum BornAndBroughtUpInSamePlace 
        { 
            CantSay = 0,
            BornAndBroughtUpHere = 1,
            BornHereBroughtUpElse = 2,
            BroughtUpHereBornElse = 3,
            Neither = 4,
        }
        #endregion Enums

        #region CVoter Calc Enums
        public enum CVoterAge
        { }

        public enum CvoterEducation
        {

        }

        public enum CVoterIncome
        { }


        #endregion CVoter Calc Enums
    }
}
