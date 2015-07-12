writeToCsv = function(i,dir="C:\\Business\\ElectionAnalysis\\Bihar\\Data\\Results\\2005FebPollingBoothWise\\")
{
    require(xlsx)
    print(i)
    paddedInt = sprintf("%03d",i)
    filename = paste(dir,"AC",paddedInt,".xlsx",sep="")
    data = read.xlsx2(filename,1,as.data.frame = TRUE,colClasses = NA)
    outFile = paste(dir,i,".csv",sep="")
    
    write.table(data,outFile,sep="\t")
}