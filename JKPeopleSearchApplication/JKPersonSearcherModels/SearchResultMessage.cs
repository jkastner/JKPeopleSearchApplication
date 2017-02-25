using System;
using System.Collections.Generic;
using System.Linq;

namespace JKPersonSearcherModels
{
    public class SearchResultMessage
    {
        public string MessageInformation { get; set; }
        public PersonInformation[] SearchResults { get; set; }
        public SearchResultType SearchResultTypeDescription { get; set; }

        public static SearchResultMessage FailResultWithMessage(string message)
        {
            var ret = new SearchResultMessage()
            {
                SearchResults = new PersonInformation[0],
                MessageInformation = message,
                SearchResultTypeDescription = SearchResultType.NoResults
            };
            return ret;
        }

        public static SearchResultMessage SuccessResultWithMessage(IEnumerable<PersonInformation> foundInfo)
        {
            var infoArray = foundInfo.ToArray();
            var ret = new SearchResultMessage()
            {
                SearchResults = infoArray,
                MessageInformation = $"Search succeeded with {infoArray.Length} results.",
                SearchResultTypeDescription = SearchResultType.SuccessfulSearch
            };
            return ret;
        }


        public string ToJsonString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
