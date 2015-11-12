tweets <- read.delim("I:/ArchishaData/ElectionData/US/FormattedTweets.tsv", header=FALSE)
names(tweets)[1] = "TweetId"
names(tweets)[2] = "Tweet"
judgements <- read.delim("I:/ArchishaData/ElectionData/US/JudgementsParsed.tsv", header=FALSE)
names(judgements)[1] = "TweetId"
names(judgements)[2] = "Judge"
names(judgements)[3] = "Judgement"
hrs = merge(tweets,judgements,by="TweetId",all.x=TRUE);
hrs = merge(tweets,judgements,by="TweetId",all.x=TRUE);
hrs_train = merge(tweets,judgements,by="TweetId");
hrs_train_unique = unique(hrs_train)
write.table(hrs_train_unique[,c(3,4,2,1)],"I:/ArchishaData/ElectionData/US/TrainingSet.tsv",quote=TRUE,sep="\t",row.names=FALSE,col.names=FALSE)