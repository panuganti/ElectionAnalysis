using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BiharElectionsLibrary
{
    public class ShapeFileData
    {
        public Dictionary<int, string> ShapeFileAcIds { get; set; }

        public static ShapeFileData LoadFromFile(string filename)
        {
            var allLines = File.ReadAllLines(filename).Skip(1).ToArray();

            var shapeFiles =
                allLines.Select(line => line.Split('\t'))
                    .Select(
                        cols =>
                            new ShapeFileDataRow
                            {
                                ACName = cols[4],
                                AcNo = Int32.Parse(cols[3]),
                            })
                    .ToList();

            return new ShapeFileData
            {
                ShapeFileAcIds = shapeFiles.ToDictionary(t => t.AcNo, t => t.ACName),
            };
        }
    }

    public class ShapeFileDataRow
    {
        public int AcNo { get; set; }
        public string ACName { get; set; }        
    }
}
