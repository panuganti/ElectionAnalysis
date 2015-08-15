using System.Collections.Generic;

namespace BiharElectionsLibrary
{
    public static class EnumExtensions
    {
        public static Dictionary<PoliticalParty, string> PartyColors = new Dictionary<PoliticalParty, string>()
        {
            {PoliticalParty.bjp, "darkorange"},
            {PoliticalParty.inc, "lightblue"},
            {PoliticalParty.rjd, "green"},
            {PoliticalParty.jdu, "lightgreen"},
            {PoliticalParty.others, "black"},
            {PoliticalParty.rslp, "yellow"},
            {PoliticalParty.ljp, "yellow"},
            {PoliticalParty.nda, "darkorange"},
            {PoliticalParty.upa, "lightblue"}
        };

        public static string ToColor(this PoliticalParty party)
        {
            if (PartyColors.ContainsKey(party))
            {
                return PartyColors[party];
            }
            return PartyColors[PoliticalParty.others];
        }
    }
}
