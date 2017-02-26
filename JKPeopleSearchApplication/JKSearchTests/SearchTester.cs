using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKPeopleSearchApplication;
using JKPersonSearcherModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JKSearchTests
{
    [TestClass]
    public class SearchTester
    {
        private List<PersonInformation> _myInfo;

        [TestInitialize]
        public void DataSetup()
        {
            _myInfo = SeedData.GetSeedInformation().ToList();
        }

        [TestMethod]
        public void DoubleCheckSeedData()
        {
            Assert.IsTrue(_myInfo.Any());

        }

        [TestMethod]
        public void SearchEmptyText()
        {
            var results = PersonSearcher.SearchForMatch("", _myInfo);
            Assert.AreEqual(results.SearchResultTypeDescription, SearchResultType.NoResults);
            Assert.IsTrue(results.SearchResults != null);
            Assert.IsTrue(results.SearchResults.Length==0);
        }


        [TestMethod]
        public void SearchSingleLetter()
        {
            var results = PersonSearcher.SearchForMatch("a", _myInfo);
            Assert.AreEqual(results.SearchResultTypeDescription, SearchResultType.SuccessfulSearch);
            Assert.IsTrue(results.SearchResults.Length > 0);
        }


        [TestMethod]
        public void SearchCaseInsensitive()
        {
            var results = PersonSearcher.SearchForMatch("st", _myInfo);
            var resultsUpper = PersonSearcher.SearchForMatch("ST", _myInfo);

            Assert.IsTrue(results.SearchResultTypeDescription==SearchResultType.SuccessfulSearch);
            Assert.AreEqual(results.SearchResultTypeDescription, resultsUpper.SearchResultTypeDescription);
            Assert.AreEqual(results.SearchResults.Length, resultsUpper.SearchResults.Length);

        }

        [TestMethod]
        public void SearchNonsense()
        {
            var garbageSearch = Guid.NewGuid() + "" + Guid.NewGuid();
            var res = PersonSearcher.SearchForMatch(garbageSearch, _myInfo);
            Assert.IsTrue(res.SearchResultTypeDescription==SearchResultType.NoResults);
            Assert.IsTrue(res.SearchResults.Length==0);
        }

        [TestMethod]
        public void SearchFirstNameMatch()
        {
            var newSample = GenerateSamplePerson();
            _myInfo.Add(newSample);
            var res = PersonSearcher.SearchForMatch("Unlikely", _myInfo);
            Assert.IsTrue(res.SearchResultTypeDescription==SearchResultType.SuccessfulSearch);
            Assert.IsTrue(res.SearchResults.Length == 1);
            Assert.AreEqual(res.SearchResults.First(), newSample);
        }

        const string DummyFirstName = "UnlikelyFirstName";
        const string DummyLastName = "MrImprobable";
        private PersonInformation GenerateSamplePerson()
        {
            return new PersonInformation()
            {
                FirstName = DummyFirstName,
                LastName = DummyLastName,
                PersonInformationImage = SeedData.GetGenericImage(),
                PersonInformationId = -1,
                Address = "123 Fake Street, Springfield FakeState, 555",
                Age = 9001,
                Interests = "Being made up, questioning reality"

            };
        }


        [TestMethod]
        public void SearchExactMatch()
        {
            int duplicateEntries = 125;
            for(int index = 0;index<duplicateEntries;index++)
            {
                _myInfo.Add(GenerateSamplePerson());
            }
            var result = PersonSearcher.SearchForMatch(DummyFirstName + " " + DummyLastName, _myInfo);
            Assert.AreEqual(duplicateEntries, result.SearchResults.Length);
            Assert.AreEqual(result.SearchResultTypeDescription, SearchResultType.PerfectMatch);
        }

    }
}
