parse = function(pcNo, electionId, stateId, inDir="J:\\ArchishaData\\ElectionData\\RawData\\IndiaVotes", outDir="")
{
    htmlFile = paste(inDir,"\\",electionId, stateId, pcNo,".html",sep="");
    tsvFile = paste(outDir,"\\",pcNo,".tsv",sep="");
    table = readHTMLTable(htmlFile);
    write.table(table,tsvFile,col.names=FALSE,row.names=FALSE,sep="\t",quote=FALSE)
}