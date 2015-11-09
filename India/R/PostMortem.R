BiharPredictions = read.delim("C:/Users/rajkiran/Downloads/BiharPredictions.tsv", strip.white=TRUE, stringsAsFactors = FALSE)
results = read.delim("C:/Users/rajkiran/Downloads/BiharResults.tsv", strip.white=TRUE, stringsAsFactors = FALSE)
names(results)[1] = "AcNo";
predictions_results = merge(BiharPredictions,results,by=c("AcNo"))
predictions_results$District = NULL
predictions_results$X = NULL
predictions_results$X.1 = NULL
names(predictions_results)[5] = "PredictedWinnerParty"
names(predictions_results)[10] = "WinnerParty"
names(predictions_results)[11] = "WinnerVotes"
names(predictions_results)[13] = "RunnerUpParty"
names(predictions_results)[14] = "RunnerUpVotes"
pr = predictions_results
pr = cbind(pr,pr$WinnerParty);
names(pr)[16] = "WinnerAlliance"
pr$WinnerAlliance = as.character(pr$WinnerAlliance)
pr$WinnerAlliance[pr$WinnerAlliance=="IND"] = "others"
pr$WinnerAlliance[pr$WinnerAlliance=="JDU"] = "upa"
pr$WinnerAlliance[pr$WinnerAlliance=="BJP"] = "nda"
pr$WinnerAlliance[pr$WinnerAlliance=="INC"] = "upa"
pr$WinnerAlliance[pr$WinnerAlliance=="JD(U)"] = "upa"
pr$WinnerAlliance[pr$WinnerAlliance=="RJD"] = "upa"
pr$WinnerAlliance[pr$WinnerAlliance=="LJP"] = "nda"
pr$WinnerAlliance[pr$WinnerAlliance=="RLSP"] = "nda"
pr$WinnerAlliance[pr$WinnerAlliance=="CPI(ML)"] = "others"
pr$WinnerAlliance[pr$WinnerAlliance=="Hindustani Awam Morcha (Secular)"] = "nda"
pr$PredictionCorrectness = ifelse(pr$WinnerAlliance==pr$Prediction,"Correct","Wrong")
