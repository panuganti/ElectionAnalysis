using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractFeatures
{
    class PrePoll
    {
        public static void ExtractPrePollFeatures2010()
        {
            string filename = @"I:\ArchishaData\ElectionData\RawData\CVoter\Bihar2010FinalValidVSNo.csv";
            string outfile = @"I:\ArchishaData\ElectionData\Bihar\CVoterData\2010\PrePoll\PrepollFeatures2010.tsv";
            var prepoll2010 = File.ReadAllLines(filename).Skip(1).Select(x =>
            {
                var parts = x.Split(',');
                return new { AcNo = parts[3], Gender = intParse(parts[25]), Age = intParse(parts[26]), Education = parseEduOccuIncome(parts[27]), Occupation = parseEduOccuIncome(parts[28]), FamilyIncome = parseEduOccuIncome(parts[29]), CasteCategory = CasteCategoryMapping(parts[30]), Caste = parts[31], LastAssemblyVote = intParse(parts[32]), VoteVS = intParse(parts[33]), LastPCVote = intParse(parts[34]), PCVote = intParse(parts[35]), UrbanRural = UrbanRural2010(parts[52]), LivingStd = intParse(parts[47]), ViewsAboutState = intParse(parts[48]), Weight = doubleParse(parts[53]) };
            });
            var outlines = new List<string>();
            outlines.Add(String.Join("\t", new[] { "AcNo", "Gender", "Age", "Education", "Occupation", "FamilyIncome", "CasteCategory", "PrevVS", "VS", "PrevPC", "PC", "UrbanRural", "LivingStd", "Views", "Weight" }));
            outlines.AddRange(prepoll2010.Select(x =>
                        String.Join("\t",
                            new[]
                            {
                                x.AcNo, x.Gender.ToString(), x.Age.ToString(), x.Education.ToString(), x.Occupation.ToString(), x.FamilyIncome.ToString(), x.CasteCategory.ToString(),
                                x.LastAssemblyVote.ToString(), x.VoteVS.ToString(), x.LastPCVote.ToString(), x.PCVote.ToString(), x.UrbanRural.ToString(),
                                x.LivingStd.ToString(), x.ViewsAboutState.ToString(), x.Weight.ToString()
                            })));
            File.WriteAllLines(outfile, outlines);
        }

        public static void ExtractPrePollFeatures2015()
        {
            string filename = @"I:\ArchishaData\ElectionData\RawData\CVoter\Bihar2015FinalValidVSNo.csv";
            string outfile = @"I:\ArchishaData\ElectionData\Bihar\CVoterData\2015\PrePoll\PrepollFeatures2015.tsv";
            var prepoll2015 = File.ReadAllLines(filename).Skip(1).Select(x =>
            {
                var parts = x.Split(',');
                return new { AcNo = parts[2], Gender = intParse(parts[4]), Age = intParse(parts[5]), Education = parseEduOccuIncome(parts[6]), Occupation = parseEduOccuIncome(parts[8]), FamilyIncome = parseEduOccuIncome(parts[9]), Caste = parts[10], LastAssemblyVote = PrevVS2015(parts[11]), VoteVS = PrevVS2015(parts[12]), LastPCVote = PrevVS2015(parts[14]), PCVote = PrevVS2015(parts[15]), CasteCategory = parts[25], UrbanRural = UrbanRural2015(parts[19]), LivingStd = intParse(parts[17]), ViewsAboutState = intParse(parts[18]), Weight = doubleParse(parts[30]) };
            });
            var outlines = new List<string>();
            outlines.Add(String.Join("\t", new[] { "AcNo", "Gender", "Age", "Education", "Occupation", "FamilyIncome", "CasteCategory", "PrevVS", "VS", "PrevPC", "PC", "UrbanRural", "LivingStd", "Views", "Weight" }));
            outlines.AddRange(prepoll2015.Select(x =>
                        String.Join("\t",
                            new[]
                            {
                                x.AcNo, x.Gender.ToString(), x.Age.ToString(), x.Education.ToString(), x.Occupation.ToString(), x.FamilyIncome.ToString(), x.CasteCategory.ToString(),
                                x.LastAssemblyVote.ToString(), x.VoteVS.ToString(), x.LastPCVote.ToString(), x.PCVote.ToString(), x.UrbanRural.ToString(),
                                x.LivingStd.ToString(), x.ViewsAboutState.ToString(), x.Weight.ToString()
                            })));
            File.WriteAllLines(outfile, outlines);
        }

        static double doubleParse(string str) { double val = 0; double.TryParse(str, out val); return val; }
        static int intParse(string str) { int val = 0; int.TryParse(str, out val); return val; }

        static int parseEduOccuIncome(string str) { return str == "NA" ? 99 : intParse(str); }

        #region 2010

        static int PrevVS2015(string str)
        {
            var val = intParse(str);
            switch (val)
            {
                case 12:
                case 80:
                case 79:
                case 77:
                case 94:
                    return 1;
                case 11:
                    return 6;
                case 92:
                case 81:
                case 20:
                    return 4;
                case 13:
                    return 3;
                case 37:
                    return 2;
                case 45:
                    return 5;
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 96:
                case 74:
                case 76:
                case 68:
                case 26:
                case 35:
                case 36:
                case 38:
                case 78:
                case 98:
                case 99:
                case 997:
                case 999:
                case 0:
                    return 7;
                default:
                    return 7;
            }
        }

        static int UrbanRural2010(string str)
        {
            var val = intParse(str);
            return val == 2 ? 1 : val;
        }
        #endregion 2010

        #region 2015
        static int UrbanRural2015(string str)
        {
            var val = intParse(str);
            switch (val)
            {
                case 2:
                case 3:
                    return 1;
                case 1:
                    return 3;
                case 0:
                    return 0;
                default:
                    throw new Exception();
            }
        }
        static int CasteCategoryMapping(string str)
        {
            var val = intParse(str);
            switch (val)
            {
                case 6:
                case 7:
                case 0:
                    return 5;
                case 5:
                    return 4;
                case 4:
                    return 3;
                case 3:
                    return 2;
                default:
                    return 5;
            }
        }
        #endregion 2015
    }
}
