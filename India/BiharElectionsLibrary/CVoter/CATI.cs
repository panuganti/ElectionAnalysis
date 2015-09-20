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
        public int SocialGroup { get; set; }
        public int PrevAssemblyVote { get; set; }
        public int CurrentAssemblyVote { get; set; }
        public int AllianceVote { get; set; }
        public int LokSabhaVote { get; set; }
        public Condition LivingStandardLastOneYear { get; set; }
        public Condition LivingStandardNextYear { get; set; }
        public int ViewsAboutTodaysState { get; set; }
        public int ConnectionWithVillageLife { get; set; }
        public int BornInSameCityOrVillage { get; set; }
        #endregion C Questions

        #region CVoter Calculations
        public int Age { get; set; }
        public int EducationRecorded { get; set;}
        public int IncomeRecorded { get; set; }
        public int WorkingGroup { get; set; }
        public int SocialGroup { get; set; }
        public int AC10 { get; set; }
        public int LS14 { get; set; }
        public int VSVoteWithAlliance2015 { get; set; }
        public int Weight { get; set; }
        #endregion CVoter Calculations

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
    }
}
