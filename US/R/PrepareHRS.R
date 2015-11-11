tweets <- read.delim("D:/ArchishaData/ElectionData/US/FormattedTweets.tsv", header=FALSE)
names(tweets)[1] = "TweetId"
names(tweets)[2] = "Tweet"
judgements <- read.delim("D:/ArchishaData/ElectionData/US/JudgementsParsed.tsv", header=FALSE)
names(judgements)[1] = "TweetId"
names(judgements)[2] = "Judgement"
hrs = merge(tweets,judgements,by="TweetId",all.x=TRUE);
hrs = merge(tweets,judgements,by="TweetId",all.x=TRUE);
hrs_train = merge(tweets,judgements,by="TweetId");
hrs_train_unique = unique(hrs_train)
