combineQualData = function(i,combinedData,fileExt,path="I:\\ArchishaData\\ElectionData\\Bihar\\CVoterData\\2015\\QualitativeTables\\")
{
    file = paste(path,i,fileExt,sep="");
    if (length(readLines(file)) > 1)
    {
        data = read.csv(file,header=TRUE,sep="\t",colClasses="character");
        data = cbind(toString(i),data);
        names(data)[1] = "acId";
        combinedData = rbind(combinedData,data);
    }
    combinedData;
}