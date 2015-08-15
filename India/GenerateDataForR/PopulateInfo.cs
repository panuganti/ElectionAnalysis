using System.Threading.Tasks;
using BiharElectionsLibrary;

namespace GenerateDataForR
{
    class PopulateInfo
    {
        public static State GenerateState(string stateDivisionsFilename, 
            string acInfoFilename, string censusDataDir, string districtsList)
        {
            var state = LoadElectionHierarchy(stateDivisionsFilename,
                acInfoFilename);
            LoadCensusData(state, censusDataDir, districtsList);
            return state;
        }

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
