using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using JKPersonSearcherModels;

namespace JKPeopleSearchApplication.Models
{
    public class PersonInfoContext : DbContext
    {
        public DbSet<PersonInformation> AllPersonInfo { get; set; }
    }
}