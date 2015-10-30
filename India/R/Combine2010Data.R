cand_result <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2010/Extraction2010.tsv",strip.white=TRUE)
cand_result_pegf = cand_result[cand_result$Result != "Bad",];
cand_result_peg = cand_result_pegf[cand_result_pegf$Result != "Fair",];
cand_result_pe = cand_result_peg[cand_result_peg$Result != "Good",];
cand_result_alliance = cbind(cand_result_pe,cand_result_pe[,3])
names(cand_result_alliance)[5] = "Party"
names(cand_result_alliance)[3] = "ActualParty"
cand_result_alliance$Party[cand_result_alliance$Party=="jdu"] = "bjp";
cand_result_alliance$Party[cand_result_alliance$Party=="ljp"] = "rjd";
cand_result_alliance$Party[cand_result_alliance$Party=="inc"] = "others";
cand_result_alliance$Party[cand_result_alliance$Party=="ind"] = "others";
cand_result_alliance$Party[cand_result_alliance$Party=="bsp"] = "others";
cand_result_alliance$Party[cand_result_alliance$Party=="cpi"] = "others";
cand_result_alliance$Party[cand_result_alliance$Party=="ncp"] = "others";
#TODO: Check if any Ac has both win/loss as others
CandParams <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2010/CandParams2010Final.txt",strip.white=TRUE)
names(CandParams)[1] = "AcNo";
#Group CandParams by alliance
cand_result_candParams = merge(cand_result_alliance,CandParams,by=c("AcNo","Party","CandidateName"),all.x=TRUE)
caste <- read.delim("I:/ArchishaData/ElectionData/Bihar/CVoterData/2010/Qualitative/CombinedQualitativeData/CasteShares_bjp_rjd_inc_others.tsv",strip.white=TRUE);
result_cand_caste = merge(cand_result_candParams,caste,by=c("AcNo","Party"),all.x=TRUE)
#PartyParams
PartyParams2010Refined <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2010/PartyParamsRefined.tsv",strip.white=TRUE);
partyParams = cbind(PartyParams2010Refined,PartyParams2010Refined[,2]);
names(partyParams)[6] = "Party"
names(partyParams)[2] = "PartyParamOrig"
partyParams$Party[partyParams$Party=="inc"] = "others";
partyParams$Party[partyParams$Party=="jdu"] = "bjp";
partyParams$Party[partyParams$Party=="ljp"] = "rjd";
partyParams$Party[partyParams$Party=="ncp"] = "others";
partyParams$Party[partyParams$Party=="cpi"] = "others";
partyParams$Party[partyParams$Party=="ind"] = "others";
partyParams$Party[partyParams$Party=="shs"] = "others";
partyParams$Party[partyParams$Party=="cpm"] = "others";
partyParams$Party[partyParams$Party=="bsp"] = "others";
partyParams$Party[partyParams$Party=="jmm"] = "others";
partyParams$Party[partyParams$Party=="cpi(ml)"] = "others";
partyParams$Party[partyParams$Party=="sp"] = "others";
names(partyParams)[3] ="Strength";
names(partyParams)[4] ="Unity";
names(partyParams)[5] ="BoothMgmt";
require(dplyr);
gAcParty = group_by(partyParams,AcNo,Party);
partyParamsRef = summarize(gAcParty,Strength=mean(Strength),Unity=mean(Unity),BoothMgmt=mean(BoothMgmt));
result_cand_caste_party = merge(result_cand_caste,partyParamsRef,by=c("AcNo","Party"),all.x=TRUE);
dev_local <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2010/dev_local.tsv",strip.white=TRUE);
result_cand_caste_party_dev_local = merge(result_cand_caste_party,dev_local,by=c("AcNo"),all.x=TRUE);
extraction2010 = result_cand_caste_party_dev_local[,order(names(result_cand_caste_party_dev_local))];
names(extraction2010)[46] = "LocalLeadersSupport";
names(extraction2010)[51] = "PartyLeadersSupport";
extraction2010$ReligiousInfluence = NULL;
extraction2010[is.na(extraction2010)] = 0;
write.table(extraction2010,file="I:/ArchishaData/ElectionData/Bihar/Predictions2010/2010Extraction.tsv",quote=FALSE,sep="\t",row.names=FALSE)
#View(extraction2010)