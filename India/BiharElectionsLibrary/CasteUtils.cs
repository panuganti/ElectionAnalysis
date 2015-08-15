using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiharElectionsLibrary
{
    public static class CasteUtils
    {
        public static string GetLastName(string name)
        {
            var normalizedName = name;
            if (!normalizedName.Contains('_'))
            {
                normalizedName = Utils.GetNormalizedName(normalizedName);
            }
            return normalizedName.Split('_').Last();
        }

        public static string GetFirstName(string name)
        {
            var titlesList = new List<string> {"dr", "mr", "mrs"};
            var normalizedName = name;
            if (!normalizedName.Contains('_'))
            {
                normalizedName = Utils.GetNormalizedName(normalizedName);
            }
            var nameParts = normalizedName.Split('_');
            if (nameParts.Count() == 2)
            {
                return nameParts[0];
            }
            if (nameParts.Count() == 3)
            {
                if (!titlesList.Contains(nameParts[0]))
                {
                    return nameParts[0];                    
                }
                return nameParts[1];
            }
            throw new Exception(normalizedName);
        }

        public static Dictionary<string, Caste> NameToCastes = new Dictionary<string, Caste>()
        {
            {"singh", Caste.rajput},
            {"modi", Caste.obc},
            {"khan", Caste.muslim},
            {"hussain", Caste.muslim},
            {"husain", Caste.muslim},
            {"shahidi", Caste.muslim},
            {"begum", Caste.muslim},
            {"ali", Caste.muslim},
            {"sheikh", Caste.muslim},
            {"azam", Caste.muslim},
            {"abbasi", Caste.muslim},
            {"ahmed", Caste.muslim},
            {"ahmad", Caste.muslim},
            {"khatoon", Caste.muslim},
            {"rahman", Caste.muslim},
            {"alam", Caste.muslim},
            {"azad", Caste.muslim},
            {"mohammed", Caste.muslim},
            {"mahamad", Caste.muslim},
            {"sharif", Caste.muslim},
            {"hasan", Caste.muslim},
            {"abdullah", Caste.muslim},
            {"miya", Caste.muslim},
            {"kamran", Caste.muslim},
            {"perwaiz", Caste.muslim},
            {"ahamad", Caste.muslim},
            {"akhtar", Caste.muslim},
            {"anwar", Caste.muslim},
            {"shabbir", Caste.muslim},
            {"yasin", Caste.muslim},
            {"nasim", Caste.muslim},
            {"nazim", Caste.muslim},
            {"zia", Caste.muslim},
            {"iman", Caste.muslim},
            {"rizvi", Caste.muslim},
            {"yussouf", Caste.muslim},
            {"yusuf", Caste.muslim},
            {"zafar", Caste.muslim},
            {"mastan", Caste.muslim},
            {"aslam", Caste.muslim},
            {"siddique", Caste.muslim},
            {"shahid", Caste.muslim},
            {"anjum", Caste.muslim},
            {"haque", Caste.muslim},
            {"aftab", Caste.muslim},
            {"husen", Caste.muslim},
            {"rasul", Caste.muslim},
            {"ishtiaque", Caste.muslim},
            {"siddiqee", Caste.muslim},
            {"habib", Caste.muslim},
            {"ausaf", Caste.muslim},
            {"faiaj", Caste.muslim},
            {"jamaal", Caste.muslim},
            {"lukman", Caste.muslim},
            {"jawaid", Caste.muslim},
            {"hasnain", Caste.muslim},
            {"iliyas", Caste.muslim},
            {"ezaz", Caste.muslim},
            {"shakoor", Caste.muslim},
            {"maksud", Caste.muslim},
            {"gafoor", Caste.muslim},
            {"shamim", Caste.muslim},
            {"imam", Caste.muslim},
            {"siddiqui", Caste.muslim},
            {"islam", Caste.muslim},
            {"hassan", Caste.muslim},
            {"abbas", Caste.muslim},
            {"rizwan", Caste.muslim},
            {"farman", Caste.muslim},
            {"mustafa", Caste.muslim},
            {"fatmi", Caste.muslim},
            {"mohasin", Caste.muslim},
            {"irfanurrahman", Caste.muslim},
            {"raza", Caste.muslim},
            {"haider", Caste.muslim},
            {"ishrat", Caste.muslim},
            {"samshed", Caste.muslim},
            {"hashmi", Caste.muslim},
            {"kalamudin", Caste.muslim},
            {"basir", Caste.muslim},
            {"jamal", Caste.muslim},
            {"samsher", Caste.muslim},
            {"israil", Caste.muslim},
            {"naushad", Caste.muslim},
            {"nasir", Caste.muslim},
            {"firoz", Caste.muslim},
            {"ghafoor", Caste.muslim},
            {"ajam", Caste.muslim},
            {"hasmi", Caste.muslim},
            {"akhatar", Caste.muslim},
            {"shahnavaj", Caste.muslim},
            {"hak", Caste.muslim},
            {"imran", Caste.muslim},
            {"nisa", Caste.muslim},
            {"baig", Caste.muslim},
            {"asgher", Caste.muslim},
            {"mobin", Caste.muslim},
            {"miyan", Caste.muslim},
            {"isteyaque", Caste.muslim},
            {"umar", Caste.muslim},
            {"emam", Caste.muslim},
            {"sahin", Caste.muslim},
            {"muslam", Caste.muslim},
            {"ashraf", Caste.muslim},
            {"usman", Caste.muslim},
            {"rasid", Caste.muslim},
            {"quasmi", Caste.muslim},
            {"iqbal", Caste.muslim},
            {"mumtaz", Caste.muslim},
            {"nahid", Caste.muslim},
            {"parwez", Caste.muslim},
            {"syed", Caste.muslim},
            {"pravej", Caste.muslim},
            {"ibrahim", Caste.muslim},
            {"rahaman", Caste.muslim},
            {"hussin", Caste.muslim},
            {"sadique", Caste.muslim},
            {"gulam", Caste.muslim},
            {"quadri", Caste.muslim},
            {"faisal", Caste.muslim},
            {"ajim", Caste.muslim},
            {"mohammd", Caste.muslim},
            {"amzad", Caste.muslim},
            {"kauser", Caste.muslim},
            {"irfan", Caste.muslim},
        };

        public static Caste GetCasteForMinorSpellingErrors(string lastname)
        {
            var match = NameToCastes.FirstOrDefault(x => x.Key.Length > 4 && Utils.LevenshteinDistance(x.Key, lastname) < 2 && x.Value != Caste.muslim);
            if (!match.Equals(default(KeyValuePair<string, Caste>)))
            {
                return match.Value;
            }
            return Caste.error;            
        }

        public static Caste GetCasteUsingPhonetics(string lastname)
        {
            if (lastname.Length < 6) // Algo is better with longer names
            {
                return Caste.error;
            }

            var phonetic = StringHasher.Hash(lastname, StringHasher.HashType.Metaphone);
            if (phonetic.Equals(String.Empty))
            {
                return Caste.error;
            }

            var phoneticKeys = NameToCastes.Keys.Where(x => x.Length > 5).Select(x => StringHasher.Hash(x, StringHasher.HashType.Metaphone)).Distinct();
            if (!phoneticKeys.Contains(phonetic))
            {
                return Caste.error;
            }

            var match = NameToCastes.FirstOrDefault(x => x.Key.Length > 5 && StringHasher.Hash(x.Key, StringHasher.HashType.Metaphone).Equals(phonetic));
            if (!match.Equals(default(KeyValuePair<string, Caste>)))
            {
                return match.Value;
            }
            return Caste.error;
        }

        public static Caste GetCasteFromName(string name)
        {
            var genericCastes = new List<Caste>() {Caste.others, Caste.obc, Caste.error, Caste.ebc, Caste.uch};
            
            var normalizedName = Utils.GetNormalizedName(name);
            if (normalizedName.Contains("yadav"))
            {
                return Caste.yadav;
            }
            var lastName = GetLastName(normalizedName);
            var caste = Utils.GetCaste(lastName);
            if (!genericCastes.Contains(caste))
            {
                return caste;
            }

            if (NameToCastes.ContainsKey(lastName))
            {
                return NameToCastes[lastName];
            }

            /* Too many errors.. need more stricter versions
            var casteByPhonetic = GetCasteUsingPhonetics(lastName);
            if (casteByPhonetic != Caste.error)
            {
                return casteByPhonetic;
            }
             */

            var casteByPhonetic = GetCasteForMinorSpellingErrors(lastName);
            if (casteByPhonetic != Caste.error)
            {
                return casteByPhonetic;
            }

            // Note: muslims can have hindu surnames.. It's better to identify by firstname..and if it fails, then try by lastname.
            switch (lastName)
            {
                case "agarwal":
                case "gupta":
                    return Caste.baniya;
                case "shrivastava":
                case "srivastava":
                case "shrivastwa":
                case "srivastva":
                case "srivastwa":
                case "sahay":
                case "sarkar":
                case "bishwas":
                case "biswas":
                    return Caste.kayastha;
                case "tiwari":
                case "mishra":
                case "mishr":
                case "prasad":
                case "sharma":
                case "shukla":
                case "shukal":
                case "rao":
                case "pandey":
                case "panday":
                case "pandit":
                case "pathak":
                case "dubey":
                case "chaubey":
                case "jha":
                case "giri":
                case "dwivedi":
                case "choubey":
                case "ojha":
                case "kishor":
                case "tiwary":
                case "pande":
                case "trivedi":
                    return Caste.brahmin; // todo: check
            }
            if (lastName.EndsWith("uddin") || lastName.EndsWith("udeen") || lastName.EndsWith("ullah") || lastName.EndsWith("ulah") || normalizedName.StartsWith("z"))
            {
                return Caste.muslim;
            }
            return Caste.others;
        }
    }
}
