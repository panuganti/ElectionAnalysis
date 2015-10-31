e = extraction2015WithResults
s = sample.split(1:243,SplitRatio=0.7)
train = e[s[e$AcNo],]
test = e[!s[e$AcNo],]
write.table(train,"I:/ArchishaData/ElectionData/Bihar/Predictions2015/TrainSet.txt",quote=FALSE,sep="\t",row.names=FALSE)
write.table(test,"I:/ArchishaData/ElectionData/Bihar/Predictions2015/TestSet.txt",quote=FALSE,sep="\t",row.names=FALSE)