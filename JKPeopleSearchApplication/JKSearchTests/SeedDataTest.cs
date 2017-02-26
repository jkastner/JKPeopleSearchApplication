using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //Line number, and the naughty data that broke it.
            List<Tuple<int, PersonInformation>> badData = new List<Tuple<int, PersonInformation>>();
            for (int index = 0; index < createdInfo.Count; index++)
            {
                var curData = createdInfo[index];
                Assert.IsNotNull(curData.FirstName);
                Assert.IsNotNull(curData.LastName);
                if (String.IsNullOrWhiteSpace(curData.FirstName) ||
                    String.IsNullOrWhiteSpace(curData.LastName) ||
                    String.IsNullOrWhiteSpace(curData.Address) ||
                    String.IsNullOrWhiteSpace(curData.Interests) ||
                    curData.PersonInformationImage == null ||
                    curData.PersonInformationImage.Image == null ||
                    curData.PersonInformationImage.Image.Length == 0)
                {
                    badData.Add(new Tuple<int, PersonInformation>(index, curData));
                }
            }
            Assert.IsFalse(badData.Any());
        }
    }
}
