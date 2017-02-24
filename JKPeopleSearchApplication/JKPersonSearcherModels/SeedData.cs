using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JKPersonSearcherModels
{
    public class SeedData
    {
        private SeedData()
        {

        }

        public static IReadOnlyList<PersonInformation> GetSeedInformation()
        {
            SeedData sd = new SeedData();
            return sd.SeedInformation();
        }

        /// <summary>
        /// Generates a few hundred random names for testing purposes.
        /// I'll probably move this to a unit test class later.
        /// </summary>
        /// <returns></returns>
        private IReadOnlyList<PersonInformation> SeedInformation()
        {
            List<PersonInformation> ret = new List<PersonInformation>();
            var currentAssembly = this.GetType().Assembly;

            using (var stream = currentAssembly.GetManifestResourceStream("JKPersonSearcherModels.SampleData.txt"))
            using (var reader = new StreamReader(stream))
            {
                List<String> allLines = new List<string>();
                String line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    allLines.Add(line);
                }
                //Skip the header line
                allLines.RemoveAt(0);
                foreach (string curLine in allLines.Skip(1))
                {
                    //We can rely on these hardcoded values as the SeedData is static.
                    //The seed data was generated once by a separate process
                    //0    1(unused)        2      3               4  
                    //Name	Notes	       Age	FakeAddresses	Hobbies (or interests)
                    var tabEntries = curLine.Split('\t');
                    PersonInformation newPerson = new PersonInformation()
                    {
                        FirstName = tabEntries[0].Split(null)[0],
                        LastName = tabEntries[0].Split(null)[1],
                        Age = int.Parse(tabEntries[2]),
                        Address = tabEntries[3],
                        Interests = tabEntries[4],
                        PersonInformationImage = GenericImage
                    };
                    ret.Add(newPerson);
                }


            }
            return ret;
        }

        private PersonImage _genericImage = null;

        public PersonImage GenericImage
        {
            get
            {
                if (_genericImage == null)
                {
                    var currentAssembly = this.GetType().Assembly;

                    using (var stream = currentAssembly.GetManifestResourceStream("JKPersonSearcherModels.DefaultProfile.png"))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        _genericImage = new PersonImage()
                        {
                            Image = buffer
                        };
                    };
                }
                return _genericImage;
            }

        }
    }
}
