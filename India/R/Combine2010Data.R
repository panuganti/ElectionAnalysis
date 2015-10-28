dev_local <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2010/dev_local.tsv")
cand_result <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2010/Extraction2010.tsv")
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
CandParams <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2010/CandParams2010Final.txt")
names(CandParams)[1] = "AcNo";
#Group CandParams by alliance
cand_result_candParams = merge(cand_result_alliance,CandParams,by=c("AcNo","Party","CandidateName"),all.x=TRUE)
