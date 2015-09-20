using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiharElectionsLibrary.CVoter
{
    class CATIReader
    {
        public static List<CATISurvey> Survey(string filename)
        {
            var survey = new List<CATISurvey>();
            var allLines = File.ReadAllLines(filename);
            foreach(var line in allLines)
            {
                int placeHolder = 0;
                var cols = line.Split('\t').Select(t => t.Trim()).ToArray();
                var response = new CATISurvey()
                {
                    State = cols[0],
                    Dist = cols[0],
                    AcName = cols[0],
                    AcId = cols[0],
                };
                survey.Add(response);
            }
            return survey;
        }
    }
}
