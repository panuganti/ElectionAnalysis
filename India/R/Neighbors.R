neighbors = function(acShapes, outFile)
{
    require(spdep)
    n = poly2nb(acShapes)
    for (i in 1:length(acShapes))
    {
        x = unlist(n[i])
        line = x[1]
        for (j in 2:length(x))
        {
            line = paste(line,x[j],sep=",")
        }
        print(line)
        write(line,outFile,append=TRUE)
    }
}