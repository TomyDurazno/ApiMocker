using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class User
    {
        public int id { get; set; }
        public string firstName { get; set; }

        public string middleName { get; set; }

        public string lastName { get; set; }

        public string suffix { get; set; }

        public string emailAddress { get; set; }

        public string password { get; set; }

        public int roleId { get; set; }

        public string role { get; set; }
    }
}