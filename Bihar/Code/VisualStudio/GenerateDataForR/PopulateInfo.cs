using System.Threading.Tasks;
using BiharElectionsLibrary;

namespace GenerateDataForR
{
    class PopulateInfo
    {
        public static State LoadElectionHierarchy(string stateDivisionsFilename, 
            string acInfoFilename)
        {
            var stateHierarchy = State.LoadDivisionsAndDistrictsFromFile(stateDivisionsFilename);
            stateHierarchy.LoadPCsAndACs(acInfoFilename);
            return stateHierarchy;
        }

        public static void LoadCensusData(State state, string dataPath, string distListRelPath)
        {
            var populator = new PopulateCensusData(dataPath);
            populator.ParseAndPopulateDistrictListPage(distListRelPath, state);
        }
    }
}
