downloadDebriefs = function(ac, url="http://dashboard.cvoterindia.com/biharqualitative/debriefing/", destFolder="J:/ArchishaData/ElectionData/RawData/CVoter/QualitativeDebriefs/")
{
    filename = paste(url,ac,".pdf",sep="")
    destFilename = paste(destFolder,ac,".pdf",sep="")
    if (!file.exists(destFilename))
    {
        download.file(filename,destFilename,method="curl")
    }
}