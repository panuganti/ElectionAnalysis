using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BiharElectionsLibrary
{
    public class MuslimPopulationData
    {
        public Dictionary<string, double> MuslimPopulation { get; set; }

        public static MuslimPopulationData LoadFromFile(string filename)
        {
            var allLines = File.ReadAllLines(filename).Skip(1).ToArray();
            var dict = new Dictionary<string, double>();
            foreach (var line in allLines)
            {
                var cols = line.Split('\t');
                var acName = cols[0].ToLower();
                dict.Add(acName,double.Parse(cols[1]));
            }
            return new MuslimPopulationData {MuslimPopulation = dict};
        }

        public Dictionary<int, double> GetMuslimPopulationByAcId(State state)
        {
            var dict = new Dictionary<int, double>();
            // First populate those where we find matches..
            foreach (var keyValuePair in MuslimPopulation)
            {
                var name = keyValuePair.Key;
                var acs = state.ACs.Where(x => x.Name.Equals(name)).ToArray();
                if (acs.Count() == 1)
                {
                    dict.Add(acs.First().No, keyValuePair.Value);
                    continue;
                }
                var closeAcs = state.ACs.Where(x => Utils.LevenshteinDistance(x.Name, name) < 2).ToArray();
                if (closeAcs.Length == 1)
                {
                    dict.Add(closeAcs.First().No, keyValuePair.Value);
                }
            }
            return dict;
        }
    }
}
