parseQualData = function(acId, path="I:\\ArchishaData\\ElectionData\\RawData\\CVoter\\QualitativeData\\", outPath = "I:\\ArchishaData\\ElectionData\\Bihar\\CVoterData\\")
{
    print(acId)
    if (!require(XML)) install.packages('XML')
    library(XML)
    infile = paste(path,acId,".html",sep="")
    outPath2010 = paste(outPath,"2010\\QualitativeTables\\",sep="")
    outPath2015 = paste(outPath,"2015\\QualitativeTables\\",sep="")
    tables = readHTMLTable(infile);
    localIssues2010 = tables[[2]];
    localIssues2015 = tables[[3]];
    devParams2010 = tables[[4]];
    devParams2015 = tables[[5]];
    candParams2010 = tables[[6]];
    candParams2015 = tables[[7]];
    finalCandParams2015 = tables[[8]];
    partyParams2010 = tables[[9]];
    partyParams2015 = tables[[10]];
    casteShares2010 = tables[[11]];
    casteShares2015 = tables[[12]];
    write.table(localIssues2010,paste(outPath2010,acId,"_localIssues.txt",sep=""),quote=FALSE,sep="\t",row.names=FALSE)
    write.table(localIssues2015,paste(outPath2015,acId,"_localIssues.txt",sep=""),quote=FALSE,sep="\t",row.names=FALSE)
    write.table(devParams2010,paste(outPath2010,acId,"_devParams.txt",sep=""),quote=FALSE,sep="\t",row.names=FALSE)
    write.table(devParams2015,paste(outPath2015,acId,"_devParams.txt",sep=""),quote=FALSE,sep="\t",row.names=FALSE)
    write.table(candParams2010,paste(outPath2010,acId,"_candParams.txt",sep=""),quote=FALSE,sep="\t",row.names=FALSE)
    write.table(candParams2015,paste(outPath2015,acId,"_candParams.txt",sep=""),quote=FALSE,sep="\t",row.names=FALSE)
    write.table(partyParams2010,paste(outPath2010,acId,"_partyParams.txt",sep=""),quote=FALSE,sep="\t",row.names=FALSE)
    write.table(partyParams2015,paste(outPath2015,acId,"_partyParams.txt",sep=""),quote=FALSE,sep="\t",row.names=FALSE)
    write.table(casteShares2010,paste(outPath2010,acId,"_casteShares.txt",sep=""),quote=FALSE,sep="\t",row.names=FALSE)
    write.table(casteShares2015,paste(outPath2015,acId,"_casteShares.txt",sep=""),quote=FALSE,sep="\t",row.names=FALSE)
}