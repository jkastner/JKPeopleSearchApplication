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


        public SearchResultMessage(string messageInformation, 
            PersonInformation[] searchResults,
            SearchResultType searchResultTypeDescription)
        {
            MessageInformation = messageInformation;
            SearchResults = searchResults;
            SearchResultTypeDescription = searchResultTypeDescription;
        }



        [Pure]
        public string ToJsonString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
