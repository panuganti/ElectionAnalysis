using System;
using System.Collections.Generic;
using System.Linq;

namespace BiharElectionsLibrary
{
    /// <summary>
    /// Contains approximate string matching
    /// </summary>
    public static class Utils
    {
        public static string GetNormalizedName(string originalString)
        {
            var str = originalString.Trim(new[] {'\n', '\t'});
            return
                String.Join("_",
                    str.Split(new[] { '&', '/', '-', ' ', '.' }, StringSplitOptions.RemoveEmptyEntries))
                    .Trim('_')
                    .ToLower();
        }

        public static string GetCandidatePositionRating(int acNo, string candidateName,
            Dictionary<int, ACResult> results)
        {
            var constituencyResults = results[acNo];

            Candidate candidate = null;
            int count = 0;
            var resultsInOrder = constituencyResults.Votes.OrderByDescending(t => t.Votes);
            foreach (var result in resultsInOrder)
            {
                if (CheckNameSimilarity(result.Candidate.Name, candidateName))
                {
                    candidate = result.Candidate;
                    break;
                }
                count++;
            }

            if (candidate == null)
            {
                return "Bad";
            }
            return GetRating(count, resultsInOrder.Count());
        }

        private static string GetRating(int position, int totalResults)
        {
            switch (position)
            {
                case 0:
                    return "Perfect";
                case 1:
                    return "Excellent";
                case 2:
                    return "Good";
                default:
                    return "Fair";
            }
        }

        /* dinesh kumar singh = dinesh kr. singh yadav
         *  dr. ranvijay kumar = dr. ran vijay kumar
         *  satyadeo narain arya = s. n. arya
         *  jitendra kumar = dr. jitendra
         *  pannalal singh "patel" = panna lal singh
         *  baidhnath sahani = brijnath sehni
         *  narendra n. yadav = narendra narayan
         */
        public static bool CheckNameSimilarity(string electoralName, string surveyName)
        {
            electoralName = electoralName.Trim(new []{' ','.'}).ToLower();
            surveyName = surveyName.Trim(new[] { ' ', '.' }).ToLower();
            if (electoralName.Equals(surveyName))
            {
                return true;
            }
            string[] electoralNameParts = electoralName.Split(' ');
            string[] surveyNameParts = surveyName.Split(' ');

            electoralNameParts = ReplaceWithEquivalentStrings(electoralNameParts);
            surveyNameParts = ReplaceWithEquivalentStrings(surveyNameParts);
            
            // If there are two words that are within a distance of 2 each, 
            //then accept it to be similar
            int count = electoralNameParts.Count(
                    electoralNamePart => surveyNameParts
                        .Min(t => LevenshteinDistance(electoralNamePart, t)) <= 2);
            if (count >=2)
            {
                return true;
            }

            if (electoralNameParts.Length == 2 && surveyNameParts.Length == 2)
            {// Match if the first names are same even if last names are different
                if (electoralNameParts[0] == surveyNameParts[0])
                {
                    return true;
                }
            }

            if (CheckNamePartBreakUpIssue(electoralNameParts, surveyNameParts) || CheckNamePartBreakUpIssue(surveyNameParts, electoralNameParts))
            {
                return true;
            }

            return false;
        }

        private static bool CheckNamePartBreakUpIssue(string[] firstStrings, string[] secondStrings)
        {
            if (firstStrings.Length == 2 && secondStrings.Length == 3)
            {
                if ((LevenshteinDistance(firstStrings[0], secondStrings[0] + secondStrings[1]) <= 2) &&
                    LevenshteinDistance(firstStrings[1], secondStrings[2]) < 2)
                {
                    return true;
                }
            }
            return false;
        }

        private static string[] ReplaceWithEquivalentStrings(IEnumerable<string> inputStrings)
        {
            inputStrings = inputStrings.Select(
                t => LookUps.EquivalentStringsDictionary.ContainsKey(t) ? LookUps.EquivalentStringsDictionary[t] : t).ToArray();
            if (inputStrings.Count() == 4)
            {
                return inputStrings.Skip(1).ToArray();
            }
            return inputStrings.ToArray();
        }

