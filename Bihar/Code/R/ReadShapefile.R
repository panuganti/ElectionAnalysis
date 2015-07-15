## Read Shapefile

acShapes = readOGR("D:\\ArchishaData\\ElectionData\\CommonData\\ShapeFiles\\bihar\\bihar.assembly.shp", layer = "bihar.assembly")
neighbors = poly2nb(acShapes)