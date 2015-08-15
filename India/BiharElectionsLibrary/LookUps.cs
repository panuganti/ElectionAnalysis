using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiharElectionsLibrary
{
    public static class LookUps
    {
        public static readonly Dictionary<string, string> EquivalentStringsDictionary = new Dictionary<string, string>
        {
            {"pd", "prasad"},
            {"kr", "kumar"},
            {"md", "mohammad"},
            {"deo","dev"},
            {"zfar", "zafar"},
            {"afak", "afaque"},
            {"idradeo", "inderdev"},
            {"virsin","brishin"}
        };


        public static Dictionary<Caste, CasteCategory> CasteToCasteCategoryMapping = new Dictionary
            <Caste, CasteCategory>
        {
            {Caste.brahmin, CasteCategory.uch},         // Verified
            {Caste.vaishya, CasteCategory.uch},         // Verified
            {Caste.bhumihar, CasteCategory.uch},        // Verified
            {Caste.brahminBhumihar, CasteCategory.uch},
            {Caste.rajput, CasteCategory.uch},          // Verified
            {Caste.rajputBhumihar, CasteCategory.uch},
            {Caste.thakur, CasteCategory.uch},          // Verified
            {Caste.tyagi, CasteCategory.uch},           // Verified
            {Caste.bhantBrahmin, CasteCategory.uch},    // Verified
            {Caste.rajvanshi, CasteCategory.uch},
            {Caste.rajbhar, CasteCategory.uch},
            {Caste.kayastha, CasteCategory.uch},        // Verified
            {Caste.khatri, CasteCategory.uch},
            {Caste.arora, CasteCategory.uch},
            {Caste.jain, CasteCategory.uch}, // todo: WHY ? 

            {Caste.baniya, CasteCategory.obc},          // Verified
            {Caste.kanu, CasteCategory.obc},            // Verified
            {Caste.halwai, CasteCategory.obc},          // Verified
            {Caste.yadav, CasteCategory.obc},           // Verified
            {Caste.ahir, CasteCategory.obc},
            {Caste.ahir, CasteCategory.obc},            // Verified
            {Caste.chaudhary, CasteCategory.obc},       // Verified
            {Caste.teli, CasteCategory.obc},            // Verified
            {Caste.dhobhi, CasteCategory.obc},          // Verified
            {Caste.naayi, CasteCategory.obc},           // Verified
            {Caste.maali, CasteCategory.obc},           // Verified
            {Caste.kurmi, CasteCategory.obc},       // Verified
            {Caste.bind, CasteCategory.obc},        // Verified
            {Caste.dhanuk, CasteCategory.obc},       // Verified
            {Caste.koiri, CasteCategory.obc},       // Verified
            {Caste.panwari, CasteCategory.obc},       // Verified
            {Caste.lohar, CasteCategory.obc},       // Verified
            {Caste.badayi, CasteCategory.obc},       // Verified
            {Caste.mallaha, CasteCategory.obc},       // Verified
            {Caste.nishad, CasteCategory.obc},       // Verified
            {Caste.kumhar, CasteCategory.obc},       // Verified
            {Caste.jat, CasteCategory.obc},         // Verified
            {Caste.sunnar, CasteCategory.obc},      // Verified
            {Caste.noniya, CasteCategory.obc},      // Verified
            {Caste.kushwaha, CasteCategory.obc},    // Verified
            {Caste.mandal, CasteCategory.obc},         // Verified
            {Caste.saini, CasteCategory.obc},
            {Caste.vishvakarma, CasteCategory.obc},


            {Caste.awadhiya, CasteCategory.obc},
            {Caste.chouhan, CasteCategory.obc},
            {Caste.kahar, CasteCategory.ebc},
            {Caste.sahni, CasteCategory.obc},


            {Caste.chamar, CasteCategory.sc},
            {Caste.valmiki, CasteCategory.sc},
            {Caste.nunia, CasteCategory.sc},
            {Caste.koli, CasteCategory.sc},
            {Caste.dusad, CasteCategory.sc},
            {Caste.paswan, CasteCategory.sc},
            {Caste.musahar, CasteCategory.sc},
            {Caste.pasi, CasteCategory.sc},
            {Caste.dom, CasteCategory.sc},
            {Caste.halkor, CasteCategory.sc},
            {Caste.ramvasi, CasteCategory.sc},
            {Caste.chanev, CasteCategory.sc},
            {Caste.jatav, CasteCategory.sc},
            {Caste.majhabi, CasteCategory.sc},
            
            {Caste.manjhi, CasteCategory.sc},
            {Caste.sc, CasteCategory.sc},

            {Caste.kol, CasteCategory.st},
            {Caste.bheel, CasteCategory.st},
            {Caste.gond, CasteCategory.st},
            {Caste.muriya, CasteCategory.st},
            {Caste.nut, CasteCategory.st},
            {Caste.sahariya, CasteCategory.st},

            {Caste.shia, CasteCategory.muslim},
            {Caste.sunni, CasteCategory.muslim},
            {Caste.muslim, CasteCategory.muslim},
            {Caste.pathan, CasteCategory.muslim},
            {Caste.shaikh, CasteCategory.muslim},
            {Caste.sayed, CasteCategory.muslim},
            {Caste.siddiqui, CasteCategory.muslim},
            {Caste.ansari, CasteCategory.muslim},
            {Caste.mansoori, CasteCategory.muslim},
            {Caste.qureshi, CasteCategory.muslim},
            {Caste.siya, CasteCategory.muslim},

            {Caste.christian, CasteCategory.christian},
            {Caste.buddhist, CasteCategory.budhist},
            {Caste.punjabi, CasteCategory.punjabi},

            {Caste.others, CasteCategory.obc}, // TODO: CHECK
        };
    }
}
