downloadResults = function(ac, url="http://ceobihar.nic.in/Form20/include_oct_2005/", destFolder="./")
{
    
    paddedAC = sprintf("%03d",ac)
    filename = paste(url,"AC",paddedAC,".pdf",sep="")
    destFilename = paste(destFolder,paddedAC,".pdf",sep="")
    message(filename)
    message(destFilename)
    download.file(filename,destFilename,method="curl")
}