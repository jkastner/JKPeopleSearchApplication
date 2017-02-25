using System;
using System.Linq;
using JKPersonSearcherModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JKSearchTests
{
    [TestClass]
    public class SeedDataTest
    {
        [TestMethod]
        public void SeedDataGeneraionTest()
        {
            var createdInfo = SeedData.GetSeedInformation().OfType<PersonInformation>().ToList();
            Assert.IsTrue(createdInfo.Any());
            //I doubt I'd have less than 10 items in my seed data, even if I mix it up.
            Assert.IsTrue(createdInfo.Count>=10);
        }
    }
}
