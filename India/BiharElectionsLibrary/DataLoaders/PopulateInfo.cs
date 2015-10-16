using BiharElectionsLibrary;

namespace BiharElectionsLibrary
{
    public class PopulateInfo
    {
        public static State LoadElectionHierarchy(string stateDivisionsFilename, 
            string acInfoFilename)
        {
            var state = new State {Name = "Bihar"};
            state = HierarchyLoader.LoadDivisionsAndDistrictsFromFile(stateDivisionsFilename, state);
            state = HierarchyLoader.LoadPCsAndACs(acInfoFilename, state);
            return state;
        }

        public static void LoadCensusData(State state, string dataPath, string distListRelPath)
        {
            var populator = new PopulateCensusData(dataPath);
            populator.ParseAndPopulateDistrictListPage(distListRelPath, state);
        }
    }
}
