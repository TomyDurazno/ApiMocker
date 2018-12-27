using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class User
    {
        public int userId { get; set; }
        public string username
        {
            get
            {
                return firstName + " " + lastName;
            }

        }
        public string firstName { get; set; }

        public string middleName { get; set; }

        public string lastName { get; set; }
        public string name { get; set; }

        public string suffix { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public int appRoleId { get; set; }

        public string appRole { get; set; }
    }
}