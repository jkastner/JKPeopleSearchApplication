using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        public static IReadOnlyList<PersonInformation> GetSeedInformation(bool randomizeImages)
        {
            SeedData sd = new SeedData();
            return sd.SeedInformation(randomizeImages);
        }

        public static PersonImage GetGenericImage()
        {
            SeedData sd = new SeedData();
            return sd.GenericImage;
        }

        //Used to randomize profile pictures
        static Random _random = new Random();


        /// <summary>
        /// Generates a few hundred random names for testing purposes.
        /// </summary>
        private IReadOnlyList<PersonInformation> SeedInformation(bool randomizeImages)
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

                    var selectedImage = GenericImage;
                    if (randomizeImages && _random.NextDouble() > .5)
                    {
                        //We use the smaller image so the recoloring won't take too long.
                        selectedImage = ProduceAlteredImage();
                    }
                    PersonInformation newPerson = new PersonInformation()
                    {
                        FirstName = tabEntries[0].Split(null)[0],
                        LastName = tabEntries[0].Split(null)[1],
                        Age = int.Parse(tabEntries[2]),
                        Address = tabEntries[3],
                        Interests = tabEntries[4],
                        PersonInformationImage = selectedImage
                    };
                    ret.Add(newPerson);
                }


            }
            return ret;
        }

        /// <summary>
        /// This is used to randomize the background color of the profile image,
        ///  to showcase some variety.
        /// </summary>
        private PersonImage ProduceAlteredImage()
        {
            Bitmap profileImage;
            using (var ms = new MemoryStream(SmallImage.Image))
            {
                profileImage = new Bitmap(ms);
            }
            Color randomColor = Color.FromArgb(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
            for (int x = 0; x < profileImage.Width; x++)
            {
                for (int y = 0; y < profileImage.Height; y++)
                {
                    Color gotColor = profileImage.GetPixel(x, y);
                    if (!AlmostBlack(gotColor))
                    {
                        profileImage.SetPixel(x, y, randomColor);
                    }
                }
            }
            Byte[] bytes;
            using (MemoryStream stream = new MemoryStream())
            {
                profileImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                bytes = stream.ToArray();
            }
            return new PersonImage()
            {
                Image = bytes,
            };

        }

        private bool AlmostBlack(Color gotColor)
        {
            return gotColor.R < 2 && gotColor.B < 2 && gotColor.G < 2;
        }

        private static PersonImage _smallImage = null;

        private PersonImage SmallImage
        {
            get
            {
                if (_smallImage == null)
                {
                    var currentAssembly = this.GetType().Assembly;

                    using (var stream = currentAssembly.GetManifestResourceStream("JKPersonSearcherModels.SmallProfile.png"))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        _smallImage = new PersonImage()
                        {
                            Image = buffer
                        };
                    };
                }
                return _smallImage;
            }
        }


        private static PersonImage _genericImage = null;

        private PersonImage GenericImage
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
