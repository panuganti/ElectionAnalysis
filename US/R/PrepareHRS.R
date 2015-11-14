tweets <- read.delim("I:/ArchishaData/ElectionData/US/FormattedTweets.tsv", header=FALSE)
names(tweets) = c("TweetId","Tweet","NoOfImages","AnyHashtags","AnyLinks","NoOfMentions","Hashtags","HashtagWithDemCands","HashtagWithRepCands","TweetWithDemCands","TweetWithRepCands")
judgements <- read.delim("I:/ArchishaData/ElectionData/US/JudgementsParsed.tsv", header=FALSE)
names(judgements) = c("TweetId","Judge","Judgement");
hrs = merge(tweets,judgements,by="TweetId",all.x=TRUE);
hrs = merge(tweets,judgements,by="TweetId",all.x=TRUE);
hrs_train = merge(tweets,judgements,by="TweetId");
hrs_train_unique = unique(hrs_train)
hrs_train_unique[hrs_train_unique$Judgement=="AntiDemocratic",] = "ProRepublican"
write.table(hrs_train_unique[,c(13,2,3,4,5,6,7,8,9,10,11)],"I:/ArchishaData/ElectionData/US/TrainingSet.tsv",quote=TRUE,sep=",",row.names=FALSE,col.names=FALSE)
write.table(hrs_train_unique[,c(12,1,13,2,3,4,5,6,7,8,9,10,11)],"I:/ArchishaData/ElectionData/US/JudgedTrainingSet.tsv",quote=TRUE,sep="\t",row.names=FALSE,col.names=TRUE)

write.table(tset,"I:/ArchishaData/ElectionData/US/tSet.tsv",quote=TRUE,sep=",",row.names=FALSE,col.names=FALSE)
