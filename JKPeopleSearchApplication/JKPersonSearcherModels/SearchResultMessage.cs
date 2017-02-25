using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKPersonSearcherModels
{
    public class SearchResultMessage
    {
        public string MessageInformation { get; set; }
        public bool SearchSucceeded { get; set; }
        public int ResultCount { get; set; }
        public PersonInformation[] SearchResults { get; set; }

        public static String JsonFailResultWithMessage(String message)
        {
            var ret = new SearchResultMessage()
            {
                SearchResults = new PersonInformation[0],
                SearchSucceeded = false,
                ResultCount = 0,
                MessageInformation = message
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(ret);
        }

        public static String JsonSuccessResultWithMessage(IEnumerable<PersonInformation> foundInfo)
        {
            var infoArray = foundInfo.ToArray();
            var ret = new SearchResultMessage()
            {
                SearchResults = infoArray,
                SearchSucceeded = true,
                ResultCount = infoArray.Length,
                MessageInformation = $"Search succeeded with {infoArray.Length} results."
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(ret);
        }
    }
}
