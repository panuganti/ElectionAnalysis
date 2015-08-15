using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapeCensusData
{
    class Program
    {
        static void Main(string[] args)
        {
            /* 
             * Get list of districts from
             * http://www.census2011.co.in/census/state/districtlist/bihar.html
             * Parse and get the subdistrict/block link for each district..
             * 
             * for each district
             * {
             *  Fetch html page for each of the subdistrict link
             *  Store list of Blocks in Region Hierarchy,
             *  Get link for each of the blocks
             * 
             *      for each block, 
             *      {
             *          fetch html page ,
             *          Parse and get list of Municipalities, Census Towns and Villages and their links
             *          Store the list in Region Hierarchy
             * 
             *              {
             *                  for each village, fetch html page,
             *                  Parse and populate Village params
             * 
             *                  for each census town, fetch html page,
             *                  Parse and populate Census town params
             * 
             *                  for each municipality, fetch html page,
             *                  Parse and populate municipality params
             *              }
             *      }
             * }
             * 
             */
        }

        public static void ScrapeAndPopulateCensusData()
        {
            
        }
    }
}
