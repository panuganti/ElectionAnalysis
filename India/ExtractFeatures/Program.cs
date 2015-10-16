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
        }

        private static void ExtractFeatures()
        {
            string rootDir = File.ReadAllLines(@".\Config.ini").First();
            string stateDivisionsFilename = Path.Combine(rootDir, ConfigurationManager.AppSettings["StateDivisionsFilename"]);
            string acInfoFilename = Path.Combine(rootDir, ConfigurationManager.AppSettings["ACs"]);

            string indiaVotesResults2014Dir = Path.Combine(rootDir, ConfigurationManager.AppSettings["2014ACWiseResultsIndiaVotes"]);
            string indiaVotesResults2010Dir = Path.Combine(rootDir, ConfigurationManager.AppSettings["2010ACWiseResultsIndiaVotes"]);
            string indiaVotesResults2009Dir = Path.Combine(rootDir, ConfigurationManager.AppSettings["2009ACWiseResultsIndiaVotes"]);

            State state = PopulateInfo.LoadElectionHierarchy(stateDivisionsFilename, acInfoFilename);

            var indiaVotesResults2014 = ResultsLoader.LoadResultsFromIndiaVotesData(indiaVotesResults2014Dir, 2014);
            var indiaVotesResults2009 = ResultsLoader.LoadResultsFromIndiaVotesData(indiaVotesResults2009Dir, 2009);
            var indiaVotesResults2010 = ResultsLoader.LoadACResultsFromIndiaVotesData(indiaVotesResults2010Dir, 2010);
            List<Result> conflatedResults2009 = ResultsConflator.ConflateResults(indiaVotesResults2009, state);
            List<Result> conflatedResults2010 = ResultsConflator.ConflateResultsAndDistrictInfo(indiaVotesResults2010, state);
            List<Result> conflatedResults2014 = ResultsConflator.ConflateResults(indiaVotesResults2014, state);            

        }
    }
}