        public static List<int> GetIndexOfClosestMatchInTheList(List<string> acNames, string acName)
        {
            var normalizedList = acNames.Select(GetNormalizedName).ToList();
            var normalizedAcName = GetNormalizedName(acName);
            var matches = normalizedList.FindAll(t => t.Equals(normalizedAcName)).ToList();
            if (matches.Count() == 1)
            {// Simple case match found
                var match = matches.First();
                return new List<int>(normalizedList.FindIndex(t=> t.Equals(match)));
            }
            Console.WriteLine("Match not found for {0}", acName);
            // Remove names in parenthesis on both sides and try to match

            // if there are two matches.. return both ?

            // Get matches by levenshtein distance < 2
            return new List<int>(-1);
        }
        
        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            var d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public static CasteCategory GetCasteCategory(string caste)
        {
            caste = caste.Trim(new[] { '(', ')', ' ', 's' }).ToLower();
            CasteCategory casteCategory;
            var parsed = Enum.TryParse(caste, true, out casteCategory);

            if (parsed)
            {
                return casteCategory;
            }

            string[] casteParts = caste.Split(new[] { '/', '&', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            if (casteParts.Length > 1)
            {
                foreach (var castePart in casteParts)
                {
                    return GetCasteCategory(castePart);
                }
            }

            switch (caste)
            {
                case "bc":
                case "vaishiya":
                case "vaishya":
                    return CasteCategory.obc;
                case "rest-obc":
                case "rest obc":
                    return CasteCategory.obc;
                case "sahni":
                case "sahanis":
                case "sahnis":
                case "sehni":
                case "yadav":
                case "kurmi":
                case "koiri":
                case "koyari":
                case "awadhiya":
                case "awadhia":
                    return CasteCategory.obc;
                case "muslims":
                    return CasteCategory.muslim;
                case "kayasth":
                case "kaishthya":
                case "kayastha":
                    return CasteCategory.uch;
                case "dalit":
                case "dalkit":
                case "dalikt":
                case "dali":
                case "harijan":
                case "harijans":
                case "paswan":
                case "mallaha":
                case "mallah":
                case "kushwaha":
                case "chamar":
                case "st":
                    return CasteCategory.sc;

                case "brahman":
                case "brahmans":
                case "bhahman":
                case "brahmin":
                case "thakur":
                case "chouhan":
                case "chauhan":
                    return CasteCategory.uch;
                case "rajpoot":
                case "rajput":
                case "bhumihar":
                case "bumihar":
                case "baniya":
                case "kahar":
                case "rajvanshi":
                case "chaudhary":
                case "choudhari":
                case "chaudhari":
                    return CasteCategory.uch;
                case "others":
                case "other":
                case "othr":
                case "othrs":
                case "bind":
                case "nunia":
                case "rajbhar":
                case "chanev":
                case "dhanuk":
                case "mandal":
                case "ebc":
                    return CasteCategory.obc;
                case "seat details":
                case "ac name":
                case "c":
                    return CasteCategory.error;
                default:

                    if (caste.Contains("rajput") || caste.Contains("rajpoot") || caste.Contains("bhumihar") ||
                        caste.Contains("bumihar"))
                    {
                        return CasteCategory.uch;
                    }
                    if (caste.Contains("kurmi"))
                    {
                        return CasteCategory.obc;
                    }

                    // todo: replace multiple spaces with single space and replace '-', '&' and '\s' with '_'
                    throw new Exception("Cannot determine caste category");
            }
        }


        #region Enum Converters

        public static Caste GetCaste(string casteInString)
        {
            casteInString = casteInString.Trim(new[] { '(', ')', ' ', 's' }).ToLower();
            Caste caste;
            var parsed = Enum.TryParse(casteInString, true, out caste);

            if (parsed)
            {
                return caste;
            }
            Console.WriteLine("Could not convert {0}", casteInString);

            switch (casteInString)
            {
                case "rest-obc":
                case "rest obc":
                case "bc":
                    return Caste.obc;
                case "vaishya":
                    return Caste.vaishya;
                case "sahanis":
                case "sahnis":
                case "sehni":
                    return Caste.sahni;
                case "koyari":
                    return Caste.koiri;
                case "kori":
                    return Caste.koli;
                case "balmiki":
                    return Caste.valmiki;
                case "awadhia":
                    return Caste.awadhiya;
                case "muslims":
                    return Caste.muslim;
                case "kayasth":
                case "kaishthya":
                    return Caste.kayastha;
                case "dalit":
                case "dalkit":
                case "dalikt":
                case "dali":
                case "harijan":
                case "harijans":
                    return Caste.sc;
                case "mallah":
                    return Caste.mallaha;
                case "brahman":
                case "brahmans":
                case "bhahman":
                    return Caste.brahmin;
                case "chauhan":
                    return Caste.chouhan;
                case "rajpoot":
                    return Caste.rajput;
                case "bhumihar":
                    return Caste.bhumihar;
                case "chaudhary":
                case "choudhari":
                case "chaudhari":
                case "choudhary":
                    return Caste.chaudhary;
                case "yadaw":
                case "yadawa":
                case "yadwa":
                    return Caste.yadav;
                case "manjhee":
                    return Caste.manjhi;
                case "kushawaha":
                    return Caste.kushwaha;
                case "other":
                case "othr":
                case "othrs":
                    return Caste.others;
                case "seat details":
                case "ac name":
                case "c":
                    return Caste.error;
                default:
                    if ((casteInString.Contains("rajput") || casteInString.Contains("rajpoot")) &&
                        casteInString.Contains("bhumihar") || casteInString.Contains("bumihar"))
                    {
                        return Caste.rajputBhumihar;
                    }
                    if ((casteInString.Contains("brahmin")) &&
                        casteInString.Contains("bhumihar") || casteInString.Contains("bumihar"))
                    {
                        return Caste.brahminBhumihar;
                    }
                    if (casteInString.Contains("kurmi"))
                    {
                        return Caste.kurmi;
                    }
                    // todo: replace multiple spaces with single space and replace '-', '&' and '\s' with '_'
                    Console.WriteLine("Cannot determine caste category");
                    return Caste.error;
            }
        }

        public static PoliticalPartyParameter GetPoliticalPartyParameter(string parameter)
        {
            parameter = parameter.Trim(new[] { '(', ')', ' ', 's' }).ToLower();
            PoliticalPartyParameter partyParameter;
            var parsed = Enum.TryParse(parameter, true, out partyParameter);
            if (parsed)
            {
                return partyParameter;
            }
            Console.WriteLine("Could not convert {0}", parameter);
            throw new Exception("Could not convert");
        }

        public static CandidateParameter GetCandidateParameter(string parameter)
        {
            parameter = parameter.Trim(new[] { '(', ')', ' ', 's' }).ToLower();
            CandidateParameter candidateParameter;
            var parsed = Enum.TryParse(parameter, true, out candidateParameter);
            if (parsed)
            {
                return candidateParameter;
            }
            Console.WriteLine("Could not convert {0}", parameter);
            throw new Exception("Could not convert");            
        }

        public static DevelopmentIssueCategory GetDevelopmentParameter(string parameter)
        {
            parameter = parameter.Trim(new[] { '(', ')', ' ', 's'}).ToLower();
            DevelopmentIssueCategory developmentParameter;
            var parsed = Enum.TryParse(parameter, true, out developmentParameter);
            if (parsed)
            {
                return developmentParameter;
            }
            Console.WriteLine("Could not convert DevIssue {0}", parameter);
            throw new Exception("Could not convert");
        }

        public static LocalIssuesCategory GetLocalIssuesCategory(string localIssue)
        {
            localIssue = localIssue.Trim().ToLower().Replace(' ','_');
            switch (localIssue)
            {
                case "security_issues_with_nepal_border":
                case "broken_embankment_with_effect_50000_people_last_year":
                case "problem_of_rehebalitation":
                case "bridge_over_bhutahi_balan_river":
                case "tharmal_power_station":
                case "sugar_mill":
                case "motipur_sugar_mill":
                case "sugar_mills":
                case "making_districts_of_maharajganj":
                case "barna_bridge":
                case "stone_chips_industry":
                case "railway_washing_pit":
                case "sub_division":
                case "bagaha_sugar_factory":
                case "noc_for_ongc_oil_search_(gandak_basin)":
                case "sugar/paper_factory":
                case "bettiah_govt_medical_college":
                case "oil_depot":
                case "chakiya_sugar_factory":
                case "sugar_factory":
                case "demand_to_declear_it_as_industrial_state":
                case "bridge_over_kamla_river":
                case "bridge_for_tedagach_block":
                case "mahananda_basen_project":
                case "cotton_base_industry":
                case "kachoda_bandh":
                case "bridge":
                case "sansamusa_(sugar_mills)":
                case "bridge_on_old_gandak_river_in_somnaha":
                case "noon_river_canal_project":
                case "cornflex_related_industry":
                case "ganga_katav":
                case "ntpc_issue":
                case "durgawati_jalasaya_project":
                case "demand_to_open_rohtas_industry":
                    return LocalIssuesCategory.UltraLocalIssue;
                case "backwardness":
                case "very_backward_area":
                case "development_in_last_5_years_0_10":
                case "development_in_last_5_years_______________0_-_10":
                case "basic_infrastructure":
                case "development":
                case "devlopment":
                case "development_in_last_5_years":
                case "tharu_backwardness":
                case "local_development":
                    return LocalIssuesCategory.Development;
                case "flood_by_adwara_group_river":
                case "flood":
                case "most_flooded_area_of_the_city":
                case "flood_area_/_dar_area":
                case "flood/_dahtar_area":
                case "flood_water":
                case "floods":
                    return LocalIssuesCategory.NaturalCalamity;
                case "road":
                case "trafic":
                case "road_&_transport":
                case "transport":
                case "road_transport":
                case "poor_road_condition":
                case "link_road":
                    return LocalIssuesCategory.Roads;
                case "corruption":
                case "corruption_in_govt_department":
                case "corruption_in_govt._department":
                case "corruption_in_rehabilitation__work":
                case "corruption_in_bpl_policy":
                case "curruption_in_govt._department":
                case "curruption_in_devlopment_works":
                case "corruption_in_bpl_banifit_polices":
                case "corruption_in_mla_funds":
                case "implementation_of_narega_project":
                case "corruption_in_govt.services":
                    return LocalIssuesCategory.Corruption;
                case "electricity_problem":
                case "electricity":
                case "drinking_water_missing":
                case "drinking_water":
                case "water":
                case "sanitation_problem":
                case "ignorence":
                case "poor_road_and_electricity_condition":
                case "garbage_problem":
                case "cleaness":
                case "drinking_water_(thori_panchayat)":
                case "job_card/bpl_card":
                case "appl/bpl/job_card":
                case "apl/bpl_card":
                case "electric_sub_station":
                case "railway_track":
                case "education":
                case "demand_govt._college":
                case "health_services":
                case "upgradation_of_railway_track":
                    return LocalIssuesCategory.Services;
                case "development_in_last_5_years_by_local_mla":
                case "mla_performance":
                case "image_of_the_candidate":
                case "candidate":
                case "candidate_performance":
                    return LocalIssuesCategory.MLAPerformance;
                case "performance_of_state_government":
                    return LocalIssuesCategory.StateGovtPerformance;
                case "naxilite":
                case "naxalite":
                case "naxolite":
                case "naxals_problem":
                case "naxals_rpoblems":
                case "naxalism":
                case "naxlisam":
                case "naxal_area":
                case "naxal_belt":
                    return LocalIssuesCategory.Naxalism;
                case "employment_growth":
                case "unemployment":
                case "employment":
                case "industry":
                case "industries":
                case "bunkar_problem":
                    return LocalIssuesCategory.Unemployment;
                case "irrigation":
                case "farmers_problems":
                case "irregation":
                case "agricultural_market":
                case "reparing_of_bandh_for_flood_protection":
                case "rice_base_industry":
                case "demand_to_establish_aggricultural_basec_industries":
                case "problem_of_irrigation":
                case "irrgation":
                case "farmers_rpoblems":
                    return LocalIssuesCategory.FarmerIssues;
                case "communal_polarization":
                case "communal_polaristion":
                case "minority_hostel":
                case "communal_issue":
                case "communal_polarisation":
                case "communalism":
                    return LocalIssuesCategory.CommunalPolarization;
                case "cast_polarisation":
                case "castism":
                case "caste_polarization":
                case "caste_polaristion":
                case "caste_polarisation":
                case "dalit_caste":
                case "caste_polarisatiom":
                case "caste":
                case "caste_polrisation":
                case "cast_base_fight":
                case "caste_polaristion(muslim-yadav_factor)":
                case "party_equations":
                case "caste_base_fight":
                    return LocalIssuesCategory.CastePolarization;
                case "low_and_order":
                case "crime":
                case "violence":
                case "law_order_abductions":
                case "law_&_order_/_abductions":
                    return LocalIssuesCategory.LawAndOrder;
                case "price_rise":
                case "price_hick":
                case "prick_hick":
                    return LocalIssuesCategory.Inflation;
                case "poverty":
                    return LocalIssuesCategory.Poverty;
                    default: 
                    Console.WriteLine("Error: {0}", localIssue);
                    return LocalIssuesCategory.Error;
            }
        }

        public static Gender GetGender(string genderString)
        {
            genderString = genderString.Trim(new[] { '(', ')', ' ' }).ToLower();
            Gender gender;
            var parsed = Enum.TryParse(genderString, true, out gender);
            if (parsed)
            {
                return gender;
            }
            Console.WriteLine("Could not convert Gender {0}", genderString);
            if (genderString.Equals(String.Empty))
            {
                return Gender.M; // Default is assumed to be male
            }
            throw new Exception("Could not convert");
        }

        public static PoliticalParty GetParty(string partyName)
        {
            var partyNameLower = partyName.Replace("(", "").Replace(")", "").ToLower();
            PoliticalParty politicalParty;
            var parsed = Enum.TryParse(partyNameLower, true, out politicalParty);
            if (parsed) return politicalParty;

            switch (partyName)
            {
                case "Bharatiya Janta Party":
                    return PoliticalParty.bjp;
                case "Janata Dal (United)":
                    return PoliticalParty.jdu;
                case "Rashtriya Janata Dal":
                    return PoliticalParty.rjd;
                case "Communist Party Of India (MARXIST)":
                case "Communist Party Of India (MARXIST-LENINIST) (LIBERATION)":
                case "All India Forward Bloc":
                    return PoliticalParty.cpi;
                case "Bahujan Samaj Party":
                    return PoliticalParty.bsp;
                case "Independent":
                    return PoliticalParty.ind;
                case "Aam Aadmi Party":
                    return PoliticalParty.aap;
                case "Samajwadi Party":
                    return PoliticalParty.sp;
                case "Lok Dal":
                    return PoliticalParty.ld;
            }

            if (partyNameLower == "jd(u)" || partyNameLower == "jd u")
            {
                return PoliticalParty.jdu;
            }
            if (partyNameLower.StartsWith("cp"))
            {
                return PoliticalParty.cpi;
            }
            if (partyNameLower.Contains("new") || partyNameLower.Contains("now"))
            {
                //Console.WriteLine("Error for {0}", partyName);
                return PoliticalParty.Error;
            }
            return PoliticalParty.others;
        }
        
        public static ConstituencyCasteCategory GetCategory(string casteCategory)
        {
            casteCategory = casteCategory.Trim(new[] { '(', ')' }).ToLower();
            ConstituencyCasteCategory constituencyCasteCategory;
            var parsed = Enum.TryParse(casteCategory, true, out constituencyCasteCategory);

            if (!parsed)
            {
                if (casteCategory.Contains("sc"))
                {
                    return ConstituencyCasteCategory.SC;
                }
                if (casteCategory.Contains("st"))
                {
                    return ConstituencyCasteCategory.ST;
                }
            }
            return ConstituencyCasteCategory.GEN;
        }

        #endregion Enum Converters
    }
}

