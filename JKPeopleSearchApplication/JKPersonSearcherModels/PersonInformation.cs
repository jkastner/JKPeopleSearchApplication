using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name="First Name")]
        public String FirstName { get; set; }
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        public int PersonInformationId { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Interests { get; set; }
        public virtual PersonImage PersonInformationImage { get; set; }

        public String FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }
}
