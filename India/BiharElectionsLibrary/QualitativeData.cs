using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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


        #region Load Data from file

        #region PartyParameters
        public static Dictionary<PoliticalParty, Dictionary<PoliticalPartyParameter, int>> BuildPartyParametersDataForAnAssembly(
            IEnumerable<string> partyDataForAnAssembly)
        {
            var partyParameterScores = new Dictionary<PoliticalParty, Dictionary<PoliticalPartyParameter, int>>();

            foreach (string[] columns in partyDataForAnAssembly.Select(entry => entry.Split('\t')))
            {
                if (columns[6].Equals(String.Empty))
                {
                    //Console.WriteLine("Empty PartyData Value: {0}", String.Join("\t", columns));
                }
                PoliticalParty politicalParty = Utils.GetParty(columns[4]);
                if (politicalParty == PoliticalParty.Error)
                {
                    continue;
                }
                if (!partyParameterScores.ContainsKey(politicalParty))
                {
                    partyParameterScores.Add(politicalParty, new Dictionary<PoliticalPartyParameter, int>());
                }

                if (partyParameterScores[politicalParty].ContainsKey(Utils.GetPoliticalPartyParameter(columns[5])))
                {
                    continue;
                }

                partyParameterScores[politicalParty].Add(Utils.GetPoliticalPartyParameter(columns[5]),
                    columns[6].Equals(String.Empty) ? 99 : Int32.Parse(columns[6]));
            }
            return partyParameterScores;
        }

        public static Dictionary<int,Dictionary<PoliticalParty, Dictionary<PoliticalPartyParameter, int>>>
            BuildPartyParametersDataFromFile(string filename)
        {
            var dict = new Dictionary<int, Dictionary<PoliticalParty, Dictionary<PoliticalPartyParameter, int>>>();
            string[] allLines = File.ReadAllLines(filename);

            for (int acNo = 0; acNo < 243; acNo++)
            {
                var linesOfAC = allLines.Skip(1).Where(t => Int32.Parse(t.Split('\t')[0]) == acNo).ToArray();
                dict.Add(acNo, BuildPartyParametersDataForAnAssembly(linesOfAC));
            }
            return dict;
        }

        #endregion PartyParameters

        #region DevelopmentIssues

        public static Dictionary<DevelopmentIssueCategory, int> BuildDevelopmentRatingsDataForAnAssembly(string[] linesOfAC)
        {
            var dict = new Dictionary<DevelopmentIssueCategory, int>();

            foreach (string[] columns in linesOfAC.Select(entry => entry.Split('\t')))
            {
                // Ignore the 2nd entry for Communal polarization
                if (dict.ContainsKey(Utils.GetDevelopmentParameter(columns[4]))) { continue; }

                if (columns[5].Equals(String.Empty))
                {
                    //Console.WriteLine("Empty DevelopmentData Value: {0}", String.Join("\t", columns));
                }

                dict.Add(Utils.GetDevelopmentParameter(columns[4]),
                    columns[5].Equals(String.Empty) ? 99 : Int32.Parse(columns[5]));
            }

            return dict;
        }

        public static Dictionary<int, Dictionary<DevelopmentIssueCategory, int>>
            BuildDevelopmentRatingsDataFromFile(string filename)
        {
            var dict = new Dictionary<int, Dictionary<DevelopmentIssueCategory, int>>();
            string[] allLines = File.ReadAllLines(filename);

            for (int acNo = 0; acNo < 243; acNo++)
            {
                var linesOfAC = allLines.Skip(1).Where(t => Int32.Parse(t.Split('\t')[0]) == acNo).ToArray();
                dict.Add(acNo, BuildDevelopmentRatingsDataForAnAssembly(linesOfAC));
            }
            return dict;
        }

        #endregion DevelopmentIssues

        #region LocalIssues

        public static Dictionary<LocalIssuesCategory, int> BuildLocalIssueRatingsDataForAnAssembly(string[] linesOfAC)
        {
            var dict = new Dictionary<LocalIssuesCategory, int>();

            foreach (string[] columns in linesOfAC.Select(entry => entry.Split('\t')))
            {
                // Ignore the 2nd entry for Communal polarization
                if (dict.ContainsKey(Utils.GetLocalIssuesCategory(columns[3]))) { continue; }

                if (columns[4].Equals(String.Empty))
                {
                    //Console.WriteLine("Empty DevelopmentData Value: {0}", String.Join("\t", columns));
                }

                dict.Add(Utils.GetLocalIssuesCategory(columns[3]),
                    columns[4].Equals(String.Empty) ? 99 : Int32.Parse(columns[5]));
            }

            return dict;
        }

        public static Dictionary<int, Dictionary<LocalIssuesCategory, int>>
            BuildLocalIssueRatingsDataFromFile(string filename)
        {
            var dict = new Dictionary<int, Dictionary<LocalIssuesCategory, int>>();
            string[] allLines = File.ReadAllLines(filename);

            for (int acNo = 0; acNo < 243; acNo++)
            {
                var linesOfAC = allLines.Skip(1).Where(t => Int32.Parse(t.Split('\t')[0]) == acNo).ToArray();
                dict.Add(acNo, BuildLocalIssueRatingsDataForAnAssembly(linesOfAC));
            }
            return dict;
        }

        #endregion LocalIssues

        #region CandidateData

        public static Dictionary<CandidateParameter, int> BuildSingleCandidate(string[] singleCandidateDataForAnAssembly)
        {
            var dict = new Dictionary<CandidateParameter, int>();

            foreach (string[] columns in singleCandidateDataForAnAssembly.Select(entry => entry.Split('\t')))
            {
                if (columns[8].Equals(String.Empty))
                {
                    // Console.WriteLine("Empty CandidateData Value: {0}", String.Join("\t", columns));
                }

                if (dict.ContainsKey(Utils.GetCandidateParameter(columns[7])))
                {
                    continue;
                }

                dict.Add(Utils.GetCandidateParameter(columns[7]),
                    columns[8].Equals(String.Empty) ? 0 : Int32.Parse(columns[8]));
            }
            return dict;
        }

        // We are assuming that candidate names in a constituency are unique..
        // This is not necessarily the case in general, but, it is true for qualitative data
        public static Dictionary<Candidate, Dictionary<CandidateParameter, int>> BuildCandidateRatingsForAnAssembly(
            string[] linesOfAC)
        {
            var dict = new Dictionary<Candidate, Dictionary<CandidateParameter, int>>();

            var wrongCandidateNames = new List<string> { "new", "new seat", "new face" };
            foreach (var candidateName in linesOfAC.Select(t => t.Split('\t')[4]).Distinct())
            {
                if (wrongCandidateNames.Contains(candidateName))
                {
                    continue;
                }
                var candidate = new Candidate() { Name = candidateName };
                var candidateRatings =
                    BuildSingleCandidate(linesOfAC.Where(t => t.Split('\t')[4] == candidateName).ToArray());
                dict.Add(candidate, candidateRatings);
            }
            return dict;
        }

        public static Dictionary<int, Dictionary<Candidate, Dictionary<CandidateParameter, int>>>
            BuildCandidateRatingsFromFile(string filename)
        {
            var dict = new Dictionary<int, Dictionary<Candidate, Dictionary<CandidateParameter, int>>>();
            string[] allLines = File.ReadAllLines(filename);

            for (int acNo = 0; acNo < 243; acNo++)
            {
                var linesOfAC = allLines.Skip(1).Where(t => Int32.Parse(t.Split('\t')[0]) == acNo).ToArray();
                dict.Add(acNo, BuildCandidateRatingsForAnAssembly(linesOfAC));
            }

            return dict;
        }

        #endregion CandidateData

        #region CasteData

        public static CasteData BuildCasteDataForAnAssembly(string[] casteShareDataForAnAssembly)
        {
            var casteData = new CasteData
            {
                CastePartySupport = new Dictionary<Caste, Dictionary<PoliticalParty, double>>(),
                CasteShare = new Dictionary<Caste, double>()
            };

            foreach (string[] columns in casteShareDataForAnAssembly.Select(entry => entry.Split('\t')))
            {
                if (Utils.GetCaste(columns[4]) == Caste.error) { continue; }

                #region Population %
                if (!casteData.CasteShare.ContainsKey(Utils.GetCaste(columns[4])))
                {
                    if (columns[5].Equals(String.Empty) && columns[8].Equals(String.Empty))
                    {
                        // Console.WriteLine("Empty CasteShare Value: {0}", String.Join("\t", columns));
                        casteData.CasteShare.Add(Utils.GetCaste(columns[4]), 0);
                    }
                    else
                    {
                        casteData.CasteShare.Add(Utils.GetCaste(columns[4]),
                            columns[5].Equals(String.Empty) ? double.Parse(columns[8]) : double.Parse(columns[5]));
                    }
                }
                #endregion Population %

                if (!casteData.CastePartySupport.ContainsKey(Utils.GetCaste(columns[4])))
                {
                    casteData.CastePartySupport.Add(Utils.GetCaste(columns[4]), new Dictionary<PoliticalParty, double>());
                }

                if (casteData.CastePartySupport[Utils.GetCaste(columns[4])].ContainsKey(Utils.GetParty(columns[7])))
                {
                    continue;
                }
                casteData.CastePartySupport[Utils.GetCaste(columns[4])].Add(Utils.GetParty(columns[7]),
                    columns[6].Equals(String.Empty) ? 0 : double.Parse(columns[6]));
            }
            
            return casteData;
            /*
             * TODO:
             * 1. Combine OTHER, OTHERS,OTHR into one
             * 2. Remove AC Name and Seat Details as Castes
             * 3. Combine the following 
             * {"Vaishya", "Vaishiya"}, {"Rest-OBC", "Rest OBC"}, {"Sahni", "Sahanis"}, {"Muslims", "Muslim"}, {"Kayasth","Kayastha"}, {"Dalit","Dalkit","Dali", "SC"},
             * {"Brahman", "Brahmin", "Brahmans", "Bhahman"}, {"Rajput", "Rajpoot"}
             * Q. Ask about how to combine hierarchies..Rajput, Rajput-Bumihar
             */
        }

        public static Dictionary<int,CasteData> BuildCasteShareDataFromFile(string filename)
        {
            var dict = new Dictionary<int, CasteData>();
            string[] allLines = File.ReadAllLines(filename);

            for (int acNo = 0; acNo < 243; acNo++)
            {
                var linesOfAC = allLines.Skip(1).Where(t => Int32.Parse(t.Split('\t')[0]) == acNo).ToArray();
                dict.Add(acNo, BuildCasteDataForAnAssembly(linesOfAC));
            }
            return dict;
        }

        #endregion CasteData

        #endregion Load Data from file
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

        #region Load Data from files

        public static QualitativeDataOfAYear BuildQualitativeDataOfAYear(int year, string localIssuesFilename,
            string developmentIssuesFilename, string partyParametersFilename, string casteDataFilename,
            string candidateDataFilename)
        {
            string[] casteShareData = File.ReadAllLines(casteDataFilename);

            var qualitativeDataOfAYear = new QualitativeDataOfAYear
            {
                Year = year,
                ConstituencyWiseQualitativeData =
                    new Dictionary<AssemblyConstituency, QualitativeDataOfAConstituencyInAYear>()
            };

            var partyParametersData =
                QualitativeDataOfAConstituencyInAYear.BuildPartyParametersDataFromFile(partyParametersFilename);
            var developmentRatingsData =
                QualitativeDataOfAConstituencyInAYear.BuildDevelopmentRatingsDataFromFile(developmentIssuesFilename);
            var localIssueRatingsData =
                QualitativeDataOfAConstituencyInAYear.BuildLocalIssueRatingsDataFromFile(localIssuesFilename);
            var candidateRatingsData =
                QualitativeDataOfAConstituencyInAYear.BuildCandidateRatingsFromFile(candidateDataFilename);

            for (int acNo = 1; acNo <= 243; acNo++)
            {
                var ac = new AssemblyConstituency { No = acNo };
                var qualitativeDataOfAConstituencyInAYear = new QualitativeDataOfAConstituencyInAYear();


                // Party Parameter Ratings
                qualitativeDataOfAConstituencyInAYear.PartyParameterRatings = partyParametersData[acNo];
                qualitativeDataOfAConstituencyInAYear.DevelopmentIssueRatings = developmentRatingsData[acNo];
                qualitativeDataOfAConstituencyInAYear.LocalIssueRatings = localIssueRatingsData[acNo];
                qualitativeDataOfAConstituencyInAYear.CandidateParameterRatings = candidateRatingsData[acNo];                
            }

            return qualitativeDataOfAYear;
        }

        #endregion
    }

    public class QualitativeData
    {
        public Dictionary<int, QualitativeDataOfAYear> YearWiseQualitativeData { get; set; }
    }
}
