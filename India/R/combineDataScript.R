#Script to combine Qualitative Data
combinedData = c();for (n in 1:243) {combinedData = combineQualData(n,combinedData,"_partyParams.txt")}; combinedData[combinedData == "Â"] = NA; View(combinedData)
write.table(combinedData,"I:\\ArchishaData\\ElectionData\\Bihar\\CVoterData\\2015\\Qualitative\\CombinedQualitativeData\\PartyParams.txt",row.names=FALSE,quote=FALSE,sep="\t",na="0")
