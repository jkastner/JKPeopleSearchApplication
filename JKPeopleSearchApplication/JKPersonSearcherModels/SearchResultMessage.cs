using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace JKPersonSearcherModels
{
    public class SearchResultMessage
    {
        public string MessageInformation { get; set; }
        public PersonInformation[] SearchResults { get; set; }
        public SearchResultType SearchResultTypeDescription { get; set; }
        public string RequestTimeStamp { get; set; }


        public static SearchResultMessage FailResultWithMessage(string message, string requestTimeStamp)
        {
            var ret = new SearchResultMessage()
            {
                SearchResults = new PersonInformation[0],
                MessageInformation = message,
                RequestTimeStamp = requestTimeStamp,
                SearchResultTypeDescription = SearchResultType.NoResults
            };
            return ret;
        }

        public static SearchResultMessage SuccessResultWithMessage(IEnumerable<PersonInformation> foundInfo,
            String searchInput, String requestTimeStamp)
        {
            var infoArray = foundInfo.ToArray();
            var ret = new SearchResultMessage()
            {
                SearchResults = infoArray,
                RequestTimeStamp = requestTimeStamp,
                MessageInformation = $"Search for '{searchInput}' succeeded with {infoArray.Length} results.",
                SearchResultTypeDescription = SearchResultType.SuccessfulSearch
            };
            return ret;
        }


        [Pure]
        public string ToJsonString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
