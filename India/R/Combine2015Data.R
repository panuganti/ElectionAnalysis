AllCand <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2015/AllCandidates.txt",strip.white=TRUE,stringsAsFactors=FALSE)
AllCandAlliance = cbind(AllCand,AllCand[,5],stringsAsFactors=FALSE)
names(AllCandAlliance)[6] = "Party";
names(AllCandAlliance)[5] = "OrigParty";
AllCandAlliance$Party[AllCandAlliance$Party=="inc"] = "rjd"
AllCandAlliance$Party[AllCandAlliance$Party=="jdu"] = "rjd"
AllCandAlliance$Party[AllCandAlliance$Party=="ljp"] = "bjp"
AllCandAlliance$Party[AllCandAlliance$Party=="rlsp"] = "bjp"
AllCandAlliance$Party[AllCandAlliance$Party=="ham"] = "bjp"
AllCandAlliance$Party[AllCandAlliance$Party=="aimim"] = "others"
AllCandAlliance$Party[AllCandAlliance$Party=="sp"] = "others"
AllCandAlliance$Party[AllCandAlliance$Party=="jap"] = "others"
CandParams2015_bjp_rjd_others <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2015/CandParams2015Final_bjp_rjd_others.tsv",strip.white=TRUE)
CandParams = merge(AllCandAlliance,CandParams2015_bjp_rjd_others,by=c("AcNo","Party","CandidateName"),all.x=TRUE);
# looks good so far.. just compare with 2014 data
PartyParams2015Refined <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2015/PartyParams2015Refined.tsv",strip.white=TRUE);
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
require(dplyr);
gAcParty = group_by(partyParams,AcNo,Party);
partyParamsRef = summarize(gAcParty,Strength=mean(Strength),Unity=mean(Unity),BoothMgmt=mean(BoothMgmt));
cand_party = merge(CandParams,partyParamsRef,by=c("AcNo","Party"),all.x=TRUE);
casteSharePerAcPartyParams2015 <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2015/casteSharePerAcPartyParams2015.tsv",strip.white=TRUE);
cand_party_caste = merge(cand_party, casteSharePerAcPartyParams2015, by=c("AcNo","Party"),all.x=TRUE);
dev_local <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2015/AcFeatureVector.tsv",strip.white=TRUE);
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
extraction2015 = cand_party_caste_dev_local[,order(names(cand_party_caste_dev_local))]
extraction2015$AcName = NULL
extraction2015$CandParamActualParty = NULL
extraction2015$CandParamOrigAcNo = NULL
extraction2015$CasteGroup = NULL
extraction2015$Phase = NULL
extraction2015[is.na(extraction2015)] = 0;
extraction2015 = extraction2015[,order(names(extraction2015))]
extraction2015 = cbind(extraction2015,"Result");
extraction2015 = extraction2015[,order(names(extraction2015))];
extraction2015[,1] = NA;
names(extraction2015)[1] = "Result";
extraction2015 = extraction2015[,order(names(extraction2015))];
write.table(extraction2015,file="I:/ArchishaData/ElectionData/Bihar/Predictions2015/2015Extraction.tsv",quote=FALSE,sep="\t",row.names=FALSE)
#View(extraction2015)
