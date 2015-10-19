combineQualData = function(i,combinedData,path="D:\\ArchishaData\\ElectionData\\Bihar\\CVoterData\\2010\\QualitativeTables\\")
{
    print(i)
    file = paste(path,i,"_casteShares.txt",sep="");
    print(file)
    if (length(readLines(file)) > 1)
    {
    data = read.csv(file,header=TRUE,sep="\t",colClasses="character");
        data = cbind(toString(i),data);
        names(data)[1] = "acId";
        combinedData = rbind(combinedData,data);
    }
    combinedData;
}