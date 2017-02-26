using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using JKPersonSearcherModels;

namespace JKPeopleSearchApplication
{
    public static class PersonSearcher
    {

        [Pure]
        public static SearchResultMessage SearchForMatch(String searchInput,
            IEnumerable<PersonInformation> searchParam, String requestTimeStamp)
        {
            var fixedInput = searchInput.Trim().ToLower();
            if (String.IsNullOrWhiteSpace(fixedInput))
            {
                return SearchResultMessage.FailResultWithMessage("", requestTimeStamp);
            }
            var searchTargets = searchParam.ToList();

            //If there are any perfect matches, we should just return those.
            var perfectMatches = searchTargets.Where(
                x => (x.FullName).ToLower().Equals(fixedInput)).ToArray();
            if (perfectMatches.Any())
            {
                //It's possible that there could be so many perfect matches that the system could flood.
                //But for this sample project we're ignoring that unlikely scenario.
                //The user would have to have some way of narrowing which John Smith they're 
                //looking for anyway.
                var ret = SearchResultMessage.SuccessResultWithMessage(perfectMatches, searchInput,
                    requestTimeStamp);
                ret.SearchResultTypeDescription = SearchResultType.PerfectMatch;
                return ret;
            }
            var matchingPeople = searchTargets.Where(
                x =>
                    x.FirstName.ToLower().Contains(fixedInput) ||
                    x.LastName.ToLower().Contains(fixedInput)
                ).ToArray();

            if (matchingPeople.Any())
            {
                return SearchResultMessage.SuccessResultWithMessage(matchingPeople, searchInput, requestTimeStamp);
            }
            else
            {
                return SearchResultMessage.FailResultWithMessage($"Search for {searchInput} yielded no results.",
                    requestTimeStamp);
            }
        }

    }
}