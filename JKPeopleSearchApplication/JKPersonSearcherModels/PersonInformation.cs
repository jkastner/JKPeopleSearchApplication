using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKPersonSearcherModels
{
    /// <summary>
    /// A simple representation of the data that comprises a searchable person.
    /// This object is in a separate library from the web logic to allow for reuse elsewhere.
    /// </summary>
    public class PersonInformation
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }

        public int PersonInformationId { get; set; }

    }
}
