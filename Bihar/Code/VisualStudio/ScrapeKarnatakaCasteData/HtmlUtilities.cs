using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;


namespace ScrapeKarnatakaCasteData
{
    public class HtmlUtilities
    {
        #region new stuff

        public static int retry = 0;

        public static string Scrape(string uri)
        {               
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string str = reader.ReadToEnd();
                reader.Close();
                retry = 0;
                return str;
            }
            catch(Exception e)
            {
                if (retry > 3) { return null; }
                retry++;
                return Scrape(uri);
            }
        }

        public static List<string> GetDistricts(string state)
        {
            var uri = String.Format("http://164.100.133.130/SES/DistrictController_search?state_code={0}", state);
            var response = Scrape(uri);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            return dict.Keys.Skip(1).ToList();
        }

        public static List<string> GetTehsilList(string state, string dist)
        {
            var uri = String.Format("http://164.100.133.130/SES/TehsilController_search?state_code={0}&district_code={1}", state, dist);
            var response = Scrape(uri);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            return dict.Keys.Skip(1).ToList();
        }

        public static List<string> GetTownList(string state, string dist, string tehsil)
        {
            var uri = String.Format("http://164.100.133.130/SES/VTCController_search?state_code={0}&district_code={1}&tehsil_code={2}", 
                state, dist, tehsil);
            var response = Scrape(uri);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            return dict.Keys.Skip(1).ToList();
        }

        public static List<string> GetWardList(string state, string dist, string tehsil, string town)
        {
            var uri = String.Format("http://164.100.133.130/SES/WardController_search?state_code={0}&district_code={1}&tehsil_code={2}&vtc_code={3}",
                state, dist, tehsil, town);
            var response = Scrape(uri);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            return dict.Keys.Skip(1).ToList();
        }

        public static List<string> GetBlockList(string state, string dist, string tehsil, string town, string ward)
        {
            var uri = String.Format("http://164.100.133.130/SES/BlockController_search?state_code={0}&district_code={1}&tehsil_code={2}&vtc_code={3}&ward_id={4}",
                state, dist, tehsil, town, ward);
            var response = Scrape(uri);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            return dict.Keys.Skip(1).ToList();
        }

        public static Dictionary<string, string> GetHouseholds(string state, string dist, string tehsil, string town, 
            string ward, string block)
        {
            var uri = String.Format(
                "http://164.100.133.130/SES/?state_selection={0}&district_selection={1}&tehsil_selection={2}&vtc_selection={3}&ward_selection={4}&block_selection={5}",
                state, dist, tehsil, town, ward, block);
            var response = Scrape(uri);
            var dict = new Dictionary<string, string>();
            if (response == null) { return dict; }
            var pattern = "javascript:showData\\(\"([\\^a-zA-Z0-9_\\-\\.xml]+)";
            var regex = new Regex(pattern);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);
            var tableDiv = htmlDoc.GetElementbyId("household");
            var anchors = tableDiv.Descendants("a");
            
            foreach(var anchor in anchors)
            {
                if (!anchor.Attributes.Contains("href")) {continue;}
                var house = anchor.InnerText;
                var linkHref = anchor.Attributes["href"].Value;
                var link = regex.Match(linkHref).Groups[1].Value;
                dict.Add(house,link);
            }
            return dict;
        }

        public static string GetHouseInfo(string link)
        {
            var uri = String.Format("http://164.100.133.130/SES/jsp/houseHold.jsp?url={0}",link.Replace("^","%255E"));
            var response = Scrape(uri);
            return response;
        }

        #endregion new stuff

    }
}
