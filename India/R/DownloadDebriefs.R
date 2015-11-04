downloadDebriefs = function(ac, url="http://dashboard.cvoterindia.com/biharqualitative/debriefing/", destFolder="C:/Users/rapanuga/Desktop/QualitativeDebriefs/")
{
    print(ac)
    filename = paste(url,ac,".pdf",sep="")
    destFilename = paste(destFolder,ac,".pdf",sep="")
    download.file(filename,destFilename,method="curl")
}