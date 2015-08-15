downloadResults = function(ac, url="http://ceobihar.nic.in/Form20/include_feb_2005/", destFolder="E:/NMW/GitHub/ElectionAnalysis/Bihar/Data/Results/2005FebPollingBoothWise/")
{
    
    paddedAC = sprintf("%03d",ac)
    filename = paste(url,"AC",paddedAC,".pdf",sep="")
    destFilename = paste(destFolder,"AC",paddedAC,".pdf",sep="")
    
    download.file(filename,destFilename,method="curl")
}