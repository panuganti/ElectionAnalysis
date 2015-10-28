AllCand <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2015/AllCandidatesSwati.txt")
alliance = AllCand[,5]
alliance[alliance=="inc"] = "rjd"
alliance[alliance=="jdu"] = "rjd"
alliance[alliance=="ljp"] = "bjp"
alliance[alliance=="rlsp"] = "bjp"
alliance[alliance=="ham"] = "bjp"
alliance[alliance=="aimim"] = "others"
alliance[alliance=="sp"] = "others"
alliance[alliance=="jap"] = "others"
AllCandAlliance = cbind(AllCand,alliance)
names(AllCandAlliance)[5] = "OrigParty"
names(AllCandAlliance)[6] = "Party"
CandParams2015_bjp_rjd_others <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2015/CandParams2015Final_bjp_rjd_others.tsv")
CandParams = merge(AllCandAlliance,CandParams2015_bjp_rjd_others,by=c("AcNo","Party","CandidateName"),all.x=TRUE);
# looks good so far.. just compare with 2014 data
PartyParams2015Refined <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2015/PartyParams2015Refined.tsv");
partyParams = cbind(PartyParams2015Refined,PartyParams2015Refined[,2]);
names(partyParams)[6] = "Party"
names(partyParams)[2] = "PartyParamOrig"
partyParams$Party[partyParams$Party=="inc"] = "rjd";
partyParams$Party[partyParams$Party=="jdu"] = "rjd";
partyParams$Party[partyParams$Party=="rlsp"] = "bjp";
partyParams$Party[partyParams$Party=="ham"] = "bjp";
partyParams$Party[partyParams$Party=="ljp"] = "bjp";
partyParams$Party[partyParams$Party=="ncp"] = "others";
partyParams$Party[partyParams$Party=="cpi"] = "others";
partyParams$Party[partyParams$Party=="ind"] = "others";
partyParams$Party[partyParams$Party=="shs"] = "others";
partyParams$Party[partyParams$Party=="cpm"] = "others";
partyParams$Party[partyParams$Party=="bsp"] = "others";
partyParams$Party[partyParams$Party=="jmm"] = "others";
partyParams$Party[partyParams$Party=="cpi(ml)"] = "others";
partyParams$Party[partyParams$Party=="aap"] = "others";
partyParams$Party[partyParams$Party=="sp"] = "others";
names(partyParams)[3] ="Strength";
names(partyParams)[4] ="Unity";
names(partyParams)[5] ="BoothMgmt";
gAcParty = group_by(partyParams,AcNo,Party);
partyParamsRef = summarize(gAcParty,Strength=mean(Strength),Unity=mean(Unity),BoothMgmt=mean(BoothMgmt));
cand_party = merge(CandParams,partyParamsRef,by=c("AcNo","Party"),all.x=TRUE);View(cand_party);
casteSharePerAcPartyParams2015 <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2015/casteSharePerAcPartyParams2015.tsv")
cand_party_caste = merge(cand_party, casteSharePerAcPartyParams2015, by=c("AcNo","Party"),all.x=TRUE);
dev_local <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2015/AcFeatureVector.tsv")
cand_party_caste_dev_local = merge(cand_party_caste,dev_local,by=c("AcNo"),all.x=TRUE);
names(cand_party_caste_dev_local)[7] = "CandParamOrigAcNo"
names(cand_party_caste_dev_local)[6] = "ActualParty"
names(cand_party_caste_dev_local)[8] = "CandParamActualParty"
names(cand_party_caste_dev_local)[11] = "Effectiveness"
names(cand_party_caste_dev_local)[13] = "CasteGroup"
names(cand_party_caste_dev_local)[14] = "MusclePower"
names(cand_party_caste_dev_local)[15] = "FinancialStatus"
names(cand_party_caste_dev_local)[16] = "PartyLeadersSupport"
names(cand_party_caste_dev_local)[17] = "LocalLeadersSupport"
write.table(cand_party_caste_dev_local,file="I:/ArchishaData/ElectionData/Bihar/Predictions2015/cand_party_caste_dev_local_2015.tsv",quote=FALSE,sep="\t",row.names=FALSE)
View(cand_party_caste_dev_local)

