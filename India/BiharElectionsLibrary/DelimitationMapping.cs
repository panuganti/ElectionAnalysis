using System;
using System.Collections.Generic;
using System.Linq;

namespace BiharElectionsLibrary
{
    public class DelimitationMapping
    {
        public Dictionary<Constituency, Constituency> DelimitatioinMapping2010To2005 { get; set; }

        public static DelimitationMapping GenerateDelimitationMapping(List<ACResult> results2010,
            List<ACResult> results2005)
        {

            var delimitationMapping = new Dictionary<Constituency, Constituency>();
            int count = 0;
            for (int i = 1; i <= 243; i++)
            {
                int acNo = i;
                var result2010 = results2010.First(t => t.Constituency.No == acNo);
                var constituencies2005 =
                    results2005.Where(
                        t => Utils.LevenshteinDistance(t.Constituency.Name, result2010.Constituency.Name) < 2).ToArray();
                if (constituencies2005.Count() == 1 || (constituencies2005.Count() > 1 &&
                                                        constituencies2005.Count(
                                                            t =>
                                                                t.Constituency.Name.Equals(result2010.Constituency.Name)) ==
                                                        constituencies2005.Count()))
                {
                    delimitationMapping.Add(result2010.Constituency, constituencies2005.First().Constituency);
                    continue;
                }
                if (constituencies2005.Count() > 1 &&
                    constituencies2005.Min(
                        t => Utils.LevenshteinDistance(t.Constituency.Name, result2010.Constituency.Name) < 2))
                {
                    delimitationMapping.Add(result2010.Constituency,
                        constituencies2005.OrderBy(
                            t => Utils.LevenshteinDistance(t.Constituency.Name, result2010.Constituency.Name))
                            .First().Constituency);
                    continue;
                }
                delimitationMapping.Add(result2010.Constituency, null);
                /*
                Console.WriteLine("Cannot find mapping for {0}, outputs:{1}", result2010.Constituency.Name,
                    String.Join(";", constituencies2005.Select(t => t.Constituency.Name)));
                 */
                count++;
            }
            Console.WriteLine("{0} mappings missing", count);

            return new DelimitationMapping {DelimitatioinMapping2010To2005 = delimitationMapping};
        }
    }
}
