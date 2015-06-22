namespace BiharElectionsLibrary
{
    public enum YesNo
    {
        Yes = 1,
        No = 2,
        Others = 0
    }
    public enum Gender
    {
        M,
        F,
        O,
        Error = 0
    }

    public enum ConstituencyCasteCategory
    {
        // ReSharper disable InconsistentNaming
        GEN,
        SC,
        ST,
        Error = 0
        // ReSharper restore InconsistentNaming
    }

    public enum PoliticalParty
    {
        // ReSharper disable InconsistentNaming
        bjp,
        rslp,
        jdu,
        rjd,
        ljp,
        inc,
        ind,
        cpi,
        sp,
        bsp,
        ncp,
        aap,
        jmm,
        
        // Alliances
        nda,
        upa,

        // Misc
        others,
        cantSay = 99,
        didNotVote,
        missingVote,
        nota = 0,
        Error
        // ReSharper restore InconsistentNaming
    }

    public enum PoliticalPartyParameter
    {
        PartyOrganizationStrength,
        LocalLeadersUnityAndCoordination,
        BoothManagement,
        Error = 0
    }

    public enum DataIssues
    {
        CasteCategoryDataMissing,
        LocalIssuesDataMissing,
        Error = 0
    }

    public enum FamilyIncome
    {
        LT3K = 1,
        GT3KLT6k = 2,
        GT6KLT10K = 3,
        GT10KLT20K = 4,
        GT20KLT50K = 5,
        GT50KLT1Lac = 6,
        GT1Lac = 7,
        CantSay = 0
    }

    public enum Occupation
    {
        StudentUnemployed = 1,
        HouseWife = 2,
        Farmer = 3,
        FarmLaborer = 4,
        FishingDiary = 5,
        GovtEmployee = 6,
        PrivateEmployee = 7,
        SelfEmployed = 8,
        CommonLaborer = 9,
        Others = 0
    }

    public enum EducationLevel
    {
        Illiterate = 0,
        Literate = 1,
        Primary = 2,
        HighSchool = 3,
        HigherSecondary = 4,
        Graduate = 5,
        PostGraduate = 6,
        Professional = 7,
    }

    public enum CandidateParameter
    {
        Availability,
        Honesty,
        EffectivenessInWorking,
        Popularity,
        ReligiousInfluence,
        MusclePower,
        FinancialStatus,
        UnderstandingWithPartyLeaders,
        RespectInLocalLeaders,
        Winnability,
        Overall,
        Error = 0
    }

    public enum DevelopmentIssueCategory
    {
        Electricity,
        Water,
        Road,
        Health,
        Education,
        UnEmployment,
        PublicTransport,
        LawAndOrder,
        CastePolarization, // Why is this in development ? 
        CommunalPolarization,
        Error = 0
    }

    public enum CasteCategory
    {
        // ReSharper disable InconsistentNaming
        uch,
        obc,
        ebc,
        sc,
        st,
        muslim,
        christian,
        budhist,
        jain,
        punjabi,
        error = 0
        // ReSharper restore InconsistentNaming
    }

    public enum Caste
    {
        // ReSharper disable InconsistentNaming
        awadhiya,
        ahir,
        arora,
        brahmin,
        bhumihar,
        brahminBhumihar,
        baniya,
        bheel,
        bind,
        badayi,
        bhantBrahmin,
        chanev,
        chouhan,
        chamar,
        chaudhary,
        dusad,
        dom,
        dhanuk,
        dhobhi,
        gond,
        halkor,
        halwai,
        jatav,
        jat,
        kahar,
        kayastha,
        kurmi,
        koiri,
        kol,
        koli,
        kushwaha,
        kanu,
        kumhar,
        khatri,
        lohar,
        maali,
        mallaha,
        mandal,
        manjhi,
        musahar,
        muriya,
        majhabi,
        nunia,
        noniya,
        nut,
        naayi,
        nishad,
        panwari,
        pasi,
        paswan,
        rajput,
        rajputBhumihar,
        rajvanshi,
        rajbhar,
        ramvasi,
        sahni,
        sahariya,
        sunnar,
        saini,
        teli,
        thakur,
        tyagi,
        vaishya,
        valmiki,
        vishvakarma,
        yadav,

        // Muslims
        pathan,
        shia,
        sunni,
        shaikh,
        sayed,
        siddiqui,
        ansari,
        mansoori,
        qureshi,
        siya,        

        // CasteCategories
        uch,
        obc,
        ebc,
        sc,
        st,
        punjabi,

        muslim,
        buddhist,
        jain,
        sikh,
        christian,

        // Default
        others,
        error = 0
        // ReSharper restore InconsistentNaming
    }

    #region QuantitativeEnums

    public enum DecisiveFactor
    {
        LocalCandidate = 1,
        Party = 2,
        CasteCommunity = 3,
        CMCandidate = 4,
        StateMinisters = 5,
        CentralMinisters = 6,
        CantSay = 0
    }

    public enum Issues
    {
        Development,
        Stability,
        Inflation,
        Party,
        SittingMLAPerformance,
        FavoriteMLA,
        PoliticalIssue,
        LawAndOrder,
        CMCandidate,
        PMCandidate,
        LocalCandidate,
        Corruption,
        Reservation,
        CastePolarization,
        CommunalPolarization,
        NaturalCalamity,
        MLAPerformance,
        StateGovtPerformance,
        CentralGovtPerformance,
        Roads,
        Services,
        Unemployment,
        Naxalism,
        Poverty,
        UltraLocalIssue,
        FarmerIssues,
        Kidnapping,
        Justice,
        Others,
        NoIssue = 0
    }
    
    public enum LocationType
    {
        Urban = 1,
        SemiUrban = 2,
        Rural = 3
    }

    #endregion QuantitativeEnums


    public enum LocalIssuesCategory
    {
        Corruption,
        CastePolarization,
        CommunalPolarization,
        Development,
        NaturalCalamity,
        MLAPerformance,
        StateGovtPerformance,
        LawAndOrder,
        Roads,
        Services,
        Education,
        Health,
        Unemployment,
        Naxalism,
        Inflation,
        Poverty,
        UltraLocalIssue,
        FarmerIssues,
        Kidnapping,
        Error = 0
    }

    public enum Condition
    {
        Improved = 1,
        RemainedSame = 2,
        Deteriorated = 3,
        CantSay = 0
    }

    public enum AdminHierarchy
    {
        PM,
        CentralGovt,
        CM,
        StateGovt,
        MP,
        MLA,
        LocalGovt,
    }
}
