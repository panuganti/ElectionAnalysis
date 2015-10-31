SureWinners <- read.delim("I:/ArchishaData/ElectionData/Bihar/Predictions2015/SureWinners.txt");
StrongCandidates <- read.delim("C:/Users/rapanuga/Desktop/StrongCandidates.txt");
names(StrongCandidates)[2] = "Party";
results2015 = rbind(SureWinners[,c("AcNo","Party")],StrongCandidates);
results = results2015[order(results2015$AcNo),];
results$Party[results$Party=="jdu"] = "rjd";
results$Party[results$Party=="jdu"] = "rjd";
results$Party[results$Party=="ham"] = "bjp";
results$Party[results$Party=="ljp"] = "bjp";
results$Party[results$Party=="inc"] = "rjd";
result = unique(results);
results2015 = cbind(result,"Perfect");
names(results2015)[3] = "Result";
extraction2015WithResults = merge(extraction2015,results2015,by=c("AcNo","Party"),all.x=TRUE);
extraction2015WithResults$Result.x = NULL;
names(extraction2015WithResults)[length(names(extraction2015WithResults))] = "Result";
write.table(extraction2015WithResults,file="I:/ArchishaData/ElectionData/Bihar/Predictions2015/2015ExtractionWResults.tsv",quote=FALSE,sep="\t",row.names=FALSE)
