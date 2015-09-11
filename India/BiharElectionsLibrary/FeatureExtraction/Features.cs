using System.Collections.Generic;
using System.Linq;

namespace BiharElectionsLibrary
{
    public class Features
    {
        private readonly List<ACResult> _results2009;
        private readonly List<ACResult> _results2010;
        private readonly List<ACResult> _results2014;
        private Dictionary<string, string> FeatureVector { get; set; }

        public Features(List<ACResult> results2009, List<ACResult> results2010, List<ACResult> results2014)
        {
            _results2009 = results2009;
            _results2010 = results2010;
            _results2014 = results2014;
            FeatureVector = new Dictionary<string, string>();
        }

        public List<Stability> StabilityFeatures()
        {
            var acs = _results2014.Select(t => t.Constituency);
            var stabilityData = new List<Stability>();
            
            foreach (var ac in acs)
            {
                var stability = new Stability {AcId = ac.No};
                var winnerParty2009 = _results2009.First(t => t.Constituency.No == ac.No).GetWinningParty();
                var winnerParty2010 = _results2010.First(t => t.Constituency.No == ac.No).GetWinningParty();
                var winnerParty2014 = _results2014.First(t => t.Constituency.No == ac.No).GetWinningParty();

                if ((winnerParty2009 == winnerParty2010) && (winnerParty2009 == winnerParty2014))
                {
                    stability.Party = winnerParty2014.ToString();
                    stability.IsStable = true;
                }
                else
                {
                    stability.Party = winnerParty2014.ToString();
                    stability.IsStable = false;
                }
                stabilityData.Add(stability);
            }
            return stabilityData;
        }
    }

    public class Stability
    {
        public int AcId { get; set; }
        public string Party { get; set; }
        public bool IsStable { get; set; }
    }
}
